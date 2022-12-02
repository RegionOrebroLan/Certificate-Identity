using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RegionOrebroLan.CertificateIdentity.Configuration;

namespace Application.Controllers
{
	public class ErrorController : SiteController
	{
		#region Constructors

		public ErrorController(IOptionsMonitor<ExceptionHandlingOptions> exceptionHandlingOptionsMonitor, IIdentityServerInteractionService interactionService, ILoggerFactory loggerFactory) : base(loggerFactory)
		{
			this.ExceptionHandlingOptionsMonitor = exceptionHandlingOptionsMonitor ?? throw new ArgumentNullException(nameof(exceptionHandlingOptionsMonitor));
			this.InteractionService = interactionService ?? throw new ArgumentNullException(nameof(interactionService));
		}

		#endregion

		#region Properties

		protected internal virtual IOptionsMonitor<ExceptionHandlingOptions> ExceptionHandlingOptionsMonitor { get; }
		protected internal virtual IIdentityServerInteractionService InteractionService { get; }

		#endregion

		#region Methods

		public virtual async Task<IActionResult> Index(string errorId)
		{
			var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			var exceptionHandling = this.ExceptionHandlingOptionsMonitor.CurrentValue;
			var identityServerError = await this.InteractionService.GetErrorContextAsync(errorId);

			if(identityServerError != null)
			{
				dictionary.Add(nameof(identityServerError.Error), identityServerError.Error);

				if(exceptionHandling.Detailed)
					dictionary.Add("Details", identityServerError.ErrorDescription);

				dictionary.Add(nameof(identityServerError.RequestId), identityServerError.RequestId);
			}
			else
			{
				var exceptionHandlerPathFeature = this.HttpContext.Features.Get<IExceptionHandlerPathFeature>();

				dictionary.Add(nameof(identityServerError.Error), exceptionHandlerPathFeature?.Error.GetType().Name);

				if(exceptionHandling.Detailed)
					dictionary.Add("Details", exceptionHandlerPathFeature?.Error.ToString());
			}

			return await Task.FromResult(this.Content(string.Join(Environment.NewLine, dictionary.Select(entry => $"{entry.Key}: {entry.Value}"))));
		}

		#endregion
	}
}