﻿#Region "Microsoft.VisualBasic::791253ac0ae3855858015a1cc34e76ac, tutorials\core.test\LinuxRunTest\child\Module1.vb"

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

    ' Module Module1
    ' 
    '     Sub: Main
    ' 
    ' /********************************************************************************/

#End Region

#Region "Microsoft.VisualBasic::79e000a08e00363a08f3a622b2d25fa1, ..\..\core.test"

    ' Author:
    ' 
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 


    ' Source file summaries:

    ' Module Module1
    ' 
    '     Sub: Main
    ' 
    ' 

#End Region

#Region "Microsoft.VisualBasic::bfa5e9bdb2a53100de723fd333a6c2c0, ..\..\core.test"

    ' Author:
    ' 
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 


    ' Source file summaries:

    ' Module Module1
    ' 
    '     Sub: Main
    ' 
    ' 
    ' 

#End Region

Imports Microsoft.VisualBasic.Language.UnixBash

Module Module1

    Sub Main()

        Call App.Command.__DEBUG_ECHO

        Dim ps1 As New PS1("[\u@\h \W \A #\#]\$ ")

        For i As Integer = 0 To 100
            Call $"{ps1.ToString}  ---> {i}%".__DEBUG_ECHO
            Call Threading.Thread.Sleep(300)
        Next
    End Sub

End Module
