using Newtonsoft.Json.Linq;
using System.Web;
using System.Web.Http;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using Umbraco.Core;
using Umbraco.Core.PropertyEditors;
using Newtonsoft.Json;
using Umbraco.Web;
using System;
using Umbraco.Core.Logging;
using Umbraco.Core.Strings;

namespace skttl.ParameterEditorGenerator
{
	[PluginController("ParameterEditorGenerator")]
    public class GeneratorController : UmbracoAuthorizedApiController
    {

		private string GetViewPath(string view)
		{
			try
			{
				var viewpath = view;
				if (view.Contains("?")) viewpath = view.Substring(0, view.IndexOf("?"));
				if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(viewpath))) return view;

				viewpath = "~/umbraco/views/propertyeditors/" + view + "/" + view + ".html";
				if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(viewpath))) return viewpath;

			}
			catch (Exception e)
			{

			}

			return view;
		}

        [HttpGet]
		public JObject GetPackageManifestByDataTypeId(int datatypeId)
		{
			var dts = ApplicationContext.Current.Services.DataTypeService;

			var datatype = dts.GetDataTypeDefinitionById(datatypeId);

			var packageManifest = new JObject();

			var propertyEditors = new JArray();

			var propertyEditorManifest = new JObject();

			var propertyEditor = PropertyEditorResolver.Current.GetByAlias(datatype.PropertyEditorAlias);
			var prevalues = dts.GetPreValuesCollectionByDataTypeId(datatypeId);

			propertyEditorManifest["alias"] = "Peg." + datatype.Name.Replace("-", "").Replace("  "," ").ToCleanString(CleanStringType.PascalCase) + "." + datatype.GetUdi().Guid;
			propertyEditorManifest["name"] = datatype.Name;
			propertyEditorManifest["editor"] = new JObject();
			propertyEditorManifest["editor"]["view"] = GetViewPath(propertyEditor.ValueEditor.View);
			propertyEditorManifest["editor"]["hideLabel"] = propertyEditor.ValueEditor.HideLabel;
			propertyEditorManifest["editor"]["valueType"] = propertyEditor.ValueEditor.ValueType;

			propertyEditorManifest["defaultConfig"] = new JObject();

			foreach (var prevalue in prevalues.FormatAsDictionary())
			{
				if (prevalue.Value.Value != null && prevalue.Value.Value.DetectIsJson())
				{
					propertyEditorManifest["defaultConfig"][prevalue.Key] = JsonConvert.DeserializeObject<dynamic>(prevalue.Value.Value);
				}
				else
				{
					propertyEditorManifest["defaultConfig"][prevalue.Key] = prevalue.Value.Value;
				}
			}

			propertyEditorManifest["isParameterEditor"] = true;
			propertyEditorManifest["group"] = "Generated Parameter Editors";
			propertyEditorManifest["icon"] = propertyEditor.Icon;


			propertyEditors.Add(propertyEditorManifest);
			packageManifest["propertyEditors"] = propertyEditors;
			
			return packageManifest;
		}

		[HttpGet]
		public bool FolderExists(string folderName)
		{
			return System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/App_Plugins/" + folderName));
		}

		[HttpPost]
		public bool SavePackageManifest(dynamic data)
		{
			try
			{
				string folderName = data.folderName;
				JObject packageManifest = data.packageManifest;

				if (!FolderExists(folderName))
				{
					System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/App_Plugins/" + folderName));
				}

				var path = HttpContext.Current.Server.MapPath("~/App_Plugins/" + folderName + "/package.manifest");
				System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(packageManifest, Formatting.Indented));
				return System.IO.File.Exists(path);
			}
			catch (Exception ex)
			{
				LogHelper.Error<GeneratorController>("Failed saving package.manifest", ex);
				return false;
			}
		}

		[HttpGet]
		public bool IsDebugMode()
		{
			return UmbracoContext.Current.IsDebug;
		}

		[HttpPost]
		public bool RestartAppPool()
		{
			try
			{
				UmbracoContext.Application.RestartApplicationPool(UmbracoContext.Current.HttpContext);
				return true;
			}
			catch (Exception ex)
			{
				LogHelper.Error<GeneratorController>("Failed restarting app pool", ex);
				return false;
			}
		}
	}
}
