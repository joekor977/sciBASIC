﻿#Region "Microsoft.VisualBasic::1ddfaed037c4777658a426ce710f2f53, gr\Microsoft.VisualBasic.Imaging\SVG\XML\Image.vb"

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

    '     Class Image
    ' 
    '         Properties: data, height, width, x, y
    '                     zIndex
    ' 
    '         Constructor: (+4 Overloads) Sub New
    '         Function: GetGDIObject, SaveAs, ToString
    '         Operators: +
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.FileIO
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.MIME.Markup.HTML.CSS
Imports Microsoft.VisualBasic.Net.Http

Namespace SVG.XML

    ''' <summary>
    ''' Does SVG support embedding of bitmap images?
    ''' 
    ''' + http://stackoverflow.com/questions/6249664/does-svg-support-embedding-of-bitmap-images
    ''' </summary>
    Public Class Image : Implements CSSLayer

        <XmlAttribute> Public Property x As Single
        <XmlAttribute> Public Property y As Single
        <XmlAttribute> Public Property width As String
        <XmlAttribute> Public Property height As String

        <XmlAttribute("href", [Namespace]:=SVGWriter.Xlink)>
        Public Property data As String
        <XmlAttribute("z-index")>
        Public Property zIndex As Integer Implements CSSLayer.zIndex

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function GetGDIObject() As Bitmap
            Return Base64Codec.GetImage(DataURI.URIParser(data).base64)
        End Function

        Sub New()
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Sub New(image As Bitmap, Optional size As Size = Nothing)
            Call Me.New(image, New SizeF(size.Width, size.Height))
        End Sub

        Sub New(image As Drawing.Image, Optional size As SizeF = Nothing)
            data = New DataURI(image).ToString

            With size Or image.Size.SizeF.AsDefault(Function() size.IsEmpty)
                width = .Width
                height = .Height
            End With
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Sub New(url$, Optional size As Size = Nothing)
            Call Me.New(MapNetFile(url).LoadImage, size)
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overrides Function ToString() As String
            Return $"<image x=""{x}"" y=""{y}"" width=""{width}"" height=""{height}"" xlink:href=""{data}"">"
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function SaveAs(fileName$, Optional format As ImageFormats = ImageFormats.Png) As Boolean
            Return GetGDIObject.SaveAs(fileName, format)
        End Function

        Public Shared Operator +(img As Image, offset As PointF) As Image
            img = DirectCast(img.MemberwiseClone, Image)
            img.x += offset.X
            img.y += offset.Y
            Return img
        End Operator
    End Class
End Namespace
