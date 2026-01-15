import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";

/**
 * 取得城市名稱文字標籤
 * @param value 城市列舉值
 * @returns 顯示文字（未選擇 / 未定義 / 基隆市 / 臺北市 ...）
 */
export const getCityLabel = (value: DbAtomCityEnum|null): string => {
  switch (value) {
    case DbAtomCityEnum.NotSelected:
      return "未選擇";
    case DbAtomCityEnum.Undefined:
      return "未定義";
    case DbAtomCityEnum.Keelung:
      return "基隆市";
    case DbAtomCityEnum.NewTaipei:
      return "新北市";
    case DbAtomCityEnum.Taipei:
      return "臺北市";
    case DbAtomCityEnum.Taoyuan:
      return "桃園市";
    case DbAtomCityEnum.HsinchuCounty:
      return "新竹縣";
    case DbAtomCityEnum.HsinchuCity:
      return "新竹市";
    case DbAtomCityEnum.Miaoli:
      return "苗栗縣";
    case DbAtomCityEnum.Taichung:
      return "臺中市";
    case DbAtomCityEnum.Changhua:
      return "彰化縣";
    case DbAtomCityEnum.Nantou:
      return "南投縣";
    case DbAtomCityEnum.Yunlin:
      return "雲林縣";
    case DbAtomCityEnum.ChiayiCounty:
      return "嘉義縣";
    case DbAtomCityEnum.ChiayiCity:
      return "嘉義市";
    case DbAtomCityEnum.Tainan:
      return "臺南市";
    case DbAtomCityEnum.Kaohsiung:
      return "高雄市";
    case DbAtomCityEnum.Pingtung:
      return "屏東縣";
    case DbAtomCityEnum.Taitung:
      return "臺東縣";
    case DbAtomCityEnum.Hualien:
      return "花蓮縣";
    case DbAtomCityEnum.Yilan:
      return "宜蘭縣";
    case DbAtomCityEnum.Penghu:
      return "澎湖縣";
    case DbAtomCityEnum.Kinmen:
      return "金門縣";
    case DbAtomCityEnum.Lienchiang:
      return "連江縣";
    default:
      return "-";
  }
};
