﻿Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports GDC.PH.AIDE.BusinessLayer

Public Class TasksSet
    Implements ITasks, INotifyPropertyChanged

    Private cTasks As clsTasks
    Private cTasksFactory As clsTasksFactory

    Private cTasks_sp As clsTasks_sp

    Public Sub New()
        cTasks = New clsTasks()
        cTasksFactory = New clsTasksFactory
    End Sub

    Public Sub New(ByVal objTask As clsTasks)
        cTasks = objTask
        cTasksFactory = New clsTasksFactory
    End Sub

    'Public Sub New(myRecord As IDataRecord)
    '    MyBase.New(myRecord)
    'End Sub

#Region "Database Field"

    Public Property TASK_ID As Integer Implements ITasks.TASK_ID
        Get
            Return cTasks.TASK_ID
        End Get
        Set(ByVal value As Integer)
            cTasks.TASK_ID = value
        End Set
    End Property

    Public Property EMP_ID As Integer Implements ITasks.EMP_ID
        Get
            Return cTasks.EMP_ID
        End Get
        Set(ByVal value As Integer)
            cTasks.EMP_ID = value
        End Set
    End Property

    Public Property INC_ID As String Implements ITasks.INC_ID
        Get
            Return cTasks.INC_ID
        End Get
        Set(ByVal value As String)
            cTasks.INC_ID = value
        End Set
    End Property

    Public Property TASK_TYPE As Integer Implements ITasks.TASK_TYPE
        Get
            Return cTasks.TASK_TYPE
        End Get
        Set(ByVal value As Integer)
            cTasks.TASK_TYPE = value
        End Set
    End Property

    Public Property PROJ_ID As Integer Implements ITasks.PROJ_ID
        Get
            Return cTasks.PROJ_ID
        End Get
        Set(ByVal value As Integer)
            cTasks.PROJ_ID = value
        End Set
    End Property

    Public Property INC_DESCR() As String Implements ITasks.INC_DESCR
        Get
            Return cTasks.INC_DESCR
        End Get
        Set(ByVal value As String)
            cTasks.INC_DESCR = value
        End Set
    End Property

    Public Property TASK_DESCR() As String Implements ITasks.TASK_DESCR
        Get
            Return cTasks.TASK_DESCR
        End Get
        Set(ByVal value As String)
            cTasks.TASK_DESCR = value
        End Set
    End Property

    Public Property DATE_STARTED() As DateTime Implements ITasks.DATE_STARTED
        Get
            Return cTasks.DATE_STARTED
        End Get
        Set(ByVal value As DateTime)
            cTasks.DATE_STARTED = value
        End Set
    End Property

    Public Property TARGET_DATE() As DateTime Implements ITasks.TARGET_DATE
        Get
            Return cTasks.TARGET_DATE
        End Get
        Set(ByVal value As DateTime)
            cTasks.TARGET_DATE = value
        End Set
    End Property

    Public Property COMPLTD_DATE() As DateTime Implements ITasks.COMPLTD_DATE
        Get
            Return cTasks.COMPLTD_DATE
        End Get
        Set(ByVal value As DateTime)
            cTasks.COMPLTD_DATE = value
        End Set
    End Property

    Public Property DATE_CREATED() As DateTime Implements ITasks.DATE_CREATED
        Get
            Return cTasks.DATE_CREATED
        End Get
        Set(ByVal value As DateTime)
            cTasks.DATE_CREATED = value
        End Set
    End Property

    Public Property HOURSWORKED_DATE As DateTime Implements ITasks.HOURSWORKED_DATE
        Get
            Return cTasks.DATE_CREATED
        End Get
        Set(ByVal value As DateTime)
            cTasks.DATE_CREATED = value
        End Set
    End Property

    Public Property STATUS() As Integer Implements ITasks.STATUS
        Get
            Return cTasks.STATUS
        End Get
        Set(ByVal value As Integer)
            cTasks.STATUS = value
        End Set
    End Property

    Public Property REMARKS() As String Implements ITasks.REMARKS
        Get
            Return cTasks.REMARKS
        End Get
        Set(ByVal value As String)
            cTasks.REMARKS = value
        End Set
    End Property

    Public Property EFFORT_EST() As Double Implements ITasks.EFFORT_EST
        Get
            Return cTasks.EFFORT_EST
        End Get
        Set(ByVal value As Double)
            cTasks.EFFORT_EST = value
        End Set
    End Property

    Public Property ACT_EFFORT_EST() As Double Implements ITasks.ACT_EFFORT_EST
        Get
            Return cTasks.ACT_EFFORT_EST
        End Get
        Set(ByVal value As Double)
            cTasks.ACT_EFFORT_EST = value
        End Set
    End Property

    Public Property ACT_EFFORT_EST_WK() As Double Implements ITasks.ACT_EFFORT_EST_WK
        Get
            Return cTasks.ACT_EFFORT_EST_WK
        End Get
        Set(ByVal value As Double)
            cTasks.ACT_EFFORT_EST_WK = value
        End Set
    End Property

    Public Property PROJECT_CODE() As Integer Implements ITasks.PROJECT_CODE
        Get
            Return cTasks.PROJECT_CODE
        End Get
        Set(ByVal value As Integer)
            cTasks.PROJECT_CODE = value
        End Set
    End Property

    Public Property REWORK() As Byte Implements ITasks.REWORK
        Get
            Return cTasks.REWORK
        End Get
        Set(ByVal value As Byte)
            cTasks.REWORK = value
        End Set
    End Property

    Public Property PHASE() As String Implements ITasks.PHASE
        Get
            Return cTasks.PHASE
        End Get
        Set(ByVal value As String)
            cTasks.PHASE = value
        End Set
    End Property

    Public Property OTHERS_1() As String Implements ITasks.OTHERS_1
        Get
            Return cTasks.OTHERS_1
        End Get
        Set(ByVal value As String)
            cTasks.OTHERS_1 = value
        End Set
    End Property

    Public Property OTHERS_2() As String Implements ITasks.OTHERS_2
        Get
            Return cTasks.OTHERS_2
        End Get
        Set(ByVal value As String)
            cTasks.OTHERS_2 = value
        End Set
    End Property

    Public Property OTHERS_3() As String Implements ITasks.OTHERS_3
        Get
            Return cTasks.OTHERS_3
        End Get
        Set(ByVal value As String)
            cTasks.OTHERS_3 = value
        End Set
    End Property
#End Region

#Region "STORED PROCS"
    Public Function getTaskDetailByTaskId(taskID As Integer) As TasksSet Implements ITasks.getTaskDetailByTaskId
        Try
            Dim key As clsTasksKeys = New clsTasksKeys(taskID)
            Dim objTasks As clsTasks
            objTasks = Me.cTasksFactory.getTaskDetailByTaskId(key)
            If IsNothing(objTasks) Then
                Throw New NoRecordFoundException("Record Not Found!")
            End If
            Dim _tasks As TasksSet = New TasksSet(objTasks)
            Return _tasks
        Catch ex As Exception
            If (ex.InnerException.GetType() = GetType(SqlException)) Then
                Throw New DatabaseConnExceptionFailed("Database Connection Failed!")
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function getTaskSummaryWeekly(ByVal empId As Integer, dateStart As DateTime) As List(Of TasksSet) Implements ITasks.getTaskSummaryWeekly
        Try
            Dim objTasksList As List(Of clsTasks)
            objTasksList = Me.cTasksFactory.getTaskSummaryWeekly(empId, dateStart)
            If IsNothing(objTasksList) Then
                Throw New NoRecordFoundException("Record Not Found!")
            End If
            Dim _tasksList As List(Of TasksSet) = New List(Of TasksSet)
            For Each _task As clsTasks In objTasksList
                _tasksList.Add(New TasksSet(_task))
            Next
            Return _tasksList
        Catch ex As Exception
            If (ex.InnerException.GetType() = GetType(SqlException)) Then
                Throw New DatabaseConnExceptionFailed("Database Connection Failed!")
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function getMyTasks(ByVal empId As Integer) As List(Of TasksSet) Implements ITasks.getMyTasks
        Try
            Dim objTasksList As List(Of clsTasks)
            objTasksList = Me.cTasksFactory.getMyTasks(empId)
            If IsNothing(objTasksList) Then
                Throw New NoRecordFoundException("Record Not Found!")
            End If
            Dim _tasksList As List(Of TasksSet) = New List(Of TasksSet)
            For Each _task As clsTasks In objTasksList
                _tasksList.Add(New TasksSet(_task))
            Next
            Return _tasksList
        Catch ex As Exception
            If (ex.InnerException.GetType() = GetType(SqlException)) Then
                Throw New DatabaseConnExceptionFailed("Database Connection Failed!")
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
#End Region

    Public Function InsertNewTask(task As TasksSet) As Boolean Implements ITasks.InsertNewTask
        Try
            Return Me.cTasksFactory.Insert(cTasks)
        Catch ex As Exception
            If (ex.InnerException.GetType() = GetType(SqlException)) Then
                Throw New DatabaseConnExceptionFailed("Database Connection Failed!")
            Else
                Throw New UpdateFailedException("Insert Tasks Failed!")
            End If
        End Try
    End Function

    Public Function UpdateTask() As Boolean Implements ITasks.UpdateTask
        Try
            Return Me.cTasksFactory.Update(cTasks)
        Catch ex As Exception
            If (ex.InnerException.GetType() = GetType(SqlException)) Then
                Throw New DatabaseConnExceptionFailed("Database Connection Failed!")
            Else
                Throw New UpdateFailedException("Update Tasks Failed!")
            End If
        End Try
    End Function

    Public Function GetAllTasks() As List(Of TasksSet) Implements ITasks.GetAllTasks
        Try
            Dim lstTasks As List(Of clsTasks)
            Dim lstTasksSet As New List(Of TasksSet)

            lstTasks = cTasksFactory.GetAll()

            If Not IsNothing(lstTasks) Then
                For Each cList As clsTasks In lstTasks
                    lstTasksSet.Add(New TasksSet(cList))
                Next
            Else
                Throw New NoRecordFoundException("No records found!")
            End If

            Return lstTasksSet
        Catch ex As Exception
            If (ex.InnerException.GetType() = GetType(SqlException)) Then
                Throw New DatabaseConnExceptionFailed("Database Connection Failed")
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(<CallerMemberName> Optional propertyName As [String] = "")
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

End Class

