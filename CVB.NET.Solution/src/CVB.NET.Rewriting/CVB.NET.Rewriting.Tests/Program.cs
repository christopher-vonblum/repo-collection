using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVB.NET.Rewriting.Compiler.Steps.ILTransformation;

namespace CVB.NET.Rewriting.Tests
{
    public class Program
    {
        public static void Main()
        {
            IlTransformationTask rwrite = new IlTransformationTask();

            rwrite.RootIntermediateAssemblyPath = @"C:\Users\IBM_ADMIN\Documents\Privat\CVB.NET.Solution\src\CVB.NET.Domain.Model\obj\\Debug\CVB.NET.Domain.Model.dll";

            rwrite.Execute();
        }
    }
}
