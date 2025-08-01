Imports MySql.Data.MySqlClient

Public Class Form14
    ' 1. Declare conn properly
    Private connectionString As String = "server=localhost;user id=root;password=;database=LoadLog;"
    Private conn As New MySqlConnection(connectionString)
    Private cmd As MySqlCommand
    Private query As String

    ' Helper to refresh DataGridView
    Private Sub RefreshData()
        DataGridView1.Rows.Clear()
        Try
            conn.Open()
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
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If DataGridView1.SelectedRows.Count = 0 OrElse ComboBox2.SelectedIndex = -1 OrElse String.IsNullOrWhiteSpace(TextBox1.Text) Then
                MessageBox.Show("Please select an order, payment method, and enter amount.")
                Exit Sub
            End If

            ' Sanitize inputs
            Dim orderId As String = DataGridView1.SelectedRows(0).Cells("OrderID").Value.ToString().Replace("'", "''")
            Dim paymentMethod As String = ComboBox2.SelectedItem.ToString().Replace("'", "''")
            Dim amount As String = TextBox1.Text.Replace("'", "''")

            conn.Open()

            ' Insert payment record
            query = $"INSERT INTO payments (OrderID, PaymentDate, PaymentMethod, Amount, PaymentStatus) " &
                    $"VALUES ('{orderId}', NOW(), '{paymentMethod}', '{amount}', 'PAID');"
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            ' Calculate total paid for this order
            Dim totalPaid As Decimal = 0D
            query = $"SELECT IFNULL(SUM(Amount),0) FROM payments WHERE OrderID='{orderId}';"
            cmd = New MySqlCommand(query, conn)
            totalPaid = Convert.ToDecimal(cmd.ExecuteScalar())

            ' Get the order's total amount
            Dim orderTotal As Decimal = 0D
            query = $"SELECT TotalAmount FROM orders WHERE OrderID='{orderId}';"
            cmd = New MySqlCommand(query, conn)
            orderTotal = Convert.ToDecimal(cmd.ExecuteScalar())

            ' Decide order status
            Dim newStatus As String
            If totalPaid >= orderTotal Then
                newStatus = "PAID"
            ElseIf totalPaid > 0 Then
                newStatus = "PARTIAL"
            Else
                newStatus = "PENDING"
            End If

            ' Update order status
            query = $"UPDATE orders SET OrderStatus='{newStatus}' WHERE OrderID='{orderId}';"
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            MessageBox.Show("Payment recorded successfully.")

            ' Refresh DataGridView
            RefreshData()
            ComboBox2.SelectedIndex = -1
            TextBox1.Clear()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim row As DataGridViewRow = DataGridView1.SelectedRows(0)
            TextBox1.Text = row.Cells("TotalAmount").Value.ToString()
        End If
    End Sub

    ' Optional: update amount box when a row is selected (even by keyboard)
    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim row As DataGridViewRow = DataGridView1.SelectedRows(0)
            TextBox1.Text = row.Cells("TotalAmount").Value.ToString()
        End If
    End Sub

End Class