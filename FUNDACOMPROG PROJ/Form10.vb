
Imports MySql.Data.MySqlClient

Public Class Form10
    Private connectionString As String = "server=localhost;user id=root;password=;database=LoadLog;"
    Private conn As New MySqlConnection(connectionString)
    Private cmd As MySqlCommand
    Private da As MySqlDataAdapter
    Private ds As DataSet
    Private query As String

    Private Sub Form10_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RefreshPayments()
    End Sub

    Private Sub RefreshPayments()
        Try
            Using tempConn As New MySqlConnection(connectionString)
                Dim sql As String = "SELECT * FROM payments"
                Dim dataAdapter1 As New MySqlDataAdapter(sql, tempConn)
                ds = New DataSet()
                dataAdapter1.Fill(ds, "payments")
                DataGridView1.DataSource = ds.Tables("payments")
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading payments: " & ex.Message)
        End Try
    End Sub

    ' ADD PAYMENT
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Try
            If String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox4.Text) Then
                MessageBox.Show("OrderID, PaymentMethod, and Amount are required.")
                Exit Sub
            End If

            conn.Open()
            query = "INSERT INTO payments (OrderID, PaymentMethod, Amount, PaymentStatus, TransactionReference) VALUES (" &
                    $"{TextBox2.Text}, " &
                    $"'{TextBox3.Text}', " &
                    $"{TextBox4.Text}, " &
                    $"'{TextBox5.Text}', " &
                    $"'{TextBox6.Text}')"

            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            MessageBox.Show("Payment added successfully.")
            RefreshPayments()

            TextBox1.Clear()
            TextBox2.Clear()
            TextBox3.Clear()
            TextBox4.Clear()
            TextBox5.Clear()
            TextBox6.Clear()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    ' UPDATE PAYMENT
    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Try
            If String.IsNullOrWhiteSpace(TextBox1.Text) Then
                MessageBox.Show("PaymentID is required for update.")
                Exit Sub
            End If

            conn.Open()
            query = "UPDATE payments SET " &
                    $"OrderID = {TextBox2.Text}, " &
                    $"PaymentMethod = '{TextBox3.Text}', " &
                    $"Amount = {TextBox4.Text}, " &
                    $"PaymentStatus = '{TextBox5.Text}', " &
                    $"TransactionReference = '{TextBox6.Text}' " &
                    $"WHERE PaymentID = {TextBox1.Text}"

            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            MessageBox.Show("Payment updated successfully.")
            RefreshPayments()

            TextBox1.Clear()
            TextBox2.Clear()
            TextBox3.Clear()
            TextBox4.Clear()
            TextBox5.Clear()
            TextBox6.Clear()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    ' DELETE PAYMENT
    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Try
            If String.IsNullOrWhiteSpace(TextBox1.Text) Then
                MessageBox.Show("Please enter the PaymentID to delete.")
                Exit Sub
            End If

            conn.Open()
            query = $"DELETE FROM payments WHERE PaymentID = {TextBox1.Text}"
            cmd = New MySqlCommand(query, conn)
            Dim rowsAffected = cmd.ExecuteNonQuery()

            If rowsAffected > 0 Then
                MessageBox.Show("Payment deleted successfully.")
            Else
                MessageBox.Show("No payment found with that ID.")
            End If

            RefreshPayments()

            TextBox1.Clear()
            TextBox2.Clear()
            TextBox3.Clear()
            TextBox4.Clear()
            TextBox5.Clear()
            TextBox6.Clear()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

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
        Form8.Show()
        Me.Hide()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Form9.Show()
        Me.Hide()
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Form11.Show()
        Me.Hide()
    End Sub
End Class

