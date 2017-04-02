Imports Microsoft.VisualBasic.Imaging

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

Namespace com.apporiented.algorithm.clustering.visualization


    ''' <summary>
    ''' Implemented by visual components of the dendrogram.
    ''' @author lars
    ''' 
    ''' </summary>
    Public Interface Paintable

        Sub paint(g As Graphics2D, xDisplayOffset As Integer, yDisplayOffset As Integer, xDisplayFactor As Double, yDisplayFactor As Double, decorated As Boolean)

    End Interface

End Namespace