﻿using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("CVB.NET.DataAccess")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("CVB.NET.DataAccess")]
[assembly: AssemblyCopyright("Copyright © Microsoft 2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components. If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly: Guid("9ed0b5af-8009-4fcd-b7e7-dcb5d1479f5d")]

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