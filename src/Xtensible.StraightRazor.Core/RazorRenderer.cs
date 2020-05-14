using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RazorLight;

namespace Xtensible.StraightRazor.Core
{
	public class RazorRenderer
	{

		private RazorLightEngine Engine { get; }
		public RazorRenderer(Assembly operatingAssembly, string pageRoot = "Pages")
		{
			var rootDir = GetApplicationRoot();

			var engine = new RazorLightEngineBuilder()
				.SetOperatingAssembly(operatingAssembly)
				.UseFileSystemProject(Path.Combine(rootDir, pageRoot))
				.UseMemoryCachingProvider()
				.Build();

			Engine = engine;
		}

		public Task<string> RenderAsync<T>(string viewName, T model = null, ExpandoObject viewBag = null) where T : class
		{
			if (!viewName.EndsWith(".cshtml"))
			{
				viewName += ".cshtml";
			}

			return Engine.CompileRenderAsync<T>(viewName, model, viewBag);
		}

		public async Task<ContentResult> ViewAsync<T>(string viewName, T model = null, ExpandoObject viewBag = null)
			where T : class
		{
			var html = await RenderAsync(viewName, model, viewBag).ConfigureAwait(false);
			return new ContentResult
			{
				ContentType = "text/html",
				Content = html,
				StatusCode = StatusCodes.Status200OK
			};
		}

		private string GetApplicationRoot()
		{
			var exePath = Path.GetDirectoryName(System.Reflection
				.Assembly.GetExecutingAssembly().CodeBase);
		
			var path = exePath.Replace("file:\\","");
			path = Path.Combine(path, "..");
			return path;
		}
	}
	
}
