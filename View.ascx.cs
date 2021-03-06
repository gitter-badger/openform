#region Copyright

// 
// Copyright (c) 2015
// by Satrabel
// 

#endregion

#region Using Statements

using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Common;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using Satrabel.OpenForm.Components;

#endregion

namespace Satrabel.OpenForm
{

    public partial class View : PortalModuleBase, IActionable
    {

        #region Event Handlers

        protected override void OnInit(EventArgs e)
        {

            base.OnInit(e);
            cmdSave.NavigateUrl = Globals.NavigateURL("", "result=1");
            ServicesFramework.Instance.RequestAjaxScriptSupport();
            ServicesFramework.Instance.RequestAjaxAntiForgerySupport();
            //JavaScript.RequestRegistration(CommonJs.DnnPlugins); ;
            //JavaScript.RequestRegistration(CommonJs.jQueryFileUpload);

        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                string Template = ModuleContext.Settings["template"] as string;

                InitForm(Template);
            }
        }

        private void InitForm(string Template)
        {
            bool TemplateDefined = !string.IsNullOrEmpty(Template);
            string settings = ModuleContext.Settings["data"] as string;
            bool SettingsDefined = !string.IsNullOrEmpty(settings);

            if (!TemplateDefined || !SettingsDefined)
            {
                pHelp.Visible = true;
            }
            if (ModuleContext.PortalSettings.UserInfo.IsSuperUser)
            {
                hlTempleteExchange.NavigateUrl = ModuleContext.EditUrl("ShareTemplate");
                hlTempleteExchange.Visible = true;
            }
            if (pHelp.Visible && ModuleContext.EditMode)
            {
                hlEditSettings.NavigateUrl = ModuleContext.EditUrl("EditSettings");
                hlEditSettings.Visible = true;
                scriptList.Items.AddRange(OpenFormUtils.GetTemplatesFiles(PortalSettings, ModuleId, Template).ToArray());
                scriptList.Visible = true;
            }


            if (string.IsNullOrEmpty(Template))
            {
                ScopeWrapper.Visible = false;
            }

        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, "Update Successful", DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.GreenSuccess);
        }


        protected void cmdCancel_Click(object sender, EventArgs e)
        {
        }

        #endregion
        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
        {
            get
            {
                var Actions = new ModuleActionCollection();
                Actions.Add(ModuleContext.GetNextActionID(),
                          Localization.GetString("EditSettings.Action", LocalResourceFile),
                          ModuleActionType.ContentOptions,
                          "",
                          "~/DesktopModules/OpenContent/images/settings.gif",
                          ModuleContext.EditUrl("EditSettings"),
                          false,
                          SecurityAccessLevel.Host,
                          true,
                          false);

                Actions.Add(ModuleContext.GetNextActionID(),
                            Localization.GetString(ModuleActionType.AddContent, LocalResourceFile),
                            ModuleActionType.AddContent,
                            "",
                            "",
                            ModuleContext.EditUrl(),
                            false,
                            SecurityAccessLevel.Edit,
                            true,
                            false);

                var scriptFileSetting = Settings["template"] as string;
                if (!string.IsNullOrEmpty(scriptFileSetting))
                {
                    Actions.Add(ModuleContext.GetNextActionID(),
                               Localization.GetString("EditTemplate.Action", LocalResourceFile),
                               ModuleActionType.ContentOptions,
                               "",
                               "~/DesktopModules/OpenContent/images/edittemplate.png",
                               ModuleContext.EditUrl("EditTemplate"),
                               false,
                               SecurityAccessLevel.Host,
                               true,
                               false);
                }
                /*
                Actions.Add(ModuleContext.GetNextActionID(),
                           Localization.GetString("EditData.Action", LocalResourceFile),
                           ModuleActionType.EditContent,
                           "",
                           "",
                           ModuleContext.EditUrl("EditData"),
                           false,
                           SecurityAccessLevel.Host,
                           true,
                           false);
                */
                Actions.Add(ModuleContext.GetNextActionID(),
                           Localization.GetString("ShareTemplate.Action", LocalResourceFile),
                           ModuleActionType.ContentOptions,
                           "",
                           "~/DesktopModules/OpenContent/images/exchange.png",
                           ModuleContext.EditUrl("ShareTemplate"),
                           false,
                           SecurityAccessLevel.Host,
                           true,
                           false);

                return Actions;
            }
        }
        public string CurrentCulture
        {
            get
            {
                string lang = LocaleController.Instance.GetCurrentLocale(PortalId).Code.Substring(0, 2);
                return lang + "_" + lang.ToUpper();
            }
        }

        protected void scriptList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModuleController mc = new ModuleController();
            mc.UpdateModuleSetting(ModuleId, "template", scriptList.SelectedValue);
            //InitForm(scriptList.SelectedValue);

        }
    }
}

