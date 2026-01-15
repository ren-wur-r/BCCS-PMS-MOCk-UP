using System.ComponentModel;

namespace DataModelLibrary.Database.AtomCity;

/// <summary>資料庫-原子-城市-列舉</summary>
public enum DbAtomCityEnum : short
{
    /// <summary>未選擇</summary>
    [Description("未選擇")]
    NotSelected = -1,

    /// <summary>未定義</summary>
    [Description("未定義")]
    Undefined = 0,

    /// <summary>基隆市</summary>
    [Description("基隆市")]
    Keelung = 1,

    /// <summary>新北市</summary>
    [Description("新北市")]
    NewTaipei = 2,

    /// <summary>臺北市</summary>
    [Description("臺北市")]
    Taipei = 3,

    /// <summary>桃園市</summary>
    [Description("桃園市")]
    Taoyuan = 4,

    /// <summary>新竹縣</summary>
    [Description("新竹縣")]
    HsinchuCounty = 5,

    /// <summary>新竹市</summary>
    [Description("新竹市")]
    HsinchuCity = 6,

    /// <summary>苗栗縣</summary>
    [Description("苗栗縣")]
    Miaoli = 7,

    /// <summary>臺中市</summary>
    [Description("臺中市")]
    Taichung = 8,

    /// <summary>彰化縣</summary>
    [Description("彰化縣")]
    Changhua = 9,

    /// <summary>南投縣</summary>
    [Description("南投縣")]
    Nantou = 10,

    /// <summary>雲林縣</summary>
    [Description("雲林縣")]
    Yunlin = 11,

    /// <summary>嘉義縣</summary>
    [Description("嘉義縣")]
    ChiayiCounty = 12,

    /// <summary>嘉義市</summary>
    [Description("嘉義市")]
    ChiayiCity = 13,

    /// <summary>臺南市</summary>
    [Description("臺南市")]
    Tainan = 14,

    /// <summary>高雄市</summary>
    [Description("高雄市")]
    Kaohsiung = 15,

    /// <summary>屏東縣</summary>
    [Description("屏東縣")]
    Pingtung = 16,

    /// <summary>臺東縣</summary>
    [Description("臺東縣")]
    Taitung = 17,

    /// <summary>花蓮縣</summary>
    [Description("花蓮縣")]
    Hualien = 18,

    /// <summary>宜蘭縣</summary>
    [Description("宜蘭縣")]
    Yilan = 19,

    /// <summary>澎湖縣</summary>
    [Description("澎湖縣")]
    Penghu = 20,

    /// <summary>金門縣</summary>
    [Description("金門縣")]
    Kinmen = 21,

    /// <summary>連江縣</summary>
    [Description("連江縣")]
    Lienchiang = 22
}