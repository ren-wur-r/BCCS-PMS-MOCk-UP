<script setup lang="ts">
import { ref, computed } from "vue";
import { useMockDataStore, type Announcement } from "@/stores/mockDataStore";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";

const mockStore = useMockDataStore();
const employeeInfoStore = useEmployeeInfoStore();

const isExpanded = ref(true);
const newAnnouncementTitle = ref("");
const newAnnouncementContent = ref("");
const showAddModal = ref(false);

const canPost = computed(() => {
  const role = employeeInfoStore.managerRoleName;
  return role.includes("總經理") || role.includes("處長") || role.includes("Admin");
});

const handleAdd = () => {
  if (!newAnnouncementTitle.value || !newAnnouncementContent.value) return;
  
  mockStore.addAnnouncement({
    title: newAnnouncementTitle.value,
    content: newAnnouncementContent.value,
    date: new Date().toISOString().split('T')[0],
    author: employeeInfoStore.employeeName
  });
  
  newAnnouncementTitle.value = "";
  newAnnouncementContent.value = "";
  showAddModal.value = false;
};

const handleDelete = (id: number) => {
  if (confirm("確定要刪除此公告嗎？")) {
    mockStore.removeAnnouncement(id);
  }
};
</script>

<template>
  <div></div>
</template>
