<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StorageSpaceServices_Settings.ascx.cs" Inherits="WebsitePanel.Portal.ProviderControls.StorageSpaceServices_Settings" %>

<table cellpadding="1" cellspacing="0" width="100%">
    <tr>
        <td class="SubHead" width="200" nowrap>
            <asp:Label ID="lblWebDavSiteAppPoolIdentity" runat="server" meta:resourcekey="lblWebDavSiteAppPoolIdentity" Text="Webdav site application pool identity:"></asp:Label>
        </td>
        <td width="100%">
            <asp:TextBox runat="server" ID="txtWebDavSiteAppPoolIdentity" Width="300px" CssClass="NormalTextBox"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valRequiredFolder" runat="server" ControlToValidate="txtWebDavSiteAppPoolIdentity"
                ErrorMessage="*"></asp:RequiredFieldValidator>
        </td>
    </tr>
</table>

