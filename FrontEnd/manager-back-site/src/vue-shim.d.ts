// 因為 TypeScript 預設無法理解 .vue 檔案
// 程式中如果有 import 到 vue 檔案，需加上.vue，讓 TypeScript 知道 .vue 是一個 Vue 元件

// declare module '*.vue'告訴TypeScript 編譯器，所有以 .vue 結尾的檔案都應該被視為 Vue 的單一檔案元件
declare module "*.vue" {
    import { DefineComponent } from "vue";
    const component: DefineComponent<{}, {}, any>;
    export default component;
  }
  