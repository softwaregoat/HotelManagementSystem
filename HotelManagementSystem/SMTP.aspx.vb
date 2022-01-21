Imports System.Data.SqlClient
Imports System.IO

Public Class SMTP
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

            Dim dt As DataTable = QueryDataTable("SELECT * FROM SMTP_Info")
            For Each row As DataRow In dt.Rows
                txtHost.Text = row("Host").ToString()
                txtPort.Text = row("Port").ToString()
                txtUserName.Text = row("UserName").ToString
                txtPassword.Attributes("type") = "text"
                txtPassword.Text = row("Password").ToString
                txtPassword.Attributes("type") = "Password"
            Next

            If (Not (Session("FlushMessage")) Is Nothing) Then
                lblMessege.Text = Session("FlushMessage").ToString()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "showModal();", True)
                Session("FlushMessage") = Nothing
            End If

        End If
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim dt As DataTable = QueryDataTable("SELECT * FROM SMTP_Info")
        If dt.Rows.Count > 0 Then
            ExecuteSQLQuery("UPDATE   SMTP_Info SET Host= @Host, Port= @Port, UserName=@UserName, Password=@Password")
            Session("FlushMessage") = "Data has been updated successfully."
            Response.Redirect("SMTP.aspx")
        Else
            ExecuteSQLQuery("INSERT INTO  SMTP_Info (Host , Port , UserName, Password) VALUES (@Host ,@Port ,@UserName, @Password)")
            Session("FlushMessage") = "Data has been saved successfully."
            Response.Redirect("SMTP.aspx")
        End If
    End Sub

    Private Sub ExecuteSQLQuery(ByVal sql As String)
        Using con As New SqlConnection(CnString)
            Using cmd As New SqlCommand(sql)
                cmd.Parameters.AddWithValue("@Host", txtHost.Text)
                cmd.Parameters.AddWithValue("@Port", txtPort.Text)
                cmd.Parameters.AddWithValue("@UserName", txtUserName.Text)
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text)
                cmd.Connection = con
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        End Using
    End Sub
End Class