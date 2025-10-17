
using EmployeeCRUD.Application.EmployeeModule.Document;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        static PdfService()
        {
            // Set the license type to Community for free use
            QuestPDF.Settings.License = LicenseType.Community;
        }
        public byte[] GeneratePdf<TDocument>(TDocument document) where TDocument : IDocument
        {
            //document.ShowInCompanion();
            return document.GeneratePdf();
        }
    }
}
