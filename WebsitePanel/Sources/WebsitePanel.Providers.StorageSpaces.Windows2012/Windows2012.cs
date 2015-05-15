using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using WebsitePanel.Providers.OS;
using WebsitePanel.Providers.Utils;
using WebsitePanel.Server.Utils;
namespace WebsitePanel.Providers.StorageSpaces
{
    public class Windows2012 : WebsitePanel.Providers.OS.Windows2012, IStorageSpace
    {
        #region Properties
        internal string PrimaryDomainController
        {
            get { return ProviderSettings["PrimaryDomainController"]; }
        }

        #endregion Properties


        public override bool IsInstalled()
        {
            Server.Utils.OS.WindowsVersion version = WebsitePanel.Server.Utils.OS.GetVersion();
            return version == WebsitePanel.Server.Utils.OS.WindowsVersion.WindowsServer2012 ||
                   version == WebsitePanel.Server.Utils.OS.WindowsVersion.WindowsServer2012R2;
        }

        #region HostingServiceProvider methods

        public override string[] Install()
        {
            List<string> messages = new List<string>();

            try
            {
                if (CheckFileServicesInstallation())
                {
                    InstallFsrmService();
                }
            }
            catch (Exception ex)
            {
                messages.Add(String.Format("Error isntalling FSRM Service: {0}", ex.Message));
            }

            return messages.ToArray();
        }

        public override void DeleteServiceItems(ServiceProviderItem[] items)
        {
            foreach (ServiceProviderItem item in items)
            {
                try
                {
                    if (item is HomeFolder)
                        // delete home folder
                        FileUtils.DeleteFile(item.Name);
                }
                catch (Exception ex)
                {
                    Log.WriteError(String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
                }
            }
        }

        public override ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(ServiceProviderItem[] items)
        {
            List<ServiceProviderItemDiskSpace> itemsDiskspace = new List<ServiceProviderItemDiskSpace>();
            foreach (ServiceProviderItem item in items)
            {
                if (item is HomeFolder)
                {
                    try
                    {
                        string path = item.Name;

                        Log.WriteStart(String.Format("Calculating '{0}' folder size", path));

                        // calculate disk space
                        ServiceProviderItemDiskSpace diskspace = new ServiceProviderItemDiskSpace();
                        diskspace.ItemId = item.Id;
                        diskspace.DiskSpace = FileUtils.CalculateFolderSize(path);
                        itemsDiskspace.Add(diskspace);

                        Log.WriteEnd(String.Format("Calculating '{0}' folder size", path));
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(ex);
                    }
                }
            }
            return itemsDiskspace.ToArray();
        }

        #endregion

        #region Storage Spaces

        public void UpdateStorageSettings(string fullPath, long qouteSizeBytes, QuotaType type)
        {
            UpdateFolderQuota(fullPath, qouteSizeBytes, type);
        }

        public void ClearStorageSettings(string fullPath)
        {
            Log.WriteStart("ClearStorageSettings");
            Log.WriteInfo("FolderPath : {0}", fullPath);

            Runspace runSpace = null;

            try
            {
                runSpace = OpenRunspace();

                RemoveOldQuotaOnFolder(runSpace, fullPath);
            }
            catch (Exception ex)
            {
                Log.WriteError("ClearStorageSettings", ex);
                throw;
            }
            finally
            {
                CloseRunspace(runSpace);
                Log.WriteEnd("ClearStorageSettings");
            }

        } 

        #endregion

        #region Storage Space Folders

        public void CreateStorageFolder(string fullPath)
        {
            FileUtils.CreateDirectory(fullPath);
        }

        public string ShareFolder(string fullPath)
        {
            try
            {
                Log.WriteStart("ShareFolder");
                Log.WriteInfo("FolderPath : {0}", fullPath);

                ManagementClass managementClass = new ManagementClass("Win32_Share");

                ManagementBaseObject inParams = managementClass.GetMethodParameters("Create");

                ManagementBaseObject outParams;

                // Set the input parameters

                inParams["Path"] = fullPath;

                inParams["Type"] = 0x0; // Disk Drive


                // Invoke the method on the ManagementClass object

                outParams = managementClass.InvokeMethod("Create", inParams, null);

                // Check to see if the method invocation was successful

                if ((uint)(outParams.Properties["ReturnValue"].Value) != 0)
                {
                    throw new Exception("Unable to share directory.");
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
            finally 
            {
                Log.WriteEnd("ShareFolder");
            }
        }

        #endregion

        public void UpdateFolderQuota(string fullPath, long qouteSizeBytes, QuotaType quotaType)
        {
            var driveLetter = Path.GetPathRoot(fullPath);
            var pathWithoutDriveLetter = fullPath.Replace(driveLetter, string.Empty);
            driveLetter = driveLetter.Replace(":\\", string.Empty);

            SetQuotaLimitOnFolder(pathWithoutDriveLetter, driveLetter, quotaType, (qouteSizeBytes / 1024).ToString() + "KB", 0, String.Empty, String.Empty);
        }

        public List<SystemFile> GetAllDriveLetters()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            var folders = new List<SystemFile>();

            foreach (var drive in drives)
            {
                var folder = new SystemFile();

                folder.Name = drive.Name;

                folders.Add(folder);
            }

            return folders;
        }

        public List<SystemFile> GetSystemSubFolders(string path)
        {
            DirectoryInfo rootDir = new DirectoryInfo(path);
            DirectoryInfo[] subdirs = rootDir.GetDirectories();

            var folders = new List<SystemFile>();

            foreach (var subdir in subdirs)
            {
                var folder = new SystemFile();

                folder.Name = subdir.FullName;

                folders.Add(folder);
            }

            return folders;
        }
    }
}