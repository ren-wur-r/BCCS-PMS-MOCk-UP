<script setup lang="ts">
const props = defineProps<{
  modelValue: boolean;
  disabled?: boolean;
}>();

const emit = defineEmits(["update:modelValue"]);
</script>

<template>
  <v-switch
    :model-value="modelValue"
    density="comfortable"
    :disabled="disabled"
    color="success"
    :label="props.modelValue ? '啟用' : '停用'"
    hide-details
    @update:model-value="emit('update:modelValue', $event)"
  />
</template>

<style scoped>
.v-switch .v-label {
  padding-inline-start: 10px;
  font-size: var(--font-size-md);
}
</style>
