Imports System.IO

Public Class UsersList
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

            GridView1.DataSource = FetchDataFromTable(" SELECT * FROM UserInformation ORDER BY USER_ID DESC")
            GridView1.DataBind()

            If (Not (Session("FlushMessage")) Is Nothing) Then
                lblMessege.Text = Session("FlushMessage").ToString()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "showModal();", True)
                Session("FlushMessage") = Nothing
            End If
        End If
    End Sub
    Protected Sub SendEdit(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnSend As Button = CType(sender, Button)
        Dim row As GridViewRow = CType(btnSend.NamingContainer, GridViewRow)
        Session("x_user_id") = Encrypt(row.Cells(0).Text)
        Response.Redirect("~/UserInformation.aspx?act=edit")
    End Sub
    Protected Sub SendDelete(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnSend As Button = CType(sender, Button)
        Dim row As GridViewRow = CType(btnSend.NamingContainer, GridViewRow)
        QueryDataTable("DELETE UserInformation WHERE USER_ID= " & ConvertingNumbers(row.Cells(0).Text) & " ")
        QueryDataTable("DELETE Permissions WHERE USER_ID= " & ConvertingNumbers(row.Cells(0).Text) & " ")
        Session("FlushMessage") = "Data has been Deleted successfully."
        Response.Redirect("UsersList.aspx")
    End Sub
End Class