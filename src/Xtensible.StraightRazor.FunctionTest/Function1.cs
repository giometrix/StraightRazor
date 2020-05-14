using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xtensible.StraightRazor.Core;
using Xtensible.StraightRazor.FunctionTest.Models;

namespace Xtensible.StraightRazor.FunctionTest
{
	public static class Function1
	{
		[FunctionName("Index")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
			HttpRequest req,
			ILogger log)
		{
			var engine = new RazorRenderer(typeof(Function1).Assembly);
			var html = await engine.RenderAsync("Index", new ViewModel {Name = "Ted"});
			return new ContentResult
			{
				Content = html,
				ContentType = "text/html"
			};



		}

		[FunctionName("WildcardExample")]
		public static async Task<ActionResult> WildcardExample(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get",
				Route = "img/{*restOfPath}")] 
			HttpRequest req,
			string restOfPath)
		{

			
			return new OkObjectResult(new {Route = restOfPath});
		}
}
}