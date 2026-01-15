import { defineStore } from "pinia";
import { ref } from "vue";
import type { MbsBscHttpGetAllRegionRspItemMdl } from "@/services/pms-http/basic/basicHttpFormat";

export const useManagerRegionListStore = defineStore("managerRegionList", () => {
  const managerRegionList = ref<MbsBscHttpGetAllRegionRspItemMdl[]>([]);

  /** 設定管理者地區列表 */
  const setManagerRegionList = (data: MbsBscHttpGetAllRegionRspItemMdl[]) => {
    managerRegionList.value = data;
    saveToLocalStorage();
  };

  /** 取得管理者地區列表 */
  const getManagerRegionList = (): MbsBscHttpGetAllRegionRspItemMdl[] => {
    return managerRegionList.value;
  };

  /** 根據ID取得管理者地區名稱 */
  const getManagerRegionNameById = (id: number): string => {
    const region = managerRegionList.value.find(r => r.managerRegionID === id);
    return region?.managerRegionName || "";
  };

  /** 根據名稱取得管理者地區ID */
  const getManagerRegionIdByName = (name: string): number | null => {
    const region = managerRegionList.value.find(r => r.managerRegionName === name);
    return region?.managerRegionID || null;
  };

  /** 檢查地區ID是否存在 */
  const isValidRegionId = (id: number): boolean => {
    return managerRegionList.value.some(r => r.managerRegionID === id);
  };

  /** 儲存至 localStorage */
  const saveToLocalStorage = () => {
    localStorage.setItem(
      "managerRegionList",
      JSON.stringify(managerRegionList.value)
    );
  };

  /** 從 localStorage 讀取資料 */
  const loadFromLocalStorage = () => {
    const stored = localStorage.getItem("managerRegionList");
    if (stored) {
      try {
        const parsed = JSON.parse(stored);
        managerRegionList.value = Array.isArray(parsed) ? parsed : [];
      } catch {
        managerRegionList.value = [];
      }
    }
  };

  /** 清除管理者地區資料 */
  const removeManagerRegionList = () => {
    managerRegionList.value = [];
    localStorage.removeItem("managerRegionList");
  };

  /** 檢查是否有資料 */
  const hasData = (): boolean => {
    return managerRegionList.value.length > 0;
  };

  // 初始化時讀取資料
  loadFromLocalStorage();

  return {
    managerRegionList,
    setManagerRegionList,
    getManagerRegionList,
    getManagerRegionNameById,
    getManagerRegionIdByName,
    isValidRegionId,
    removeManagerRegionList,
    hasData,
  };
});