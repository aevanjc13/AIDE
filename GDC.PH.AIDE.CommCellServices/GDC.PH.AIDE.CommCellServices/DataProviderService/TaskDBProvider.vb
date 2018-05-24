Imports System.Collections.ObjectModel
Imports UI_AIDE_CommCellServices.ServiceReference1

Public Class TaskDBProvider

    Private _taskStatusList As New ObservableCollection(Of MyTaskStatusList)
    Private _categoryStatusList As New ObservableCollection(Of MyCategoryStatusList)
    Private _phaseStatusList As New ObservableCollection(Of MyPhaseStatusList)
    Private _reworkStatusList As New ObservableCollection(Of MyReworkStatusList)

    Private _tasksSpList As New ObservableCollection(Of MyTasksSp)

    Public Sub SetMyTaskStatusList(ByVal _status As StatusGroup)
        Dim status As New MyTaskStatusList
        status.Key = _status.Status
        status.Value = _status.Description
        _taskStatusList.Add(status)
    End Sub

    Public Sub SetMyCategoryStatusList(ByVal _status As StatusGroup)
        Dim status As New MyCategoryStatusList
        status.Key = _status.Status
        status.Value = _status.Description
        _categoryStatusList.Add(status)
    End Sub

    Public Sub SetMyPhaseStatusList(ByVal _status As StatusGroup)
        Dim status As New MyPhaseStatusList
        status.Key = _status.Status
        status.Value = _status.Description
        _phasestatusList.Add(status)
    End Sub

    Public Sub SetMyReworkStatusList(ByVal _status As StatusGroup)
        Dim status As New MyReworkStatusList
        status.Key = _status.Status
        status.Value = _status.Description
        _reworkStatusList.Add(status)
    End Sub

    Public Sub SetTasksSpList(ByVal _tasks As TaskSummary)
        Dim _objTask As MyTasksSp = New MyTasksSp With {.EmployeeName = _tasks.Name, _
                                                        .FriAt = _tasks.FridayAT, _
                                                        .FriCt = _tasks.FridayCT, _
                                                        .FriOt = _tasks.FridayOT, _
                                                        .LastWeekOutstanding = _tasks.OutstandingTaskLastWeek, _
                                                        .MonAt = _tasks.MondayAT, _
                                                        .MonCt = _tasks.MondayCT, _
                                                        .MonOt = _tasks.MondayOT, _
                                                        .ThuAt = _tasks.ThursdayAT, _
                                                        .ThuCt = _tasks.ThursdayCT, _
                                                        .ThuOt = _tasks.ThursdayOT, _
                                                        .TueAt = _tasks.TuesdayAT, _
                                                        .TueCt = _tasks.TuesdayCT, _
                                                        .TueOt = _tasks.TuesdayOT, _
                                                        .WedAt = _tasks.WednesdayAT, _
                                                        .WedCt = _tasks.WednesdayCT, _
                                                        .WedOt = _tasks.WednesdayOT}
        _tasksSpList.Add(_objTask)
    End Sub

    Public Function GetTaskStatusList() As ObservableCollection(Of MyTaskStatusList)
        Return _taskstatusList
    End Function

    Public Function GetCategoryStatusList() As ObservableCollection(Of MyCategoryStatusList)
        Return _categoryStatusList
    End Function

    Public Function GetPhaseStatusList() As ObservableCollection(Of MyPhaseStatusList)
        Return _phaseStatusList
    End Function

    Public Function GetReworkStatusList() As ObservableCollection(Of MyReworkStatusList)
        Return _reworkStatusList
    End Function

    Public Function GetTasksSp() As ObservableCollection(Of MyTasksSp)
        Return _tasksSpList
    End Function
End Class

Public Class MyTasks
    Public Property TaskId As Integer
    Public Property EmpId As Integer
    Public Property IncId As String
    Public Property TaskType As Short
    Public Property ProjId As Integer
    Public Property IncDescr As String
    Public Property TaskDescr As String
    Public Property DateStarted As Date
    Public Property TargetDate As Date
    Public Property CompltdDate As Date
    Public Property DateCreated As Date
    Public Property Status As Short
    Public Property Remarks As String
    Public Property EffortEst As Double
    Public Property ActEffortEst As Double
    Public Property ActEffortEstWk As Double
    Public Property ProjectCode As Integer
    Public Property Rework As Short
    Public Property Phase As String
    Public Property HoursWorked_Date As String
    Public Property Others1 As String
    Public Property Others2 As String
    Public Property Others3 As String
End Class

Public Class MyTasksSp

    Public Property EmployeeName As String
    Public Property LastWeekOutstanding As Integer

    Public Property MonAt As Integer
    Public Property MonCt As Integer
    Public Property MonOt As Integer

    Public Property TueAt As Integer
    Public Property TueCt As Integer
    Public Property TueOt As Integer

    Public Property WedAt As Integer
    Public Property WedCt As Integer
    Public Property WedOt As Integer

    Public Property ThuAt As Integer
    Public Property ThuCt As Integer
    Public Property ThuOt As Integer

    Public Property FriAt As Integer
    Public Property FriCt As Integer
    Public Property FriOt As Integer

End Class

Public Class MyTaskStatusList
    Public Property Key As Integer
    Public Property Value As String
End Class

Public Class MyCategoryStatusList
    Public Property Key As Integer
    Public Property Value As String
End Class

Public Class MyPhaseStatusList
    Public Property Key As Integer
    Public Property Value As String
End Class

Public Class MyReworkStatusList
    Public Property Key As Integer
    Public Property Value As String
End Class


