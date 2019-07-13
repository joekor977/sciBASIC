﻿#Region "Microsoft.VisualBasic::b1b484722aa851bd456e5249ecc18985, www\Microsoft.VisualBasic.NETProtocol\NETProtocol\Protocols\UserAPI.vb"

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

    '     Module UserAPI
    ' 
    ' 
    '         Enum Protocols
    ' 
    '             GetData, InitUser
    ' 
    ' 
    ' 
    '  
    ' 
    '     Properties: ProtocolEntry
    ' 
    '     Function: InitUser, Uid
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Net.Protocols
Imports Microsoft.VisualBasic.Net.Protocols.Reflection
Imports Microsoft.VisualBasic.Net.Tcp
Imports Microsoft.VisualBasic.Serialization

Namespace NETProtocol.Protocols

    ''' <summary>
    ''' 用户客户端所调用的协议
    ''' </summary>
    Module UserAPI

        Public Enum Protocols
            InitUser
            ''' <summary>
            ''' 获取得到推送的消息
            ''' </summary>
            GetData
        End Enum

        Public ReadOnly Property ProtocolEntry As Long = New Protocol(GetType(Protocols)).EntryPoint

        ''' <summary>
        ''' 在服务器端调用得到用户的唯一标识符
        ''' </summary>
        ''' <param name="sId"></param>
        ''' <returns></returns>
        Public Function Uid(sId As String) As Long
            sId &= Now.ToBinary.ToString
            sId = SecurityString.GetMd5Hash(sId)
            Return SecurityString.ToLong(sId)
        End Function

        Public Function InitUser(remote As IPEndPoint, uid As String) As InitPOSTBack
            Dim req = RequestStream.CreateProtocol(ProtocolEntry, Protocols.InitUser, uid)
            Dim rep = New TcpRequest(remote).SendMessage(req)
            Dim args = rep.LoadObject(AddressOf JSON.LoadJSON(Of InitPOSTBack))
            args.Portal.IPAddress = remote.IPAddress ' 服务器端偷懒了
            Return args
        End Function
    End Module
End Namespace
