using accomondationApp.Models;
using Newtonsoft.Json;
using System.Text.Json;


namespace accomondationApp.Utilities
{
    public static class DBSettingProvider
    {
        private const string fileName = "dbsetting.json";
        public static void WriteDBSetting()
        {
            var user = new DBConnectionSetting
            {
                UserId = "aG90ZWxkYnVzZXI=",
                Password = "UEVwYW1hMTI=",
                Database = "aG90ZWxBcHBEQg==",
                Server = "bG9jYWxob3N0"                
            };
            
            string jsonString = System.Text.Json.JsonSerializer.Serialize(user);
            File.WriteAllText(fileName, jsonString);
        }

        public static DBConnectionSetting GetDBSetting()
        {
            if (File.Exists(fileName))
            {
                using var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                using var reader = new StreamReader(fileStream);
                var fileContent = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<DBConnectionSetting>(fileContent);
            }
            else
            {
                WriteDBSetting();
                return GetDBSetting();
            }
        }

        public static string ReturnConnectionString()
        {
            var dbSetting = DBSettingProvider.GetDBSetting();
            return $"Data Source = {EncryptDecrypt.DecodeFrom64(dbSetting.Server)}; Initial Catalog = {EncryptDecrypt.DecodeFrom64(dbSetting.Database)}; user id = {EncryptDecrypt.DecodeFrom64(dbSetting.UserId)}; password = {EncryptDecrypt.DecodeFrom64(dbSetting.Password)}; TrustServerCertificate = True;";
        }
    }
}