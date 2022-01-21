Imports System.Data.SqlClient
Imports System.IO
Public Class UserInformation
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

            DropDownListRoles.DataSource = FetchDataFromTable("SELECT [ROLE_ID], [ROLE_NAME] FROM [UserRoles]")
            DropDownListRoles.DataTextField = "ROLE_NAME"
            DropDownListRoles.DataValueField = "ROLE_ID"
            DropDownListRoles.DataBind()
            DropDownListRoles.Items.Insert(0, New ListItem("Please select role"))

            If Request.UrlReferrer IsNot Nothing Then
                If System.IO.Path.GetFileName(Request.UrlReferrer.AbsolutePath) = "UsersList" Then
                    If Request.QueryString("act") IsNot Nothing Then
                        If Request.QueryString("act") = "edit" Then
                            If (Not (Session("x_user_id")) Is Nothing) Then
                                txtEmail.Enabled = False
                                btnSubmit.Text = "Update"
                                Dim dt As DataTable = QueryDataTable("SELECT  *  FROM   UserInformation   WHERE   (USER_ID = " & ConvertingNumbers(Decrypt(Session("x_user_id"))) & ")")
                                For Each row As DataRow In dt.Rows
                                    txtFirstName.Text = row("FirstName").ToString
                                    txtLastName.Text = row("LastName").ToString
                                    txtEmail.Text = row("Email").ToString
                                    txtIpAddress.Text = row("IP_Address").ToString
                                    DropDownListRoles.SelectedIndex = DropDownListRoles.Items.IndexOf(DropDownListRoles.Items.FindByValue(row("ROLE_ID").ToString))
                                    Dim password As String = Decrypt(row("Password"))
                                    txtPassword.Text = password

                                    txtPassword.Attributes("value") = txtPassword.Text
                                    txtRePassword.Text = password

                                    txtRePassword.Attributes("value") = txtRePassword.Text
                                Next
                            Else
                                Response.Redirect("UserInformation.aspx")
                            End If
                        Else
                            Response.Redirect("UserInformation.aspx")
                        End If
                    End If
                End If
            End If

            If (Not (Session("FlushMessage")) Is Nothing) Then
                lblMessege.Text = Session("FlushMessage").ToString()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "showModal();", True)
                Session("FlushMessage") = Nothing
            End If
        End If
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If btnSubmit.Text = "Confirmar" Then
            Dim dtUser As DataTable = QueryDataTable("SELECT *  FROM      UserInformation  WHERE  (Email = '" + txtEmail.Text + "')")
            Dim dtUserIp As DataTable = QueryDataTable("SELECT *  FROM      UserInformation  WHERE  (IP_Address = '" + txtIpAddress.Text + "')")
            If dtUser.Rows.Count > 0 Then
                Session("FlushMessage") = "This email address is already in use."
                Response.Redirect("UserInformation.aspx")
            ElseIf dtUserIp.Rows.Count > 0 Then
                Session("FlushMessage") = "This IP address is already in use."
                Response.Redirect("UserInformation.aspx")
            Else
                Using con As New SqlConnection(CnString)
                    Using cmd As New SqlCommand("INSERT INTO  UserInformation (FirstName , LastName , Email, Password,ROLE_ID, RegistrationDate, LastLoginDate,IP_Address) VALUES (@FirstName , @LastName , @Email, @Password,@ROLE_ID, '" & Format(Now, "MM/dd/yyyy") & "', '" & Format(Now, "MM/dd/yyyy") & "',@IP_Address) SELECT SCOPE_IDENTITY()")
                        cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text)
                        cmd.Parameters.AddWithValue("@LastName", txtLastName.Text)
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text)
                        cmd.Parameters.AddWithValue("@Password", Encrypt(txtPassword.Text))
                        cmd.Parameters.AddWithValue("@ROLE_ID", DropDownListRoles.SelectedValue)
                        cmd.Parameters.AddWithValue("@IP_Address", txtIpAddress.Text)
                        cmd.Connection = con
                        con.Open()
                        Dim USR_ID As Double = Convert.ToInt32(cmd.ExecuteScalar())
                        con.Close()
                        Insert_Default_Permissions(USR_ID)
                    End Using
                End Using
                Session("FlushMessage") = "User information has been saved successfully."
                Response.Redirect("UserInformation.aspx")
            End If
        ElseIf btnSubmit.Text = "Update" Then
            Using con As New SqlConnection(CnString)
                Using cmd As New SqlCommand("UPDATE   UserInformation  SET FirstName=@FirstName , LastName=@LastName, Password= @Password,ROLE_ID=@ROLE_ID,IP_Address=@IP_Address  WHERE   (USER_ID = " & ConvertingNumbers(Decrypt(Session("x_user_id"))) & ") ")
                    cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text)
                    cmd.Parameters.AddWithValue("@LastName", txtLastName.Text)
                    cmd.Parameters.AddWithValue("@Password", Encrypt(txtPassword.Text))
                    cmd.Parameters.AddWithValue("@ROLE_ID", DropDownListRoles.SelectedValue)
                    cmd.Parameters.AddWithValue("@IP_Address", txtIpAddress.Text)
                    cmd.Connection = con
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                End Using
            End Using
            Session("x_user_id") = vbNull
            Session("FlushMessage") = "User information has been updated successfully."
            Response.Redirect("UserInformation.aspx")
        End If
    End Sub
End Class