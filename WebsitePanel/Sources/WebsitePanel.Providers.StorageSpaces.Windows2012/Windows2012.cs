namespace WebsitePanel.Providers.StorageSpaces
{
    public class Windows2012 : HostingServiceProviderBase
    {
        public override bool IsInstalled()
        {
            Server.Utils.OS.WindowsVersion version = WebsitePanel.Server.Utils.OS.GetVersion();
            return version == WebsitePanel.Server.Utils.OS.WindowsVersion.WindowsServer2012 ||
                   version == WebsitePanel.Server.Utils.OS.WindowsVersion.WindowsServer2012R2;
        }
    }
}