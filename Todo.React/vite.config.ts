import { defineConfig, loadEnv } from 'vite'
import react from '@vitejs/plugin-react-swc'
import { fileURLToPath } from "url";
        
// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '');

  return {
    define:
      { __API_URL__: JSON.stringify(env.VITE_API_URL) },
    plugins: [react()],
    resolve: {
      alias: {
          '@': fileURLToPath(new URL('./src', import.meta.url)),
      }
    },
    server: {
      proxy: {
          '^/api': {
              target: env.VITE_API_URL,
              secure: mode === 'development' ? false : true,
              changeOrigin: true,
              rewrite: (path) => path.replace(/^\/api/, '')
          }
      },
      port: 5173,
    }
  }
})
