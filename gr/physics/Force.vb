﻿#Region "Microsoft.VisualBasic::ff12c433a4da43a5deb6338e381a4ef2, gr\physics\Force.vb"

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

    ' Class Force
    ' 
    '     Properties: angle, source, strength
    ' 
    '     Constructor: (+2 Overloads) Sub New
    ' 
    '     Function: ToString
    ' 
    '     Sub: void
    ' 
    '     Operators: -, (+2 Overloads) *, ^, +, (+2 Overloads) <
    '                (+2 Overloads) <>, (+2 Overloads) =, (+2 Overloads) >
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Math
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Math

''' <summary>
''' 力
''' </summary>
Public Class Force

    ''' <summary>
    ''' 力的大小
    ''' </summary>
    ''' <returns></returns>
    Public Property strength As Double
    ''' <summary>
    ''' 力的方向，与水平的夹角，使用弧度
    ''' </summary>
    ''' <returns></returns>
    Public Property angle As Double
    Public Property source As String

    Sub New()
    End Sub

    Sub New(F#, angle#, <CallerMemberName> Optional trace$ = Nothing)
        Me.strength = F#
        Me.angle = angle
        Me.source = trace
    End Sub

    Public Sub void()
        strength = 0
        angle = 0
    End Sub

    Public Overrides Function ToString() As String
        Dim d$ = angle.ToDegrees.ToString("F2")
        Return $"a={d}, {strength.ToString("F2")}N [{source}]"
    End Function

    Public Shared Operator ^(f As Force, n As Double) As Double
        Return f.strength ^ n
    End Operator

    Public Shared Operator *(x As Double, f As Force) As Double
        Return x * f.strength
    End Operator

    Public Shared Operator *(x As Integer, f As Force) As Double
        Return x * f.strength
    End Operator

    Public Shared Operator =(f As Force, strength#) As Boolean
        Return f.strength = strength
    End Operator

    Public Shared Operator =(f As Force, strength%) As Boolean
        Return Abs(f.strength - strength) <= 0.0001
    End Operator

    Public Shared Operator <>(f As Force, strength#) As Boolean
        Return Not f = strength
    End Operator

    Public Shared Operator <>(f As Force, strength%) As Boolean
        Return Not f = strength
    End Operator

    Public Shared Operator >(strength#, f As Force) As Boolean
        Return strength > f.strength
    End Operator

    Public Shared Operator <(strength#, f As Force) As Boolean
        Return strength < f.strength
    End Operator

    Public Shared Operator >(f1 As Force, f2 As Force) As Boolean
        Return f1.strength > f2.strength
    End Operator

    Public Shared Operator <(f1 As Force, f2 As Force) As Boolean
        Return f1.strength < f2.strength
    End Operator

    ''' <summary>
    ''' 这个力的反向力
    ''' </summary>
    ''' <param name="f"></param>
    ''' <returns></returns>
    Public Shared Operator -(f As Force) As Force
        Return New Force With {
            .strength = f.strength,
            .angle = f.angle + PI,
            .source = $"Reverse({f.source})"
        }
    End Operator

    ''' <summary>
    ''' 使用平行四边形法则进行力的合成
    ''' </summary>
    ''' <param name="f1"></param>
    ''' <param name="f2"></param>
    ''' <returns></returns>
    Public Shared Operator +(f1 As Force, f2 As Force) As Force
        Return Math.ParallelogramLaw(f1, f2)
    End Operator
End Class
