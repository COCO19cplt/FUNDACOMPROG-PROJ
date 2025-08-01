Imports MySql.Data.MySqlClient

Public Class Form14
    Private connectionString As String = "server=localhost;user id=root;password=;database=LoadLog;"
    Private conn As New MySqlConnection(connectionString)
    Private cmd As MySqlCommand
    Private query As String

    ' Make sure these labels exist on your form!oh
    ' LabelTotalAmount, LabelAmountPaid, LabelOutstanding

    Private Sub RefreshData()
        DataGridView1.Rows.Clear()
        Try
            If conn.State <> ConnectionState.Open Then
                conn.Open()
            End If
            query = "SELECT o.OrderID, GROUP_CONCAT(s.ServiceName SEPARATOR ', ') AS Services, o.TotalAmount, o.OrderStatus, o.OrderDate " &
                    "FROM orders o " &
                    "JOIN order_items oi ON o.OrderID = oi.OrderID " &
                    "JOIN services s ON oi.ServiceID = s.ServiceID " &
                    "WHERE o.OrderStatus = 'PENDING' " &
                    "GROUP BY o.OrderID, o.TotalAmount, o.OrderStatus, o.OrderDate;"
            cmd = New MySqlCommand(query, conn)
            Dim rdr As MySqlDataReader = cmd.ExecuteReader()
            While rdr.Read()
                DataGridView1.Rows.Add(rdr("OrderID").ToString(), rdr("Services").ToString(), rdr("TotalAmount").ToString(), rdr("OrderStatus").ToString(), rdr("OrderDate").ToString())
            End While
            rdr.Close()
        Catch ex As Exception
            MessageBox.Show("Error loading orders: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
        ResetLabels()
    End Sub

    Private Sub ResetLabels()
        Label1.Text = "Order Total: ₱0"
        Label2.Text = "Amount Paid: ₱0"
        Label3.Text = "Outstanding: ₱0"
        TextBox1.Text = ""
    End Sub

    Private Sub Form14_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.Rows.Clear()
        DataGridView1.Columns.Clear()
        ComboBox2.Items.Clear()

        DataGridView1.Columns.Add("OrderID", "Order ID")
        DataGridView1.Columns.Add("Services", "Services")
        DataGridView1.Columns.Add("TotalAmount", "Total Amount")
        DataGridView1.Columns.Add("OrderStatus", "Status")
        DataGridView1.Columns.Add("OrderDate", "Date")

        RefreshData()

        ComboBox2.Items.Add("Cash")
        ComboBox2.Items.Add("GCash")
        ComboBox2.Items.Add("Card")
        DataGridView1.ReadOnly = True
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        ResetLabels()
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        ' FIX: Only process if a valid, non-blank row is selected
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim row As DataGridViewRow = DataGridView1.SelectedRows(0)
            ' Make sure the row is not a new row or blank row
            If row.IsNewRow OrElse row.Cells("OrderID").Value Is Nothing OrElse row.Cells("OrderID").Value.ToString() = "" Then
                ResetLabels()
                Exit Sub
            End If

            Dim orderId As String = row.Cells("OrderID").Value.ToString()
            Dim totalAmount As Decimal
            If Not Decimal.TryParse(row.Cells("TotalAmount").Value.ToString(), totalAmount) Then totalAmount = 0D
            Dim amountPaid As Decimal = 0D

            Try
                If conn.State <> ConnectionState.Open Then
                    conn.Open()
                End If
                query = $"SELECT IFNULL(SUM(Amount),0) FROM payments WHERE OrderID='{orderId}';"
                cmd = New MySqlCommand(query, conn)
                amountPaid = Convert.ToDecimal(cmd.ExecuteScalar())
            Catch ex As Exception
                amountPaid = 0D
            Finally
                If conn.State = ConnectionState.Open Then conn.Close()
            End Try

            Dim outstanding As Decimal = totalAmount - amountPaid
            If outstanding < 0 Then outstanding = 0D

            Label1.Text = "Order Total: ₱" & totalAmount.ToString("F2")
            Label2.Text = "Amount Paid: ₱" & amountPaid.ToString("F2")
            Label3.Text = "Outstanding: ₱" & outstanding.ToString("F2")

            ' Suggest outstanding amount as default, but only if positive
            If outstanding > 0 Then
                TextBox1.Text = outstanding.ToString("F2")
            Else
                TextBox1.Text = ""
            End If
        Else
            ResetLabels()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If DataGridView1.SelectedRows.Count = 0 OrElse ComboBox2.SelectedIndex = -1 OrElse String.IsNullOrWhiteSpace(TextBox1.Text) Then
                MessageBox.Show("Please select an order, payment method, and enter amount.")
                Exit Sub
            End If

            Dim row As DataGridViewRow = DataGridView1.SelectedRows(0)
            If row.IsNewRow OrElse row.Cells("OrderID").Value Is Nothing OrElse row.Cells("OrderID").Value.ToString() = "" Then
                MessageBox.Show("Please select a valid order.")
                Exit Sub
            End If

            Dim orderId As String = row.Cells("OrderID").Value.ToString().Replace("'", "''")
            Dim paymentMethod As String = ComboBox2.SelectedItem.ToString().Replace("'", "''")
            Dim amountToPay As Decimal
            If Not Decimal.TryParse(TextBox1.Text, amountToPay) Then
                MessageBox.Show("Enter a valid payment amount.")
                Exit Sub
            End If

            Dim totalAmount As Decimal
            If Not Decimal.TryParse(row.Cells("TotalAmount").Value.ToString(), totalAmount) Then totalAmount = 0D
            Dim amountPaid As Decimal = 0D
            If conn.State <> ConnectionState.Open Then
                conn.Open()
            End If
            query = $"SELECT IFNULL(SUM(Amount),0) FROM payments WHERE OrderID='{orderId}';"
            cmd = New MySqlCommand(query, conn)
            amountPaid = Convert.ToDecimal(cmd.ExecuteScalar())
            Dim outstanding As Decimal = totalAmount - amountPaid
            If outstanding < 0 Then outstanding = 0D

            If amountToPay <= 0 OrElse amountToPay > outstanding Then
                MessageBox.Show("Payment must be more than 0 and not exceed the outstanding balance.")
                conn.Close()
                Exit Sub
            End If

            query = $"INSERT INTO payments (OrderID, PaymentDate, PaymentMethod, Amount, PaymentStatus) " &
                    $"VALUES ('{orderId}', NOW(), '{paymentMethod}', '{amountToPay}', 'PENDING');"
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            query = $"SELECT IFNULL(SUM(Amount),0) FROM payments WHERE OrderID='{orderId}';"
            cmd = New MySqlCommand(query, conn)
            Dim totalPaidAfter As Decimal = Convert.ToDecimal(cmd.ExecuteScalar())

            Dim newPaymentStatus As String
            If totalPaidAfter >= totalAmount Then
                newPaymentStatus = "PAID"
            ElseIf totalPaidAfter > 0 Then
                newPaymentStatus = "PARTIAL"
            Else
                newPaymentStatus = "PENDING"
            End If

            query = $"UPDATE payments SET PaymentStatus='{newPaymentStatus}' WHERE PaymentID = (SELECT PaymentID FROM (SELECT PaymentID FROM payments WHERE OrderID='{orderId}' ORDER BY PaymentID DESC LIMIT 1) AS sub);"
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            MessageBox.Show("Payment recorded successfully.")

            RefreshData()
            ComboBox2.SelectedIndex = -1
            TextBox1.Clear()
            If conn.State = ConnectionState.Open Then conn.Close()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

End Class