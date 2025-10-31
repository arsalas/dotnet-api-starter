using System.ComponentModel.DataAnnotations;


namespace ApiStarter.DTOs.Auth;

public record LoginDto
{
	[Required(ErrorMessage = Messages.Validation.EmailRequired)]
	[EmailAddress(ErrorMessage = Messages.Validation.EmailFormat)]
	public string Email { get; set; }

	[Required(ErrorMessage = Messages.Validation.PasswordRequired)]
	[MinLength(8, ErrorMessage = Messages.Validation.PasswordTooShort)]
	[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
			ErrorMessage = Messages.Validation.PasswordExpression)]
	public string Password { get; set; }
}