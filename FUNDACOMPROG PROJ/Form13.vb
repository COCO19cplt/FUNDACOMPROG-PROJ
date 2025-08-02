Imports MySql.Data.MySqlClient
Public Class Form13
    Private connectionString As String = "server=localhost;user id=root;password=;database=LoadLog;"
    Private conn As New MySqlConnection(connectionString)
    Private cmd As MySqlCommand
    Private da As MySqlDataAdapter
    Private ds As DataSet
    Private query As String

    ' This property will be set by Form12 when launching Form13
    Public Property CurrentCustomerID As Integer

    Private Sub Form13_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupServiceGrid()
        SetupCartGrid()
        LoadServices()
    End Sub

    Private Sub SetupServiceGrid()
        DataGridView1.Columns.Clear()
        DataGridView1.Rows.Clear()
        DataGridView1.Columns.Add("ServiceID", "ServiceID")
        DataGridView1.Columns("ServiceID").Visible = False
        DataGridView1.Columns.Add("ServiceName", "Service Name")
        DataGridView1.Columns.Add("PricePerKg", "Price Per Kg")
        DataGridView1.Columns.Add("EstimatedDuration", "Duration (min)")
    End Sub

    Private Sub SetupCartGrid()
        DataGridView2.Columns.Clear()
        DataGridView2.Rows.Clear()
        DataGridView2.Columns.Add("ServiceID", "ServiceID")
        DataGridView2.Columns("ServiceID").Visible = False
        DataGridView2.Columns.Add("ServiceName", "Service Name")
        DataGridView2.Columns.Add("Quantity", "Quantity")
        DataGridView2.Columns.Add("PricePerKg", "Price Per Kg")
        DataGridView2.Columns.Add("Subtotal", "Subtotal")
        DataGridView2.Columns.Add("Description", "Description")
    End Sub

    Private Sub LoadServices()
        DataGridView1.Rows.Clear()
        conn.Open()
        Dim cmd As New MySqlCommand("SELECT ServiceID, ServiceName, PricePerKg, EstimatedDuration FROM services", conn)
        Dim reader As MySqlDataReader = cmd.ExecuteReader()
        While reader.Read()
            DataGridView1.Rows.Add(reader("ServiceID"), reader("ServiceName"), reader("PricePerKg"), reader("EstimatedDuration"))
        End While
        reader.Close()
        conn.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a service.")
            Exit Sub
        End If

        Dim serviceID = DataGridView1.SelectedRows(0).Cells("ServiceID").Value
        Dim serviceName = DataGridView1.SelectedRows(0).Cells("ServiceName").Value.ToString()
        Dim price = Convert.ToDecimal(DataGridView1.SelectedRows(0).Cells("PricePerKg").Value)
        Dim quantity = Convert.ToInt32(NumericUpDown1.Value)
        Dim description = TextBox1.Text
        Dim subtotal = price * quantity

        DataGridView2.Rows.Add(serviceID, serviceName, quantity, price, subtotal, description)
        UpdateCartTotal()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TabControl1.SelectedIndex = 1
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        TabControl1.SelectedIndex = 0
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If DataGridView2.SelectedRows.Count > 0 Then
            DataGridView2.Rows.RemoveAt(DataGridView2.SelectedRows(0).Index)
            UpdateCartTotal()
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If DataGridView2.Rows.Count = 0 Or DataGridView2.Rows.GetRowCount(DataGridViewElementStates.None) = 1 Then
            MessageBox.Show("Cart is empty. Please add at least one service.")
            Exit Sub
        End If

        Dim totalAmount As Decimal = 0
        Dim orderID As Integer = 0

        For Each row As DataGridViewRow In DataGridView2.Rows
            If Not row.IsNewRow Then
                totalAmount += Convert.ToDecimal(row.Cells("Subtotal").Value)
            End If
        Next

        Try
            conn.Open()

            ' Insert into orders table using the correct CustomerID
            query = "INSERT INTO orders (CustomerID, TotalAmount) VALUES ('" & CurrentCustomerID & "','" & totalAmount & "')"
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            ' Get the new OrderID
            cmd = New MySqlCommand("SELECT LAST_INSERT_ID();", conn)
            orderID = Convert.ToInt32(cmd.ExecuteScalar())

            ' Insert each order item
            For Each row As DataGridViewRow In DataGridView2.Rows
                If Not row.IsNewRow Then
                    Dim serviceID = row.Cells("ServiceID").Value.ToString()
                    Dim description = row.Cells("Description").Value.ToString().Replace("'", "''")
                    Dim quantity = row.Cells("Quantity").Value.ToString()
                    Dim unitPrice = row.Cells("PricePerKg").Value.ToString()
                    Dim subtotal = row.Cells("Subtotal").Value.ToString()
                    query = "INSERT INTO order_items (OrderID, ServiceID, ItemDescription, Quantity, UnitPrice, SubTotal) VALUES " &
                        "('" & orderID & "','" & serviceID & "','" & description & "','" & quantity & "','" & unitPrice & "','" & subtotal & "')"
                    cmd = New MySqlCommand(query, conn)
                    cmd.ExecuteNonQuery()
                End If
            Next

            MessageBox.Show("Order confirmed and saved to database!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            DataGridView2.Rows.Clear()
            UpdateCartTotal()
            TabControl1.SelectedIndex = 0

        Catch ex As Exception
            MessageBox.Show("Error saving order: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try

        ' Show Form14 and pass OrderID
        Dim frm14 As New Form14()
        frm14.CurrentOrderID = orderID
        frm14.Show()
    End Sub

    Private Sub UpdateCartTotal()
        Dim total As Decimal = 0
        For Each row As DataGridViewRow In DataGridView2.Rows
            If Not row.IsNewRow AndAlso row.Cells("Subtotal").Value IsNot Nothing Then
                total += Convert.ToDecimal(row.Cells("Subtotal").Value)
            End If
        Next
        Label1.Text = "Total: " & total.ToString("C2")
    End Sub

    Private Sub TabPage2_Click(sender As Object, e As EventArgs) Handles TabPage2.Click

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged

    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged

    End Sub
End Class
