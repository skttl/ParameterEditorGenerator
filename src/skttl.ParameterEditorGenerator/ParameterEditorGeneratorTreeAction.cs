using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Trees;

namespace skttl.ParameterEditorGenerator
{

	public class ParameterEditorGeneratorTreeAction : ApplicationEventHandler
	{
		protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
		{
			TreeControllerBase.MenuRendering += TreeControllerBase_MenuRendering;
		}

		void TreeControllerBase_MenuRendering(TreeControllerBase sender, MenuRenderingEventArgs e)
		{
			var textService = sender.ApplicationContext.Services.TextService;

			var user = UmbracoContext.Current.Security.CurrentUser;

			if (e.Menu != null && sender.TreeAlias == "dataTypes" && e.NodeId.StartsWith("-") == false)
			{

				var label = textService.Localize("skttlParameterEditorGenerator/generatePackageManifest", CultureInfo.CurrentCulture);
				e.Menu.Items.Insert(0, new Umbraco.Web.Models.Trees.MenuItem("generatePackageManifest", label) { Icon = "icon icon-autofill", SeperatorBefore = true, AdditionalData = {
						{ "actionView", "/App_Plugins/skttl.ParameterEditorGenerator/view.html" },
						{ "dataTypeId", e.NodeId }
					} });
			}
		}
	}

}
