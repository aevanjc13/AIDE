Imports System.Collections.ObjectModel
Imports System.ServiceModel
Imports UI_AIDE_CommCellServices.ServiceReference1

<CallbackBehavior(ConcurrencyMode:=ConcurrencyMode.Single, UseSynchronizationContext:=False)>
Class TaskAdminPage
    Implements IAideServiceCallback

    Public frame As Frame
    Public mainWindow As MainWindow

    Dim lstMyTasks As New ObservableCollection(Of TasksSpModel)
    Dim taskDBProvider As New TaskDBProvider
    Dim taskSpViewModel As New TasksSpViewModel

    Dim client As AideServiceClient

    Public Sub New(_frame As Frame, _mainWindow As MainWindow)
        InitializeComponent()
        frame = _frame
        mainWindow = _mainWindow
        SetDates()
        LoadEmployeeTaskAll()
    End Sub

#Region "Common Methods"
    Private Function InitializeService() As Boolean
        Dim bInitialize As Boolean = False
        Try
            Dim context As InstanceContext = New InstanceContext(Me)
            client = New AideServiceClient(context)
            client.Open()
            bInitialize = True
        Catch ex As SystemException
            client.Abort()
        End Try
        Return bInitialize
    End Function

    Public Sub NotifyError(message As String) Implements IAideServiceCallback.NotifyError

    End Sub

    Public Sub NotifyOffline(EmployeeName As String) Implements IAideServiceCallback.NotifyOffline

    End Sub

    Public Sub NotifyPresent(EmployeeName As String) Implements IAideServiceCallback.NotifyPresent

    End Sub

    Public Sub NotifySuccess(message As String) Implements IAideServiceCallback.NotifySuccess

    End Sub

    Public Sub NotifyUpdate(objData As Object) Implements IAideServiceCallback.NotifyUpdate

    End Sub
#End Region

#Region "Private Methods"

    Private Sub SetDates()
        Dim today As Date = Date.Today

        Dim dayMonDiff As Integer = today.DayOfWeek - DayOfWeek.Monday
        Dim dayTueDiff As Integer = today.DayOfWeek - DayOfWeek.Tuesday
        Dim dayWedDiff As Integer = today.DayOfWeek - DayOfWeek.Wednesday
        Dim dayThuDiff As Integer = today.DayOfWeek - DayOfWeek.Thursday
        Dim dayFriDiff As Integer = today.DayOfWeek - DayOfWeek.Friday

        Dim monday As Date = today.AddDays(-dayMonDiff)
        Dim tuesday As Date = today.AddDays(-dayTueDiff)
        Dim wednesday As Date = today.AddDays(-dayWedDiff)
        Dim thursday As Date = today.AddDays(-dayThuDiff)
        Dim friday As Date = today.AddDays(-dayFriDiff)

        lblMonday.Content = monday
        lblTuesday.Content = tuesday
        lblWednesday.Content = wednesday
        lblThursday.Content = thursday
        lblFriday.Content = friday
    End Sub

    Private Sub LoadEmployeeTaskAll()
        Try
            If Me.InitializeService Then
                Dim lstTasks As TaskSummary()

                lstTasks = client.ViewTaskSummaryAll(Convert.ToDateTime(Date.Now).ToString("yyyy-MM-dd"))

                For Each objTasks As TaskSummary In lstTasks
                    taskDBProvider.SetTasksSpList(objTasks)
                Next

                For Each iTasks As MyTasksSp In taskDBProvider.GetTasksSp()
                    lstMyTasks.Add(New TasksSpModel(iTasks))
                Next

                taskSpViewModel.TasksSpList = lstMyTasks

                Me.DataContext = taskSpViewModel
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "FAILED")
        End Try
    End Sub
#End Region
    
End Class
