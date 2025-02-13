import { defineConfig, loadEnv } from 'vite'
import { fileURLToPath } from "url";
import tsconfigPaths from "vite-tsconfig-paths";
import { reactRouter } from "@react-router/dev/vite";
        
// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '');

  return {
    css: {
      preprocessorOptions: {
        scss: {
          api: 'modern-compiler', // or "modern"
          silenceDeprecations: ['mixed-decls', 'color-functions', 'global-builtin', 'import']
        }
      }
    },
    define:
      { __API_URL__: JSON.stringify(env.VITE_API_URL) },
    plugins: [reactRouter(), tsconfigPaths()],
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url)),
        '~bootstrap': fileURLToPath(new URL('./node_modules/bootstrap', import.meta.url))
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
