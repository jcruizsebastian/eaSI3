using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using TiempoGastado.Models;

namespace TiempoGastado.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public ActionResult Obtener2(TiempoGastado.Models.HomeModel homemodel)
        {
            List<IssueConveter.Model.WorkLog> worklog = ObtenerTrabajo(homemodel);

            return View("Datos", worklog);
        }

        private static List<IssueConveter.Model.WorkLog> ObtenerTrabajo(HomeModel homemodel)
        {
            JiraConnector.JiraWorkLogService jiraWorkLogService = new JiraConnector.JiraWorkLogService("jcruiz", "_*_d1d4ct1c109", "https://jira.openfinance.es/");
            var worklog = jiraWorkLogService.GetWorklog(DateTime.Parse(homemodel.initDate), DateTime.Parse(homemodel.endDate), homemodel.user, homemodel.project);
            return worklog;
        }

        public IActionResult Obtener(TiempoGastado.Models.HomeModel homemodel)
        {
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("My sheet");

            int CuentaColumnas = 0;
            int cuentaFilas = 0;

            IRow row = sheet.CreateRow(cuentaFilas++);
            ICell cell = row.CreateCell(CuentaColumnas++);

            cell.SetCellValue("Author");
            cell = row.CreateCell(CuentaColumnas++);
            cell.SetCellValue("TimeSpent");
            cell = row.CreateCell(CuentaColumnas++);
            cell.SetCellValue("TimeSpentSeconds");
            cell = row.CreateCell(CuentaColumnas++);
            cell.SetCellValue("Comment");
            cell = row.CreateCell(CuentaColumnas++);
            cell.SetCellValue("RecordDate");
            cell = row.CreateCell(CuentaColumnas++);
            cell.SetCellValue("IssueId");
            cell = row.CreateCell(CuentaColumnas++);
            cell.SetCellValue("Key");
            cell = row.CreateCell(CuentaColumnas++);
            cell.SetCellValue("Summary");
            cell = row.CreateCell(CuentaColumnas++);
            cell.SetCellValue("si3ID");
            cell = row.CreateCell(CuentaColumnas++);
            cell.SetCellValue("Type");
            cell = row.CreateCell(CuentaColumnas++);
            cell.SetCellValue("Components");
            cell = row.CreateCell(CuentaColumnas++);
            cell.SetCellValue("Status");
            cell = row.CreateCell(CuentaColumnas++);
            cell.SetCellValue("FixVersions");
            
            foreach(var trabajo in ObtenerTrabajo(homemodel))
            {
                CuentaColumnas = 0;
                row = sheet.CreateRow(cuentaFilas++);

                cell.SetCellValue(trabajo.Author);
                cell = row.CreateCell(CuentaColumnas++);
                cell.SetCellValue(trabajo.TimeSpent);
                cell = row.CreateCell(CuentaColumnas++);
                cell.SetCellValue(trabajo.TimeSpentSeconds);
                cell = row.CreateCell(CuentaColumnas++);
                cell.SetCellValue(trabajo.Comment);
                cell = row.CreateCell(CuentaColumnas++);
                cell.SetCellValue(trabajo.RecordDate);
                cell = row.CreateCell(CuentaColumnas++);
                cell.SetCellValue(trabajo.IssueId);
                cell = row.CreateCell(CuentaColumnas++);
                cell.SetCellValue(trabajo.Key);
                cell = row.CreateCell(CuentaColumnas++);
                cell.SetCellValue(trabajo.Summary);
                cell = row.CreateCell(CuentaColumnas++);
                cell.SetCellValue(trabajo.si3ID);
                cell = row.CreateCell(CuentaColumnas++);
                cell.SetCellValue(trabajo.Type);
                cell = row.CreateCell(CuentaColumnas++);
                cell.SetCellValue(trabajo.Components);
                cell = row.CreateCell(CuentaColumnas++);
                cell.SetCellValue(trabajo.Status);
                cell = row.CreateCell(CuentaColumnas++);
                cell.SetCellValue(trabajo.FixVersions);
            }

            FileStream ms = new FileStream("C:\\SIOC\\ejemplo.xlsx", FileMode.Create, System.IO.FileAccess.Write);
            workbook.Write(ms);
            ms.Close();

            return File(ms, "application/vnd.ms-excel", "trabajo.xlsx");
        }
    }
}
