// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Identity.Client;

namespace BookReservations.App.Msal;

public class PCAWrapper
{
    private readonly IPublicClientApplication PCA;

    private const string ClientId = "707b9ce6-f2e3-48da-9649-fc2586c71205";
    private const string TenantId = "f2f7ce9e-e999-4d27-8d95-23ef635c3f8f";
    private const string Authority = $"https://login.microsoftonline.com/{TenantId}";
    public static string[] Scopes = { $"api://{ClientId}/access_as_user" };

    public PCAWrapper()
    {
        // Create PCA once. Make sure that all the config parameters below are passed
        PCA = PublicClientApplicationBuilder
                                    .Create(ClientId)
                                    .WithRedirectUri(PlatformConfig.Instance.RedirectUri)
                                    .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
                                    .Build();
    }

    /// <summary>
    /// Acquire the token silently
    /// </summary>
    /// <param name="scopes">desired scopes</param>
    /// <returns>Authentication result</returns>
    public async Task<AuthenticationResult> AcquireTokenSilentAsync(string[] scopes)
    {
        var accts = await PCA.GetAccountsAsync().ConfigureAwait(false);
        var acct = accts.FirstOrDefault();

        var authResult = await PCA.AcquireTokenSilent(scopes, acct).ExecuteAsync().ConfigureAwait(false);
        return authResult;

    }

    /// <summary>
    /// Perform the interactive acquisition of the token for the given scope
    /// </summary>
    /// <param name="scopes">desired scopes</param>
    /// <returns></returns>
    internal async Task<AuthenticationResult> AcquireTokenInteractiveAsync(string[] scopes)
    {

        return await PCA.AcquireTokenInteractive(scopes)
                                .WithAuthority(Authority)
                                .WithTenantId(TenantId)
                                .WithParentActivityOrWindow(PlatformConfig.Instance.ParentWindow)
                                .WithUseEmbeddedWebView(true)
                                .ExecuteAsync()
                                .ConfigureAwait(false);
    }

    /// <summary>
    /// Signout may not perform the complete signout as company portal may hold
    /// the token.
    /// </summary>
    /// <returns></returns>
    internal async Task SignOutAsync()
    {
        var accounts = await PCA.GetAccountsAsync().ConfigureAwait(false);
        foreach (var acct in accounts)
        {
            await PCA.RemoveAsync(acct).ConfigureAwait(false);
        }
    }
}