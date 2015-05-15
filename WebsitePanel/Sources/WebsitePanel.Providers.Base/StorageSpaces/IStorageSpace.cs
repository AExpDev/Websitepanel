using System.Collections.Generic;
using System.IO;
using WebsitePanel.Providers.OS;

namespace WebsitePanel.Providers.StorageSpaces
{
    public interface IStorageSpace
    {
        List<SystemFile> GetAllDriveLetters();
        List<SystemFile> GetSystemSubFolders(string path);
        void UpdateStorageSettings(string fullPath, long qouteSizeBytes, QuotaType type);
        void ClearStorageSettings(string fullPath);
        void UpdateFolderQuota(string fullPath, long qouteSizeBytes, QuotaType type);
    }
}