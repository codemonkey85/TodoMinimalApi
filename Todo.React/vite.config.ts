import { defineConfig, loadEnv } from 'vite'
import react from '@vitejs/plugin-react-swc'
import basicSsl from '@vitejs/plugin-basic-ssl'
import { fileURLToPath } from "url";

const baseFolder =
    process.env.APPDATA !== undefined && process.env.APPDATA !== ''
        ? `${process.env.APPDATA}/ASP.NET/https`
    : `${process.env.HOME}/.aspnet/https`;
        
// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '');

  return {
    define:
      { __API_URL__: JSON.stringify(env.VITE_API_URL) },
    plugins: [react(), basicSsl(
      {
        name: 'todo-react',
        certDir: baseFolder
      }
    )],
    resolve: {
      alias: {
          '@': fileURLToPath(new URL('./src', import.meta.url)),
      }
    },
    server: {
      proxy: {
          '^/api': {
              target: "https://localhost:7128/api",
              secure: true
          }
      },
      port: 5173,
    }
  }
})
