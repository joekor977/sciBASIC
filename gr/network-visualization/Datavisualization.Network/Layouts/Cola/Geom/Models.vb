﻿#Region "Microsoft.VisualBasic::91cb0b4bbe8515648421f6affacc35ab, gr\network-visualization\Datavisualization.Network\Layouts\Cola\Geom\Models.vb"

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

    '     Class PolyPoint
    ' 
    '         Properties: polyIndex
    ' 
    '     Class tangentPoly
    ' 
    '         Properties: ltan, rtan
    ' 
    '     Class BiTangent
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '     Class BiTangents
    ' 
    ' 
    ' 
    '     Class TVGPoint
    ' 
    ' 
    ' 
    '     Class VisibilityVertex
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '     Class VisibilityEdge
    ' 
    '         Properties: length
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Imaging.LayoutModel
Imports Microsoft.VisualBasic.My.JavaScript

Namespace Layouts.Cola

    Public Class PolyPoint : Inherits Point2D

        Public Property polyIndex() As Integer

    End Class

    Public Class tangentPoly

        ''' <summary>
        ''' index of rightmost tangent point V[rtan]
        ''' </summary>
        ''' <returns></returns>
        Public Property rtan() As Integer
        ''' <summary>
        ''' index of leftmost tangent point V[ltan]
        ''' </summary>
        ''' <returns></returns>
        Public Property ltan() As Integer

    End Class

    Public Class BiTangent

        Public t1 As Integer, t2 As Integer

        Public Sub New()
        End Sub

        Public Sub New(t1 As Integer, t2 As Integer)
            Me.t1 = t1
            Me.t2 = t2
        End Sub
    End Class

    Public Class BiTangents : Inherits JavaScriptObject

        Public rl As BiTangent
        Public lr As BiTangent
        Public ll As BiTangent
        Public rr As BiTangent

    End Class

    Public Class TVGPoint : Inherits Point2D
        Public vv As VisibilityVertex
    End Class

    Public Class VisibilityVertex

        Public id As Integer
        Public polyid As Double
        Public polyvertid As Double
        Public p As TVGPoint

        Public Sub New(id As Integer, polyid As Double, polyvertid As Double, p As TVGPoint)
            Me.id = id
            Me.polyid = polyid
            Me.polyvertid = polyvertid
            Me.p = p

            p.vv = Me
        End Sub
    End Class

    Public Class VisibilityEdge

        Public source As VisibilityVertex
        Public target As VisibilityVertex

        Public ReadOnly Property length() As Double
            Get
                Dim dx = Me.source.p.X - Me.target.p.X
                Dim dy = Me.source.p.Y - Me.target.p.Y
                Return Math.Sqrt(dx * dx + dy * dy)
            End Get
        End Property

        Sub New(source As VisibilityVertex, target As VisibilityVertex)
            Me.source = source
            Me.target = target
        End Sub
    End Class
End Namespace
