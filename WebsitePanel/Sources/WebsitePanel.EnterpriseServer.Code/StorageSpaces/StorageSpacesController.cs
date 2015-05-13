using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using WebsitePanel.Providers.Common;
using WebsitePanel.Providers.ResultObjects;
using WebsitePanel.Providers.StorageSpaces;

namespace WebsitePanel.EnterpriseServer
{
    public class StorageSpacesController
    {
        public static StorageSpaceLevelPaged GetStorageSpaceLevelsPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return GetStorageSpaceLevelsPagedInternal(filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        private static StorageSpaceLevelPaged GetStorageSpaceLevelsPagedInternal(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            DataSet ds = DataProvider.GetStorageSpaceLevelsPaged(filterColumn, filterValue, sortColumn, startRow, maximumRows);

            var result = new StorageSpaceLevelPaged
            {
                RecordsCount = (int) ds.Tables[0].Rows[0][0]
            };

            var tmpLevels = new List<StorageSpaceLevel>();

            ObjectUtils.FillCollectionFromDataView(tmpLevels, ds.Tables[1].DefaultView);

            result.Levels = tmpLevels.ToArray();

            return result;
        }

        public static StorageSpaceLevel GetStorageSpaceLevelById(int levelId)
        {
            return GetStorageSpaceLevelByIdInternal(levelId);
        }

        private static StorageSpaceLevel GetStorageSpaceLevelByIdInternal(int levelId)
        {
            return ObjectUtils.FillObjectFromDataReader<StorageSpaceLevel>(DataProvider.GetStorageSpaceLevelById(levelId));
        }

        public static IntResult SaveStorageSpaceLevel(StorageSpaceLevel level, List<ResourceGroupInfo> groups)
        {
            return SaveStorageSpaceLevelInternal(level, groups);
        }

        private static IntResult SaveStorageSpaceLevelInternal(StorageSpaceLevel level, List<ResourceGroupInfo> groups)
        {

            var result = TaskManager.StartResultTask<IntResult>("STORAGE_SPACES", "SAVE_STORAGE_SPACE_LEVEL");

            try
            {
                if (level == null)
                {
                    throw new ArgumentNullException("level");
                }

                if (level.Id > 0)
                {
                    DataProvider.UpdateStorageSpaceLevel(level);

                    TaskManager.Write("Updating Storage Space Level with id = {0}",
                        level.Id.ToString(CultureInfo.InvariantCulture));

                    result.Value = level.Id;
                }
                else
                {
                    result.Value = DataProvider.InsertStorageSpaceLevel(level);
                    TaskManager.Write("Inserting new Storage Space Level, obtained id = {0}",
                        level.Id.ToString(CultureInfo.InvariantCulture));

                    level.Id = result.Value;
                }

                var resultGroup = SaveLevelResourceGroups(result.Value, groups);

                if (!resultGroup.IsSuccess)
                {
                    throw new Exception("Error saving resource groups");
                }
            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error saving Storage Space Level", exception);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        public static ResultObject RemoveStorageSpaceLevel(int id)
        {
            return RemoveStorageSpaceLevelInternal(id);
        }

        private static ResultObject RemoveStorageSpaceLevelInternal(int id)
        {

            var result = TaskManager.StartResultTask<ResultObject>("STORAGE_SPACES", "REMOVE_STORAGE_SPACE_LEVEL");

            try
            {
                if (id < 1)
                {
                    throw new ArgumentException("Id must be greater than 0");
                }

                DataProvider.RemoveStorageSpaceLevel(id);

            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error removing Storage Space Level", exception);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }


        public static List<ResourceGroupInfo> GetLevelResourceGroups(int levelId)
        {
            return GetLevelResourceGroupsInternal(levelId);
        }

        private static List<ResourceGroupInfo> GetLevelResourceGroupsInternal(int levelId)
        {
            return ObjectUtils.CreateListFromDataReader<ResourceGroupInfo>(DataProvider.GetStorageSpaceLevelResourceGroups(levelId)).ToList();
        }

        public static ResultObject SaveLevelResourceGroups(int levelId,  List<ResourceGroupInfo> newGroups)
        {
            return SaveLevelResourceGroupsInternal(levelId, newGroups);
        }

        private static ResultObject SaveLevelResourceGroupsInternal(int levelId, IEnumerable<ResourceGroupInfo> newGroups)
        {
            var result = TaskManager.StartResultTask<ResultObject>("STORAGE_SPACES", "REMOVE_STORAGE_SPACE_LEVEL");

            try
            {
                if (levelId < 1)
                {
                    throw new ArgumentException("Level Id must be greater than 0");
                }

                DataProvider.RemoveStorageSpaceLevelResourceGroups(levelId);

                if (newGroups != null)
                {
                    foreach (var newGroup in newGroups)
                    {
                        DataProvider.AddStorageSpaceLevelResourceGroup(levelId, newGroup.GroupId);
                    }
                }

            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error saving Storage Space Level Resource Groups", exception);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }
    }
}