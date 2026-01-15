import { useRouter } from "vue-router";
import { useTokenStore } from "@/stores/token";

/**
 * token驗證相關的共用邏輯
 *
 * 用途：
 * - 在需要登入狀態的操作前進行驗證
 * - 若未登入，自動導向登入頁
 */
export function useAuth() {
  const tokenStore = useTokenStore();
  const router = useRouter();
  const useMockData = import.meta.env.VITE_USE_MOCK_DATA === "true";

  /**
   * 檢查是否存在登入 Token
   * @returns 是否通過驗證
   *
   * 使用情境：
   * - API 呼叫前
   * - 需要登入權限的操作前
   */
  const requireToken = (): boolean => {
    if (useMockData) {
      if (!tokenStore.token) {
        tokenStore.setToken("mock-token");
      }
      return true;
    }
    if (!tokenStore.token) {
      router.push("/login");
      return false;
    }
    return true;
  };

  return {
    requireToken,
  };
}
