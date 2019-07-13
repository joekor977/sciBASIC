﻿#Region "Microsoft.VisualBasic::0ad9c27db04a246cde7cf84c89873018, www\githubAPI\Class\SearchUser.vb"

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

    '     Class SearchResult
    ' 
    '         Properties: incomplete_results, items, total_count
    ' 
    '     Class User
    ' 
    '         Properties: avatar_url, bio, blog, company, created_at
    '                     email, events_url, followers, followers_url, following
    '                     following_url, gists_url, gravatar_id, hireable, html_url
    '                     id, location, login, name, organizations_url
    '                     public_gists, public_repos, received_events_url, repos_url, score
    '                     site_admin, starred_url, subscriptions_url, type, updated_at
    '                     url
    ' 
    '         Function: ToString
    ' 
    '     Structure UserModel
    ' 
    '         Properties: Followers, Followings, Repositories, Stars, User
    ' 
    '         Function: ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace [Class]

    Public Class SearchResult(Of T As Class) : Inherits BaseClass
        Public Property total_count As Integer
        Public Property incomplete_results As Boolean
        Public Property items As T()
    End Class

    ''' <summary>
    ''' <see cref="login"/>是主键<see cref="INamedValue.Key"/>
    ''' </summary>
    Public Class User : Inherits BaseClass
        Implements INamedValue

        Public Property score As Double
        Public Property login As String Implements INamedValue.Key
        Public Property id As String
        Public Property avatar_url As String
        Public Property gravatar_id As String
        Public Property url As String
        Public Property html_url As String
        Public Property followers_url As String
        Public Property following_url As String
        Public Property gists_url As String
        Public Property starred_url As String
        Public Property subscriptions_url As String
        Public Property organizations_url As String
        Public Property repos_url As String
        Public Property events_url As String
        Public Property received_events_url As String
        Public Property type As String
        Public Property site_admin As String
        Public Property name As String
        Public Property company As String
        Public Property blog As String
        Public Property location As String
        Public Property email As String
        Public Property hireable As String
        Public Property bio As String
        Public Property public_repos As String
        Public Property public_gists As String
        Public Property followers As String
        Public Property following As String
        Public Property created_at As String
        Public Property updated_at As String

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class

    Public Structure UserModel

        Public Property User As User
        Public Property Followers As String()
        Public Property Followings As String()
        Public Property Repositories As String()
        ''' <summary>
        ''' username/repository
        ''' </summary>
        ''' <returns></returns>
        Public Property Stars As NamedValue(Of String)()

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Structure
End Namespace
