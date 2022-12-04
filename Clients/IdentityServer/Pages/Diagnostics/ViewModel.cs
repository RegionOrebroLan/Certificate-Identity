// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System.Security.Claims;

namespace IdentityServer.Pages.Diagnostics;

public class ViewModel
{
	#region Properties

	public virtual IList<Claim> AuthenticationClaims { get; } = new List<Claim>();
	public virtual IList<string> AuthenticationClients { get; } = new List<string>();
	public virtual IDictionary<string, string> AuthenticationProperties { get; } = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	public virtual IList<Claim> UserClaims { get; } = new List<Claim>();

	#endregion
}