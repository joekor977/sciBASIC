﻿#Region "Microsoft.VisualBasic::4b567f876a7b51a239bd4ef6e47c0fc9, Microsoft.VisualBasic.Core\CommandLine\Interpreters\InteractiveConsole.vb"

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

    '     Class InteractiveConsole
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: RunApp
    ' 
    '         Sub: RunAppInternal
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports Microsoft.VisualBasic.ApplicationServices.Terminal
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Language.UnixBash
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.Expressions

Namespace CommandLine

    ''' <summary>
    ''' interactive console in ``/i`` mode, example:
    ''' 
    ''' ```bash
    ''' # This command will makes your CLI program enter 
    ''' # the interactive console mode.
    ''' # 
    ''' # type quit() for exit.
    ''' #
    ''' App /i
    ''' ```
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class InteractiveConsole : Inherits Interpreter

        Sub New(App As Type)
            Call MyBase.New(App)
        End Sub

        Public Function RunApp() As Integer
            Dim input As Value(Of String) = ""
            Dim ps1 As PS1 = PS1.Fedora12

            Call Console.Write(ps1.ToString)
            Call Console.Write(" ")

            Do While Not (input = Console.ReadLine).TextEquals("quit()")
                With input.Value
                    If Not .StringEmpty Then
                        Call RunAppInternal(CLITools.TryParse(.ByRef))
                    End If
                End With

                Call Console.Write(ps1.ToString)
                Call Console.Write(" ")
            Loop

            Return 0
        End Function

        ''' <summary>
        ''' Contains sevral build in command about file system operation and the program CLI interpreter commands
        ''' </summary>
        ''' <param name="cmd"></param>
        Private Sub RunAppInternal(cmd As CommandLine)
            Select Case cmd.Name.ToLower

                Case "ls"   ' list directory

                    Dim directory$ = If(cmd.Tokens.ElementAtOrDefault(1), App.CurrentDirectory)
                    Dim directories = FileIO.FileSystem.GetDirectories(directory)
                    Dim files = FileIO.FileSystem.GetFiles(directory)

                    For Each dir As String In directories
                        Call Console.WriteLine("<directory> " & dir.Replace("\"c, "/"c).Split("/"c).Last)
                    Next

                    Dim table = files _
                        .Select(Function(path)
                                    Return {
                                        path.Replace("\"c, "/"c).Split("/"c).Last,
                                        New FileInfo(path).Length & " Bytes"
                                    }
                                End Function) _
                        .ToArray

                    Call PrintAsTable.Print(table, New StreamWriter(Console.OpenStandardOutput))

                Case "cd"   ' change directory

                    App.CurrentDirectory = cmd.Tokens.ElementAtOrDefault(1)

                Case "cat"  ' display text file content

                    Call Console.WriteLine(cmd.Tokens.ElementAtOrDefault(1).ReadAllText)

                Case "help" ' view commandline help 

                    Call MyBase.Execute(args:=New CommandLine With {.Name = "?"})

                Case "/@set"

                    ' /@set var value
                    Dim var$ = cmd.Tokens.ElementAtOrDefault(1)
                    Dim value$ = cmd _
                        .Tokens _
                        .ElementAtOrDefault(2) _
                        .Interpolate(AddressOf App.GetVariable, escape:=False)

                    Call App.JoinVariable(var, value)

                Case "/@get"

                    ' /@get var
                    Call Console.WriteLine(App.GetVariable(cmd.Tokens.ElementAtOrDefault(1)))

                Case Else

                    Call MyBase.Execute(args:=cmd)

            End Select
        End Sub
    End Class
End Namespace