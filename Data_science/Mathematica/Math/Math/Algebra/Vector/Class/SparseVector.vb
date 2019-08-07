﻿Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Language.Vectorization
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Linq.Which

Namespace LinearAlgebra

    ''' <summary>
    ''' 稀疏的向量
    ''' </summary>
    ''' <remarks>
    ''' 在这个向量中存在大量的零，主要适用于节省计算内存
    ''' 因为有<see cref="index"/>索引的存在，所以假若零数值比较少的话，
    ''' 使用这个稀疏向量来存储数据反而会导致内存被过度占用
    ''' </remarks>
    Public Class SparseVector : Inherits Vector

        ''' <summary>
        ''' 非零值的索引号
        ''' </summary>
        ''' <remarks>
        ''' 为了保持访问性能，<see cref="buffer"/>数组并不会频繁的更改其长度
        ''' 所以在设置某个元素为零的时候，是通过将这个索引对应的元素设置为-1进行标记删除的
        ''' -1表示空缺下来的元素
        ''' </remarks>
        ReadOnly index As List(Of Integer)
        ReadOnly dimension%

        Public Overrides ReadOnly Property Length As Integer
            Get
                Return dimension
            End Get
        End Property

        Public Overrides ReadOnly Property [Dim] As Integer
            Get
                Return dimension
            End Get
        End Property

#Region "Index properties"
        Default Public Overrides Property Item(booleans As IEnumerable(Of Boolean)) As Vector(Of Double)
            Get
                Return New Vector(Of Double)(IsTrue(booleans).Select(Function(index) Me(index)))
            End Get
            Set(value As Vector(Of Double))
                For Each index As SeqValue(Of Integer) In IsTrue(booleans).SeqIterator
                    Me(index.value) = value(index)
                Next
            End Set
        End Property

        Default Public Overrides Property Item(index As Integer) As Double
            Get
                Dim i As Integer = Me.index.IndexOf(index)

                If i = -1 Then
                    Return 0
                Else
                    Return buffer(i)
                End If
            End Get
            Set
                Dim i As Integer = Me.index.IndexOf(index)

                If Value = 0.0 Then
                    If i = -1 Then
                        ' 将原来的零值设置为零值，则无变化
                        ' do nothing
                    Else
                        ' 将非零值设置为零
                        Me.index.Remove(index)
                        Me.buffer(i) = 0
                    End If
                End If
            End Set
        End Property

        Default Public Overloads Property Item(range As IEnumerable(Of Integer)) As Vector
            Get
                Return New SparseVector(range.Select(Function(index) Me(index)))
            End Get
            Set
                For Each index As SeqValue(Of Integer) In range.SeqIterator
                    Me(index.value) = Value(index)
                Next
            End Set
        End Property

        Default Public Overloads Property Item(range As IntRange) As SparseVector
            Get
                Return Me(range.ToArray)
            End Get
            Set
                Me(range.ToArray) = Value
            End Set
        End Property
#End Region

        ''' <summary>
        ''' All of the element its ABS value less than this precision threshold will be treated as ZERO value
        ''' So the larger of this threshold value, the lower precision precision of all of the math algorithm
        ''' that related to this <see cref="SparseVector"/>.
        ''' 
        ''' (当元素的绝对值小于这个值之后就会被当作为零，可以根据情况来设置这个公用属性来控制稀疏向量的计算精度)
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property Precision As Double = 0.00001

        Const PrecisionEnvironmentConfigName$ = "/sparse_vector.zero_precision"

        Shared Sub New()
            Dim precision$ = App.GetVariable(PrecisionEnvironmentConfigName)

            If Not precision.StringEmpty Then
                SparseVector.Precision = precision.ParseDouble

                Call $"The precision controls config of the sparse vector was set to {precision}".__INFO_ECHO
            End If
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="data"></param>
        ''' <param name="index"></param>
        ''' <param name="length">
        ''' 因为存在大量零，数组中并不是存储真实的数据，而是非零值，所以在这里必须要有一个长度来标记出当前的这个向量的长度
        ''' </param>
        Private Sub New(data As IEnumerable(Of Double), index As IEnumerable(Of Integer), length%)
            Call MyBase.New(data)

            Me.index = index.AsList
            Me.dimension = length
        End Sub

        Sub New(data As IEnumerable(Of Double))
            Dim dimension As Integer = 0
            Dim buffer As New List(Of Double)

            index = New List(Of Integer)

            For Each x As Double In data
                If Math.Abs(x) < Precision Then
                    ' 0.0
                Else
                    ' has a non-ZERO value
                    index += dimension
                    buffer += x
                End If

                dimension += 1
            Next

            Me.dimension = dimension
            Me.buffer = buffer
        End Sub

        Public Overloads Shared Function Equals(a#, b#) As Boolean
            Return Math.Abs(a - b) <= Precision
        End Function

        Public Overrides Iterator Function GetEnumerator() As IEnumerator(Of Double)
            Dim j As VBInteger = -1

            For i As Integer = 0 To dimension - 1
                If (j = index.IndexOf(i)) > -1 Then
                    Yield buffer(j)
                Else
                    Yield 0.0
                End If
            Next
        End Function

        Public Overloads Shared Operator *(v As SparseVector, multipl As Double) As SparseVector
            Return New SparseVector(From x As Double In v Select x * multipl)
        End Operator

        Public Overloads Shared Operator /(v As SparseVector, div As Double) As SparseVector
            Return New SparseVector(From x As Double In v Select x / div)
        End Operator

        Public Overloads Shared Operator -(v As SparseVector, minus As Double) As SparseVector
            Return New SparseVector(From x As Double In v Select x - minus)
        End Operator

        Public Overloads Shared Operator +(v As SparseVector, add As Double) As SparseVector
            Return New SparseVector(From x As Double In v Select x + add)
        End Operator

        Public Overloads Shared Operator ^(v As SparseVector, p As Double) As SparseVector
            Return New SparseVector(From x As Double In v Select x ^ p)
        End Operator

        Public Overloads Shared Operator ^(x As SparseVector, y As Vector) As SparseVector
            If x.Dim <> y.Dim Then
                Throw New InvalidConstraintException()
            Else
                Return New SparseVector(From i As Integer In x.Sequence Select x(i) ^ y(i))
            End If
        End Operator
    End Class
End Namespace