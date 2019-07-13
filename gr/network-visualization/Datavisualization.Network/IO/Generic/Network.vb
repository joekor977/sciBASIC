﻿#Region "Microsoft.VisualBasic::0abbf070d59aa78a4178bc831f6e9e99, gr\network-visualization\Datavisualization.Network\IO\Generic\Network.vb"

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

    '     Class Network
    ' 
    '         Properties: edges, IsEmpty, nodes
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: GetNode, HaveNode, Load, Save
    ' 
    '         Sub: RemoveDuplicated, RemoveSelfLoop
    ' 
    '         Operators: (+4 Overloads) -, (+2 Overloads) ^, (+4 Overloads) +, <=, >=
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.Data.csv.Extensions
Imports Microsoft.VisualBasic.Language

Namespace FileStream.Generic

    ''' <summary>
    ''' The network csv data information with specific type of the datamodel
    ''' </summary>
    ''' <typeparam name="T_Node"></typeparam>
    ''' <typeparam name="T_Edge"></typeparam>
    ''' <remarks></remarks>
    Public Class Network(Of T_Node As Node, T_Edge As NetworkEdge) : Inherits UnixBash.FileSystem.File
        Implements IKeyValuePairObject(Of T_Node(), T_Edge())
        Implements ISaveHandle

        Public Property nodes As T_Node() Implements IKeyValuePairObject(Of T_Node(), T_Edge()).Key
            Get
                If __nodes Is Nothing Then
                    __nodes = New Dictionary(Of T_Node)
                End If
                Return __nodes.Values.ToArray
            End Get
            Set(value As T_Node())
                If value Is Nothing Then
                    __nodes = New Dictionary(Of T_Node)
                Else
                    __nodes = value.ToDictionary
                End If
            End Set
        End Property

        Public Property edges As T_Edge() Implements IKeyValuePairObject(Of T_Node(), T_Edge()).Value
            Get
                If __edges Is Nothing Then
                    __edges = New List(Of T_Edge)
                End If
                Return __edges.ToArray
            End Get
            Set(value As T_Edge())
                If value Is Nothing Then
                    __edges = New List(Of T_Edge)
                Else
                    __edges = value.AsList
                End If
            End Set
        End Property

        ''' <summary>
        ''' 判断这个网络模型之中是否是没有任何数据
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IsEmpty As Boolean
            Get
                Return __nodes.IsNullOrEmpty AndAlso __edges.IsNullOrEmpty
            End Get
        End Property

        Sub New()
            __nodes = New Dictionary(Of T_Node)
            __edges = New List(Of T_Edge)
        End Sub

        Dim __nodes As Dictionary(Of T_Node)
        Dim __edges As List(Of T_Edge)

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function HaveNode(id$) As Boolean
            Return __nodes.ContainsKey(id)
        End Function

        ''' <summary>
        ''' 移除的重复的边
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RemoveDuplicated()
            Dim LQuery As T_Edge() =
                edges _
                .GroupBy(Function(ed) ed.GetNullDirectedGuid(True)) _
                .Select(Function(g) g.First) _
                .ToArray

            edges = LQuery
        End Sub

        ''' <summary>
        ''' 移除自身与自身的边
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RemoveSelfLoop()
            Dim LQuery = LinqAPI.Exec(Of T_Edge) _
 _
                () <= From x As T_Edge
                      In edges
                      Where Not x.SelfLoop
                      Select x

            edges = LQuery
        End Sub

        ''' <summary>
        '''
        ''' </summary>
        ''' <param name="outDIR">The data directory for the data export, if the value of this directory is null then the data
        ''' will be exported at the current work directory.
        ''' (进行数据导出的文件夹，假若为空则会保存数据至当前的工作文件夹之中)</param>
        ''' <param name="encoding">The file encoding of the exported node and edge csv file.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Save(outDIR$, encoding As Encoding) As Boolean Implements ISaveHandle.Save
            With outDIR Or App.CurrentDirectory.AsDefault
                Call nodes.SaveTo($"{ .ByRef}/nodes.csv", False, encoding)
                Call edges.SaveTo($"{ .ByRef}/network-edges.csv", False, encoding)
            End With

            Return True
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Function Load(directory As String) As Network(Of T_Node, T_Edge)
            Return New Network(Of T_Node, T_Edge) With {
                .edges = $"{directory}/network-edges.csv".LoadCsv(Of T_Edge),
                .nodes = $"{directory}/nodes.csv".LoadCsv(Of T_Node)
            }
        End Function

        Public Shared Operator +(net As Network(Of T_Node, T_Edge), x As T_Node) As Network(Of T_Node, T_Edge)
            Call net.__nodes.Add(x)
            Return net
        End Operator

        Public Shared Operator -(net As Network(Of T_Node, T_Edge), x As T_Node) As Network(Of T_Node, T_Edge)
            Call net.__nodes.Remove(x)
            Return net
        End Operator

        Public Shared Operator +(net As Network(Of T_Node, T_Edge), x As T_Edge) As Network(Of T_Node, T_Edge)
            Call net.__edges.Add(x)
            Return net
        End Operator

        Public Shared Operator -(net As Network(Of T_Node, T_Edge), x As T_Edge) As Network(Of T_Node, T_Edge)
            Call net.__edges.Remove(x)
            Return net
        End Operator

        ''' <summary>
        '''
        ''' </summary>
        ''' <param name="net"></param>
        ''' <param name="x">由于会调用ToArray，所以这里建议使用Iterator</param>
        ''' <returns></returns>
        Public Shared Operator +(net As Network(Of T_Node, T_Edge), x As IEnumerable(Of T_Node)) As Network(Of T_Node, T_Edge)
            Call net.__nodes.AddRange(x.ToArray)
            Return net
        End Operator

        Public Shared Operator -(net As Network(Of T_Node, T_Edge), lst As IEnumerable(Of T_Node)) As Network(Of T_Node, T_Edge)
            For Each x In lst
                Call net.__nodes.Remove(x)
            Next

            Return net
        End Operator

        ''' <summary>
        '''
        ''' </summary>
        ''' <param name="net"></param>
        ''' <param name="x">由于会调用ToArray，所以这里建议使用Iterator</param>
        ''' <returns></returns>
        Public Shared Operator +(net As Network(Of T_Node, T_Edge), x As IEnumerable(Of T_Edge)) As Network(Of T_Node, T_Edge)
            Call net.__edges.AddRange(x.ToArray)
            Return net
        End Operator

        Public Shared Operator -(net As Network(Of T_Node, T_Edge), lst As IEnumerable(Of T_Edge)) As Network(Of T_Node, T_Edge)
            For Each x In lst
                Call net.__edges.Remove(x)
            Next

            Return net
        End Operator

        ''' <summary>
        ''' Network contains node?
        ''' </summary>
        ''' <param name="net"></param>
        ''' <param name="node"></param>
        ''' <returns></returns>
        Public Shared Operator ^(net As Network(Of T_Node, T_Edge), node As String) As Boolean
            Return net.__nodes.ContainsKey(node)
        End Operator

        ''' <summary>
        ''' Network contains node?
        ''' </summary>
        ''' <param name="net"></param>
        ''' <param name="node"></param>
        ''' <returns></returns>
        Public Shared Operator ^(net As Network(Of T_Node, T_Edge), node As T_Node) As Boolean
            Return net ^ node.ID
        End Operator

        ''' <summary>
        ''' GET node
        ''' </summary>
        ''' <param name="net"></param>
        ''' <param name="node"></param>
        ''' <returns></returns>
        Public Overloads Shared Operator &(net As Network(Of T_Node, T_Edge), node As String) As T_Node
            If net.__nodes.ContainsKey(node) Then
                Return net.__nodes(node)
            Else
                Return Nothing
            End If
        End Operator

        ''' <summary>
        ''' Select nodes from the network based on the input identifers <paramref name="nodes"/>
        ''' </summary>
        ''' <param name="net"></param>
        ''' <param name="nodes"></param>
        ''' <returns></returns>
        Public Shared Operator <=(net As Network(Of T_Node, T_Edge), nodes As IEnumerable(Of String)) As T_Node()
            Dim LQuery = (From sId As String In nodes Select net.__nodes(sId)).ToArray
            Return LQuery
        End Operator

        Public Shared Operator >=(net As Network(Of T_Node, T_Edge), nodes As IEnumerable(Of String)) As T_Node()
            Return net <= nodes
        End Operator

        Public Function GetNode(name As String) As T_Node
            Return Me & name
        End Function
    End Class
End Namespace
