﻿#Region "Microsoft.VisualBasic::7d3db090cb0d8a40b5ad3751dcf30348, ..\sciBASIC#\gr\Datavisualization.Network\Datavisualization.Network\IO\FileStream\Json\json.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xieguigang (xie.guigang@live.com)
    '       xie (genetics@smrucc.org)
    ' 
    ' Copyright (c) 2016 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
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

#End Region

Imports Microsoft.VisualBasic.Serialization.JSON

Namespace FileStream.Json

    Public Class net

        Public Property nodes As node()
        Public Property edges As edges()

        ''' <summary>
        ''' 优先加载的样式名称
        ''' </summary>
        ''' <returns></returns>
        Public Property style As String
        ''' <summary>
        ''' All unique vaue of the property <see cref="node.type"/>
        ''' </summary>
        ''' <returns></returns>
        Public Property types As Dictionary(Of String, String)

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class

    Public Class edges

        Public Property source As Integer
        Public Property target As Integer
        Public Property A As String
        Public Property B As String
        Public Property value As String
        Public Property weight As String
        Public Property id As String
        Public Property Data As Dictionary(Of String, String)

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class

    Public Class node

        Public Property id As Integer
        Public Property name As String
        Public Property degree As Integer
        Public Property type As String
        Public Property Data As Dictionary(Of String, String)

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class
End Namespace
