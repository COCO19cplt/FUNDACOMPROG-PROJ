Imports MySql.Data.MySqlClient

Public Class Form14
    Private connectionString As String = "server=localhost;user id=root;password=;database=LoadLog;"
    Private conn As New MySqlConnection(connectionString)
    Private cmd As MySqlCommand
    Private query As String
    ' Add this line at the top of Form14, after Public Class Form14
    Public Property CurrentOrderID As Integer
    Public Property LastPaidOrderID As Integer = 0

    ' Helper to refresh orders grid


    Private Sub RefreshData()

        Try
            Dim dt As New DataTable()
            Dim query As String = "SELECT o.OrderID, GROUP_CONCAT(s.ServiceName SEPARATOR ', ') AS Services, o.TotalAmount, o.OrderStatus, o.OrderDate " &
                              "FROM orders o " &
                              "JOIN order_items oi ON o.OrderID = oi.OrderID " &
                              "JOIN services s ON oi.ServiceID = s.ServiceID " &
                              "WHERE o.OrderStatus IN ('PENDING','PARTIAL') " &
                              "GROUP BY o.OrderID, o.TotalAmount, o.OrderStatus, o.OrderDate;"
            Using conn As New MySqlConnection(connectionString)
                Using da As New MySqlDataAdapter(query, conn)
                    da.Fill(dt)
                End Using
            End Using

            DataGridView1.Rows.Clear()
            For Each row As DataRow In dt.Rows
                DataGridView1.Rows.Add(row("OrderID").ToString(),
                                   row("Services").ToString(),
                                   row("TotalAmount").ToString(),
                                   row("OrderStatus").ToString(),
                                   row("OrderDate").ToString())
            Next

        Catch ex As Exception
            MessageBox.Show("Error loading orders: " & ex.Message)
        End Try
    End Sub



    ' Show remaining balance for selected order
    Private Sub ShowRemainingBalance(orderId As String)
        If String.IsNullOrWhiteSpace(orderId) Then
            Label1.Text = "Remaining Balance: ₱0.00"
            Return
        End If
        Try
            If conn.State <> ConnectionState.Open Then conn.Open()
            Dim totalPaidQuery As String = $"SELECT IFNULL(SUM(Amount),0) FROM payments WHERE OrderID='{orderId}';"
            cmd = New MySqlCommand(totalPaidQuery, conn)
            Dim totalPaid As Decimal = Convert.ToDecimal(cmd.ExecuteScalar())

            Dim totalOrderQuery As String = $"SELECT TotalAmount FROM orders WHERE OrderID='{orderId}';"
            cmd = New MySqlCommand(totalOrderQuery, conn)
            Dim orderTotal As Decimal = Convert.ToDecimal(cmd.ExecuteScalar())

            Dim remaining As Decimal = orderTotal - totalPaid
            Label1.Text = $"Remaining Balance: ₱{remaining:0.00}"

            ' Optionally disable payment controls if fully paid
            Button1.Enabled = remaining > 0
            TextBox1.Enabled = remaining > 0
            ComboBox1.Enabled = remaining > 0
        Catch ex As Exception
            Label1.Text = "Remaining Balance: ₱0.00"
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub Form14_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.Rows.Clear()
        DataGridView1.Columns.Clear()
        ComboBox1.Items.Clear()

        DataGridView1.Columns.Add("OrderID", "Order ID")
        DataGridView1.Columns.Add("Services", "Services")
        DataGridView1.Columns.Add("TotalAmount", "Total Amount")
        DataGridView1.Columns.Add("OrderStatus", "Status")
        DataGridView1.Columns.Add("OrderDate", "Date")

        RefreshData()

        ComboBox1.Items.Add("Cash")
        ComboBox1.Items.Add("GCash")
        ComboBox1.Items.Add("Card")
        DataGridView1.ReadOnly = True
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        Label1.Text = "Remaining Balance: ₱0.00"
    End Sub

    ' When an order is selected, show remaining balance
    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim orderId As String = DataGridView1.SelectedRows(0).Cells("OrderID").Value.ToString()
            ShowRemainingBalance(orderId)
        Else
            Label1.Text = "Remaining Balance: ₱0.00"
        End If
    End Sub

    ' Confirm Payment
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If DataGridView1.SelectedRows.Count = 0 OrElse ComboBox1.SelectedIndex = -1 OrElse String.IsNullOrWhiteSpace(TextBox1.Text) Then
            MessageBox.Show("Please select an order, payment method, and enter amount.")
            Exit Sub
        End If

        Dim orderId As String = DataGridView1.SelectedRows(0).Cells("OrderID").Value.ToString().Replace("'", "''")
        Dim paymentMethod As String = ComboBox1.SelectedItem.ToString().Replace("'", "''")
        Dim amount As Decimal
        If Not Decimal.TryParse(TextBox1.Text, amount) OrElse amount <= 0 Then
            MessageBox.Show("Enter a valid payment amount.")
            Exit Sub
        End If

        Try
            If conn.State <> ConnectionState.Open Then conn.Open()

            ' Get total paid and order total BEFORE inserting payment
            Dim totalPaidQuery As String = $"SELECT IFNULL(SUM(Amount),0) FROM payments WHERE OrderID='{orderId}';"
            cmd = New MySqlCommand(totalPaidQuery, conn)
            Dim totalPaid As Decimal = Convert.ToDecimal(cmd.ExecuteScalar())

            Dim totalOrderQuery As String = $"SELECT TotalAmount FROM orders WHERE OrderID='{orderId}';"
            cmd = New MySqlCommand(totalOrderQuery, conn)
            Dim orderTotal As Decimal = Convert.ToDecimal(cmd.ExecuteScalar())

            Dim remaining As Decimal = orderTotal - totalPaid
            If amount > remaining Then
                MessageBox.Show("Payment exceeds remaining balance!")
                Exit Sub
            End If

            ' Insert payment
            query = $"INSERT INTO payments (OrderID, PaymentDate, PaymentMethod, Amount, PaymentStatus) " &
                $"VALUES ('{orderId}', NOW(), '{paymentMethod}', {amount}, 'PENDING');"
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            ' Recalculate after payment
            totalPaid += amount
            remaining = orderTotal - totalPaid

            ' Determine new status
            Dim newPaymentStatus As String
            If totalPaid >= orderTotal Then
                newPaymentStatus = "PAID"
            ElseIf totalPaid > 0 Then
                newPaymentStatus = "PARTIAL"
            Else
                newPaymentStatus = "PENDING"
            End If

            ' Update latest payment status
            query = $"UPDATE payments SET PaymentStatus='{newPaymentStatus}' " &
                $"WHERE PaymentID = (SELECT PaymentID FROM (SELECT PaymentID FROM payments WHERE OrderID='{orderId}' ORDER BY PaymentID DESC LIMIT 1) AS sub);"
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            ' Update order status
            query = $"UPDATE orders SET OrderStatus='{newPaymentStatus}' WHERE OrderID='{orderId}';"
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            MessageBox.Show("Payment recorded successfully.")
            ' After payment is recorded and before RefreshData()
            LastPaidOrderID = Convert.ToInt32(orderId)
            RefreshData()
            ShowRemainingBalance(orderId)
            ComboBox1.SelectedIndex = -1
            TextBox1.Clear()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    ' Delete order (and all data)
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select an order to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim orderId As String = DataGridView1.SelectedRows(0).Cells("OrderID").Value.ToString()
        If MessageBox.Show($"Are you sure you want to delete Order #{orderId} and all its data?",
                       "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        Try
            If conn.State <> ConnectionState.Open Then conn.Open()
            Using tx As MySqlTransaction = conn.BeginTransaction()
                cmd = conn.CreateCommand()
                cmd.Transaction = tx
                cmd.CommandText = $"DELETE FROM payments    WHERE OrderID = '{orderId}';"
                cmd.ExecuteNonQuery()
                cmd.CommandText = $"DELETE FROM order_items WHERE OrderID = '{orderId}';"
                cmd.ExecuteNonQuery()
                cmd.CommandText = $"DELETE FROM orders      WHERE OrderID = '{orderId}';"
                cmd.ExecuteNonQuery()
                tx.Commit()
            End Using

            MessageBox.Show($"Order #{orderId} deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information)
            RefreshData()
            Label1.Text = "Remaining Balance: ₱0.00"
        Catch ex As Exception
            MessageBox.Show("Error deleting order: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        If LastPaidOrderID = 0 Then
            MessageBox.Show("No recent paid order to view.")
            Exit Sub
        End If

        Dim waitingRoomForm As New WAITING_ROOM()
        waitingRoomForm.CurrentOrderID = LastPaidOrderID
        waitingRoomForm.Show()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

End Class