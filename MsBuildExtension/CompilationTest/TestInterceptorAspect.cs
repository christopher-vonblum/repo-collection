using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Aspects;

namespace CompilationTest
{
    public class TestInterceptorAspect : LocationInterceptionAspect
    {
        public override void OnGetValue(LocationInterceptionArgs args)
        {
            Console.WriteLine(args.LocationFullName + " got called");
        }
    }
}
