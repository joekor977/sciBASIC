﻿#Region "Microsoft.VisualBasic::15939da101ac28942dc583364c4fdc73, Microsoft.VisualBasic.Core\ComponentModel\Ranges\Selector\SelectorExtensions.vb"

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

    '     Module SelectorExtensions
    ' 
    '         Function: OrderSelector, Values
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices

Namespace ComponentModel.Ranges

    Public Module SelectorExtensions

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function OrderSelector(Of T As IComparable)(src As IEnumerable(Of T), Optional asc As Boolean = True) As OrderSelector(Of T)
            Return New OrderSelector(Of T)(src, asc)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function Values(Of T)(numerics As IEnumerable(Of NumericTagged(Of T))) As IEnumerable(Of T)
            Return numerics.Select(Function(x) x.value)
        End Function
    End Module
End Namespace
