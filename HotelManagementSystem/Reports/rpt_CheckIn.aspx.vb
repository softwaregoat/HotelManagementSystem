Public Class rpt_CheckIn
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Not Me.Page.User.Identity.IsAuthenticated Then
                FormsAuthentication.RedirectToLoginPage()
            End If
            If IsNumeric(Request.QueryString("id")) Then
                HiddenField1.Value = ConvertingNumbers(Request.QueryString("id"))
            Else
                Response.Redirect("~/DataNotFound.aspx")
            End If
        End If
    End Sub

End Class