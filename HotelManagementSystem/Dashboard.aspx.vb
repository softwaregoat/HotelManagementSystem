Public Class Dashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Not Me.Page.User.Identity.IsAuthenticated Or Session("LOGGED_USER_ID") Is Nothing Then
                FormsAuthentication.RedirectToLoginPage()
            End If

            If Date.Now.Hour < 12 Then
                lblGreeting.Text = "Good Morning!"
            ElseIf Date.Now.Hour < 17 Then
                lblGreeting.Text = "Good Afternoon!"
            Else
                lblGreeting.Text = "Good Evening!"
            End If

            Try
                Dim dt As DataTable = QueryDataTable("SELECT * FROM UserInformation   WHERE  (USER_ID = " & ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)) & ")   ")
                For Each row As DataRow In dt.Rows
                    lblUserFullName.Text = row("FirstName").ToString() & " " & row("LastName").ToString()
                Next
                Dim dt_CheckIn As DataTable = QueryDataTable(" SELECT  *  FROM   GuestInformation   WHERE        (Status = 'CheckIn') ")
                lblCheckIn.Text = dt_CheckIn.Rows.Count.ToString()
                Dim dt_CheckOut As DataTable = QueryDataTable(" SELECT  *  FROM   GuestInformation   WHERE        (Status = 'CheckOut') ")
                lblCheckOut.Text = dt_CheckOut.Rows.Count.ToString()
                Dim dt_Reservation As DataTable = QueryDataTable(" SELECT  *  FROM   GuestInformation   WHERE        (Status = 'Reservation')  ")
                lblReservation.Text = dt_Reservation.Rows.Count.ToString()
                Dim dt_Cancelled As DataTable = QueryDataTable(" SELECT  *  FROM   GuestInformation   WHERE        (Status = 'Cancelled') ")
                lblCancelled.Text = dt_Cancelled.Rows.Count.ToString()
                Dim dt_DirtyRoom As DataTable = QueryDataTable(" SELECT  *  FROM   GuestInformation   WHERE        (DirtyRoom = 'Y') ")
                lblDirtyRoom.Text = dt_DirtyRoom.Rows.Count.ToString()
                Dim dt_FoodItem As DataTable = QueryDataTable(" SELECT  *  FROM   FoodItem")
                lblFoodItem.Text = dt_FoodItem.Rows.Count.ToString()
                Dim dt_FoodServices As DataTable = QueryDataTable(" SELECT  *  FROM   FoodServices")
                lblInvoice.Text = dt_FoodServices.Rows.Count.ToString()
                Dim dt_UserInformation As DataTable = QueryDataTable(" SELECT  *  FROM   UserInformation")
                lblUsers.Text = dt_UserInformation.Rows.Count.ToString()
            Catch ex As Exception

            End Try

            If (Not (Session("FlushMessage")) Is Nothing) Then
                lblMessege.Text = Session("FlushMessage").ToString()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "showModal();", True)
                Session("FlushMessage") = Nothing
            End If

        End If
    End Sub

End Class