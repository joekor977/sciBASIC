﻿#Region "Microsoft.VisualBasic::6aa1ffd850b84765f6e3684cf295a61e, Data_science\DataMining\hierarchical-clustering\hierarchical-clustering\HierarchyBuilder\HierarchyTreeNode.vb"

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

    '     Class HierarchyTreeNode
    ' 
    '         Properties: Left, LinkageDistance, Right
    ' 
    '         Constructor: (+2 Overloads) Sub New
    '         Function: Agglomerate, compareTo, GetOtherCluster, Reverse, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System
Imports System.Text
Imports Microsoft.VisualBasic.DataMining.HierarchicalClustering.Hierarchy

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

Namespace Hierarchy

    Public Class HierarchyTreeNode : Implements IComparable(Of HierarchyTreeNode)

        Private Shared globalIndex As Long = 0

        Public Sub New()
        End Sub

        Public Sub New(left As Cluster, right As Cluster, distance As Double)
            Me.Left = left
            Me.Right = right
            LinkageDistance = distance
        End Sub

        Public Function GetOtherCluster(c As Cluster) As Cluster
            Return If(Left Is c, Right, Left)
        End Function

        Public Property Left As Cluster
        Public Property Right As Cluster
        Public Property LinkageDistance As Double

        ''' <returns> 
        ''' a new ClusterPair with the two left/right inverted
        ''' </returns>
        Public Function Reverse() As HierarchyTreeNode
            Return New HierarchyTreeNode(Right(), Left(), LinkageDistance)
        End Function

        Public Function compareTo(o As HierarchyTreeNode) As Integer Implements IComparable(Of HierarchyTreeNode).CompareTo
            Dim result As Integer

            If o Is Nothing OrElse o.LinkageDistance = 0 Then
                result = -1
            ElseIf LinkageDistance = 0 Then
                result = 1
            Else
                result = LinkageDistance.CompareTo(o.LinkageDistance)
            End If

            Return result
        End Function

        Public Function Agglomerate(name As String) As Cluster
            If name Is Nothing Then
                globalIndex += 1
                name = "clstr#" & (globalIndex)
            End If

            Dim cluster As New Cluster(name) With {
                .Distance = New Distance(LinkageDistance)
            }

            ' New clusters will track their children's leaf names; i.e. each cluster knows what part of the original data it contains
            cluster.AppendLeafNames(Left.LeafNames)
            cluster.AppendLeafNames(Right.LeafNames)
            cluster.AddChild(Left)
            cluster.AddChild(Right)
            Left.Parent = cluster
            Right.Parent = cluster

            cluster.Distance.Weight = Left.WeightValue + Right.WeightValue

            Return cluster
        End Function

        Public Overrides Function ToString() As String
            Dim sb As New StringBuilder

            If Left IsNot Nothing Then
                sb.Append(Left.Name)
            End If

            If Right IsNot Nothing Then
                If sb.Length > 0 Then
                    sb.Append(" + ")
                End If
                sb.Append(Right.Name)
            End If

            Call sb _
                .Append(" : ") _
                .Append(LinkageDistance)

            Return sb.ToString()
        End Function
    End Class
End Namespace
