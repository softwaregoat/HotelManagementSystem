Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Web.Services
Public Class Reservation
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

            txtRevCheckInDate.Text = Format(Now, "dd/MM/yyyy")

            ddlRoomNo.DataSource = FetchDataFromTable("SELECT        ROOM_ID, Room_No + '  - [Booked -' + Occupied + ']' AS RoomType  FROM            RoomDetails")
            ddlRoomNo.DataTextField = "RoomType"
            ddlRoomNo.DataValueField = "ROOM_ID"
            ddlRoomNo.DataBind()
            ddlRoomNo.Items.Insert(0, New ListItem("Please select"))

        End If
    End Sub

    Private Sub btnReservationSubmit_Click(sender As Object, e As EventArgs) Handles btnReservationSubmit.Click
        Dim dt_checkRoom As DataTable = QueryDataTable(" SELECT   *   FROM    RoomDetails  WHERE    (Occupied = 'Y') AND (ROOM_ID = " & ConvertingNumbers(ddlRoomNo.SelectedValue) & ") ")
        If dt_checkRoom.Rows.Count > 0 Then
            ClientScript.RegisterStartupScript(Me.[GetType](), "myalert", "alert('Room has been already booked.');", True)
        Else
            '-----------------------------------
            Dim RevCheckInDate As Date = DateTime.ParseExact(txtRevCheckInDate.Text.ToString, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
            Using con As New SqlConnection(CnString)
                Using cmd As New SqlCommand("INSERT INTO  GuestInformation (GuestName, Address, PhoneNo, Note, Check_In_Date, Check_In_Time, Check_Out_Date, Check_Out_Time, No_Of_Day, ROOM_ID, Rent_Day, No_Of_Adult, No_Of_Children, PaymentType, PaidAmount, Status) VALUES (@GuestName, @Address, @PhoneNo, @Note, @Check_In_Date, @Check_In_Time, @Check_Out_Date, @Check_Out_Time, @No_Of_Day, @ROOM_ID, @Rent_Day, @No_Of_Adult, @No_Of_Children, @PaymentType, @PaidAmount, 'Reservation') SELECT SCOPE_IDENTITY()")
                    cmd.Parameters.AddWithValue("@GuestName", txtGuestName.Text)
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text)
                    cmd.Parameters.AddWithValue("@PhoneNo", txtPhoneNo.Text)
                    cmd.Parameters.AddWithValue("@Note", txtNotes.Text)
                    cmd.Parameters.AddWithValue("@Check_In_Date", RevCheckInDate)
                    cmd.Parameters.AddWithValue("@Check_In_Time", txtRevCheckInTime.Text)
                    cmd.Parameters.AddWithValue("@Check_Out_Date", RevCheckInDate)
                    cmd.Parameters.AddWithValue("@Check_Out_Time", txtRevCheckInTime.Text)
                    cmd.Parameters.AddWithValue("@No_Of_Day", txtNoOfDay.Text)
                    cmd.Parameters.AddWithValue("@ROOM_ID", ddlRoomNo.SelectedValue)
                    cmd.Parameters.AddWithValue("@PaymentType", ddlPaymentType.SelectedValue)
                    cmd.Parameters.AddWithValue("@Rent_Day", txtChangeRentnDay.Text)
                    cmd.Parameters.AddWithValue("@No_Of_Adult", txtNoOfAdult.Text)
                    cmd.Parameters.AddWithValue("@No_Of_Children", txtNoOfChildren.Text)
                    cmd.Parameters.AddWithValue("@PaidAmount", ConvertingNumbers(txPayment.Text))
                    cmd.Connection = con
                    con.Open()
                    Dim GUEST_ID As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    con.Close()
                    RoomBooking(GUEST_ID, ddlRoomNo.SelectedValue)
                    HOTEL_SERVICE_CHARGE_CALCULATION(GUEST_ID)
                End Using
            End Using
            Session("FlushMessage") = "Data has been saved successfully."
            Response.Redirect("GuestList.aspx")
            '-----------------------------------
        End If
    End Sub
End Class