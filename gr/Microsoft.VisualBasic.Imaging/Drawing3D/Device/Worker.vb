﻿#Region "Microsoft.VisualBasic::b1e4d1dcf511092ad98cea101c1b4cda, gr\Microsoft.VisualBasic.Imaging\Drawing3D\Device\Worker.vb"

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

    '     Class Worker
    ' 
    ' 
    '         Delegate Function
    ' 
    ' 
    '         Delegate Sub
    ' 
    '             Properties: model
    ' 
    '             Constructor: (+1 Overloads) Sub New
    ' 
    '             Function: Run
    ' 
    '             Sub: CreateBuffer, (+2 Overloads) Dispose, Pause, RenderingThread
    ' 
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.Imaging.Drawing3D.Math3D
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Parallel.Tasks

Namespace Drawing3D.Device

    ''' <summary>
    ''' 三维图形设备的工作线程管理器
    ''' </summary>
    Public Class Worker : Inherits IDevice(Of GDIDevice)
        Implements IDisposable
        Implements ITaskDriver

        Public Delegate Function ModelData() As IEnumerable(Of Surface)
        Public Delegate Sub CameraControl(ByRef camera As Camera)

        Dim buffer As IEnumerable(Of Polygon)
        Dim spaceThread As New UpdateThread(20, AddressOf CreateBuffer)
        Dim debugger As Debugger

        Public ReadOnly Property model As ModelData
            Get
                Return device.Model
            End Get
        End Property

        Dim __horizontalPanel As New Surface With {
            .brush = New SolidBrush(Color.FromArgb(128, Color.Gray)),
            .vertices = {
                New Point3D(100, 100),
                New Point3D(100, -100),
                New Point3D(-100, -100),
                New Point3D(-100, 100)
            }
        }

        Public Sub New(dev As GDIDevice)
            MyBase.New(dev)
            debugger = New Debugger(device)
        End Sub

        Private Sub CreateBuffer()
            Dim now& = App.NanoTime

            With device._camera
                Dim surfaces As New List(Of Surface)(model()())

                If device.ShowHorizontalPanel Then
                    surfaces += __horizontalPanel
                End If

                Dim matrix As New Math3D.Matrix(surfaces)
                Dim vector As Vector3D = .Rotate(matrix.Matrix)

                buffer = matrix.TranslateBuffer(
                    device._camera,
                    vector,
                    device.LightIllumination)

                If .angleX > 360 Then
                    .angleX = 0
                End If
                If .angleY > 360 Then
                    .angleY = 0
                End If
                If .angleZ > 360 Then
                    .angleZ = 0
                End If

                Call device.RotationThread.Tick()
            End With

            debugger.BufferWorker = App.NanoTime - now
        End Sub

        ''' <summary>
        ''' 1 frame
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub RenderingThread(sender As Object, e As PaintEventArgs) Handles device.Paint
            Dim canvas As Graphics = e.Graphics
            Dim now& = App.NanoTime

            canvas.CompositingQuality = CompositingQuality.HighQuality
            canvas.InterpolationMode = InterpolationMode.HighQualityBilinear

            With device
                If Not buffer Is Nothing Then
                    Call canvas.Clear(device.bg)
                    Call canvas.BufferPainting(buffer, .DrawPath)
                End If
                If Not .Plot Is Nothing Then
                    Call .Plot()(canvas, ._camera)
                End If
                If device.ShowDebugger Then
                    Call debugger.DrawInformation(canvas)
                End If
            End With

            debugger.RenderingWorker = App.NanoTime - now
        End Sub

        Public Function Run() As Integer Implements ITaskDriver.Run
            Return spaceThread.Start
        End Function

        Public Sub Pause()
            Call spaceThread.Stop()
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    Call spaceThread.Dispose()
                    Call debugger.Dispose()
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace
