using Application.Services.AIService;
using Application.Services.AIService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Adapters.AIService.Google;

internal class GoogleBatchServiceAdapter : IAIBatchService
{
    public Task<List<AIAnalysisResponse>> GenerateBatchAnalysisAsync(List<AIAnalysisRequest> requests, string aiModel, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("this method cant be written");
    }
}
