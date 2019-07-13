﻿#Region "Microsoft.VisualBasic::26198b2cb036b2ecebdbc7875d65edc7, gr\network-visualization\Datavisualization.Network\Layouts\ForceDirected\Layout\AbstractRenderer.vb"

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

    '     Class AbstractRenderer
    ' 
    '         Properties: PhysicsEngine
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Sub: DirectDraw, Draw
    ' 
    ' 
    ' /********************************************************************************/

#End Region

'! 
'@file AbstractRenderer.cs
'@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
'		<http://github.com/juhgiyo/epForceDirectedGraph.cs>
'@date August 08, 2013
'@brief Abstract Renderer Interface
'@version 1.0
'
'@section LICENSE
'
'The MIT License (MIT)
'
'Copyright (c) 2013 Woong Gyu La <juhgiyo@gmail.com>
'
'Permission is hereby granted, free of charge, to any person obtaining a copy
'of this software and associated documentation files (the "Software"), to deal
'in the Software without restriction, including without limitation the rights
'to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
'copies of the Software, and to permit persons to whom the Software is
'furnished to do so, subject to the following conditions:
'
'The above copyright notice and this permission notice shall be included in
'all copies or substantial portions of the Software.
'
'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
'IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
'FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
'AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
'LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
'OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
'THE SOFTWARE.
'
'@section DESCRIPTION
'
'An Interface for the Abstract Renderer Class.
'
'

Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts.Interfaces

Namespace Layouts

    Public MustInherit Class AbstractRenderer
        Implements IRenderer

        Public ReadOnly Property PhysicsEngine As IForceDirected
            Get
                Return forceDirected
            End Get
        End Property

        Protected forceDirected As IForceDirected

        Public Sub New(forceDirected As IForceDirected)
            Me.forceDirected = forceDirected
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="iTimeStep"><see cref="IForceDirected.Calculate(Single)"/></param>
        ''' <param name="physicsUpdate"></param>
        Public Sub Draw(iTimeStep As Single, Optional physicsUpdate As Boolean = True) Implements IRenderer.Draw
            If physicsUpdate Then
                ' 计算力的变化
                Call forceDirected.Calculate(iTimeStep)
            End If

            ' 清理画板
            Call Clear()
            Call DirectDraw()
        End Sub

        ''' <summary>
        ''' 不计算位置而直接更新绘图
        ''' </summary>
        Public Overridable Sub DirectDraw()
            forceDirected.EachEdge(Sub(edge As Edge, spring As Spring) drawEdge(edge, spring.point1.position, spring.point2.position))
            forceDirected.EachNode(Sub(node As Node, point As LayoutPoint) drawNode(node, point.position))
        End Sub

        Public MustOverride Sub Clear() Implements IRenderer.Clear
        Protected MustOverride Sub drawEdge(iEdge As Edge, iPosition1 As AbstractVector, iPosition2 As AbstractVector)
        Protected MustOverride Sub drawNode(iNode As Node, iPosition As AbstractVector)

    End Class
End Namespace
