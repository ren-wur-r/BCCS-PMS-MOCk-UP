import { defineConfig, loadEnv } from "vite"; 
import vue from "@vitejs/plugin-vue";
import { fileURLToPath } from "url";
import { copyFileSync } from "fs";
import { resolve } from "path";

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), "");
  return {
    base: env.VITE_BASE_PATH,
    server: {
      proxy: {
        '/api': {
          target: 'http://192.168.3.236:8100/ProjectManagerWebApi',
          changeOrigin: true,
          rewrite: (path) => path.replace(/^\/api/, '/api'),
        },
        'api': {
          target: 'http://192.168.3.236:8100/ProjectManagerWebApi',
          changeOrigin: true,
        },
      },
    },
    plugins: [
      vue(),
      // 自定義插件來複製對應的 web.config
      {
        name: "copy-webconfig",
        writeBundle() {
          const webConfigMap = {
            development: "web.config.dev",
            staging: "web.config.staging",
            production: "web.config.prod",
          };

          const sourceFile = webConfigMap[mode] || "web.config";
          const sourcePath = resolve(__dirname, "public", sourceFile);
          const targetPath = resolve(__dirname, "dist", "web.config");

          try {
            copyFileSync(sourcePath, targetPath);
            console.log(`Copied ${sourceFile} to web.config`);
          } catch (error) {
            console.error(`Failed to copy web.config: ${error}`);
          }
        },
      },
    ],
    resolve: {
      alias: {
        "@": fileURLToPath(new URL("./src", import.meta.url)),
      },
    },
  };
});
