﻿using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("CVB.NET.TorNetwork")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("CVB.NET.TorNetwork")]
[assembly: AssemblyCopyright("Copyright © Microsoft 2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components. If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly: Guid("daa212a5-7b3c-497c-a1e9-3939ca6bba67")]

// Version information for an assembly consists of the following four values:
//
//  Major Version
//  Minor Version 
//  Build Number
//  Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
#if (DEBUG)

[assembly: AssemblyVersion("1.0.0")]
#endif
#if !(DEBUG)
[assembly: AssemblyVersion("1.0.1.*")]
#endif

[assembly: AssemblyFileVersion("1.0.0")]