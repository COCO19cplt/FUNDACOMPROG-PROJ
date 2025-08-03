Imports MySql.Data.MySqlClient

Public Class Staffform
    Private connectionString As String = "server=localhost;user id=root;password=;database=LoadLog;"

    Private Sub Staffform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadOrders()
        LoadCustomers()
        LoadItems()
        LoadServices()
        LoadPayments()
        LoadItemStatuses()
        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("Pending")
        ComboBox1.Items.Add("Preparing")
        ComboBox1.Items.Add("Ready to pick up")
        ComboBox1.Items.Add("Completed")


    End Sub


    '=== TAB 1: Orders ===
    Private Sub LoadOrders()
        Dim dt As New DataTable()
        Using da As New MySqlDataAdapter("SELECT * FROM orders", connectionString)
            da.Fill(dt)
        End Using
        DataGridView1.DataSource = dt
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click 'Add
        ' Add order code here (show modal form for input)
        ' Example:
        ' Using f As New AddOrderForm()
        '     If f.ShowDialog() = DialogResult.OK Then LoadOrders()
        ' End Using
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click 'Update
        If DataGridView1.SelectedRows.Count = 0 Then Exit Sub
        Dim orderId As String = DataGridView1.SelectedRows(0).Cells("OrderID").Value.ToString()
        ' Update order code here (show modal form and pass orderId)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click 'Delete
        If DataGridView1.SelectedRows.Count = 0 Then Exit Sub
        Dim orderId As String = DataGridView1.SelectedRows(0).Cells("OrderID").Value.ToString()
        Using conn As New MySqlConnection(connectionString)
            conn.Open()
            Using cmd As New MySqlCommand($"DELETE FROM orders WHERE OrderID='{orderId}'", conn)
                cmd.ExecuteNonQuery()
            End Using
        End Using
        LoadOrders()
    End Sub

    '=== TAB 2: Customers ===
    Private Sub LoadCustomers()
        Dim dt As New DataTable()
        Using da As New MySqlDataAdapter("SELECT * FROM customers", connectionString)
            da.Fill(dt)
        End Using
        DataGridView2.DataSource = dt
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click 'Add
        ' Add customer code here
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click 'Update
        If DataGridView2.SelectedRows.Count = 0 Then Exit Sub
        Dim customerId As String = DataGridView2.SelectedRows(0).Cells("CustomerID").Value.ToString()
        ' Update customer code here
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click 'Delete
        If DataGridView2.SelectedRows.Count = 0 Then Exit Sub
        Dim customerId As String = DataGridView2.SelectedRows(0).Cells("CustomerID").Value.ToString()
        Using conn As New MySqlConnection(connectionString)
            conn.Open()
            Using cmd As New MySqlCommand($"DELETE FROM customers WHERE CustomerID='{customerId}'", conn)
                cmd.ExecuteNonQuery()
            End Using
        End Using
        LoadCustomers()
    End Sub

    '=== TAB 3: Items ===
    Private Sub LoadItems()
        Dim dt As New DataTable()
        Using da As New MySqlDataAdapter("SELECT * FROM order_items", connectionString)
            da.Fill(dt)
        End Using
        DataGridView3.DataSource = dt
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click 'Add
        ' Add item code here
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click 'Update
        If DataGridView3.SelectedRows.Count = 0 Then Exit Sub
        Dim itemId As String = DataGridView3.SelectedRows(0).Cells("ItemID").Value.ToString()
        ' Update item code here
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click 'Delete
        If DataGridView3.SelectedRows.Count = 0 Then Exit Sub
        Dim itemId As String = DataGridView3.SelectedRows(0).Cells("ItemID").Value.ToString()
        Using conn As New MySqlConnection(connectionString)
            conn.Open()
            Using cmd As New MySqlCommand($"DELETE FROM order_items WHERE ItemID='{itemId}'", conn)
                cmd.ExecuteNonQuery()
            End Using
        End Using
        LoadItems()
    End Sub

    '=== TAB 4: Services ===
    Private Sub LoadServices()
        Dim dt As New DataTable()
        Using da As New MySqlDataAdapter("SELECT * FROM services", connectionString)
            da.Fill(dt)
        End Using
        DataGridView4.DataSource = dt
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click 'Add
        ' Add service code here
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click 'Update
        If DataGridView4.SelectedRows.Count = 0 Then Exit Sub
        Dim serviceId As String = DataGridView4.SelectedRows(0).Cells("ServiceID").Value.ToString()
        ' Update service code here
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click 'Delete
        If DataGridView4.SelectedRows.Count = 0 Then Exit Sub
        Dim serviceId As String = DataGridView4.SelectedRows(0).Cells("ServiceID").Value.ToString()
        Using conn As New MySqlConnection(connectionString)
            conn.Open()
            Using cmd As New MySqlCommand($"DELETE FROM services WHERE ServiceID='{serviceId}'", conn)
                cmd.ExecuteNonQuery()
            End Using
        End Using
        LoadServices()
    End Sub

    '=== TAB 5: Payments ===
    Private Sub LoadPayments()
        Dim dt As New DataTable()
        Using da As New MySqlDataAdapter("SELECT * FROM payments", connectionString)
            da.Fill(dt)
        End Using
        DataGridView5.DataSource = dt
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click 'Add
        ' Add payment code here
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click 'Update
        If DataGridView5.SelectedRows.Count = 0 Then Exit Sub
        Dim paymentId As String = DataGridView5.SelectedRows(0).Cells("PaymentID").Value.ToString()
        ' Update payment code here
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click 'Delete
        If DataGridView5.SelectedRows.Count = 0 Then Exit Sub
        Dim paymentId As String = DataGridView5.SelectedRows(0).Cells("PaymentID").Value.ToString()
        Using conn As New MySqlConnection(connectionString)
            conn.Open()
            Using cmd As New MySqlCommand($"DELETE FROM payments WHERE PaymentID='{paymentId}'", conn)
                cmd.ExecuteNonQuery()
            End Using
        End Using
        LoadPayments()
    End Sub

    '=== TAB 6: Status Update ===
    Private Sub LoadItemStatuses()
        Dim dt As New DataTable()
        Using da As New MySqlDataAdapter("SELECT ItemID, OrderID, ServiceID, Quantity, ItemStatus FROM order_items", connectionString)
            da.Fill(dt)
        End Using
        DataGridView6.DataSource = dt
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click 'Update status
        If DataGridView6.SelectedRows.Count = 0 Or ComboBox1.SelectedIndex = -1 Then
            MessageBox.Show("Select an item and choose a status.")
            Exit Sub
        End If
        Dim itemId As String = DataGridView6.SelectedRows(0).Cells("ItemID").Value.ToString()
        Dim newStatus As String = ComboBox1.SelectedItem.ToString()
        Using conn As New MySqlConnection(connectionString)
            conn.Open()
            Using cmd As New MySqlCommand($"UPDATE order_items SET ItemStatus='{newStatus}' WHERE ItemID='{itemId}'", conn)
                cmd.ExecuteNonQuery()
            End Using
        End Using
        MessageBox.Show("Item status updated!")
        LoadItemStatuses()
    End Sub

    Private Sub TabPage3_Click(sender As Object, e As EventArgs) Handles TabPage3.Click

    End Sub

    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click

    End Sub
End Class