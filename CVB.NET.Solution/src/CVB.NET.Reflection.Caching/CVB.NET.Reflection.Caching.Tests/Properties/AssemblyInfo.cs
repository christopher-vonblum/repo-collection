﻿using System.Reflection;
using System.Runtime.InteropServices;

// Allgemeine Informationen über eine Assembly werden über folgende 
// Attribute gesteuert. Ändern Sie diese Attributwerte, um die Informationen zu ändern,
// die einer Assembly zugeordnet sind.

[assembly: AssemblyTitle("CVB.NET.ReflectionCaching.Tests")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("CVB.NET.ReflectionCaching.Tests")]
[assembly: AssemblyCopyright("Copyright © Microsoft 2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Wenn ComVisible auf "false" festgelegt wird, sind die Typen innerhalb dieser Assembly 
// für COM-Komponenten unsichtbar. Wenn Sie auf einen Typ in dieser Assembly von 
// COM aus zugreifen müssen, sollten Sie das ComVisible-Attribut für diesen Typ auf "True" festlegen.

[assembly: ComVisible(false)]

// Die folgende GUID bestimmt die ID der Typbibliothek, wenn dieses Projekt für COM verfügbar gemacht wird

[assembly: Guid("c856c51a-b0f0-4ff3-b071-1f1aba66b004")]

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