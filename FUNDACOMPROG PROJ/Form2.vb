Imports MySql.Data.MySqlClient

Public Class Form2
    Dim conn As New MySqlConnection("server=localhost;user id=root;password=;database=LoadLog;")
    Dim query As String
    Dim cmd As MySqlCommand

    ' Public static property to hold UserID after login
    Public Shared LoggedInUserID As Integer

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim username As String = TextBox1.Text.Trim().Replace("'", "''")
        Dim password As String = TextBox2.Text.Trim().Replace("'", "''")

        If username = "" Or password = "" Then
            MessageBox.Show("Please enter both username and password.")
            Exit Sub
        End If

        Try
            conn.Open()
            query = "SELECT * FROM users WHERE Username='" & username & "' AND Password='" & password & "'"
            cmd = New MySqlCommand(query, conn)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            If reader.Read() Then
                ' Get UserID from the users table
                LoggedInUserID = Convert.ToInt32(reader("UserID"))

                Dim role As String = reader("Role").ToString().ToLower()
                If (username = "johnluis@pgmail.com" And password = "pilaradmin") OrElse
                   (username = "iyannaangela@gmail.com" And password = "marquezadmin") Then
                    ' Admins
                    Form4.Show()
                    Me.Hide()
                ElseIf username = "rosaly@gmail.com" And password = "staffrosaly" OrElse
                       (username = "judith@gmail.com" And password = "staffjudith") OrElse
                       (username = "mary@gmail.com" And password = "staffmary") Then
                    ' Staff
                    Staffform.Show()
                    Me.Hide()
                ElseIf (username = "edgar@gmail.com" And password = "inventoryedgar") Then
                    ' Inventory Manager
                    Dim invForm As New Form15()
                    invForm.CurrentUserID = LoggedInUserID  ' Pass UserID to inventory form
                    invForm.Show()
                    Me.Hide()
                Else
                    ' All other users are customers
                    Form12.Show()
                    Me.Hide()
                End If
            Else
                MessageBox.Show("Invalid username or password.")
            End If

            reader.Close()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form17.Show()
        Me.Hide()
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub
End Class