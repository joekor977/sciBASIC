﻿#Region "Microsoft.VisualBasic::93adfba91d581116b612acefc52cd2c2, ..\visualbasic_App\gr\Datavisualization.Network\Datavisualization.Network\FindPath\PQDijkstra\Example.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xieguigang (xie.guigang@live.com)
    '       xie (genetics@smrucc.org)
    ' 
    ' Copyright (c) 2016 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.

#End Region

Namespace Dijkstra.PQDijkstra

    Public Class Finder : Inherits PQDijkstraProvider

        Private totalNodes As Integer = 100000
        Private cost As Single(,)
        Private dijkstra As DijkstraFast
        Private rv As Random

        Sub New(totalNodes As Integer)
            Call MyBase.New(totalNodes)

            cost = New Single(totalNodes - 1, 3) {}
            ' initialize the cost matrix
            rv = New Random()
            For i As Integer = 0 To totalNodes - 1
                cost(i, 0) = CSng(rv.[Next](1000)) * 0.01F
                cost(i, 1) = CSng(rv.[Next](1000)) * 0.01F
                cost(i, 2) = CSng(rv.[Next](1000)) * 0.01F
                cost(i, 3) = CSng(rv.[Next](1000)) * 0.01F
            Next
        End Sub

        ''' <summary>
        ''' a function to get relative position from one node to another
        ''' </summary>
        ''' <param name="start"></param>
        ''' <param name="finish"></param>
        ''' <returns></returns>
        Private Function GetRelativePosition(start As Integer, finish As Integer) As Integer
            If start - 1 = finish Then
                Return 0

            ElseIf start + 1 = finish Then
                Return 1

            ElseIf start + 5 = finish Then
                Return 2

            ElseIf start - 5 = finish Then
                Return 3
            End If

            Return -1
        End Function

        Protected Overrides Function getInternodeTraversalCost(start As Integer, finish As Integer) As Single
            Dim relativePosition As Integer = GetRelativePosition(start, finish)
            If relativePosition < 0 Then
                Return Single.MaxValue
            End If
            Return cost(start, relativePosition)
        End Function

        Protected Overrides Function GetNearbyNodes(startingNode As Integer) As IEnumerable(Of Integer)
            Dim nearbyNodes As New List(Of Integer)(4)

            If startingNode >= totalNodes - 5 Then
                startingNode = -1
            End If
            If startingNode <= 5 Then
                startingNode = -1
            End If

            ' in the order as defined in GetRelativePosition
            nearbyNodes.Add(startingNode - 1)
            nearbyNodes.Add(startingNode + 1)
            nearbyNodes.Add(startingNode + 5)
            nearbyNodes.Add(startingNode - 5)
            Return nearbyNodes
        End Function
    End Class
End Namespace
