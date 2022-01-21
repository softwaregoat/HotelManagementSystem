Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Mail

Public Class ForgotPassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Using con As New SqlConnection(CnString)
            Using cmd As New SqlCommand("SELECT   USER_ID  FROM     UserInformation  WHERE        (Email = @Email)")
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text)
                cmd.Connection = con
                con.Open()
                Dim USER_ID As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                con.Close()
                If USER_ID = 0 Then
                    lblMessege.Text = "Email address does not exist."
                Else
                    '----------------------------------------
                    Dim ResetCode As Double = Format(Now, "ddMMyyyyHHmmss")
                    ExecuteSqlQuery("UPDATE UserInformation SET ResetCode='" + ResetCode.ToString() + "' , UseResetCode='N'  WHERE   (USER_ID = " & USER_ID & ")")
                    Dim strHost, strPort, strUserName, strPassword As String
                    Dim dt_smtp As DataTable = QueryDataTable(" SELECT    *     FROM            SMTP_Info")
                    If dt_smtp.Rows.Count > 0 Then
                        strHost = dt_smtp.Rows(0)("Host").ToString()
                        strPort = dt_smtp.Rows(0)("Port").ToString()
                        strUserName = dt_smtp.Rows(0)("UserName").ToString()
                        strPassword = dt_smtp.Rows(0)("Password").ToString()
                        Dim msg As MailMessage = New MailMessage()
                        msg.[To].Add(txtEmail.Text)
                        msg.From = New MailAddress("Password Reset" & "<" & txtEmail.Text & ">")
                        msg.ReplyToList.Add(txtEmail.Text)
                        msg.Subject = "Hotel Management System"
                        msg.Body = "Reset Code :" & ResetCode.ToString() 'txtMessage.Text
                        Dim smt As SmtpClient = New SmtpClient()
                        smt.Host = strHost
                        Dim ntwd As Net.NetworkCredential = New NetworkCredential()
                        ntwd.UserName = strUserName 'Your Email ID  
                        ntwd.Password = strPassword ' Your Password  
                        smt.UseDefaultCredentials = True
                        smt.Credentials = ntwd
                        smt.Port = strPort
                        smt.EnableSsl = True
                        smt.Send(msg)
                    End If
                    Response.Redirect("PasswordReset.aspx")
                    'Try

                    'Catch ex As Exception
                    '    Response.Redirect("Default.aspx")
                    'End Try
                    '----------------------------------------
                End If
            End Using
        End Using
        'lblMessege.Text = "For Demonstration Purposes Only."
    End Sub
End Class