using Microsoft.AspNetCore.Hosting;
using System.Text.Encodings.Web;

namespace MyShop.Web.Services
{
	public class EmailBodyBuilder : IEmailBodyBuilder
	{
		private readonly IWebHostEnvironment _webHostEnvironment;

		public EmailBodyBuilder(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}
        public string GetEmailBody(string header, string body, string url, string linkTitle)
        {
            try
            {
                var filePath = $"{_webHostEnvironment.WebRootPath}/templates/email.html";
                using StreamReader str = new(filePath);
                var template = str.ReadToEnd();
                return template
                    .Replace("[header]", header)
                    .Replace("[body]", body)
                    .Replace("[url]", url)
                    .Replace("[linkTitle]", linkTitle);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading email template: {ex.Message}");
                throw;
            }
        }

    }
}