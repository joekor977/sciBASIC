﻿Imports System.Drawing
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing3D
Imports Microsoft.VisualBasic.Imaging.Drawing3D.Models.Isometric.Shapes

''' <summary>
''' sciBASIC framework logo generator. A demo for the sciBASIC graphics artist system.
''' </summary>
Module Program

    Const save$ = "./logo.png"

    Sub Main()
        Using g As Graphics2D = New Size(500, 300).CreateGDIDevice(filled:=Color.White)

            Dim isometricView As New IsometricEngine

            isometricView.Add(New Prism(New Point3D(0, 0, 0), 1, 1, 1), Color.FromArgb(33, 150, 243))
            isometricView.Draw(g)

            Call g.DrawString("sci", New Font(FontFace.SegoeUI, 24), Brushes.CadetBlue, New PointF(200, 200))
            Call g.DrawString("BASIC#", New Font(FontFace.SegoeUI, 36), Brushes.DeepSkyBlue, New PointF(300, 100))
            Call g.DrawString("http://sciBASIC.NET", New Font(FontFace.SegoeUI, 24), Brushes.Blue, New PointF(250, 200))

            Call g.ImageResource.SaveAs(path:=save, format:=ImageFormats.Png)
        End Using

        'Call save _
        '    .LoadImage _
        '    .ColorReplace(Color.White, Color.Transparent) _
        '    .SaveAs(save)
    End Sub
End Module
