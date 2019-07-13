﻿#Region "Microsoft.VisualBasic::aa742310460cc18ff1701b360d567bc7, Microsoft.VisualBasic.Core\ApplicationServices\VBDev\XmlDoc\ProjectNamespace.vb"

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

    '     Class ProjectNamespace
    ' 
    '         Properties: Path, Types
    ' 
    '         Constructor: (+3 Overloads) Sub New
    '         Function: [GetType], EnsureType, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

' Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
'    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. 

Imports System.Runtime.CompilerServices

Namespace ApplicationServices.Development.XmlDoc.Assembly

    ''' <summary>
    ''' A namespace within a project -- typically a collection of related types.  Equates to a .net Namespace.
    ''' </summary>
    Public Class ProjectNamespace

        Protected project As Project
        Protected _types As Dictionary(Of String, ProjectType)

        Public Property Path() As String

        Public ReadOnly Property Types() As IEnumerable(Of ProjectType)
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Me._types.Values
            End Get
        End Property

        Public Sub New(project As Project)
            Me.project = project
            Me._types = New Dictionary(Of String, ProjectType)()
        End Sub

        Public Sub New(proj As Project, types As Dictionary(Of String, ProjectType))
            Me.project = proj
            Me._types = types
        End Sub

        Protected Sub New(ns As ProjectNamespace)
            Call Me.New(ns.project, ns._types)
        End Sub

        Public Overrides Function ToString() As String
            Return Path
        End Function

        Public Overloads Function [GetType](typeName As String) As ProjectType
            If Me._types.ContainsKey(typeName.ToLower()) Then
                Return Me._types(typeName.ToLower())
            End If

            Return Nothing
        End Function

        Public Function EnsureType(typeName As String) As ProjectType
            Dim pt As ProjectType = Me.[GetType](typeName)

            If pt Is Nothing Then
                pt = New ProjectType(Me) With {
                    .Name = typeName
                }

                Me._types.Add(typeName.ToLower(), pt)
            End If

            Return pt
        End Function
    End Class
End Namespace
