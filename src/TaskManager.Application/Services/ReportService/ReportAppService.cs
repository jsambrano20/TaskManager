using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Interfaces.Services;
using System.IO;
using System.Drawing;

namespace TaskManager.Application.Services.ReportService
{
    public class ReportAppService : IReportAppService
    {
        private readonly ITaskRepository _taskRepository;

        public ReportAppService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public byte[] GeneratePdfReport()
        {
            var reportData = GetCompletedTasksByUserInLast30Days();

            using var stream = new MemoryStream();
            var document = new PdfDocument();
            var page = document.AddPage();
            var graphics = XGraphics.FromPdfPage(page);

            var font = new XFont("Verdana", 12);
            graphics.DrawString("Relatório de Tarefas Concluídas", font, XBrushes.Black, new XPoint(40, 40));

            int yPos = 80;
            graphics.DrawString("Usuário", font, XBrushes.Black, new XPoint(40, yPos));
            graphics.DrawString("Tarefas Concluídas", font, XBrushes.Black, new XPoint(200, yPos));

            yPos += 20;

            foreach (var item in reportData)
            {
                graphics.DrawString(item.Key, font, XBrushes.Black, new XPoint(40, yPos));
                graphics.DrawString(item.Value.ToString(), font, XBrushes.Black, new XPoint(200, yPos));
                yPos += 20;
            }

            document.Save(stream);
            return stream.ToArray();
        }

        private Dictionary<string, int> GetCompletedTasksByUserInLast30Days()
        {
            return _taskRepository.GetCompletedTasksByUserInLast30Days();
        }
    }
}
