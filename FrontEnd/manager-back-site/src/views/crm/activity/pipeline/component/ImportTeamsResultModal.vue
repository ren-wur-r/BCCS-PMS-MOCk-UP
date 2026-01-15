<script setup lang="ts">

interface ImportTeamsResultModalProps {
  show: boolean;
  resultList: ImportTeamsItemMdl[];
  totalCount: number;
}

/** 匯入-Teams-項目-模型 */
interface ImportTeamsItemMdl {
  teamsName: string | null;
  teamsFirstJoinTime: string | null;
  teamsLastLeaveTime: string | null;
  teamsMeetingDuration: string;
  teamsEmail: string;
  teamsParticipantId: string;
  teamsRole: string;
  teamsContacterRegisterLastName: string;
  teamsContacterRegisterFirstName: string;
  teamsContacterRegisterEmail: string;
  teamsRegistrationTime: string | null;
  teamsRegistrationStatus: string;
  teamsCompanyName: string;
  teamsContacterDepartment: string;
  teamsContacterJobTitle: string;
  teamsCompanyTelephone: string;
  teamsContacterPhone: string;
  teamsActivityInfoSource: string;
  teamsContacterIsConsent: boolean;
}
const props = defineProps<ImportTeamsResultModalProps>();
const emit = defineEmits(["close"]);
</script>

<template>
  <!-- 匯入失敗彈窗 -->
  <div v-if="props.show" class="fixed inset-0 bg-black/40 flex items-center justify-center z-50">
    <div
      class="bg-white rounded-lg shadow-lg w-[90vw] max-w-[1000px] max-h-[90vh] flex flex-col overflow-hidden"
    >
      <!-- 標題 -->
      <div class="flex justify-between items-center p-4 shrink-0">
        <h2 class="text-lg font-bold text-brand-200">匯入 Teams 失敗</h2>
        <p class="text-gray-700 text-[12px]">總共{{ props.totalCount }} 筆</p>
      </div>

      <div class="p-4 flex flex-col flex-1 overflow-hidden">
        <p class="error-tip">失敗原因 : 無公司全名、無名稱 、無電子郵件、隱私權政策為No</p>
      </div>

      <!-- 內容-->
      <div class="flex-1 rounded-md bg-white p-2">
        <!-- 滾動區域 -->
        <div class="overflow-x-auto overflow-y-auto max-h-[55vh]">
          <table class="w-full text-xs border-collapse">
            <thead class="bg-gray-100 sticky top-0">
              <tr>
                <th class="border px-4 py-3">名稱</th>
                <th class="border px-4 py-3">首次加入</th>
                <th class="border px-4 py-3">上次離開</th>
                <th class="border px-4 py-3">會議持續時間</th>
                <th class="border px-4 py-3">電子郵件</th>
                <th class="border px-4 py-3">參與者識別碼(UPN)</th>
                <th class="border px-4 py-3">角色</th>
                <th class="border px-4 py-3">註冊名字</th>
                <th class="border px-4 py-3">註冊姓氏</th>
                <th class="border px-4 py-3">註冊電子郵件</th>
                <th class="border px-4 py-3">註冊時間</th>
                <th class="border px-4 py-3">註冊狀態</th>
                <th class="border px-4 py-3">公司全名</th>
                <th class="border px-4 py-3">部門</th>
                <th class="border px-4 py-3">職稱</th>
                <th class="border px-4 py-3">公司電話</th>
                <th class="border px-4 py-3">手機號碼</th>
                <th class="border px-4 py-3">如何得知</th>
                <th class="border px-4 py-3">隱私權政策</th>
              </tr>
            </thead>

            <tbody>
              <tr v-for="(item, index) in props.resultList" :key="index">
                <td class="border px-4 py-3">{{ item.teamsName }}</td>
                <td class="border px-4 py-3">{{ item.teamsFirstJoinTime }}</td>
                <td class="border px-4 py-3">{{ item.teamsLastLeaveTime }}</td>
                <td class="border px-4 py-3">{{ item.teamsMeetingDuration }}</td>
                <td class="border px-4 py-3">{{ item.teamsEmail }}</td>
                <td class="border px-4 py-3">{{ item.teamsParticipantId }}</td>
                <td class="border px-4 py-3">{{ item.teamsRole }}</td>
                <td class="border px-4 py-3">{{ item.teamsContacterRegisterLastName }}</td>
                <td class="border px-4 py-3">{{ item.teamsContacterRegisterFirstName }}</td>
                <td class="border px-4 py-3">{{ item.teamsContacterRegisterEmail }}</td>
                <td class="border px-4 py-3">{{ item.teamsRegistrationTime }}</td>
                <td class="border px-4 py-3">{{ item.teamsRegistrationStatus }}</td>
                <td class="border px-4 py-3">{{ item.teamsCompanyName }}</td>
                <td class="border px-4 py-3">{{ item.teamsContacterDepartment }}</td>
                <td class="border px-4 py-3">{{ item.teamsContacterJobTitle }}</td>
                <td class="border px-4 py-3">{{ item.teamsCompanyTelephone }}</td>
                <td class="border px-4 py-3">{{ item.teamsContacterPhone }}</td>
                <td class="border px-4 py-3">{{ item.teamsActivityInfoSource }}</td>
                <td class="border px-4 py-3">{{ item.teamsContacterIsConsent }}</td>
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
