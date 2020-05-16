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
using Microsoft.Net.Http.Headers;
using MimeTypes;
using RazorLight;

namespace Xtensible.StraightRazor.Core
{
	public class RazorRenderer
	{

		private IHttpContextAccessor HttpContextAccessor { get; }
		private RazorLightEngine Engine { get; }
		private string ImageRoot { get; }
		private string StyleRoot { get; }
		private string ScriptRoot { get; }
		private RazorRendererOptions Options { get; }

		public RazorRenderer(Assembly operatingAssembly, IHttpContextAccessor httpContextAccessor, RazorRendererOptions options = null)
		{
			options ??= new RazorRendererOptions();
			Options = options;

			var rootDir = GetApplicationRoot();

			var engine = new RazorLightEngineBuilder()
				.SetOperatingAssembly(operatingAssembly)
				.UseFileSystemProject(Path.Combine(rootDir, options.PageRoot))
				.UseMemoryCachingProvider()
				.Build();

			Engine = engine;
			ImageRoot = Path.Combine(rootDir, options.ImageRoot);
			StyleRoot = Path.Combine(rootDir, options.StyleRoot);
			ScriptRoot = Path.Combine(rootDir, options.ScriptRoot);
			HttpContextAccessor = httpContextAccessor;

		}

		public Task<string> RenderAsync<T>(string viewName, T model = null, ExpandoObject viewBag = null) where T : class
		{
			if (!viewName.EndsWith(".cshtml"))
			{
				viewName += ".cshtml";
			}

			return Engine.CompileRenderAsync<T>(viewName, model, viewBag);
		}

		public async Task<ContentResult> ViewAsync<T>(string viewName, T model = null, ExpandoObject viewBag = null, ResponseOptions responseOptions = null)
			where T : class
		{
			responseOptions ??= Options.DefaultPageResponseOptions;
			HttpContextAccessor.HttpContext.Response.Headers.Add("Cache-Control", responseOptions.CacheHeader.ToString());

			var html = await RenderAsync(viewName, model, viewBag).ConfigureAwait(false);
			return new ContentResult
			{
				ContentType = "text/html",
				Content = html,
				StatusCode = StatusCodes.Status200OK
			};
		}

		public ActionResult Image(string path, ResponseOptions responseOptions = null)
		{
			path = Path.Combine(ImageRoot, path);
			return StaticFile(path, responseOptions);
		}

		public ActionResult Style(string path, ResponseOptions responseOptions = null)
		{
			path = Path.Combine(StyleRoot, path);
			return StaticFile(path, responseOptions);
		}

		public ActionResult Script(string path, ResponseOptions responseOptions = null)
		{
			path = Path.Combine(ScriptRoot, path);
			return StaticFile(path, responseOptions);
		}

		public ActionResult StaticFile(string path, ResponseOptions responseOptions)
		{
			responseOptions ??= Options.DefaultStaticFileResponseOptions;
			HttpContextAccessor.HttpContext.Response.Headers.Add("Cache-Control", responseOptions.CacheHeader.ToString());


			if (File.Exists(path))
			{
				var file = new FileInfo(path);
				var extension = file.Extension;
				var contentType = MimeTypeMap.GetMimeType(extension);
				return new FileStreamResult(File.OpenRead(path), contentType);
			}
			return new NotFoundResult();
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
