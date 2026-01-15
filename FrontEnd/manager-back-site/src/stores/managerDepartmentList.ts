import { defineStore } from "pinia";
import { ref } from "vue";
import type { MbsBscHttpGetAllDepartmentRspItemMdl } from "@/services/pms-http/basic/basicHttpFormat";

export const useManagerDepartmentListStore = defineStore("managerDepartmentList", () => {
  const managerDepartmentList = ref<MbsBscHttpGetAllDepartmentRspItemMdl[]>([]);

  /** 設定管理者部門列表 */
  const setManagerDepartmentList = (data: MbsBscHttpGetAllDepartmentRspItemMdl[]) => {
    managerDepartmentList.value = data;
    saveToLocalStorage();
  };

  /** 取得管理者部門列表 */
  const getManagerDepartmentList = (): MbsBscHttpGetAllDepartmentRspItemMdl[] => {
    return managerDepartmentList.value;
  };

  /** 根據ID取得管理者部門名稱 */
  const getManagerDepartmentNameById = (id: number): string => {
    const department = managerDepartmentList.value.find(d => d.managerDepartmentID === id);
    return department?.managerDepartmentName || "";
  };

  /** 根據名稱取得管理者部門ID */
  const getManagerDepartmentIdByName = (name: string): number | null => {
    const department = managerDepartmentList.value.find(d => d.managerDepartmentName === name);
    return department?.managerDepartmentID || null;
  };

  /** 檢查部門ID是否存在 */
  const isValidDepartmentId = (id: number): boolean => {
    return managerDepartmentList.value.some(d => d.managerDepartmentID === id);
  };

  /** 儲存至 localStorage */
  const saveToLocalStorage = () => {
    localStorage.setItem(
      "managerDepartmentList",
      JSON.stringify(managerDepartmentList.value)
    );
  };

  /** 從 localStorage 讀取資料 */
  const loadFromLocalStorage = () => {
    const stored = localStorage.getItem("managerDepartmentList");
    if (stored) {
      try {
        const parsed = JSON.parse(stored);
        managerDepartmentList.value = Array.isArray(parsed) ? parsed : [];
      } catch {
        managerDepartmentList.value = [];
      }
    }
  };

  /** 清除管理者部門資料 */
  const removeManagerDepartmentList = () => {
    managerDepartmentList.value = [];
    localStorage.removeItem("managerDepartmentList");
  };

  /** 檢查是否有資料 */
  const hasData = (): boolean => {
    return managerDepartmentList.value.length > 0;
  };

  // 初始化時讀取資料
  loadFromLocalStorage();

  return {
    managerDepartmentList,
    setManagerDepartmentList,
    getManagerDepartmentList,
    getManagerDepartmentNameById,
    getManagerDepartmentIdByName,
    isValidDepartmentId,
    removeManagerDepartmentList,
    hasData,
  };
});