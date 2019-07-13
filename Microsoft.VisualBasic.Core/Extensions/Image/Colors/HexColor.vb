﻿#Region "Microsoft.VisualBasic::ef41705e254d691dbb8f24e0267176a3, Microsoft.VisualBasic.Core\Extensions\Image\Colors\HexColor.vb"

    ' Author:
    ' 
    ' 
    ' 
    ' 
    ' 
    ' 
    ' 



    ' /********************************************************************************/

    ' Summaries:

    '     Module HexColor
    ' 
    '         Function: ConvertToRbg, HexToColor, hexToRbgNew, OLE
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Globalization

Namespace Imaging

    ''' <summary>
    ''' Convert hex color string to RGB color
    ''' </summary>
    ''' <remarks>http://stackoverflow.com/questions/13356486/convert-hex-color-string-to-rgb-color</remarks>
    Public Module HexColor

        Public Function ConvertToRbg(HexColor As String) As Color
            Dim Red As String
            Dim Green As String
            Dim Blue As String
            HexColor = HexColor.Trim("#"c)
            '"&H" &
            Red = GetHexInteger(Mid(HexColor, 1, 2))
            Green = GetHexInteger(Mid(HexColor, 3, 2))
            Blue = GetHexInteger(Mid(HexColor, 5, 2))
            Return Color.FromArgb(Integer.Parse(Red), Integer.Parse(Green), Integer.Parse(Blue))
        End Function

        Public Function HexToColor(hexColor As String) As Color
            If hexColor.IndexOf("#"c) <> -1 Then
                hexColor = hexColor.Replace("#", "")
            End If
            Dim red As Integer = 0
            Dim green As Integer = 0
            Dim blue As Integer = 0
            If hexColor.Length = 6 Then
                red = Integer.Parse(hexColor.Substring(0, 2), NumberStyles.AllowHexSpecifier)
                green = Integer.Parse(hexColor.Substring(2, 2), NumberStyles.AllowHexSpecifier)
                blue = Integer.Parse(hexColor.Substring(4, 2), NumberStyles.AllowHexSpecifier)
            ElseIf hexColor.Length = 3 Then
                red = Integer.Parse(hexColor(0).ToString() + hexColor(0).ToString(), NumberStyles.AllowHexSpecifier)
                green = Integer.Parse(hexColor(1).ToString() + hexColor(1).ToString(), NumberStyles.AllowHexSpecifier)
                blue = Integer.Parse(hexColor(2).ToString() + hexColor(2).ToString(), NumberStyles.AllowHexSpecifier)
            End If
            Return Color.FromArgb(red, green, blue)
        End Function

        Public Function hexToRbgNew(Hex As String) As Color
            Hex = Strings.Replace(Hex, "#", "")
            Dim red As String = "&H" & Hex.Substring(0, 2)
            Hex = Strings.Replace(Hex, red, "", , 1)
            Dim green As String = "&H" & Hex.Substring(0, 2)
            Hex = Strings.Replace(Hex, green, "", , 1)
            Dim blue As String = "&H" & Hex.Substring(0, 2)
            Hex = Strings.Replace(Hex, blue, "", , 1)
            Return Color.FromArgb(red, green, blue)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="c">Example: ``#ffffff``</param>
        ''' <returns></returns>
        Public Function OLE(c As String) As Color
            c = Strings.Replace(c, "#", "")
            c = "&H" & c
            ColorTranslator.FromOle(c)
        End Function
    End Module
End Namespace
