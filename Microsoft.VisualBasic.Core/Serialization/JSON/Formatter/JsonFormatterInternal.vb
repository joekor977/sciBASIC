﻿#Region "Microsoft.VisualBasic::f1977fad5b8481516a06d917ec139c65, ..\sciBASIC#\Microsoft.VisualBasic.Core\Serialization\JSON\Formatter\JsonFormatterInternal.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xieguigang (xie.guigang@live.com)
    '       xie (genetics@smrucc.org)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
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

#End Region

Imports System.Text
Imports Microsoft.VisualBasic.Serialization.JSON.Formatter.Internals.Strategies

Namespace Serialization.JSON.Formatter.Internals

    Friend NotInheritable Class JsonFormatterInternal

        ReadOnly context As JsonFormatterStrategyContext

        Public Sub New(context As JsonFormatterStrategyContext)
            Me.context = context

            Me.context.ClearStrategies()
            Me.context.AddCharacterStrategy(New OpenBracketStrategy())
            Me.context.AddCharacterStrategy(New CloseBracketStrategy())
            Me.context.AddCharacterStrategy(New OpenSquareBracketStrategy())
            Me.context.AddCharacterStrategy(New CloseSquareBracketStrategy())
            Me.context.AddCharacterStrategy(New SingleQuoteStrategy())
            Me.context.AddCharacterStrategy(New DoubleQuoteStrategy())
            Me.context.AddCharacterStrategy(New CommaStrategy())
            Me.context.AddCharacterStrategy(New ColonCharacterStrategy())
            Me.context.AddCharacterStrategy(New SkipWhileNotInStringStrategy(ControlChars.Lf))
            Me.context.AddCharacterStrategy(New SkipWhileNotInStringStrategy(ControlChars.Cr))
            Me.context.AddCharacterStrategy(New SkipWhileNotInStringStrategy(ControlChars.Tab))
            Me.context.AddCharacterStrategy(New SkipWhileNotInStringStrategy(" "c))
        End Sub

        Public Function Format(json As String) As String
            If json Is Nothing Then
                Return String.Empty
            End If

            If json.Trim() = String.Empty Then
                Return String.Empty
            End If

            Dim input As New StringBuilder(json)
            Dim output As New StringBuilder()

            Me.PrettyPrintCharacter(input, output)

            Return output.ToString()
        End Function

        Private Sub PrettyPrintCharacter(input As StringBuilder, output As StringBuilder)
            For i As Integer = 0 To input.Length - 1
                Me.context.PrettyPrintCharacter(input(i), output)
            Next
        End Sub
    End Class
End Namespace
