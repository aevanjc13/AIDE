﻿
Imports GDC.PH.AIDE.BusinessLayer
Imports GDC.PH.AIDE.DCService
Imports GDC.PH.AIDE.Entity
''' <summary>
''' BY HYACINTH ARMARLES AND GIANN CARLO CAMILO
''' </summary>
''' <remarks></remarks>
Public Class ProjectManagement
    Inherits ManagementBase


    ''' <summary>
    ''' HYACINCTH AMARLES
    ''' </summary>
    ''' <param name="objProject"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateNewProject(ByVal objProject As Project) As StateData
        Dim projSet As New ProjectSet
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType
        Try
            Me.InsertNewProject(objProject)
            'For Each _assigned As AssignedProject In lstAssigned
            '    Me.InsertAssignedProject(_assigned)
            'Next
        Catch ex As Exception
            status = NotifyType.IsError
            message = GetExceptionMessage(ex)
        End Try
        state = GetStateData(status)
        Return state
    End Function
    ''' <summary>
    ''' HYACINCTH AMARLES
    ''' </summary>
    ''' <param name="objProject"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InsertNewProject(ByVal objProject As Project) As StateData
        Dim projSet As New ProjectSet
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType
        Try
            SetFields(projSet, objProject)
            If projSet.InsertNewProject() Then
                status = NotifyType.IsSuccess
            End If
        Catch ex As Exception
            status = NotifyType.IsError
        End Try
        state = GetStateData(status)
        Return state
    End Function
    ''' <summary>
    ''' GIANN CARLO CAMILO
    ''' </summary>
    ''' <param name="objProject"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InsertAssignedProject(ByVal objProject As AssignedProject) As StateData
        Dim projSet As New AssignedProjectSet
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType
        Try
            SetAssignedFields(projSet, objProject) 'ERROR: Unable to cast object of type 'GDC.WeServ.EPAD.DCService.AssignedProject' to type 'GDC.WeServ.EPAD.DCService.Project'.
            If projSet.InsertAssignedProj() Then
                status = NotifyType.IsSuccess
            End If
        Catch ex As Exception
            status = NotifyType.IsError
        End Try
        state = GetStateData(status)
        Return state
    End Function

   Public Function UpdateProject(ByVal objProject As Project) As StateData
        Dim projSet As New ProjectSet
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType
        Try
            SetFields(projSet, objProject)
            If projSet.UpdateProject() Then
                status = NotifyType.IsSuccess
            End If
        Catch ex As Exception
            status = NotifyType.IsError
        End Try
        state = GetStateData(status)
        Return state
    End Function

    ''' <summary>
    ''' VIEW PROJECT WITH EMPLOYEES ---- GIANN CARLO CAMILO
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ViewProject() As StateData
        Dim projSet As New ProjectSet
        Dim lstProject As List(Of ProjectSet)
        Dim objProjects As New List(Of ViewProject)
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType

        Try

            lstProject = projSet.ViewProject()

            If Not IsNothing(lstProject) Then
                For Each objProject As ProjectSet In lstProject
                    objProjects.Add(DirectCast(GetMappedFields(objProject), ViewProject))
                Next

                status = NotifyType.IsSuccess
            End If

        Catch ex As Exception
            status = NotifyType.IsError
            message = GetExceptionMessage(ex)
        End Try
        state = GetStateData(status, objProjects, message)
        Return state
    End Function

    ' Get this Function
    Public Function GetProjectLists()
        Dim projectSet As New ProjectSet
        Dim lstProject As List(Of ProjectSet)
        Dim objProject As New List(Of Project)
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType

        Try
            lstProject = projectSet.GetProjectLists()

            If Not IsNothing(lstProject) Then
                For Each objList As ProjectSet In lstProject
                    objProject.Add(DirectCast(GetMappedFields(objList), Project))
                Next
                status = NotifyType.IsSuccess
            End If

        Catch ex As Exception
            status = NotifyType.IsError
            message = GetExceptionMessage(ex)
        End Try
        state = GetStateData(status, objProject, message)
        Return state
    End Function

    Public Function GetProjectByID(ByVal ProjId As Integer) As StateData
        Dim projectset As New ProjectSet
        Dim objprofile As Project = Nothing
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType
        Try
            projectset = projectset.GetProjectByID(ProjId)
            If Not IsNothing(projectset) Then
                objprofile = DirectCast(GetMappedFields(projectset), Project)
                status = NotifyType.IsSuccess
            End If
        Catch ex As Exception
            status = NotifyType.IsError
            message = GetExceptionMessage(ex)
        End Try
        state = GetStateData(status, objprofile, message)
        Return state
    End Function
    ''' <summary>
    ''' GIANN CARLO CAMILO
    ''' </summary>
    ''' <param name="lstAssigned"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateNewAssignedProject(ByVal lstAssigned As List(Of AssignedProject)) As StateData
        Dim projSet As New ProjectSet
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType
        Try

            For Each _assigned As AssignedProject In lstAssigned
                Me.InsertAssignedProject(_assigned)
            Next
        Catch ex As Exception
            status = NotifyType.IsError
            message = GetExceptionMessage(ex)
        End Try
        state = GetStateData(status)
        Return state
    End Function

     ''' <summary>
    ''' VIEW PROJECT WITH EMPLOYEES - GIANN CARLO CAMILO
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ViewProjectListofEmployee() As StateData
        Dim projSet As New ProjectSet
        Dim lstProject As List(Of ProjectSet)
        Dim objProjects As New List(Of ViewProject)
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType

        Try

            lstProject = projSet.ViewProjectListofEmployee()

            If Not IsNothing(lstProject) Then
                For Each objProject As ProjectSet In lstProject
                    objProjects.Add(DirectCast(GetMappedFieldsProject(objProject), ViewProject))
                Next

                status = NotifyType.IsSuccess
            End If

        Catch ex As Exception
            status = NotifyType.IsError
            message = GetExceptionMessage(ex)
        End Try
        state = GetStateData(status, objProjects, message)
        Return state
    End Function



    Public Overrides Function GetStateData(status As NotifyType, Optional data As Object = Nothing, Optional message As String = "") As StateData
        Dim state As New StateData
        state.Data = data
        state.Message = message
        state.NotifyType = status
        Return state
    End Function

    Public Overrides Function GetMappedFields(objData As Object) As Object
        Dim objProject As ProjectSet = DirectCast(objData, ProjectSet)
        Dim viewProjectData As New ViewProject
        Dim projectData As New Project

        projectData.ProjectID = objProject.ProjectId
        projectData.ProjectName = objProject.ProjectName
        projectData.Category = objProject.Category
        projectData.Billability = objProject.Billability

        viewProjectData.Status = objProject.Status
        viewProjectData.Name = objProject.Name
        viewProjectData.Month1 = objProject.FirstMonth
        viewProjectData.Month2 = objProject.SecondMonth
        viewProjectData.Month3 = objProject.ThirdMonth

        If Not IsNothing(viewProjectData) Then
            Return projectData
        Else
            Return viewProjectData
        End If

    End Function


    ''' <summary>
    ''' GIANN CARLO CAMILO
    ''' </summary>
    ''' <param name="objData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMappedFieldsProject(objData As Object) As Object
        Dim objProject As ProjectSet = DirectCast(objData, ProjectSet)
        Dim viewProjectData As New ViewProject

        viewProjectData.Status = objProject.Status
        viewProjectData.Name = objProject.Name
        viewProjectData.Month1 = objProject.FirstMonth
        viewProjectData.Month2 = objProject.SecondMonth
        viewProjectData.Month3 = objProject.ThirdMonth
        Return viewProjectData
    End Function

    Public Overrides Function GetExceptionMessage(ex As Exception) As String
        Dim message As String = ex.Message
        Return message
    End Function

    Public Overrides Sub SetFields(ByRef objResult As Object, objData As Object)
        Dim objProject As Project = DirectCast(objData, Project)
        Dim projectData As New ProjectSet
        projectData.ProjectId = objProject.ProjectID
        projectData.ProjectName = objProject.ProjectName
        projectData.Billability = objProject.Billability
        projectData.Category = objProject.Category
        objResult = projectData
    End Sub
    ''' <summary>
    ''' GIANN CARLO CAMILO
    ''' </summary>
    ''' <param name="objResult"></param>
    ''' <param name="objData"></param>
    ''' <remarks></remarks>
    Public Sub SetAssignedFields(ByRef objResult As Object, objData As Object)
        Dim objProject As AssignedProject = DirectCast(objData, AssignedProject)
        Dim projectData As New AssignedProjectSet
        projectData.ProjectID = objProject.ProjectID
        projectData.EmpID = objProject.EmployeeID
        projectData.DateCreated = objProject.DateCreated
        projectData.StartPeriod = objProject.StartPeriod
        projectData.EndPeriod = objProject.EndPeriod
        objResult = projectData
    End Sub
End Class
