﻿using System.Reflection;
using System.Runtime.InteropServices;

// Allgemeine Informationen über eine Assembly werden über folgende 
// Attribute gesteuert. Ändern Sie diese Attributwerte, um die Informationen zu ändern,
// die einer Assembly zugeordnet sind.

[assembly: AssemblyTitle("CVB.NET.Aspects.Tests")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("CVB.NET.Aspects.Tests")]
[assembly: AssemblyCopyright("Copyright © Microsoft 2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Wenn ComVisible auf "false" festgelegt wird, sind die Typen innerhalb dieser Assembly 
// für COM-Komponenten unsichtbar. Wenn Sie auf einen Typ in dieser Assembly von 
// COM aus zugreifen müssen, sollten Sie das ComVisible-Attribut für diesen Typ auf "True" festlegen.

[assembly: ComVisible(false)]

// Die folgende GUID bestimmt die ID der Typbibliothek, wenn dieses Projekt für COM verfügbar gemacht wird

[assembly: Guid("9386c1d0-0e11-442a-9d31-f6beb517446f")]

// Versionsinformationen für eine Assembly bestehen aus den folgenden vier Werten:
//
//  Hauptversion
//  Nebenversion 
//  Buildnummer
//  Revision
//
// Sie können alle Werte angeben oder die standardmäßigen Build- und Revisionsnummern 
// übernehmen, indem Sie "*" eingeben:
// [Assembly: AssemblyVersion("1.0.*")]
#if (DEBUG)

[assembly: AssemblyVersion("1.0.0")]
#endif
#if !(DEBUG)
[assembly: AssemblyVersion("1.0.1.*")]
#endif

[assembly: AssemblyFileVersion("1.0.0")]