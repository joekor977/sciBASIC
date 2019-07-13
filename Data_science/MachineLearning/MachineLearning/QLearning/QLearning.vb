﻿#Region "Microsoft.VisualBasic::67bb961de269ca975642792bbcf9fa14, Data_science\MachineLearning\MachineLearning\QLearning\QLearning.vb"

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

    '     Class QLearning
    ' 
    '         Properties: GoalPenalty, GoalRewards, Q
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Sub: finishQLearn, RunLearningLoop
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace QLearning

    ''' <summary>
    ''' Q Learning sample class <br/>
    ''' <b>The goal of this code sample is for the character @ to reach the goal area G</b> <br/>
    ''' compile using "javac QLearning.java" <br/>
    ''' test using "java QLearning" <br/>
    ''' 
    ''' @author A.Liapis (Original author), A. Hartzen (2013 modifications) 
    ''' </summary>
    Public MustInherit Class QLearning(Of T As ICloneable)

        Protected ReadOnly _stat As QState(Of T)

        Public ReadOnly Property Q As QTable(Of T)

        Sub New(state As QState(Of T), provider As Func(Of Integer, QTable(Of T)))
            _stat = state
            Q = provider(ActionRange)
        End Sub

        Protected MustOverride Sub __init()

        ''' <summary>
        ''' The size of the <see cref="QTable"/>
        ''' </summary>
        ''' <returns></returns>
        Public MustOverride ReadOnly Property ActionRange As Integer

        Public MustOverride ReadOnly Property GoalReached As Boolean

        ''' <summary>
        ''' Takes a action for the agent.
        ''' </summary>
        ''' <param name="i">Iteration counts.</param>
        Protected MustOverride Sub __run(i As Integer)
        ''' <summary>
        ''' If the <see cref="GoalReached"/> then reset and continute learning.
        ''' </summary>
        ''' <param name="i">机器学习的当前的迭代次数</param>
        Protected MustOverride Sub __reset(i As Integer)

        ''' <summary>
        ''' 目标达成所得到的奖励
        ''' </summary>
        ''' <returns></returns>
        Public Property GoalRewards As Integer = 10
        ''' <summary>
        ''' 目标没有达成的罚分
        ''' </summary>
        ''' <returns></returns>
        Public Property GoalPenalty As Integer = -100

        Public Sub RunLearningLoop(n As Integer)
            For count As Integer = 0 To n
                Call __reset(count)

                ' CHECK IF WON, THEN RESET
                Do While Not GoalReached
                    Call __run(count)

                    If Not GoalReached Then
                        ' 目标还没有达成，则罚分
                        Call Q.UpdateQvalue(GoalPenalty, _stat.Current)
                    End If
                Loop

                ' REWARDS AND ADJUSTMENT OF WEIGHTS SHOULD TAKE PLACE HERE
                Call Q.UpdateQvalue(GoalRewards, _stat.Current)
            Next

            Call finishQLearn()
        End Sub

        ''' <summary>
        ''' You can save you Q table by overrides at here.
        ''' </summary>
        Protected Overridable Sub finishQLearn()

        End Sub
    End Class
End Namespace
