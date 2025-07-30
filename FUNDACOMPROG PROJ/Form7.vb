Imports MySql.Data.MySqlClient
Public Class Form7

    Private connectionString As String = "server=localhost;user id=root;password=;database=LoadLog;"
    Private conn As New MySqlConnection(connectionString)
    Private cmd As MySqlCommand
    Private da As MySqlDataAdapter
    Private ds As DataSet
    Private query As String
    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Using conn As New MySqlConnection(connectionString)
                Dim sql As String = "SELECT * FROM inventory"
                Dim dataAdapter1 As New MySqlDataAdapter(sql, conn)
                ds = New DataSet()
                dataAdapter1.Fill(ds, "inventory")
                DataGridView1.DataSource = ds.Tables("inventory")
            End Using
        Catch ex As Exception
            MsgBox("Error in collecting data from Database. Error is :" & ex.Message)
        End Try
    End Sub
    Private Sub RefreshData()
        Try
            Using tempConn As New MySqlConnection(connectionString)
                Dim sql As String = "SELECT * FROM Services"
                Dim dataAdapter1 As New MySqlDataAdapter(sql, tempConn)
                ds = New DataSet()
                dataAdapter1.Fill(ds, "Services")
                DataGridView1.DataSource = ds.Tables("Services")
            End Using
        Catch ex As Exception
            MsgBox("Error in collecting data from Database. Error is :" & ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form4.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form5.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form6.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Me.Show()
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
           String.IsNullOrWhiteSpace(TextBox4.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox5.Text) OrElse
                String.IsNullOrWhiteSpace(TextBox6.Text) Then
                MessageBox.Show("Please fill in all required fields.")
                Exit Sub
            End If

            ' 2. Open the connection
            conn.Open()

            ' 3. Build your INSERT query - FIXED: Added missing closing parenthesis
            query = $"INSERT INTO inventory (ItemID, ItemName, Quantity ,Unit , ReorderLevel, SupplierInfo) " &
                $"VALUES ('{TextBox1.Text}', '{TextBox2.Text}', '{TextBox3.Text}','{TextBox4.Text}','{TextBox5.Text}','{TextBox6.Text}');"

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
            TextBox6.Clear()
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
                MessageBox.Show("Please select a Inventory row to update.")
                Return
            End If

            ' 2. Validate the 4 textboxes - Now checking all 4 including CustomerID
            If String.IsNullOrWhiteSpace(TextBox1.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox4.Text) OrElse
                String.IsNullOrWhiteSpace(TextBox5.Text) OrElse
                String.IsNullOrWhiteSpace(TextBox6.Text) Then
                MessageBox.Show("Please fill in ItemID, ItemName, Quantity, Unit, ReorderLevel, and SupplierInfo.")
                Return
            End If


            Dim newItemId As Integer
            If Not Integer.TryParse(TextBox1.Text, newItemId) Then
                MessageBox.Show("ItemID must be a valid number.")
                Return
            End If

            Dim originalItemId As Integer = Convert.ToInt32(DataGridView1.CurrentRow.Cells("ItemID").Value)

            ' 5. Open connection
            conn.Open()


            Dim sql As String = $"UPDATE inventory SET " &
                               $"ItemID = {newItemId}, " &
                               $"ItemName = '{TextBox2.Text}', " &
                               $"Quantity = '{TextBox3.Text}', " &
                               $"Unit = '{TextBox4.Text}' " &
                                $"ReorderLevel = '{TextBox5.Text}' " &
                                 $"SupplierInfo = '{TextBox6.Text}' " &
                               $"WHERE ItemID = {originalItemId};"



            cmd = New MySqlCommand(sql, conn)
            cmd.ExecuteNonQuery()
            MessageBox.Show("inventory updated successfully.")
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
        $"Are you sure you want to delete Services '{nameToDelete}'?",
        "Confirm Delete",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning
    )
            If result = DialogResult.No Then Exit Sub

            conn.Open()

            ' 3. Delete from Customers matching the exact username
            query = $"DELETE FROM inventory WHERE ItemName = '{nameToDelete}';"
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            MessageBox.Show("Inventory deleted successfully.")
            conn.Close()

            ' 4. Refresh and clear
            TextBox1.Clear()
            TextBox2.Clear()
            TextBox3.Clear()
            TextBox4.Clear()
            TextBox5.Clear()
            TextBox6.Clear()
            RefreshData()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

End Class