<script setup lang="ts">
import { reactive, watch } from "vue";
import { useManagerRegionListStore } from "@/stores/managerRegionList";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomPermissionKindEnum } from "@/constants/DbAtomPermissionKindEnum";
import { DbManagerRegionConst } from "@/constants/DbManagerRegionConst";
import { permissionTableConfig } from "@/components/feature/permission/permissionModel";
import type { PermissionItemMdl } from "@/components/feature/permission/permissionModel";
import { getPermissionKindLabel } from "@/utils/getPermissionKindLabel";

const managerRegionListStore = useManagerRegionListStore();
// ----------------------------------------------------------------------------
/** 權限列表 */
const permissionList = defineModel<PermissionItemMdl[]>("permissionList", {
  required: true,
});
/** 管理地區 ID */
const managerRegionID = defineModel<number | null>("managerRegionID", {
  required: true,
});

const props = defineProps<{
  readonly: boolean;
}>();

// ----------------------------------------------------------------------------
/**
 * 各群組的展開/收合狀態
 * - key: 群組的唯一識別碼 (groupKey)
 * - value: true=展開, false=收合
 * - 預設值: 全部展開
 */
const groupExpandedState = reactive<Record<string, boolean>>(
  Object.fromEntries(permissionTableConfig.map((g) => [g.groupKey, true]))
);

/**
 * 切換指定群組的展開/收合狀態
 */
const toggleGroup = (groupKey: string) => {
  groupExpandedState[groupKey] = !groupExpandedState[groupKey];
};

// ----------------------------------------------------------------------------
/** 取得資料設定 */
const getPermissionByMenu = (targetMenu: DbAtomMenuEnum): PermissionItemMdl => {
  // 先從 permissionTableConfig 中找到對應的選單配置
  let menuConfig = null;
  for (const group of permissionTableConfig) {
    const found = group.menuItemList.find((item) => item.menuEnum === targetMenu);
    if (found) {
      menuConfig = found;
      break;
    }
  }

  const existingPermission = permissionList.value.find((p) => p.atomMenu === targetMenu);

  // 如果有現有權限資料，且配置存在
  if (existingPermission && menuConfig) {
    // 如果配置中的權限選項只有一個，強制使用配置中的值（用於專案管理、工項管理等固定權限）
    return {
      atomMenu: targetMenu,
      managerRegionID: existingPermission.managerRegionID,
      atomPermissionKindIdView:
        menuConfig.permissionView.length === 1
          ? menuConfig.permissionView[0]
          : existingPermission.atomPermissionKindIdView,
      atomPermissionKindIdCreate:
        menuConfig.permissionCreate.length === 1
          ? menuConfig.permissionCreate[0]
          : existingPermission.atomPermissionKindIdCreate,
      atomPermissionKindIdEdit:
        menuConfig.permissionEdit.length === 1
          ? menuConfig.permissionEdit[0]
          : existingPermission.atomPermissionKindIdEdit,
      atomPermissionKindIdDelete:
        menuConfig.permissionDelete.length === 1
          ? menuConfig.permissionDelete[0]
          : existingPermission.atomPermissionKindIdDelete,
    };
  }

  // 如果只有現有權限但沒有配置，直接返回現有權限
  if (existingPermission) {
    return existingPermission;
  }

  // 如果找到配置但沒有現有權限，使用配置中的第一個權限選項作為預設值
  if (menuConfig) {
    return {
      atomMenu: targetMenu,
      managerRegionID: DbManagerRegionConst.Denied.ID,
      atomPermissionKindIdView: menuConfig.permissionView[0] || DbAtomPermissionKindEnum.Denied,
      atomPermissionKindIdCreate: menuConfig.permissionCreate[0] || DbAtomPermissionKindEnum.Denied,
      atomPermissionKindIdEdit: menuConfig.permissionEdit[0] || DbAtomPermissionKindEnum.Denied,
      atomPermissionKindIdDelete: menuConfig.permissionDelete[0] || DbAtomPermissionKindEnum.Denied,
    };
  }

  // 如果都找不到，返回全部無權限的預設值
  return {
    atomMenu: targetMenu,
    managerRegionID: DbManagerRegionConst.Denied.ID,
    atomPermissionKindIdView: DbAtomPermissionKindEnum.Denied,
    atomPermissionKindIdCreate: DbAtomPermissionKindEnum.Denied,
    atomPermissionKindIdEdit: DbAtomPermissionKindEnum.Denied,
    atomPermissionKindIdDelete: DbAtomPermissionKindEnum.Denied,
  };
};

// ----------------------------------------------------------------------------
/** 取得地區顯示文字 */
const getRegionDisplayText = (regionID: number | null): string => {
  if (regionID === DbManagerRegionConst.Denied.ID) {
    return "無權限";
  }
  // 直接從 store 中查找地區名稱
  const regionName = managerRegionListStore.getManagerRegionNameById(regionID || 0);
  if (regionName) {
    return regionName;
  }

  return "無對應地區";
};

// ----------------------------------------------------------------------------
/** 定義權限等級順序（從高到低） */
const getPermissionLevel = (permission: DbAtomPermissionKindEnum): number => {
  switch (permission) {
    case DbAtomPermissionKindEnum.Everyone:
      return 3; // 最高權限
    case DbAtomPermissionKindEnum.Self:
      return 2; // 中等權限
    case DbAtomPermissionKindEnum.Denied:
      return 1; // 最低權限
    default:
      return 0;
  }
};

// ----------------------------------------------------------------------------
/** 更新權限 */
const updatePermission = (
  targetMenu: DbAtomMenuEnum,
  permissionType: "view" | "create" | "edit" | "delete",
  newValue: DbAtomPermissionKindEnum
) => {
  // 如果是只讀模式，不執行更新
  if (props.readonly) return;

  // 複製權限列表以避免直接修改 props
  const updatedPermissionList = [...permissionList.value];

  // 找到目標權限項目的索引
  const index = updatedPermissionList.findIndex((p) => p.atomMenu === targetMenu);

  // 如果找不到目標權限項目，則返回
  if (index === -1) return;

  // 建立新物件
  const targetPermission = { ...updatedPermissionList[index] };

  // 更新對應欄位
  switch (permissionType) {
    case "view":
      targetPermission.atomPermissionKindIdView = newValue;
      break;
    case "create":
      targetPermission.atomPermissionKindIdCreate = newValue;
      break;
    case "edit":
      targetPermission.atomPermissionKindIdEdit = newValue;
      break;
    case "delete":
      targetPermission.atomPermissionKindIdDelete = newValue;
      break;
  }

  // 若是非 view 權限，且新權限比 view 權限更高，則同步提升 view 權限
  if (permissionType !== "view") {
    const newLevel = getPermissionLevel(newValue);
    const viewLevel = getPermissionLevel(targetPermission.atomPermissionKindIdView);

    if (newLevel > viewLevel) {
      targetPermission.atomPermissionKindIdView = newValue;
    }
  }

  // 將更新後的物件放回列表
  updatedPermissionList[index] = targetPermission;
  permissionList.value = updatedPermissionList;
};

// ----------------------------------------------------------------------------
/** 更新地區 */
const updateRegion = (targetMenu: DbAtomMenuEnum, newValue: number) => {
  // 如果是只讀模式，不執行更新
  if (props.readonly) return;

  // 複製權限列表以避免直接修改 props
  const updatedPermissionList = [...permissionList.value];

  // 找到目標權限項目的索引
  const index = updatedPermissionList.findIndex((p) => p.atomMenu === targetMenu);

  // 如果找不到目標權限項目，則返回
  if (index === -1) return;

  // 建立新物件並更新
  updatedPermissionList[index] = {
    ...updatedPermissionList[index],
    managerRegionID: newValue ?? DbManagerRegionConst.Denied.ID,
  };

  permissionList.value = updatedPermissionList;
};
// ----------------------------------------------------------------------------
/** 取得特定權限項目的地區選項 */
const getRegionOptionsForPermission = () => {
  // 基本選項：無權限
  const options: { id: number; name: string }[] = [
    { id: DbManagerRegionConst.Denied.ID, name: "無權限" },
  ];

  // 加入當前選中的管理地區（如果存在且不是無權限）
  if (managerRegionID.value !== null && managerRegionID.value !== DbManagerRegionConst.Denied.ID) {
    const regionName = managerRegionListStore.getManagerRegionNameById(managerRegionID.value);
    if (regionName) {
      options.push({
        id: managerRegionID.value,
        name: regionName,
      });
    }
  }

  return options;
};

// 監聽 managerRegionID 變化，自動將原本選擇舊地區的權限改為無權限
watch(
  () => managerRegionID.value,
  (newRegionId, oldRegionId) => {
    // 如果是只讀模式，不執行更新
    if (props.readonly) return;

    // 如果舊地區ID存在且與新地區ID不同，且權限列表不為空
    if (oldRegionId !== null && newRegionId !== oldRegionId && permissionList.value.length > 0) {
      const updatedPermissionList = permissionList.value.map((permission) => {
        // 如果該權限項目的地區ID不是無權限(不等於 Denied.ID)，則改為無權限
        if (permission.managerRegionID !== DbManagerRegionConst.Denied.ID) {
          return {
            ...permission,
            managerRegionID: DbManagerRegionConst.Denied.ID,
          };
        }
        return permission;
      });
      permissionList.value = updatedPermissionList;
    }
  }
);
</script>

<template>
  <div class="overflow-x-auto">
    <table class="min-w-full border border-gray-300 text-sm">
      <!-- 表頭 -->
      <thead class="bg-brand-200 text-white border-b border-gray-300">
        <tr>
          <th class="w-1/4 px-4 py-3 text-start text-sm font-normal border-r border-gray-300">
            目錄
          </th>
          <th class="px-4 py-3 text-center text-sm font-normal border-r border-gray-300">地區</th>
          <th class="px-4 py-3 text-center text-sm font-normal border-r border-gray-300">檢視</th>
          <th class="px-4 py-3 text-center text-sm font-normal border-r border-gray-300">新增</th>
          <th class="px-4 py-3 text-center text-sm font-normal border-r border-gray-300">編輯</th>
          <th class="px-4 py-3 text-center text-sm font-normal">刪除</th>
        </tr>
      </thead>

      <!-- 每個群組作為一個 tbody -->
      <tbody v-for="group in permissionTableConfig" :key="group.groupKey" class="bg-white">
        <!-- 群組標題列 -->
        <tr
          class="cursor-pointer bg-gray-100 hover:bg-gray-200 border-b border-gray-300 py-2"
          @click="toggleGroup(group.groupKey)"
        >
          <td colspan="6" class="px-4 py-2">
            <div class="flex items-center">
              <svg
                class="w-4 h-4 mr-2 transition-transform"
                :class="{ 'rotate-90': groupExpandedState[group.groupKey] }"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M9 5l7 7-7 7"
                />
              </svg>
              <span class="text-sm font-medium text-gray-800">
                {{ group.groupTitle }}
              </span>
            </div>
          </td>
        </tr>

        <!-- 群組內容行 -->
        <template v-if="groupExpandedState[group.groupKey]">
          <tr
            v-for="item in group.menuItemList"
            :key="item.menuEnum"
            class="border-b border-gray-300"
          >
            <!-- 功能名稱 -->
            <td class="w-1/4 px-4 py-2 text-sm text-gray-900 border-r border-gray-300">
              {{ item.menuName }}
            </td>

            <!-- 地區 -->
            <td class="w-1/6 px-4 py-2 text-center border-r border-gray-300">
              <!-- 地區唯讀模式：顯示文字 -->
              <span v-if="props.readonly || !item.hasRegion">
                <template v-if="item.hasRegion">
                  {{ getRegionDisplayText(getPermissionByMenu(item.menuEnum).managerRegionID) }}
                </template>
                <template v-else>
                  <span class="text-gray-400">-</span>
                </template>
              </span>
              <!-- 地區編輯模式：顯示下拉選單 -->
              <select
                v-else
                class="select-box"
                :value="getPermissionByMenu(item.menuEnum).managerRegionID"
                @change="
                  updateRegion(item.menuEnum, Number(($event.target as HTMLSelectElement).value))
                "
              >
                <!-- 只顯示該權限項目的地區選項 -->
                <option
                  v-for="option in getRegionOptionsForPermission()"
                  :key="option.id"
                  :value="option.id"
                >
                  {{ option.name }}
                </option>
              </select>
            </td>

            <!-- 檢視權限 -->
            <td class="w-auto px-4 py-2 text-center border-r border-gray-300">
              <!-- 權限類型預設只有無權限 -->
              <span
                v-if="
                  item.permissionView.length === 1 &&
                  item.permissionView[0] === DbAtomPermissionKindEnum.Denied
                "
                class="text-gray-400"
              >
                -
              </span>
              <!-- 是否為 readonly 模式或只有單一選項 -->
              <span v-else-if="props.readonly || item.permissionView.length === 1">
                {{
                  getPermissionKindLabel(
                    getPermissionByMenu(item.menuEnum).atomPermissionKindIdView
                  )
                }}
              </span>
              <!-- 顯示下拉選單 -->
              <select
                v-else
                class="select-box"
                :value="getPermissionByMenu(item.menuEnum).atomPermissionKindIdView"
                @change="
                  updatePermission(
                    item.menuEnum,
                    'view',
                    Number(($event.target as HTMLSelectElement).value) as DbAtomPermissionKindEnum
                  )
                "
              >
                <option v-for="kind in item.permissionView" :key="kind" :value="kind">
                  {{ getPermissionKindLabel(kind) }}
                </option>
              </select>
            </td>

            <!-- 新增權限 -->
            <td class="px-4 py-2 text-center border-r border-gray-300">
              <!-- 權限類型預設只有無權限 -->
              <span
                v-if="
                  item.permissionCreate.length === 1 &&
                  item.permissionCreate[0] === DbAtomPermissionKindEnum.Denied
                "
                class="text-gray-400"
              >
                -
              </span>
              <!-- 是否為 readonly 模式或只有單一選項 -->
              <span v-else-if="props.readonly || item.permissionCreate.length === 1">
                {{
                  getPermissionKindLabel(
                    getPermissionByMenu(item.menuEnum).atomPermissionKindIdCreate
                  )
                }}
              </span>
              <!-- 顯示下拉選單 -->
              <select
                v-else
                class="select-box"
                :value="getPermissionByMenu(item.menuEnum).atomPermissionKindIdCreate"
                @change="
                  updatePermission(
                    item.menuEnum,
                    'create',
                    Number(($event.target as HTMLSelectElement).value) as DbAtomPermissionKindEnum
                  )
                "
              >
                <option v-for="kind in item.permissionCreate" :key="kind" :value="kind">
                  {{ getPermissionKindLabel(kind) }}
                </option>
              </select>
            </td>

            <!-- 編輯權限 -->
            <td class="w-auto px-4 py-2 text-center border-r border-gray-300">
              <!-- 權限類型預設只有無權限 -->
              <span
                v-if="
                  item.permissionEdit.length === 1 &&
                  item.permissionEdit[0] === DbAtomPermissionKindEnum.Denied
                "
                class="text-gray-400"
              >
                -
              </span>
              <!-- 是否為 readonly 模式或只有單一選項 -->
              <span v-else-if="props.readonly || item.permissionEdit.length === 1">
                {{
                  getPermissionKindLabel(
                    getPermissionByMenu(item.menuEnum).atomPermissionKindIdEdit
                  )
                }}
              </span>
              <!-- 顯示下拉選單 -->
              <select
                v-else
                class="select-box"
                :value="getPermissionByMenu(item.menuEnum).atomPermissionKindIdEdit"
                @change="
                  updatePermission(
                    item.menuEnum,
                    'edit',
                    Number(($event.target as HTMLSelectElement).value) as DbAtomPermissionKindEnum
                  )
                "
              >
                <option v-for="kind in item.permissionEdit" :key="kind" :value="kind">
                  {{ getPermissionKindLabel(kind) }}
                </option>
              </select>
            </td>

            <!-- 刪除權限 -->
            <td class="px-4 py-2 text-center">
              <!-- 權限類型預設只有無權限 -->
              <span
                v-if="
                  item.permissionDelete.length === 1 &&
                  item.permissionDelete[0] === DbAtomPermissionKindEnum.Denied
                "
                class="text-gray-400"
              >
                -
              </span>
              <!-- 是否為 readonly 式或只有單一選項 -->
              <span v-else-if="props.readonly || item.permissionDelete.length === 1">
                {{
                  getPermissionKindLabel(
                    getPermissionByMenu(item.menuEnum).atomPermissionKindIdDelete
                  )
                }}
              </span>
              <!-- 顯示下拉選單 -->
              <select
                v-else
                class="select-box"
                :value="getPermissionByMenu(item.menuEnum).atomPermissionKindIdDelete"
                @change="
                  updatePermission(
                    item.menuEnum,
                    'delete',
                    Number(($event.target as HTMLSelectElement).value) as DbAtomPermissionKindEnum
                  )
                "
              >
                <option v-for="kind in item.permissionDelete" :key="kind" :value="kind">
                  {{ getPermissionKindLabel(kind) }}
                </option>
              </select>
            </td>
          </tr>
        </template>
      </tbody>
    </table>
  </div>
</template>
