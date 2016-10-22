using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Apps72.Dev.Data;
using KendoUIMVC5.ReferenceTablesEditor;
using System.Web.Routing;
using System.Threading;
using System.Globalization;

namespace KendoUIMVC5.Controllers
{
    public class HomeController : Controller
    {
        ReferenceTables _references = new ReferenceTables()
        {
            ConnectionString = "Server=(localdb)\\ProjectsV12;Database=TrasysSkillsManager;Trusted_Connection=True;",
        };

        protected override void Initialize(RequestContext requestContext)
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-gb");
            base.Initialize(requestContext);
        }

        public ActionResult GetAllTables()
        {
            return Json(_references.Table.Select(t => t.Name), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            var model = new ReferenceTablesModel()
            {
                SelectedTableName = this.SelectedTableName,
                DataTable = _references.Read(this.SelectedTableName)
            };
            return View(model);
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request, string tableName)
        {
            return Json(_references.Read(tableName).ToDataSourceResult(request));
        }

        public ActionResult Update([DataSourceRequest] DataSourceRequest request, string tableName)
        {
            try
            {
                _references.Update(tableName, Request.Form);
                return Json(ModelState.ToDataSourceResult());
            }
            catch (Exception ex)
            {
                return this.Json(new DataSourceResult() { Errors = ex.Message });
            }            
        }

        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, string tableName)
        {
            try
            {
                _references.Delete(tableName, Request.Form);
                return Json(ModelState.ToDataSourceResult());
            }
            catch (Exception ex)
            {
                return this.Json(new DataSourceResult() { Errors = ex.Message });
            }
        }

        public ActionResult Add([DataSourceRequest] DataSourceRequest request, string tableName)
        {
            try
            {
                _references.Add(tableName, Request.Form);
                return Json(ModelState.ToDataSourceResult());
            }
            catch (Exception ex)
            {
                return this.Json(new DataSourceResult() { Errors = ex.Message });
            }
        }

        private string SelectedTableName
        {
            get
            {
                string name = Request.Form["cboListOfReferenceTables"];
                return string.IsNullOrEmpty(name) ? _references.Table.First().Name : name;
            }
        }
    }

    public class ReferenceTablesModel
    {
        public DataTable DataTable { get; set; }

        public string SelectedTableName { get; set; }
    }
}
