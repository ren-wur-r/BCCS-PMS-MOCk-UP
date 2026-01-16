import { ref } from "vue";

const moduleTitleOverride = ref<string | null>(null);

const setModuleTitle = (title: string) => {
  moduleTitleOverride.value = title;
};

const clearModuleTitle = () => {
  moduleTitleOverride.value = null;
};

export const useModuleTitleStore = () => ({
  moduleTitleOverride,
  setModuleTitle,
  clearModuleTitle,
});
