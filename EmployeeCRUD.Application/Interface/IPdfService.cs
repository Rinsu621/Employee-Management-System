using EmployeeCRUD.Application.EmployeeModule.Dtos;
using MigraDoc.DocumentObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Interface
{
    public interface IPdfService
    {
        byte[] GeneratePdf<T>(T data, Action<Document, T> renderAction);
    }
}
