using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class Service
{
    private readonly ILogger<Service> _logger;
    private readonly IConfiguration _configuration;

    public Service(ILogger<Service> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public void FileMove()
    {
        try
        {
            _logger.LogInformation("檔案移動功能 is starting");
            //取得取檔和讀檔路徑
            string getFilePath = _configuration["AppSettings:getFilePath"];
            string targetFilePath = _configuration["AppSettings:targetFilePath"];

            //檔案是否存在

            if (!Directory.Exists(getFilePath))
            {
                _logger.LogWarning($"檔案來源路徑：{getFilePath}資料夾不存在");
            }
            else if (!Directory.Exists(targetFilePath))
            {
                _logger.LogWarning($"檔案複製路徑：{targetFilePath}資料夾不存在");
            }
            else
            {
                //取得路徑中所有檔案
                string[] files = Directory.GetFiles(getFilePath);

                if (files.Length == 0) _logger.LogWarning($"路徑：{getFilePath}資料夾下無檔案可移動");
                else
                {
                    foreach (string file in files)
                    {
                        string fileName = Path.GetFileName(file);
                        string targetFile = targetFilePath + @"\" + fileName;
                        //檔案移動
                        System.IO.File.Move(file, targetFile);
                        _logger.LogInformation($"檔案：{targetFile}複製成功");
                    }
                    foreach (string file in files)
                    {
                        //string fileName = Path.GetFileName(file);
                        //檔案刪除
                        System.IO.File.Delete(file);
                        _logger.LogInformation($"檔案：{file}刪除成功");
                    }

                }
            }
            _logger.LogInformation("檔案移動功能 is Finish");
        }
        catch (Exception ex)
        {
            _logger.LogError($"發生例外錯誤：{ex.Message}");
            throw;
        }

    }
}

