﻿Imports System
Imports System.ComponentModel

Namespace Microsoft.VisualBasic.CompilerServices
    <AttributeUsage(AttributeTargets.Class, Inherited:=False, AllowMultiple:=False), EditorBrowsable(EditorBrowsableState.Never), DynamicallyInvokableAttribute> _
    Public NotInheritable Class StandardModuleAttribute
        Inherits Attribute
    End Class
End Namespace
