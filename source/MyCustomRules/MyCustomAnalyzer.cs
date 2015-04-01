using StyleCop;
using StyleCop.CSharp;

namespace MyCustomRules
{
    /// <summary>
    /// Custom analyzer for demo purposes. This must be targeted at .NET 3.5 to match the StyleCop assembly
    /// Also the XML file must match this file name, else the rule does not run
    /// See http://stylecopplus.codeplex.com/wikipage?title=How%20to%20Create%20StyleCop%20Custom%20Rule for details
    /// on this sample
    /// </summary>
    [SourceAnalyzer(typeof(CsParser))]
    public class MyCustomAnalyzer : SourceAnalyzer
    {
        /// <summary>
        /// Extremely simple analyzer for demo purposes.
        /// </summary>
        public override void AnalyzeDocument(CodeDocument document)
        {
            CsDocument doc = (CsDocument)document;

            // skipping wrong or auto-generated documents
            if (doc.RootElement == null || doc.RootElement.Generated)
                return;

            // check all class entries
            doc.WalkDocument(CheckClasses);
        }

        /// <summary>
        /// Checks whether specified element conforms custom rule CR0001.
        /// </summary>
        private bool CheckClasses(
            CsElement element,
            CsElement parentElement,
            object context)
        {
            // if current element is not a class then continue walking
            if (element.ElementType != ElementType.Class)
                return true;

            // check whether class name contains "a" letter
            Class classElement = (Class)element;
            if (classElement.Declaration.Name.Contains("a"))
            {
                // add violation
                // (note how custom message arguments could be used)
                AddViolation(
                    classElement,
                    classElement.Location,
                    "AvoidUsingAInClassNames",
                    classElement.FriendlyTypeText);
            }

            // continue walking in order to find all classes in file
            return true;
        }
    }
}