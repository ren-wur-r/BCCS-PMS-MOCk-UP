using System;
using System.Data;
using System.IO;
using System.Text.Json;
using CommonLibrary.CmnExcel.Format;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace CommonLibrary.CmnExcel.Service;

/// <summary>通用-Excel-服務</summary>
public class CmnExcelService : ICmnExcelService
{
    /// <summary>副檔名xlsx</summary>
    private const string _EXTENSION_XLSX = ".xlsx";

    /// <summary>副檔名xls</summary>
    private const string _EXTENSION_XLS = ".xls";

    /// <summary>logger</summary>
    private readonly ILogger<CmnExcelService> _logger;

    /// <summary>建構式</summary>
    public CmnExcelService(ILogger<CmnExcelService> logger)
    {
        this._logger = logger;
    }

    #region xlsx

    /// <summary>通用-Excel-建立Xlsx</summary>
    public CmnExcelCreateXlsxRspMdl CreateXlsx(CmnExcelCreateXlsxReqMdl reqObj)
    {
        // 判斷檔案是否存在
        if (File.Exists(reqObj.AbsoluteFileName))
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        // 判斷副檔名
        var extension = Path.GetExtension(reqObj.AbsoluteFileName).ToLower();
        if (extension != CmnExcelService._EXTENSION_XLSX)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        FileStream writeFileStream = null;
        XSSFWorkbook workbook = null;
        try
        {
            workbook = new XSSFWorkbook();

            // 若沒有工作表則新增
            if (workbook.NumberOfSheets == 0)
            {
                workbook.CreateSheet("Sheet1");
            }

            // 取得第一個工作表
            var sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            // 判斷是否有資料
            if (reqObj.DataTable == null
                || reqObj.DataTable.Rows.Count == 0)
            {
                // 無資料，回傳成功

                writeFileStream = File.Open(reqObj.AbsoluteFileName, FileMode.Create, FileAccess.Write);
                workbook.Write(writeFileStream, true);
                workbook.Close();

                var etmpryRspObj = new CmnExcelCreateXlsxRspMdl()
                {
                    IsSuccess = true,
                };
                return etmpryRspObj;
            }

            var rowIndex = 0;
            foreach (DataRow rowItem in reqObj.DataTable.Rows)
            {
                var row = sheet.CreateRow(rowIndex);

                var colIndex = 0;
                foreach (var cellItem in rowItem.ItemArray)
                {
                    var cell = row.CreateCell(colIndex);
                    cell.SetCellValue(cellItem?.ToString() ?? string.Empty);
                    colIndex++;
                }

                rowIndex++;
            }

            //for (var indexTableRow = 0; indexTableRow < reqObj.DataTable.Rows.Count; indexTableRow++)
            //{
            //    var row = sheet.CreateRow(indexTableRow);

            //    for (var indexTableColumn = 0; indexTableColumn < reqObj.DataTable.Columns.Count; indexTableColumn++)
            //    {
            //        var cell = row.CreateCell(indexTableColumn);
            //        cell.SetCellValue(reqObj.DataTable.Rows[indexTableRow][indexTableColumn]?.ToString() ?? string.Empty);
            //    }
            //}

            writeFileStream = File.Open(reqObj.AbsoluteFileName, FileMode.Create, FileAccess.Write);
            workbook.Write(writeFileStream, true);
            workbook.Close();

            var rspObj = new CmnExcelCreateXlsxRspMdl()
            {
                IsSuccess = true,
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
        finally
        {
            workbook?.Dispose();
            writeFileStream?.Close();
            writeFileStream?.Dispose();
        }
    }

    /// <summary>通用-Excel-附加Xlsx</summary>
    public CmnExcelAppendXlsxRspMdl AppendXlsx(CmnExcelAppendXlsxReqMdl reqObj)
    {
        // 判斷檔案是否存在
        if (File.Exists(reqObj.AbsoluteFileName) == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        // 判斷副檔名
        var extension = Path.GetExtension(reqObj.AbsoluteFileName).ToLower();
        if (extension != CmnExcelService._EXTENSION_XLSX)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        FileStream readFileStream = null;
        FileStream writeFileStream = null;
        XSSFWorkbook workbook = null;
        try
        {
            readFileStream = new FileStream(reqObj.AbsoluteFileName, FileMode.Open, FileAccess.Read);
            workbook = new XSSFWorkbook(readFileStream);

            // 若沒有工作表則新增
            if (workbook.NumberOfSheets == 0)
            {
                workbook.CreateSheet("Sheet1");
            }

            // 取得第一個工作表
            var sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            // 判斷是否有資料
            if (reqObj.DataTable == null
                || reqObj.DataTable.Rows.Count == 0)
            {
                // 無資料，回傳成功

                writeFileStream = new FileStream(reqObj.AbsoluteFileName, FileMode.Create, FileAccess.Write);
                workbook.Write(writeFileStream, true);
                workbook.Close();

                var etmpryRspObj = new CmnExcelAppendXlsxRspMdl()
                {
                    IsSuccess = true,
                };
                return etmpryRspObj;
            }

            var rowIndex = sheet.LastRowNum + 1;
            foreach (DataRow rowItem in reqObj.DataTable.Rows)
            {
                var row = sheet.CreateRow(rowIndex);

                var colIndex = 0;
                foreach (var cellItem in rowItem.ItemArray)
                {
                    var cell = row.CreateCell(colIndex);
                    cell.SetCellValue(cellItem?.ToString() ?? string.Empty);
                    colIndex++;
                }

                rowIndex++;
            }

            //int lastStartRow = sheet.LastRowNum + 1;
            //for (int indexTableRow = 0; indexTableRow < reqObj.DataTable.Rows.Count; indexTableRow++)
            //{
            //    var row = sheet.CreateRow(lastStartRow + indexTableRow);
            //    for (int indexTableColumn = 0; indexTableColumn < reqObj.DataTable.Columns.Count; indexTableColumn++)
            //    {
            //        var cell = row.CreateCell(indexTableColumn);
            //        cell.SetCellValue(reqObj.DataTable.Rows[indexTableRow][indexTableColumn]?.ToString() ?? string.Empty);
            //    }
            //}

            writeFileStream = new FileStream(reqObj.AbsoluteFileName, FileMode.Create, FileAccess.Write);
            workbook.Write(writeFileStream, true);
            workbook.Close();

            var rspObj = new CmnExcelAppendXlsxRspMdl()
            {
                IsSuccess = true,
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
        finally
        {
            workbook?.Dispose();
            readFileStream?.Close();
            readFileStream?.Dispose();
            writeFileStream?.Close();
            writeFileStream?.Dispose();
        }
    }

    /// <summary>通用-Excel-讀取Xlsx依檔案</summary>
    public CmnExcelReadXlsxByFileRspMdl ReadXlsxByFile(CmnExcelReadXlsxByFileReqMdl reqObj)
    {
        // 判斷檔案是否存在
        if (File.Exists(reqObj.AbsoluteFileName) == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        // 判斷副檔名
        var extension = Path.GetExtension(reqObj.AbsoluteFileName).ToLower();
        if (extension != CmnExcelService._EXTENSION_XLSX)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        FileStream readStream = null;
        XSSFWorkbook workbook = null;
        try
        {
            readStream = new FileStream(reqObj.AbsoluteFileName, FileMode.Open, FileAccess.Read);
            workbook = new XSSFWorkbook(readStream);

            // 取得工作表
            var sheet = workbook.NumberOfSheets > 0 ? workbook.GetSheetAt(0) : null;
            if (sheet == null)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            // 檢查工作表是否有資料
            if (sheet.PhysicalNumberOfRows == 0)
            {
                // 沒資料，回傳空資料表

                var emptyRspObj = new CmnExcelReadXlsxByFileRspMdl
                {
                    DataTable = new DataTable(),
                };
                return emptyRspObj;
            }

            var maxColumn = sheet.GetRow(0).Cells.Count;

            // 定義 dataTable 
            var dataTable = new DataTable();

            // 初始化 DataTable 的欄位 (Columns)
            for (var columnIndex = 0; columnIndex < maxColumn; columnIndex++)
            {
                dataTable.Columns.Add($"Column{columnIndex}");
            }

            // 讀取資料
            for (var rowIndex = 0; rowIndex < sheet.PhysicalNumberOfRows; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                var dataRow = dataTable.NewRow();

                for (var columnIndex = 0; columnIndex < maxColumn; columnIndex++)
                {
                    dataRow[columnIndex] = row.GetCell(columnIndex)?.ToString() ?? string.Empty;
                }
                dataTable.Rows.Add(dataRow);
            }

            workbook.Close();

            var rspObj = new CmnExcelReadXlsxByFileRspMdl
            {
                DataTable = dataTable
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
        finally
        {
            workbook?.Dispose();
            readStream?.Close();
            readStream?.Dispose();
        }
    }

    /// <summary>通用-Excel-讀取Xlsx依IFormFile</summary>
    public CmnExcelReadXlsxByIFormFilRspMdl ReadXlsxByIFormFile(CmnExcelReadXlsxByIFormFilReqMdl reqObj)
    {
        // 判斷檔案是否存在
        if (reqObj.FormFile == null)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        // 判斷副檔名
        var extension = Path.GetExtension(reqObj.FormFile.FileName).ToLower();
        if (extension != CmnExcelService._EXTENSION_XLSX)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        Stream readStream = null;
        XSSFWorkbook workbook = null;
        try
        {
            readStream = reqObj.FormFile.OpenReadStream();
            workbook = new XSSFWorkbook(readStream);

            // 取得工作表
            var sheet = workbook.NumberOfSheets > 0 ? workbook.GetSheetAt(0) : null;
            if (sheet == null)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            // 檢查工作表是否有資料
            if (sheet.PhysicalNumberOfRows == 0)
            {
                // 沒資料，回傳空資料表

                var emptyRspObj = new CmnExcelReadXlsxByIFormFilRspMdl
                {
                    DataTable = new DataTable(),
                };
                return emptyRspObj;
            }

            var maxColumn = sheet.GetRow(0).Cells.Count;

            // 定義 dataTable 
            var dataTable = new DataTable();

            // 初始化 DataTable 的欄位 (Columns)
            for (var columnIndex = 0; columnIndex < maxColumn; columnIndex++)
            {
                dataTable.Columns.Add($"Column{columnIndex}");
            }

            // 讀取資料
            for (var rowIndex = 0; rowIndex < sheet.PhysicalNumberOfRows; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                var dataRow = dataTable.NewRow();

                for (var columnIndex = 0; columnIndex < maxColumn; columnIndex++)
                {
                    dataRow[columnIndex] = row?.GetCell(columnIndex)?.ToString() ?? string.Empty;
                }
                dataTable.Rows.Add(dataRow);
            }

            workbook.Close();

            var rspObj = new CmnExcelReadXlsxByIFormFilRspMdl
            {
                DataTable = dataTable
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
        finally
        {
            workbook?.Dispose();
            readStream?.Close();
            readStream?.Dispose();
        }
    }

    #endregion

    #region xls

    /// <summary>通用-Excel-建立Xls</summary>
    public CmnExcelCreateXlsRspMdl CreateXls(CmnExcelCreateXlsReqMdl reqObj)
    {
        // 判斷檔案是否存在
        if (File.Exists(reqObj.AbsoluteFileName))
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        // 判斷副檔名
        var extension = Path.GetExtension(reqObj.AbsoluteFileName).ToLower();
        if (extension != CmnExcelService._EXTENSION_XLS)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        FileStream writeFileStream = null;
        HSSFWorkbook workbook = null;
        try
        {
            workbook = new HSSFWorkbook();

            // 若沒有工作表則新增
            if (workbook.NumberOfSheets == 0)
            {
                workbook.CreateSheet("Sheet1");
            }

            // 取得第一個工作表
            var sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            // 判斷是否有資料
            if (reqObj.DataTable == null
                || reqObj.DataTable.Rows.Count == 0)
            {
                // 無資料，回傳成功

                writeFileStream = File.Open(reqObj.AbsoluteFileName, FileMode.Create, FileAccess.Write);
                workbook.Write(writeFileStream, true);
                workbook.Close();

                var etmpryRspObj = new CmnExcelCreateXlsRspMdl()
                {
                    IsSuccess = true,
                };
                return etmpryRspObj;
            }

            var rowIndex = 0;
            foreach (DataRow rowItem in reqObj.DataTable.Rows)
            {
                var row = sheet.CreateRow(rowIndex);

                var colIndex = 0;
                foreach (var cellItem in rowItem.ItemArray)
                {
                    var cell = row.CreateCell(colIndex);
                    cell.SetCellValue(cellItem?.ToString() ?? string.Empty);
                    colIndex++;
                }

                rowIndex++;
            }

            //for (var indexTableRow = 0; indexTableRow < reqObj.DataTable.Rows.Count; indexTableRow++)
            //{
            //    var row = sheet.CreateRow(indexTableRow);

            //    for (var indexTableColumn = 0; indexTableColumn < reqObj.DataTable.Columns.Count; indexTableColumn++)
            //    {
            //        var cell = row.CreateCell(indexTableColumn);
            //        cell.SetCellValue(reqObj.DataTable.Rows[indexTableRow][indexTableColumn]?.ToString() ?? string.Empty);
            //    }
            //}

            writeFileStream = File.Open(reqObj.AbsoluteFileName, FileMode.Create, FileAccess.Write);
            workbook.Write(writeFileStream);
            workbook.Close();

            var rspObj = new CmnExcelCreateXlsRspMdl()
            {
                IsSuccess = true,
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
        finally
        {
            workbook?.Dispose();
            writeFileStream?.Close();
            writeFileStream?.Dispose();
        }
    }

    /// <summary>通用-Excel-附加Xls</summary>
    public CmnExcelAppendXlsRspMdl AppendXls(CmnExcelAppendXlsReqMdl reqObj)
    {
        // 判斷檔案是否存在
        if (File.Exists(reqObj.AbsoluteFileName) == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        // 判斷副檔名
        var extension = Path.GetExtension(reqObj.AbsoluteFileName).ToLower();
        if (extension != CmnExcelService._EXTENSION_XLS)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        FileStream readFileStream = null;
        FileStream writeFileStream = null;
        HSSFWorkbook workbook = null;
        try
        {
            readFileStream = new FileStream(reqObj.AbsoluteFileName, FileMode.Open, FileAccess.Read);
            workbook = new HSSFWorkbook(readFileStream);

            // 若沒有工作表則新增
            if (workbook.NumberOfSheets == 0)
            {
                workbook.CreateSheet("Sheet1");
            }

            // 取得第一個工作表
            var sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            // 判斷是否有資料
            if (reqObj.DataTable == null
                || reqObj.DataTable.Rows.Count == 0)
            {
                // 無資料，回傳成功

                writeFileStream = new FileStream(reqObj.AbsoluteFileName, FileMode.Create, FileAccess.Write);
                workbook.Write(writeFileStream, true);
                workbook.Close();

                var etmpryRspObj = new CmnExcelAppendXlsRspMdl()
                {
                    IsSuccess = true,
                };
                return etmpryRspObj;
            }

            var rowIndex = sheet.LastRowNum + 1;
            foreach (DataRow rowItem in reqObj.DataTable.Rows)
            {
                var row = sheet.CreateRow(rowIndex);

                var colIndex = 0;
                foreach (var cellItem in rowItem.ItemArray)
                {
                    var cell = row.CreateCell(colIndex);
                    cell.SetCellValue(cellItem?.ToString() ?? string.Empty);
                    colIndex++;
                }

                rowIndex++;
            }

            var lastStartRow = sheet.LastRowNum + 1;
            for (var indexTableRow = 0; indexTableRow < reqObj.DataTable.Rows.Count; indexTableRow++)
            {
                var row = sheet.CreateRow(lastStartRow + indexTableRow);
                for (var indexTableColumn = 0; indexTableColumn < reqObj.DataTable.Columns.Count; indexTableColumn++)
                {
                    var cell = row.CreateCell(indexTableColumn);
                    cell.SetCellValue(reqObj.DataTable.Rows[indexTableRow][indexTableColumn]?.ToString() ?? string.Empty);
                }
            }

            writeFileStream = new FileStream(reqObj.AbsoluteFileName, FileMode.Create, FileAccess.Write);
            workbook.Write(writeFileStream);
            workbook.Close();

            var rspObj = new CmnExcelAppendXlsRspMdl()
            {
                IsSuccess = true,
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
        finally
        {
            workbook?.Dispose();
            readFileStream?.Close();
            readFileStream?.Dispose();
            writeFileStream?.Close();
            writeFileStream?.Dispose();
        }
    }

    /// <summary>通用-Excel-讀取Xls依檔案</summary>
    public CmnExcelReadXlsByFileRspMdl ReadXlsByFile(CmnExcelReadXlsByFileReqMdl reqObj)
    {
        // 判斷檔案是否存在
        if (File.Exists(reqObj.AbsoluteFileName) == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        // 判斷副檔名
        var extension = Path.GetExtension(reqObj.AbsoluteFileName).ToLower();
        if (extension != CmnExcelService._EXTENSION_XLS)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        FileStream readStream = null;
        HSSFWorkbook workbook = null;
        try
        {
            readStream = new FileStream(reqObj.AbsoluteFileName, FileMode.Open, FileAccess.Read);
            workbook = new HSSFWorkbook(readStream);

            // 取得工作表
            var sheet = workbook.NumberOfSheets > 0 ? workbook.GetSheetAt(0) : null;
            if (sheet == null)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            // 檢查工作表是否有資料
            if (sheet.PhysicalNumberOfRows == 0)
            {
                // 沒資料，回傳空資料表
                var emptyRspObj = new CmnExcelReadXlsByFileRspMdl
                {
                    DataTable = new DataTable(),
                };
                return emptyRspObj;
            }

            var maxColumn = sheet.GetRow(0).Cells.Count;

            // 定義 dataTable 
            var dataTable = new DataTable();

            // 初始化 DataTable 的欄位 (Columns)
            for (var columnIndex = 0; columnIndex < maxColumn; columnIndex++)
            {
                dataTable.Columns.Add($"Column{columnIndex}");
            }

            // 讀取資料
            for (var rowIndex = 0; rowIndex < sheet.PhysicalNumberOfRows; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                var dataRow = dataTable.NewRow();

                for (var columnIndex = 0; columnIndex < maxColumn; columnIndex++)
                {
                    dataRow[columnIndex] = row?.GetCell(columnIndex)?.ToString() ?? string.Empty;
                }
                dataTable.Rows.Add(dataRow);
            }

            workbook.Close();

            var rspObj = new CmnExcelReadXlsByFileRspMdl
            {
                DataTable = dataTable
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
        finally
        {
            workbook?.Dispose();
            readStream?.Close();
            readStream?.Dispose();
        }
    }

    /// <summary>通用-Excel-讀取Xls依IFormFile</summary>
    public CmnExcelReadXlsByIFormFilRspMdl ReadXlsByIFormFile(CmnExcelReadXlsByIFormFilReqMdl reqObj)
    {
        // 判斷檔案是否存在
        if (reqObj.FormFile == null)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        // 判斷副檔名
        var extension = Path.GetExtension(reqObj.FormFile.FileName).ToLower();
        if (extension != CmnExcelService._EXTENSION_XLS)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        Stream readStream = null;
        HSSFWorkbook workbook = null;
        try
        {
            readStream = reqObj.FormFile.OpenReadStream();
            workbook = new HSSFWorkbook(readStream);

            // 取得工作表
            var sheet = workbook.NumberOfSheets > 0 ? workbook.GetSheetAt(0) : null;
            if (sheet == null)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            // 檢查工作表是否有資料
            if (sheet.PhysicalNumberOfRows == 0)
            {
                // 沒資料，回傳空資料表
                var emptyRspObj = new CmnExcelReadXlsByIFormFilRspMdl
                {
                    DataTable = new DataTable(),
                };
                return emptyRspObj;
            }

            var maxColumn = sheet.GetRow(0).Cells.Count;

            // 定義 dataTable 
            var dataTable = new DataTable();

            // 初始化 DataTable 的欄位 (Columns)
            for (var columnIndex = 0; columnIndex < maxColumn; columnIndex++)
            {
                dataTable.Columns.Add($"Column{columnIndex}");
            }

            // 讀取資料
            for (var rowIndex = 0; rowIndex < sheet.PhysicalNumberOfRows; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                var dataRow = dataTable.NewRow();

                for (var columnIndex = 0; columnIndex < maxColumn; columnIndex++)
                {
                    dataRow[columnIndex] = row?.GetCell(columnIndex)?.ToString() ?? string.Empty;
                }
                dataTable.Rows.Add(dataRow);
            }

            workbook.Close();

            var rspObj = new CmnExcelReadXlsByIFormFilRspMdl
            {
                DataTable = dataTable
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
        finally
        {
            workbook?.Dispose();
            readStream?.Close();
            readStream?.Dispose();
        }
    }

    #endregion

}
