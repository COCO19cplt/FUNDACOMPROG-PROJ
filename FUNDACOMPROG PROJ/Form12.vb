Imports MySql.Data.MySqlClient

Public Class Form12

    Private connectionString As String = "server=localhost;user id=root;password=;database=LoadLog;"
    Private conn As New MySqlConnection(connectionString)
    Private cmd As MySqlCommand
    Private da As MySqlDataAdapter
    Private ds As DataSet
    Private query As String
    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            ' 1. Validate user input
            If String.IsNullOrWhiteSpace(TextBox1.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox4.Text) Then
                MessageBox.Show("Please fill in all required fields (Name, Phone, Email).")
                Exit Sub
            End If

            ' 2. Open the connection
            conn.Open()

            ' 3. Build your INSERT query (NO AddWithValue, direct concatenation)
            Dim query As String = "INSERT INTO customers (CustomerName, PhoneNo, HomeAddress, Email) VALUES (" &
            "'" & TextBox1.Text.Replace("'", "''") & "', " &
            "'" & TextBox2.Text.Replace("'", "''") & "', " &
            "'" & TextBox3.Text.Replace("'", "''") & "', " &
            "'" & TextBox4.Text.Replace("'", "''") & "')"

            ' 4. Create and execute the command
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            ' 5. Retrieve the new CustomerID
            cmd = New MySqlCommand("SELECT LAST_INSERT_ID();", conn)
            Dim newCustomerID As Integer = Convert.ToInt32(cmd.ExecuteScalar())

            MessageBox.Show("Customer registered successfully.")

            ' 6. Clear fields
            TextBox1.Clear()
            TextBox2.Clear()
            TextBox3.Clear()
            TextBox4.Clear()

            ' 7. Open Form13 and pass the new CustomerID
            Dim frm13 As New Form13()
            frm13.CurrentCustomerID = newCustomerID
            frm13.Show()
            Me.Hide()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub


    ' --- Inside Form12 after inserting the customer: ---



    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Form13.Show()
    End Sub


    ' When the form loads, populate the customer grid


    ' Fetch all customers into DataGridView1



    ' Button2: Add new customer from TextBox2–TextBox5

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        TextBox5.Show()

        Try
            ' 1. Validate input
            If String.IsNullOrWhiteSpace(TextBox5.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox1.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
               String.IsNullOrWhiteSpace(TextBox4.Text) Then
                MessageBox.Show("Please enter CustomerID and all updated details.")
                Exit Sub
            End If

            ' 2. Open connection
            conn.Open()

            ' 3. Build the UPDATE query
            query = "UPDATE customers SET " &
                    "CustomerName = '" & TextBox1.Text.Replace("'", "''") & "', " &
                    "PhoneNo = '" & TextBox2.Text.Replace("'", "''") & "', " &
                    "HomeAddress = '" & TextBox3.Text.Replace("'", "''") & "', " &
                    "Email = '" & TextBox4.Text.Replace("'", "''") & "' " &
                    "WHERE CustomerID = " & TextBox5.Text

            ' 4. Execute
            cmd = New MySqlCommand(query, conn)
            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

            If rowsAffected > 0 Then
                MessageBox.Show("Customer updated successfully.")
            Else
                MessageBox.Show("No customer found with the provided ID.")
            End If

            ' 5. Clear inputs
            TextBox1.Clear()
            TextBox2.Clear()
            TextBox3.Clear()
            TextBox4.Clear()
            TextBox5.Clear()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub Form12_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        Form13.Show()
        Me.Hide()
    End Sub
End Class




