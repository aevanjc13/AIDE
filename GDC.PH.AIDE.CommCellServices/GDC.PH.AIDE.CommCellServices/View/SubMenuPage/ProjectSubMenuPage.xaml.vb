Class ProjectSubMenuPage
    Private _pFrame As New Frame
    Public Sub New(pFrame As Frame)
        _pFrame = pFrame
        InitializeComponent()
    End Sub

    Private Sub AssignedProject_Click(sender As Object, e As RoutedEventArgs)
        _pFrame.Navigate(New ViewProjectUI(_pFrame))
    End Sub

    Private Sub CreateProject_Click(sender As Object, e As RoutedEventArgs)
        _pFrame.Navigate(New CreateProjectPage(_pFrame))
    End Sub
End Class
