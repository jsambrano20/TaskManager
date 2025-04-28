using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.Interfaces.Services;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Manager")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportAppService _reportAppService;

        public ReportsController(IReportAppService reportAppService)
        {
            _reportAppService = reportAppService;
        }

        [HttpGet("export/pdf")]
        public IActionResult ExportToPdf()
        {
            var pdfBytes = _reportAppService.GeneratePdfReport();

            return File(pdfBytes, "application/pdf", "Relatorio_Tarefas.pdf");
        }
    }
}
