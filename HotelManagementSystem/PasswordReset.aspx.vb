Imports System.Data.SqlClient

Public Class PasswordReset
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Using con As New SqlConnection(CnString)
            Using cmd As New SqlCommand(" SELECT  *   FROM    UserInformation  WHERE   (Email = @Email) AND (ResetCode = @ResetCode) AND (UseResetCode = 'N') ")
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text)
                cmd.Parameters.AddWithValue("@ResetCode", txtResetCode.Text)
                cmd.Connection = con
                con.Open()
                Dim USER_ID As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                con.Close()
                If USER_ID = 0 Then
                    lblMessege.Text = "Invalid password reset code."
                Else
                    '---------------------------------------
                    Using con2 As New SqlConnection(CnString)
                        Using cmd2 As New SqlCommand(" UPDATE UserInformation   SET   Password=@Password, ResetCode='0', UseResetCode='Y'  WHERE    (USER_ID = @USR_ID) ")
                            cmd2.Parameters.AddWithValue("@USR_ID", USER_ID)
                            cmd2.Parameters.AddWithValue("@Password", Encrypt(txtPassword.Text))
                            cmd2.Connection = con2
                            con2.Open()
                            Dim rRES_ID As Integer = Convert.ToInt32(cmd2.ExecuteScalar())
                            con2.Close()
                            If rRES_ID = 0 Then
                                ClientScript.RegisterStartupScript(Me.[GetType](), "myalert", "alert('Password Reset Successfully.');", True)
                                Response.Redirect("Default.aspx")
                            Else
                                ClientScript.RegisterStartupScript(Me.[GetType](), "myalert", "alert('An error occurred.');", True)
                                Response.Redirect("PasswordReset.aspx")
                            End If
                        End Using
                    End Using
                    '---------------------------------------
                End If
            End Using
        End Using
    End Sub
End Class