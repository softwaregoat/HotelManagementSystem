Imports System.IO
Imports System.Web.Services

Public Class ExtraServices
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
        End If
    End Sub
    <WebMethod()>
    Public Shared Function FindGuestBill(GUEST_ID As Double) As String
        HOTEL_SERVICE_CHARGE_CALCULATION(GUEST_ID)
        Dim dt As DataTable = QueryDataTable("SELECT * FROM      GuestInformation   WHERE        (GUEST_ID = " & GUEST_ID & ")")
        If dt.Rows.Count > 0 Then
            sqlStr = " SELECT    GuestInformation.GUEST_ID, GuestInformation.GuestName, GuestInformation.Address, GuestInformation.PhoneNo, GuestInformation.Check_In_Date, GuestInformation.Check_Out_Date,  GuestInformation.Check_In_Time,  GuestInformation.Check_Out_Time, 
                         GuestInformation.No_Of_Day, RoomType.ROOM_TYPE, RoomDetails.Room_No, GuestInformation.Rent_Day, GuestInformation.Total_Extra_Bed_Cost, GuestInformation.Total_Charges, 
                         GuestInformation.Total_Extra_Bed_Cost + GuestInformation.Total_Charges AS TotalBedCost, GuestInformation.Boarding, GuestInformation.Food, GuestInformation.Laundry, GuestInformation.Telephone, 
                         GuestInformation.BAR, GuestInformation.DINNER, GuestInformation.Breakfat, GuestInformation.SPA, GuestInformation.BanquetDinner, GuestInformation.Cleaning, GuestInformation.ServiceCharges, 
                         GuestInformation.OtherCharges, GuestInformation.TAX_1, GuestInformation.TAX_2, GuestInformation.TAX_3, GuestInformation.GrandTotal, GuestInformation.DiscountAmount, 
                         GuestInformation.NetAmount, GuestInformation.PaidAmount, GuestInformation.BalanceAmount, GuestInformation.PaymentType, GuestInformation.CheckOutNote
                         FROM    RoomType INNER JOIN RoomDetails ON RoomType.ROOM_TYPE_ID = RoomDetails.ROOM_TYPE_ID RIGHT OUTER JOIN
                         GuestInformation ON RoomDetails.ROOM_ID = GuestInformation.ROOM_ID WHERE        (GuestInformation.GUEST_ID = " & GUEST_ID & ") "
            Dim SelectedCustInfo As DataSet = FetchDataFromTable(sqlStr)
            Return SelectedCustInfo.GetXml
        Else
            Return 0
        End If
    End Function
    <WebMethod()>
    Public Shared Function SaveExtraServiceCharges(GUEST_ID As Double, Boarding As Double, Food As Double, Laundry As Double, Telephone As Double, BAR As Double, DINNER As Double, Breakfat As Double, SPA As Double, BanquetDinner As Double, Cleaning As Double, ServiceCharges As Double, OtherCharges As Double) As String
        ExecuteSqlQuery(" UPDATE GuestInformation SET Boarding=" & Boarding & ", Food=" & Food & ", Laundry=" & Laundry & ", Telephone=" & Telephone & ", BAR=" & BAR & ", DINNER=" & DINNER & ", Breakfat=" & Breakfat & ", SPA=" & SPA & ", BanquetDinner=" & BanquetDinner & ", Cleaning=" & Cleaning & ", ServiceCharges=" & ServiceCharges & ", OtherCharges=" & OtherCharges & "  WHERE  (GUEST_ID = " & GUEST_ID & ") ")
        HOTEL_SERVICE_CHARGE_CALCULATION(GUEST_ID)
        Return 1
    End Function
End Class