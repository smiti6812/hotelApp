namespace accomondationApp.Models
{
    public class DBConnectionSetting
    {
        public required string UserId { get; set; }
        public required string Password { get; set; }
        public required string Server { get; set; }
        public required string Database { get; set; }        
        public required string FileName { get; set; }
    }
}
