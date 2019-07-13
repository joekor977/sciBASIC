﻿#Region "Microsoft.VisualBasic::e23833117986edfc9fb6234ef77f8780, Data\BinaryData\DataStorage\HDF5\structure\SymbolTableEntry.vb"

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

    '     Class SymbolTableEntry
    ' 
    '         Properties: cacheType, linkNameOffset, objectHeaderAddress, objectHeaderScratchpadFormat, scratchpadSpace
    '                     size, symbolicLinkScratchpadFormat, totalSymbolTableEntrySize
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Sub: printValues
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
Imports Microsoft.VisualBasic.Data.IO.HDF5.device
Imports BinaryReader = Microsoft.VisualBasic.Data.IO.HDF5.device.BinaryReader

Namespace HDF5.struct


    Public Class SymbolTableEntry : Inherits HDF5Ptr

        Public ReadOnly Property linkNameOffset() As Long
        Public ReadOnly Property objectHeaderAddress() As Long
        Public ReadOnly Property cacheType() As Integer
        Public ReadOnly Property scratchpadSpace() As Byte()

        ''' <summary>
        ''' only work for cache type = 1
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property objectHeaderScratchpadFormat() As ObjectHeaderScratchpadFormat

        ''' <summary>
        ''' only work for cache type = 2
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property symbolicLinkScratchpadFormat() As SymbolicLinkScratchpadFormat

        Public ReadOnly Property totalSymbolTableEntrySize() As Integer
        Public ReadOnly Property size() As Long

        Dim reserved As Integer

        Public Sub New(sb As Superblock, address As Long)
            Call MyBase.New(address)

            Dim [in] As BinaryReader = sb.FileReader(address)

            Me.linkNameOffset = ReadHelper.readO([in], sb)
            Me.objectHeaderAddress = ReadHelper.readO([in], sb)
            Me.totalSymbolTableEntrySize = sb.sizeOfOffsets * 2
            Me.cacheType = [in].readInt()
            Me.reserved = [in].readInt()
            Me.totalSymbolTableEntrySize += 8

            If Me.cacheType = 0 Then
                Me.scratchpadSpace = [in].readBytes(16)
            ElseIf Me.cacheType = 1 Then
                Me.objectHeaderScratchpadFormat = New ObjectHeaderScratchpadFormat(sb, [in].offset)

                ' skip 
                Dim size As Integer = Me.objectHeaderScratchpadFormat.totalObjectHeaderScratchpadFormatSize
                Dim remained As Integer = 16 - size

                Call [in].skipBytes(remained)
            ElseIf Me.cacheType = 2 Then
                Me.symbolicLinkScratchpadFormat = New SymbolicLinkScratchpadFormat(sb, [in].offset)

                ' skip
                Dim size As Integer = Me.symbolicLinkScratchpadFormat.totalSymbolicLinkScratchpadFormatSize
                Dim remained As Integer = 16 - size

                Call [in].skipBytes(remained)
            End If

            Me.totalSymbolTableEntrySize += 16

            If sb.sizeOfOffsets = 8 Then
                Me.size = 40
            Else
                Me.size = 32
            End If
        End Sub

        Protected Friend Overrides Sub printValues(console As TextWriter)
            console.WriteLine("SymbolTableEntry >>>")
            console.WriteLine("address : " & Me.m_address)
            console.WriteLine("link name offset : " & Me.linkNameOffset)
            console.WriteLine("object header address : " & Me.objectHeaderAddress)
            console.WriteLine("cache type : " & Me.cacheType)
            console.WriteLine("reserved : " & Me.reserved)

            If Me.cacheType = 0 Then
                console.WriteLine("scratchpad space : " &
                                  (Me.scratchpadSpace(0) And &HFF).ToString("x") &
                                  (Me.scratchpadSpace(1) And &HFF).ToString("x") &
                                  (Me.scratchpadSpace(2) And &HFF).ToString("x") &
                                  (Me.scratchpadSpace(3) And &HFF).ToString("x") &
                                  (Me.scratchpadSpace(4) And &HFF).ToString("x") &
                                  (Me.scratchpadSpace(5) And &HFF).ToString("x") &
                                  (Me.scratchpadSpace(6) And &HFF).ToString("x") &
                                  (Me.scratchpadSpace(7) And &HFF).ToString("x") &
                                  (Me.scratchpadSpace(8) And &HFF).ToString("x") &
                                  (Me.scratchpadSpace(9) And &HFF).ToString("x") &
                                  (Me.scratchpadSpace(10) And &HFF).ToString("x") &
                                  (Me.scratchpadSpace(11) And &HFF).ToString("x") &
                                  (Me.scratchpadSpace(12) And &HFF).ToString("x") &
                                  (Me.scratchpadSpace(13) And &HFF).ToString("x") &
                                  (Me.scratchpadSpace(14) And &HFF).ToString("x") &
                                  (Me.scratchpadSpace(15) And &HFF).ToString("x")
                                 )

            ElseIf Me.cacheType = 1 Then
                Me.objectHeaderScratchpadFormat.printValues(console)
            ElseIf Me.cacheType = 2 Then
                Me.symbolicLinkScratchpadFormat.printValues(console)
            End If

            console.WriteLine("total symbol table entry size : " & Me.totalSymbolTableEntrySize)
            console.WriteLine("SymbolTableEntry <<<")
        End Sub
    End Class

End Namespace
