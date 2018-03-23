using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;
using Umbraco.Core;
using Umbraco.Core.Security;
using Umbraco.Web.Trees;

namespace CareerApplicationForm.WebSite.CustomSettings
{
    public class CustomApplicationStarted : ApplicationEventHandler
    {
        //system.Configuration.ConfigurationManager.AppSettings["UmbracoCustomButtons.UserType"]

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            TreeControllerBase.MenuRendering += TreeControllerBase_MenuRendering;

        }

        private void TreeControllerBase_MenuRendering(TreeControllerBase sender, MenuRenderingEventArgs e)
        {
              var _umbracoHelper = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);
        //check if the User Type is vailed to view the New Added Buttons 
        //the Alias string is put in the webconfig in case you want to change it later 
        var ValidType = internalCurrentUser().Groups.SingleOrDefault(y=>y.Alias== System.Configuration.ConfigurationManager.AppSettings["UmbracoCustomButtons.UserType"]);
            if (ValidType == null)
            {
                return;
            }
            var TargtedNode = _umbracoHelper.TypedContent(e.NodeId);


            //check if the User Type is vailed to view the New Added Buttons 
            //the Alias string is put in the webconfig in case you want to change it later 
            if (TargtedNode == null || TargtedNode.DocumentTypeAlias!= System.Configuration.ConfigurationManager.AppSettings["UmbracoCustomButtons.TargetDocumentType"])
            {
                return;
            }

            

            switch (sender.TreeAlias)
            {
                //show those on content section only
                case "content":
                    
                        
                        var excelMenuItem = new Umbraco.Web.Models.Trees.MenuItem("itemAlias", "Export As Excel");
                        var mailingMenuItem = new Umbraco.Web.Models.Trees.MenuItem("itemAlias", "Send Mail");

                        //sets the view html paths
                        excelMenuItem.AdditionalData.Add("actionView", "/App_Plugins/ExportToExcel/ExportToExcel.html");
                        mailingMenuItem.AdditionalData.Add("actionView", "/App_Plugins/SendMail/SendMail.html");

                        //sets the icons
                        excelMenuItem.Icon = "download";
                        mailingMenuItem.Icon = "message";

                         
                        //insert at index 5
                        e.Menu.Items.Insert(5, excelMenuItem);
                        //insert at index 6
                        e.Menu.Items.Insert(6, mailingMenuItem);

                    break;
            }
        }

        private Umbraco.Core.Models.Membership.IUser internalCurrentUser()
        {
            var userService = Umbraco.Core.ApplicationContext.Current.Services.UserService;

            // Snippet from: http://issues.umbraco.org/issue/U4-6342#comment=67-19466
            var httpCtxWrapper = new System.Web.HttpContextWrapper(System.Web.HttpContext.Current);
            var umbTicket = httpCtxWrapper.GetUmbracoAuthTicket();

            if (umbTicket == null || umbTicket.Name.IsEmpty() || umbTicket.Expired) return null;

            var user = userService.GetByUsername(umbTicket.Name);
            return user;
        }
    }
}