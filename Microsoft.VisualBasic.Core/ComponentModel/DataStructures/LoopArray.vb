﻿#Region "Microsoft.VisualBasic::6cbca453e2ad0186dd0aa5e35dce4f3a, Microsoft.VisualBasic.Core\ComponentModel\DataStructures\LoopArray.vb"

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

    '     Class LoopArray
    ' 
    '         Properties: Buffer, Current, Length
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: [GET], [Next], GetEnumerator, IEnumerable_GetEnumerator, ToString
    ' 
    '         Sub: [Set], Break, Reset
    ' 
    '         Operators: +
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace ComponentModel.DataStructures

    ''' <summary>
    ''' Infinite loop iterates of the target element collection.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    Public Class LoopArray(Of T) : Implements IEnumerable(Of T)

        Dim array As T()
        Dim p%

        Public ReadOnly Property Buffer As T()
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return array
            End Get
        End Property

        Public ReadOnly Property Length As Integer
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return array.Length
            End Get
        End Property

        Public ReadOnly Property Current As SeqValue(Of T)
            Get
                Return New SeqValue(Of T)() With {
                    .i = p,
                    .value = array(.i)
                }
            End Get
        End Property

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Sub New(source As IEnumerable(Of T))
            array = source.ToArray
        End Sub

        ''' <summary>
        ''' Gets the next elements in the array, is move to end, then the index will 
        ''' moves to the array begining position.
        ''' </summary>
        ''' <returns></returns>
        Public Function [Next]() As T
            Dim i As Integer = p

            If p < array.Length - 1 Then
                p += 1
            Else
                p = 0
            End If

            Return array(i)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub [Set](i%)
            p = i%
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub Reset()
            p = 0
        End Sub

        Public Overrides Function ToString() As String
            Return array.Take(10).ToArray.GetJson
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="delta%">The pointer move delta</param>
        ''' <returns></returns>
        Public Function [GET](delta%) As T
            p += delta

            If p >= 0 Then
                If p <= array.Length - 1 Then
                    ' 正常的下标范围内，不需要进行任何处理
                Else
                    p = p - array.Length
                End If
            Else
                p = array.Length + p
            End If

            Return array(p)
        End Function

        Public Shared Narrowing Operator CType(array As LoopArray(Of T)) As T()
            Return array.Buffer
        End Operator

        Public Shared Widening Operator CType(array As T()) As LoopArray(Of T)
            Return New LoopArray(Of T)(array)
        End Operator

        Public Shared Operator +(array As LoopArray(Of T)) As SeqValue(Of T)
            Call array.Next()
            Return array.Current
        End Operator

        Dim _break As Boolean

        ''' <summary>
        ''' Exit the Infinite loop iterator <see cref="GetEnumerator()"/>
        ''' </summary>
        Public Sub Break()
            _break = True
        End Sub

        ''' <summary>
        ''' Infinite loop iterates of the target element collection.
        ''' </summary>
        ''' <returns></returns>
        Public Iterator Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
            _break = False

            Do While Not _break
                Yield [Next]()
            Loop
        End Function

        Private Iterator Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Yield GetEnumerator()
        End Function
    End Class
End Namespace
