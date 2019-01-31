﻿#Region "Microsoft.VisualBasic::857576b799b84ac584db0baddc575e25, Data_science\MachineLearning\NeuralNetwork\ActiveFunctions\ReLU.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:

    '     Class ReLU
    ' 
    '         Properties: Store
    ' 
    '         Function: [Function], Derivative, Derivative2
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.MachineLearning.NeuralNetwork.StoreProcedure

Namespace NeuralNetwork.Activations

    Public Class ReLU : Implements IActivationFunction

        Public ReadOnly Property Store As ActiveFunction Implements IActivationFunction.Store
            Get
                Return New ActiveFunction With {
                    .Arguments = {},
                    .name = NameOf(ReLU)
                }
            End Get
        End Property

        Public Function [Function](x As Double) As Double Implements IActivationFunction.Function
            If x < 0 Then
                Return 0.0
            Else
                Return x
            End If
        End Function

        Public Function Derivative(x As Double) As Double Implements IActivationFunction.Derivative
            Return 1
        End Function

        Public Function Derivative2(y As Double) As Double Implements IActivationFunction.Derivative2
            Return 1
        End Function
    End Class
End Namespace