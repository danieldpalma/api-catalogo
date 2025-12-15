using System.ComponentModel.DataAnnotations;

namespace ApiCatalogo.DTOs;

public class LoginModel
{
	[Required(ErrorMessage = "Username is required.")]
	public string? Name { get; set; }
	[Required(ErrorMessage = "Password is required.")]
	public string? Password { get; set; }
}
