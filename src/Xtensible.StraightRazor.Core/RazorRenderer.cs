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

		private RazorLightEngine Engine { get; }
		private string ImageRoot { get; }
		private string StyleRoot { get; }
		private string ScriptRoot { get; }

		public RazorRenderer(Assembly operatingAssembly, RazorRendererOptions options = null)
		{
			options ??= new RazorRendererOptions();

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

		public ActionResult Image(string path)
		{
			path = Path.Combine(ImageRoot, path);
			return StaticFile(path);
		}

		public ActionResult Style(string path)
		{
			path = Path.Combine(StyleRoot, path);
			return StaticFile(path);
		}

		public ActionResult Script(string path)
		{
			path = Path.Combine(ScriptRoot, path);
			return StaticFile(path);
		}

		public ActionResult StaticFile(string path)
		{
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
