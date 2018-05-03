﻿Imports System
Imports System.Dynamic
Imports System.Linq.Expressions

Namespace Microsoft.VisualBasic.CompilerServices
    Friend Class VBInvokeDefaultBinder
        Inherits InvokeBinder
        ' Methods
        Public Sub New(CallInfo As CallInfo, ReportErrors As Boolean)
            MyBase.New(CallInfo)
            Me._reportErrors = ReportErrors
        End Sub

        Public Overrides Function Equals(_other As Object) As Boolean
            Dim binder As VBInvokeDefaultBinder = TryCast(_other, VBInvokeDefaultBinder)
            Return (((Not binder Is Nothing) AndAlso MyBase.CallInfo.Equals(binder.CallInfo)) AndAlso (Me._reportErrors = binder._reportErrors))
        End Function

        Public Overrides Function FallbackInvoke(target As DynamicMetaObject, packedArgs As DynamicMetaObject(), errorSuggestion As DynamicMetaObject) As DynamicMetaObject
            If IDOUtils.NeedsDeferral(target, packedArgs, Nothing) Then
                Return MyBase.Defer(target, packedArgs)
            End If
            Dim args As Expression() = Nothing
            Dim argNames As String() = Nothing
            Dim argValues As Object() = Nothing
            IDOUtils.UnpackArguments(packedArgs, MyBase.CallInfo, args, argNames, argValues)
            If ((Not errorSuggestion Is Nothing) AndAlso Not NewLateBinding.CanBindInvokeDefault(target.Value, argValues, argNames, Me._reportErrors)) Then
                Return errorSuggestion
            End If
            Dim left As ParameterExpression = Expression.Variable(GetType(Object), "result")
            Dim expression2 As ParameterExpression = Expression.Variable(GetType(Object()), "array")
            Dim right As Expression = Expression.Call(GetType(NewLateBinding).GetMethod("FallbackInvokeDefault1"), target.Expression, Expression.Assign(expression2, Expression.NewArrayInit(GetType(Object), args)), Expression.Constant(argNames, GetType(String())), Expression.Constant(Me._reportErrors))
            Dim variables As ParameterExpression() = New ParameterExpression() {left, expression2}
            Dim expressions As Expression() = New Expression() {Expression.Assign(left, right), IDOUtils.GetWriteBack(args, expression2), left}
            Return New DynamicMetaObject(Expression.Block(variables, expressions), IDOUtils.CreateRestrictions(target, packedArgs, Nothing))
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return ((VBInvokeDefaultBinder._hash Xor MyBase.CallInfo.GetHashCode) Xor Me._reportErrors.GetHashCode)
        End Function


        ' Fields
        Private Shared ReadOnly _hash As Integer = GetType(VBInvokeDefaultBinder).GetHashCode
        Private ReadOnly _reportErrors As Boolean
    End Class
End Namespace
