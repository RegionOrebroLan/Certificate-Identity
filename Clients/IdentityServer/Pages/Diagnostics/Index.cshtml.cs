using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Test;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Diagnostics;

[SecurityHeaders]
[Authorize]
public class Index : PageModel
{
	#region Fields

	private ViewModel _view;

	#endregion

	#region Constructors

	public Index(TestUserStore userStore)
	{
		this.UserStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
	}

	#endregion

	#region Properties

	protected internal virtual TestUserStore UserStore { get; }

	public virtual ViewModel View
	{
		get
		{
			if(this._view == null)
			{
				var view = new ViewModel();

				var authenticateResult = this.HttpContext.AuthenticateAsync().Result;

				var claims = new List<Claim>();
				claims.AddRange(authenticateResult.Principal.Claims);

				var subject = authenticateResult.Principal.GetSubjectId();

				var user = this.UserStore.FindBySubjectId(subject);

				foreach(var claim in user.Claims)
				{
					if(claims.Any(item => string.Equals(item.Type, claim.Type, StringComparison.OrdinalIgnoreCase)))
						continue;

					claims.Add(claim);
				}

				claims.Sort((first, second) => string.Compare(first?.Type, second?.Type, StringComparison.OrdinalIgnoreCase));

				foreach(var claim in claims)
				{
					view.Claims.Add(claim);
				}

				if(authenticateResult.Properties.Items.ContainsKey("client_list"))
				{
					var encoded = authenticateResult.Properties.Items["client_list"];
					var bytes = Base64Url.Decode(encoded);
					var value = Encoding.UTF8.GetString(bytes);

					foreach(var client in JsonSerializer.Deserialize<string[]>(value))
					{
						view.Clients.Add(client);
					}
				}

				foreach(var (key, value) in authenticateResult.Properties.Items)
				{
					view.Properties.Add(key, value);
				}

				this._view = view;
			}

			return this._view;
		}
	}

	#endregion

	#region Methods

	public async Task<IActionResult> OnGet()
	{
		var localAddresses = new string[] { "127.0.0.1", "::1", this.HttpContext.Connection.LocalIpAddress.ToString() };
		if(!localAddresses.Contains(this.HttpContext.Connection.RemoteIpAddress.ToString()))
			return this.NotFound();

		return await Task.FromResult(this.Page());
	}

	#endregion
}