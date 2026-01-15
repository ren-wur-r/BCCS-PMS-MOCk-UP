using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnFolderFile.Format;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.CmnFolderFile.Service;

/// <summary>通用-資料夾檔案-服務</summary>
public class CmnFolderFileService : ICmnFolderFileService
{
    /// <summary>logger</summary>
    private readonly ILogger<CmnFolderFileService> _logger;

    /// <summary>建構式</summary>
    public CmnFolderFileService(
        ILogger<CmnFolderFileService> logger)
    {
        this._logger = logger;
    }

    /// <summary>取得檔案類型</summary>
    public string GetContentType(string fileName)
    {
        var provider = new FileExtensionContentTypeProvider();

        if (!provider.TryGetContentType(fileName, out var contentType))
        {
            contentType = "application/octet-stream";
        }
        return contentType;
    }

    /// <summary>通用-資料夾檔案-合併路徑</summary>
    public CmnFolderFileCombinePathRspMdl CombinePath(CmnFolderFileCombinePathReqMdl reqObj)
    {
        try
        {
            var resultPath = Path.Combine(reqObj.PathList.ToArray())
                .Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            var rspObj = new CmnFolderFileCombinePathRspMdl
            {
                ResultPath = resultPath,
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
    }

    /// <summary>通用-資料夾檔案-建立檔案</summary>
    public async Task<CmnFolderFileCreateFileRspMdl> CreateFileAsync(CmnFolderFileCreateFileReqMdl reqObj)
    {
        FileStream stream = null;
        try
        {
            // 建立檔案路徑實體
            var file = new FileInfo(reqObj.LocalAbsoluteFileName);

            // 建立實體目錄路徑
            if (!Directory.Exists(file.DirectoryName))
            {
                _ = Directory.CreateDirectory(file.DirectoryName);
            }

            stream = file.Create();

            var rspObj = new CmnFolderFileCreateFileRspMdl
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
            await stream?.FlushAsync();
            stream?.Dispose();
        }
    }

    /// <summary>通用-資料夾檔案-儲存檔案</summary>
    public async Task<CmnFolderFileSaveFileRspMdl> SaveFileAsync(CmnFolderFileSaveFileReqMdl reqObj)
    {
        FileStream stream = null;
        try
        {
            var combinePathRspObj = this.CombinePath(new CmnFolderFileCombinePathReqMdl()
            {
                PathList = new List<string>()
                {
                    reqObj.BasePath,
                    reqObj.RelativeFilePath
                }
            });
            if (combinePathRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var file = new FileInfo(combinePathRspObj.ResultPath);

            // 建立實體目錄路徑
            _ = Directory.CreateDirectory(file.DirectoryName);

            stream = file.Create();

            await reqObj.FormFile.CopyToAsync(stream);

            var rspObj = new CmnFolderFileSaveFileRspMdl
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
            await stream?.FlushAsync();
            stream?.Dispose();
        }
    }

    /// <summary>通用-資料夾檔案-刪除檔案</summary>
    public CmnFolderFileDeleteFileRspMdl DeleteFile(CmnFolderFileDeleteFileReqMdl reqObj)
    {
        if (File.Exists(reqObj.LocalAbsoluteFileName) == false)
        {
            var notExistRspObj = new CmnFolderFileDeleteFileRspMdl
            {
                IsSuccess = true,
            };
            return notExistRspObj;
        }

        try
        {
            File.Delete(reqObj.LocalAbsoluteFileName);

            var rspObj = new CmnFolderFileDeleteFileRspMdl
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
    }

    /// <summary>通用-資料夾檔案-讀取檔案</summary>
    public CmnFolderFileReadFileRspMdl ReadFile(CmnFolderFileReadFileReqMdl reqObj)
    {
        // 確保路徑不為空
        if (string.IsNullOrWhiteSpace(reqObj.LocalAbsoluteFileName))
        {
            this._logger.LogWarning($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        FileStream fileStream = null;
        MemoryStream memoryStream = null;
        try
        {
            var file = new FileInfo(reqObj.LocalAbsoluteFileName);

            // 檢查檔案是否存在
            if (file.Exists == false)
            {
                this._logger.LogWarning($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var fileBytes = File.ReadAllBytes(reqObj.LocalAbsoluteFileName);

            var rspObj = new CmnFolderFileReadFileRspMdl
            {
                FileByteList = fileBytes,
                ContentType = this.GetContentType(file.Extension),
                FileName = file.Name
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
            fileStream?.Dispose();
            memoryStream?.Dispose();
        }
    }

    /// <summary>通用-資料夾檔案-驗證檔案</summary>
    public CmnFolderFileValidateFileRspMdl ValidateFile(CmnFolderFileValidateFileReqMdl reqObj)
    {
        var rspObj = new CmnFolderFileValidateFileRspMdl()
        {
            IsValid = false,
        };

        if (reqObj.AllowSize != default
            && reqObj.FormFile.Length > reqObj.AllowSize.Value)
        {
            return rspObj;
        }

        // 取得副檔名
        var reqFileExtension = Path.GetExtension(reqObj.FormFile.FileName).ToLowerInvariant();

        if (reqObj.AllowExtensionList != default
            && reqObj.AllowExtensionList.Contains(reqFileExtension) == false)
        {
            return rspObj;
        }

        rspObj.IsValid = true;
        return rspObj;
    }

    /// <summary>通用-資料夾檔案-刪除資料夾</summary>
    public CmnFolderFileDeleteFolderRspMdl DeleteFolder(CmnFolderFileDeleteFolderReqMdl reqObj)
    {
        try
        {
            var absoluteFolder = Path.Combine(reqObj.BaseFolder, reqObj.RelativeFolder);

            // 不存在就出去
            if (Directory.Exists(absoluteFolder) == false)
            {
                return new CmnFolderFileDeleteFolderRspMdl
                {
                    IsSeccess = true
                };
            }

            // 刪除目錄
            Directory.Delete(absoluteFolder, true);

            var rspObj = new CmnFolderFileDeleteFolderRspMdl
            {
                IsSeccess = true
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
    }

    /// <summary>通用-資料夾檔案-轉換Base64格式</summary>
    public async Task<CmnFolderFileTryParseBase64ImageRspMdl> TryParseBase64ImageAsync(CmnFolderFileTryParseBase64ImageReqMdl reqObj)
    {
        MemoryStream memoryStream = null;
        try
        {
            memoryStream = new MemoryStream();
            await reqObj.FormFile.CopyToAsync(memoryStream);

            var base64Image = Convert.ToBase64String(memoryStream.ToArray());
            var rspObj = new CmnFolderFileTryParseBase64ImageRspMdl
            {
                Base64Image = base64Image,
                DataUriImage = $"data:{reqObj.FormFile.ContentType};base64,{base64Image}",
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
            if (memoryStream != null)
            {
                memoryStream.Dispose();
            }
        }
    }

    /// <summary>通用-資料夾檔案-儲存檔案 from Byte list</summary>
    public async Task<CmnFolderFileSaveFileFromByteListRspMdl> SaveFileFromByteListAsync(CmnFolderFileSaveFileFromByteListReqMdl reqObj)
    {
        try
        {
            var combinePathRspObj = this.CombinePath(new CmnFolderFileCombinePathReqMdl()
            {
                PathList = new List<string>()
                {
                    reqObj.BasePath,
                    reqObj.RelativeFilePath
                }
            });
            if (combinePathRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var file = new FileInfo(combinePathRspObj.ResultPath);

            // 建立實體目錄路徑
            _ = Directory.CreateDirectory(file.DirectoryName);

            // 直接寫入檔案
            await File.WriteAllBytesAsync(combinePathRspObj.ResultPath, reqObj.FileByteList);

            var rspObj = new CmnFolderFileSaveFileFromByteListRspMdl
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
    }
}
