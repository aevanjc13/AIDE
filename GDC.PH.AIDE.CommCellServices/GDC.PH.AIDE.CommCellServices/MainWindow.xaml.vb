Imports UI_AIDE_CommCellServices.ServiceReference1
Imports System.Collections.ObjectModel
Imports System.ServiceModel
Imports System
Imports System.Windows
Imports System.Windows.Threading

Class MainWindow
    Implements IAideServiceCallback

    Public email As String = "a.batongbacal@ph.fujitsu.com"
    Private departmentID As Integer
    Private empID As Integer
    Private permission As Integer

    Dim profileDBProvider As New ProfileDBProvider
    Dim profileViewModel As New ProfileViewModel
    Dim profile As Profile
    Dim aideClientService As AideServiceClient

    Public Sub New()
        InitializeComponent()
        profile = New Profile
        profile.Emp_ID = 173692
        profile.Dept_ID = 1
        attendance()
    End Sub

    Public Sub New(_email As String)
        InitializeComponent()
        getTime()
        email = _email
        MsgBox("Welcome " & email, MsgBoxStyle.Information)
        SetEmployeeData()
        attendance()
    End Sub

#Region "Property declarations"
    Private _EmailAddress As String
    Public Property EmailAddress() As String
        Get
            Return _EmailAddress
        End Get
        Set(ByVal value As String)
            _EmailAddress = value
        End Set
    End Property

    Private _EmployeeID As Integer
    Public Property EmployeeID() As Integer
        Get
            Return _EmployeeID
        End Get
        Set(ByVal value As Integer)
            _EmployeeID = value
        End Set
    End Property

    Private _DeptID As Integer
    Public Property DeptID() As Integer
        Get
            Return _DeptID
        End Get
        Set(ByVal value As Integer)
            _DeptID = value
        End Set
    End Property

    Private _PosID As String
    Public Property PosID() As String
        Get
            Return _PosID
        End Get
        Set(ByVal value As String)
            _PosID = value
        End Set
    End Property

    Private _EmployeeName As String
    Public Property EmployeeName() As String
        Get
            Return _EmployeeName
        End Get
        Set(ByVal value As String)
            _EmployeeName = value
        End Set
    End Property

    Private _IsManagerSignedOn As Boolean
    Public Property IsManagerSignedOn() As Boolean
        Get
            Return _IsManagerSignedOn
        End Get
        Set(ByVal value As Boolean)
            _IsManagerSignedOn = value
        End Set
    End Property

    Private _IsSignedOn As Boolean
    Public Property IsSignedOn() As Boolean
        Get
            Return _IsSignedOn
        End Get
        Set(ByVal value As Boolean)
            _IsSignedOn = value
        End Set
    End Property

#End Region

#Region "Common Methods"

    Public Function InitializeService() As Boolean
        Dim bInitialize As Boolean = False
        Try
            Dim Context As InstanceContext = New InstanceContext(Me)
            aideClientService = New AideServiceClient(Context)
            aideClientService.Open()
            bInitialize = True
        Catch ex As SystemException
            aideClientService.Abort()
        End Try
        Return bInitialize
    End Function

    Private Function SignOn() As Boolean
        Try
            profile = aideClientService.SignOn(email)
            Return True
        Catch ex As SystemException
            aideClientService.Abort()
            Return False
        End Try
    End Function

    Public Sub SaveProfile(ByVal _profile As Profile)
        Try
            If Not IsNothing(_profile) Then
                EmployeeID = _profile.Emp_ID
                DeptID = _profile.Dept_ID
                PosID = _profile.Position
                If (_profile.Permission = 1) Then
                    IsManagerSignedOn = True
                Else
                    IsManagerSignedOn = False
                End If

                profileDBProvider.SetMyProfile(_profile)
                profileViewModel.SelectedUser = New ProfileModel(profileDBProvider.GetMyProfile())
            End If
        Catch ex As SystemException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "FAILED")
        End Try
    End Sub

    Private Sub SetEmployeeData()
        Try
            If email <> String.Empty Then
                If Me.InitializeService Then
                    If Me.SignOn Then
                        IsSignedOn = True
                        SaveProfile(profile)
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "FAILED")
        End Try
    End Sub
#End Region

    Private Sub HomeBtn_Click(sender As Object, e As RoutedEventArgs) Handles HomeBtn.Click
        Dim s As ProfileModel = New ProfileModel
        s.EmailAddress = email
        'FrameNavi.Navigate(New HomePage(email))
    End Sub
    Public Sub attendance()
        Try
            InitializeService()
            Dim empID As New AttendanceSummary
            empID.EmployeeID = EmployeeID
            aideClientService.InsertAttendance(empID)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ImprovementBtn_Click(sender As Object, e As RoutedEventArgs) Handles ImprovementBtn.Click
        PagesFrame.Navigate(New ThreeC_Page(email, PagesFrame))
        SubMenuFrame.Navigate(New ImproveSubMenuPage(PagesFrame, email, Me))
    End Sub

    Public Sub NotifyError(message As String) Implements IAideServiceCallback.NotifyError
        If message <> String.Empty Then
            MessageBox.Show(message)
        End If
    End Sub

    Public Sub NotifyOffline(EmployeeName As String) Implements IAideServiceCallback.NotifyOffline

    End Sub

    Public Sub NotifyPresent(EmployeeName As String) Implements IAideServiceCallback.NotifyPresent

    End Sub

    Public Sub NotifySuccess(message As String) Implements IAideServiceCallback.NotifySuccess
        If message <> String.Empty Then
            MessageBox.Show(message)
        End If
    End Sub

    Public Sub NotifyUpdate(objData As Object) Implements IAideServiceCallback.NotifyUpdate

    End Sub

    Public Sub getTime()
        Dim timers As DispatcherTimer = New DispatcherTimer
        timers.Interval = TimeSpan.FromSeconds(1)

        dispatcherTimer_Tick()
        timers.Start()
    End Sub

    Private Sub dispatcherTimer_Tick()
        TimeTxt.Text = Date.Now.ToShortTimeString
        DateTxt.Text = Date.Now.ToLongDateString
    End Sub

    Private Sub ExitBtn_Click(sender As Object, e As RoutedEventArgs)
        If MsgBox("Are you sure to quit?", vbInformation + MsgBoxStyle.YesNo, "Quit") = vbYes Then
            Environment.Exit(0)
        End If
    End Sub

    Private Sub EmployeesBtn_Click(sender As Object, e As RoutedEventArgs)
        PagesFrame.Navigate(New ContactListPage(PagesFrame, email))
        SubMenuFrame.Navigate(New BlankSubMenu())
    End Sub

    Private Sub SkillsBtn_Click(sender As Object, e As RoutedEventArgs)
        empID = profile.Emp_ID
        departmentID = profile.Dept_ID

        If profile.Permission = 1 Then
            PagesFrame.Navigate(New SkillsMatrixManagerPage(empID, departmentID))
        Else
            PagesFrame.Navigate(New SkillsMatrixPage(empID))
        End If
        'PagesFrame.Navigate(New ContactListPage(PagesFrame, email))
        SubMenuFrame.Navigate(New BlankSubMenu())
    End Sub

    Private Sub ProjectBtn_Click(sender As Object, e As RoutedEventArgs)
        PagesFrame.Navigate(New CreateProjectPage(PagesFrame))
        SubMenuFrame.Navigate(New ProjectSubMenuPage(PagesFrame))

        'PagesFrame.Navigate(New ViewProjectUI(PagesFrame))
        'SubMenuFrame.Navigate(New BlankSubMenu())
    End Sub

    Private Sub TaskBtn_Click(sender As Object, e As RoutedEventArgs)
        If IsManagerSignedOn Then
            PagesFrame.Navigate(New TaskAdminPage(PagesFrame, Me))
        Else
            PagesFrame.Navigate(New TaskPage(PagesFrame, Me))
        End If
    End Sub

    Private Sub AttendanceBtn_Click(sender As Object, e As RoutedEventArgs)
        permission = profile.Permission
        empID = profile.Emp_ID
        departmentID = profile.Dept_ID

        PagesFrame.Navigate(New ResourcePlannerPage(empID, departmentID, permission))
    End Sub

    Private Sub BirthdayBtn_Click(sender As Object, e As RoutedEventArgs)
        PagesFrame.Navigate(New BirthdayPage(PagesFrame, email))
    End Sub
End Class
