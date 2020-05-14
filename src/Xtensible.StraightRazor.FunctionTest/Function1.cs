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
		static RazorRenderer Engine = new RazorRenderer(typeof(Function1).Assembly);
		[FunctionName("Index")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
			HttpRequest req,
			ILogger log)
		{
			return await Engine.ViewAsync("Index", new ViewModel {Name = "Ted"});



		}

		[FunctionName("Image")]
		public static ActionResult Image(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get",
				Route = "img/{*restOfPath}")] 
			HttpRequest req,
			string restOfPath)
		{

			return Engine.Image(restOfPath);
		}

		[FunctionName("Style")]
		public static ActionResult Style(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get",
				Route = "css/{*restOfPath}")]
			HttpRequest req,
			string restOfPath)
		{

			return Engine.Style(restOfPath);
		}

		[FunctionName("Scripts")]
		public static ActionResult Scripts(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get",
				Route = "scripts/{*restOfPath}")]
			HttpRequest req,
			string restOfPath)
		{

			return Engine.Script(restOfPath);
		}

	}
}