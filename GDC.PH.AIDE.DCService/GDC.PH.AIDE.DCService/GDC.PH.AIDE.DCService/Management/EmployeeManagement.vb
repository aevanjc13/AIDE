﻿Imports GDC.PH.AIDE.DCService
Imports GDC.PH.AIDE.Entity
Public Class EmployeeManagement
    Inherits ManagementBase

    Private m_stateData As StateData

    Public Sub New()
        m_stateData = New StateData()
    End Sub
    Public Overrides Sub SetFields(ByRef objResult As Object, objData As Object)
        Dim objEmployee As Employee = DirectCast(objData, Employee)
        Dim employeeData As New EmployeeSet
        employeeData.GroupID = objEmployee.GroupID
        employeeData.BirthDate = objEmployee.Birthdate
        employeeData.DateHired = objEmployee.DateHired
        employeeData.EmployeeID = objEmployee.EmployeeID
        employeeData.FirstName = objEmployee.FirstName
        employeeData.ImagePath = objEmployee.LastName
        employeeData.MiddleInitial = objEmployee.MiddleInitial
        employeeData.NickName = objEmployee.Nickname
        employeeData.Position = objEmployee.Position
        employeeData.Status = objEmployee.Status
        objResult = employeeData
    End Sub

    Public Overrides Function GetExceptionMessage(ex As Exception) As String
        Dim message As String = ex.Message
        Return message
    End Function

    Public Overrides Function GetMappedFields(objData As Object) As Object
        Dim objEmployee As EmployeeSet = DirectCast(objData, EmployeeSet)
        Dim employeeData As New Employee
        employeeData.GroupID = objEmployee.GroupID
        employeeData.Birthdate = objEmployee.BirthDate
        employeeData.DateHired = objEmployee.DateHired
        employeeData.EmployeeID = objEmployee.EmployeeID
        employeeData.FirstName = objEmployee.FirstName
        employeeData.ImagePath = objEmployee.ImagePath
        employeeData.LastName = objEmployee.LastName
        employeeData.MiddleInitial = objEmployee.MiddleInitial
        employeeData.Nickname = objEmployee.NickName
        employeeData.Position = objEmployee.Position
        employeeData.Status = objEmployee.Status
        employeeData.EmailAddress = objEmployee.EmailAddress
        employeeData.EmailAddress2 = objEmployee.EmailAddress2
        employeeData.Local = objEmployee.Local
        employeeData.CellNumber = objEmployee.CelNo
        Return employeeData
    End Function

    Public Overrides Function GetStateData(status As NotifyType, Optional data As Object = Nothing, Optional message As String = "") As StateData
        Dim state As New StateData
        state.Data = data
        state.Message = message
        state.NotifyType = status
        Return state
    End Function

    Public Function GetEmployeeList() As StateData
        Dim empSet As New EmployeeSet
        Dim lstEmployee As List(Of EmployeeSet)
        Dim objEmployees As New List(Of Employee)
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType

        Try

            lstEmployee = empSet.GetEmployeeList()

            If Not IsNothing(lstEmployee) Then
                For Each objEmployee As EmployeeSet In lstEmployee
                    objEmployees.Add(DirectCast(GetMappedFields(objEmployee), Employee))
                Next

                status = NotifyType.IsSuccess
            End If

        Catch ex As Exception
            status = NotifyType.IsError
            message = GetExceptionMessage(ex)
        End Try
        state = GetStateData(status, objEmployees, message)
        Return state
    End Function

    Public Function GetEmployee(ByVal empId As Integer) As StateData
        Dim empSet As New EmployeeSet
        Dim employeeData As Employee = Nothing
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType

        Try

            Dim objEmployee As EmployeeSet = empSet.GetEmployee(empId)

            If Not IsNothing(objEmployee) Then
                employeeData = DirectCast(GetMappedFields(objEmployee), Employee)
                status = NotifyType.IsSuccess
            End If

        Catch ex As Exception
            status = NotifyType.IsError
            message = GetExceptionMessage(ex)
        End Try
        state = GetStateData(status, employeeData, message)
        Return state
    End Function
    Public Function UpdateEmployee(ByVal employeeData As Employee) As StateData
        Dim empSet As New EmployeeSet
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType
        Try
            SetFields(empSet, employeeData)
            If empSet.UpdateEmployee() Then
                status = NotifyType.IsSuccess
            End If
        Catch ex As Exception
            status = NotifyType.IsError
        End Try
        state = GetStateData(status)
        Return state
    End Function
End Class
