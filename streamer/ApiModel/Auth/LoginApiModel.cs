namespace streamer.ApiModel.Auth
{
    public class LoginApiModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? RememberMe { get; set; }
    }
}
