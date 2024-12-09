using accomondationApp.Models;
using Newtonsoft.Json;
using System.Text.Json;


namespace accomondationApp.Utilities
{
    public static class DBSettingProvider
    {       
        private static Dictionary<string, DBConnectionSetting> fileNames = new Dictionary<string, DBConnectionSetting>()
        {
            {"hotelDBSetting",new DBConnectionSetting()
                {
                    UserId = "aG90ZWxkYnVzZXI=",
                    Password = "UEVwYW1hMTI=",
                    Database = "aG90ZWxBcHBEQg==",
                    Server = "bG9jYWxob3N0",
                    FileName = "dbsetting.json"
                } 
            },
            {"userDBSetting",
                new DBConnectionSetting()
                {
                    UserId = "aG90ZWxkYnVzZXI=",
                    Password = "UEVwYW1hMTI=",
                    Database = "VXNlckRC",
                    Server = "bG9jYWxob3N0",
                    FileName = "userdbsetting.json"
                }
            }
        };
        public static void WriteDBSetting(string dbName)
        {           
            string jsonString = System.Text.Json.JsonSerializer.Serialize(fileNames[dbName]);
            File.WriteAllText(fileNames[dbName].FileName, jsonString);
        }

        public static DBConnectionSetting GetDBSetting(string dbName)
        {
            if (File.Exists(fileNames[dbName].FileName))
            {
                using var fileStream = new FileStream(fileNames[dbName].FileName, FileMode.Open, FileAccess.Read);
                using var reader = new StreamReader(fileStream);
                var fileContent = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<DBConnectionSetting>(fileContent);
            }
            else
            {
                WriteDBSetting(dbName);
                return GetDBSetting(dbName);
            }
        }
        public static string ReturnDBConnectionString(string dbName)
        {
            var dbSetting = DBSettingProvider.GetDBSetting(dbName);          
            return $"Data Source = {EncryptDecrypt.DecodeFrom64(dbSetting.Server)}; Initial Catalog = {EncryptDecrypt.DecodeFrom64(dbSetting.Database)}; user id = {EncryptDecrypt.DecodeFrom64(dbSetting.UserId)}; password = {EncryptDecrypt.DecodeFrom64(dbSetting.Password)}; TrustServerCertificate = True;MultipleActiveResultSets=True;";
        }       
    }
}