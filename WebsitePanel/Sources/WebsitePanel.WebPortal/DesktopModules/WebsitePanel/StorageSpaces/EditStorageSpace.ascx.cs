using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsitePanel.Providers.OS;
using WebsitePanel.Providers.StorageSpaces;

namespace WebsitePanel.Portal.StorageSpaces
{
    public partial class EditStorageSpace : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // servers.Module = Module;

            if (!Page.IsPostBack)
            {
                var services = ES.Services.Servers.GetRawServicesByGroupId(EnterpriseServer.ServiceGroupIds.StorageSpace);

                ddlStorageService.DataSource = services.Tables[0];
                ddlStorageService.DataTextField = "ServiceName";
                ddlStorageService.DataValueField = "ServiceID";
                ddlStorageService.DataBind();

                var levels = ES.Services.StorageSpaces.GetStorageSpaceLevelsPaged(string.Empty, string.Empty, string.Empty, 0, int.MaxValue);

                foreach (var level in levels.Levels)
                {
                    ddlSsLevel.Items.Add(new ListItem(level.Name, level.Id.ToString()));
                }

                var storage = ES.Services.StorageSpaces.GetStorageSpaceById(PanelRequest.StorageSpaceId);

                if (storage != null)
                {
                    txtName.Text = storage.Name;
                    txtPath.Text = storage.Path;
                    txtStorageSize.Text = ConvertBytesToGB(storage.FsrmQuotaSizeBytes).ToString();

                    switch (storage.FsrmQuotaType)
                    {
                        case QuotaType.Hard:
                            rbtnQuotaHard.Checked = true;
                            break;
                        case QuotaType.Soft:
                            rbtnQuotaSoft.Checked = true;
                            break;
                    }

                    ddlStorageService.SelectedValue = storage.ServiceId.ToString();
                    ddlSsLevel.SelectedValue = storage.LevelId.ToString();

                }
            }
        }

        private bool SaveStorageSpace(out int storageId)
        {
            StorageSpace storage = ES.Services.StorageSpaces.GetStorageSpaceById(PanelRequest.StorageSpaceId)
                                      ?? new StorageSpace();

            storage.Id = PanelRequest.StorageSpaceId;
            storage.Name = txtName.Text;
            storage.Path = txtPath.Text;
            storage.LevelId = Utils.ParseInt(ddlSsLevel.SelectedValue);
            storage.ServiceId = Utils.ParseInt(ddlStorageService.SelectedValue);

            var serviceInfo = ES.Services.Servers.GetServiceInfo(storage.ServiceId);

            storage.ServerId = serviceInfo.ServerId;

            storage.FsrmQuotaType = rbtnQuotaSoft.Checked ? QuotaType.Soft : QuotaType.Hard;
            storage.FsrmQuotaSizeBytes = Convert.ToInt64(txtStorageSize.Text)*1024*1024*1024;

            var result = ES.Services.StorageSpaces.SaveStorageSpace(storage);

            storageId = result.Value;

            messageBox.ShowMessage(result, "STORAGE_SPACE_SAVE", null);

            return result.IsSuccess;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            int spaceId;
            if (SaveStorageSpace(out spaceId) && PanelRequest.StorageSpaceId <= 0)
            {
                EditStorageSpaceRedirect(spaceId);
            }
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            int spaceId;
            if (SaveStorageSpace(out spaceId))
            {
                Response.Redirect(EditUrl(null));
            }
        }

        private void EditStorageSpaceRedirect(int id)
        {
            Response.Redirect(EditUrl("StorageSpaceId", id.ToString(), "edit_storage_space"));
        }

        protected decimal ConvertBytesToGB(object size)
        {
            return Math.Round(Convert.ToDecimal(size) / (1024 * 1024 * 1024), 2);
        }
    }
}
