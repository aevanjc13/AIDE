Imports GDC.PH.AIDE.BusinessLayer
''' <summary>
''' By Jhunell G. Barcenas
''' </summary>
''' <remarks></remarks>
Public Interface IResourcePlanner

    Property EmployeeID As Integer
    Property Name As String
    Property dateFrom As Date
    Property dateTo As Date
    Property Status As Integer
    Property Description As String
    Property Image_Path As String
    Property dateEntry As Date

    'Function InsertResourcePlanner(ByVal resource As ResourcePlannerSet) As Boolean
    Function UpdateResourcePlanner(ByVal resource As ResourcePlannerSet) As Boolean
    Function ViewEmpResourcePlanner(ByVal deptID As Integer) As List(Of ResourcePlannerSet)
    Function GetStatusResourcePlanner() As List(Of ResourcePlannerSet)
    Function GetResourcePlannerByEmpID(ByVal empID As Integer, ByVal deptID As Integer, ByVal month As Integer, ByVal year As Integer) As List(Of ResourcePlannerSet)
    Function GetAllEmpResourcePlanner(ByVal deptID As Integer, ByVal month As Integer, ByVal year As Integer) As List(Of ResourcePlannerSet)
    Function GetAllEmpResourcePlannerByStatus(ByVal deptID As Integer, ByVal month As Integer, ByVal year As Integer, ByVal status As Integer) As List(Of ResourcePlannerSet)
    Function GetAllStatusResourcePlanner() As List(Of ResourcePlannerSet)
End Interface
