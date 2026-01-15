import { ref } from "vue";
import { defineStore } from "pinia";

export const useTokenStore = defineStore("token", () => {
  const token = ref<string | null>(null);

  /** 儲存 Token */
  const setToken = (newToken: string) => {
    token.value = newToken;
    localStorage.setItem("token", JSON.stringify({ token: newToken }));
  };

  /** 清除 Token */
  const removeToken = () => {
    token.value = null;
    localStorage.removeItem("token");
  };

  /** 更新 Pinia 的 token */
  const getToken = () => {
    const storedToken = localStorage.getItem("token");
    token.value = storedToken ? JSON.parse(storedToken).token : null;
  };

  // 初始化時讀取 token
  getToken();

  return { token, setToken, removeToken, getToken };
});