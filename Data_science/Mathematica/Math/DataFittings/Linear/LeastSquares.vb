﻿#Region "Microsoft.VisualBasic::e09cc0c2fc1227c9e6ec7e58fae43993, Data_science\Mathematica\Math\DataFittings\Linear\LeastSquares.vb"

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

    ' Module LeastSquares
    ' 
    '     Function: (+3 Overloads) LinearFit, (+3 Overloads) PolyFit, SeriesLength
    ' 
    '     Sub: calcError, gaussSolve
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Math.LinearAlgebra

' 尘中远，于2014.03.20
' 主页：http://blog.csdn.net/czyt1988/article/details/21743595
' 参考：http://blog.csdn.net/maozefa/article/details/1725535

''' <summary>
''' 曲线拟合类，只适用于线性拟合：
''' 
''' + ``y = a*x + b``
''' + ``y = a + a1*x + a2*x^2 + ... + an*x^n``
''' </summary>
Public Module LeastSquares

    ''' <summary>
    ''' 直线拟合-一元回归,拟合的结果可以使用getFactor获取，或者使用getSlope获取斜率，getIntercept获取截距
    ''' </summary>
    ''' <param name="x">观察值的x</param>
    ''' <param name="y">观察值的y</param>
    ''' <returns></returns>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function LinearFit(x As Vector, y As Vector) As FitResult
        Return LinearFit(x.ToArray, y.ToArray, SeriesLength(x, y))
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function LinearFit(x As Double(), y As Double()) As FitResult
        Return LinearFit(x.ToArray, y.ToArray, SeriesLength(x, y))
    End Function

    Public Function LinearFit(x As Double(), y As Double(), length As Integer) As FitResult
        Dim t1 As Double = 0, t2 As Double = 0, t3 As Double = 0, t4 As Double = 0
        Dim factor#() = New Double(1) {}

        For i As Integer = 0 To length - 1
            t1 += x(i) * x(i)
            t2 += x(i)
            t3 += x(i) * y(i)
            t4 += y(i)
        Next

        factor(1) = (t3 * length - t2 * t4) / (t1 * length - t2 * t2)
        factor(0) = (t1 * t4 - t2 * t3) / (t1 * length - t2 * t2)

        Dim result As New FitResult With {
            .Polynomial = New Polynomial With {
                .Factors = factor
            }
        }

        ' 计算误差
        calcError(x, y, length, result)

        Return result
    End Function

    ''' <summary>
    ''' 多项式拟合，拟合y=a0+a1*x+a2*x^2+……+apoly_n*x^poly_n
    ''' </summary>
    ''' <param name="x">观察值的x</param>
    ''' <param name="y">观察值的y</param>
    ''' <param name="poly_n">期望拟合的阶数，若poly_n=2，则y=a0+a1*x+a2*x^2</param>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function PolyFit(x As Vector, y As Vector, poly_n As Integer) As FitResult
        Return PolyFit(x.ToArray, y.ToArray, SeriesLength(x, y), poly_n)
    End Function

    ''' <summary>
    ''' 多项式拟合
    ''' </summary>
    ''' <param name="x#"></param>
    ''' <param name="y#"></param>
    ''' <param name="poly_n%">最高的阶数</param>
    ''' <returns></returns>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function PolyFit(x#(), y#(), poly_n%) As FitResult
        Return PolyFit(x.ToArray, y.ToArray, SeriesLength(x, y), poly_n)
    End Function

    Public Function PolyFit(x As Double(), y As Double(), length%, poly_n%) As FitResult
        Dim i As Integer
        Dim j As Integer
        ' double *tempx,*tempy,*sumxx,*sumxy,*ata;
        Dim tempx As New List(Of Double)(length, 1.0)
        Dim tempy As New List(Of Double)(y) ' y+length ?
        Dim sumxx As New List(Of Double)(poly_n * 2 + 1, fill:=0)
        Dim ata As New List(Of Double)((poly_n + 1) * (poly_n + 1), fill:=0)
        Dim sumxy As New List(Of Double)(poly_n + 1, fill:=0)
        Dim result As New FitResult With {
            .Polynomial = New Polynomial With {
                .Factors = New Double(poly_n) {}
            }
        }

        For i = 0 To 2 * poly_n
            sumxx(i) = 0
            j = 0
            While j < length
                sumxx(i) += tempx(j)
                tempx(j) *= x(j)
                j += 1
            End While
        Next
        For i = 0 To poly_n
            sumxy(i) = 0
            j = 0
            While j < length
                sumxy(i) += tempy(j)
                tempy(j) *= x(j)
                j += 1
            End While
        Next
        For i = 0 To poly_n
            For j = 0 To poly_n
                ata(i * (poly_n + 1) + j) = sumxx(i + j)
            Next
        Next

        Call gaussSolve(poly_n + 1, ata, result.Polynomial.Factors, sumxy)
        ' 计算拟合后的数据并计算误差
        Call calcError(x, y, length, result)

        Return result
    End Function

    ''' <summary>
    ''' 获取两个vector的安全size
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="y"></param>
    ''' <returns>最小的一个长度</returns>
    Public Function SeriesLength(x As IEnumerable(Of Double), y As IEnumerable(Of Double)) As Integer
        Dim xl% = x.Count
        Dim yl% = y.Count
        Return If(xl > yl, yl, xl)
    End Function

    Private Sub calcError(x As Double(), y As Double(), length As Integer, ByRef result As FitResult)
        Dim mean_y As Double = y.Sum / length
        Dim yi#
        Dim err As New List(Of TestPoint)

        For i As Integer = 0 To length - 1
            yi = result.GetY(x(i))

            ' 计算回归平方和
            result.SSR += ((yi - mean_y) * (yi - mean_y))
            ' 残差平方和
            result.SSE += ((yi - y(i)) * (yi - y(i)))

            err += New TestPoint With {
                .X = x(i),
                .Y = y(i),
                .Yfit = yi
            }
        Next

        result.RMSE = Math.Sqrt(result.SSE / CDbl(length))
        result.ErrorTest = err
    End Sub

    Private Sub gaussSolve(n%, ByRef A As List(Of Double), ByRef x#(), ByRef b As List(Of Double))
        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim r As Integer
        Dim max As Double

        For k = 0 To n - 2
            max = Math.Abs(A(k * n + k))
            ' find maxmum
            r = k
            For i = k + 1 To n - 2
                If max < Math.Abs(A(i * n + i)) Then
                    max = Math.Abs(A(i * n + i))
                    r = i
                End If
            Next
            If r <> k Then
                For i = 0 To n - 1
                    ' change array:A[k]&A[r]
                    max = A(k * n + i)
                    A(k * n + i) = A(r * n + i)
                    A(r * n + i) = max
                Next
            End If
            max = b(k)
            ' change array:b[k]&b[r]
            b(k) = b(r)
            b(r) = max
            For i = k + 1 To n - 1
                For j = k + 1 To n - 1
                    A(i * n + j) -= A(i * n + k) * A(k * n + j) / A(k * n + k)
                Next
                b(i) -= A(i * n + k) * b(k) / A(k * n + k)
            Next
        Next

        i = n - 1

        While i >= 0
            j = i + 1
            x(i) = b(i)
            While j < n
                x(i) -= A(i * n + j) * x(j)
                j += 1
            End While
            x(i) /= A(i * n + i)
            i -= 1
        End While
    End Sub
End Module
