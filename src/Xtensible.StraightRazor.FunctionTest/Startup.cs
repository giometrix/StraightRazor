using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xtensible.StraightRazor.Core;
using Xtensible.StraightRazor.FunctionTest;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Xtensible.StraightRazor.FunctionTest
{
	public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddSingleton<RazorRenderer>(x => new RazorRenderer(typeof(Startup).Assembly, x.GetService<IHttpContextAccessor>()));
		}
	}

}
