//-----------------------------------------------------------------------
//
//  Microsoft Windows Client Platform
//  Copyright (C) Microsoft Corporation
//
//  File:      ITextMetrics.cs
//
//  Contents:  Layout measurement of formatted text up to the point where text is
//             broken by line breaking.
//
//  Spec:      http://team/sites/Avalon/Specs/Text%20Formatting%20API.doc
//
//  Created:   1-20-2005 Worachai Chaoweeraprasit (wchao)
//
//------------------------------------------------------------------------


using System;
using System.Windows.Media.TextFormatting;
using System.Runtime.InteropServices;

namespace MS.Internal.TextFormatting
{
    /// <summary>
    /// Measurement of formatted text up to the point where text is broken
    /// by the line breaking process
    /// </summary>
    internal interface ITextMetrics
    {
        /// <summary>
        /// Client to get the number of text source positions of this line
        /// </summary>
        int Length 
        { get; }


        /// <summary>
        /// Client to get the number of characters following the last character 
        /// of the line that may trigger reformatting of the current line.
        /// </summary>
        int DependentLength 
        { get; }


        /// <summary>
        /// Client to get the number of newline characters at line end
        /// </summary>
        int NewlineLength 
        { get; }


        /// <summary>
        /// Client to get distance from paragraph start to line start
        /// </summary>
        double Start 
        { get; }


        /// <summary>
        /// Client to get the total width of this line
        /// </summary>
        double Width 
        { get; }


        /// <summary>
        /// Client to get the total width of this line including width of whitespace characters at the end of the line.
        /// </summary>
        double WidthIncludingTrailingWhitespace 
        { get; }


        /// <summary>
        /// Client to get the height of the line
        /// </summary>
        double Height 
        { get; }


        /// <summary>
        /// Client to get the overall height of the list items marker of the line if any.
        /// </summary>
        double MarkerHeight 
        { get; }


        /// <summary>
        /// Client to get the distance from top to baseline of this text line
        /// </summary>
        double Baseline 
        { get; }


        /// <summary>
        /// Client to get the distance from the before edge of line height 
        /// to the baseline of marker of the line if any.
        /// </summary>
        double MarkerBaseline 
        { get; }
    }
}

