Imports System.Collections.ObjectModel
Imports System.ServiceModel
Imports UI_AIDE_CommCellServices.ServiceReference1

<CallbackBehavior(ConcurrencyMode:=ConcurrencyMode.Single, UseSynchronizationContext:=False)>
Class TaskAddPage
    Implements IAideServiceCallback

    Public frame As Frame
    Public mainWindow As MainWindow

#Region "Provider Declaration"
    Dim taskDBProvider As New TaskDBProvider
    Dim projectDBProvider As New ProjectDBProvider
#End Region

#Region "View Model Declarations"
    Dim tasksViewModel As New TasksViewModel
    Dim projectViewModel As New ProjectViewModel
#End Region

#Region "Model Declaration"
    Dim tasksModel As New TasksModel
#End Region

    Dim tasks As New Tasks
    Dim client As AideServiceClient

    Public Sub New(_frame As Frame, _mainWindow As MainWindow)
        InitializeComponent()
        frame = _frame
        mainWindow = _mainWindow
        LoadData()
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

    Private Sub LoadData()
        Me.InitializeService()

        CreateTaskID()
        SetEmployeeID()

        ' Load Items For Category Combobox
        Try
            Dim lstStatus As StatusGroup() = client.GetStatusList(2)
            Dim listCategoryStatus As New ObservableCollection(Of CategoryStatusModel)

            For Each objStatus As StatusGroup In lstStatus
                taskDBProvider.SetMyCategoryStatusList(objStatus)
            Next

            For Each myStatus As MyCategoryStatusList In taskDBProvider.GetCategoryStatusList()
                listCategoryStatus.Add(New CategoryStatusModel(myStatus))
            Next

            tasksViewModel.CategoryStatusList = listCategoryStatus
        Catch ex As SystemException
            client.Abort()
        End Try

        ' Load Items For Task Status Combobox
        Try
            Dim lstStatus As StatusGroup() = client.GetStatusList(3)
            Dim listTaskStatus As New ObservableCollection(Of TaskStatusModel)

            For Each objStatus As StatusGroup In lstStatus
                taskDBProvider.SetMyTaskStatusList(objStatus)
            Next

            For Each myStatus As MyTaskStatusList In taskDBProvider.GetTaskStatusList()
                listTaskStatus.Add(New TaskStatusModel(myStatus))
            Next

            tasksViewModel.TaskStatusList = listTaskStatus
        Catch ex As SystemException
            client.Abort()
        End Try

        ' Load Items For Phase Status Combobox
        Try
            Dim lstStatus As StatusGroup() = client.GetStatusList(4)
            Dim listPhaseStatus As New ObservableCollection(Of PhaseStatusModel)

            For Each objStatus As StatusGroup In lstStatus
                taskDBProvider.SetMyPhaseStatusList(objStatus)
            Next

            For Each myStatus As MyPhaseStatusList In taskDBProvider.GetPhaseStatusList()
                listPhaseStatus.Add(New PhaseStatusModel(myStatus))
            Next

            tasksViewModel.PhaseStatusList = listPhaseStatus
        Catch ex As SystemException
            client.Abort()
        End Try

        ' Load Items For Rework Combobox
        Try
            Dim lstStatus As StatusGroup() = client.GetStatusList(5)
            Dim listReworkStatus As New ObservableCollection(Of ReworkStatusModel)

            For Each objStatus As StatusGroup In lstStatus
                taskDBProvider.SetMyReworkStatusList(objStatus)
            Next

            For Each myStatus As MyReworkStatusList In taskDBProvider.GetReworkStatusList()
                listReworkStatus.Add(New ReworkStatusModel(myStatus))
            Next

            tasksViewModel.ReworkStatusList = listReworkStatus
        Catch ex As SystemException
            client.Abort()
        End Try

        tasksViewModel.NewTasks = tasksModel
        Me.DataContext = tasksViewModel

        ' Load Items For Projects
        Try
            Dim lstProjects As Project() = client.GetProjectList()
            Dim listProjects As New ObservableCollection(Of ProjectModel)

            For Each objProjects As Project In lstProjects
                projectDBProvider.SetProjectList(objProjects)
            Next

            For Each myProjects As MyProjectList In projectDBProvider.GetProjectList()
                listProjects.Add(New ProjectModel(myProjects))
            Next

            projectViewModel.ProjectList = listProjects
            cboProject.DataContext = projectViewModel
        Catch ex As Exception
            client.Abort()
        End Try

    End Sub

    Private Sub CreateTaskID()
        Try
            If Me.InitializeService() Then
                Dim lstTasks As Tasks() = client.GetAllTasks()
                Dim totalCount As Integer

                totalCount = lstTasks.Length + 1
                tasksModel.TaskId = totalCount
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "FAILED")
        End Try
    End Sub

    Private Sub SetEmployeeID()
        tasksModel.EmpId = mainWindow.EmployeeID
    End Sub

    Private Sub btnCreate_Click(sender As Object, e As RoutedEventArgs) Handles btnCreate.Click
        If Not FindMissingFields(Me.DataContext) Then
            GetDataContext(Me.DataContext)

            Try
                Dim result As Integer = MsgBox("Are you sure?", MsgBoxStyle.YesNo, "CREATE")

                If result = vbYes Then
                    If Me.InitializeService Then
                        client.CreateTask(tasks)
                        MsgBox("Successfully Created", MsgBoxStyle.Information, "Success")
                        ClearValues()
                    End If
                End If
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Failed")
            End Try
        End If
    End Sub

    Private Sub btnBack_Click(sender As Object, e As RoutedEventArgs) Handles btnBack.Click
        frame.Navigate(New TaskPage(frame, mainWindow))
    End Sub

    Private Sub GetDataContext(obj As TasksViewModel)
        tasks.TaskID = obj.NewTasks.TaskId
        tasks.EmpID = obj.NewTasks.EmpId
        tasks.IncidentID = obj.NewTasks.IncId
        tasks.DateStarted = obj.NewTasks.DateStarted
        tasks.TargetDate = obj.NewTasks.TargetDate

        If Not obj.NewTasks.CompltdDate.Equals(String.Empty) Then
            tasks.CompletedDate = obj.NewTasks.CompltdDate
        End If

        tasks.DateCreated = Date.Now.ToShortDateString
        tasks.Status = obj.NewTasks.Status
        tasks.EffortEst = obj.NewTasks.EffortEst
        tasks.ActualEffortEst = obj.NewTasks.ActEffortEst
        tasks.EffortEstWk = obj.NewTasks.ActEffortEstWk
        tasks.TaskType = obj.NewTasks.TaskType
        tasks.ProjectID = cboProject.SelectedValue
        tasks.ProjectCode = cboProject.SelectedValue
        tasks.Rework = obj.NewTasks.Rework
        tasks.Phase = obj.NewTasks.Phase
        tasks.TaskDescr = obj.NewTasks.TaskDescr
        tasks.IncidentDescr = obj.NewTasks.IncDescr
        tasks.Remarks = obj.NewTasks.Remarks
        tasks.Others1 = obj.NewTasks.Others1
        tasks.Others2 = obj.NewTasks.Others2
        tasks.Others3 = obj.NewTasks.Others3
    End Sub

    Private Sub ClearValues()
        tasksModel.IncId = Nothing

        tasksModel.EffortEst = Nothing
        tasksModel.ActEffortEst = Nothing
        tasksModel.ActEffortEstWk = Nothing

        tasksModel.DateStarted = Nothing
        tasksModel.TargetDate = Nothing
        tasksModel.CompltdDate = Nothing

        tasksModel.IncDescr = Nothing
        tasksModel.Remarks = Nothing
        tasksModel.TaskDescr = Nothing

        tasksModel.Status = Nothing
        tasksModel.TaskType = Nothing
        tasksModel.Phase = Nothing

        cboProject.SelectedIndex = -1
    End Sub

    Private Function FindMissingFields(obj As TasksViewModel) As Boolean
        If obj.NewTasks.DateStarted.Equals(String.Empty) _
            OrElse obj.NewTasks.TargetDate.Equals(String.Empty) _
            OrElse obj.NewTasks.Status.Equals(0) _
            OrElse obj.NewTasks.EffortEst.Equals(String.Empty) _
            OrElse obj.NewTasks.ActEffortEst.Equals(String.Empty) _
            OrElse obj.NewTasks.ActEffortEstWk.Equals(String.Empty) _
            OrElse obj.NewTasks.TaskType.Equals(0) _
            OrElse cboProject.SelectedIndex.Equals(-1) _
            OrElse obj.NewTasks.Phase.Equals(0) Then

            MsgBox("Please Fill All Required Fields", MsgBoxStyle.Exclamation, "FAILED")
            Return True
        End If
        Return False
    End Function

    Private Sub dpStartDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles dpStartDate.SelectedDateChanged
        If Not dpStartDate.Text.Equals(String.Empty) Then
            dpTargetDate.DisplayDateStart = tasksViewModel.NewTasks.DateStarted
            dpCompltdDate.DisplayDateStart = tasksViewModel.NewTasks.DateStarted
        End If
    End Sub
End Class
