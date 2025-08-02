Imports MySql.Data.MySqlClient

Public Class Form9
    Private RefreshData()
    ' 1. Declare conn properly
    Private connectionString As String = "server=localhost;user id=root;password=;database=LoadLog;"
    Private conn As New MySqlConnection(connectionString)
    Private cmd As MySqlCommand
    Private da As MySqlDataAdapter
    Private ds As DataSet
    Private query As String





    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Try
            ' Validate
            If String.IsNullOrWhiteSpace(TextBox1.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox4.Text) Then

                MessageBox.Show("Please fill in all required fields.")
                Exit Sub
            End If

            ' 2. Open the connection
            conn.Open()

            ' 3. Build your INSERT query
            query = $"INSERT INTO CUSTOMERS (CustomerName, PhoneNo, HomeAddress, Email) " &
                    $"VALUES ('{TextBox1.Text}', '{TextBox2.Text}', '{TextBox3.Text}', '{TextBox4.Text}');"

            ' 4. Create and execute the command
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            MessageBox.Show("Customer added successfully.")
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            ' 5. Ensure the connection is closed
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Try
            ' 1. Make sure a row is selected in the grid
            If DataGridView1.CurrentRow Is Nothing Then
                MessageBox.Show("Please select a customer row to update.")
                Return
            End If

            ' 2. Validate the 4 textboxes
            If String.IsNullOrWhiteSpace(TextBox1.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox4.Text) Then

                MessageBox.Show("Please fill in Name, Phone, Address and Email.")
                Return
            End If

            ' 3. Grab CustomerID from the selected grid row
            Dim customerId As Integer =
            Convert.ToInt32(DataGridView1.CurrentRow.Cells("CustomerID").Value)

            ' 4. Open connection
            conn.Open()

            ' 5. Build and execute the UPDATE
            Dim sql = $"UPDATE CUSTOMERS SET " &
                  $"CustomerName = '{TextBox1.Text}', " &
                  $"PhoneNo      = '{TextBox2.Text}', " &
                  $"HomeAddress  = '{TextBox3.Text}', " &
                  $"Email        = '{TextBox4.Text}' " &
                  $"WHERE CustomerID = {customerId};"

            cmd = New MySqlCommand(sql, conn)
            cmd.ExecuteNonQuery()

            MessageBox.Show("Customer updated successfully.")
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            ' 6. Ensure connection is closed
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

            ' 7. Refresh the grid to show changes

        End Try
    End Sub


    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Form9_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Try
            ' 1. Validate that a customer name is entered
            If String.IsNullOrWhiteSpace(TextBox1.Text) Then
                MessageBox.Show("Please enter the customer name to delete.")
                Exit Sub
            End If

            Dim nameToDelete As String = TextBox1.Text

            ' 2. Confirm with the user
            Dim result As DialogResult = MessageBox.Show(
                $"Are you sure you want to delete customer '{nameToDelete}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            )
            If result = DialogResult.No Then Exit Sub

            conn.Open()

            ' 3. Delete from CUSTOMERS matching the exact name
            query = $"DELETE FROM CUSTOMERS WHERE CustomerName = '{nameToDelete}';"
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            MessageBox.Show("Customer deleted successfully.")
            conn.Close()

            ' 4. Refresh and clear

            TextBox1.Clear()
            TextBox2.Clear()
            TextBox3.Clear()
            TextBox4.Clear()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Form11.Show()
    End Sub
End Class