Imports System.Data.SqlClient
Imports System.IO
Imports System.Net

Public Class _Default
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Dim chkDefaultUser As DataTable = QueryDataTable("SELECT * FROM UserInformation")
            If Not chkDefaultUser.Rows.Count > 0 Then
                Dim sql As String = "INSERT INTO UserInformation (FirstName, LastName, Email, Password, RegistrationDate, LastLoginDate, ResetCode) VALUES " &
                               " ('System', 'Administrator', 'admin@example.com', 'srZDpoFBuo/e8VgvooropQ==', '" & Format(Now, "MM/dd/yyyy") & "', '" & Format(Now, "MM/dd/yyyy") & "', 0) SELECT SCOPE_IDENTITY() "
                Dim USR_ID As Double = 0
                Using con As New SqlConnection(CnString)
                    Using cmd As New SqlCommand(sql)
                        cmd.Connection = con
                        con.Open()
                        USR_ID = Convert.ToInt32(cmd.ExecuteScalar())
                        con.Close()
                        Insert_Default_Permissions(USR_ID)
                    End Using
                End Using
                QueryDataTable("UPDATE Permissions Set AllowUser=1 WHERE (USER_ID = " & USR_ID & ") And (WebForm = 'Permission') ")
            End If
        End If
    End Sub
    Protected Sub ValidateUser(ByVal sender As Object, ByVal e As EventArgs)
        Dim externalIP As String = GetIpAddress()

        If Equals(externalIP, "::1") Then
            externalIP = "127.0.0.1"
        End If
        If externalIP.Length < 8 Then
            externalIP = "127.0.0.1"
        End If
        externalIP = externalIP.Replace(" & vbCrLf", "")
        Dim dtUserIpExist As DataTable = QueryDataTable("SELECT *  FROM      UserInformation  WHERE  (IP_Address != '' AND Email = '" + txtEmail.Text + "')")
        Dim dtUserIp As DataTable = QueryDataTable("SELECT *  FROM      UserInformation  WHERE  (IP_Address = '" + externalIP.TrimEnd + "' AND Email = '" + txtEmail.Text + "')")

        If dtUserIpExist.Rows.Count = 0 Then

        Else

            If dtUserIp.Rows.Count = 0 Then
                Session("FlushMessage") = "No tiene permiso para accesar, revisar con el administrador."
                'lblMessege.Text = externalIP.TrimEnd
                lblMessege.Text = "No tiene permiso para accesar, revisar con el administrador."
                Return
                'Response.Redirect("Default.aspx")
            End If
        End If




        Dim userId As Integer = 0
        Dim RoleId As Integer = 0
        Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("ValidateUser")
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Username", txtEmail.Text)
                cmd.Parameters.AddWithValue("@Password", Encrypt(txtPassword.Text))
                cmd.Connection = con
                con.Open()
                userId = Convert.ToInt32(cmd.ExecuteScalar())
            End Using

            Select Case userId
                Case -1
                    lblMessege.Text = "Username and/or password is incorrect."
                    Exit Select
                Case -2
                    lblMessege.Text = "Account has not been activated."
                    Exit Select
                Case Else
                    Session("LOGGED_USER_ID") = Encrypt(userId.ToString)

                    Using cmd2 As New SqlCommand("GetRoleName")
                        cmd2.CommandType = CommandType.StoredProcedure
                        cmd2.Parameters.AddWithValue("@Username", txtEmail.Text)
                        cmd2.Connection = con
                        RoleId = Convert.ToInt32(cmd2.ExecuteScalar())
                    End Using



                    Dim loggedOutAfterInactivity As Integer = 43200 'Minutes
                    'Keep the session alive as long as the authentication cookie.
                    Session.Timeout = loggedOutAfterInactivity
                    '            Dim cookie As HttpCookie = HttpContext.Current.Request.Cookies(FormsAuthentication.FormsCookieName)
                    '            If cookie IsNot Nothing Then
                    '                Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(cookie.Value)
                    '                If HttpContext.Current.User IsNot Nothing Then
                    '                    HttpContext.Current.User = New System.Security.Principal.GenericPrincipal(New FormsIdentity(ticket), New String(-1) {})
                    '                End If
                    '                Dim newTicket As New FormsAuthenticationTicket(
                    'ticket.Version, ticket.Name, ticket.IssueDate,
                    'ticket.IssueDate.AddMinutes(loggedOutAfterInactivity),
                    'ticket.IsPersistent, ticket.UserData)
                    '                cookie.Value = FormsAuthentication.Encrypt(newTicket)
                    '                cookie.Expires = Now.AddDays(30)
                    '            End If

                    con.Close()
                    FormsAuthentication.RedirectFromLoginPage(userId, True)
                    If RoleId > 0 Then
                        Dim httpRequestBase As HttpRequestBase = New HttpRequestWrapper(HttpContext.Current.Request)
                        Dim httpResponseBase As HttpResponseBase = New HttpResponseWrapper(HttpContext.Current.Response)
                        Dim newCookieHelper As CookieHelper = New CookieHelper(httpRequestBase, httpResponseBase)
                        newCookieHelper.SetLoginCookie(txtEmail.Text, Encrypt(txtPassword.Text), True)
                        Response.Redirect("RoomView")
                    End If
                    Exit Select
            End Select
        End Using
    End Sub

    Private Function GetIpAddress() As String
        Dim externalIP As String = ""
        'externalIP = If(Request.ServerVariables("HTTP_X_FORWARDED_FOR"), Request.ServerVariables("REMOTE_ADDR")).Split(","c)(0).Trim()
        'Dim client As New WebClient
        ''// Add a user agent header in case the requested URI contains a query.
        'client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR1.0.3705;)")
        'Dim baseurl As String = "http://checkip.dyndns.org/"
        '' with proxy server only:
        'Dim proxy As IWebProxy = WebRequest.GetSystemWebProxy()
        'proxy.Credentials = CredentialCache.DefaultNetworkCredentials
        'client.Proxy = proxy
        'Dim data As Stream
        'Try
        '    data = client.OpenRead(baseurl)
        'Catch ex As Exception
        '    MsgBox("open url " & ex.Message)
        '    'Exit Sub
        'End Try
        'Dim reader As StreamReader = New StreamReader(data)
        'Dim s As String = reader.ReadToEnd()
        'data.Close()
        'reader.Close()
        's = s.Replace("<html><head><title>Current IP Check</title></head><body>", "").Replace("</body></html>", "").Replace("Current IP Address: ", "").ToString()
        'externalIP = s
        externalIP = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If externalIP = "" Or externalIP Is Nothing Then
            externalIP = Request.ServerVariables("REMOTE_ADDR")
        End If
        Return externalIP
    End Function
End Class

Public Class CookieHelper
    Private _request As HttpRequestBase
    Private _response As HttpResponseBase

    Public Sub New(ByVal request As HttpRequestBase, ByVal response As HttpResponseBase)
        _request = request
        _response = response
    End Sub

    Public Sub SetLoginCookie(ByVal userName As String, ByVal password As String, ByVal isPermanentCookie As Boolean)
        If _response IsNot Nothing Then

            If isPermanentCookie Then
                Dim userAuthTicket As FormsAuthenticationTicket = New FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.MaxValue, True, password, FormsAuthentication.FormsCookiePath)
                Dim encUserAuthTicket As String = FormsAuthentication.Encrypt(userAuthTicket)
                Dim userAuthCookie As HttpCookie = New HttpCookie(FormsAuthentication.FormsCookieName, encUserAuthTicket)
                If userAuthTicket.IsPersistent Then userAuthCookie.Expires = userAuthTicket.Expiration
                userAuthCookie.Path = FormsAuthentication.FormsCookiePath
                _response.Cookies.Add(userAuthCookie)
            Else
                FormsAuthentication.SetAuthCookie(userName, isPermanentCookie)
            End If
        End If
    End Sub

End Class

