Imports System.Data.SqlClient
Imports System.IO

Public Class HotelInformation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then

            If Not Me.Page.User.Identity.IsAuthenticated Then
                FormsAuthentication.RedirectToLoginPage()
            End If
            Dim currentPageFileName As String = New FileInfo(Me.Request.Url.LocalPath).Name
            Try
                CheckPermissions(currentPageFileName.Split(".aspx")(0).ToString, ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)))
            Catch ex As Exception

            End Try

            Dim dt As DataTable = QueryDataTable("SELECT * FROM HotelInformation")
            For Each row As DataRow In dt.Rows
                txtHotelName.Text = row("HotelName").ToString()
                txtAddress.Text = row("Address").ToString()
                txtContact.Text = row("Contact").ToString
                txtTinNo.Text = row("TIN_NO").ToString
            Next

            If (Not (Session("FlushMessage")) Is Nothing) Then
                lblMessege.Text = Session("FlushMessage").ToString()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "showModal();", True)
                Session("FlushMessage") = Nothing
            End If

        End If
    End Sub
    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim dt As DataTable = QueryDataTable("SELECT * FROM HotelInformation")
        If dt.Rows.Count > 0 Then
            ExecuteSQLQuery("UPDATE   HotelInformation SET HotelName= @HotelName, Address= @Address, Contact=@Contact, TIN_NO=@TIN_NO")
            Session("FlushMessage") = "Data has been updated successfully."
            Response.Redirect("HotelInformation.aspx")
        Else
            ExecuteSQLQuery("INSERT INTO  HotelInformation (HotelName , Address , Contact, TIN_NO) VALUES (@HotelName ,@Address ,@Contact, @TIN_NO)")
            Session("FlushMessage") = "Data has been saved successfully."
            Response.Redirect("HotelInformation.aspx")
        End If
    End Sub
    Private Sub ExecuteSQLQuery(ByVal sql As String)
        Using con As New SqlConnection(CnString)
            Using cmd As New SqlCommand(sql)
                cmd.Parameters.AddWithValue("@HotelName", txtHotelName.Text)
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text)
                cmd.Parameters.AddWithValue("@Contact", txtContact.Text)
                cmd.Parameters.AddWithValue("@TIN_NO", txtTinNo.Text)
                cmd.Connection = con
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        End Using
    End Sub
End Class