Imports MySql.Data.MySqlClient

Public Class Form15
    Dim conn As New MySqlConnection("server=localhost;user id=root;password=;database=loadlog")

    Private Sub Form15_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadInventory()
        LoadUsers()
        LoadUnits()
    End Sub

    Private Sub LoadInventory()
        Try
            Dim cmd As New MySqlCommand("SELECT * FROM inventory", conn)
            Dim adapter As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            adapter.Fill(dt)
            DataGridView1.DataSource = dt
        Catch ex As Exception
            MessageBox.Show("Error loading inventory: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadUsers()
        Try
            Dim cmd As New MySqlCommand("SELECT UserID FROM users", conn)
            Dim adapter As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            adapter.Fill(dt)
            ComboBox2.DataSource = dt
            ComboBox2.DisplayMember = "UserID"
            ComboBox2.ValueMember = "UserID"
        Catch ex As Exception
            MessageBox.Show("Error loading users: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadUnits()
        ComboBox1.Items.Clear()
        ComboBox1.Items.AddRange(New String() {"pcs", "kg", "liters", "packs"})
        ComboBox1.SelectedIndex = 0
    End Sub

    ' INSERT
    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Dim itemName As String = TextBox1.Text.Trim()
        Dim quantity As Integer = NumericUpDown1.Value
        Dim unit As String = ComboBox1.Text.Trim()
        Dim lastRestocked As String = DateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss")
        Dim costPerUnitStr As String = TextBox3.Text.Trim()
        Dim userID As String = ComboBox2.Text.Trim()

        If itemName = "" Then
            MessageBox.Show("Item Name is required.")
            Return
        ElseIf unit = "" Then
            MessageBox.Show("Unit is required.")
            Return
        ElseIf userID = "" Then
            MessageBox.Show("UserID is required.")
            Return
        End If

        Dim restockedPart As String = If(DateTimePicker1.Checked, "'" & lastRestocked & "'", "NULL")
        Dim costPart As String = If(costPerUnitStr = "", "NULL", costPerUnitStr)

        Dim query As String = "INSERT INTO inventory (ItemName, Quantity, Unit, LastRestocked, CostPerUnit, UserID) VALUES (" &
                              "'" & itemName.Replace("'", "''") & "', " & quantity & ", '" & unit.Replace("'", "''") & "', " &
                              restockedPart & ", " & costPart & ", " & userID & ")"

        Try
            Dim cmd As New MySqlCommand(query, conn)
            conn.Open()
            cmd.ExecuteNonQuery()
            conn.Close()
            MessageBox.Show("Item inserted.")
            LoadInventory()
        Catch ex As Exception
            MessageBox.Show("Insert error: " & ex.Message)
            conn.Close()
        End Try
    End Sub

    ' UPDATE
    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Dim itemID As String = TextBox4.Text.Trim()
        Dim itemName As String = TextBox1.Text.Trim()
        Dim quantity As Integer = NumericUpDown1.Value
        Dim unit As String = ComboBox1.Text.Trim()
        Dim lastRestocked As String = DateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss")
        Dim costPerUnitStr As String = TextBox3.Text.Trim()
        Dim userID As String = ComboBox2.Text.Trim()

        If itemID = "" Then
            MessageBox.Show("Enter ItemID to update.")
            Return
        End If

        Dim restockedPart As String = If(DateTimePicker1.Checked, "'" & lastRestocked & "'", "NULL")
        Dim costPart As String = If(costPerUnitStr = "", "NULL", costPerUnitStr)

        Dim query As String = "UPDATE inventory SET " &
                              "ItemName = '" & itemName.Replace("'", "''") & "', " &
                              "Quantity = " & quantity & ", " &
                              "Unit = '" & unit.Replace("'", "''") & "', " &
                              "LastRestocked = " & restockedPart & ", " &
                              "CostPerUnit = " & costPart & ", " &
                              "UserID = " & userID & " " &
                              "WHERE ItemID = " & itemID

        Try
            Dim cmd As New MySqlCommand(query, conn)
            conn.Open()
            Dim rows = cmd.ExecuteNonQuery()
            conn.Close()
            If rows > 0 Then
                MessageBox.Show("Item updated.")
                LoadInventory()
            Else
                MessageBox.Show("ItemID not found.")
            End If
        Catch ex As Exception
            MessageBox.Show("Update error: " & ex.Message)
            conn.Close()
        End Try
    End Sub

    ' DELETE
    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Dim itemID As String = TextBox4.Text.Trim()

        If itemID = "" Then
            MessageBox.Show("Enter ItemID to delete.")
            Return
        End If

        Dim query As String = "DELETE FROM inventory WHERE ItemID = " & itemID

        Try
            Dim cmd As New MySqlCommand(query, conn)
            conn.Open()
            Dim rows = cmd.ExecuteNonQuery()
            conn.Close()
            If rows > 0 Then
                MessageBox.Show("Item deleted.")
                LoadInventory()
            Else
                MessageBox.Show("ItemID not found.")
            End If
        Catch ex As Exception
            MessageBox.Show("Delete error: " & ex.Message)
            conn.Close()
        End Try
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub
End Class
