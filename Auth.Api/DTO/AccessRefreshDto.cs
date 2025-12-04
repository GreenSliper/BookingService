namespace Auth.Api.DTO
{
	public class AccessRefreshDto
	{
		public string AccessToken { get; set; } = string.Empty;
		public string RefreshToken { get; set; } = string.Empty;
	}
}
