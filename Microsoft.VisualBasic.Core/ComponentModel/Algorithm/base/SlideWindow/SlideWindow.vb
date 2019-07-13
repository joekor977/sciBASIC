﻿#Region "Microsoft.VisualBasic::7975f639c9878a467e69a57bf62f0424, Microsoft.VisualBasic.Core\ComponentModel\Algorithm\base\SlideWindow\SlideWindow.vb"

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

    '     Structure SlideWindow
    ' 
    '         Properties: Index, Items, left, Length, right
    ' 
    '         Function: GetEnumerator, GetEnumerator1, ToString
    ' 
    '         Sub: Assign
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace ComponentModel.Algorithm.base

    ''' <summary>
    ''' A slide window data model.(滑窗操作的数据模型)
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <remarks></remarks>
    Public Structure SlideWindow(Of T)
        Implements IEnumerable(Of T), IAddressOf
        Implements IGrouping(Of Integer, T)
        Implements Value(Of T()).IValueOf
        Implements IRange(Of Integer)

        ''' <summary>
        ''' The position of the current Windows in the Windows list.(在创建的滑窗的队列之中当前的窗口对象的位置)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Index As Integer Implements IAddressOf.Address, IGrouping(Of Integer, T).Key
        ''' <summary>
        ''' The elements in this slide window.(这个划窗之中的元素的列表)
        ''' </summary>
        ''' <returns></returns>
        Public Property Items As T() Implements Value(Of T()).IValueOf.Value

#Region "Index Range"

        Default Public ReadOnly Property Item(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                If index < 0 Then
                    Return Items(Length + index)
                Else
                    Return Items(index)
                End If
            End Get
        End Property

        ''' <summary>
        ''' The left start position of the current slide Windows segment on the original sequence.
        ''' (当前窗口在原始的序列之中的左端起始位点)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property left As Integer Implements IRange(Of Integer).Min

        Public ReadOnly Property right As Integer Implements IRange(Of Integer).Max
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return left + Length
            End Get
        End Property

#End Region

        ''' <summary>
        ''' The length of the slide window.(窗口长度)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Length As Integer
            Get
                If Items.IsNullOrEmpty Then
                    Return 0
                Else
                    Return Items.Length
                End If
            End Get
        End Property

        Public Sub Assign(address As Integer) Implements IAddress(Of Integer).Assign
            Index = address
        End Sub

        Public Overrides Function ToString() As String
            Return $"{Index} --> {Items.GetJson}"
        End Function

        Public Iterator Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
            For Each o As T In Items
                Yield o
            Next
        End Function

        Private Iterator Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
            Yield GetEnumerator()
        End Function
    End Structure
End Namespace
