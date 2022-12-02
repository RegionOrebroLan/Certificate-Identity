using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	public abstract class SiteController : Controller
	{
		#region Constructors

		protected SiteController(ILoggerFactory loggerFactory)
		{
			this.Logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
		}

		#endregion

		#region Properties

		protected internal virtual ILogger Logger { get; }

		#endregion
	}
}