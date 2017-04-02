Imports System.Collections.Generic

'
'*****************************************************************************
' Copyright 2013 Lars Behnke
' 
' Licensed under the Apache License, Version 2.0 (the "License");
' you may not use this file except in compliance with the License.
' You may obtain a copy of the License at
' 
'   http://www.apache.org/licenses/LICENSE-2.0
' 
' Unless required by applicable law or agreed to in writing, software
' distributed under the License is distributed on an "AS IS" BASIS,
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' See the License for the specific language governing permissions and
' limitations under the License.
' *****************************************************************************
'

Namespace com.apporiented.algorithm.clustering


	Public Class CompleteLinkageStrategy
		Implements LinkageStrategy

        Public Function calculateDistance( distances As ICollection(Of Distance)) As Distance Implements LinkageStrategy.calculateDistance
            Dim max As Double = Double.NaN

            For Each dist As Distance In distances
                If Double.IsNaN(max) OrElse dist.Distance > max Then max = dist.Distance
            Next dist
            Return New Distance(max)
        End Function
    End Class

End Namespace