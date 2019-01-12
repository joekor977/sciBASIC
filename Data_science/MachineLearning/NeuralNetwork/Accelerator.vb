﻿Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.GAF
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.GAF.Helper
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.Models
Imports Microsoft.VisualBasic.MachineLearning.NeuralNetwork.StoreProcedure

Namespace NeuralNetwork.Accelerator

    Public Module GAExtensions

        <Extension>
        Public Sub RunGAAccelerator(network As Network, trainingSet As Sample(), Optional populationSize% = 1000)
            Dim synapses = network.PopulateAllSynapses _
                .GroupBy(Function(s) s.ToString) _
                .Select(Function(sg)
                            Return New NamedCollection(Of Synapse)(sg.Key, sg.ToArray)
                        End Function) _
                .ToArray
            Dim population As Population(Of WeightVector) = New WeightVector(synapses).InitialPopulation(populationSize)
            Dim fitness As Fitness(Of WeightVector) = New Fitness(network, synapses, trainingSet)
            Dim ga As New GeneticAlgorithm(Of WeightVector)(population, fitness)
            Dim engine As New EnvironmentDriver(Of WeightVector)(ga) With {
                .Iterations = 10000,
                .Threshold = 0.005
            }

            Call engine.AttachReporter(Sub(i, e, g) EnvironmentDriver(Of WeightVector).CreateReport(i, e, g).ToString.__DEBUG_ECHO)
            Call engine.Train()
        End Sub
    End Module

    Public Class WeightVector : Implements Chromosome(Of WeightVector), ICloneable

        Friend weights#()

        Shared ReadOnly random As New Random

        Sub New(Optional synapses As NamedCollection(Of Synapse)() = Nothing)
            If Not synapses Is Nothing Then
                Me.weights = New Double(synapses.Length - 1) {}

                For i As Integer = 0 To weights.Length - 1
                    weights(i) = synapses(i).First.Weight
                Next
            End If
        End Sub

        Public Overrides Function ToString() As String
            Return ""
        End Function

        Public Function Crossover(another As WeightVector) As IEnumerable(Of WeightVector) Implements Chromosome(Of WeightVector).Crossover
            Dim thisClone As WeightVector = Me.Clone()
            Dim otherClone As WeightVector = another.Clone()
            Call random.Crossover(thisClone.weights, another.weights)
            Return {thisClone, otherClone}
        End Function

        Public Function Mutate() As WeightVector Implements Chromosome(Of WeightVector).Mutate
            Dim result As WeightVector = Me.Clone()
            Call result.weights.Mutate(random)
            Return result
        End Function

        Public Function Clone() As Object Implements ICloneable.Clone
            Dim weights#() = New Double(Me.weights.Length - 1) {}
            Call Array.Copy(Me.weights, Scan0, weights, Scan0, weights.Length)
            Return New WeightVector() With {
                .weights = weights
            }
        End Function
    End Class

    Public Class Fitness : Implements Fitness(Of WeightVector)

        Dim network As Network
        Dim synapses As NamedCollection(Of Synapse)()
        Dim dataSets As Sample()

        Sub New(network As Network, synapses As NamedCollection(Of Synapse)(), trainingSet As Sample())
            Me.dataSets = trainingSet
            Me.network = network
            Me.synapses = synapses
        End Sub

        Public Function Calculate(chromosome As WeightVector) As Double Implements Fitness(Of WeightVector).Calculate
            For i As Integer = 0 To chromosome.weights.Length - 1
                For Each s In synapses(i)
                    s.Weight = chromosome.weights(i)
                Next
            Next

            Dim errors As New List(Of Double)

            For Each dataSet As Sample In dataSets
                Call network.ForwardPropagate(dataSet.status, False)
                Call network.BackPropagate(dataSet.target, False)
                Call errors.Add(TrainingUtils.CalculateError(network, dataSet.target))
            Next

            Return errors.Average
        End Function
    End Class
End Namespace