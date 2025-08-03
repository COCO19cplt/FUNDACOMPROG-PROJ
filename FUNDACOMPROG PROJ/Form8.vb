Imports MySql.Data.MySqlClient
Public Class Form8

    Private connectionString As String = "server=localhost;user id=root;password=;database=LoadLog;"
    Private conn As New MySqlConnection(connectionString)
    Private cmd As MySqlCommand
    Private da As MySqlDataAdapter
    Private ds As DataSet
    Private query As String
    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Using conn As New MySqlConnection(connectionString)
                Dim sql As String = "SELECT * FROM orders"
                Dim dataAdapter1 As New MySqlDataAdapter(sql, conn)
                ds = New DataSet()
                dataAdapter1.Fill(ds, "orders")
                DataGridView1.DataSource = ds.Tables("orders")
            End Using
        Catch ex As Exception
            MsgBox("Error in collecting data from Database. Error is :" & ex.Message)
        End Try
    End Sub
    Private Sub RefreshData()
        Try
            Using tempConn As New MySqlConnection(connectionString)
                Dim sql As String = "SELECT * FROM Orders"
                Dim dataAdapter1 As New MySqlDataAdapter(sql, tempConn)
                ds = New DataSet()
                dataAdapter1.Fill(ds, "orders")
                DataGridView1.DataSource = ds.Tables("orders")
            End Using
        Catch ex As Exception
            MsgBox("Error in collecting data from Database. Error is :" & ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form4.Show()
        Me.Hide()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form5.Show()
        Me.Hide()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form6.Show()
        Me.Hide()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Form7.Show()
        Me.Hide()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Me.Show()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Try
            ' 1. Validate (we need CustomerID, PickupDate, TotalAmount, Status, Instructions)
            If String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox4.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox5.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox6.Text) Then

                MessageBox.Show("Please fill in CustomerID, PickupDate, TotalAmount, Status, and Instructions.")
                Exit Sub
            End If

            ' 2. Open the connection
            conn.Open()

            ' 3. Build your INSERT query for orders (omit auto‑inc OrderID & OrderDate)
            query = "INSERT INTO orders " &
                    "(CustomerID, PickupDate, TotalAmount, OrderStatus, SpecialInstructions) VALUES " &
                    "(" &
                      $"{TextBox2.Text}, " &                                  ' CustomerID
                      $"'{TextBox3.Text}', " &                               ' PickupDate
                      $"{TextBox4.Text}, " &                                 ' TotalAmount
                      $"'{TextBox5.Text}', " &                               ' OrderStatus
                      $"'{TextBox6.Text}'" &                                 ' SpecialInstructions
                    ");"

            ' 4. Execute
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            MessageBox.Show("Order added successfully.")

            ' 5. Clear input textboxes (except TextBox1)
            TextBox2.Clear()
            TextBox3.Clear()
            TextBox4.Clear()
            TextBox5.Clear()
            TextBox6.Clear()

            ' 6. Refresh grid to show new order
            RefreshData()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            ' 7. Ensure the connection is closed
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Try
            ' 1. Validate: need OrderID to delete
            If String.IsNullOrWhiteSpace(TextBox1.Text) Then
                MessageBox.Show("Please enter the OrderID to delete.")
                Exit Sub
            End If

            ' 2. Open the connection
            conn.Open()

            ' 3. Build your DELETE query against orders
            Dim deleteSql As String = $"DELETE FROM orders WHERE OrderID = {TextBox1.Text}"

            ' 4. Execute the command
            cmd = New MySqlCommand(deleteSql, conn)
            Dim rowsAffected = cmd.ExecuteNonQuery()

            If rowsAffected > 0 Then
                MessageBox.Show("Order deleted successfully.")
            Else
                MessageBox.Show("No order found with that ID.")
            End If

            ' 5. Clear textboxes and refresh grid
            TextBox1.Clear()
            TextBox2.Clear()
            TextBox3.Clear()
            TextBox4.Clear()
            TextBox5.Clear()
            TextBox6.Clear()
            RefreshData()   ' make sure this now selects FROM orders

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            ' 6. Ensure the connection is closed
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Form9.Show()
        Me.Hide()
    End Sub
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Form10.Show()
        Me.Hide()
    End Sub
    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Form11.Show()
        Me.Hide()
    End Sub
    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click

    End Sub

End Class
