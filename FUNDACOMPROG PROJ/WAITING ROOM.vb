Imports MySql.Data.MySqlClient

Public Class WAITING_ROOM
    Private connectionString As String = "server=localhost;user id=root;password=;database=LoadLog;"
    Private conn As New MySqlConnection(connectionString)

    Public Property CurrentOrderID As Integer = 0
    Public Property CurrentCustomerID As Integer = 0

    Private Sub WAITING_ROOM_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = "Thank you for availing our services!"
        ShowOrderInfo()
    End Sub

    Private Sub ShowOrderInfo()
        If CurrentOrderID = 0 Then
            MessageBox.Show("No order selected. Please go back and select an order.")
            Label2.Text = ""
            Label3.Text = ""
            Label4.Text = ""
            Label5.Text = ""
            Label6.Text = ""
            Label7.Text = ""
            DataGridView1.Rows.Clear()
            Exit Sub
        End If

        Try
            If conn.State <> ConnectionState.Open Then conn.Open()

            Dim orderIdStr As String = CurrentOrderID.ToString().Replace("'", "''")
            ' Get PAYMENT STATUS (OrderStatus)
            Dim orderQuery As String = "SELECT o.OrderID, o.OrderStatus, o.TotalAmount, o.OrderDate, c.CustomerName " &
                                       "FROM orders o JOIN customers c ON o.CustomerID = c.CustomerID WHERE o.OrderID = '" & orderIdStr & "'"
            Dim orderCmd As New MySqlCommand(orderQuery, conn)
            Dim reader = orderCmd.ExecuteReader()
            If reader.Read() Then
                Label2.Text = "Order Number: " & reader("OrderID").ToString()
                Label3.Text = "Customer: " & reader("CustomerName").ToString()
                Label4.Text = "Payment Status: " & reader("OrderStatus").ToString() ' PAID, PARTIAL, PENDING
                Label5.Text = "Total: ₱" & reader("TotalAmount").ToString()
                Label6.Text = "Date: " & reader("OrderDate").ToString()
            Else
                Label2.Text = "Order not found."
                Label3.Text = ""
                Label4.Text = ""
                Label5.Text = ""
                Label6.Text = ""
            End If
            reader.Close()

            ' Get ITEM STATUS (Service Progress)
            Dim itemQuery As String = "SELECT s.ServiceName, oi.Quantity, oi.ItemStatus " &
                                      "FROM order_items oi JOIN services s ON oi.ServiceID = s.ServiceID " &
                                      "WHERE oi.OrderID = '" & orderIdStr & "'"
            Dim da As New MySqlDataAdapter(itemQuery, conn)
            Dim dt As New DataTable()
            da.Fill(dt)

            DataGridView1.Rows.Clear()
            DataGridView1.Columns.Clear()
            DataGridView1.Columns.Add("Service", "Service")
            DataGridView1.Columns.Add("Quantity", "Quantity")
            DataGridView1.Columns.Add("ItemStatus", "Item Status")

            Dim allStatuses As New List(Of String)
            For Each row As DataRow In dt.Rows
                DataGridView1.Rows.Add(row("ServiceName").ToString(), row("Quantity").ToString(), row("ItemStatus").ToString())
                allStatuses.Add(row("ItemStatus").ToString())
            Next

            ' Summary for Label7: if all items are same status, show that, else show "Mixed"
            If allStatuses.Count = 0 Then
                Label7.Text = "Progress: No items."
            ElseIf allStatuses.Distinct().Count = 1 Then
                Label7.Text = "Progress: " & allStatuses(0)
            Else
                Label7.Text = "Progress: Mixed (" & String.Join(", ", allStatuses.Distinct()) & ")"
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading order info: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ShowOrderInfo()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim serviceForm As New Form13()
        serviceForm.CurrentCustomerID = Me.CurrentCustomerID
        serviceForm.Show()
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub
End Class
