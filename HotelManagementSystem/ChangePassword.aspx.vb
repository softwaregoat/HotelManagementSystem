Imports System.Data.SqlClient
Imports System.IO
Public Class ChangePassword
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
            If (Not (Session("FlushMessage")) Is Nothing) Then
                lblMessege.Text = Session("FlushMessage").ToString()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "showModal();", True)
                Session("FlushMessage") = Nothing
            End If
        End If
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim userId As Integer = 0
        Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("SELECT   USER_ID  FROM    UserInformation  WHERE        (USER_ID = @USER_ID) AND (Password = @Password)")
                cmd.Parameters.AddWithValue("@USER_ID", ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)))
                cmd.Parameters.AddWithValue("@Password ", Encrypt(txtCurrentPassword.Text))
                cmd.Connection = con
                con.Open()
                userId = Convert.ToInt32(cmd.ExecuteScalar())
                con.Close()
            End Using
        End Using
        If userId = 0 Then
            Session("FlushMessage") = "Sorry! Current password does not match."
            Response.Redirect("ChangePassword.aspx")
        Else
            Using con As New SqlConnection(CnString)
                Using cmd As New SqlCommand("UPDATE   UserInformation  SET  Password=@Password  WHERE   (USER_ID = " & ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)) & ") ")
                    cmd.Parameters.AddWithValue("@Password", Encrypt(txtNewPassword.Text))
                    cmd.Connection = con
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                End Using
            End Using
            Session("FlushMessage") = "Password updated successful."
            Response.Redirect("ChangePassword.aspx")
        End If
    End Sub
End Class