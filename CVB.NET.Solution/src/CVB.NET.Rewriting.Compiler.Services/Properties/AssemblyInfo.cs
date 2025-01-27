using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CVB.NET.Rewriting.Compiler.Ioc;
using CVB.NET.Rewriting.Compiler.Ioc.Service;
using CVB.NET.Rewriting.Compiler.Services;
using CVB.NET.Rewriting.Compiler.Services.Interfaces;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("CVB.NET.Rewriting.Compiler.ExecutionUnits.Services")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("IBM")]
[assembly: AssemblyProduct("CVB.NET.Rewriting.Compiler.ExecutionUnits.Services")]
[assembly: AssemblyCopyright("Copyright © IBM 2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("7d23fae7-649d-4f69-967f-22bb9cb95d3b")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: ServiceAssembly(typeof(DefaultServicesInstaller))]