<script setup lang="ts">
import { ref, onMounted, onUnmounted, watch } from "vue";
import type { MbsBscHttpHeartbeatReqMdl } from "@/services/pms-http/basic/basicHttpFormat";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { heartbeat } from "@/services/pms-http/basic/basicHttpService";
import { useTokenStore } from "@/stores/token";
import { useRouter } from "vue-router";

const router = useRouter();
const tokenStore = useTokenStore();
// -----------------------------------------------------------------------
// 心跳計時器的ID
let heartbeatInterval: ReturnType<typeof setInterval> | null = null;
// 心跳是否正在運行
const isHeartbeatRunning = ref(false);

/** 登出清除 Token & 導回登入頁面 */
const logout = () => {
  stopHeartbeat();
  tokenStore.removeToken();
  router.push("/login");
};

/** 發送心跳 */
const sendHeartbeat = async () => {
  // 每次發送心跳時重新取得最新的 token
  tokenStore.getToken();
  // console.log("token", tokenStore.token);

  // 檢查Token是否存在
  if (!tokenStore.token) {
    logout();
    return;
  }

  // 呼叫api
  let requestData: MbsBscHttpHeartbeatReqMdl = {
    employeeLoginToken: tokenStore.token,
  };
  let responseData = await heartbeat(requestData);
  if (responseData == null) {
    // console.log("系統錯誤");
    logout();
    return;
  }
  if (responseData.errorCode != MbsErrorCodeEnum.Success) {
    // console.log("心跳失敗")
    logout();
    return;
  }
};

/** 啟動心跳 每六分鐘 */
const startHeartbeat = () => {
  if (isHeartbeatRunning.value) {
    return; // 如果心跳已經在執行，則不再啟動
  }
  heartbeatInterval = setInterval(sendHeartbeat, 360000); // 每6分鐘發送一次
  // console.log("計時器ID", heartbeatInterval);
  isHeartbeatRunning.value = true;
  // console.log("心跳啟動");
};

/** 停止心跳 */
const stopHeartbeat = () => {
  // 如果心跳不存在
  if (!heartbeatInterval) {
    console.log("心跳提早停止");
    return; // 提早結束
  }
  // 清除計時器
  clearInterval(heartbeatInterval);
  heartbeatInterval = null;
  isHeartbeatRunning.value = false;
  // console.log("心跳停止");
};

onMounted(() => {
  // 從 store 讀取 token
  tokenStore.getToken();

  // 如果 token 存在，啟動心跳
  if (tokenStore.token) {
    startHeartbeat();
  } else {
    // 如果 token 不存在，執行登出操作
    logout();
  }
});

onUnmounted(() => {
  stopHeartbeat();
});

watch(
  () => tokenStore.token,
  (newToken) => {
    // 當 Token 為 null 時，停止心跳並登出
    if (!newToken) {
      // console.log("Token不存在 停止心跳並登出");
      logout();
    } else {
      // 當 Token 有效時，重新啟動心跳
      console.log("Token已更新 重新啟動心跳");
      startHeartbeat();
    }
  }
);
</script>

<template>
  <router-view />
</template>
