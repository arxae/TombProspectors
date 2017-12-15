namespace TombProspectors.Controllers.Models
{
	public class NewUserRegistrationModel
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string PasswordConfirm { get; set; }
		public string Email { get; set; }
	}
}