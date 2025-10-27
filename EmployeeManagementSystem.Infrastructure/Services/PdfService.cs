
using EmployeeManagementSystem.Application.EmployeeModule.Document;
using EmployeeManagementSystem.Application.EmployeeModule.Dtos;
using EmployeeManagementSystem.Application.Interface;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Infrastructure.Services
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
