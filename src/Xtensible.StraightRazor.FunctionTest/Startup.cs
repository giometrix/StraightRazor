using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Xtensible.StraightRazor.FunctionTest;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Xtensible.StraightRazor.FunctionTest
{
	public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			
		}
	}

}
