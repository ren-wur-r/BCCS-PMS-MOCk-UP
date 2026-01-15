<script setup lang="ts">
import { reactive, watch } from 'vue';

interface Props {
    data: SysPrdSpecificationListItem;
}

interface Emits {
    (e: 'confirm', payload: SysPrdSpecificationListItem): void
    (e: 'close'): void
}
// Props
const props = defineProps<Props>();
// Emits
const emit = defineEmits<Emits>();
//------------------------------------------------------
/** 系統設定-產品-規格-列表-項目 */
interface SysPrdSpecificationListItem {
    managerProductSpecificationID: number;
    managerProductSpecificationName: string;
    managerProductSpecificationSellPrice: number;
    managerProductSpecificationCostPrice: number;
    managerProductSpecificationIsEnable: boolean;
}

const sysPrdSpecificationListItemObj = reactive<SysPrdSpecificationListItem>({
    managerProductSpecificationID: 0,
    managerProductSpecificationName: '',
    managerProductSpecificationSellPrice: 0,
    managerProductSpecificationCostPrice: 0,
    managerProductSpecificationIsEnable: true,
})

/** 系統設定-產品-規格-驗證模型 */
interface SysPrdSpecificationValidateMdl {
    managerProductSpecificationID: boolean;
    managerProductSpecificationName: boolean;
    managerProductSpecificationSellPrice: boolean;
    managerProductSpecificationCostPrice: boolean;
    managerProductSpecificationIsEnable: boolean;
}

/** 系統設定-產品-規格-驗證物件 */
const sysPrdSpecificationValidObj = reactive<SysPrdSpecificationValidateMdl>({
    managerProductSpecificationID: true,
    managerProductSpecificationName: true,
    managerProductSpecificationSellPrice: true,
    managerProductSpecificationCostPrice: true,
    managerProductSpecificationIsEnable: true,
});

//------------------------------------------------------
/** 驗證【規格】欄位 */
const validateSpecificationField = () => {
    let isValid = true;

    // 規格名稱
    if (
        typeof sysPrdSpecificationListItemObj.managerProductSpecificationName !== "string" ||
        !sysPrdSpecificationListItemObj.managerProductSpecificationName.trim() ||
        sysPrdSpecificationListItemObj.managerProductSpecificationName.trim().length > 100
    ) {
        sysPrdSpecificationValidObj.managerProductSpecificationName = false;
        isValid = false;
    } else {
        sysPrdSpecificationValidObj.managerProductSpecificationName = true;
    }

    // 售價
    if (
        sysPrdSpecificationListItemObj.managerProductSpecificationSellPrice === null
        ||
        isNaN(sysPrdSpecificationListItemObj.managerProductSpecificationSellPrice)
    ) {
        sysPrdSpecificationValidObj.managerProductSpecificationSellPrice = false;
        isValid = false;
    } else {
        sysPrdSpecificationValidObj.managerProductSpecificationSellPrice = true;
    }

    // 成本
    if (
        sysPrdSpecificationListItemObj.managerProductSpecificationCostPrice === null ||
        isNaN(sysPrdSpecificationListItemObj.managerProductSpecificationCostPrice)
    ) {
        sysPrdSpecificationValidObj.managerProductSpecificationCostPrice = false;
        isValid = false;
    } else {
        sysPrdSpecificationValidObj.managerProductSpecificationCostPrice = true;
    }

    // 狀態
    if (typeof sysPrdSpecificationListItemObj.managerProductSpecificationIsEnable !== "boolean") {
        sysPrdSpecificationValidObj.managerProductSpecificationIsEnable = false;
        isValid = false;
    } else {
        sysPrdSpecificationValidObj.managerProductSpecificationIsEnable = true;
    }

    return isValid;
};

/** 點擊提交更新規格按鈕 */
const clickSubmitUpdateSpecificationBtn = () => {
    if (validateSpecificationField()) {

        emit('confirm', { ...sysPrdSpecificationListItemObj });

        // 關閉彈跳視窗
        emit('close');
    }
};

//------------------------------------------------------
watch(() => props.data,
    (val) => {
        if (!val) return;
        Object.assign(sysPrdSpecificationListItemObj, val)
    },
    {
        immediate: true
    });
</script>

<template>
    <div class="modal-overlay">
        <div class="max-w-md w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col">
            <!-- 標題區域 -->
            <div class="flex items-center justify-between p-3 flex-shrink-0">
                <h2 class="modal-title">編輯規格</h2>
                <button class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
                    aria-label="關閉" @click="emit('close');">
                    ×
                </button>
            </div>

            <hr class="flex-shrink-0" />

            <!-- 內容區域 - 可滾動 -->
            <div class="p-8 overflow-y-auto flex-1">
                <div class="space-y-4">
                    <!-- 規格名稱 -->
                    <div>
                        <label class="form-label">規格名稱 <span class="required-label">*</span></label>
                        <input v-model="sysPrdSpecificationListItemObj.managerProductSpecificationName"
                            class="input-box" placeholder="請輸入規格名稱" />
                        <p class="error-wrapper">
                            <span v-if="!sysPrdSpecificationValidObj.managerProductSpecificationName" class="error-tip">
                                不可為空，長度不可超過 100 個字
                            </span>
                        </p>
                    </div>

                    <!-- 售價和成本 -->
                    <div class="flex gap-4">
                        <!-- 售價 -->
                        <div class="flex-1">
                            <label class="form-label">售價 <span class="required-label">*</span></label>
                            <input v-model.number="sysPrdSpecificationListItemObj.managerProductSpecificationSellPrice"
                                type="number" class="input-box" placeholder="請輸入售價" />
                            <p class="error-wrapper">
                                <span v-if="!sysPrdSpecificationValidObj.managerProductSpecificationSellPrice"
                                    class="error-tip">
                                    不可為空
                                </span>
                            </p>
                        </div>

                        <!-- 成本 -->
                        <div class="flex-1">
                            <label class="form-label">成本 <span class="required-label">*</span></label>
                            <input v-model.number="sysPrdSpecificationListItemObj.managerProductSpecificationCostPrice"
                                type="number" class="input-box" placeholder="請輸入成本" />
                            <p class="error-wrapper">
                                <span v-if="!sysPrdSpecificationValidObj.managerProductSpecificationCostPrice"
                                    class="error-tip">
                                    不可為空
                                </span>
                            </p>
                        </div>
                    </div>

                    <!-- 狀態 -->
                    <div>
                        <label class="form-label">狀態 <span class="required-label">*</span></label>
                        <select v-model="sysPrdSpecificationListItemObj.managerProductSpecificationIsEnable"
                            class="select-box">
                            <option :value="true">啟用</option>
                            <option :value="false">停用</option>
                        </select>
                        <p class="h-3"></p>
                    </div>
                </div>
            </div>

            <!-- 按鈕區域 - 固定在底部 -->
            <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
                <div class="flex justify-end gap-2">
                    <button class="btn-cancel" @click="
                        () => {
                            emit('close');
                        }
                    ">
                        取消
                    </button>
                    <button class="btn-submit" @click="clickSubmitUpdateSpecificationBtn()">送出</button>
                </div>
            </div>
        </div>
    </div>

</template>