Imports UI_AIDE_CommCellServices.ServiceReference1
Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports System.ServiceModel

''' <summary>
''' By Christian Lois Valondo
''' </summary>
''' <remarks></remarks>
<CallbackBehavior(ConcurrencyMode:=ConcurrencyMode.Single, UseSynchronizationContext:=False)>
Class NewContactList
    Implements ServiceReference1.IAideServiceCallback
#Region "Fields"

    Private mainFrame As Frame
    Private client As ServiceReference1.AideServiceClient
    Private contacts As New ContactListModel
    Private email As String

#End Region

#Region "Constructor"

    Public Sub New(_contacts As ContactListModel, mainFrame As Frame, _email As String)

        InitializeComponent()

        Me.mainFrame = mainFrame
        Me.email = _email
        Me.contacts = _contacts
        Me.DataContext = contacts
        AssignEvents()
    End Sub

#End Region

#Region "Events"

    Private Sub btnCUpdate_Click(sender As Object, e As RoutedEventArgs) Handles btnCUpdate.Click
        Try
            e.Handled = True
            Dim contactList As New ContactList
            If txtCCelNo.Text = String.Empty AndAlso txtCLocation.Text = String.Empty Then
                MessageBox.Show("Please Fill up the form")
            Else
                contactList.EmpID = txtCEmpID.Text
                contactList.EMADDRESS = txtCEmail.Text
                contactList.EMADDRESS2 = txtCEmail2.Text
                contactList.LOC = txtCLocation.Text
                contactList.lOCAL = txtCLocal.Text
                contactList.CELL_NO = txtCCelNo.Text
                contactList.HOUSEPHONE = txtCHome.Text
                contactList.OTHERPHONE = txtCOther.Text
                contactList.DateReviewed = DateTime.Now.Date

                Dim result As Integer = MessageBox.Show("Are you sure you want to continue?", "Warning", MessageBoxButton.OKCancel)
                If result = 1 Then
                    If InitializeService() Then
                        client.UpdateContactListByEmpID(contactList)
                        ClearFields()
                        mainFrame.Navigate(New ContactListPage(mainFrame, email))
                    End If
                Else
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Failed")
        End Try
    End Sub

    Private Sub btnCCancel_Click(sender As Object, e As RoutedEventArgs) Handles btnCCancel.Click
        mainFrame.Navigate(New ContactListPage(mainFrame, email))
    End Sub

#End Region

#Region "Functions"

    Private Sub AssignEvents()
        AddHandler btnCCancel.Click, AddressOf btnCCancel_Click
        AddHandler btnCUpdate.Click, AddressOf btnCUpdate_Click
    End Sub 'Assign events to buttons

    Private Sub ClearFields()
        txtCEmpID.Clear()
        txtCCelNo.Clear()
        txtCEmail.Clear()
        txtCEmail2.Clear()
        txtCHome.Clear()
        txtCLocal.Clear()
        txtCLocation.Clear()
    End Sub

    Private Sub LoadData()
        txtCCelNo.Text = contacts.CEL_NO
        txtCEmail.Text = contacts.EMAIL_ADDRESS
        txtCEmail2.Text = contacts.EMAIL_ADDRESS2
        txtCHome.Text = contacts.HOMEPHONE
        txtCOther.Text = contacts.OTHER_PHONE
        txtCEmpID.Text = contacts.EMP_ID
        txtCLocal.Text = contacts.LOCAL
        txtCLocation.Text = contacts.LOCATION
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
