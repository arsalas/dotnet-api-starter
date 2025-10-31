namespace ApiStarter.Constants;

public static class Messages
{
	public static class Auth
	{
		public const string InvalidCredentials = "Credenciales inválidas";
		public const string UserLocked = "Usuario bloqueado temporalmente";
		public const string RegistrationSuccess = "Usuario registrado exitosamente";
	}

	public static class Users
	{
		public const string NotFound = "Usuario no encontrado";
		public const string EmailExists = "El email ya está registrado";
		public const string UpdateSuccess = "Usuario actualizado correctamente";
		public const string DeleteSuccess = "Usuario eliminado correctamente";
	}

	public static class Courses
	{
		public const string NotFound = "Curso no encontrado";
		public const string AccessDenied = "No tienes acceso a este curso";
		public const string EnrollmentSuccess = "Inscripción realizada correctamente";
	}

	public static class Validation
	{
		public const string EmailRequired = "El email es requerido";
		public const string EmailFormat = "Email inválido";
		public const string PasswordRequired = "La contraseña es requerida";
		public const string PasswordTooShort = "La contraseña debe tener al menos 8 caracteres";
		public const string PasswordExpression = "Debe tener mayúscula, minúscula y número";
	}
}