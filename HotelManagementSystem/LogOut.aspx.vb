Public Class LogOut
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("LOGGED_USER_ID") = Nothing
        Session.Remove("LOGGED_USER_ID")
        FormsAuthentication.SignOut()
    End Sub

    Private Sub LogOut_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Response.Redirect("Default.aspx")
    End Sub
End Class