cd %~dp0
set MSBUILDEMITSOLUTION=1
C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe "G:\projects\MsBuildExtension\MsBuildExtension.sln" /t:CompilationTest /p:Configuration="Debug" /p:Platform="Any CPU" /p:BuildProjectReferences=false /debug