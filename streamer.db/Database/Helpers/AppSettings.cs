namespace streamer.db.Database.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ServiceAddress { get; set; }
        public string MailjetApiKey { get; set; }
        public string MailjetApiSecret { get; set; }
        public string DbUsername { get; set; }
        public string DbPassword { get; set; }
        public string EmailLogin { get; set; }
        public string EmailPassword { get; set; }
        public string Domain { get; set; }
        public string StripeSecretKey { get; set; }
        public string StripePublicKey { get; set; }
        public string StripeProductId { get; set; } // keeptime.de
        public string StripeTaxId { get; set; } 
        public string Mode { get; set; }

        public string ParseLoginAs { get; set; }
        public string ParseLoginAsPassword { get; set; }
        public string ParseUser { get; set; }
        public string ParsePassword { get; set; }
        public string TeamsWebHookUrl { get; set; }
    }
}
