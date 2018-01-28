﻿#Region "Microsoft.VisualBasic::3989fb837d9326b2e7d743c74b0605d8, ..\sciBASIC#\Microsoft.VisualBasic.Core\ComponentModel\DataStructures\Tree\BinaryTree\Abstract.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xieguigang (xie.guigang@live.com)
    '       xie (genetics@smrucc.org)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
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

Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.Repository
Imports Microsoft.VisualBasic.Language

Namespace ComponentModel.DataStructures.BinaryTree

    ''' <summary>
    ''' The generic object comparer model:
    ''' 
    ''' + ``> 0`` means ``a > b``
    ''' + ``= 0`` means ``a = 0``
    ''' + ``&lt;0`` means ``a &lt; b``
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="a"></param>
    ''' <param name="b"></param>
    ''' <returns></returns>
    Public Delegate Function CompareOf(Of T)(a As T, b As T) As Integer

    Public MustInherit Class TreeMap(Of K, V)
        Implements IKeyedEntity(Of K)
        Implements Value(Of V).IValueOf
        Implements IComparable(Of K)

        Public Property Key As K Implements IKeyedEntity(Of K).Key
        Public Property value As V Implements Value(Of V).IValueOf.Value

        Public MustOverride Function CompareTo(other As K) As Integer Implements IComparable(Of K).CompareTo

    End Class
End Namespace
