﻿#Region "Microsoft.VisualBasic::b760b1a419b57420278cc2710f6bb3db, Data\BinaryData\DataStorage\HDF5\dataset\CompactDataset.vb"

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

    '     Class CompactDataset
    ' 
    '         Properties: dimensions, rawData, size
    ' 
    '         Function: getBuffer
    ' 
    ' 
    ' /********************************************************************************/

#End Region

'*****************************************************************************
' This file is part of jHDF. A pure Java library for accessing HDF5 files.
' 
' http://jhdf.io
' 
' Copyright 2019 James Mudd
' 
' MIT License see 'LICENSE' file
' *****************************************************************************

Imports System.IO
Imports Microsoft.VisualBasic.Data.IO.HDF5.struct

Namespace HDF5.dataset

    ''' <summary>
    ''' Compact: The array is stored in one contiguous block as part of this object header message.
    ''' </summary>
    Public Class CompactDataset : Inherits Hdf5Dataset

        ''' <summary>
        ''' (2 bytes) This field contains the size of the raw data for the dataset array, in bytes.
        ''' </summary>
        ''' <returns></returns>
        Public Property size As Integer
        ''' <summary>
        ''' This field contains the raw data for the dataset array.
        ''' </summary>
        ''' <returns></returns>
        Public Property rawData As Byte()

        Public Overrides ReadOnly Property dimensions As Integer()
            Get
                Return dataSpace.dimensionLength
            End Get
        End Property

        Protected Overrides Function getBuffer(sb As Superblock) As MemoryStream
            Return New MemoryStream(rawData)
        End Function
    End Class
End Namespace
