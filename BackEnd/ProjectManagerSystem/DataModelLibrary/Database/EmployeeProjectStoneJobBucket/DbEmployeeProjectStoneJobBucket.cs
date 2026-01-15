using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary.Database.EmployeeProjectStoneJobBucket;

/// <summary>資料庫-員工-專案里程碑工項清單-常數</summary>
public static class DbEmployeeProjectStoneJobBucket
{
    /// <summary>預設</summary>
    public static class Default
    {
        /// <summary>名稱</summary>
        public static string Name { get; } = "是否完成";

        /// <summary>是否完成</summary>
        public static bool IsFinish { get; } = false;
    }
}
