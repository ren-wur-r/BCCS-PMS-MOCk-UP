<script setup lang="ts">
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import {
  projectTypeTemplateSeeds,
  contractTemplateSeeds,
  reminderRuleSeed,
  type ProjectTypeTemplate,
  type ProjectTemplateStage,
  type ProjectTemplateOutput,
  type ContractTemplate,
} from "@/views/work/project/projectTemplateSeeds";

const router = useRouter();

const projectTypes = ref<ProjectTypeTemplate[]>(
  JSON.parse(JSON.stringify(projectTypeTemplateSeeds)) as ProjectTypeTemplate[]
);
const contractTemplates = ref<ContractTemplate[]>(
  JSON.parse(JSON.stringify(contractTemplateSeeds)) as ContractTemplate[]
);

const selectedTypeId = ref<number | null>(
  projectTypes.value.length > 0 ? projectTypes.value[0].projectTypeId : null
);
const selectedType = computed(
  () => projectTypes.value.find((item) => item.projectTypeId === selectedTypeId.value) ?? null
);

const isAddingType = ref(false);
const isEditingType = ref(false);
const typeForm = ref({
  projectTypeName: "",
  description: "",
  contractTemplateId: 0,
});

const isAddingStage = ref(false);
const editingStageId = ref<number | null>(null);
const stageForm = ref({
  stageName: "",
  ownerRoleLabel: "",
  requiresWorkLog: true,
  requiresOnsiteLog: false,
});

const isAddingOutput = ref(false);
const editingOutputId = ref<number | null>(null);
const outputForm = ref({
  stageId: 0,
  outputName: "",
  outputKind: "document" as ProjectTemplateOutput["outputKind"],
  required: true,
});

const isAddingContract = ref(false);
const contractForm = ref({
  contractTemplateName: "",
  description: "",
});

const resetTypeForm = () => {
  typeForm.value = {
    projectTypeName: "",
    description: "",
    contractTemplateId: contractTemplates.value[0]?.contractTemplateId ?? 0,
  };
};

const startAddType = () => {
  isAddingType.value = true;
  isEditingType.value = false;
  resetTypeForm();
};

const startEditType = () => {
  if (!selectedType.value) return;
  isEditingType.value = true;
  isAddingType.value = false;
  typeForm.value = {
    projectTypeName: selectedType.value.projectTypeName,
    description: selectedType.value.description,
    contractTemplateId: selectedType.value.contractTemplateId,
  };
};

const saveType = () => {
  if (isAddingType.value) {
    const newId =
      Math.max(0, ...projectTypes.value.map((item) => item.projectTypeId)) + 1;
    projectTypes.value.push({
      projectTypeId: newId,
      projectTypeName: typeForm.value.projectTypeName.trim() || "未命名類型",
      description: typeForm.value.description.trim(),
      contractTemplateId: typeForm.value.contractTemplateId,
      stageTemplates: [],
    });
    selectedTypeId.value = newId;
  } else if (isEditingType.value && selectedType.value) {
    selectedType.value.projectTypeName =
      typeForm.value.projectTypeName.trim() || selectedType.value.projectTypeName;
    selectedType.value.description = typeForm.value.description.trim();
    selectedType.value.contractTemplateId = typeForm.value.contractTemplateId;
  }
  isAddingType.value = false;
  isEditingType.value = false;
};

const cancelTypeForm = () => {
  isAddingType.value = false;
  isEditingType.value = false;
};

const startAddStage = () => {
  if (!selectedType.value) return;
  isAddingStage.value = true;
  editingStageId.value = null;
  stageForm.value = {
    stageName: "",
    ownerRoleLabel: "",
    requiresWorkLog: true,
    requiresOnsiteLog: false,
  };
};

const startEditStage = (stage: ProjectTemplateStage) => {
  editingStageId.value = stage.stageId;
  isAddingStage.value = false;
  stageForm.value = {
    stageName: stage.stageName,
    ownerRoleLabel: stage.ownerRoleLabel,
    requiresWorkLog: stage.requiresWorkLog,
    requiresOnsiteLog: stage.requiresOnsiteLog,
  };
};

const saveStage = () => {
  if (!selectedType.value) return;
  if (isAddingStage.value) {
    const newId =
      Math.max(0, ...selectedType.value.stageTemplates.map((item) => item.stageId)) + 1;
    selectedType.value.stageTemplates.push({
      stageId: newId,
      stageName: stageForm.value.stageName.trim() || "未命名階段",
      ownerRoleLabel: stageForm.value.ownerRoleLabel.trim() || "未設定",
      requiredOutputs: [],
      requiresWorkLog: stageForm.value.requiresWorkLog,
      requiresOnsiteLog: stageForm.value.requiresOnsiteLog,
    });
  } else if (editingStageId.value !== null) {
    const target = selectedType.value.stageTemplates.find(
      (item) => item.stageId === editingStageId.value
    );
    if (target) {
      target.stageName = stageForm.value.stageName.trim() || target.stageName;
      target.ownerRoleLabel = stageForm.value.ownerRoleLabel.trim() || target.ownerRoleLabel;
      target.requiresWorkLog = stageForm.value.requiresWorkLog;
      target.requiresOnsiteLog = stageForm.value.requiresOnsiteLog;
    }
  }
  isAddingStage.value = false;
  editingStageId.value = null;
};

const cancelStageForm = () => {
  isAddingStage.value = false;
  editingStageId.value = null;
};

const moveStage = (index: number, delta: number) => {
  if (!selectedType.value) return;
  const targetIndex = index + delta;
  if (targetIndex < 0 || targetIndex >= selectedType.value.stageTemplates.length) return;
  const list = selectedType.value.stageTemplates;
  const [stage] = list.splice(index, 1);
  list.splice(targetIndex, 0, stage);
};

const startAddOutput = (stageId: number) => {
  isAddingOutput.value = true;
  editingOutputId.value = null;
  outputForm.value = {
    stageId,
    outputName: "",
    outputKind: "document",
    required: true,
  };
};

const startEditOutput = (stageId: number, output: ProjectTemplateOutput) => {
  isAddingOutput.value = false;
  editingOutputId.value = output.outputId;
  outputForm.value = {
    stageId,
    outputName: output.outputName,
    outputKind: output.outputKind,
    required: output.required,
  };
};

const saveOutput = () => {
  if (!selectedType.value) return;
  const stage = selectedType.value.stageTemplates.find(
    (item) => item.stageId === outputForm.value.stageId
  );
  if (!stage) return;
  if (isAddingOutput.value) {
    const newId =
      Math.max(0, ...stage.requiredOutputs.map((item) => item.outputId)) + 1;
    stage.requiredOutputs.push({
      outputId: newId,
      outputName: outputForm.value.outputName.trim() || "未命名產出",
      outputKind: outputForm.value.outputKind,
      required: outputForm.value.required,
    });
  } else if (editingOutputId.value !== null) {
    const output = stage.requiredOutputs.find((item) => item.outputId === editingOutputId.value);
    if (output) {
      output.outputName = outputForm.value.outputName.trim() || output.outputName;
      output.outputKind = outputForm.value.outputKind;
      output.required = outputForm.value.required;
    }
  }
  isAddingOutput.value = false;
  editingOutputId.value = null;
};

const cancelOutputForm = () => {
  isAddingOutput.value = false;
  editingOutputId.value = null;
};

const moveOutput = (stage: ProjectTemplateStage, index: number, delta: number) => {
  const targetIndex = index + delta;
  if (targetIndex < 0 || targetIndex >= stage.requiredOutputs.length) return;
  const list = stage.requiredOutputs;
  const [output] = list.splice(index, 1);
  list.splice(targetIndex, 0, output);
};

const startAddContract = () => {
  isAddingContract.value = true;
  contractForm.value = {
    contractTemplateName: "",
    description: "",
  };
};

const saveContract = () => {
  const newId =
    Math.max(0, ...contractTemplates.value.map((item) => item.contractTemplateId)) + 1;
  contractTemplates.value.push({
    contractTemplateId: newId,
    contractTemplateName: contractForm.value.contractTemplateName.trim() || "未命名合約",
    description: contractForm.value.description.trim(),
  });
  isAddingContract.value = false;
};

const cancelContract = () => {
  isAddingContract.value = false;
};
</script>

<template>
  <div class="flex flex-col h-full p-4 gap-4">
    <div class="grid grid-cols-12 gap-4 h-full">
      <div class="col-span-4 flex flex-col gap-4">
        <div class="bg-white rounded-lg p-4 shadow-sm">
          <div class="flex items-center justify-between">
            <h2 class="subtitle">專案類型</h2>
            <button class="btn-add" @click="startAddType">新增類型</button>
          </div>
          <p class="text-xs text-gray-500 mt-1">
            選擇類型後，可設定標準階段、產出與合約模板。
          </p>
          <div class="mt-3 space-y-2">
            <button
              v-for="type in projectTypes"
              :key="type.projectTypeId"
              class="w-full text-left rounded-lg border px-3 py-2 transition"
              :class="
                type.projectTypeId === selectedTypeId
                  ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                  : 'border-gray-200 hover:border-cyan-400'
              "
              @click="selectedTypeId = type.projectTypeId"
            >
              <p class="font-semibold">{{ type.projectTypeName }}</p>
              <p class="text-xs text-gray-500 line-clamp-2">{{ type.description || '無描述' }}</p>
            </button>
          </div>
        </div>

        <div class="bg-white rounded-lg p-4 shadow-sm">
          <div class="flex items-center justify-between">
            <h2 class="subtitle">合約模板</h2>
            <button class="btn-add" @click="startAddContract">新增合約</button>
          </div>
          <div class="mt-3 space-y-2">
            <div
              v-for="contract in contractTemplates"
              :key="contract.contractTemplateId"
              class="rounded-lg border border-gray-200 p-3"
            >
              <p class="font-semibold text-gray-700">{{ contract.contractTemplateName }}</p>
              <p class="text-xs text-gray-500">{{ contract.description || '無描述' }}</p>
            </div>
          </div>

          <div v-if="isAddingContract" class="mt-4 space-y-2">
            <input
              v-model="contractForm.contractTemplateName"
              class="input-box"
              placeholder="合約名稱"
            />
            <textarea
              v-model="contractForm.description"
              class="input-box min-h-[80px]"
              placeholder="合約說明"
            />
            <div class="flex justify-end gap-2">
              <button class="btn-cancel" @click="cancelContract">取消</button>
              <button class="btn-submit" @click="saveContract">儲存</button>
            </div>
          </div>
        </div>
      </div>

      <div class="col-span-8 flex flex-col gap-4">
        <div class="bg-white rounded-lg p-6 shadow-sm">
          <div class="flex items-center justify-between">
            <div>
              <h2 class="subtitle">類型設定</h2>
              <p class="text-xs text-gray-500 mt-1">
                提醒規則：剩 {{ reminderRuleSeed.supervisorNotifyBeforeDays }} 天通知直屬主管；
                剩 {{ reminderRuleSeed.gmNotifyBeforeDays }} 天通知總經理並列為風險
              </p>
            </div>
            <div class="flex gap-2">
              <button class="btn-update" @click="startEditType" :disabled="!selectedType">
                編輯
              </button>
            </div>
          </div>

          <div v-if="selectedType" class="mt-4 grid grid-cols-2 gap-4">
            <div>
              <label class="form-label">類型名稱</label>
              <input class="input-box" :value="selectedType.projectTypeName" disabled />
            </div>
            <div>
              <label class="form-label">綁定合約模板</label>
              <input
                class="input-box"
                :value="
                  contractTemplates.find(
                    (item) => item.contractTemplateId === selectedType.contractTemplateId
                  )?.contractTemplateName || '-'
                "
                disabled
              />
            </div>
            <div class="col-span-2">
              <label class="form-label">描述</label>
              <textarea class="input-box min-h-[80px]" :value="selectedType.description" disabled />
            </div>
          </div>

          <div v-else class="text-sm text-gray-500 mt-3">請先選擇專案類型。</div>

          <div v-if="isAddingType || isEditingType" class="mt-6 border-t pt-4 space-y-3">
            <div class="grid grid-cols-2 gap-4">
              <div>
                <label class="form-label">類型名稱</label>
                <input v-model="typeForm.projectTypeName" class="input-box" />
              </div>
              <div>
                <label class="form-label">綁定合約模板</label>
                <select v-model="typeForm.contractTemplateId" class="select-box">
                  <option
                    v-for="contract in contractTemplates"
                    :key="contract.contractTemplateId"
                    :value="contract.contractTemplateId"
                  >
                    {{ contract.contractTemplateName }}
                  </option>
                </select>
              </div>
            </div>
            <div>
              <label class="form-label">描述</label>
              <textarea v-model="typeForm.description" class="input-box min-h-[80px]" />
            </div>
            <div class="flex justify-end gap-2">
              <button class="btn-cancel" @click="cancelTypeForm">取消</button>
              <button class="btn-submit" @click="saveType">儲存</button>
            </div>
          </div>
        </div>

        <div class="bg-white rounded-lg p-6 shadow-sm">
          <div class="flex items-center justify-between">
            <h2 class="subtitle">標準階段與產出</h2>
            <button class="btn-add" :disabled="!selectedType" @click="startAddStage">
              新增階段
            </button>
          </div>

          <div v-if="!selectedType" class="text-sm text-gray-500 mt-4">
            請先選擇專案類型以設定階段。
          </div>

          <div v-else class="mt-4 space-y-4">
            <div
              v-for="(stage, stageIndex) in selectedType.stageTemplates"
              :key="stage.stageId"
              class="rounded-lg border border-gray-200 bg-gray-50 p-4"
            >
              <div class="flex items-center justify-between">
                <div>
                  <p class="font-semibold text-gray-700">{{ stage.stageName }}</p>
                  <p class="text-xs text-gray-500">負責角色：{{ stage.ownerRoleLabel }}</p>
                </div>
                <div class="flex items-center gap-2">
                  <button class="btn-update" @click="startEditStage(stage)">編輯</button>
                  <button class="btn-cancel" @click="moveStage(stageIndex, -1)">上移</button>
                  <button class="btn-cancel" @click="moveStage(stageIndex, 1)">下移</button>
                </div>
              </div>
              <div class="flex gap-2 mt-2">
                <span
                  class="rounded-full bg-cyan-50 px-2 py-0.5 text-xs text-cyan-700"
                  v-if="stage.requiresWorkLog"
                >
                  需工作日誌
                </span>
                <span
                  class="rounded-full bg-amber-50 px-2 py-0.5 text-xs text-amber-700"
                  v-if="stage.requiresOnsiteLog"
                >
                  需駐點打卡
                </span>
              </div>

              <div class="mt-3">
                <div class="flex items-center justify-between">
                  <p class="text-sm font-semibold text-gray-600">產出清單</p>
                  <button class="btn-add" @click="startAddOutput(stage.stageId)">新增產出</button>
                </div>
                <div v-if="stage.requiredOutputs.length === 0" class="text-sm text-gray-500 mt-2">
                  尚未設定產出
                </div>
                <div
                  v-else
                  class="mt-2 space-y-2"
                >
                  <div
                    v-for="(output, outputIndex) in stage.requiredOutputs"
                    :key="output.outputId"
                    class="flex items-center justify-between rounded-md border border-gray-200 bg-white px-3 py-2"
                  >
                    <div>
                      <p class="text-sm text-gray-700">{{ output.outputName }}</p>
                      <p class="text-xs text-gray-500">
                        類型：{{ output.outputKind }}｜{{ output.required ? "必填" : "選填" }}
                      </p>
                    </div>
                    <div class="flex items-center gap-2">
                      <button
                        class="btn-update"
                        @click="startEditOutput(stage.stageId, output)"
                      >
                        編輯
                      </button>
                      <button class="btn-cancel" @click="moveOutput(stage, outputIndex, -1)">
                        上移
                      </button>
                      <button class="btn-cancel" @click="moveOutput(stage, outputIndex, 1)">
                        下移
                      </button>
                    </div>
                  </div>
                </div>

                <div v-if="isAddingOutput || editingOutputId !== null" class="mt-3 space-y-2">
                  <div class="grid grid-cols-2 gap-4">
                    <div>
                      <label class="form-label">產出名稱</label>
                      <input v-model="outputForm.outputName" class="input-box" />
                    </div>
                    <div>
                      <label class="form-label">產出類型</label>
                      <select v-model="outputForm.outputKind" class="select-box">
                        <option value="document">文件</option>
                        <option value="meeting">會議</option>
                        <option value="report">報告</option>
                        <option value="other">其他</option>
                      </select>
                    </div>
                  </div>
                  <div class="flex items-center gap-2">
                    <input v-model="outputForm.required" type="checkbox" class="w-4 h-4" />
                    <span class="text-sm text-gray-600">必填</span>
                  </div>
                  <div class="flex justify-end gap-2">
                    <button class="btn-cancel" @click="cancelOutputForm">取消</button>
                    <button class="btn-submit" @click="saveOutput">儲存</button>
                  </div>
                </div>
              </div>
            </div>

            <div v-if="isAddingStage || editingStageId !== null" class="border-t pt-4 space-y-3">
              <div class="grid grid-cols-2 gap-4">
                <div>
                  <label class="form-label">階段名稱</label>
                  <input v-model="stageForm.stageName" class="input-box" />
                </div>
                <div>
                  <label class="form-label">負責角色</label>
                  <input v-model="stageForm.ownerRoleLabel" class="input-box" />
                </div>
              </div>
              <div class="flex items-center gap-4">
                <label class="flex items-center gap-2 text-sm text-gray-600">
                  <input v-model="stageForm.requiresWorkLog" type="checkbox" class="w-4 h-4" />
                  需工作日誌
                </label>
                <label class="flex items-center gap-2 text-sm text-gray-600">
                  <input v-model="stageForm.requiresOnsiteLog" type="checkbox" class="w-4 h-4" />
                  需駐點打卡
                </label>
              </div>
              <div class="flex justify-end gap-2">
                <button class="btn-cancel" @click="cancelStageForm">取消</button>
                <button class="btn-submit" @click="saveStage">儲存</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
