﻿#Region "Microsoft.VisualBasic::75917d1f7b9e3ef81aad687b788cae2e, Microsoft.VisualBasic.Core\test\TypeTest.vb"

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

    ' Module TypeTest
    ' 
    '     Function: CharArray
    ' 
    '     Sub: Main, patternMatch, test
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Serialization.JSON

Module TypeTest

    Sub Main()

        Call patternMatch()


        Call test()

        Dim o As [Variant](Of String, Integer(), Char())

        o = "string"

        Console.WriteLine(CStr(o))
        Console.WriteLine(o Like GetType(String))
        Console.WriteLine(o Like GetType(Integer()))
        Console.WriteLine(o Like GetType(Char()))

        o = {1, 2, 1231, 31, 23, 12}
        Console.WriteLine(CType(o, Integer()).GetJson)
        Console.WriteLine(o Like GetType(String))
        Console.WriteLine(o Like GetType(Integer()))
        Console.WriteLine(o Like GetType(Char()))

        o = {"f"c, "s"c, "d"c, "f"c, "s"c, "d"c, "f"c, "s"c, "d"c, "f"c, "s"c, "d"c, "f"c}
        Console.WriteLine(CType(o, Char()).GetJson)
        Console.WriteLine(o Like GetType(String))
        Console.WriteLine(o Like GetType(Integer()))
        Console.WriteLine(o Like GetType(Char()))

        Pause()
    End Sub

    Sub patternMatch()
        Dim src As [Variant](Of String(), Integer(), Byte()) = {23, 4, 2342, 42}

        Select Case src
            Case src Like GetType(String())
                Console.WriteLine("is string")
            Case src Like GetType(Integer())
                Console.WriteLine("is integer")
            Case src Like GetType(Byte())
                Console.WriteLine("is bytes")
            Case Else
                Throw New Exception
        End Select

        Pause()
    End Sub

    Sub test()

        Dim chars As [Variant](Of Char(), Integer())

        chars = CharArray("Hello world!", False)

        Console.WriteLine(CType(chars, Char()).GetJson)
        Console.WriteLine(chars Like GetType(Integer()))
        Console.WriteLine(chars Like GetType(Char()))

        chars = CharArray("Hello world!", True)

        Console.WriteLine(CType(chars, Integer()).GetJson)
        Console.WriteLine(chars Like GetType(Integer()))
        Console.WriteLine(chars Like GetType(Char()))

        Pause()
    End Sub

    Public Function CharArray(s As String, ascii As Boolean) As [Variant](Of Char(), Integer())
        If ascii Then
            Return s.Select(Function(c) AscW(c)).ToArray
        Else
            Return s.ToArray
        End If
    End Function
End Module
