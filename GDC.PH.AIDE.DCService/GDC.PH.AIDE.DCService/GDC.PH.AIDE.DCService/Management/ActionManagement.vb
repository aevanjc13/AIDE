﻿Imports GDC.PH.AIDE.Entity

''' <summary>
''' By Jester Sanchez
''' </summary>
''' <remarks></remarks>
Public Class ActionManagement
    Inherits ManagementBase
    Public Overrides Function GetExceptionMessage(ex As Exception) As String
        Dim message As String = ex.Message
        Return message
    End Function

    Public Overrides Function GetMappedFields(objData As Object) As Object
        Dim objAction As ActionListSet = DirectCast(objData, ActionListSet)
        Dim ActionData As New Action
        ActionData.Act_ID = objAction.ActionID
        ActionData.Act_Message = objAction.ActionMessage
        ActionData.Act_Assignee = objAction.ActionAssignee
        ActionData.Act_DueDate = objAction.ActionDueDate
        ActionData.Act_DateClosed = objAction.ActionDateClosed
        ActionData.Act_NickName = objAction.ActionNickName
        Return ActionData
    End Function

    Public Overrides Function GetStateData(status As NotifyType, Optional data As Object = Nothing, Optional message As String = "") As StateData
        Dim state As New StateData
        state.Data = data
        state.Message = message
        state.NotifyType = status
        Return state
    End Function

    Public Overrides Sub SetFields(ByRef objResult As Object, objData As Object)
        Dim objAction As Action = DirectCast(objData, Action)
        Dim ActionData As New ActionListSet
        ActionData.ActionID = objAction.Act_ID
        ActionData.ActionMessage = objAction.Act_Message
        ActionData.ActionAssignee = objAction.Act_Assignee
        ActionData.ActionDueDate = objAction.Act_DueDate
        ActionData.ActionDateClosed = objAction.Act_DateClosed
        ActionData.ActionNickName = objAction.Act_NickName
        objResult = ActionData
    End Sub

    Public Function InsertAction(ByVal _action As Action) As StateData
        Dim _actionset As New ActionListSet
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType
        Try
            SetFields(_actionset, _action)
            If _actionset.InsertActionList() Then
                status = NotifyType.IsSuccess
            End If
        Catch ex As Exception
            status = NotifyType.IsError
        End Try
        state = GetStateData(status)
        Return state
    End Function

    Public Function UpdateAction(ByVal _action As Action) As StateData
        Dim _actionset As New ActionListSet
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType
        Try
            SetFields(_actionset, _action)
            If _actionset.UpdateActionList() Then
                status = NotifyType.IsSuccess
            End If
        Catch ex As Exception
            status = NotifyType.IsError
        End Try
        state = GetStateData(status)
        Return state
    End Function

    Public Function GetActionList() As StateData
        Dim _actionset As New ActionListSet
        Dim lstactionlist As List(Of ActionListSet)
        Dim objAction As New List(Of Action)
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType
        Try
            lstactionlist = _actionset.GetAllActionList()
            If Not IsNothing(lstactionlist) Then
                For Each objact As ActionListSet In lstactionlist
                    objAction.Add(DirectCast(GetMappedFields(objact), Action))
                Next
                status = NotifyType.IsSuccess
            End If
        Catch ex As Exception
            status = NotifyType.IsError
            message = GetExceptionMessage(ex)
        End Try
        state = GetStateData(status, objAction, message)
        Return state
    End Function

    Public Function GetActionByMessage(ByVal _Message As String) As StateData
        Dim _actionset As New ActionListSet
        Dim lstactionlist As List(Of ActionListSet)
        Dim objAction As New List(Of Action)
        Dim message As String = ""
        Dim state As StateData
        Dim status As NotifyType
        Try
            lstactionlist = _actionset.GetActionListByMessage(_Message)
            If Not IsNothing(lstactionlist) Then
                For Each objact As ActionListSet In lstactionlist
                    objAction.Add(DirectCast(GetMappedFields(objact), Action))
                Next
            End If
        Catch ex As Exception
            status = NotifyType.IsError
            message = GetExceptionMessage(ex)
        End Try
        state = GetStateData(status, objAction, message)
        Return state
    End Function
End Class
