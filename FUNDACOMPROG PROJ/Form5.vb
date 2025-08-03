Imports MySql.Data.MySqlClient
Public Class Form5

    Private connectionString As String = "server=localhost;user id=root;password=;database=LoadLog;"
    Private conn As New MySqlConnection(connectionString)
    Private cmd As MySqlCommand
    Private da As MySqlDataAdapter
    Private ds As DataSet
    Private query As String
    Private Sub RefreshData()
        Try
            Using tempConn As New MySqlConnection(connectionString)
                Dim sql As String = "SELECT * FROM customers"
                Dim dataAdapter1 As New MySqlDataAdapter(sql, tempConn)
                ds = New DataSet()
                dataAdapter1.Fill(ds, "customers")
                DataGridView1.DataSource = ds.Tables("customers")
            End Using
        Catch ex As Exception
            MsgBox("Error in collecting data from Database. Error is :" & ex.Message)
        End Try
    End Sub
    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Using conn As New MySqlConnection(connectionString)
                Dim sql As String = "SELECT * FROM Customers"
                Dim dataAdapter1 As New MySqlDataAdapter(sql, conn)
                ds = New DataSet()
                dataAdapter1.Fill(ds, "Customers")
                DataGridView1.DataSource = ds.Tables("Customers")
            End Using
        Catch ex As Exception
            MsgBox("Error in collecting data from Database. Error is :" & ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form4.Show()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Try
            ' 1. Validate
            If String.IsNullOrWhiteSpace(TextBox1.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox4.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox5.Text) Then

                MessageBox.Show("Please fill in all required fields.")
                Exit Sub
            End If

            ' 2. Open the connection
            conn.Open()

            ' 3. Build your INSERT query - FIXED: Added missing closing parenthesis
            query = $"INSERT INTO customers (CustomerID, CustomerName, PhoneNo,HomeAddress, Email) " &
                $"VALUES ('{TextBox1.Text}', '{TextBox2.Text}', '{TextBox3.Text}','{TextBox4.Text}','{TextBox5.Text}');"

            ' 4. Create and execute the command
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            MessageBox.Show("User added successfully.")

            ' Clear textboxes and refresh grid
            TextBox1.Clear()
            TextBox2.Clear()
            TextBox3.Clear()
            TextBox4.Clear()
            TextBox5.Clear()
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
                MessageBox.Show("Please select a Customer row to update.")
                Return
            End If

            ' 2. Validate the 4 textboxes - Now checking all 4 including CustomerID
            If String.IsNullOrWhiteSpace(TextBox1.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox4.Text) OrElse
                String.IsNullOrWhiteSpace(TextBox4.Text) Then
                MessageBox.Show("Please fill in CustomerID, CustomerName, PhoneNo, HomeAddress, and Email.")
                Return
            End If


            Dim newCustomerId As Integer
            If Not Integer.TryParse(TextBox1.Text, newCustomerId) Then
                MessageBox.Show("CustomerID must be a valid number.")
                Return
            End If

            Dim originalCustomerId As Integer = Convert.ToInt32(DataGridView1.CurrentRow.Cells("CustomerID").Value)

            ' 5. Open connection
            conn.Open()


            Dim sql As String = $"UPDATE Customers SET " &
                               $"CustomerID = {newCustomerId}, " &
                               $"CustomerName = '{TextBox2.Text}', " &
                               $"PhoneNo = '{TextBox3.Text}', " &
                               $"HomeAddress = '{TextBox4.Text}' " &
                               $"Email = '{TextBox5.Text}' " &
                               $"WHERE CustomerID = {originalCustomerId};"



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
                MessageBox.Show("Please enter the CustomerName to delete.")
                Exit Sub
            End If

            Dim nameToDelete As String = TextBox2.Text

            ' 2. Confirm with the user
            Dim result As DialogResult = MessageBox.Show(
        $"Are you sure you want to delete Customer '{nameToDelete}'?",
        "Confirm Delete",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning
    )
            If result = DialogResult.No Then Exit Sub

            conn.Open()

            ' 3. Delete from Customers matching the exact username
            query = $"DELETE FROM Customers WHERE CustomerName = '{nameToDelete}';"
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            MessageBox.Show("Customer deleted successfully.")
            conn.Close()

            ' 4. Refresh and clear
            TextBox1.Clear()
            TextBox2.Clear()
            TextBox3.Clear()
            TextBox4.Clear()
            TextBox5.Clear()
            RefreshData()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form6.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Form7.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Form8.Show()
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Form11.Show()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs)

    End Sub
End Class
