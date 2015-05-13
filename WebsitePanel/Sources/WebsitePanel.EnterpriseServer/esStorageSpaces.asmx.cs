using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Services;
using Microsoft.Web.Services3;
using WebsitePanel.Providers.Common;
using WebsitePanel.Providers.ResultObjects;
using WebsitePanel.Providers.StorageSpaces;

namespace WebsitePanel.EnterpriseServer
{
    /// <summary>
    /// Summary description for esSystem
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class esStorageSpaces : System.Web.Services.WebService
    {
        [WebMethod]
        public StorageSpaceLevelPaged GetStorageSpaceLevelsPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return StorageSpacesController.GetStorageSpaceLevelsPaged(filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public StorageSpaceLevel GetStorageSpaceLevelById(int id)
        {
            return StorageSpacesController.GetStorageSpaceLevelById(id);
        }

        [WebMethod]
        public IntResult SaveStorageSpaceLevel(StorageSpaceLevel level, List<ResourceGroupInfo> groups )
        {
            return StorageSpacesController.SaveStorageSpaceLevel(level, groups);
        }

        [WebMethod]
        public List<ResourceGroupInfo> GetLevelResourceGroups(int id)
        {
            return StorageSpacesController.GetLevelResourceGroups(id);
        }

        [WebMethod]
        public ResultObject SaveLevelResourceGroups(int levelId, List<ResourceGroupInfo> newGroups)
        {
            return StorageSpacesController.SaveLevelResourceGroups(levelId, newGroups);
        }

        [WebMethod]
        public ResultObject RemoveStorageSpaceLevel(int id)
        {
            return StorageSpacesController.RemoveStorageSpaceLevel(id);
        }
    }
}
