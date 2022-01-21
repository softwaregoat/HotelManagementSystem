Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Services

Public Class CheckOut
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

            Dim dt_cust As DataTable = QueryDataTable("SELECT * FROM TAXInfo")
            For Each chk_row As DataRow In dt_cust.Rows
                lblTAX1.Text = chk_row("TAX_Name_1").ToString() & " - " & chk_row("Rate_1").ToString() & " %"
                lblTAX2.Text = chk_row("TAX_Name_2").ToString() & " - " & chk_row("Rate_2").ToString() & " %"
                lblTAX3.Text = chk_row("TAX_Name_3").ToString() & " - " & chk_row("Rate_3").ToString() & " %"
            Next
        End If
    End Sub
    <WebMethod()>
    Public Shared Function SaveCheckOut(GUEST_ID As Double, CheckOutNote As String, Check_Out_Time As String, DiscountAmount As Double, PaymentType As String, PaidAmount As Double) As String
        Using con As New SqlConnection(CnString)
            Using cmd As New SqlCommand("UPDATE   GuestInformation SET CheckOutNote=@CheckOutNote, Check_Out_Time=@Check_Out_Time, DiscountAmount=@DiscountAmount, PaymentType=@PaymentType, PaidAmount=@PaidAmount  WHERE  GUEST_ID= @GUEST_ID")
                cmd.Parameters.AddWithValue("@GUEST_ID", GUEST_ID)
                cmd.Parameters.AddWithValue("@CheckOutNote", CheckOutNote)
                cmd.Parameters.AddWithValue("@Check_Out_Time", Check_Out_Time)
                cmd.Parameters.AddWithValue("@DiscountAmount", DiscountAmount)
                cmd.Parameters.AddWithValue("@PaymentType", PaymentType)
                cmd.Parameters.AddWithValue("@PaidAmount", PaidAmount)
                cmd.Connection = con
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        End Using
        HOTEL_SERVICE_CHARGE_CALCULATION(GUEST_ID)
        Return 1
    End Function
    <WebMethod()>
    Public Shared Function CompleteCheckOut(GUEST_ID As Double, CheckOutNote As String, Check_Out_Time As String, DiscountAmount As Double, PaymentType As String, PaidAmount As Double) As String
        Dim ROOM_ID As Double
        Dim dt_guestinfo As DataTable = QueryDataTable(" SELECT    *  FROM     GuestInformation  WHERE    (GUEST_ID = " & GUEST_ID & ") ")
        If dt_guestinfo.Rows.Count > 0 Then
            ROOM_ID = dt_guestinfo.Rows(0)("ROOM_ID")
        End If
        ExecuteSqlQuery("UPDATE  RoomDetails SET Occupied='N', GUEST_ID=0  WHERE   (ROOM_ID =" & ROOM_ID & ")  ")

        Using con As New SqlConnection(CnString)
            Using cmd As New SqlCommand("UPDATE   GuestInformation SET CheckOutNote=@CheckOutNote, Check_Out_Time=@Check_Out_Time, DiscountAmount=@DiscountAmount, PaymentType=@PaymentType, PaidAmount=@PaidAmount, Status = 'CheckOut'  WHERE  GUEST_ID= @GUEST_ID")
                cmd.Parameters.AddWithValue("@GUEST_ID", GUEST_ID)
                cmd.Parameters.AddWithValue("@CheckOutNote", CheckOutNote)
                cmd.Parameters.AddWithValue("@Check_Out_Time", Check_Out_Time)
                cmd.Parameters.AddWithValue("@DiscountAmount", DiscountAmount)
                cmd.Parameters.AddWithValue("@PaymentType", PaymentType)
                cmd.Parameters.AddWithValue("@PaidAmount", PaidAmount)
                cmd.Connection = con
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        End Using
        HOTEL_SERVICE_CHARGE_CALCULATION(GUEST_ID)
        Return 1
    End Function
End Class