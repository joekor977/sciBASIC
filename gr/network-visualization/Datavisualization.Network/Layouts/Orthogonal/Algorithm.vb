﻿Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Math.Statistics.Linq
Imports randf = Microsoft.VisualBasic.Math.RandomExtensions
Imports stdNum = System.Math

Namespace Layouts.Orthogonal

    Public Module Algorithm

        <Extension>
        Public Function ResetNodeSize(g As NetworkGraph, size$) As NetworkGraph
            Dim sizeVals As Double() = size.Split(","c).Select(AddressOf Val).ToArray

            For Each node As Node In g.vertex
                node.data.size = sizeVals
            Next

            Return g
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="graph"></param>
        ''' <param name="gridSize"></param>
        ''' <param name="delta">
        ''' some minimum distance between node has to be ensured
        ''' </param>
        <Extension>
        Public Sub DoLayout(graph As NetworkGraph, gridSize As Size, Optional delta# = 1)
            ' 只针对非孤立的网络节点来进行布局的计算
            ' 孤立节点会在for循环中的swap步骤进行被动布局
            Dim V As Node() = graph.GetConnectedVertex.ToArray
            Dim compactionDir = True
            Dim iterationCount = 90 * V.Length
            ' T的作用是用来计算交换的范围
            ' 随着迭代的进行T将会越来越小
            ' 交换的范围从开始的非常大到最终的非常小
            ' 从而使网络的布局变化从变化剧烈到稳定
            Dim T As Double = 2 * V.Length
            Dim k As Double = (0.2 / T) ^ (1 / iterationCount)
            Dim cellSize As Double = V.GridCellSize

            Call "Initialize grid model....".__INFO_ECHO

            Dim grid As New Grid(gridSize, cellSize)
            Dim workspace As New Workspace With {
                .g = graph,
                .grid = grid,
                .V = V,
                .cellSize = cellSize,
                .width = V.ToDictionary(Function(n) n.label, Function(n) n.width(cellSize, delta)),
                .height = V.ToDictionary(Function(n) n.label, Function(n) n.height(cellSize, delta))
            }

            Call "Create random node layout...".__INFO_ECHO

            Call grid.PutRandomNodes(graph)

            Call "Running layout iteration...".__INFO_ECHO
            Call "Phase 1".__INFO_ECHO

            Dim [stop] = iterationCount \ 2
            Dim totalEdgeLength As Double = workspace.totalEdgeLength

            For i As Integer = 0 To [stop]
                For j As Integer = 0 To V.Length - 1
                    ' To perform local optimization, every node is moved to a location that minimizes
                    ' the total length of its adjacent edges.
                    Dim x = graph.neighboursMedianX(V(j)) + randf.randf(-T, T)
                    Dim y = graph.neighboursMedianY(V(j)) + randf.randf(-T, T)
                    Dim cell As Point = grid.FindIndex(x, y)
                    Dim gridCell As GridCell = grid(cell)
                    Dim currentCell As GridCell = grid.FindCell(V(j).label)

                    ' if vj has not changed it’s place from the previous iteration then
                    If Not gridCell Is currentCell AndAlso Not gridCell.node Is V(j) Then
                        Call grid.SwapNode(currentCell.index, gridCell.index)
                    Else
                        ' Try to swap vj with nodes nearby;
                        Call workspace.SwapNearbyNode(origin:=gridCell)
                    End If
                Next

                Call Console.WriteLine("[{0}] {1}%, T={2}, total:={3}", i, (100 * i / [stop]).ToString("F2"), T, totalEdgeLength)

                If iterationCount Mod 9 = 0 Then
                    workspace.compact(compactionDir, 3, False)
                    compactionDir = Not compactionDir
                End If

                T = T * k
                totalEdgeLength = workspace.totalEdgeLength
            Next

            Call "Phase 2".__INFO_ECHO

            workspace.compact(True, 3, True)
            workspace.compact(False, 3, True)

            For i As Integer = iterationCount \ 2 + 1 To iterationCount
                For j As Integer = 0 To V.Length - 1
                    Dim wj = workspace.width(V(j).label)
                    Dim hj = workspace.height(V(j).label)
                    Dim x = graph.neighboursMedianX(V(j)) + randf.randf(-T * wj, T * wj)
                    Dim y = graph.neighboursMedianY(V(j)) + randf.randf(-T * hj, T * hj)
                    Dim cell As Point = grid.FindIndex(x, y)
                    Dim gridCell As GridCell = grid(cell)
                    Dim currentCell As GridCell = grid.FindCell(V(j).label)

                    ' if vj has not changed it’s place from the previous iteration then
                    If Not gridCell Is currentCell AndAlso Not gridCell.node Is V(j) Then
                        Call grid.SwapNode(currentCell.index, gridCell.index)
                    Else
                        ' Try to swap vj with nodes nearby;
                        Call workspace.SwapNearbyNode(origin:=gridCell)
                    End If
                Next

                Call Console.WriteLine("[{0}] {1}%, T={2}", i, (100 * i / iterationCount).ToString("F2"), T)

                If iterationCount Mod 9 = 0 Then
                    workspace.compact(compactionDir, stdNum.Max(1, 1 + 2 * (iterationCount - i - 30) / (0.5 * iterationCount)), False)
                    compactionDir = Not compactionDir
                End If

                T = T * k
            Next

            Call "Do layout job done!".__INFO_ECHO
        End Sub

        <Extension>
        Private Sub SwapNearbyNode(workspace As Workspace, origin As GridCell)
            Dim totalLenBefore As Double = workspace.totalEdgeLength
            Dim totalLenAfter As Double
            Dim gain As Double

            For Each nearby As GridCell In workspace.grid.GetAdjacentCells(origin.index).Shuffles
                If nearby.node Is Nothing Then
                    ' 附近的单元格是没有节点的，直接放置进去?
                    Call workspace.grid.MoveNode(origin.index, nearby.index)
                Else
                    Call workspace.grid.SwapNode(origin.index, nearby.index)
                End If

                totalLenAfter = workspace.totalEdgeLength
                gain = totalLenAfter - totalLenBefore

                If gain > 0 Then
                    Exit For
                Else
                    ' restore
                    Call workspace.grid.SwapNode(origin.index, nearby.index)
                End If
            Next
        End Sub

        <Extension>
        Private Function neighboursMedianX(g As NetworkGraph, v As Node) As Double
            Dim edges = g.GetEdges(v).ToArray
            Dim x = edges _
                .Select(Function(e)
                            If e.U Is v Then
                                Return e.V.data.initialPostion.x
                            Else
                                Return e.U.data.initialPostion.x
                            End If
                        End Function) _
                .ToArray
            Dim median As Double = x.Median

            Return median
        End Function

        <Extension>
        Private Function neighboursMedianY(g As NetworkGraph, v As Node) As Double
            Dim edges = g.GetEdges(v)
            Dim y = edges _
                .Select(Function(e)
                            If e.U Is v Then
                                Return e.V.data.initialPostion.y
                            Else
                                Return e.U.data.initialPostion.y
                            End If
                        End Function)
            Dim median = y.Median

            Return median
        End Function
    End Module
End Namespace