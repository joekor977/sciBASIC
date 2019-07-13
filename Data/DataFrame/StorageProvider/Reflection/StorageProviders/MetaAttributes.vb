﻿#Region "Microsoft.VisualBasic::2a4a949334b0df68c216c2dd7ee9c597, Data\DataFrame\StorageProvider\Reflection\StorageProviders\MetaAttributes.vb"

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

    '     Module MetaAttributeParser
    ' 
    '         Function: GetEntry, LoadData, MakeDictionaryType
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Reflection
Imports Microsoft.VisualBasic.Data.csv.StorageProvider.ComponentModels
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq

Namespace StorageProvider.Reflection

    ''' <summary>
    ''' 解析出字典域标记信息
    ''' </summary>
    Public Module MetaAttributeParser

        Public Function GetEntry(type As Type) As ComponentModels.MetaAttribute
            Dim attrEntry As Type = GetType(Reflection.MetaAttribute)
            Dim metaAttr = LinqAPI.DefaultFirst(Of ComponentModels.MetaAttribute) _
 _
                () <= From prop As PropertyInfo
                      In type.GetProperties(BindingFlags.Public Or BindingFlags.Instance)
                      Let attrs As Object() = prop.GetCustomAttributes(attrEntry, inherit:=True)
                      Where Not attrs.IsNullOrEmpty
                      Let mattr As MetaAttribute = DirectCast(attrs.First, MetaAttribute)
                      Select New ComponentModels.MetaAttribute(mattr, prop)

            Return metaAttr
        End Function

        ''' <summary>
        ''' 将csv文档里面的数据加载进入对象数组的字典属性之中
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="buf"></param>
        ''' <param name="DataSource"></param>
        ''' <param name="Schema"></param>
        ''' <returns></returns>
        Public Function LoadData(Of T As Class)(buf As IEnumerable(Of T), DataSource As DynamicObjectLoader(), Schema As SchemaProvider) As IEnumerable(Of T)
            If Not Schema.HasMetaAttributes Then
                Return buf
            End If

            Dim MetaEntry As ComponentModels.MetaAttribute = Schema.MetaAttributes
            Dim type As Type = MetaEntry.MetaAttribute.TypeId
            Dim typeCast As Scripting.LoadObject = Scripting.CasterString(type)
            Dim attrs = LinqAPI.Exec(Of KeyValuePair(Of String, Integer)) _
 _
                () <= From Name As KeyValuePair(Of String, Integer)
                      In DataSource.First.Schema
                      Where Not String.IsNullOrEmpty(Name.Key) AndAlso
                          Not Schema.ContainsField(Name.Key)
                      Select Name

            Dim hashType As Type = MakeDictionaryType(type)
            Dim o As Object

            For Each x As SeqValue(Of T) In buf.SeqIterator
                Dim hash As IDictionary =
                    DirectCast(Activator.CreateInstance(hashType), IDictionary)
                Dim source As DynamicObjectLoader = DataSource(x.i)

                For Each attrName As KeyValuePair(Of String, Integer) In attrs
                    o = typeCast(source.GetValue(attrName.Value))
                    Call hash.Add(attrName.Key, o)
                Next

                Call MetaEntry.BindProperty.SetValue(x.value, hash, Nothing)
            Next

            Return buf
        End Function

        ''' <summary>
        ''' Function returns type of <see cref="Dictionary(Of String, ValueType)"/>
        ''' </summary>
        ''' <param name="ValueType">Type of the value in the dictionary, and the key type is <see cref="String"/></param>
        ''' <returns></returns>
        Public Function MakeDictionaryType(ValueType As Type) As Type
            Dim GenericType As Type = GetType(Dictionary(Of ,)) ' Type.GetType("System.Collections.Generic.IEnumerable")
            GenericType = GenericType.MakeGenericType({GetType(String), ValueType})
            Return GenericType
        End Function
    End Module
End Namespace
