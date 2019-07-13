﻿#Region "Microsoft.VisualBasic::5cb4b8211909ca5f5cc5c8ff64ee8f20, Data_science\Visualization\Plots-statistics\Heatmap\PlotExtensions.vb"

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

    '     Module PlotExtensions
    ' 
    '         Function: KmeansReorder, LoadDataSet
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.DataMining.KMeans
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.Correlations
Imports Microsoft.VisualBasic.Math.Correlations.Correlations
Imports Microsoft.VisualBasic.Math.DataFrame

Namespace Heatmap

    Public Module PlotExtensions

        ''' <summary>
        ''' (这个函数是直接加在已经计算好了的相关度数据).假若使用这个直接加载数据来进行heatmap的绘制，
        ''' 请先要确保数据集之中的所有数据都是经过归一化的，假若没有归一化，则确保函数参数
        ''' <paramref name="normalization"/>的值为真
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="uidMap$"></param>
        ''' <param name="normalization">是否对输入的数据集进行归一化处理？</param>
        ''' <param name="correlation">
        ''' 默认为<see cref="Correlations.GetPearson(Double(), Double())"/>方法
        ''' </param>
        ''' <returns></returns>
        <Extension>
        Public Function LoadDataSet(path As String,
                                    Optional uidMap$ = Nothing,
                                    Optional normalization As Boolean = False,
                                    Optional correlation As ICorrelation = Nothing) As NamedValue(Of Dictionary(Of String, Double))()

            Dim ds As IEnumerable(Of DataSet) = DataSet.LoadDataSet(path, uidMap)

            If normalization Then
                Return ds.CorrelatesNormalized(correlation).ToArray
            Else
                Return LinqAPI.Exec(Of NamedValue(Of Dictionary(Of String, Double))) _
 _
                    () <= From x As DataSet
                          In ds
                          Select New NamedValue(Of Dictionary(Of String, Double)) With {
                              .Name = x.ID,
                              .Value = x.Properties
                          }
            End If
        End Function

        <Extension>
        Public Function KmeansReorder(data As NamedValue(Of Dictionary(Of String, Double))(), Optional n% = 5) As NamedValue(Of Dictionary(Of String, Double))()
            Dim keys$() = data(Scan0%).Value.Keys.ToArray
            Dim entityList As ClusterEntity() = LinqAPI.Exec(Of ClusterEntity) _
 _
                () <= From x As NamedValue(Of Dictionary(Of String, Double))
                      In data
                      Select New ClusterEntity With {
                          .uid = x.Name,
                          .entityVector = keys _
                              .Select(Function(k) x.Value(k)) _
                              .ToArray
                      }

            Dim clusters As ClusterCollection(Of ClusterEntity)

            n = entityList.Length / n

            If n = 0 OrElse entityList.Length <= 2 Then
                clusters = New ClusterCollection(Of ClusterEntity)

                For Each x As ClusterEntity In entityList
                    Dim c As New KMeansCluster(Of ClusterEntity)

                    Call c.Add(x)
                    Call clusters.Add(c)
                Next
            Else
                clusters = entityList.ClusterDataSet(n)
            End If

            Dim out As New List(Of NamedValue(Of Dictionary(Of String, Double)))

            ' 通过kmeans计算出keys的顺序
            Dim keysEntity = keys _
                .Select(Function(k)
                            Return New ClusterEntity With {
                                .uid = k,
                                .entityVector = data _
                                    .Select(Function(x) x.Value(k)) _
                                    .ToArray
                            }
                        End Function) _
                .ToArray

            Dim keysOrder As New List(Of String)

            For Each cluster In keysEntity.ClusterDataSet(CInt(keys.Length / 5))
                For Each k In cluster
                    keysOrder += k.uid
                Next
            Next

            For Each cluster In clusters
                For Each entity As ClusterEntity In cluster
                    out += New NamedValue(Of Dictionary(Of String, Double)) With {
                    .Name = entity.uid,
                    .Value = keysOrder _
                        .SeqIterator _
                        .ToDictionary(Function(x) x.value,
                                      Function(x) entity.entityVector(x.i))
                }
                Next
            Next

            Return out
        End Function
    End Module
End Namespace
