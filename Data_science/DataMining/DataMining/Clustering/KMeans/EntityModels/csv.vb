﻿#Region "Microsoft.VisualBasic::c1787e82d350a4c7d21feea6f82e1ec6, Data_science\DataMining\DataMining\Clustering\KMeans\EntityModels\csv.vb"

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

    '     Class EntityClusterModel
    ' 
    '         Properties: Cluster, ID
    ' 
    '         Function: FromDataSet, FromModel, ToModel, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel

Namespace KMeans

    ''' <summary>
    ''' 存储在Csv文件里面的数据模型，近似等价于<see cref="DataSet"/>，只不过多带了一个用来描述cluster的<see cref="Cluster"/>属性标签
    ''' </summary>
    Public Class EntityClusterModel : Inherits DynamicPropertyBase(Of Double)
        Implements INamedValue

        Public Property ID As String Implements INamedValue.Key

        ''' <summary>
        ''' 聚类结果的类编号
        ''' </summary>
        ''' <returns></returns>
        Public Property Cluster As String

        ''' <summary>
        ''' 用于生成聚类所需要的数据集，所以通过这个函数所构建的数据集对象的<see cref="Cluster"/>属性值都是空的
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="data"></param>
        ''' <returns></returns>
        Public Shared Function FromDataSet(Of T As {INamedValue, DynamicPropertyBase(Of Double)})(data As T) As EntityClusterModel
            Return New EntityClusterModel With {
                .ID = data.Key,
                .Properties = data.Properties
            }
        End Function

        Public Overrides Function ToString() As String
            Return ID
        End Function

        Public Function ToModel() As ClusterEntity
            Return New ClusterEntity With {
                .uid = ID,
                .entityVector = Properties.Values.ToArray
            }
        End Function

        Public Shared Iterator Function FromModel(data As IEnumerable(Of NamedValue(Of Dictionary(Of String, Double)))) As IEnumerable(Of EntityClusterModel)
            For Each x As NamedValue(Of Dictionary(Of String, Double)) In data
                Yield New EntityClusterModel With {
                    .ID = x.Name,
                    .Properties = x.Value
                }
            Next
        End Function
    End Class
End Namespace
