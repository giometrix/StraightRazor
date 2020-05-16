using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Net.Http.Headers;

namespace Xtensible.StraightRazor.Core
{
	public class ResponseOptions
	{
		public CacheControlHeaderValue CacheHeader { get; set; }
	}
}
