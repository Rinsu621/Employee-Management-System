using EmployeeCRUD.Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.GeminiModule.Queries
{
    public record GenerateGeminiResponseQuery(string Prompt):IRequest<string>;

    public class GenerateGeminiResponseQueryHandler : IRequestHandler<GenerateGeminiResponseQuery, string>
    {
        private readonly IGeminiService geminiService;
        public GenerateGeminiResponseQueryHandler(IGeminiService _geminiService)
        {
            geminiService = _geminiService;
        }
        public async Task<string> Handle(GenerateGeminiResponseQuery request, CancellationToken cancellationToken)
        {
            return await geminiService.GenerateResponseAsync(request.Prompt);
        }
    }
}
