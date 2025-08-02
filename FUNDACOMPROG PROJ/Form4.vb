Imports MySql.Data.MySqlClient

Public Class Form4

    Private connectionString As String = "server=localhost;user id=root;password=;database=LoadLog;"

    ' 1. Declare conn properly
    Private conn As New MySqlConnection(connectionString)
    Private cmd As MySqlCommand
    Private da As MySqlDataAdapter
    Private ds As DataSet
    Private query As String

    Private Sub RefreshData()
        Try
            Using tempConn As New MySqlConnection(connectionString)
                Dim sql As String = "SELECT * FROM USERS"
                Dim dataAdapter1 As New MySqlDataAdapter(sql, tempConn)
                ds = New DataSet()
                dataAdapter1.Fill(ds, "USERS")
                DataGridView1.DataSource = ds.Tables("USERS")
            End Using
        Catch ex As Exception
            MsgBox("Error in collecting data from Database. Error is :" & ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Show()
    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form6.Show()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form5.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Form7.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Form8.Show()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Try
            ' 1. Validate
            If String.IsNullOrWhiteSpace(TextBox1.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox4.Text) Then

                MessageBox.Show("Please fill in all required fields.")
                Exit Sub
            End If

            ' 2. Open the connection
            conn.Open()

            ' 3. Build your INSERT query - FIXED: Added missing closing parenthesis
            query = $"INSERT INTO USERS (UserID, Username, Password, Role) " &
                $"VALUES ('{TextBox1.Text}', '{TextBox2.Text}', '{TextBox3.Text}','{TextBox4.Text}');"

            ' 4. Create and execute the command
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            MessageBox.Show("User added successfully.")

            ' Clear textboxes and refresh grid
            TextBox1.Clear()
            TextBox2.Clear()
            TextBox3.Clear()
            TextBox4.Clear()
            RefreshData()


        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            ' 5. Ensure the connection is closed
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

        End Try

    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click

        Try
            ' 1. Make sure a row is selected in the grid
            If DataGridView1.CurrentRow Is Nothing Then
                MessageBox.Show("Please select a user row to update.")
                Return
            End If

            ' 2. Validate the 4 textboxes - Now checking all 4 including UserID
            If String.IsNullOrWhiteSpace(TextBox1.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox4.Text) Then
                MessageBox.Show("Please fill in UserID, Username, Password, and Role.")
                Return
            End If


            Dim newUserId As Integer
            If Not Integer.TryParse(TextBox1.Text, newUserId) Then
                MessageBox.Show("UserID must be a valid number.")
                Return
            End If

            Dim originalUserId As Integer = Convert.ToInt32(DataGridView1.CurrentRow.Cells("UserID").Value)

            ' 5. Open connection
            conn.Open()


            Dim sql As String = $"UPDATE USERS SET " &
                               $"UserID = {newUserId}, " &
                               $"Username = '{TextBox2.Text}', " &
                               $"Password = '{TextBox3.Text}', " &
                               $"Role = '{TextBox4.Text}' " &
                               $"WHERE UserID = {originalUserId};"



            cmd = New MySqlCommand(sql, conn)
            cmd.ExecuteNonQuery()
            MessageBox.Show("User updated successfully.")
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            ' 6. Ensure connection is closed
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            ' 7. Refresh the grid to show changes
            RefreshData()
        End Try
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Try
            ' 1. Validate that a username is entered
            If String.IsNullOrWhiteSpace(TextBox2.Text) Then
                MessageBox.Show("Please enter the username to delete.")
                Exit Sub
            End If

            Dim nameToDelete As String = TextBox2.Text

            ' 2. Confirm with the user
            Dim result As DialogResult = MessageBox.Show(
        $"Are you sure you want to delete user '{nameToDelete}'?",
        "Confirm Delete",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning
    )
            If result = DialogResult.No Then Exit Sub

            conn.Open()

            ' 3. Delete from USERS matching the exact username
            query = $"DELETE FROM USERS WHERE Username = '{nameToDelete}';"
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            MessageBox.Show("User deleted successfully.")
            conn.Close()

            ' 4. Refresh and clear
            TextBox1.Clear()
            TextBox2.Clear()
            TextBox3.Clear()
            TextBox4.Clear()
            RefreshData()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            Using tempConn As New MySqlConnection(connectionString)
                Dim sql As String = "SELECT * FROM USERS"
                Dim dataAdapter1 As New MySqlDataAdapter(sql, tempConn)
                ds = New DataSet()
                dataAdapter1.Fill(ds, "USERS")
                DataGridView1.DataSource = ds.Tables("USERS")
            End Using
        Catch ex As Exception
            MsgBox("Error in collecting data from Database. Error is :" & ex.Message)
        End Try
    End Sub

    Private Sub MaskedTextBox1_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles MaskedTextBox1.MaskInputRejected

    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Form11.Show()
    End Sub
End Class
