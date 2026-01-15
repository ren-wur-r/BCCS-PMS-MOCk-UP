// eslint.config.js
import js from "@eslint/js";
import tseslint from "typescript-eslint";
import pluginVue from "eslint-plugin-vue";
import prettierConfig from "eslint-config-prettier";

export default [
  {
    ignores: ["dist/**", "node_modules/**"],
  },

  // JavaScript 推薦配置
  js.configs.recommended,

  // TypeScript 推薦配置
  ...tseslint.configs.recommended,

  // Vue 推薦配置
  ...pluginVue.configs["flat/recommended"],

  // TypeScript 文件配置
  {
    files: ["**/*.ts", "**/*.tsx"],
    languageOptions: {
      parser: tseslint.parser,
      parserOptions: {
        ecmaVersion: "latest",
        sourceType: "module",
      },
    },
  },

  // Vue 文件配置
  {
    files: ["**/*.vue"],
    languageOptions: {
      parserOptions: {
        parser: tseslint.parser,
        ecmaVersion: "latest",
        sourceType: "module",
        extraFileExtensions: [".vue"],
      },
    },
    rules: {
      "@typescript-eslint/no-unused-vars": [
        "error",
        {
          argsIgnorePattern: "^_",
          varsIgnorePattern: "^_",
        },
      ],
      "vue/multi-word-component-names": "off",
      // SVG 和複雜組件友善的設定
      "vue/first-attribute-linebreak": [
        "error",
        {
          singleline: "beside", // 簡單的單行標籤第一個屬性在旁邊
          multiline: "below", // 多行時第一個屬性要換行
        },
      ],
      "vue/max-attributes-per-line": [
        "error",
        {
          singleline: 5, // 單行可以放更多屬性（適合簡單標籤）
          multiline: 1, // 多行時每行一個
        },
      ],
      "vue/html-closing-bracket-newline": [
        "error",
        {
          singleline: "never",
          multiline: "always",
        },
      ],
    },
  },

  // 全局規則調整
  {
    rules: {
      "no-undef": "off", // TypeScript 會處理這個
      "@typescript-eslint/no-explicit-any": "warn", // 檢查是否使用 any 型別
      "vue/html-self-closing": "off", // 不強制自閉合標籤
      "vue/attribute-hyphenation": "off", // 不強制屬性使用連字號命名
      "vue/no-v-html": "off", // 允許使用 v-html 指令
      "vue/first-attribute-linebreak": "off", // 關閉第一個屬性換行檢查
    },
  },

  // Prettier 配置 (要放在最後)
  prettierConfig,
];
