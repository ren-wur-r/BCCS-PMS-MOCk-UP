// 檔案名稱：useRouteParamId.ts
import { useRoute } from "vue-router";

/**
 * 從路由參數中取得並驗證指定參數名稱的 ID
 * @param paramName 預設為 "id"
 * @returns 驗證後的 ID（有效時為 number，否則為 null）
 */
export const useRouteParamId = (paramName: string = "id"): number | null => {
  const route = useRoute();
  const id = Number(route.params[paramName]);
  return !id || isNaN(id) ? null : id;
};
