Imports System.Data.SqlClient
Imports System.IO

Public Class ChangeRoom
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

            ddlRoomNo.DataSource = FetchDataFromTable("SELECT   ROOM_ID, Room_No + '  - [Booked -' + Occupied + ']' AS RoomType  FROM     RoomDetails")
            ddlRoomNo.DataTextField = "RoomType"
            ddlRoomNo.DataValueField = "ROOM_ID"
            ddlRoomNo.DataBind()
            ddlRoomNo.Items.Insert(0, New ListItem("Please select"))


            If (Not (Session("FlushMessage")) Is Nothing) Then
                lblMessege.Text = Session("FlushMessage").ToString()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "showModal();", True)
                Session("FlushMessage") = Nothing
            End If

        End If
    End Sub
    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim dt_checkRoom As DataTable = QueryDataTable(" SELECT   *   FROM    RoomDetails  WHERE    (Occupied = 'Y') AND (ROOM_ID = " & ConvertingNumbers(ddlRoomNo.SelectedValue) & ") ")
        If dt_checkRoom.Rows.Count > 0 Then
            Dim current_room As DataTable = QueryDataTable("SELECT   * FROM            RoomDetails    WHERE        (GUEST_ID = " & ConvertingNumbers(txtGuestID.Text) & ") AND (ROOM_ID = " & ConvertingNumbers(ddlRoomNo.SelectedValue) & ") AND (Occupied = 'Y')")
            If current_room.Rows.Count > 0 Then
                ROOM_ALTER_DATA()
                HOTEL_SERVICE_CHARGE_CALCULATION(ConvertingNumbers(txtGuestID.Text))
                Session("FlushMessage") = "Room alter has been saved successfully."
                Response.Redirect("ChangeRoom.aspx")
            Else
                ClientScript.RegisterStartupScript(Me.[GetType](), "myalert", "alert('Room has been already booked by another guest.');", True)
            End If
        Else
            ROOM_ALTER_DATA()
            HOTEL_SERVICE_CHARGE_CALCULATION(ConvertingNumbers(txtGuestID.Text))
            Session("FlushMessage") = "Room alter has been saved successfully."
            Response.Redirect("ChangeRoom.aspx")
        End If
    End Sub
    Private Sub ROOM_ALTER_DATA()
        Dim CURRENT_ROOM_ID As Double
        Dim dt_checkRoom As DataTable = QueryDataTable(" SELECT  * FROM   GuestInformation  WHERE    (GUEST_ID = " & ConvertingNumbers(txtGuestID.Text) & ") ")
        If dt_checkRoom.Rows.Count > 0 Then
            CURRENT_ROOM_ID = dt_checkRoom.Rows(0)("ROOM_ID")
        Else
            CURRENT_ROOM_ID = 0
        End If

        ExecuteSqlQuery("UPDATE  GuestInformation   SET  ROOM_ID= " & ConvertingNumbers(ddlRoomNo.SelectedValue) & "  WHERE    (GUEST_ID = " & ConvertingNumbers(txtGuestID.Text) & ")  ")

        Using con As New SqlConnection(CnString)
            Using cmd As New SqlCommand("INSERT INTO  ChangeRoom (GUEST_ID, FROM_ROOM_ID, NEW_ROOM_ID, Reason, Room_Alter_Date) VALUES (@GUEST_ID, @FROM_ROOM_ID, @NEW_ROOM_ID, @Reason, '" + Format(Now, "MM/dd/yyyy") + "')")
                cmd.Parameters.AddWithValue("@GUEST_ID", ConvertingNumbers(txtGuestID.Text))
                cmd.Parameters.AddWithValue("@FROM_ROOM_ID", ConvertingNumbers(CURRENT_ROOM_ID))
                cmd.Parameters.AddWithValue("@NEW_ROOM_ID", ddlRoomNo.SelectedValue)
                cmd.Parameters.AddWithValue("@Reason", txtReason.Text)
                cmd.Connection = con
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        End Using
        RoomBooking(ConvertingNumbers(txtGuestID.Text), ddlRoomNo.SelectedValue)
    End Sub
End Class