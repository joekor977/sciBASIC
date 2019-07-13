﻿#Region "Microsoft.VisualBasic::52ff4afa1ca6add8a0958b335f203044, Data\BinaryData\DataStorage\HDF5\structure\DataObjects\Headers\ObjectHeader.vb"

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

    '     Class ObjectHeader
    ' 
    '         Properties: headerMessages, objectHeaderSize, objectReferenceCount, totalNumberOfHeaderMessages, version
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: readVersion1
    ' 
    '         Sub: printValues, readVersion2
    ' 
    ' 
    ' /********************************************************************************/

#End Region

'
' * Mostly copied from NETCDF4 source code.
' * refer : http://www.unidata.ucar.edu
' * 
' * Modified by iychoi@email.arizona.edu
' 


Imports System.IO
Imports Microsoft.VisualBasic.Data.IO.HDF5.struct.messages
Imports BinaryReader = Microsoft.VisualBasic.Data.IO.HDF5.device.BinaryReader

Namespace HDF5.struct

    Public Class ObjectHeader : Inherits HDF5Ptr

        Public ReadOnly Property version As Integer
        Public ReadOnly Property totalNumberOfHeaderMessages As Integer
        Public ReadOnly Property objectReferenceCount As Integer
        Public ReadOnly Property objectHeaderSize As Integer
        Public ReadOnly Property headerMessages As New List(Of ObjectHeaderMessage)

        Public Sub New(sb As Superblock, address As Long)
            Call MyBase.New(address)

            Dim [in] As BinaryReader = sb.FileReader(address)

            Me.version = [in].readByte()

            If Me.version = 1 Then

                [in].skipBytes(1)

                Me.totalNumberOfHeaderMessages = [in].readShort()
                Me.objectReferenceCount = [in].readInt()
                Me.objectHeaderSize = [in].readInt()

                [in].skipBytes(4)

                readVersion1([in], sb, [in].offset, Me.totalNumberOfHeaderMessages, Long.MaxValue)
            Else
                readVersion2([in], sb, [in].offset)
            End If
        End Sub

        Private Function readVersion1([in] As BinaryReader, sb As Superblock, address As Long, readMessages As Integer, maxBytes As Long) As Integer
            Dim count As Integer = 0
            Dim byteRead As Integer = 0
            ' read messages
            Dim messageOffset As Long = address

            [in].offset = address

            While count < readMessages AndAlso byteRead < maxBytes
                Dim msg As New ObjectHeaderMessage([in], sb, messageOffset)

                messageOffset += msg.headerLength + msg.sizeOfHeaderMessageData
                byteRead += msg.headerLength + msg.sizeOfHeaderMessageData

                count += 1

                If msg.headerMessageType Is ObjectHeaderMessageType.ObjectHeaderContinuation Then
                    ' CONTINUE
                    Dim cmsg As ContinueMessage = msg.continueMessage
                    Dim continuationBlockFilePos As Long = cmsg.offset

                    count += readVersion1([in], sb, continuationBlockFilePos, readMessages - count, cmsg.length)
                ElseIf msg.headerMessageType IsNot ObjectHeaderMessageType.NIL Then
                    ' NOT NIL
                    Me.headerMessages.Add(msg)
                End If
            End While
            Return count
        End Function

        Private Sub readVersion2([in] As BinaryReader, sb As Superblock, address As Long)
            Throw New IOException("version not implented")
        End Sub

        Protected Friend Overrides Sub printValues(console As TextWriter)
            console.WriteLine("ObjectHeader >>>")
            console.WriteLine("address : " & Me.m_address)
            console.WriteLine("version : " & Me.version)
            console.WriteLine("number of messages : " & Me.totalNumberOfHeaderMessages)
            console.WriteLine("object reference count : " & Me.objectReferenceCount)
            console.WriteLine("object header size : " & Me.objectHeaderSize)

            For i As Integer = 0 To Me.headerMessages.Count - 1
                Me.headerMessages(i).printValues(console)
            Next

            console.WriteLine("ObjectHeader <<<")
        End Sub
    End Class

End Namespace
