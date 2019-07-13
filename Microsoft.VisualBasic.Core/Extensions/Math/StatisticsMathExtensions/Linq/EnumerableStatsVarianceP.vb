﻿#Region "Microsoft.VisualBasic::473e67c72806d54d9b38834ee8ef01b7, Microsoft.VisualBasic.Core\Extensions\Math\StatisticsMathExtensions\Linq\EnumerableStatsVarianceP.vb"

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

    '     Module EnumerableStatsVarianceP
    ' 
    '         Function: (+20 Overloads) VarianceP
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports sys = System.Math

Namespace Math.Statistics.Linq

    Public Module EnumerableStatsVarianceP
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of nullable System.Decimal values.
        '
        ' Parameters:
        '   source:
        '     A sequence of nullable System.Decimal values to calculate the VarianceP of.
        '
        ' Returns:
        '     The VarianceP of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Decimal.MaxValue.
        <Extension>
        Public Function VarianceP(source As IEnumerable(Of Decimal?)) As Decimal
            Dim values As IEnumerable(Of Decimal) = source.Coalesce()
            If values.Any() Then
                Return values.VarianceP()
            End If

            Return Nothing
        End Function
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of System.Decimal values.
        '
        ' Parameters:
        '   source:
        '     A sequence of System.Decimal values to calculate the VarianceP of.
        '
        ' Returns:
        '     The VarianceP of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Decimal.MaxValue.
        <Extension>
        Public Function VarianceP(source As IEnumerable(Of Decimal)) As Decimal
            Return CDec(source.[Select](Function(x) CDbl(x)).VarianceP())
        End Function
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of nullable System.Double values.
        '
        ' Parameters:
        '   source:
        '     A sequence of nullable System.Double values to calculate the VarianceP of.
        '
        ' Returns:
        '     The VarianceP of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        <Extension>
        Public Function VarianceP(source As IEnumerable(Of Double?)) As Double
            Dim values As IEnumerable(Of Double) = source.Coalesce()
            If values.Any() Then
                Return values.VarianceP()
            End If

            Return Nothing
        End Function
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of System.Double values.
        '
        ' Parameters:
        '   source:
        '     A sequence of System.Double values to calculate the VarianceP of.
        '
        ' Returns:
        '     The VarianceP of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        <Extension>
        Public Function VarianceP(source As IEnumerable(Of Double)) As Double
            Dim avg As Double = source.Average()
            Dim d As Double = source.Aggregate(0.0, func:=Function(total, [next]) As Double
                                                              total += sys.Pow([next] - avg, 2)
                                                              Return total
                                                          End Function)
            Return d / source.Count()
        End Function
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of nullable System.Single values.
        '
        ' Parameters:
        '   source:
        '     A sequence of nullable System.Single values to calculate the VarianceP of.
        '
        ' Returns:
        '     The VarianceP of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        <Extension>
        Public Function VarianceP(source As IEnumerable(Of Single?)) As Single
            Dim values As IEnumerable(Of Single) = source.Coalesce()
            If values.Any() Then
                Return values.VarianceP()
            End If

            Return Nothing
        End Function
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of System.Single values.
        '
        ' Parameters:
        '   source:
        '     A sequence of System.Single values to calculate the VarianceP of.
        '
        ' Returns:
        '     The VarianceP of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        <Extension>
        Public Function VarianceP(source As IEnumerable(Of Single)) As Single
            Return CSng(source.[Select](Function(x) CDbl(x)).VarianceP())
        End Function
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of nullable System.Int32 values.
        '
        ' Parameters:
        '   source:
        '     A sequence of nullable System.Int32values to calculate the VarianceP of.
        '
        ' Returns:
        '     The VarianceP of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Int64.MaxValue.
        <Extension>
        Public Function VarianceP(source As IEnumerable(Of Integer?)) As Double
            Dim values As IEnumerable(Of Integer) = source.Coalesce()
            If values.Any() Then
                Return values.VarianceP()
            End If

            Return Nothing
        End Function
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of System.Int32 values.
        '
        ' Parameters:
        '   source:
        '     A sequence of System.Int32 values to calculate the VarianceP of.
        '
        ' Returns:
        '     The VarianceP of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Int64.MaxValue.
        <Extension>
        Public Function VarianceP(source As IEnumerable(Of Integer)) As Double
            Return source.[Select](Function(x) CDbl(x)).VarianceP()
        End Function
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of nullable System.Int64 values.
        '
        ' Parameters:
        '   source:
        '     A sequence of nullable System.Int64 values to calculate the VarianceP of.
        '
        ' Returns:
        '     The VarianceP of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Int64.MaxValue.
        <Extension>
        Public Function VarianceP(source As IEnumerable(Of Long?)) As Double
            Dim values As IEnumerable(Of Long) = source.Coalesce()
            If values.Any() Then
                Return values.VarianceP()
            End If

            Return Nothing
        End Function
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of System.Int64 values.
        '
        ' Parameters:
        '   source:
        '     A sequence of System.Int64 values to calculate the VarianceP of.
        '
        ' Returns:
        '     The VarianceP of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Int64.MaxValue.
        <Extension>
        Public Function VarianceP(source As IEnumerable(Of Long)) As Double
            Return source.[Select](Function(x) CDbl(x)).VarianceP()
        End Function
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of nullable System.Decimal values that
        '     are obtained by invoking a transform function on each element of the input
        '     sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values to calculate the VarianceP of.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The VarianceP of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source or selector is null.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Decimal.MaxValue.
        <Extension>
        Public Function VarianceP(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Decimal?)) As Decimal
            Return source.[Select](selector).VarianceP()
        End Function
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of System.Decimal values that are obtained
        '     by invoking a transform function on each element of the input sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values that are used to calculate an VarianceP.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The VarianceP of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source or selector is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Decimal.MaxValue.
        <Extension>
        Public Function VarianceP(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Decimal)) As Decimal
            Return source.[Select](selector).VarianceP()
        End Function
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of nullable System.Double values that
        '     are obtained by invoking a transform function on each element of the input
        '     sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values to calculate the VarianceP of.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The VarianceP of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source or selector is null.
        <Extension>
        Public Function VarianceP(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Double?)) As Double
            Return source.[Select](selector).VarianceP()
        End Function
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of System.Double values that are obtained
        '     by invoking a transform function on each element of the input sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values to calculate the VarianceP of.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The VarianceP of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source or selector is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        <Extension>
        Public Function VarianceP(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Double)) As Double
            Return source.[Select](selector).VarianceP()
        End Function
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of nullable System.Single values that
        '     are obtained by invoking a transform function on each element of the input
        '     sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values to calculate the VarianceP of.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The VarianceP of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source or selector is null.
        <Extension>
        Public Function VarianceP(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Single?)) As Single
            Return source.[Select](selector).VarianceP()
        End Function
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of System.Single values that are obtained
        '     by invoking a transform function on each element of the input sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values to calculate the VarianceP of.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The VarianceP of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source or selector is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        <Extension>
        Public Function VarianceP(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Single)) As Single
            Return source.[Select](selector).VarianceP()
        End Function
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of nullable System.Int32 values that are
        '     obtained by invoking a transform function on each element of the input sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values to calculate the VarianceP of.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The VarianceP of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source or selector is null.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Int64.MaxValue.
        <Extension>
        Public Function VarianceP(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Integer?)) As Double
            Return source.[Select](selector).VarianceP()
        End Function
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of System.Int32 values that are obtained
        '     by invoking a transform function on each element of the input sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values to calculate the VarianceP of.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The VarianceP of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source or selector is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Int64.MaxValue.
        <Extension>
        Public Function VarianceP(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Integer)) As Double
            Return source.[Select](selector).VarianceP()
        End Function
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of nullable System.Int64 values that are
        '     obtained by invoking a transform function on each element of the input sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values to calculate the VarianceP of.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The VarianceP of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        <Extension>
        Public Function VarianceP(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Long?)) As Double
            Return source.[Select](selector).VarianceP()
        End Function
        '
        ' Summary:
        '     Computes the VarianceP of a sequence of System.Int64 values that are obtained
        '     by invoking a transform function on each element of the input sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values to calculate the VarianceP of.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The VarianceP of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source or selector is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Int64.MaxValue.
        <Extension>
        Public Function VarianceP(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Long)) As Double
            Return source.[Select](selector).VarianceP()
        End Function
    End Module
End Namespace
