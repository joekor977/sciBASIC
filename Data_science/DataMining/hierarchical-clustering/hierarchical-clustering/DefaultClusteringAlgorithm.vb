﻿#Region "Microsoft.VisualBasic::79c92ec6ba4417c3eb41fba09e0d9e15, Data_science\DataMining\hierarchical-clustering\hierarchical-clustering\DefaultClusteringAlgorithm.vb"

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

    ' Class DefaultClusteringAlgorithm
    ' 
    '     Function: (+2 Overloads) createClusters, createLinkages, performClustering, performFlatClustering, performWeightedClustering
    ' 
    '     Sub: checkArguments
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.DataMining.HierarchicalClustering.Hierarchy
Imports Microsoft.VisualBasic.Linq

'
'*****************************************************************************
' Copyright 2013 Lars Behnke
' <p/>
' Licensed under the Apache License, Version 2.0 (the "License");
' you may not use this file except in compliance with the License.
' You may obtain a copy of the License at
' <p/>
' http://www.apache.org/licenses/LICENSE-2.0
' <p/>
' Unless required by applicable law or agreed to in writing, software
' distributed under the License is distributed on an "AS IS" BASIS,
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' See the License for the specific language governing permissions and
' limitations under the License.
' *****************************************************************************

Public Class DefaultClusteringAlgorithm
    Implements ClusteringAlgorithm

    Public Function performClustering(distances As Double()(), clusterNames$(), linkageStrategy As LinkageStrategy) As Cluster Implements ClusteringAlgorithm.performClustering

        Call checkArguments(distances, clusterNames, linkageStrategy)

        ' Setup model 
        Dim clusters As IList(Of Cluster) = createClusters(clusterNames)
        Dim linkages As DistanceMap = createLinkages(distances, clusters)

        ' Process 
        Dim builder As New HierarchyBuilder(clusters, linkages)

        Do While Not builder.TreeComplete
            builder.Agglomerate(linkageStrategy)
        Loop

        Return builder.RootCluster
    End Function

    Public Function performFlatClustering(distances As Double()(), clusterNames$(), linkageStrategy As LinkageStrategy, threshold As Double) As IList(Of Cluster) Implements ClusteringAlgorithm.performFlatClustering

        Call checkArguments(distances, clusterNames, linkageStrategy)

        ' Setup model 
        Dim clusters As IList(Of Cluster) = createClusters(clusterNames)
        Dim linkages As DistanceMap = createLinkages(distances, clusters)

        ' Process 
        Dim builder As New HierarchyBuilder(clusters, linkages)
        Return builder.flatAgg(linkageStrategy, threshold)
    End Function

    Private Shared Sub checkArguments(distances As Double()(), clusterNames As String(), linkageStrategy As LinkageStrategy)
        If distances Is Nothing OrElse distances.Length = 0 OrElse distances(0).Length <> distances.Length Then Throw New ArgumentException("Invalid distance matrix")
        If distances.Length <> clusterNames.Length Then Throw New ArgumentException("Invalid cluster name array")
        If linkageStrategy Is Nothing Then Throw New ArgumentException("Undefined linkage strategy")

        Dim uniqueCount As Integer = clusterNames.Distinct.Count

        If uniqueCount <> clusterNames.Length Then
            Throw New ArgumentException("Duplicate names")
        End If
    End Sub

    Public Function performWeightedClustering(distances As Double()(), clusterNames As String(), weights As Double(), linkageStrategy As LinkageStrategy) As Cluster Implements ClusteringAlgorithm.performWeightedClustering
        If weights.Length <> clusterNames.Length Then
            Throw New ArgumentException("Invalid weights array")
        Else
            Call checkArguments(distances, clusterNames, linkageStrategy)
        End If

        ' Setup model 
        Dim clusters As IList(Of Cluster) = createClusters(clusterNames, weights)
        Dim linkages As DistanceMap = Time(Function() createLinkages(distances, clusters))

        ' Process 
        Dim builder As New HierarchyBuilder(clusters, linkages)
        Do While Not builder.TreeComplete
            builder.Agglomerate(linkageStrategy)
        Loop

        Return builder.RootCluster
    End Function

    Private Function createLinkages(distances As Double()(), clusters As IList(Of Cluster)) As DistanceMap
        If clusters.Count < 100 Then
            Dim linkages As New DistanceMap

            For col As Integer = 0 To clusters.Count - 1
                For row As Integer = col + 1 To clusters.Count - 1
                    Dim link As New HierarchyTreeNode
                    Dim lCluster As Cluster = clusters(col)
                    Dim rCluster As Cluster = clusters(row)
                    link.LinkageDistance = distances(col)(row)
                    link.Left = (lCluster)
                    link.Right = (rCluster)

                    linkages.Add(link)
                Next
            Next

            Return linkages
        Else
            ' 当数量很大的时候，这里也是一个限速步骤，需要使用并行
            Dim LQuery = From c As SeqValue(Of Cluster)
                         In clusters.SeqIterator.AsParallel
                         Let col As Integer = c.i
                         Let lCluster As Cluster = c.value
                         Let factory =
                             Function()
                                 Dim list As New List(Of HierarchyTreeNode)
                                 Dim n = clusters.Count
                                 Dim copy = clusters.ToArray

                                 For row As Integer = col + 1 To n - 1
                                     Dim rCluster As Cluster = copy(row)
                                     Dim link As New HierarchyTreeNode With {
                                         .LinkageDistance = distances(col)(row),
                                         .Left = (lCluster),
                                         .Right = (rCluster)
                                     }

                                     Call list.Add(link)
                                 Next

                                 Return list
                             End Function
                         Select factory()

            Dim links = LQuery _
                .IteratesALL _
                .ToArray

            Return New DistanceMap(links)
        End If
    End Function

    ''' <summary>
    ''' 创建cluster对象
    ''' </summary>
    ''' <param name="clusterNames"></param>
    ''' <returns></returns>
    Private Shared Function createClusters(clusterNames As String()) As IList(Of Cluster)
        Return clusterNames _
            .Select(Function(clusterName) New Cluster(clusterName)) _
            .AsList
    End Function

    Private Shared Function createClusters(clusterNames As String(), weights As Double()) As IList(Of Cluster)
        Dim clusters As IList(Of Cluster) = New List(Of Cluster)

        For i As Integer = 0 To weights.Length - 1
            Dim cluster As New Cluster(clusterNames(i))
            cluster.Distance = New Distance(0.0, weights(i))
            clusters.Add(cluster)
        Next

        Return clusters
    End Function

End Class
