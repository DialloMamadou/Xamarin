using Newtonsoft.Json;

namespace fourplaces.Models
{
	public class RegisterRequest
	{
		[JsonProperty("email")]
		public string Email { get; set; }
		
		[JsonProperty("first_name")]
		public string FirstName { get; set; }
		
		[JsonProperty("last_name")]
		public string LastName { get; set; }
		
		[JsonProperty("password")]
		public string Password { get; set; }

        /*public RegisterRequest()
        {
        }

        public RegisterRequest(RegisterRequest User)
        {
            Email = User.Email;
            FirstName = User.FirstName;
            LastName = User.LastName;
            Password = User.Password;
        }*/
        public RegisterRequest(string email, string fisrtName, string lastName, string mdp)
        {
            Email = email;
            FirstName = fisrtName;
            LastName = lastName;
            Password = mdp;
        }
    }
}