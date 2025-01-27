namespace CVB.NET.BuildIntegration.Tools.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            NuspecGenerationTask testTask = new NuspecGenerationTask()
                           {
                               CurrentProjectConfiguration = "Debug",
                               NugetFeedRootDir = @"C:\Users\IBM_ADMIN\Documents\Privat\NugetFeed\",
                               BuildOutputDir = @"C:\Users\IBM_ADMIN\Documents\Privat\CVB.NET.Solution\src\CVB.NET.BuildIntegration.Tools\bin\Debug",
                               SolutionDirectory = @"G:\projects\CVB.NET.Solution\src",
                               ProjectFile =
                                   @"C:\Users\IBM_ADMIN\Documents\Privat\CVB.NET.Solution\src\CVB.NET.BuildIntegration.Tools\CVB.NET.BuildIntegration.Tools.csproj"
                           };

            Assert.IsTrue(testTask.Execute());
        }

        [TestMethod]
        public void TestMethod4()
        {
            NuspecGenerationTask testTask = new NuspecGenerationTask()
                           {
                               CurrentProjectConfiguration = "Debug",
                               NugetFeedRootDir = @"C:\Users\IBM_ADMIN\Documents\Privat\NugetFeed\",
                               BuildOutputDir = @"C:\Users\IBM_ADMIN\Documents\Privat\CVB.NET.Solution\src\CVB.NET.Ui.WindowsForms\bin\Debug",
                               SolutionDirectory = @"G:\projects\CVB.NET.Solution\src",
                               ProjectFile = @"C:\Users\IBM_ADMIN\Documents\Privat\CVB.NET.Solution\src\CVB.NET.Ui.WindowsForms\CVB.NET.Ui.WindowsForms.csproj"
                           };

            Assert.IsTrue(testTask.Execute());
        }

        [TestMethod]
        public void TestMethod2()
        {
            NuspecGenerationTask testTask = new NuspecGenerationTask()
                           {
                               CurrentProjectConfiguration = "Debug",
                               NugetFeedRootDir = @"C:\Users\IBM_ADMIN\Documents\Privat\NugetFeed\",
                               BuildOutputDir = @"C:\Users\IBM_ADMIN\Documents\Privat\CVB.NET.Solution\src\CVB.NET.Utils\bin\Debug",
                               SolutionDirectory = @"G:\projects\CVB.NET.Solution\src",
                               ProjectFile = @"C:\Users\IBM_ADMIN\Documents\Privat\CVB.NET.Solution\src\CVB.NET.Utils\CVB.NET.Utils.csproj"
                           };

            Assert.IsTrue(testTask.Execute());
        }

        [TestMethod]
        public void TestMethod3()
        {
            NuspecGenerationTask testTask = new NuspecGenerationTask()
                           {
                               CurrentProjectConfiguration = "Debug",
                               NugetFeedRootDir = @"C:\Users\IBM_ADMIN\Documents\Privat\NugetFeed\",
                               BuildOutputDir = @"C:\Users\IBM_ADMIN\Documents\Privat\CVB.NET.Solution\src\CVB.NET.Reflection.Caching\bin\Debug",
                               SolutionDirectory = @"G:\projects\CVB.NET.Solution\src",
                               ProjectFile = @"C:\Users\IBM_ADMIN\Documents\Privat\CVB.NET.Solution\src\CVB.NET.Reflection.Caching\CVB.NET.Reflection.Caching.csproj"
                           };

            Assert.IsTrue(testTask.Execute());
        }
    }
}