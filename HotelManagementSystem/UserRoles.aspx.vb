Imports System.Data.SqlClient
Imports System.IO

Public Class UserRoles
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

            If Request.QueryString("act") IsNot Nothing Then
                If Request.QueryString("act") = "edit" Then
                    '---------------------------------------
                    If IsNumeric(Request.QueryString("id")) Then
                        btnSubmit.Text = "Update"
                        hfldID.Value = ConvertingNumbers(Request.QueryString("id"))
                        Dim dt_cust As DataTable = QueryDataTable("SELECT * FROM UserRoles WHERE ROLE_ID= " & ConvertingNumbers(hfldID.Value) & " ")
                        For Each custrow As DataRow In dt_cust.Rows
                            txtRoleName.Text = custrow("ROLE_NAME").ToString()
                        Next
                    Else
                        Response.Redirect("UserRoles.aspx")
                    End If
                    '---------------------------------------
                ElseIf Request.QueryString("act") = "del" Then
                    '---------------------------------------
                    If IsNumeric(Request.QueryString("id")) Then
                        hfldID.Value = ConvertingNumbers(Request.QueryString("id"))
                        QueryDataTable("DELETE UserRoles WHERE ROLE_ID= " & ConvertingNumbers(hfldID.Value) & " ")
                        Session("FlushMessage") = "Data has been Deleted successfully."
                        Response.Redirect("UserRoles.aspx")
                    Else
                        Response.Redirect("UserRoles.aspx")
                    End If
                    '---------------------------------------
                Else
                    Response.Redirect("UserRoles.aspx")
                End If
            End If

            Dim dt As DataTable = QueryDataTable("SELECT * FROM UserRoles")
            For Each row As DataRow In dt.Rows
                GridView1.DataSource = dt
                GridView1.DataBind()
            Next

            If (Not (Session("FlushMessage")) Is Nothing) Then
                lblMessege.Text = Session("FlushMessage").ToString()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "showModal();", True)
                Session("FlushMessage") = Nothing
            End If

        End If
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If btnSubmit.Text = "Submit" Then
            Using con As New SqlConnection(CnString)
                Using cmd As New SqlCommand("INSERT INTO  UserRoles (ROLE_NAME) VALUES (@ROLE_NAME)")
                    cmd.Parameters.AddWithValue("@ROLE_NAME", txtRoleName.Text)
                    cmd.Connection = con
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                End Using
            End Using
            Session("FlushMessage") = "Data has been saved successfully."
            Response.Redirect("UserRoles.aspx")
        ElseIf btnSubmit.Text = "Update" Then
            Using con As New SqlConnection(CnString)
                Using cmd As New SqlCommand("UPDATE   UserRoles SET ROLE_NAME= @ROLE_NAME  WHERE  ROLE_ID= @ROLE_ID")
                    cmd.Parameters.AddWithValue("@ROLE_ID", ConvertingNumbers(hfldID.Value))
                    cmd.Parameters.AddWithValue("@ROLE_NAME", txtRoleName.Text)
                    cmd.Connection = con
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                End Using
            End Using
            Session("FlushMessage") = "Data has been updated successfully."
            Response.Redirect("UserRoles.aspx")
        End If
    End Sub
End Class