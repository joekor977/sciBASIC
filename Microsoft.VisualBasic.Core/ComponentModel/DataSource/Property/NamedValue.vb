﻿#Region "Microsoft.VisualBasic::529b3ac85939959a3564dad680f53aa1, Microsoft.VisualBasic.Core\ComponentModel\DataSource\Property\NamedValue.vb"

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

    '     Structure NamedValue
    ' 
    '         Properties: Description, IsEmpty, Name, Value, ValueType
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: FixValue, ToString
    '         Operators: (+2 Overloads) +, <>, =
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports System.Web.Script.Serialization
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.Language.Default

Namespace ComponentModel.DataSourceModel

    ''' <summary>
    ''' The value object have a name string.(一个具有自己的名称的变量值)
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    Public Structure NamedValue(Of T) : Implements INamedValue
        Implements IKeyValuePairObject(Of String, T)
        Implements IsEmpty

        ''' <summary>
        ''' Identifier tag data. you can using this property value as a dictionary key.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttribute>
        Public Property Name As String Implements INamedValue.Key, IKeyValuePairObject(Of String, T).Key

        ''' <summary>
        ''' Object value
        ''' </summary>
        ''' <returns></returns>
        <XmlElement> Public Property Value As T Implements IKeyValuePairObject(Of String, T).Value

        ''' <summary>
        ''' Additional description text about this variable <see cref="Value"/>
        ''' </summary>
        ''' <returns></returns>
        <XmlAttribute>
        Public Property Description As String

        ''' <summary>
        ''' Does this object have value?
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore, ScriptIgnore, DataIgnored>
        Public ReadOnly Property IsEmpty As Boolean Implements IsEmpty.IsEmpty
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return String.IsNullOrEmpty(Name) AndAlso Value Is Nothing
            End Get
        End Property

        ''' <summary>
        ''' Returns object gettype
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ValueType As Type
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                ' 假若类型参数T是基类型的话，则直接使用GetType(T)只能够得到基类型的信息，无法得到当前的实现类型的信息
                ' 故而要在这里使用对象自身的GetType方法来获取真正的类型信息
                Return Value.GetType
            End Get
        End Property

        ''' <summary>
        ''' Creates a object bind with a specific <see cref="Name"/>.
        ''' </summary>
        ''' <param name="name"></param>
        ''' <param name="value"></param>
        Sub New(name$, Optional value As T = Nothing, Optional describ As String = Nothing)
            Me.Name = name
            Me.Value = value
            Me.Description = describ
        End Sub

        ''' <summary>
        ''' View object.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Try
                ' 2018-4-9 在这里将显示修改为类型的ToString方法，这样子就可以由
                ' 用户来通过重写ToString方法来自定义显示，而非强制使用GetJson方法
                ' 将全部的对象都显示出来，对于属性很多的对象GetJson方法显示的效果不是太好
                If Description.StringEmpty Then
                    Return $"{Name} --> {Value.ToString}"
                Else
                    Return $"{Name} --> {Value.ToString} ({Description})"
                End If
            Catch ex As Exception
                Return Name
            End Try
        End Function

        Public Function FixValue(h As Func(Of T, T)) As NamedValue(Of T)
            Return New NamedValue(Of T)(Name, h(Value))
        End Function

        Public Shared Operator +(obj As NamedValue(Of T)) As T
            Return obj.Value
        End Operator

        ''' <summary>
        ''' 这个函数会将<paramref name="value"/>插入为<paramref name="table"/>的第一个元素
        ''' </summary>
        ''' <param name="value"></param>
        ''' <param name="table"></param>
        ''' <returns></returns>
        Public Shared Operator +(value As NamedValue(Of T), table As Dictionary(Of String, T)) As Dictionary(Of String, T)
            Dim newTable As New Dictionary(Of String, T) From {
                {value.Name, value.Value}
            }

            For Each map As KeyValuePair(Of String, T) In table
                Call newTable.Add(map.Key, map.Value)
            Next

            Return newTable
        End Operator

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Operator =(tuple As NamedValue(Of T), compares As T) As Boolean
            If tuple.Value Is Nothing Then
                If compares Is Nothing Then
                    Return True
                Else
                    Return False
                End If
            Else
                If compares Is Nothing Then
                    Return False
                Else
                    Return tuple.Value.Equals(compares)
                End If
            End If
        End Operator

        ''' <summary>
        ''' Convert from tuple
        ''' </summary>
        ''' <param name="tuple"></param>
        ''' <returns></returns>
        ''' 
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(tuple As (name$, value As T)) As NamedValue(Of T)
            Return New NamedValue(Of T)(tuple.name, tuple.value)
        End Operator

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(tuple As (name$, value As T, describ$)) As NamedValue(Of T)
            Return New NamedValue(Of T)(tuple.name, tuple.value, tuple.describ)
        End Operator

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Operator <>(tuple As NamedValue(Of T), compares As T) As Boolean
            Return Not tuple = compares
        End Operator

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overloads Shared Narrowing Operator CType(value As NamedValue(Of T)) As T
            Return value.Value
        End Operator
    End Structure
End Namespace
