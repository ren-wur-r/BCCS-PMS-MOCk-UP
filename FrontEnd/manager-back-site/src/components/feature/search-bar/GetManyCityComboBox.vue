<script setup lang="ts">
import { reactive, computed, watch } from "vue";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";

//--------------------------------------------------------------
// Props & Emits
//--------------------------------------------------------------
const props = defineProps<{
  disabled?: boolean;
  atomCity: DbAtomCityEnum | null;
}>();

const emit = defineEmits<{
  (e: "update:atomCity", value: DbAtomCityEnum | null): void;
}>();

//--------------------------------------------------------------
// 型別定義
//--------------------------------------------------------------
interface CityComboBoxItem {
  label: string;
  value: DbAtomCityEnum;
}

//--------------------------------------------------------------
// 狀態模型
//--------------------------------------------------------------
interface CityComboBoxViewMdl {
  keyword: string;
  showDropdown: boolean;
  cityList: CityComboBoxItem[];
}
const cityComboBoxViewObj = reactive<CityComboBoxViewMdl>({
  keyword: "",
  showDropdown: false,
  cityList: [],
});

//--------------------------------------------------------------
// 初始化城市清單
//--------------------------------------------------------------
const allCities: CityComboBoxItem[] = Object.entries(DbAtomCityEnum)
  .filter(([key]) => isNaN(Number(key))) // 排除反向映射
  .filter(([key]) => !["NotSelected", "Undefined"].includes(key))
  .map(([key, value]) => ({
    label: getCityLabel(key),
    value: value as DbAtomCityEnum,
  }));

//--------------------------------------------------------------
// 搜尋過濾
//--------------------------------------------------------------
const filteredCities = computed(() => {
  const q = cityComboBoxViewObj.keyword.trim();
  if (!q) return allCities;
  return allCities.filter((c) => c.label.includes(q));
});

//--------------------------------------------------------------
// 事件 : Focus 打開下拉
//--------------------------------------------------------------
const focusInput = () => {
  if (props.disabled) return;
  cityComboBoxViewObj.showDropdown = true;
};

//--------------------------------------------------------------
// 事件 : 點擊下拉按鈕
//--------------------------------------------------------------
const clickDropdownBtn = () => {
  if (props.disabled) return;
  cityComboBoxViewObj.showDropdown = !cityComboBoxViewObj.showDropdown;
};

//--------------------------------------------------------------
// 事件 : 選取城市
//--------------------------------------------------------------
const selectItem = (item: CityComboBoxItem) => {
  cityComboBoxViewObj.keyword = item.label;
  emit("update:atomCity", item.value);
  cityComboBoxViewObj.showDropdown = false;
};

//--------------------------------------------------------------
// 事件 : 關閉下拉
//--------------------------------------------------------------
const closeDropdown = () => {
  cityComboBoxViewObj.showDropdown = false;
};

//--------------------------------------------------------------
// 轉中文名稱
//--------------------------------------------------------------
function getCityLabel(key: string): string {
  const map: Record<string, string> = {
    Keelung: "基隆市",
    NewTaipei: "新北市",
    Taipei: "臺北市",
    Taoyuan: "桃園市",
    HsinchuCounty: "新竹縣",
    HsinchuCity: "新竹市",
    Miaoli: "苗栗縣",
    Taichung: "臺中市",
    Changhua: "彰化縣",
    Nantou: "南投縣",
    Yunlin: "雲林縣",
    ChiayiCounty: "嘉義縣",
    ChiayiCity: "嘉義市",
    Tainan: "臺南市",
    Kaohsiung: "高雄市",
    Pingtung: "屏東縣",
    Taitung: "臺東縣",
    Hualien: "花蓮縣",
    Yilan: "宜蘭縣",
    Penghu: "澎湖縣",
    Kinmen: "金門縣",
    Lienchiang: "連江縣",
  };
  return map[key] ?? key;
}
//負責當父層資料改變時，更新 ComboBox 顯示文字
watch(
  () => props.atomCity,
  (newVal) => {
    if (newVal === null || newVal === DbAtomCityEnum.NotSelected) {
      cityComboBoxViewObj.keyword = "";
    } else {
      cityComboBoxViewObj.keyword = getCityLabel(DbAtomCityEnum[newVal]);
    }
  },
  { immediate: true }
);
//使用者把文字刪光（keyword = ""）時，主動通知父層說縣市被清空了
watch(
  () => cityComboBoxViewObj.keyword,
  (val) => {
    if (!val.trim()) {
      emit("update:atomCity", DbAtomCityEnum.NotSelected);
    }
  }
);
</script>

<template>
  <div v-click-outside="closeDropdown" class="relative flex w-full">
    <!-- 輸入框 -->
    <input
      v-model="cityComboBoxViewObj.keyword"
      type="text"
      placeholder="輸入城市名稱"
      class="input-box rounded-r-none"
      :disabled="props.disabled"
      @focus="focusInput"
    />

    <!-- 下拉按鈕 -->
    <button
      type="button"
      class="flex items-center justify-center px-2 border border-gray-300 border-l-0 rounded-r-md bg-white hover:bg-gray-50 disabled:bg-gray-100 disabled:cursor-not-allowed"
      :disabled="props.disabled"
      @click="clickDropdownBtn"
    >
      <svg
        v-if="cityComboBoxViewObj.showDropdown"
        class="w-5 h-5 text-gray-500"
        width="20"
        height="25"
        viewBox="0 0 20 25"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <path
          d="M9.69149 8.50456L4.33316 15.4108C3.99899 15.8431 4.20149 16.666 4.64149 16.666H15.3582C15.7982 16.666 16.0007 15.8431 15.6665 15.4108L10.3082 8.50456C10.1307 8.27539 9.86899 8.27539 9.69149 8.50456Z"
          fill="#787676"
        />
      </svg>
      <svg
        v-else
        class="w-5 h-5 text-gray-500"
        width="20"
        height="25"
        viewBox="0 0 20 25"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <path
          d="M10.3085 16.4954L15.6668 9.58917C16.001 9.15687 15.7985 8.33398 15.3585 8.33398H4.64182C4.20182 8.33398 3.99932 9.15687 4.33349 9.58917L9.69182 16.4954C9.86932 16.7246 10.131 16.7246 10.3085 16.4954Z"
          fill="#787676"
        />
      </svg>
    </button>

    <!-- 下拉選單 -->
    <transition
      enter-active-class="transition-all duration-200 ease-out"
      enter-from-class="opacity-0 transform -translate-y-2"
      enter-to-class="opacity-100 transform translate-y-0"
      leave-active-class="transition-all duration-150 ease-in"
      leave-from-class="opacity-100 transform translate-y-0"
      leave-to-class="opacity-0 transform -translate-y-2"
    >
      <div
        v-if="cityComboBoxViewObj.showDropdown && !props.disabled"
        class="absolute top-full left-0 w-full max-h-40 overflow-y-auto border border-gray-300 bg-white z-50 shadow-md rounded-md mt-1"
      >
        <ul v-if="filteredCities.length === 0">
          <li class="px-3 py-2 text-sm text-gray-500">查無資料</li>
        </ul>
        <ul v-else>
          <li
            v-for="item in filteredCities"
            :key="item.value"
            class="px-3 py-2 text-sm cursor-pointer hover:bg-gray-100"
            @click="selectItem(item)"
          >
            {{ item.label }}
          </li>
        </ul>
      </div>
    </transition>
  </div>
</template>
