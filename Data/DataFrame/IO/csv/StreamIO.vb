﻿#Region "Microsoft.VisualBasic::a8c811978f9fe448d038f7545a375510, Data\DataFrame\IO\csv\StreamIO.vb"

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

    '     Module StreamIO
    ' 
    '         Function: (+2 Overloads) [TypeOf], SaveDataFrame
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Data.csv.StorageProvider.ComponentModels
Imports Microsoft.VisualBasic.FileIO
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Text

Namespace IO

    Public Module StreamIO

        ''' <summary>
        ''' 根据文件的头部的定义，从<paramref name="types"/>之中选取得到最合适的类型的定义
        ''' </summary>
        ''' <param name="csv"></param>
        ''' <param name="types"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function [TypeOf](csv As File, ParamArray types As Type()) As Type
            Return csv.Headers.TypeOf(types)
        End Function

        ''' <summary>
        ''' 根据文件的头部的定义，从<paramref name="types"/>之中选取得到最合适的类型的定义
        ''' </summary>
        ''' <param name="header"></param>
        ''' <param name="types">A candidate type list</param>
        ''' <returns></returns>
        ''' 
        <Extension>
        Public Function [TypeOf](header As RowObject, ParamArray types As Type()) As Type
            Dim scores As New List(Of (Type, Integer))
            Dim headers As New Index(Of String)(header)

            For Each schema In types.Select(AddressOf SchemaProvider.CreateObjectInternal)
                Dim allNames$() = schema.Properties _
                                        .Select(Function(x) x.Name) _
                                        .ToArray
                Dim matches = Aggregate p As String
                              In allNames
                              Where headers.IndexOf(p) > -1
                              Into Sum(1)

                scores += (schema.DeclaringType, matches)
            Next

            Dim desc = From score As (type As Type, score%)
                       In scores
                       Select type = score.Item1, Value = score.Item2
                       Order By Value Descending
            Dim target As Type = desc.FirstOrDefault?.type

            Return target
        End Function

        Const NullLocationRef$ = "Sorry, the ``path`` reference to a null location!"

        ''' <summary>
        ''' Save this csv document into a specific file location <paramref name="path"/>.
        ''' </summary>
        ''' <param name="path">
        ''' 假若路径是指向一个已经存在的文件，则原有的文件数据将会被清空覆盖
        ''' </param>
        ''' <remarks>当目标保存路径不存在的时候，会自动创建文件夹</remarks>
        <Extension>
        Public Function SaveDataFrame(csv As IEnumerable(Of RowObject),
                                      Optional path$ = "",
                                      Optional encoding As Encoding = Nothing,
                                      Optional tsv As Boolean = False) As Boolean

            Dim stopwatch As Stopwatch = Stopwatch.StartNew
            Dim del$ = ","c Or ASCII.TAB.AsDefault(Function() tsv)

            If path.StringEmpty Then
                Throw New NullReferenceException(NullLocationRef)
            End If

            Using out As StreamWriter = path.OpenWriter(encoding Or UTF8)
                For Each line$ In csv.Select(Function(r) r.AsLine(del))
                    Call out.WriteLine(line)
                Next
            End Using

            Call $"Generate csv file document using time {stopwatch.ElapsedMilliseconds} ms.".__INFO_ECHO

            Return True
        End Function
    End Module
End Namespace
