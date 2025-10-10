using DinkToPdf;
using DinkToPdf.Contracts;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Infrastructure.Services
{
    public class PdfService:IPdfService
    {
        private readonly IConverter converter;
        public PdfService(IConverter _converter)
        {
            converter = _converter;
        }

        public Task<byte[]> ExportAsPdf(EmployeeProfilePdfModelDto model)
        {
            //Building html from model
            var html = $@"
        <html>
        <head>
        <style>
          body {{ font-family: Arial; }}
          .card {{ border: 1px solid #ccc; padding: 20px; border-radius: 10px; width: 400px; }}
          .avatar {{ width: 100px; height: 100px; border-radius: 50%; }}
        </style>
        </head>
        <body>
          <div class='card'>
            <img class='avatar' src='data:image/png;base64,{model.AvatarBase64}' alt='avatar' />
            <h2>{model.EmpName}</h2>
            <p><b>Role:</b> {model.Role}</p>
            <p><b>Department:</b> {model.Department}</p>
            <p><b>Phone:</b> {model.Phone}</p>
            <p><b>Email:</b> {model.Email}</p>
            <p><b>Joined:</b> {model.JoinedDate.ToString("yyyy-MM-dd")}</p>
          </div>
        </body>
        </html>";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    PaperSize=PaperKind.A4,
                    Orientation=Orientation.Portrait,
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        HtmlContent=html
                    }
                }
            };
            return Task.FromResult(converter.Convert(doc));
        }
    }
}
