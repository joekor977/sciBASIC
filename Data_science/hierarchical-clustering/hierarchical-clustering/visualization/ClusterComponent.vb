Imports System.Drawing
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Vector.Text

'
'*****************************************************************************
' Copyright 2013 Lars Behnke
' 
' Licensed under the Apache License, Version 2.0 (the "License");
' you may not use this file except in compliance with the License.
' You may obtain a copy of the License at
' 
'   http://www.apache.org/licenses/LICENSE-2.0
' 
' Unless required by applicable law or agreed to in writing, software
' distributed under the License is distributed on an "AS IS" BASIS,
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' See the License for the specific language governing permissions and
' limitations under the License.
' *****************************************************************************
'

Namespace com.apporiented.algorithm.clustering.visualization




    Public Class ClusterComponent
        Implements Paintable

        Public Overridable Property Children As IList(Of ClusterComponent)
        Public Overridable Property NamePadding As Integer = 6
        Public Overridable Property DotRadius As Integer = 2
        Public Overridable Property LinkPoint As VCoord
        Public Overridable Property InitPoint As VCoord
        Public Overridable Property Cluster As com.apporiented.algorithm.clustering.Cluster
        Public Overridable Property PrintName As Boolean

        Public Sub New(cluster As com.apporiented.algorithm.clustering.Cluster, printName As Boolean, initPoint As VCoord)
            Me.PrintName = printName
            Me.Cluster = cluster
            Me.InitPoint = initPoint
            Me.LinkPoint = initPoint
        End Sub

        Public Sub paint(g As Graphics2D, xDisplayOffset As Integer, yDisplayOffset As Integer, xDisplayFactor As Double, yDisplayFactor As Double, decorated As Boolean) Implements Paintable.paint
            Dim x1, y1, x2, y2 As Integer
            Dim fontMetrics As FontMetrics = g.FontMetrics
            x1 = CInt(Fix(InitPoint.X * xDisplayFactor + xDisplayOffset))
            y1 = CInt(Fix(InitPoint.Y * yDisplayFactor + yDisplayOffset))
            x2 = CInt(Fix(LinkPoint.X * xDisplayFactor + xDisplayOffset))
            y2 = y1
            g.FillEllipse(Brushes.Black, x1 - DotRadius, y1 - DotRadius, DotRadius * 2, DotRadius * 2)
            g.DrawLine(Pens.Black, x1, y1, x2, y2)

            If Cluster.Leaf Then g.DrawString(Cluster.Name, fontMetrics, Brushes.Black, x1 + NamePadding, y1 + (fontMetrics.Height / 2) - 2)
            If decorated AndAlso Cluster.Distance IsNot Nothing AndAlso (Not Cluster.Distance.NaN) AndAlso Cluster.Distance.Distance > 0 Then
                Dim s As String = String.Format("{0:F2}", Cluster.Distance)
                Dim rect As RectangleF = fontMetrics.GetStringBounds(s, g.Graphics)
                g.DrawString(s, fontMetrics, Brushes.Black, x1 - CInt(Fix(rect.Width)), y1 - 2)
            End If

            x1 = x2
            y1 = y2
            y2 = CInt(Fix(LinkPoint.Y * yDisplayFactor + yDisplayOffset))
            g.DrawLine(Pens.Black, x1, y1, x2, y2)


            For Each child As ClusterComponent In Children
                child.paint(g, xDisplayOffset, yDisplayOffset, xDisplayFactor, yDisplayFactor, decorated)
            Next child
        End Sub

        Public Overridable ReadOnly Property RectMinX As Double
            Get

                ' TODO Better use closure / callback here
                Debug.Assert(InitPoint IsNot Nothing AndAlso LinkPoint IsNot Nothing)
                Dim val As Double = Math.Min(InitPoint.X, LinkPoint.X)
                For Each child As ClusterComponent In Children
                    val = Math.Min(val, child.RectMinX)
                Next child
                Return val
            End Get
        End Property

        Public Overridable ReadOnly Property RectMinY As Double
            Get

                ' TODO Better use closure here
                Debug.Assert(InitPoint IsNot Nothing AndAlso LinkPoint IsNot Nothing)
                Dim val As Double = Math.Min(InitPoint.Y, LinkPoint.Y)
                For Each child As ClusterComponent In Children
                    val = Math.Min(val, child.RectMinY)
                Next child
                Return val
            End Get
        End Property

        Public Overridable ReadOnly Property RectMaxX As Double
            Get

                ' TODO Better use closure here
                Debug.Assert(InitPoint IsNot Nothing AndAlso LinkPoint IsNot Nothing)
                Dim val As Double = Math.Max(InitPoint.X, LinkPoint.X)
                For Each child As ClusterComponent In Children
                    val = Math.Max(val, child.RectMaxX)
                Next child
                Return val
            End Get
        End Property

        Public Overridable ReadOnly Property RectMaxY As Double
            Get

                ' TODO Better use closure here
                Debug.Assert(InitPoint IsNot Nothing AndAlso LinkPoint IsNot Nothing)
                Dim val As Double = Math.Max(InitPoint.Y, LinkPoint.Y)
                For Each child As ClusterComponent In Children
                    val = Math.Max(val, child.RectMaxY)
                Next child
                Return val
            End Get
        End Property

        Public Overridable Function getNameWidth(g As Graphics2D, includeNonLeafs As Boolean) As Integer
            Dim width As Integer = 0
            If includeNonLeafs OrElse Cluster.Leaf Then
                Dim rect As RectangleF = g.FontMetrics.GetStringBounds(Cluster.Name, g.Graphics)
                width = CInt(Fix(rect.Width))
            End If
            Return width
        End Function

        Public Overridable Function getMaxNameWidth(g As Graphics2D, includeNonLeafs As Boolean) As Integer
            Dim width As Integer = getNameWidth(g, includeNonLeafs)
            For Each comp As ClusterComponent In Children
                Dim childWidth As Integer = comp.getMaxNameWidth(g, includeNonLeafs)
                If childWidth > width Then width = childWidth
            Next comp
            Return width
        End Function
    End Class

End Namespace