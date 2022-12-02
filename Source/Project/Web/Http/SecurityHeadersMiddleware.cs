using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RegionOrebroLan.CertificateIdentity.Web.Http.Configuration;

namespace RegionOrebroLan.CertificateIdentity.Web.Http
{
	public class SecurityHeadersMiddleware
	{
		#region Constructors

		public SecurityHeadersMiddleware(ILoggerFactory loggerFactory, RequestDelegate next, IOptionsMonitor<SecurityHeadersOptions> optionsMonitor)
		{
			this.Logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
			this.Next = next ?? throw new ArgumentNullException(nameof(next));
			this.OptionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
		}

		#endregion

		#region Properties

		protected internal virtual ILogger Logger { get; }
		protected internal virtual RequestDelegate Next { get; }
		protected internal virtual IOptionsMonitor<SecurityHeadersOptions> OptionsMonitor { get; }

		#endregion

		#region Methods

		protected internal virtual async Task<bool> ExcludePath(HttpContext httpContext, SecurityHeadersOptions options)
		{
			if(httpContext == null)
				throw new ArgumentNullException(nameof(httpContext));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(options.ExcludePaths.Any(path => httpContext.Request.Path.ToString().StartsWith(path, StringComparison.OrdinalIgnoreCase)))
				return true;

			return await Task.FromResult(false);
		}

		public virtual async Task Invoke(HttpContext httpContext)
		{
			if(httpContext == null)
				throw new ArgumentNullException(nameof(httpContext));

			var options = this.OptionsMonitor.CurrentValue;

			if(options.Enabled && !await this.ExcludePath(httpContext, options))
			{
				var responseHeaders = httpContext.Response.Headers;

				foreach(var (key, value) in options.Headers)
				{
					if(responseHeaders.ContainsKey(key))
						continue;

					responseHeaders.Add(key, value);
				}
			}

			await this.Next(httpContext);
		}

		#endregion
	}
}