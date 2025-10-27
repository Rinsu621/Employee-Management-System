using EmployeeManagementSystem.Application.EmployeeModule.Dtos;

namespace EmployeeManagementSystem.Application.Interface
{
    public interface IPdfService
    {
        byte[] GeneratePdf<TDocument>(TDocument document) where TDocument : QuestPDF.Infrastructure.IDocument;
    }
}
