﻿' NOTE: You can use the "Rename" command on the context menu to change the class name "Service1" in code, svc and config file together.
' NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.vb at the Solution Explorer and start debugging.
Imports GDC.PH.AIDE.DCService
Imports GDC.PH.AIDE.Entity
Imports System.ServiceModel.PeerResolvers

Public Enum Grouping As Short
    Attendance
    CivilStatus
    TaskCategory
    TaskStatus
    Phase
    Rework
End Enum

<ServiceBehavior(ConcurrencyMode:=ConcurrencyMode.Single, InstanceContextMode:=InstanceContextMode.PerCall)>
Public Class AIDEService
    Inherits MainService
    Implements IAideService
    Implements IAideService2



    Private Shared _callbackList As New List(Of IAIDEServiceCallback)()
    '  number of current users - 0 to begin with
    Private Shared _registeredUsers As Integer = 0
    Private Shared _DashboardManagement As DashboardManagement

    Public Sub New()
        _DashboardManagement = New DashboardManagement()
    End Sub

    Protected Overrides Sub OnReceivedResponse(e As ResponseReceivedEventArgs)
        MyBase.OnReceivedResponse(e)
        Dim state As StateData = DirectCast(e.DataReceived, StateData)
        If (state.NotifyType = NotifyType.IsSuccess) Then
            NotifySuccess(state.Message)
        Else
            NotifyError(state.Message)
        End If
    End Sub

    Public Sub NotifyError(ByVal errorMessage As String)
        'Dim registeredUser As IAIDEServiceCallback = OperationContext.Current.GetCallbackChannel(Of IAIDEServiceCallback)()
        'If Not _callbackList.Contains(registeredUser) Then
        '    _callbackList.Add(registeredUser)
        '    _callbackList(_registeredUsers).NotifyError(errorMessage)
        'Else
        '    registeredUser.NotifyError(errorMessage)
        'End If
    End Sub

    Public Sub NotifySuccess(ByVal message As String)
        'Dim registeredUser As IAIDEServiceCallback = OperationContext.Current.GetCallbackChannel(Of IAIDEServiceCallback)()
        'If Not _callbackList.Contains(registeredUser) Then
        '    _callbackList.Add(registeredUser)
        '    _callbackList(_registeredUsers).NotifySuccess(message)
        'Else
        '    registeredUser.NotifySuccess(message)
        'End If
    End Sub

    Public Sub InsertAttendance(ByVal _Attendance As AttendanceSummary) Implements IAideService.InsertAttendance
        MyBase.InsertAttendanceByEmp(_Attendance)
    End Sub

    Public Function GetMyAttendance(EmpId As Integer, WeekOf As Date) As MyAttendance Implements IAideService.GetMyAttendance
        Dim objAttendance As MyAttendance = Nothing
        MyBase.GetAttendanceEmployee(EmpId, WeekOf, objAttendance)
        Return objAttendance
    End Function

    Public Function GetAttendanceSummary(ByVal Month As Integer, ByVal Year As Integer) As List(Of AttendanceSummary) Implements IAideService.GetAttendanceSummary
        Dim lstAttendance As List(Of AttendanceSummary) = Nothing
        MyBase.GetAttendanceAll(Month, Year, lstAttendance)
        Return lstAttendance
    End Function

    Public Sub UpdateAttendance(ByVal _Attendance As AttendanceSummary) Implements IAideService.UpdateAttendance
        MyBase.UpdateAttendanceByEmp(_Attendance)
    End Sub

    Public Function GetNonBillabilityHoursByEmpID(EmpID As Integer, userChoice As Short) As NonBillableHours Implements IAideService.GetNonBillabilityHoursByEmpID
        Throw New NotImplementedException()
    End Function

    Public Function GetNonBillabilityHoursAll(userChoice As Short) As List(Of NonBillableHours) Implements IAideService.GetNonBillabilityHoursAll
        Throw New NotImplementedException()
    End Function

    Public Function GetNonBillabilityHoursSummary(inputDate As Date) As List(Of NonBillableSummary) Implements IAideService.GetNonBillabilityHoursSummary
        Dim lstNonBillabilityHoursAll As List(Of NonBillableSummary) = Nothing
        MyBase.getNonBillableHoursAllList(inputDate, lstNonBillabilityHoursAll)
        Return lstNonBillabilityHoursAll
    End Function

    Public Function GetContactInformation(Emp_ID As Integer) As Contact Implements IAideService.GetContactInformation
        Throw New NotImplementedException()
    End Function

    Public Function GetContactList() As List(Of Contact) Implements IAideService.GetContactList
        Throw New NotImplementedException()
    End Function

    Public Sub UpdateContactInformation(contact As Contact) Implements IAideService.UpdateContactInformation
        Throw New NotImplementedException()
    End Sub

   
    ''' <summary>
    ''' GIANN CARLO CAMILO
    ''' </summary>
    ''' <param name="empId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProfileInformation(empId As Integer) As Profile Implements IAideService.GetProfileInformation
        Dim objProfile As Profile = Nothing
        MyBase.GetProfile(empId, objProfile)
        Return objProfile
    End Function


    ''' <summary>
    ''' GIANN CARLO CAMILO
    ''' </summary>
    ''' <param name="emailAddress"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SignOn(emailAddress As String) As Profile Implements IAideService.SignOn
        Dim objProfile As Profile = Nothing
        Dim objResult As Object = Nothing
        Dim userName As String = ""
        Try
            Dim registeredUser As IAIDEServiceCallback = OperationContext.Current.GetCallbackChannel(Of IAIDEServiceCallback)()

            If Not _callbackList.Contains(registeredUser) Then
                _callbackList.Add(registeredUser)
            End If



            If MyBase.GetMyProfile(emailAddress, objResult) Then
                objProfile = DirectCast(objResult, Profile)

                'MyBase.UpdateAttendanceByEmp(objProfile.Emp_ID, Date.Now.Day, 0)
                _callbackList.ForEach(Sub(callback As IAIDEServiceCallback)
                                          callback.NotifyPresent(objProfile.FirstName & " " & objProfile.LastName)
                                          _registeredUsers += 1

                                      End Sub)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Return objProfile
    End Function

    ''' <summary>
    ''' GIANN CARLO CAMILO - NOT YET IMPLEMENTED
    ''' </summary>
    ''' <param name="EmployeeName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SignOff(EmployeeName As String) As Integer Implements IAideService.SignOff
        Try
            Dim registeredUser As IAIDEServiceCallback = OperationContext.Current.GetCallbackChannel(Of IAIDEServiceCallback)()
            Dim userName As String = ""
            If _callbackList.Contains(registeredUser) Then
                _callbackList.Remove(registeredUser)
                _registeredUsers -= 1
            End If
            _callbackList.ForEach(Sub(callback As IAIDEServiceCallback)
                                      callback.NotifyOffline(EmployeeName)
                                  End Sub)
        Catch ex As Exception
        End Try
        Return _registeredUsers
    End Function

  

    Public Function DashboardGetEmployeeList() As List(Of DashboardEmployee) Implements IAideService2.DashboardGetEmployeeList
        Return _DashboardManagement.DashbrdGetEmployeeList()
    End Function

    Public Function DashboardGetContactList() As List(Of DashboardContact) Implements IAideService2.DashboardGetContactList
        Return _DashboardManagement.DashbrdGetContactList()
    End Function

    Public Function DashboardGetNonBillableHours() As List(Of DashboardNonBillableHours) Implements IAideService2.DashboardGetNonBillableHours
        Return _DashboardManagement.DashbrdGetNonBillableHours()
    End Function

    Public Function DashboardGetNonBillableHoursSummary() As List(Of DashboardNonBillableHoursSummary) Implements IAideService2.DashboardGetNonBillableHoursSummary
        Return _DashboardManagement.DashbrdGetNonBillableSummary()
    End Function

    Public Function DashboardGetTeamAttendance() As List(Of DashboardTeamAttendance) Implements IAideService2.DashboardGetTeamAttendance
        Return _DashboardManagement.DashbrdGetTeamAttendance()
    End Function

    Public Function DashboardGetResourcePlanner() As List(Of AttendanceSummary) Implements IAideService2.DashboardGetResourcePlanner
        Return Nothing
    End Function

    Public Function DashboardGetTaskSummary(dateStart As Date) As List(Of TaskSummary) Implements IAideService2.DashboardGetTaskSummary
        Return _DashboardManagement.DashbrdGetTaskSummary(dateStart)
    End Function

    Public Function DashboardGetTaskSummaryTotals(dateStart As Date) As List(Of DashboardTaskSummaryTotals) Implements IAideService2.DashboardGetTaskSummaryTotals
        Return _DashboardManagement.DashbrdGetTaskSummaryTotals(dateStart)
    End Function

   

    Public Sub UpdateAttendance2(empid As Integer, day As Integer, attstatus As Integer) Implements IAideService.UpdateAttendance2
        MyBase.UpdateAttendanceByEmp(empid, day, attstatus)
    End Sub

   

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    ''' <summary>
    ''' By Jester Sanchez
    ''' </summary>

#Region "Action List"

    Public Sub InsertActionList(_Action As Action) Implements IAideService.InsertActionList
        MyBase.InsertActionLst(_Action)
    End Sub

    Public Sub UpdateActionList(_Action As Action) Implements IAideService.UpdateActionList
        MyBase.UpdateActionLst(_Action)
    End Sub

    Public Function GetActionListByMessage(_Message As String) As List(Of Action) Implements IAideService.GetActionListByMessage
        Dim Actionlist As List(Of Action) = Nothing
        MyBase.GetActionLstByMessage(_Message, Actionlist)
        Return Actionlist
    End Function

    Public Function GetActionSummary() As List(Of Action) Implements IAideService.GetActionSummary
        Dim Actionlist As List(Of Action) = Nothing
        MyBase.GetActionSummry(Actionlist)
        Return Actionlist
    End Function

#End Region

    ''' <summary>
    ''' By John Harvey Sanchez
    ''' </summary>
#Region "Lesson Learnt"

    Public Function GetLessonLearntByProblem(search As String) As List(Of LessonLearnt) Implements IAideService.GetLessonLearntByProblem
        Dim lstLessonLearnt As List(Of LessonLearnt) = Nothing
        MyBase.GetLessonLearntByProblems(search, lstLessonLearnt)
        Return lstLessonLearnt
    End Function

    Public Function GetLessonLearntList() As List(Of LessonLearnt) Implements IAideService.GetLessonLearntList
        Dim lstLessonLearnt As List(Of LessonLearnt) = Nothing
        MyBase.GetAllLessonLearntList(lstLessonLearnt)
        Return lstLessonLearnt
    End Function

    Public Sub CreateLessonLearnt(lessonLearnt As LessonLearnt) Implements IAideService.CreateLessonLearnt
        MyBase.CreateNewLessonLearnt(lessonLearnt)
    End Sub

    Public Sub UpdateLessonLearntInfo(lesson As LessonLearnt) Implements IAideService.UpdateLessonLearntInfo
        MyBase.UpdateLessonLearnt(lesson)
    End Sub

#End Region

    ''' <summary>
    ''' By Aevan Camille Batongbacal
    ''' </summary>
#Region "Success Register"

    Public Function ViewSuccessRegisterAll(email As String) As List(Of SuccessRegister) Implements IAideService.ViewSuccessRegisterAll
        Return MyBase.GetSuccessRegisterAll(email)
    End Function

    Public Function ViewSuccessRegisterByEmpID(email As String) As List(Of SuccessRegister) Implements IAideService.ViewSuccessRegisterByEmpID
        Return MyBase.GetSuccessRegisterByEmpID(email)
    End Function

    Public Function ViewSuccessRegisterBySearch(input As String, email As String) As List(Of SuccessRegister) Implements IAideService.ViewSuccessRegisterBySearch
        Return MyBase.GetSuccessRegisterBySearch(input, email)
    End Function

    Public Sub UpdateSuccessRegisterByEmpID(success As SuccessRegister) Implements IAideService.UpdateSuccessRegisterByEmpID
        MyBase.UpdateSuccessRegister(success)
    End Sub

    Public Sub CreateNewSuccessRegister(success As SuccessRegister) Implements IAideService.CreateNewSuccessRegister
        MyBase.CreateSuccessRegister(success)
    End Sub

    Public Function ViewNicknameByDeptID(email As String) As List(Of Nickname) Implements IAideService.ViewNicknameByDeptID
        Return MyBase.GetNicknameByDeptID(email)
    End Function

    Public Sub DeleteSuccessRegisterBySuccessID(successId As Integer) Implements IAideService.DeleteSuccessRegisterBySuccessID
        MyBase.DeleteSuccessRegister(successId)
    End Sub

#End Region

    ''' <summary>
    ''' By Aevan Camille Batongbacal
    ''' </summary>
#Region "Contact List"
    Public Function ViewContactListAll(ByVal email As String) As List(Of ContactList) Implements IAideService.ViewContactListAll
        Return MyBase.GetContactListAll(email)
    End Function

    Public Sub UpdateContactListByEmpID(ByVal contact As ContactList) Implements IAideService.UpdateContactListByEmpID
        MyBase.UpdateContactList(contact)
    End Sub

    Public Sub CreateNewContactByEmpID(ByVal contact As ContactList) Implements IAideService.CreateNewContactByEmpID
        MyBase.CreateContact(contact)
    End Sub
#End Region

    ''' <summary>
    ''' By Aevan Camille Batongbacal
    ''' </summary>
#Region "Birthday List"
    Public Function ViewBirthdayListAll(ByVal email As String) As List(Of BirthdayList) Implements IAideService.ViewBirthdayListAll
        Return MyBase.GetBirthdayListAll(email)
    End Function

    Public Function ViewBirthdayListByCurrentMonth(ByVal email As String) As List(Of BirthdayList) Implements IAideService.ViewBirthdayListByCurrentMonth
        Return MyBase.GetBirthdayListByMonth(email)
    End Function
#End Region

  
#Region "Concern"

    ''DISPLAY ALL CONCERN
    ''' <summary>
    ''' GIANN CARLO CAMILO/CHRISTIAN VALONDO
    ''' </summary>
    ''' <param name="email"></param>
    ''' <param name="offsetVal"></param>
    ''' <param name="nextVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SelectAllConcernLst(email As String, offsetVal As Integer, nextVal As Integer) As List(Of Concern) Implements IAideService.selectAllConcern
        Return MyBase.selectAllConcern(email, offsetVal, nextVal)
    End Function

    ''INSERT CONCERN

    ''DISPLAY ALL CONCERN
    ''' <summary>
    ''' ''' GIANN CARLO CAMILO/CHRISTIAN VALONDO
    ''' </summary>
    ''' <param name="concern"></param>
    ''' <param name="email"></param>
    ''' <remarks></remarks>
    Public Sub InsertIntoConcern(concern As Concern, email As String) Implements IAideService.InsertIntoConcern

        MyBase.InsertIntoConcerns(concern, email)
    End Sub

    ''GET GEMERATED REF NO 
    ''' <summary>
    ''' ''' GIANN CARLO CAMILO/CHRISTIAN VALONDO
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetGeneratedRefNos() As Concern Implements IAideService.GetGeneratedRefNo
        Dim objProfile As Concern = Nothing
        MyBase.GetGeneratedRefNo(objProfile)
        Return objProfile
    End Function

    ''SEARCH
    ''' <summary>
    ''' ''' GIANN CARLO CAMILO/CHRISTIAN VALONDO
    ''' </summary>
    ''' <param name="email"></param>
    ''' <param name="SearchKeyWord"></param>
    ''' <param name="offsetVal"></param>
    ''' <param name="nextVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetResultSearchs(email As String, SearchKeyWord As String, offsetVal As Integer, nextVal As Integer) As List(Of Concern) Implements IAideService.GetResultSearch
        Return MyBase.GetResultSearch(email, SearchKeyWord, offsetVal, nextVal)
    End Function


    ''DISPLAY TO COMBO BOX
    ''' <summary>
    ''' ''' GIANN CARLO CAMILO/CHRISTIAN VALONDO
    ''' </summary>
    ''' <param name="Ref_id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetListOfACtions(Ref_id As String) As List(Of Concern) Implements IAideService.GetListOfACtion
        Return MyBase.GetListOfACtion(Ref_id)
    End Function


    ''DISPLAY TO LIST BOX ACTION REFERENCES
    ''' <summary>
    ''' ''' GIANN CARLO CAMILO/CHRISTIAN VALONDO
    ''' </summary>
    ''' <param name="Ref_id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetListOfACtionsReferencesList(Ref_id As String) As List(Of Concern) Implements IAideService.GetListOfACtionsReferences
        Return MyBase.GetListOfACtionsReferences(Ref_id)
    End Function


    ''INSERT SELECTED ACTION
    ''' <summary>
    ''' ''' GIANN CARLO CAMILO/CHRISTIAN VALONDO
    ''' </summary>
    ''' <param name="concern"></param>
    ''' <remarks></remarks>
    Public Sub InsertAndDeleteSelectedActions(concern As Concern) Implements IAideService.insertAndDeleteSelectedAction

        MyBase.insertAndDeleteSelectedAction(concern)
    End Sub


    ''UPDATE SELECTED CONCERN
    ''' <summary>
    ''' ''' GIANN CARLO CAMILO/CHRISTIAN VALONDO
    ''' </summary>
    ''' <param name="concern"></param>
    ''' <remarks></remarks>
    Public Sub UpdateSelectedConcerns(concern As Concern) Implements IAideService.UpdateSelectedConcern

        MyBase.UpdateSelectedConcern(concern)
    End Sub


    ''DISPLAY ALL CONCERN BY BETWEEN DATE
    ''' <summary>
    ''' ''' GIANN CARLO CAMILO/CHRISTIAN VALONDO
    ''' </summary>
    ''' <param name="email"></param>
    ''' <param name="offsetVal"></param>
    ''' <param name="nextVal"></param>
    ''' <param name="date1"></param>
    ''' <param name="date2"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBetweenSearchConcerns(email As String, offsetVal As Integer, nextVal As Integer, date1 As Date, date2 As Date) As List(Of Concern) Implements IAideService.GetBetweenSearchConcern
        Return MyBase.GetBetweenSearchConcern(email, offsetVal, nextVal, date1, date2)
    End Function


    ''DISPLAY TO LISTVIEW LIST OF ACION VIA SEARCH
    ''' <summary>
    ''' ''' GIANN CARLO CAMILO/CHRISTIAN VALONDO
    ''' </summary>
    ''' <param name="_keywordAction"></param>
    ''' <param name="Ref_id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSearchActions(_keywordAction As String, Ref_id As String) As List(Of Concern) Implements IAideService.GetSearchAction
        Return MyBase.GetSearchAction(_keywordAction, Ref_id)
    End Function

#End Region

#Region "Skills"
    Public Overrides Function GetSkillsList() As List(Of Skills) Implements IAideService.GetSkillsList
        Return MyBase.GetAllSkillsList
    End Function

    Public Overrides Function ViewEmpSKills(ByVal deptID As Integer) As List(Of Skills) Implements IAideService.ViewEmpSKills
        Return MyBase.ViewAllEmpSkills(deptID)
    End Function

    Public Overrides Function GetSkillsProfByEmpID(ByVal id As Integer) As List(Of Skills) Implements IAideService.GetSkillsProfByEmpID
        Return MyBase.GetSkillsProfByEmpID(id)
    End Function

    Public Function GetProfLvlByEmpIDSkillIDs(empID As Integer, skillID As Integer) As Skills Implements IAideService.GetProfLvlByEmpIDSkillIDs
        Dim objSkills As Skills = Nothing
        MyBase.GetProfLvlByEmpIDSkillID(empID, skillID, objSkills)
        Return objSkills
    End Function

    Public Sub InsertNewSkill(skills As Skills) Implements IAideService.InsertNewSkills
        MyBase.InsertNewSkills(skills)
    End Sub

    Public Sub UpdateSkill(skills As Skills) Implements IAideService.UpdateSkills
        MyBase.UpdateSkills(skills)
    End Sub

    Public Function GetSkillsLastReviewByEmpIDSkillIDs(empID As Integer, skillID As Integer) As Skills Implements IAideService.GetSkillsLastReviewByEmpIDSkillID
        Dim objSkills As Skills = Nothing
        MyBase.GetSkillsLastReviewByEmpIDSkillID(empID, skillID, objSkills)
        Return objSkills
    End Function
#End Region

#Region "Project"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="project"></param>
    ''' <remarks></remarks>
    Public Sub CreateProject(project As Project) Implements IAideService.CreateProject
        MyBase.CreateNewProject(project)
    End Sub
    ''' <summary>
    ''' GIANN CARLO CAMILO/LEMUELA ABULENCIA
    ''' </summary>
    ''' <param name="assignProj"></param>
    ''' <remarks></remarks>
    Public Sub CreateAssignedProject(assignProj As List(Of AssignedProject)) Implements IAideService.CreateAssignedProject
        MyBase.CreateNewAssignedProject(assignProj)
    End Sub

    Public Sub AssignProject(Project As AssignedProject) Implements IAideService.AssignProject
        Throw New NotImplementedException()
    End Sub
    Public Sub UpdateProject(project As Project) Implements IAideService.UpdateProject
        MyBase.UpdateAssignedProject(project)
    End Sub

    Public Function GetProjectByID(ByVal projID As Integer) As Project Implements IAideService.GetProjectByID
        Dim objProject As Project = Nothing
        MyBase.GetProjectByProjID(projID, objProject)
        Return objProject
    End Function


 
    Public Function GetAllListOfProjects() As List(Of Project) Implements IAideService.GetAllListOfProject
        Return MyBase.GetAllListOfProject()
    End Function


    Public Function ViewProject() As List(Of ViewProject) Implements IAideService.ViewProject
        Dim lstViewProject As List(Of ViewProject) = Nothing
        MyBase.ViewProjectList(lstViewProject)
        Return lstViewProject
    End Function

    ''' <summary>
    ''' DISPLAY LIST PROJECT IN EVERYMONTH - GIANN CARLO CAMILO / LEMUELA ABULENCIA
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProjectList() As List(Of Project) Implements IAideService.GetProjectList
        Dim lstProject As List(Of Project) = Nothing
        MyBase.GetProjectsList(lstProject)
        Return lstProject
    End Function




    ''' <summary>
    ''' VIEW PROJECT  - GIANN CARLO CAMILO / LEMUELA ABULENCIA
    ''' </summary>
    ''' <remarks></remarks>
    Public Function ViewProjectListofEmployees() As List(Of ViewProject) Implements IAideService.ViewProjectListofEmployee
        Dim lstViewProject As List(Of ViewProject) = Nothing
        MyBase.ViewProjectListofEmployee(lstViewProject)
        Return lstViewProject
    End Function



#End Region

#Region "EmployeeList"
    ''' <summary>
    ''' GIANN CARLO CAMILO
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function GetEmployeeList() As List(Of Employee) Implements IAideService.GetEmployeeList
        Dim objEmployees As List(Of Employee) = Nothing
        MyBase.GetAllEmployees(objEmployees)
        Return objEmployees
    End Function

#End Region

#Region "Task"
    Public Sub CreateTask(task As Tasks) Implements IAideService.CreateTask
        MyBase.CreateNewTask(task)
    End Sub

    ' Get this Function
    Public Function ViewTaskSummary(Current_Date As Date, empId As Integer) As List(Of TaskSummary) Implements IAideService.ViewTaskSummary
        Dim lstTaskSummary As List(Of TaskSummary) = Nothing
        MyBase.ViewEmployeeTaskSummry(empId, Current_Date, lstTaskSummary)
        Return lstTaskSummary
    End Function

    ' Get this Function
    Public Function ViewTaskSummaryAll(Current_Date As Date) As List(Of TaskSummary) Implements IAideService.ViewTaskSummaryAll
        Dim lstTaskSummary As List(Of TaskSummary) = Nothing
        MyBase.ViewTasksSummaryAll(Current_Date, lstTaskSummary)
        Return lstTaskSummary
    End Function

    ' Get this Function
    Public Function GetAllTasks() As List(Of Tasks) Implements IAideService.GetAllTasks
        Dim objTasks As List(Of Tasks) = Nothing
        MyBase.GetTasksAll(objTasks)
        Return objTasks
    End Function

    Public Function ViewTasksByEmployee(empId As Integer) As List(Of Tasks) Implements IAideService.ViewTasksByEmployee
        Dim lstTasks As List(Of Tasks) = Nothing
        MyBase.ViewMyTasks(empId, lstTasks)
        Return lstTasks
    End Function

    Public Sub UpdateTask(Task As Tasks) Implements IAideService.UpdateTask
        MyBase.UpdateEmployeeTask(Task)
    End Sub
#End Region

#Region "Status"
    Public Function GetStatusList(groupId As Integer) As List(Of StatusGroup) Implements IAideService.GetStatusList
        Dim lstStatusGroup As List(Of StatusGroup) = Nothing
        Select Case groupId
            Case Grouping.Attendance
                MyBase.GetAttendanceStatusList(lstStatusGroup)
            Case Grouping.CivilStatus
                MyBase.GetCivilStatusList(lstStatusGroup)
            Case Grouping.TaskCategory
                MyBase.GetTaskCategoriesList(lstStatusGroup)
            Case Grouping.TaskStatus
                MyBase.GetTaskStatusList(lstStatusGroup)
            Case Grouping.Phase
                MyBase.GetPhaseStatusList(lstStatusGroup)
            Case Grouping.Rework
                MyBase.GetReworkStatusList(lstStatusGroup)
            Case Else
                MyBase.GetGenericStatusList(groupId, lstStatusGroup)
        End Select
        Return lstStatusGroup
    End Function
#End Region

#Region "Resource Planner"
    'Public Sub InsertResourcePlanners(resource As ResourcePlanner) Implements IAideService2.InsertResourcePlanner
    '    MyBase.InsertResourcePlanner(resource)
    'End Sub

    Public Sub UpdateResourcePlanners(resource As ResourcePlanner) Implements IAideService.UpdateResourcePlanner
        MyBase.UpdateResourcePlanner(resource)
    End Sub

    Public Overrides Function ViewEmpResourcePlanner(ByVal deptID As Integer) As List(Of ResourcePlanner) Implements IAideService.ViewEmpResourcePlanner
        Return MyBase.ViewEmpResourcePlanners(deptID)
    End Function

    Public Overrides Function GetStatusResourcePlanner() As List(Of ResourcePlanner) Implements IAideService.GetStatusResourcePlanner
        Return MyBase.GetStatusResourcePlanners
    End Function

    Public Function GetResourcePlannerByEmpIDs(empID As Integer, deptID As Integer, month As Integer, year As Integer) As List(Of ResourcePlanner) Implements IAideService.GetResourcePlannerByEmpID
        Return MyBase.GetResourcePlannerByEmpID(empID, deptID, month, year)
    End Function

    Public Function GetAllEmpResourcePlanners(deptID As Integer, month As Integer, year As Integer) As List(Of ResourcePlanner) Implements IAideService.GetAllEmpResourcePlanner
        Return MyBase.GetAllEmpResourcePlanner(deptID, month, year)
    End Function

    Public Function GetAllEmpResourcePlannerByStatuss(deptID As Integer, month As Integer, year As Integer, status As Integer) As List(Of ResourcePlanner) Implements IAideService.GetAllEmpResourcePlannerByStatus
        Return MyBase.GetAllEmpResourcePlannerByStatus(deptID, month, year, status)
    End Function

    Public Overrides Function GetAllStatusResourcePlanner() As List(Of ResourcePlanner) Implements IAideService.GetAllStatusResourcePlanner
        Return MyBase.GetAllStatusResourcePlanners
    End Function
#End Region

End Class
