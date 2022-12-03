// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System.Security.Claims;

namespace IdentityServer.Pages.Diagnostics;

public class ViewModel
{
	#region Properties

	public virtual IList<Claim> Claims { get; } = new List<Claim>();
	public virtual IList<string> Clients { get; } = new List<string>();
	public virtual IDictionary<string, string> Properties { get; } = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	#endregion
}