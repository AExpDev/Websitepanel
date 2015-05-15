using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Microsoft.Web.Services3;
using WebsitePanel.Providers;
using WebsitePanel.Providers.EnterpriseStorage;
using WebsitePanel.Providers.OS;
using WebsitePanel.Providers.StorageSpaces;
using WebsitePanel.Server.Utils;

namespace WebsitePanel.Server
{
    /// <summary>
    /// Summary description for StorageSpace
    /// </summary>
    [WebService(Namespace = "http://smbsaas/websitepanel/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class StorageSpaceServices : HostingServiceProviderWebService, IStorageSpace
    {
        private IStorageSpace StorageSpaceProvider
        {
            get { return (IStorageSpace)Provider; }
        }

        [WebMethod, SoapHeader("settings")]
        public List<SystemFile> GetAllDriveLetters()
        {
            try
            {
                Log.WriteStart("'{0}' GetAllDriveLetters", ProviderSettings.ProviderName);
                var result = StorageSpaceProvider.GetAllDriveLetters();
                Log.WriteEnd("'{0}' GetAllDriveLetters", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetAllDriveLetters", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<SystemFile> GetSystemSubFolders(string path)
        {
            try
            {
                Log.WriteStart("'{0}' GetSystemFolders", ProviderSettings.ProviderName);
                var result = StorageSpaceProvider.GetSystemSubFolders(path);
                Log.WriteEnd("'{0}' GetSystemFolders", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSystemFolders", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateStorageSettings(string fullPath, long qouteSizeBytes, QuotaType type)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateStorageSettings", ProviderSettings.ProviderName);
                StorageSpaceProvider.UpdateStorageSettings(fullPath, qouteSizeBytes, type);
                Log.WriteEnd("'{0}' UpdateStorageSettings", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateStorageSettings", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ClearStorageSettings(string fullPath)
        {
            try
            {
                Log.WriteStart("'{0}' ClearStorageSettings", ProviderSettings.ProviderName);
                StorageSpaceProvider.ClearStorageSettings(fullPath);
                Log.WriteEnd("'{0}' ClearStorageSettings", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ClearStorageSettings", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateFolderQuota(string fullPath, long qouteSizeBytes, QuotaType type)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateFolderQuota", ProviderSettings.ProviderName);
                StorageSpaceProvider.UpdateFolderQuota(fullPath, qouteSizeBytes, type);
                Log.WriteEnd("'{0}' UpdateFolderQuota", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateFolderQuota", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

    }
}
