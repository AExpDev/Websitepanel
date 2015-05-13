using WebsitePanel.Providers.StorageSpaces;

namespace WebsitePanel.Portal
{
    public class SsHelper
    {
        #region Space Storage Levels

        StorageSpaceLevelPaged ssLevels;

        public int GetStorageSpaceLevelsPagedCount(string filterValue)
        {
            return ssLevels.RecordsCount;
        }

        public StorageSpaceLevel[] GetStorageSpaceLevelsPaged(int maximumRows, int startRowIndex, string sortColumn, string filterValue)
        {
            ssLevels = ES.Services.StorageSpaces.GetStorageSpaceLevelsPaged("", filterValue, sortColumn, startRowIndex, maximumRows);

            return ssLevels.Levels;
        }

        #endregion 
    }
}