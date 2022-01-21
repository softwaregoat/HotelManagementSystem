Public Class rpt_FoodServiceItemList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Not Me.Page.User.Identity.IsAuthenticated Then
                FormsAuthentication.RedirectToLoginPage()
            End If
        End If
    End Sub

End Class