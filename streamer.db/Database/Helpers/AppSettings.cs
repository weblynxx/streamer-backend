namespace streamer.db.Database.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string DbUsername { get; set; }
        public string DbPassword { get; set; }
        public string TwitchClientId { get; set; }
        public string TwitchSecretId { get; set; }
        public string Domain { get; set; }
        public string Mode { get; set; }

    }
}
