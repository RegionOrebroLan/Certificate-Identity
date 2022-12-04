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
	#region Constructors

	public Index(TestUserStore userStore)
	{
		this.UserStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
	}

	#endregion

	#region Properties

	protected internal virtual TestUserStore UserStore { get; }
	public virtual ViewModel View { get; } = new();

	#endregion

	#region Methods

	protected internal virtual int CompareClaims(Claim firstClaim, Claim secondClaim)
	{
		return string.Compare(firstClaim?.Type, secondClaim?.Type, StringComparison.OrdinalIgnoreCase);
	}

	public virtual async Task<IActionResult> OnGet()
	{
		var localAddresses = new[] { "127.0.0.1", "::1", this.HttpContext.Connection.LocalIpAddress.ToString() };
		if(!localAddresses.Contains(this.HttpContext.Connection.RemoteIpAddress.ToString()))
			return this.NotFound();

		await this.PopulateModelAsync();

		return await Task.FromResult(this.Page());
	}

	protected internal virtual async Task PopulateAuthenticationClaimsAsync(AuthenticateResult authenticateResult)
	{
		await Task.CompletedTask;

		var claims = new List<Claim>(authenticateResult?.Principal?.Claims ?? Enumerable.Empty<Claim>());

		claims.Sort(this.CompareClaims);

		foreach(var claim in claims)
		{
			this.View.AuthenticationClaims.Add(claim);
		}
	}

	protected internal virtual async Task PopulateAuthenticationClientsAsync(AuthenticateResult authenticateResult)
	{
		await Task.CompletedTask;

		var properties = authenticateResult?.Properties?.Items;

		if(properties == null)
			return;

		if(!properties.TryGetValue("client_list", out var encodedClients))
			return;

		var bytes = Base64Url.Decode(encodedClients);
		var value = Encoding.UTF8.GetString(bytes);
		var clients = new SortedSet<string>(JsonSerializer.Deserialize<string[]>(value) ?? Array.Empty<string>());

		foreach(var client in clients)
		{
			this.View.AuthenticationClients.Add(client);
		}
	}

	protected internal virtual async Task PopulateAuthenticationPropertiesAsync(AuthenticateResult authenticateResult)
	{
		await Task.CompletedTask;

		foreach(var (key, value) in authenticateResult?.Properties?.Items ?? new Dictionary<string, string>())
		{
			this.View.AuthenticationProperties.Add(key, value);
		}
	}

	protected internal virtual async Task PopulateModelAsync()
	{
		var authenticateResult = await this.HttpContext.AuthenticateAsync();

		await this.PopulateAuthenticationClaimsAsync(authenticateResult);
		await this.PopulateAuthenticationClientsAsync(authenticateResult);
		await this.PopulateAuthenticationPropertiesAsync(authenticateResult);
		await this.PopulateUserClaimsAsync(authenticateResult);
	}

	protected internal virtual async Task PopulateUserClaimsAsync(AuthenticateResult authenticateResult)
	{
		await Task.CompletedTask;

		var subject = authenticateResult?.Principal?.GetSubjectId();

		if(string.IsNullOrEmpty(subject))
			return;

		var user = this.UserStore.FindBySubjectId(subject);

		if(user == null)
			return;

		var claims = new List<Claim>(user.Claims);

		claims.Sort(this.CompareClaims);

		foreach(var claim in claims)
		{
			this.View.UserClaims.Add(claim);
		}
	}

	#endregion
}