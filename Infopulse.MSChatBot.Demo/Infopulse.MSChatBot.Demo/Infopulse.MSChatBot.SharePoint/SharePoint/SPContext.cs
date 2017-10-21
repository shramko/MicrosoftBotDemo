using System;
using System.Security;
using Infopulse.MSChatBot.SharePoint.Helpers;
using Microsoft.SharePoint.Client;

namespace Infopulse.MSChatBot.SharePoint.SharePoint
{
    [Serializable]
    public class SPContext : IDisposable
    {
        public ClientContext ClientCtx { get; }
        private static readonly Lazy<SPContext> LazyInstance = new Lazy<SPContext>(() => new SPContext());

        private SPContext()
        {
            ClientCtx = new ClientContext(Config.SiteUrl);
            SecureString passWord = new SecureString();
            foreach (char c in Config.Pass.ToCharArray()) passWord.AppendChar(c);
            ClientCtx.Credentials = new SharePointOnlineCredentials(Config.Login, passWord);
        }

        public static SPContext Instance => LazyInstance.Value;
        public void Dispose()
        {
            ClientCtx?.Dispose();
        }
    }



}