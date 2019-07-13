﻿#Region "Microsoft.VisualBasic::462f070c73adc8af55140fad8bccaa36, Data\BinaryData\DataStorage\netCDF\Data\DataReader.vb"

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

    '     Module DataReader
    ' 
    '         Function: nonRecord, record
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports Microsoft.VisualBasic.Data.IO.netCDF.Components

Namespace netCDF

    ''' <summary>
    ''' Data reader methods for a given variable data value.
    ''' (在这个模块之中读取<see cref="variable.value"/>数据变量的值)
    ''' </summary>
    Module DataReader

        ''' <summary>
        ''' Read data for the given non-record variable
        ''' </summary>
        ''' <param name="buffer">Buffer for the file data</param>
        ''' <param name="variable">Variable metadata</param>
        ''' <returns>Data of the element</returns>
        ''' <remarks>
        ''' 非记录类型则是一个数组
        ''' </remarks>
        Public Function nonRecord(buffer As BinaryDataReader, variable As variable) As Object()
            ' size of the data
            Dim size = variable.size / sizeof(variable.type)
            ' iterates over the data
            Dim data As Object() = New Object(size - 1) {}

            ' 读取的结果是一个T()数组
            For i As Integer = 0 To size - 1
                data(i) = TypeExtensions.readType(buffer, variable.type, 1)
            Next

            Return data
        End Function

        ''' <summary>
        ''' Read data for the given record variable
        ''' </summary>
        ''' <param name="buffer">Buffer for the file data</param>
        ''' <param name="variable">Variable metadata</param>
        ''' <param name="recordDimension">Record dimension metadata</param>
        ''' <returns>Data of the element</returns>
        ''' <remarks>
        ''' 记录类型的数据可能是一个矩阵类型
        ''' </remarks>
        Public Function record(buffer As BinaryDataReader, variable As variable, recordDimension As recordDimension) As Object()
            Dim width% = If(variable.size, variable.size / sizeof(variable.type), 1)
            ' size of the data
            ' TODO streaming data
            Dim size = recordDimension.length
            ' iterates over the data
            Dim data As Object() = New Object(size - 1) {}
            Dim [step] = recordDimension.recordStep

            ' 读取的结果可能是一个T()()矩阵或者T()数组
            For i As Integer = 0 To size - 1
                Dim currentOffset& = buffer.Position
                Dim nextOffset = currentOffset + [step]

                If buffer.EndOfStream Then
                    data(i) = Nothing
                Else
                    data(i) = TypeExtensions.readType(buffer, variable.type, width)
                    buffer.Seek(nextOffset, SeekOrigin.Begin)
                End If
            Next

            Return data
        End Function
    End Module
End Namespace
