using System;
using Microsoft.Net.Http.Headers;

namespace Xtensible.StraightRazor.Core
{
	public class RazorRendererOptions
	{
		public string ScriptRoot { get; set; } = "Assets/Scripts";
		public string ImageRoot { get; set; } = "Assets/Images";
		public string PageRoot { get; set; } = "Assets/Pages";
		public string StyleRoot { get; set; } = "Assets/Styles";

		public ResponseOptions DefaultPageResponseOptions { get; set; } = new ResponseOptions
		{
			CacheHeader = new CacheControlHeaderValue
			{
				MaxAge = TimeSpan.FromHours(1),
				Public = true
			}
		};

		public ResponseOptions DefaultStaticFileResponseOptions { get; set; } = new ResponseOptions
		{
			CacheHeader = new CacheControlHeaderValue
			{
				MaxAge = TimeSpan.FromDays(365 * 20),
				Public = true
			}
		};
	}
}