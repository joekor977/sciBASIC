﻿#Region "Microsoft.VisualBasic::c01dc7ecd95eb21a49072f0f4bce8be0, Data\BinaryData\DataStorage\HDF5\structure\DataObjects\Headers\Message.vb"

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

    '     Class Message
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace HDF5.struct

    ''' <summary>
    ''' Data object header messages are small pieces of metadata that are stored in the 
    ''' data object header for each object in an HDF5 file. Data object header messages 
    ''' provide the metadata required to describe an object and its contents, as well as 
    ''' optional pieces of metadata that annotate the meaning or purpose of the object.
    '''
    ''' Data object header messages are either stored directly in the data object header 
    ''' for the object Or are shared between multiple objects in the file. When a message 
    ''' Is shared, a flag in the Message Flags indicates that the actual Message Data 
    ''' portion of that message Is stored in another location (such as another data object 
    ''' header, Or a heap in the file) And the Message Data field contains the information 
    ''' needed to locate the actual information for the message.
    ''' </summary>
    Public MustInherit Class Message : Inherits HDF5Ptr

        Public Sub New(address As Long)
            MyBase.New(address)
        End Sub
    End Class
End Namespace
