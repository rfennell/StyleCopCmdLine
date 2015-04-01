using StyleCop;
using StyleCop.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomStyleCopRule
{
    [SourceAnalyzer(typeof(CsParser))]
    public class AddinRule : SourceAnalyzer
    {
        public AddinRule()
        {
        }

        public override void AnalyzeDocument(CodeDocument document)
        {
            Param.RequireNotNull(document, "document");
            CsDocument csDocument = (CsDocument)document;
            if (csDocument.RootElement == null || csDocument.RootElement.Generated)
            {
                return; // don't check auto generated
            }
            else
            {
                // in this sample always throw a violation
                base.AddViolation(csDocument.RootElement, "MyCustomRule", new object[0]);
            }
        }
    }
}

