namespace MyShop.Web.Services
{
	public interface IEmailBodyBuilder
	{
		string GetEmailBody( string header, string body, string url, string linkTitle);
	}
}