﻿#Region "Microsoft.VisualBasic::9ce82021a245cc6bda6eafdf0907a545, Data_science\MachineLearning\MachineLearning\NeuralNetwork\ActiveFunctions\IActivationFunction.vb"

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

    '     Class IActivationFunction
    ' 
    '         Properties: Truncate
    ' 
    '         Function: CalculateDerivative
    ' 
    ' 
    ' /********************************************************************************/

#End Region

' AForge Neural Net Library
' AForge.NET framework
'
' Copyright © Andrew Kirillov, 2005-2008
' andrew.kirillov@gmail.com
'

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.MachineLearning.NeuralNetwork.StoreProcedure

Namespace NeuralNetwork.Activations

    ''' <summary>
    ''' Activation function interface.
    ''' </summary>
    ''' <remarks>
    ''' All activation functions, which are supposed to be used with
    ''' neurons, which calculate their output as a function of weighted sum of
    ''' their inputs, should implement this interfaces.
    ''' </remarks>
    Public MustInherit Class IActivationFunction

        Public MustOverride ReadOnly Property Store As ActiveFunction

        ''' <summary>
        ''' 因为激活函数在求导之后,结果值可能会出现无穷大
        ''' 所以可以利用这个值来限制求导之后的结果最大值
        ''' </summary>
        ''' <returns></returns>
        Public Property Truncate As Double = 100

        Default Public ReadOnly Property Evaluate(x As Double) As Double
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Me.Function(x)
            End Get
        End Property

        Public Overridable Function CalculateDerivative(x As Double) As Double
            If Truncate > 0 Then
                Return ValueTruncate(Derivative(x), Truncate)
            Else
                Return Derivative(x)
            End If
        End Function

        ''' <summary>
        ''' Calculates function value.
        ''' </summary>
        ''' <param name="x">Function input value.</param>
        ''' <returns>Function output value, <i>f(x)</i>.</returns>
        ''' <remarks>
        ''' The method calculates function value at point <paramref name="x"/>.
        ''' </remarks>
        Public MustOverride Function [Function](x As Double) As Double

        ''' <summary>
        ''' Calculates function derivative.
        ''' </summary>
        ''' <param name="x">Function input value.</param>
        ''' <returns>Function derivative, <i>f'(x)</i>.</returns>
        ''' <remarks>
        ''' The method calculates function derivative at point <paramref name="x"/>.
        ''' </remarks>
        Protected MustOverride Function Derivative(x As Double) As Double

        ''' <summary>
        ''' 必须要重写这个函数来将函数对象序列化为表达式字符串文本
        ''' </summary>
        ''' <returns></returns>
        Public MustOverride Overrides Function ToString() As String

    End Class
End Namespace
