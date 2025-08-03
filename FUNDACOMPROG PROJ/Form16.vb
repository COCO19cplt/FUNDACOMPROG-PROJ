Imports MySql.Data.MySqlClient

Public Class Form16
    Public CurrentUserID As Integer

    Dim conn As New MySqlConnection("server=localhost;user id=root;password=;database=loadlog")

    Private Sub Form16_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadExpenses()
        LoadOrders()
    End Sub

    Private Sub LoadExpenses()
        Try
            Dim cmd As New MySqlCommand("SELECT * FROM expenses", conn)
            Dim adapter As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            adapter.Fill(dt)
            DataGridView1.DataSource = dt
        Catch ex As Exception
            MessageBox.Show("Error loading expenses: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadOrders()
        Try
            Dim cmd As New MySqlCommand("SELECT OrderID FROM orders", conn)
            Dim adapter As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            adapter.Fill(dt)
            ComboBox1.DataSource = dt
            ComboBox1.DisplayMember = "OrderID"
            ComboBox1.ValueMember = "OrderID"
        Catch ex As Exception
            MessageBox.Show("Error loading orders: " & ex.Message)
        End Try
    End Sub

    ' INSERT
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim expenseType As String = TextBox1.Text.Trim()
        Dim amountStr As String = TextBox2.Text.Trim()
        Dim notes As String = TextBox3.Text.Trim()
        Dim orderID As String = ComboBox1.Text
        Dim expenseDate As String = DateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss")
        Dim userID As Integer = CurrentUserID

        If expenseType = "" Or amountStr = "" Then
            MessageBox.Show("Required fields must not be empty.")
            Return
        End If

        Dim notesPart As String = If(notes = "", "NULL", "'" & notes.Replace("'", "''") & "'")

        Dim query As String = "INSERT INTO expenses (ExpenseDate, ExpenseType, Amount, Notes, UserID, OrderID) VALUES (" &
                              "'" & expenseDate & "', '" & expenseType.Replace("'", "''") & "', " & amountStr & ", " & notesPart & ", " & userID & ", " & orderID & ")"

        Try
            Dim cmd As New MySqlCommand(query, conn)
            conn.Open()
            cmd.ExecuteNonQuery()
            conn.Close()
            MessageBox.Show("Expense inserted.")
            LoadExpenses()
        Catch ex As Exception
            MessageBox.Show("Insert error: " & ex.Message)
            conn.Close()
        End Try
    End Sub

    ' UPDATE
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim expenseID As String = TextBox4.Text.Trim()
        Dim expenseType As String = TextBox1.Text.Trim()
        Dim amountStr As String = TextBox2.Text.Trim()
        Dim notes As String = TextBox3.Text.Trim()
        Dim orderID As String = ComboBox1.Text
        Dim expenseDate As String = DateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss")
        Dim userID As Integer = CurrentUserID

        If expenseID = "" Then
            MessageBox.Show("Enter ExpenseID to update.")
            Return
        End If

        Dim notesPart As String = If(notes = "", "NULL", "'" & notes.Replace("'", "''") & "'")

        Dim query As String = "UPDATE expenses SET " &
                              "ExpenseDate = '" & expenseDate & "', " &
                              "ExpenseType = '" & expenseType.Replace("'", "''") & "', " &
                              "Amount = " & amountStr & ", " &
                              "Notes = " & notesPart & ", " &
                              "UserID = " & userID & ", " &
                              "OrderID = " & orderID & " " &
                              "WHERE ExpenseID = " & expenseID

        Try
            Dim cmd As New MySqlCommand(query, conn)
            conn.Open()
            Dim rowsAffected = cmd.ExecuteNonQuery()
            conn.Close()
            If rowsAffected > 0 Then
                MessageBox.Show("Expense updated.")
                LoadExpenses()
            Else
                MessageBox.Show("ExpenseID not found.")
            End If
        Catch ex As Exception
            MessageBox.Show("Update error: " & ex.Message)
            conn.Close()
        End Try
    End Sub

    ' DELETE
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim expenseID As String = TextBox4.Text.Trim()
        If expenseID = "" Then
            MessageBox.Show("Enter ExpenseID to delete.")
            Return
        End If

        Dim query As String = "DELETE FROM expenses WHERE ExpenseID = " & expenseID

        Try
            Dim cmd As New MySqlCommand(query, conn)
            conn.Open()
            Dim rowsAffected = cmd.ExecuteNonQuery()
            conn.Close()
            If rowsAffected > 0 Then
                MessageBox.Show("Expense deleted.")
                LoadExpenses()
            Else
                MessageBox.Show("ExpenseID not found.")
            End If
        Catch ex As Exception
            MessageBox.Show("Delete error: " & ex.Message)
            conn.Close()
        End Try
    End Sub
End Class