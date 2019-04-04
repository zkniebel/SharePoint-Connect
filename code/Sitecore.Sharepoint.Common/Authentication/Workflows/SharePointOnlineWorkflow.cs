// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharePointOnlineWorkflow.cs" company="Sitecore A/S">
//   Copyright (c) Sitecore A/S. All rights reserved.
// </copyright>
// <summary>
//   Defines the SharePointOnlineWorkflow type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.SharePoint.Client;

namespace Sitecore.Sharepoint.Common.Authentication.Workflows
{
  using System;
  using System.Net;
  using Sitecore.Diagnostics;
  using Sitecore.Sharepoint.Common.Authentication.Workflows.Helpers;

  public class SharePointOnlineWorkflow : ClaimsBasedWorkflow
  {
    private readonly SharePointOnlineCredentialsWrapper sharePointOnlineCredentialsWrapper;

    public SharePointOnlineWorkflow()
      : this(WorkflowsDefaults.Instance.GetSharePointOnlineCredentialsWrapper())
    {
    }

    public SharePointOnlineWorkflow([NotNull] SharePointOnlineCredentialsWrapper sharePointOnlineCredentialsWrapper)
    {
      Assert.ArgumentNotNull(sharePointOnlineCredentialsWrapper, "sharePointOnlineCredentialsWrapper");

      this.sharePointOnlineCredentialsWrapper = sharePointOnlineCredentialsWrapper;
    }

    [NotNull]
    public override CookieContainer GetAuthenticationCookies(
        string url,
        NetworkCredential credential)
    {
        Uri uri = new Uri(url);
        string authenticationCookie = new SharePointOnlineCredentials(credential.UserName, credential.SecurePassword).GetAuthenticationCookie(uri);
        CookieContainer cookieContainer = new CookieContainer();
        cookieContainer.SetCookies(uri, authenticationCookie);
        return cookieContainer;
    }
    }
}