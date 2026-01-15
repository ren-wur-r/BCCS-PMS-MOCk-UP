namespace DataModelLibrary.GlobalSystem.StaticConst;

/// <summary>檔案常數</summary>
public static class CasinoFileConst
{
    // db
    // 相對路徑 RelativePath     Server/Core/Dealer/Info
    // 相對檔名 RelativeFileName Server/Core/Dealer/Info/aaa.png?m={dateCode}
    // 絕對檔名 X

    // local
    // 相對路徑 RelativePath     Server/Core/Dealer/Info
    // 相對檔名 RelativeFileName Server/Core/Dealer/Info/aaa.png?m={dateCode}
    // 絕對檔名 AbsoluteFileName C://Server/Core/Dealer/Info/aaa.png?m={dateCode}

    // remote
    // 相對路徑 RelativePath     Server/Core/Dealer/Info
    // 相對檔名 RelativeFileName Server/Core/Dealer/Info/aaa.png?m={dateCode}
    // 絕對檔名 AbsoluteFileName http://yahoo.com.tw/Server/Core/Dealer/Info/aaa.png?m={dateCode}

    ///// <summary>基底資料夾</summary>
    //public static string BASE_FOLDER { get; } = AppContext.BaseDirectory;

    /// <summary>基底資料夾</summary>
    public static string BASE_FILE_FOLDER { get; } = "Upload";

    /// <summary>公司模板</summary>
    public static class CompanyTemplate
    {
        /// <summary>Email本地相對資料夾</summary>
        public static string EMAIL_LOCAL_RELATIVE_FOLDER
        {
            get
            {
                return BASE_FILE_FOLDER + "/CompanyTemplate/Email";
            }
        }
    }

    /// <summary>公司專案測驗</summary>
    public static class CompanyProjectQuiz
    {
        /// <summary>Email本地相對資料夾</summary>
        public static string EMAIL_LOCAL_RELATIVE_FOLDER
        {
            get
            {
                return BASE_FILE_FOLDER + "/CompanyProjectQuiz/Email";
            }
        }
    }
}
