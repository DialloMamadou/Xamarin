using Newtonsoft.Json;

namespace fourplaces.Models
{
    public class LoginRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("password")]
        public string Password { get; set; }

        /*public LoginRequest()
        {
        }

            public LoginRequest(LoginRequest User)
        {
            Email = User.Email;
            Password = User.Password;
        }*/
        public LoginRequest(string email, string password)
         {
             Email = email;
             Password = password;
         }
    }
}