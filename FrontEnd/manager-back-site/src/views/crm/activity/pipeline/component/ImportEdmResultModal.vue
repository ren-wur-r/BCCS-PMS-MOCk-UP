<script setup lang="ts">

interface ImportEdmResultModalProps {
  show: boolean;
  resultList: ImportEdmItemMdl[];
  totalCount: number;
}

/** 匯入-EDM-項目-模型 */
interface ImportEdmItemMdl {
  edmContacterEmail: string;
  edmFirstName: string;
  edmLastName: string;
  edmContacterTelephone: string;
  eEdmContacterPhone: string;
  edmCompanyName: string;
  edmContacterJobTitle: string;
  edmCompanyPhone: string;
  edmCompanyFax: string;
  edmCompanyAddress: string;
  edmRemark: string;
  edmContacterDepartment: string;
  edmCompanyMainKind: string;
  edmCompanySubKind: string;
  edmAccountSales: string;
  edmRegion: string;
  edmCreatedDate: string;
  edmDevice: string;
}
const props = defineProps<ImportEdmResultModalProps>();
const emit = defineEmits(["close"]);
</script>

<template>
  <!-- 匯入失敗彈窗 -->
  <div v-if="props.show" class="fixed inset-0 bg-black/40 flex items-center justify-center z-50">
    <div class="bg-white rounded-lg shadow-lg w-[90vw] max-w-[1000px] max-h-[100vh] flex flex-col">
      <!-- 標題 -->
      <div class="flex justify-between items-center p-4 shrink-0">
        <h2 class="text-lg font-bold text-brand-200">匯入 EDM 失敗</h2>
        <p class="text-gray-700 text-[12px]">總共{{ props.totalCount }} 筆</p>
      </div>

      <div class="p-4 flex flex-col flex-1 overflow-hidden">
        <p class="error-tip">失敗原因 : 無公司全名、無名稱 、無電子郵件、隱私權政策為No</p>
      </div>

      <!-- 內容-->
      <div class="flex-1 rounded-md bg-white p-2">
        <!-- 滾動區域 -->
        <div class="overflow-x-auto overflow-y-auto max-h-[55vh]">
          <table class="min-w-[1200px] border-collapse text-xs">
            <thead class="bg-gray-100 sticky top-0">
              <tr>
                <th class="border px-4 py-3">電子信箱</th>
                <th class="border px-4 py-3">First Name</th>
                <th class="border px-4 py-3">Last Name</th>
                <th class="border px-4 py-3">電話</th>
                <th class="border px-4 py-3">行動電話</th>
                <th class="border px-4 py-3">公司名稱</th>
                <th class="border px-4 py-3">職稱</th>
                <th class="border px-4 py-3">公司電話</th>
                <th class="border px-4 py-3">公司傳真</th>
                <th class="border px-4 py-3">公司地址</th>
                <th class="border px-4 py-3">備注</th>
                <th class="border px-4 py-3">部門</th>
                <th class="border px-4 py-3">主類別碼</th>
                <th class="border px-4 py-3">分類碼</th>
                <th class="border px-4 py-3">Account Sales</th>
                <th class="border px-4 py-3">區域</th>
                <th class="border px-4 py-3">Created Date</th>
                <th class="border px-4 py-3">device</th>
              </tr>
            </thead>

            <tbody>
              <tr v-for="(item, index) in props.resultList" :key="index">
                <td class="border px-4 py-3">{{ item.edmContacterEmail }}</td>
                <td class="border px-4 py-3">{{ item.edmFirstName }}</td>
                <td class="border px-4 py-3">{{ item.edmLastName }}</td>
                <td class="border px-4 py-3">{{ item.edmContacterTelephone }}</td>
                <td class="border px-4 py-3">{{ item.eEdmContacterPhone }}</td>
                <td class="border px-4 py-3">{{ item.edmCompanyName }}</td>
                <td class="border px-4 py-3">{{ item.edmContacterJobTitle }}</td>
                <td class="border px-4 py-3">{{ item.edmCompanyPhone }}</td>
                <td class="border px-4 py-3">{{ item.edmCompanyFax }}</td>
                <td class="border px-4 py-3">{{ item.edmCompanyAddress }}</td>
                <td class="border px-4 py-3">{{ item.edmRemark }}</td>
                <td class="border px-4 py-3">{{ item.edmContacterDepartment }}</td>
                <td class="border px-4 py-3">{{ item.edmCompanyMainKind }}</td>
                <td class="border px-4 py-3">{{ item.edmCompanySubKind }}</td>
                <td class="border px-4 py-3">{{ item.edmAccountSales }}</td>
                <td class="border px-4 py-3">{{ item.edmRegion }}</td>
                <td class="border px-4 py-3">{{ item.edmCreatedDate }}</td>
                <td class="border px-4 py-3">{{ item.edmDevice }}</td>
              </tr>
            </tbody>
          </table>
        </div>

              <div class="flex justify-center mt-4">
          <button
            class="btn bg-brand-200 text-white hover:bg-brand-300 min-w-[80px]"
            @click="emit('close')"
          >
            確定
          </button>
        </div>
      </div>
       </div>
    </div>
</template>
<style scoped>
th,
td {
  white-space: nowrap;
  min-width: 100px;
  min-height: 30px;
}
</style>
