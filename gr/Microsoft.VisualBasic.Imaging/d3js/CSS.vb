﻿#Region "Microsoft.VisualBasic::81d378a52bec403ef725f8547eac0250, gr\Microsoft.VisualBasic.Imaging\d3js\CSS.vb"

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

    '     Class DirectedForceGraph
    ' 
    '         Properties: link, node, text
    ' 
    '         Function: ToString
    ' 
    '     Class Font
    ' 
    '         Properties: color, font, font_size
    ' 
    '         Function: ToString
    ' 
    '     Class CssValue
    ' 
    '         Properties: opacity, stroke, strokeOpacity, strokeWidth
    ' 
    '         Function: ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.MIME.Markup.HTML

Namespace d3js.SVG.CSS

    ''' <summary>
    ''' Style generator for the value of <see cref="XmlMeta.CSS.style"/>
    ''' </summary>
    Public Class DirectedForceGraph

        Public Property node As CssValue
        Public Property link As CssValue
        Public Property text As Font

        Public Overrides Function ToString() As String
            Dim sb As New StringBuilder

            Call sb.AppendLine(".node {")
            Call sb.AppendLine(node.ToString)
            Call sb.AppendLine("}")
            Call sb.AppendLine(".link {")
            Call sb.AppendLine(link.ToString)
            Call sb.AppendLine("}")
            Call sb.AppendLine(".text {")
            Call sb.AppendLine(text.ToString)
            Call sb.AppendLine("}")

            Return sb.ToString
        End Function
    End Class

    Public Class Font

        Public Property font As String = FontFace.MicrosoftYaHei
        Public Property color As String = "gray"

        <Field("font-size")>
        Public Property font_size As Integer = 10

        Public Overrides Function ToString() As String
            Dim sb As New StringBuilder

            If Not String.IsNullOrEmpty(font) Then
                sb.AppendLine("font: " & font)
            End If
            If Not String.IsNullOrEmpty(color) Then
                sb.AppendLine("color: " & color)
            End If
            If font_size <> 0 Then
                sb.AppendLine("font-size: " & font_size)
            End If

            Return sb.ToString
        End Function
    End Class

    Public Class CssValue

        <DataFrameColumn> Public Property stroke As String
        <DataFrameColumn("stroke-width")> Public Property strokeWidth As String
        <DataFrameColumn("stroke-opacity")> Public Property strokeOpacity As String
        <DataFrameColumn> Public Property opacity As String

        Public Overrides Function ToString() As String
            Dim sb As New StringBuilder

            For Each prop In DataFrameColumnAttribute.LoadMapping(Of CssValue).Values
                Dim value As Object = prop.GetValue(Me)
                If Not value Is Nothing Then
                    Call sb.AppendLine("    " & $"{prop.Identity}: {Scripting.ToString(value)};")
                End If
            Next

            Return sb.ToString
        End Function
    End Class
End Namespace
