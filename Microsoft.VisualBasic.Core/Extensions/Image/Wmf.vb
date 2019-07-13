﻿#Region "Microsoft.VisualBasic::eadbd4efb1d726b1fd5f590f37e528e8, Microsoft.VisualBasic.Core\Extensions\Image\Wmf.vb"

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

    '     Class Wmf
    ' 
    '         Properties: FilePath, Size
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Sub: __release, Dispose, DrawCircle
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging

Namespace Imaging

    ''' <summary>
    ''' Windows Metafile (WMF) is an image file format originally designed for Microsoft Windows in the 1990s. 
    ''' Windows Metafiles are intended to be portable between applications and may contain both vector graphics 
    ''' and bitmap components. It acts in a similar manner to SVG files.
    ''' 
    ''' Essentially, a WMF file stores a list of function calls that have to be issued to the Windows Graphics 
    ''' Device Interface (GDI) layer to display an image on screen. Since some GDI functions accept pointers 
    ''' to callback functions for error handling, a WMF file may erroneously include executable code.
    ''' 
    ''' WMF Is a 16-bit format introduced in Windows 3.0. It Is the native vector format for Microsoft Office 
    ''' applications such as Word, PowerPoint, And Publisher. As of 2015 revision 12 of the Windows Metafile 
    ''' Format specification Is available for online reading Or download as PDF.
    ''' </summary>
    ''' <remarks>
    ''' The original 16 bit WMF file format was fully specified in volume 4 of the 1992 Windows 3.1 SDK documentation
    ''' (at least if combined with the descriptions of the individual functions and structures in the other volumes), 
    ''' but that specification was vague about a few details. These manuals were published as printed books available 
    ''' in bookstores with no click through EULA or other unusual licensing restrictions (just a general warning that 
    ''' if purchased as part of a software bundle, the software would be subject to one).
    ''' 
    ''' Over time the existence Of that historic specification was largely forgotten And some alternative implementations 
    ''' resorted To reverse engineering To figure out the file format from existing WMF files, which was difficult And 
    ''' Error prone. In September 2006, Microsoft again published the WMF file format specification In the context Of 
    ''' the Microsoft Open Specification Promise, promising To Not assert patent rights To file format implementors. 
    ''' </remarks>
    Public Class Wmf : Inherits GDICanvas
        Implements IDisposable

        ReadOnly curMetafile As Metafile
        ReadOnly gSource As Graphics
        ReadOnly hdc As IntPtr

        ''' <summary>
        ''' The file path of the target wmf image file.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property FilePath As String
        Public Overrides ReadOnly Property Size As Size

        ''' <summary>
        ''' The WMF format was designed to be executed by the Windows GDI layer in order to restore the image, but as 
        ''' the WMF binary files contain the definition of the GDI graphic primitives that constitute this image, it is 
        ''' possible to design alternative libraries that render WMF binary files, like the Kyktir application does, or 
        ''' convert them into other graphic formats. For example, the Batik library is able to render WMF files and 
        ''' convert them to their Scalable Vector Graphics (SVG) equivalent. The Vector Graphics package of the FreeHEP 
        ''' Java library allows the saving of Java2D drawings as Enhanced Metafiles (EMF). Inkscape and XnView can export 
        ''' to WMF or EMF.
        ''' </summary>
        ''' <param name="size"></param>
        ''' <param name="save$"></param>
        ''' <param name="backgroundColor$"></param>
        Sub New(size As Size, save$, Optional backgroundColor$ = NameOf(Color.Transparent))
            Dim bitmap As New Bitmap(size.Width, size.Height)

            gSource = Graphics.FromImage(bitmap)
            gSource.Clear(backgroundColor.TranslateColor)

            hdc = gSource.GetHdc()
            size = bitmap.Size
            curMetafile = New Metafile(save, hdc)
            Graphics = Graphics.FromImage(curMetafile)
            Graphics.SmoothingMode = SmoothingMode.HighQuality

            FilePath = save
        End Sub

        Private Sub __release()
            Call gSource.ReleaseHdc(hdc)
            Call Graphics.Dispose()
            Call gSource.Dispose()
        End Sub

        Public Overrides Sub Dispose()
            Call __release()
            MyBase.Dispose()
        End Sub

        Public Overrides Sub DrawCircle(center As PointF, fill As Color, stroke As Pen, radius As Single)
            Throw New NotImplementedException()
        End Sub
    End Class
End Namespace
