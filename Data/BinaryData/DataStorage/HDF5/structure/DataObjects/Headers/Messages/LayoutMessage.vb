﻿#Region "Microsoft.VisualBasic::12e0065cdaf2f46083a25dd94425d2f1, Data\BinaryData\DataStorage\HDF5\structure\DataObjects\Headers\Messages\LayoutMessage.vb"

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

    '     Class LayoutMessage
    ' 
    '         Properties: chunkSize, continuousSize, dataAddress, dataSize, dimensionality
    '                     type, version
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

Namespace HDF5.[Structure]

    ''' <summary>
    ''' The Data Layout message describes how the elements of a multi-dimensional array 
    ''' are stored in the HDF5 file.
    ''' </summary>
    Public Class LayoutMessage : Inherits Message

        ''' <summary>
        ''' The version number information is used for changes in the format of the 
        ''' data layout message and is described here:
        ''' 
        ''' + 0: Never used.
        ''' + 1: Used by version 1.4 And before of the library to encode layout information. 
        '''      Data space Is always allocated when the data set Is created.
        ''' + 2: Used by version 1.6.x of the library to encode layout information. Data 
        '''      space Is allocated only when it Is necessary.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property version As Integer
        ''' <summary>
        ''' Layout Class
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property type As LayoutClass
        ''' <summary>
        ''' number of Dimensions
        ''' 
        ''' An array has a fixed dimensionality. This field specifies the number of dimension 
        ''' size fields later in the message. The value stored for chunked storage is 1 greater 
        ''' than the number of dimensions in the dataset’s dataspace. For example, 2 is stored 
        ''' for a 1 dimensional dataset.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property dimensionality As Integer
        ''' <summary>
        ''' For contiguous storage, this is the address of the raw data in the file. For chunked 
        ''' storage this is the address of the v1 B-tree that is used to look up the addresses 
        ''' of the chunks. This field is not present for compact storage. If the version for 
        ''' this message is greater than 1, the address may have the “undefined address” value, 
        ''' to indicate that storage has not yet been allocated for this array.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property dataAddress As Long
        Public ReadOnly Property continuousSize As Long
        Public ReadOnly Property chunkSize As Integer()
        Public ReadOnly Property dataSize As Integer

        Public Sub New([in] As BinaryReader, sb As Superblock, address As Long)
            Call MyBase.New(address)

            [in].offset = address

            Me.version = [in].readByte()

            If Me.version < 3 Then
                Me.dimensionality = [in].readByte()
                Me.type = [in].readByte()

                ' Reserved (zero) 1 + 4 = 5 bytes
                [in].skipBytes(5)

                Dim isCompact As Boolean = (Me.type = LayoutClass.CompactStorage)

                If Not isCompact Then
                    ' Data AddressO (optional)
                    Me.dataAddress = ReadHelper.readO([in], sb)
                End If

                Me.chunkSize = New Integer(Me.dimensionality - 1) {}

                For i As Integer = 0 To Me.dimensionality - 1
                    ' Dimension n Size
                    Me.chunkSize(i) = [in].readInt()
                Next

                If isCompact Then
                    ' Dataset Element Size (optional)
                    Me.dataSize = [in].readInt()
                    Me.dataAddress = [in].offset
                End If
            Else
                Me.type = CType(CInt([in].readByte), LayoutClass)

                If Me.type = LayoutClass.CompactStorage Then
                    Me.dataSize = [in].readShort()
                    Me.dataAddress = [in].offset
                ElseIf Me.type = LayoutClass.ContiguousStorage Then
                    Me.dataAddress = ReadHelper.readO([in], sb)
                    Me.continuousSize = ReadHelper.readL([in], sb)
                ElseIf Me.type = LayoutClass.ChunkedStorage Then
                    Me.dimensionality = [in].readByte()
                    Me.dataAddress = ReadHelper.readO([in], sb)
                    Me.chunkSize = New Integer(Me.dimensionality - 1) {}

                    For i As Integer = 0 To Me.dimensionality - 1
                        Me.chunkSize(i) = [in].readInt()
                    Next
                End If
            End If
        End Sub

        Protected Friend Overrides Sub printValues(console As TextWriter)
            console.WriteLine("LayoutMessage >>>")

            console.WriteLine("address : " & Me.m_address)
            console.WriteLine("version : " & Me.version)
            console.WriteLine("number of dimensions : " & Me.dimensionality)
            console.WriteLine("type : " & Me.type)
            console.WriteLine("data address : " & Me.dataAddress)
            console.WriteLine("continuous size : " & Me.continuousSize)
            console.WriteLine("data size : " & Me.dataSize)

            For i As Integer = 0 To Me.chunkSize.Length - 1
                console.WriteLine("chunk size [" & i & "] : " & Me.chunkSize(i))
            Next

            console.WriteLine("LayoutMessage <<<")
        End Sub
    End Class

End Namespace