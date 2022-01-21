Imports System.Data.SqlClient
Imports System.IO

Public Class Permission
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

    Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim CheckBox1 As CheckBox = TryCast(e.Row.FindControl("chkRow"), CheckBox)
            If e.Row.Cells(1).Text = "True" Then
                CheckBox1.Checked = True
            End If
        End If
    End Sub
    Protected Sub GetSelectedRecords(sender As Object, e As EventArgs)
        Dim sql As String = ""
        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn(1) {New DataColumn("WebForm"), New DataColumn("AllowUser")})
        For Each row As GridViewRow In GridView1.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkRow"), CheckBox)
                If chkRow.Checked Then
                    Dim WebFormName As String = row.Cells(0).Text
                    'MsgBox(WebFormName + DropDownListUsers.SelectedValue.ToString + "YES")
                    sql = sql + " UPDATE PermissionsRoleBased SET AllowUser=1 WHERE (ROLE_ID = " & ConvertingNumbers(DropDownListRoles.SelectedItem.Value) & ") AND (WebForm = '" + WebFormName + "') "
                Else
                    Dim WebFormName As String = row.Cells(0).Text
                    sql = sql + " UPDATE PermissionsRoleBased SET AllowUser=0 WHERE (ROLE_ID = " & ConvertingNumbers(DropDownListRoles.SelectedItem.Value) & ") AND (WebForm = '" + WebFormName + "') "
                End If
            End If
        Next

        Using con As New SqlConnection(CnString)
            Using cmd As New SqlCommand(sql)
                cmd.Connection = con
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        End Using

        Session("FlushMessage") = "Permission has been saved change successfully."
        Response.Redirect("Permission.aspx")
    End Sub

End Class