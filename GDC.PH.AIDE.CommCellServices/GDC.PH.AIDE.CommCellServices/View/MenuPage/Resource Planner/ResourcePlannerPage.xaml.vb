Imports System.IO
Imports System.Data
Imports System.Collections.ObjectModel
Imports UI_AIDE_CommCellServices.ServiceReference1
Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ServiceModel

Class ResourcePlannerPage
    Implements IAideServiceCallback


#Region "Fields"
    Private client As AideServiceClient
    Private _ResourceDBProvider As New ResourcePlannerDBProvider
    Private _ResourceViewModel As New ResourcePlannerViewModel

    Dim empid As Integer
    Dim deptID As Integer
    Dim posID As Integer
    Dim positionID As Integer
    Dim month As Integer
    Dim setStatus As Integer
    Dim displayStatus As String = String.Empty
    Dim status As Integer
    Dim img As String
    Dim displayMonth As String
    Dim checkStatus As Integer
    Dim count As Integer
    Dim year As Integer
    Dim day As Integer
#End Region

    Public Sub New(empid As Integer, deptIDs As Integer, posID As Integer)
        Me.empid = empid
        Me.deptID = deptIDs
        Me.posID = posID
        Me.InitializeComponent()

        month = calendar.DisplayDate.Month
        year = calendar.DisplayDate.Year
        SetMonths()
        LoadEmployee()
        LoadCategory()
        LoadAllEmpResourcePlanner()
        LoadAllCategory()
    End Sub

    Public Function InitializeService() As Boolean
        Dim bInitialize As Boolean = False
        Try
            Dim Context As InstanceContext = New InstanceContext(Me)
            client = New AideServiceClient(Context)
            client.Open()
            bInitialize = True
        Catch ex As SystemException
            client.Abort()
        End Try
        Return bInitialize
    End Function

    Public Sub LoadEmployee()
        Try
            InitializeService()
            Dim lstresource As ResourcePlanner() = client.ViewEmpResourcePlanner(deptID)
            Dim resourcelist As New ObservableCollection(Of ResourcePlannerModel)

            For Each objResource As ResourcePlanner In lstresource
                _ResourceDBProvider.SetResourceList(objResource)
            Next

            For Each iResource As myResourceList In _ResourceDBProvider.GetResourceList()
                resourcelist.Add(New ResourcePlannerModel(iResource))
            Next
            _ResourceViewModel.ResourceList = resourcelist
            lstEmployee.DataContext = _ResourceViewModel
            img = _ResourceViewModel.ResourceList(0).EmpImage
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "FAILED")
        End Try
    End Sub

    Public Sub LoadCategory()
        Try
            InitializeService()
            Dim lstresource As ResourcePlanner() = client.GetStatusResourcePlanner()
            Dim resourcelist As New ObservableCollection(Of ResourcePlannerModel)

            For Each objResource As ResourcePlanner In lstresource
                _ResourceDBProvider.SetCategoryList(objResource)
            Next

            For Each iResource As myResourceList In _ResourceDBProvider.GetCategoryList()
                resourcelist.Add(New ResourcePlannerModel(iResource))
            Next
            _ResourceViewModel.CategoryList = resourcelist
            cbCategory.DataContext = _ResourceViewModel
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "FAILED")
        End Try
    End Sub

    'Loades All Status
    Public Sub LoadAllCategory()
        Try
            InitializeService()
            Dim lstresource As ResourcePlanner() = client.GetAllStatusResourcePlanner()
            Dim resourcelist As New ObservableCollection(Of ResourcePlannerModel)

            For Each objResource As ResourcePlanner In lstresource
                _ResourceDBProvider.SetAllCategoryList(objResource)
            Next

            For Each iResource As myResourceList In _ResourceDBProvider.GetAllCategoryList()
                resourcelist.Add(New ResourcePlannerModel(iResource))
            Next
            _ResourceViewModel.FilterCategoryList = resourcelist
            cbFilterCategory.DataContext = _ResourceViewModel
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "FAILED")
        End Try
    End Sub

    Public Sub SetCategory()
        If setStatus = 1 Then
            displayStatus = "Absent"
        ElseIf setStatus = 2 Then
            displayStatus = "Present"
        ElseIf setStatus = 3 Then
            displayStatus = "Sick Leave"
            'displayStatus = Colors.Red.ToString()
        ElseIf setStatus = 4 Then
            displayStatus = "Vacation Leave"
        End If
    End Sub

    Public Sub LoadEmpResourcePlanner()
        Try
            InitializeService()
            _ResourceDBProvider._splist.Clear()
            Dim lstresource As ResourcePlanner() = client.GetResourcePlannerByEmpID(txtEmpID.Text, deptID, month, year)
            Dim resourcelist As New ObservableCollection(Of ResourcePlannerModel)

            Dim it As New List(Of Dictionary(Of String, String))()
            Dim dict As New Dictionary(Of String, String)()

            For Each objResource As ResourcePlanner In lstresource
                _ResourceDBProvider.SetEmpRPList(objResource)
            Next
            For Each iResource As myResourceList In _ResourceDBProvider.GetEmpRPList()
                resourcelist.Add(New ResourcePlannerModel(iResource))
                setStatus = iResource.Status
                checkStatus = iResource.Status

                SetCategory()
                'dict.Add(iResource.Date_Entry.DayOfWeek, iResource.Date_Entry.Day)
                dict.Add(iResource.Date_Entry.ToLongDateString.Substring(0, iResource.Date_Entry.ToLongDateString.Length - 6), displayStatus)  ' Add list of dictionary
            Next
            it.Add(dict)
            Dim table As DataTable = New DataTable
            table = ToDataTable(it)
            dgResourcePlanner.ItemsSource = table.AsDataView
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "FAILED")
        End Try
    End Sub

    Public Sub LoadAllEmpResourcePlanner()
        Try
            InitializeService()
            _ResourceDBProvider._splist.Clear()
            Dim lstresource As ResourcePlanner() = client.GetAllEmpResourcePlanner(deptID, month, year)
            Dim resourcelist As New ObservableCollection(Of ResourcePlannerModel)

            Dim emp_id As Integer
            Dim it As New List(Of Dictionary(Of String, String))()
            Dim dict As New Dictionary(Of String, String)()

            For Each objResource As ResourcePlanner In lstresource
                _ResourceDBProvider.SetAllEmpRPList(objResource)
            Next

            For Each iResource As myResourceList In _ResourceDBProvider.GetAllEmpRPList()
                resourcelist.Add(New ResourcePlannerModel(iResource))

                setStatus = iResource.Status
                SetCategory()
                If iResource.Emp_ID = emp_id Then
                    dict.Add(iResource.Date_Entry.ToLongDateString.Substring(0, iResource.Date_Entry.ToLongDateString.Length - 6), displayStatus)
                End If
                If emp_id <> iResource.Emp_ID Then
                    If emp_id > 0 Then
                        it.Add(dict)
                    End If
                    dict = New Dictionary(Of String, String)()
                    dict.Add("Employee ID", iResource.Emp_ID)
                    dict.Add("Employee Name", iResource.Emp_Name)
                    dict.Add(iResource.Date_Entry.ToLongDateString.Substring(0, iResource.Date_Entry.ToLongDateString.Length - 6), displayStatus)
                End If
                emp_id = iResource.Emp_ID
            Next
            it.Add(dict)
            Dim table As DataTable = New DataTable
            table = ToDataTable(it)
            dgResourcePlanner.ItemsSource = table.AsDataView
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "FAILED")
        End Try
    End Sub

    'Apply in cbFilterSelectionChanged
    Public Sub LoadAllEmpResourcePlannerByStatus()
        Try
            InitializeService()
            _ResourceDBProvider._splist.Clear()
            Dim lstresource As ResourcePlanner() = client.GetAllEmpResourcePlannerByStatus(deptID, month, year, cbFilterCategory.SelectedValue)
            Dim resourcelist As New ObservableCollection(Of ResourcePlannerModel)

            Dim emp_id As Integer
            Dim it As New List(Of Dictionary(Of String, String))()
            Dim dict As New Dictionary(Of String, String)()

            For Each objResource As ResourcePlanner In lstresource
                _ResourceDBProvider.SetAllEmpRPList(objResource)
            Next

            For Each iResource As myResourceList In _ResourceDBProvider.GetAllEmpRPList()
                resourcelist.Add(New ResourcePlannerModel(iResource))

                setStatus = iResource.Status
                SetCategory()

                If iResource.Emp_ID = emp_id Then
                    dict.Add(iResource.Date_Entry.ToLongDateString.Substring(0, iResource.Date_Entry.ToLongDateString.Length - 6), displayStatus)
                End If
                If emp_id <> iResource.Emp_ID Then
                    If emp_id > 0 Then
                        it.Add(dict)
                    End If
                    dict = New Dictionary(Of String, String)()
                    dict.Add("Employee ID", iResource.Emp_ID)
                    dict.Add("Employee Name", iResource.Emp_Name)
                    dict.Add(iResource.Date_Entry.ToLongDateString.Substring(0, iResource.Date_Entry.ToLongDateString.Length - 6), displayStatus)
                End If
                emp_id = iResource.Emp_ID
            Next
            If resourcelist.Count = 0 Then
                dgResourcePlanner.ItemsSource = Nothing
            Else
                it.Add(dict)
                Dim table As DataTable = New DataTable
                table = ToDataTable(it)
                dgResourcePlanner.ItemsSource = table.AsDataView
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "FAILED")
        End Try
    End Sub

    Private Function ToDataTable(list As List(Of Dictionary(Of String, String))) As DataTable
        Dim result As New DataTable()

        If list.Count = 0 Then
            Return result
        End If

        Dim columnNames = list.SelectMany(Function(dict) dict.Keys).Distinct()
        result.Columns.AddRange(columnNames.[Select](Function(c) New DataColumn(c)).ToArray())
        For Each item As Dictionary(Of String, String) In list
            Dim row = result.NewRow()
            For Each key In item.Keys
                row(key) = item(key)
            Next
            result.Rows.Add(row)
        Next
        Return result
    End Function

    Private Sub lstEmployee_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles lstEmployee.SelectionChanged
        If empid <> lstEmployee.SelectedValue Then
            MsgBox("Sorry You Do Not Have Authorization For This Employee", MsgBoxStyle.Exclamation, "Employee Assist Tools")
        Else
            txtEmpID.Text = lstEmployee.SelectedValue
            LoadEmpResourcePlanner()
            cbCategory.IsEnabled = True
        End If
    End Sub

    Private Sub btnCreateLeave_Click(sender As Object, e As RoutedEventArgs) Handles btnCreateLeave.Click
        Try
            GetStatus()
            If posID = 1 Then '1 FOR MANAGER
                If cbCategory.SelectedValue = setStatus Then
                    Dim notify = MsgBox("There is Already an Existing " & cbCategory.Text & " For This Date" & vbNewLine & "Do you wish to proceed?", MsgBoxStyle.YesNo, "Employee Assist Tools")
                    If notify = MsgBoxResult.Yes Then
                        InsertResourcePlanner()
                    End If
                Else
                    Dim ans = MsgBox("Are you sure you want to Create a " & cbCategory.Text & " Leave", MsgBoxStyle.YesNo, "Employee Assist Tools")
                    If ans = MsgBoxResult.Yes Then
                        InsertResourcePlanner()
                    End If
                End If
            Else
                If txtEmpID.Text = empid Then
                    Dim ans = MsgBox("Are you sure you want to Create a " & cbCategory.Text & " Leave", MsgBoxStyle.YesNo, "Employee Assist Tools")
                    If ans = MsgBoxResult.Yes Then
                        InsertResourcePlanner()
                    End If
                Else
                    MsgBox("Sorry You Do Not Have Any Authorization To Create " & cbCategory.Text & " Leave For This Employee", MsgBoxStyle.Exclamation, "Employee Assist Tools")
                End If
                Clear()
                disableBtns()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Employee Assist Tools")
        End Try
    End Sub

    Public Sub InsertResourcePlanner()
        Dim Resource As New ResourcePlanner
        Resource.NAME = ""
        Resource.DESCR = ""
        Resource.Image_Path = ""
        Resource.EmpID = txtEmpID.Text
        Resource.dateFrom = dtpFrom.SelectedDate
        Resource.dateTo = dtpTo.SelectedDate
        Resource.Status = status
        client.UpdateResourcePlanner(Resource)
        _ResourceDBProvider._splist.Clear()
        MsgBox("Successfully Applied " & cbCategory.Text & " Application", MsgBoxStyle.Information, "Employee Assist Tools")
        LoadEmpResourcePlanner()
    End Sub

    Public Sub disableBtns()
        dtpFrom.IsEnabled = False
        dtpTo.IsEnabled = False
        btnCreateLeave.IsEnabled = False
    End Sub

    Public Sub GetStatus()
        '3 is for Sick Leave
        '4 is for Vacation Leave
        If cbCategory.SelectedValue = 3 Then
            status = 3
        ElseIf cbCategory.SelectedValue = 4 Then
            status = 4
        End If
    End Sub

    Public Sub Clear()
        dtpFrom.Text = String.Empty
        dtpTo.Text = String.Empty
        cbCategory.Text = String.Empty
    End Sub

    Private Sub calendar_DisplayDateChanged(sender As Object, e As CalendarDateChangedEventArgs) Handles calendar.DisplayDateChanged
        txtDisplayMonth.Text = String.Empty
        month = calendar.DisplayDate.Month
        year = calendar.DisplayDate.Year
        LoadAllEmpResourcePlanner()
    End Sub

    Private Sub dtpFrom_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles dtpFrom.SelectedDateChanged
        If dtpFrom.SelectedDate < System.DateTime.Today.ToShortDateString Then
            MsgBox("Dates Should be not the Day Before!", MsgBoxStyle.Critical, "Employee Assist Tools")
            LoadEmpResourcePlanner()
        Else
            dtpTo.IsEnabled = True
        End If
    End Sub

    Private Sub dtpTo_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles dtpTo.SelectedDateChanged
        If dtpTo.SelectedDate < dtpFrom.SelectedDate Then
            MsgBox("Date Should be not the Day Before Selected From Date!", MsgBoxStyle.Critical, "Employee Assist Tools")
            LoadEmpResourcePlanner()
        Else
            btnCreateLeave.IsEnabled = True
        End If
    End Sub

    Public Sub SetMonths()
        Select Case month
            Case "1"
                displayMonth = "January"
            Case "2"
                displayMonth = "Febuary"
            Case "3"
                displayMonth = "March"
            Case "4"
                displayMonth = "April"
            Case "5"
                displayMonth = "May"
            Case "6"
                displayMonth = "June"
            Case "7"
                displayMonth = "July"
            Case "8"
                displayMonth = "August"
            Case "9"
                displayMonth = "September"
            Case "10"
                displayMonth = "October"
            Case "11"
                displayMonth = "November"
            Case "12"
                displayMonth = "December"
        End Select
    End Sub

    Private Sub cbCategory_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cbCategory.SelectionChanged
        dtpFrom.IsEnabled = True
    End Sub

    Private Sub cbFilterCategory_DropDownClosed(sender As Object, e As EventArgs) Handles cbFilterCategory.DropDownClosed
        SetMonths()
        txtDisplayMonth.Text = cbFilterCategory.Text & " For The Month Of " & displayMonth & " " & year & ": " & dgResourcePlanner.Items.Count
        LoadAllEmpResourcePlannerByStatus()
    End Sub

    Private Sub btnViewAll_Click(sender As Object, e As RoutedEventArgs) Handles btnViewAll.Click
        txtDisplayMonth.Text = String.Empty
        cbFilterCategory.Text = String.Empty
        LoadAllEmpResourcePlanner()
    End Sub

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
End Class
