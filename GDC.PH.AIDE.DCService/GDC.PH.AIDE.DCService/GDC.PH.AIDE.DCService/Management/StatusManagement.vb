
Imports GDC.PH.AIDE.BusinessLayer
Imports GDC.PH.AIDE.DCService
Imports GDC.PH.AIDE.Entity

Public Class StatusManagement
    Inherits ManagementBase


    Public Overrides Sub SetFields(ByRef objResult As Object, objData As Object)
        Dim objStatus As StatusGroup = DirectCast(objData, StatusGroup)
        Dim statusData As New StatusSet
        statusData.Group_ID = objStatus.GroupID
        statusData.Group_Name = objStatus.GroupName
        statusData.Description = objStatus.Description
        statusData.Status = objStatus.Status
        objResult = statusData
    End Sub

    Public Function getAttendanceStatus() As StateData
        Dim statSet As New StatusSet
        Dim lstStatus As New List(Of StatusSet)
        Dim lstStatusGroup As New List(Of StatusGroup)
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType
        Try

            lstStatus = statSet.getAttendanceStatus()
            If lstStatus.Count > 0 Then
                For Each statusItem As StatusSet In lstStatus
                    lstStatusGroup.Add(DirectCast(GetMappedFields(statusItem), StatusGroup))
                Next
                status = NotifyType.IsSuccess
            End If

        Catch ex As Exception
            status = NotifyType.IsError
            message = GetExceptionMessage(ex)
        End Try
        state = GetStateData(status, lstStatusGroup, message)
        Return state
    End Function

    Public Function getCivil() As StateData
        Dim statSet As New StatusSet
        Dim lstStatus As New List(Of StatusSet)
        Dim lstStatusGroup As New List(Of StatusGroup)
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType
        Try

            lstStatus = statSet.getCivil()
            If lstStatus.Count > 0 Then
                For Each statusItem As StatusSet In lstStatus
                    lstStatusGroup.Add(DirectCast(GetMappedFields(statusItem), StatusGroup))
                Next
                status = NotifyType.IsSuccess
            End If

        Catch ex As Exception
            status = NotifyType.IsError
            message = GetExceptionMessage(ex)
        End Try
        state = GetStateData(status, lstStatusGroup, message)
        Return state
    End Function

    Public Overrides Function GetExceptionMessage(ex As Exception) As String
        Dim message As String = ex.Message
        Return message
    End Function

    Public Overrides Function GetMappedFields(objData As Object) As Object
        Dim objStatus As StatusSet = DirectCast(objData, StatusSet)
        Dim statusData As New StatusGroup
        statusData.GroupID = objStatus.Group_ID
        statusData.GroupName = objStatus.Group_Name
        statusData.Description = objStatus.Description
        statusData.Status = objStatus.Status

        Return statusData
    End Function

    Public Overrides Function GetStateData(status As NotifyType, Optional data As Object = Nothing, Optional message As String = "") As StateData
        Dim state As New StateData
        state.Data = data
        state.Message = message
        state.NotifyType = status
        Return state
    End Function

    Public Function GetTaskStatus() As StateData
        Dim statSet As New StatusSet
        Dim lstStatus As New List(Of StatusSet)
        Dim lstStatusGroup As New List(Of StatusGroup)
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType
        Try
            lstStatus = statSet.GetTaskStatus()

            If Not IsNothing(lstStatus) Then
                For Each statusItem As StatusSet In lstStatus
                    lstStatusGroup.Add(DirectCast(GetMappedFields(statusItem), StatusGroup))
                Next
                status = NotifyType.IsSuccess
            End If

        Catch ex As Exception
            status = NotifyType.IsError
            message = GetExceptionMessage(ex)
        End Try
        state = GetStateData(status, lstStatusGroup, message)
        Return state
    End Function

    Public Function GetTaskCategories() As StateData
        Dim statSet As New StatusSet
        Dim lstStatus As New List(Of StatusSet)
        Dim lstStatusGroup As New List(Of StatusGroup)
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType
        Try

            lstStatus = statSet.GetTaskCategories()
            If Not IsNothing(lstStatus) Then
                For Each statusItem As StatusSet In lstStatus
                    lstStatusGroup.Add(DirectCast(GetMappedFields(statusItem), StatusGroup))
                Next
                status = NotifyType.IsSuccess
            End If

        Catch ex As Exception
            status = NotifyType.IsError
            message = GetExceptionMessage(ex)
        End Try
        state = GetStateData(status, lstStatusGroup, message)
        Return state
    End Function

    Public Function GetPhaseStatus() As StateData
        Dim statSet As New StatusSet
        Dim lstStatus As New List(Of StatusSet)
        Dim lstStatusGroup As New List(Of StatusGroup)
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType
        Try

            lstStatus = statSet.GetPhaseStatus()
            If Not IsNothing(lstStatus) Then
                For Each statusItem As StatusSet In lstStatus
                    lstStatusGroup.Add(DirectCast(GetMappedFields(statusItem), StatusGroup))
                Next
                status = NotifyType.IsSuccess
            End If

        Catch ex As Exception
            status = NotifyType.IsError
            message = GetExceptionMessage(ex)
        End Try
        state = GetStateData(status, lstStatusGroup, message)
        Return state
    End Function

    Public Function GetReworkStatus() As StateData
        Dim statSet As New StatusSet
        Dim lstStatus As New List(Of StatusSet)
        Dim lstStatusGroup As New List(Of StatusGroup)
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType
        Try

            lstStatus = statSet.GetReworkStatus()
            If Not IsNothing(lstStatus) Then
                For Each statusItem As StatusSet In lstStatus
                    lstStatusGroup.Add(DirectCast(GetMappedFields(statusItem), StatusGroup))
                Next
                status = NotifyType.IsSuccess
            End If

        Catch ex As Exception
            status = NotifyType.IsError
            message = GetExceptionMessage(ex)
        End Try
        state = GetStateData(status, lstStatusGroup, message)
        Return state
    End Function

    Public Function getGenericStatus(ByVal GroupID As Short) As StateData
        Dim statSet As New StatusSet
        Dim lstStatus As New List(Of StatusSet)
        Dim lstStatusGroup As New List(Of StatusGroup)
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType
        Try

            lstStatus = statSet.GetListOfStatus(GroupID)
            If lstStatus.Count > 0 Then
                For Each statusItem As StatusSet In lstStatus
                    lstStatusGroup.Add(DirectCast(GetMappedFields(statusItem), StatusGroup))
                Next
                status = NotifyType.IsSuccess
            End If

        Catch ex As Exception
            status = NotifyType.IsError
            message = GetExceptionMessage(ex)
        End Try
        state = GetStateData(status, lstStatusGroup, message)
        Return state
    End Function
End Class
