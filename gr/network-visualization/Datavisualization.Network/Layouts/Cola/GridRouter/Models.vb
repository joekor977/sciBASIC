﻿#Region "Microsoft.VisualBasic::e2c52408585a6777b454473ee24ca3c1, gr\network-visualization\Datavisualization.Network\Layouts\Cola\GridRouter\Models.vb"

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

    '     Class SVGRoutePath
    ' 
    ' 
    ' 
    '     Class vsegmentsets
    ' 
    ' 
    ' 
    '     Class Segment
    ' 
    '         Sub: Reverse
    ' 
    '     Class LinkLine
    ' 
    ' 
    ' 
    '     Class NodeAccessor
    ' 
    ' 
    '         Delegate Function
    ' 
    ' 
    '         Delegate Function
    ' 
    '             Properties: getBounds, getChildren
    ' 
    '     Class LinkAccessor
    ' 
    ' 
    '         Delegate Function
    ' 
    '             Properties: getMinSeparation, getSourceIndex, getTargetIndex
    '         Delegate Sub
    ' 
    '             Properties: setLength
    ' 
    '     Class NodeWrapper
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '     Class Vert
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '     Class GridLine
    ' 
    ' 
    '         Structure Comparer
    ' 
    '             Function: Compare
    ' 
    ' 
    ' 
    ' 
    ' 
    ' 
    ' 
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging.LayoutModel
Imports Microsoft.VisualBasic.Imaging.Math2D
Imports Microsoft.VisualBasic.Language

Namespace Layouts.Cola.GridRouter

    Public Class SVGRoutePath
        Public routepath As String
        Public arrowpath As String
    End Class

    Public Class vsegmentsets
        Public segments As List(Of Segment)
        Public pos As Double
    End Class

    Public Class Segment
        Public edgeid As Integer
        Public i As Integer
        Public Points As Point2D()

        Default Public ReadOnly Property Pt(i As Integer) As Point2D
            Get
                Return Points(i)
            End Get
        End Property

        Public Sub Reverse()
            Points = Points.Reverse.ToArray
        End Sub
    End Class

    Public Class LinkLine : Inherits Line
        Public verts As List(Of Vert)
    End Class

    Public Class NodeAccessor(Of Node)

        Public Delegate Function IGetChildren(v As Node) As Integer()
        Public Delegate Function IGetBounds(v As Node) As Rectangle2D

        Public Property getChildren As IGetChildren
        Public Property getBounds As IGetBounds

    End Class

    Public Class LinkAccessor(Of Link)

        Public Delegate Function IGetIndex(l As Link) As Integer

        Public Property getSourceIndex As IGetIndex
        Public Property getTargetIndex As IGetIndex
        Public Property getMinSeparation As [Variant](Of Double, Func(Of Link(Of Node), Double))

        Public Delegate Sub SetLinkLength(l As Link, value As Double)

        Public Property setLength As SetLinkLength

    End Class

    Public Class NodeWrapper
        Public leaf As Boolean
        Public parent As NodeWrapper
        Public ports As List(Of Vert)
        Public id As Integer
        Public rect As Rectangle2D
        Public children As List(Of Integer)

        Sub New()
        End Sub

        Public Sub New(id As Double, rect As Rectangle2D, children As IEnumerable(Of Integer))
            Me.id = id
            Me.rect = rect
            Me.children = children.ToList

            leaf = Me.children.IsNullOrEmpty
        End Sub
    End Class

    Public Class Vert : Inherits Point2D

        Public id As Double
        Public node As NodeWrapper
        Public line

        Sub New(id As Double, x As Double, y As Double, Optional node As NodeWrapper = Nothing, Optional line As Object = Nothing)
            Me.id = id
            Me.X = x
            Me.Y = y
            Me.node = node
            Me.line = line
        End Sub
    End Class

    ''' <summary>
    ''' a horizontal Or vertical line of nodes
    ''' </summary>
    Public Class GridLine

        Public nodes As NodeWrapper()
        Public pos As Double

        Public Structure Comparer : Implements IComparer(Of GridLine)

            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Public Function Compare(x As GridLine, y As GridLine) As Integer Implements IComparer(Of GridLine).Compare
                Return x.pos - y.pos
            End Function
        End Structure
    End Class
End Namespace
