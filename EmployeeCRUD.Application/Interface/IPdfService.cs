using EmployeeCRUD.Application.EmployeeModule.Dtos;

namespace EmployeeCRUD.Application.Interface
{
    public interface IPdfService
    {
        byte[] GeneratePdf<TDocument>(TDocument document) where TDocument : QuestPDF.Infrastructure.IDocument;
    }
}
