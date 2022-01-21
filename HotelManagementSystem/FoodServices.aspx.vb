Imports System.Data.SqlClient
Imports System.IO
Public Class FoodServices
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


            GridView1.DataSource = FetchDataFromTable(" SELECT  TOP (500) FoodServices.BILL_NO, FoodServices.BILLING_DATE, GuestInformation.GuestName, FoodServices.G_TOTAL, FoodServices.PAYMENT, FoodServices.BALANCE
                                                       FROM   FoodServices LEFT OUTER JOIN GuestInformation ON FoodServices.GUEST_ID = GuestInformation.GUEST_ID  ORDER BY FoodServices.BILL_NO DESC ")
            GridView1.DataBind()
        End If
    End Sub

    Private Sub btnCreateNew_Click(sender As Object, e As EventArgs) Handles btnCreateNew.Click
        Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("INSERT INTO FoodServices (BILLING_DATE, USER_ID) VALUES (@BILLING_DATE, " & ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)) & ") SELECT SCOPE_IDENTITY()")
                cmd.Parameters.AddWithValue("@BILLING_DATE", Format(Now, "MM/dd/yyyy"))
                cmd.Connection = con
                con.Open()
                Dim row_id As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                con.Close()
                Response.Redirect("ItemCart.aspx?bill=" & row_id.ToString())
            End Using
        End Using
    End Sub
End Class