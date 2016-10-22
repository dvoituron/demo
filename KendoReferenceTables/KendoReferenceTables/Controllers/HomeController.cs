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
            //Table = new TableDefinition[]
            // {
            //     new TableDefinition()
            //     {
            //         Name = "Employees",
            //         SelectCommand = "SELECT French.CountryCode, French.Label AS French, English.Label AS English, Dutch.Label AS Dutch " +
            //                         "FROM       (SELECT CountryCode, Label FROM CountryTranslate WHERE LanguageCode = 'FR') AS French " +
            //                         "INNER JOIN (SELECT CountryCode, Label FROM CountryTranslate WHERE LanguageCode = 'EN') AS English ON French.CountryCode = English.CountryCode " +
            //                         "INNER JOIN (SELECT CountryCode, Label FROM CountryTranslate WHERE LanguageCode = 'NL') AS Dutch   ON Dutch.CountryCode = English.CountryCode",
            //         Identifiers = new string[] { "CountryCode" },
            //         UpdateCommands = new string[] { "UPDATE EMP SET EMPNO = @empno, ENAME = @ename, JOB = @Job, MGR = @Mgr, HIREDATE = CONVERT(datetime, @Hiredate, 103), SAL = @Sal, COMM = NULLIF(@Comm, ''), DEPTNO = @Deptno, TestB = @TestB WHERE empno = @empno" },
            //         DeleteCommands = new string[] { "DELETE FROM EMP WHERE EMPNO = @empno" },
            //         InsertCommands = new string[] { "INSERT INTO EMP (ENAME, JOB, DEPTNO) VALUES (@ename, @Job, @deptno)" }
            //     }
            // }
        };

        protected override void Initialize(RequestContext requestContext)
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-gb");
            base.Initialize(requestContext);
        }

        public ActionResult Index()
        {
            return View(_references.Read("Countries"));
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(_references.Read("Countries").ToDataSourceResult(request));
        }

        public ActionResult Update([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                _references.Update("Countries", Request.Form);
                return Json(ModelState.ToDataSourceResult());
            }
            catch (Exception ex)
            {
                return this.Json(new DataSourceResult() { Errors = ex.Message });
            }            
        }

        public ActionResult Delete([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                _references.Delete("Countries", Request.Form);
                return Json(ModelState.ToDataSourceResult());
            }
            catch (Exception ex)
            {
                return this.Json(new DataSourceResult() { Errors = ex.Message });
            }
        }

        public ActionResult Add([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                _references.Add("Countries", Request.Form);
                return Json(ModelState.ToDataSourceResult());
            }
            catch (Exception ex)
            {
                return this.Json(new DataSourceResult() { Errors = ex.Message });
            }
        }
    }
}
