Imports UI_AIDE_CommCellServices.ServiceReference1
Imports System.Reflection
Imports System.IO
Imports System.Diagnostics
Imports System.ServiceModel
Imports System.Collections.ObjectModel

'''''''''''''''''''''''''''''''''
'   AEVAN CAMILLE BATONGBACAL   '
'''''''''''''''''''''''''''''''''
<CallbackBehavior(ConcurrencyMode:=ConcurrencyMode.Single, UseSynchronizationContext:=False)>
Class BirthdayPage
    Implements ServiceReference1.IAideServiceCallback

#Region "Fields"

    Private _AideService As ServiceReference1.AideServiceClient
    Private mainFrame As Frame
    Private isEmpty As Boolean
    Private email As String
    Dim lstBirthday As BirthdayList()
    Dim lstBirthdayMonth As BirthdayList()
    Dim birthdayListVM As New BirthdayListViewModel()

    Private Enum PagingMode
        _First = 1
        _Next = 2
        _Previous = 3
        _Last = 4
    End Enum

#End Region

#Region "Constructor"

    Public Sub New(mainFrame As Frame, _email As String)

        InitializeComponent()
        Me.email = _email
        Me.mainFrame = mainFrame
        SetData()
        Me.DataContext = birthdayListVM
    End Sub

#End Region

#Region "Events"

#End Region

#Region "Functions"

    Public Sub SetData()
        Try
            If InitializeService() Then
                lstBirthday = _AideService.ViewBirthdayListAll(email)
                lstBirthdayMonth = _AideService.ViewBirthdayListByCurrentMonth(email)
                LoadData()
                LoadDataMonthly()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub LoadData()
        Try
            Dim lstBirthdayList As New ObservableCollection(Of BirthdayListModel)
            Dim birthdayListDBProvider As New BirthdayListDBProvider

            For Each objBirthday As BirthdayList In lstBirthday
                birthdayListDBProvider.SetMyBirthdayList(objBirthday)
            Next

            For Each rawUser As MyBirthdayList In birthdayListDBProvider.GetMyBirthdayList()
                lstBirthdayList.Add(New BirthdayListModel(rawUser))
            Next

            birthdayListVM.BirthdayList = lstBirthdayList
            Me.DataContext = birthdayListVM
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "FAILED")
        End Try
    End Sub

    Public Sub LoadDataMonthly()
        Try
            Dim lstBirthdayList As New ObservableCollection(Of BirthdayListModel)
            Dim birthdayListDBProvider As New BirthdayListDBProvider

            For Each objBirthday As BirthdayList In lstBirthdayMonth
                birthdayListDBProvider.SetMyBirthdayListMonth(objBirthday)
            Next

            For Each rawUser As MyBirthdayList In birthdayListDBProvider.GetMyBirthdayListMonth()
                lstBirthdayList.Add(New BirthdayListModel(rawUser))
            Next

            birthdayListVM.BirthdayListMonth = lstBirthdayList
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "FAILED")
        End Try
    End Sub

    Public Function InitializeService() As Boolean
        Dim bInitialize As Boolean = False
        Try
            Dim Context As InstanceContext = New InstanceContext(Me)
            _AideService = New AideServiceClient(Context)
            _AideService.Open()
            bInitialize = True
        Catch ex As SystemException
            _AideService.Abort()
        End Try
        Return bInitialize
    End Function
#End Region

#Region "ICallBack Function"
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
#End Region


End Class
