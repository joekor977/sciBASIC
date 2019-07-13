﻿#Region "Microsoft.VisualBasic::fad8e7d7044400b8c62ba0ae05afbcbf, Data\BinaryData\DataStorage\HDF5\structure\Infrastructure\BTree\BTreeNodeTypes.vb"

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

    '     Enum BTreeNodeTypes
    ' 
    '         group, raw_data
    ' 
    '  
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace HDF5.struct

    ''' <summary>
    ''' Each B-tree points to a particular type of data. This field indicates the type of data 
    ''' as well as implying the maximum degree K of the tree and the size of each Key field.
    ''' </summary>
    Public Enum BTreeNodeTypes

        ''' <summary>
        ''' This tree points to group nodes.
        ''' </summary>
        group
        ''' <summary>
        ''' This tree points to raw data chunk nodes.
        ''' </summary>
        raw_data
    End Enum

End Namespace
