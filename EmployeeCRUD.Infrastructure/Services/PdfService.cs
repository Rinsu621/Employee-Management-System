
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Infrastructure.Services
{
    public class PdfService:IPdfService
    {
        public byte[] GeneratePdf<T>(T data, Action<Document, T> renderAction)
        {
            var document = new Document();
            renderAction(document, data);

            var pdfRenderer = new PdfDocumentRenderer(true) // true = embed fonts
            {
                Document = document
            };

            pdfRenderer.RenderDocument();

            using var stream = new MemoryStream();
            pdfRenderer.PdfDocument.Save(stream, false);
            return stream.ToArray();
        }


        }
}
