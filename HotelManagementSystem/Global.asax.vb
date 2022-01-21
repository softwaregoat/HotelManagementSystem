
Imports System.IO

Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(sender As Object, e As EventArgs)
        ' Fires when the application is started
        Application("Title") = "HotelManagementSystem"
    End Sub
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        Dim con As HttpContext = HttpContext.Current
        Dim v = Server.GetLastError
        Dim HttpEx = TryCast(v, HttpException)
        If HttpEx IsNot Nothing AndAlso HttpEx.GetHttpCode = "404" Then
            Server.Transfer("~/NotFound.aspx")
        Else
            Dim sb = New StringBuilder
            sb.AppendLine("Page :           " & con.Request.Url.ToString)
            sb.AppendLine("Error Message :  " & v.Message)
            If v.InnerException IsNot Nothing Then
                sb.AppendLine("Inner Message :  " & v.InnerException.ToString)
            End If
            Dim fileName = Path.Combine(Server.MapPath("~/Errors"), Date.Now.ToString("ddMMyyyyhhmmss") & ".txt")
            File.WriteAllText(fileName, sb.ToString)
            Server.Transfer("~/NotFound.aspx")
        End If
    End Sub
    'Private Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim cookie As HttpCookie = HttpContext.Current.Request.Cookies(FormsAuthentication.FormsCookieName)

    '    If cookie IsNot Nothing Then
    '        Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(cookie.Value)
    '        HttpContext.Current.User = New System.Security.Principal.GenericPrincipal(New FormsIdentity(ticket), New String(-1) {})
    '    End If
    'End Sub
End Class