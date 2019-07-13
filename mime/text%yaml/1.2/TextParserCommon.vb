﻿#Region "Microsoft.VisualBasic::ab2cbe52d16bbe81bd3fc924abc28a90, mime\text%yaml\1.2\TextParserCommon.vb"

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

    '     Class YamlParser
    ' 
    '         Properties: Position
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: [Error], GetEorrorMessages, MatchTerminal, MatchTerminalRange, MatchTerminalSet
    '                   MatchTerminalString, (+2 Overloads) TerminalMatch
    ' 
    '         Sub: ClearError, SetInput
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.TagData
Imports Microsoft.VisualBasic.Language

Namespace Grammar

    Partial Public Class YamlParser

        Public Property Position() As Integer
        Public Errors As New List(Of LineValue(Of String))()

        Dim ErrorStatck As New Stack(Of Integer)()
        Dim Input As ParserInput(Of Char)

        Public Sub New()
        End Sub

        Private Sub SetInput(input__1 As ParserInput(Of Char))
            Input = input__1
            Position = 0
        End Sub

        Private Function TerminalMatch(terminal As Char) As Boolean
            If Input.HasInput(_Position) Then
                Dim symbol As Char = Input.GetInputSymbol(_Position)
                Return terminal = symbol
            End If
            Return False
        End Function

        Private Function TerminalMatch(terminal As Char, pos As Integer) As Boolean
            If Input.HasInput(pos) Then
                Dim symbol As Char = Input.GetInputSymbol(pos)
                Return terminal = symbol
            End If
            Return False
        End Function

        ''' <summary>
        ''' 如果当前位置的字符匹配<paramref name="terminal"/>那么位置会前进一个字符
        ''' 否则位置不会发生变化
        ''' </summary>
        ''' <param name="terminal"></param>
        ''' <param name="success"></param>
        ''' <returns></returns>
        Private Function MatchTerminal(terminal As Char, ByRef success As Boolean) As Char
            success = False
            If Input.HasInput(_Position) Then
                Dim symbol As Char = Input.GetInputSymbol(_Position)
                If terminal = symbol Then
                    _Position += 1
                    success = True
                End If
                Return symbol
            End If
            Return ControlChars.NullChar
        End Function

        Private Function MatchTerminalRange(start As Char, [end] As Char, ByRef success As Boolean) As Char
            success = False
            If Input.HasInput(_Position) Then
                Dim symbol As Char = Input.GetInputSymbol(_Position)
                If start <= symbol AndAlso symbol <= [end] Then
                    _Position += 1
                    success = True
                End If
                Return symbol
            End If
            Return ControlChars.NullChar
        End Function

        Private Function MatchTerminalSet(terminalSet As String, isComplement As Boolean, ByRef success As Boolean) As Char
            success = False
            If Input.HasInput(_Position) Then
                Dim symbol As Char = Input.GetInputSymbol(_Position)
                Dim match As Boolean = If(isComplement, terminalSet.IndexOf(symbol) = -1, terminalSet.IndexOf(symbol) > -1)
                If match Then
                    _Position += 1
                    success = True
                End If
                Return symbol
            End If
            Return ControlChars.NullChar
        End Function

        Private Function MatchTerminalString(terminalString As String, ByRef success As Boolean) As String
            Dim currrent_position As Integer = _Position
            For Each terminal As Char In terminalString
                MatchTerminal(terminal, success)
                If Not success Then
                    _Position = currrent_position
                    Return Nothing
                End If
            Next
            success = True
            Return terminalString
        End Function

        Private Function [Error](message As String) As Integer
            Errors += New LineValue(Of String) With {
                .Line = _Position,
                .value = message
            }
            Return Errors.Count
        End Function

        Private Sub ClearError(count As Integer)
            Errors.RemoveRange(count, Errors.Count - count)
        End Sub

        ''' <summary>
        ''' 获取得到解析的过程之中的错误消息
        ''' </summary>
        ''' <returns></returns>
        Public Function GetEorrorMessages() As String
            Dim text As New StringBuilder()
            For Each msg As LineValue(Of String) In Errors
                text.Append(Input.FormErrorMessage(msg.Line, msg.value))
                text.AppendLine()
            Next
            Return text.ToString()
        End Function
    End Class
End Namespace
