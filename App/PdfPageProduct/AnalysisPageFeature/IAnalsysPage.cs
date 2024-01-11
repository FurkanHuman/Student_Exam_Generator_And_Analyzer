using Entity.Entities.Mains;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace App.PdfPageProduct.AnalysisPageFeature;

public interface IAnalsysPage
{
    IDocument PageGenerate(AnalysisHeader analysisHeader);
}
