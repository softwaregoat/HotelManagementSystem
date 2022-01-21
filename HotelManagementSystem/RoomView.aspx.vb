Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Services
Imports HotelManagementSystem



Public Class RoomView
    Inherits System.Web.UI.Page
    Public sqlDTx As New DataTable
    Public tblOldRoomInfo As New DataTable
    Public tblRoomStatusTotal As New DataTable
    Public tblFoodItems As New DataTable
    Public changeRoomId As String
    Public changeRoomNo As String
    Public UserIds As String
    Public rentHours As String
    Public MaidCommentsIcon As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Not Me.Page.User.Identity.IsAuthenticated Or Session("LOGGED_USER_ID") Is Nothing Then
                FormsAuthentication.RedirectToLoginPage()
            End If
            Dim currentPageFileName As String = New FileInfo(Me.Request.Url.LocalPath).Name
            Try
                If Session("LOGGED_USER_ID") Is Nothing Then
                    FormsAuthentication.RedirectToLoginPage()
                    Return

                End If
                CheckPermissions(currentPageFileName.Split(".aspx")(0).ToString, ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)))
                hdnUserId.Value = Decrypt(Session("LOGGED_USER_ID").ToString)
                Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString

                If Request.QueryString("croom_id") IsNot Nothing Then
                    changeRoomId = Request.QueryString("croom_id")
                    If IsNumeric(changeRoomId) Then
                        hlnkRoomChangeCancel.NavigateUrl = "CheckIn.aspx?room_id=" + changeRoomId
                        Using con As New SqlConnection(constr)
                            'Using sqlComm2 As New SqlCommand("GetGuestInfoByRoomID")
                            '    sqlComm2.CommandType = CommandType.StoredProcedure
                            '    sqlComm2.Parameters.AddWithValue("@RoomID", changeRoomId)
                            '    sqlComm2.Connection = con
                            '    Dim billTbl As DataSet = New DataSet
                            '    Dim sqlDA As New SqlDataAdapter(sqlComm2)
                            '    Dim sqlCB As New SqlCommandBuilder(sqlDA)
                            '    sqlDA.Fill(billTbl)
                            '    If billTbl IsNot Nothing Then
                            '        If billTbl.Tables(0).Rows.Count > 0 Then
                            '            tblOldRoomInfo = billTbl.Tables(0)
                            '        End If
                            '    End If
                            'End Using



                            Using sqlComm3 As New SqlCommand("GetGuestInfoRoomChange")

                                sqlComm3.CommandType = CommandType.StoredProcedure
                                sqlComm3.Parameters.AddWithValue("@RoomID", changeRoomId)
                                sqlComm3.Connection = con
                                Dim billTbl As DataSet = New DataSet
                                Dim sqlDA As New SqlDataAdapter(sqlComm3)
                                Dim sqlCB As New SqlCommandBuilder(sqlDA)
                                sqlDA.Fill(billTbl)
                                If billTbl IsNot Nothing Then
                                    If billTbl.Tables(0).Rows.Count > 0 Then
                                        tblOldRoomInfo = billTbl.Tables(0)
                                    End If
                                End If
                            End Using





                            con.Close()
                        End Using

                    End If
                End If

                Dim foodTbl As DataSet = New DataSet
                foodTbl = FetchDataFromTable("Select *from FoodItem")
                If foodTbl IsNot Nothing Then
                    If foodTbl.Tables(0).Rows.Count > 0 Then
                        tblFoodItems = foodTbl.Tables(0)
                    End If
                End If







                If txtSearch.Text IsNot "" Then
                    If IsNumeric(txtSearch.Text) Then
                        sqlDTx = FetchDataByProcedure("dbo.[GetRoomViewByGuestId]", txtSearch.Text.Trim)
                    Else
                        If txtSearch.Text.Length >= 3 Then
                            sqlDTx = FetchDataByProcedure("dbo.[GetRoomViewByGestId]", txtSearch.Text.Trim)
                            If sqlDTx.Rows.Count = 0 Then
                                sqlDTx = FetchDataByProcedure("dbo.[GetRoomViewByGuestName]", txtSearch.Text.Trim)
                            End If
                        End If
                    End If
                Else
                    sqlDTx = FetchDataByProcedure("[dbo].[GetRoomView]", txtSearch.Text.Trim)

                    txtSearch.Text = ""
                End If




                tblRoomStatusTotal = FetchDataFromProcedure("dbo.[GetRoomStatusTotal]")

            Catch ex As Exception

            End Try


            If (Not (Session("FlushMessage")) Is Nothing) Then
                lblMessege.Text = Session("FlushMessage").ToString()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "showModal();", True)
                Session("FlushMessage") = Nothing
            End If
        End If
    End Sub
    Public Function ExecQuery(ByVal SQLQuery As String) As DataTable
        Try
            Dim sqlCon As New SqlConnection(CnString)
            Dim sqlDA As New SqlDataAdapter(SQLQuery, sqlCon)
            Dim sqlCB As New SqlCommandBuilder(sqlDA)
            sqlDTx.Reset()
            sqlDA.Fill(sqlDTx)
        Catch ex As Exception
            Dim err As String = ex.Message()
        End Try
        Return sqlDTx
    End Function

    Public Function ExecQueryGuestById(ByVal SQLQuery As String) As DataTable
        Try
            Dim sqlCon As New SqlConnection(CnString)
            Dim sqlDA As New SqlDataAdapter(SQLQuery, sqlCon)
            Dim sqlCB As New SqlCommandBuilder(sqlDA)
            sqlDTx.Reset()
            sqlDA.Fill(sqlDTx)
        Catch ex As Exception
        End Try
        Return sqlDTx
    End Function
    Protected Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        'If txtSearch.Text IsNot "" Then
        '    If IsNumeric(txtSearch.Text) Then
        '        ExecQuery("Select  *, BillDetails.Guest_ID as BGuest_ID,(select Gest_ID from GuestInformation where GUEST_ID=BillDetails.Guest_ID) as BGest_ID,(select GuestName from GuestInformation where GUEST_ID=BillDetails.Guest_ID) as BGuestName   from [dbo].[GetRoomView_vw] left join BillDetails on [GetRoomView_vw].ROOM_ID=BillDetails.Room_ID     where  ([GetRoomView_vw].GUEST_ID=" & txtSearch.Text.Trim & " or BillDetails.Guest_ID=" & txtSearch.Text.Trim & ") order by  Room_No, ROOM_TYPE_ID ")
        '    Else
        '        If txtSearch.Text.Length >= 3 Then
        '            ExecQuery("Select  *, BillDetails.Guest_ID as BGuest_ID,(select Gest_ID from GuestInformation where GUEST_ID=BillDetails.Guest_ID) as BGest_ID,(select GuestName from GuestInformation where GUEST_ID=BillDetails.Guest_ID) as BGuestName   from [dbo].[GetRoomView_vw] left join BillDetails on [GetRoomView_vw].ROOM_ID=BillDetails.Room_ID     where  [GetRoomView_vw].GEST_ID='" & txtSearch.Text.Trim & "' order by  Room_No, ROOM_TYPE_ID ")
        '            If sqlDTx.Rows.Count = 0 Then
        '                ExecQuery("Select  *, BillDetails.Guest_ID as BGuest_ID,(select Gest_ID from GuestInformation where GUEST_ID=BillDetails.Guest_ID) as BGest_ID,(select GuestName from GuestInformation where GUEST_ID=BillDetails.Guest_ID) as BGuestName   from [dbo].[GetRoomView_vw] left join BillDetails on [GetRoomView_vw].ROOM_ID=BillDetails.Room_ID     where  [GetRoomView_vw].GuestName Like '%'" & txtSearch.Text.Trim & "'%' order by  Room_No, ROOM_TYPE_ID ")
        '            End If
        '        End If
        '    End If
        'Else
        '    ExecQuery("Select  *, BillDetails.Guest_ID as BGuest_ID,(select Gest_ID from GuestInformation where GUEST_ID=BillDetails.Guest_ID) as BGest_ID,(select GuestName from GuestInformation where GUEST_ID=BillDetails.Guest_ID) as BGuestName   from [dbo].[GetRoomView_vw] left join BillDetails on [GetRoomView_vw].ROOM_ID=BillDetails.Room_ID     order by  Room_No, ROOM_TYPE_ID ")

        '    txtSearch.Text = ""
        'End If
        If txtSearch.Text IsNot "" Then
            If IsNumeric(txtSearch.Text) Then
                sqlDTx = FetchDataByProcedure("dbo.[GetRoomViewByGuestId]", txtSearch.Text.Trim)
            Else
                If txtSearch.Text.Length >= 3 Then
                    sqlDTx = FetchDataByProcedure("dbo.[GetRoomViewByGestId]", txtSearch.Text.Trim)
                    If sqlDTx.Rows.Count = 0 Then
                        sqlDTx = FetchDataByProcedure("dbo.[GetRoomViewByGuestName]", txtSearch.Text.Trim)
                    End If
                End If
            End If
        Else
            sqlDTx = FetchDataByProcedure("[dbo].[GetRoomView]", txtSearch.Text.Trim)

            txtSearch.Text = ""
        End If
    End Sub
    Public Function GetRentHours(ByVal RoomId As Integer) As String

        Try
            Dim BillTbl As DataSet = New DataSet
            Dim sqlCon As New SqlConnection(CnString)
            Dim sqlDA As New SqlDataAdapter("Select top(1) RoomRent.RENT_ID, RoomRent.Room_Hours, RoomRent.Room_Rent FROM    RoomRent INNER JOIN     BillDetails ON RoomRent.RENT_ID = BillDetails.Rent_ID where BillDetails.Room_ID='" & RoomId & "' order by BillDetails.BILL_ID desc", sqlCon)
            Dim sqlCB As New SqlCommandBuilder(sqlDA)
            sqlDA.Fill(BillTbl)
            If BillTbl IsNot Nothing Then
                If BillTbl.Tables(0).Rows.Count > 0 Then
                    rentHours = BillTbl.Tables(0).Rows(0)("Room_Hours").ToString
                    If rentHours = "1" Then
                        rentHours = rentHours + " hora"
                    Else
                        rentHours = rentHours + " horas"
                    End If
                End If
            End If

        Catch ex As Exception
        End Try
        Return rentHours
    End Function
    Public Function GetMaidCmments(ByVal RoomId As Integer) As String

        Try
            Dim GuestTbl As DataSet = New DataSet
            Dim sqlCon As New SqlConnection(CnString)
            Dim sqlDA As New SqlDataAdapter(" select top(1) Arrival_n_Departure_Date_Time as Comments from GuestInformation  
						 WHERE  ROOM_ID = " & RoomId & " and (Arrival_n_Departure_Date_Time!='' or Arrival_n_Departure_Date_Time!='-') order by Check_Out_Date desc,Check_Out_Time desc", sqlCon)
            Dim sqlCB As New SqlCommandBuilder(sqlDA)
            sqlDA.Fill(GuestTbl)
            If GuestTbl IsNot Nothing Then
                If GuestTbl.Tables(0).Rows.Count > 0 Then
                    MaidCommentsIcon = GuestTbl.Tables(0).Rows(0)("Comments").ToString
                    If MaidCommentsIcon IsNot "" Then
                        MaidCommentsIcon = "<img  src='Assets/Img/exclamation-mark.png' height='32' width='32' class='pull-right' title='" & MaidCommentsIcon & "'/>"
                    Else
                        MaidCommentsIcon = ""
                    End If
                End If
            End If

        Catch ex As Exception
        End Try
        Return rentHours
    End Function

    Protected Sub btnRoomChange_Click(sender As Object, e As EventArgs) Handles btnRoomChange.Click

        If Request.QueryString("croom_id") IsNot Nothing Then
            changeRoomId = Request.QueryString("croom_id")
        End If


        If IsNumeric(hdnNewRoomId.Value) And IsNumeric(hdnGuestId.Value) Then
            ExecuteSqlQuery("UPDATE    GuestInformation   SET   ROOM_ID = " & hdnNewRoomId.Value & "  WHERE   (GUEST_ID=" & hdnGuestId.Value & ")")
            ExecuteSqlQuery("UPDATE    BillDetails   SET   ROOM_ID = " & hdnNewRoomId.Value & ",Rent_ID=(select  Rent_Id from RoomRent where ROOM_ID=" & hdnNewRoomId.Value & " and Room_Hours=(select  Room_Hours from RoomRent where RENT_ID=(Select top 1 Rent_ID from BillDetails where Room_ID =" & changeRoomId & " order by BILL_ID desc))) WHERE   (Room_ID = " & changeRoomId & ") AND (Guest_ID=" & hdnGuestId.Value & ")")
            ExecuteSqlQuery("UPDATE    RoomDetails   SET    Occupied='N',  GUEST_ID =  0,Status_ID=1  WHERE   (ROOM_ID = " & changeRoomId & ") AND (GUEST_ID=" & hdnGuestId.Value & ")")
            ExecuteSqlQuery("UPDATE    RoomDetails   SET      Occupied='Y',GUEST_ID =  " & hdnGuestId.Value & ",Status_ID=2  WHERE   (ROOM_ID = " & hdnNewRoomId.Value & ")")
            ExecuteSqlQuery("INSERT  INTO   ChangeRoom(GUEST_ID, FROM_ROOM_ID, NEW_ROOM_ID, Reason, Room_Alter_Date) VALUES (" & hdnGuestId.Value & "," & changeRoomId & "," & hdnNewRoomId.Value & ",'Change Room','" & Now.Date & "')")
            lblMessege.Text = "Room has been changed successfully."
            Session("FlushMessage") = "Room has been changed successfully."
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "showModal();", True)
            Session("FlushMessage") = Nothing
            Response.Redirect("RoomView")

        End If
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        If txtSearch.Text IsNot "" Then
            If IsNumeric(txtSearch.Text) Then
                ExecQuery("SELECT        RoomDetails.ROOM_ID, RoomType.ROOM_TYPE, RoomDetails.Room_No, RoomDetails.Room_Rent, RoomDetails.Occupied, GuestInformation.GUEST_ID, GuestInformation.GuestName, GuestInformation.Check_In_Date, 
                         GuestInformation.Check_In_Time, GuestInformation.Check_Out_Date, GuestInformation.Check_Out_Time, GuestInformation.Status, RoomStatus.Status_ID, RoomStatus.Status AS Room_Status, RoomStatus.En_Status, RoomStatus.Status_Color,	

case when (datediff(minute,dateadd(hour,-5,GETUTCDATE()),Convert(datetime, (Convert(varchar(20),GuestInformation.Check_Out_Date)+' '+GuestInformation.Check_Out_Time)))/60)>0 then
convert(varchar(20),(datediff(minute,dateadd(hour,-5,GETUTCDATE()),Convert(datetime, (Convert(varchar(20),GuestInformation.Check_Out_Date)+' '+GuestInformation.Check_Out_Time)))/60))+' hours ' 
else ''
end as rhour,

case when  (datediff(minute,dateadd(hour,-5,GETUTCDATE()),Convert(datetime, (Convert(varchar(20),GuestInformation.Check_Out_Date)+' '+GuestInformation.Check_Out_Time)))%60)>0 then
convert(varchar(20),(datediff(minute,dateadd(hour,-5,GETUTCDATE()),Convert(datetime, (Convert(varchar(20),GuestInformation.Check_Out_Date)+' '+Check_Out_Time)))%60))+' mins '
else ''
end as rmin,
 (select top(1) case when  Promotion='Hora Gratis' then Promotion else '' end as promo from BillDetails where Room_ID=RoomDetails.ROOM_ID and Guest_ID=GuestInformation.GUEST_ID order by BILL_ID desc) as promo
FROM            RoomDetails INNER JOIN
                         RoomStatus ON RoomDetails.Status_ID = RoomStatus.Status_ID LEFT OUTER JOIN
                         GuestInformation ON RoomDetails.GUEST_ID = GuestInformation.GUEST_ID LEFT OUTER JOIN
                         RoomType ON RoomDetails.ROOM_TYPE_ID = RoomType.ROOM_TYPE_ID
						   Where (GuestInformation.GUEST_ID=" & txtSearch.Text.Trim & ") 
						 group by  RoomDetails.ROOM_ID, RoomType.ROOM_TYPE, RoomDetails.Room_No, RoomDetails.Room_Rent, RoomDetails.Occupied, GuestInformation.GUEST_ID, GuestInformation.GuestName, GuestInformation.Check_In_Date, 
                         GuestInformation.Check_In_Time, GuestInformation.Check_Out_Date,GuestInformation.Check_Out_Time, GuestInformation.Status, RoomStatus.Status_ID, RoomStatus.Status, RoomStatus.En_Status, RoomStatus.Status_Color,RoomDetails.Room_No,RoomDetails.ROOM_TYPE_ID  ORDER BY RoomDetails.Room_No,RoomDetails.ROOM_TYPE_ID ")
            Else
                If txtSearch.Text.Length >= 3 Then
                    ExecQuery("SELECT        RoomDetails.ROOM_ID, RoomType.ROOM_TYPE, RoomDetails.Room_No, RoomDetails.Room_Rent, RoomDetails.Occupied, GuestInformation.GUEST_ID, GuestInformation.GuestName, GuestInformation.Check_In_Date, 
                         GuestInformation.Check_In_Time, GuestInformation.Check_Out_Date, GuestInformation.Check_Out_Time, GuestInformation.Status, RoomStatus.Status_ID, RoomStatus.Status AS Room_Status, RoomStatus.En_Status, RoomStatus.Status_Color,	

case when (datediff(minute,dateadd(hour,-5,GETUTCDATE()),Convert(datetime, (Convert(varchar(20),GuestInformation.Check_Out_Date)+' '+GuestInformation.Check_Out_Time)))/60)>0 then
convert(varchar(20),(datediff(minute,dateadd(hour,-5,GETUTCDATE()),Convert(datetime, (Convert(varchar(20),GuestInformation.Check_Out_Date)+' '+GuestInformation.Check_Out_Time)))/60))+' hours ' 
else ''
end as rhour,

case when  (datediff(minute,dateadd(hour,-5,GETUTCDATE()),Convert(datetime, (Convert(varchar(20),GuestInformation.Check_Out_Date)+' '+GuestInformation.Check_Out_Time)))%60)>0 then
convert(varchar(20),(datediff(minute,dateadd(hour,-5,GETUTCDATE()),Convert(datetime, (Convert(varchar(20),GuestInformation.Check_Out_Date)+' '+Check_Out_Time)))%60))+' mins '
else ''
end as rmin,
 (select top(1) case when  Promotion='Hora Gratis' then Promotion else '' end as promo from BillDetails where Room_ID=RoomDetails.ROOM_ID and Guest_ID=GuestInformation.GUEST_ID order by BILL_ID desc) as promo
FROM            RoomDetails INNER JOIN
                         RoomStatus ON RoomDetails.Status_ID = RoomStatus.Status_ID LEFT OUTER JOIN
                         GuestInformation ON RoomDetails.GUEST_ID = GuestInformation.GUEST_ID LEFT OUTER JOIN
                         RoomType ON RoomDetails.ROOM_TYPE_ID = RoomType.ROOM_TYPE_ID
						 
  Where (GuestInformation.GuestName Like '%" & txtSearch.Text.Trim & "%') 
						 group by  RoomDetails.ROOM_ID, RoomType.ROOM_TYPE, RoomDetails.Room_No, RoomDetails.Room_Rent, RoomDetails.Occupied, GuestInformation.GUEST_ID, GuestInformation.GuestName, GuestInformation.Check_In_Date, 
                         GuestInformation.Check_In_Time, GuestInformation.Check_Out_Date,GuestInformation.Check_Out_Time, GuestInformation.Status, RoomStatus.Status_ID, RoomStatus.Status, RoomStatus.En_Status, RoomStatus.Status_Color,RoomDetails.Room_No,RoomDetails.ROOM_TYPE_ID  ORDER BY RoomDetails.Room_No,RoomDetails.ROOM_TYPE_ID ")

                End If
            End If

        Else
            ExecQuery("SELECT        RoomDetails.ROOM_ID, RoomType.ROOM_TYPE, RoomDetails.Room_No, RoomDetails.Room_Rent, RoomDetails.Occupied, GuestInformation.GUEST_ID, GuestInformation.GuestName, GuestInformation.Check_In_Date, 
                         GuestInformation.Check_In_Time, GuestInformation.Check_Out_Date, GuestInformation.Check_Out_Time, GuestInformation.Status, RoomStatus.Status_ID, RoomStatus.Status AS Room_Status, RoomStatus.En_Status, RoomStatus.Status_Color,	

case when (datediff(minute,dateadd(hour,-5,GETUTCDATE()),Convert(datetime, (Convert(varchar(20),GuestInformation.Check_Out_Date)+' '+GuestInformation.Check_Out_Time)))/60)>0 then
convert(varchar(20),(datediff(minute,dateadd(hour,-5,GETUTCDATE()),Convert(datetime, (Convert(varchar(20),GuestInformation.Check_Out_Date)+' '+GuestInformation.Check_Out_Time)))/60))+' hours ' 
else ''
end as rhour,

case when  (datediff(minute,dateadd(hour,-5,GETUTCDATE()),Convert(datetime, (Convert(varchar(20),GuestInformation.Check_Out_Date)+' '+GuestInformation.Check_Out_Time)))%60)>0 then
convert(varchar(20),(datediff(minute,dateadd(hour,-5,GETUTCDATE()),Convert(datetime, (Convert(varchar(20),GuestInformation.Check_Out_Date)+' '+Check_Out_Time)))%60))+' mins '
else ''
end as rmin,
 (select top(1) case when  Promotion='Hora Gratis' then Promotion else '' end as promo from BillDetails where Room_ID=RoomDetails.ROOM_ID and Guest_ID=GuestInformation.GUEST_ID order by BILL_ID desc) as promo
FROM            RoomDetails INNER JOIN
                         RoomStatus ON RoomDetails.Status_ID = RoomStatus.Status_ID LEFT OUTER JOIN
                         GuestInformation ON RoomDetails.GUEST_ID = GuestInformation.GUEST_ID LEFT OUTER JOIN
                         RoomType ON RoomDetails.ROOM_TYPE_ID = RoomType.ROOM_TYPE_ID
						 group by  RoomDetails.ROOM_ID, RoomType.ROOM_TYPE, RoomDetails.Room_No, RoomDetails.Room_Rent, RoomDetails.Occupied, GuestInformation.GUEST_ID, GuestInformation.GuestName, GuestInformation.Check_In_Date, 
                         GuestInformation.Check_In_Time, GuestInformation.Check_Out_Date,GuestInformation.Check_Out_Time, GuestInformation.Status, RoomStatus.Status_ID, RoomStatus.Status, RoomStatus.En_Status, RoomStatus.Status_Color,RoomDetails.Room_No,RoomDetails.ROOM_TYPE_ID  ORDER BY RoomDetails.Room_No,RoomDetails.ROOM_TYPE_ID ")

        End If
    End Sub



    <WebMethod()>
    Public Shared Sub Save(ByVal foodService As List(Of FoodServiceSale), ByVal uid As Integer)

        Dim BillId As Integer
        Dim sumTotal As Integer
        Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("FoodServiceInsert")
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@UserId", uid)
                cmd.Connection = con
                con.Open()
                BillId = Convert.ToInt32(cmd.ExecuteScalar())
            End Using

            For Each fItems In foodService
                If fItems.Quantity > 0 Then
                    Using cmd As New SqlCommand("FoodServiceDetailsInsert")
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@BillId", BillId)
                        cmd.Parameters.AddWithValue("@ITEM_ID", fItems.Id)
                        cmd.Parameters.AddWithValue("@Quantity", fItems.Quantity)
                        cmd.Connection = con
                        sumTotal = sumTotal + Convert.ToInt32(cmd.ExecuteScalar())
                    End Using

                End If
            Next fItems


            con.Close()
            ExecuteSqlQuery("UPDATE    FoodServices   SET      G_TOTAL=" & sumTotal & ",ITEM_COST =  " & sumTotal & "  WHERE   (BILL_NO = " & BillId & ")")

        End Using
    End Sub

    <WebMethod()>
    Public Shared Sub Eliminate(ByVal foodService As List(Of FoodServiceSale), ByVal uid As Integer)
        Dim BillId As Integer
        Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
        Using con As New SqlConnection(constr)
            For Each fItems In foodService
                If fItems.Quantity > 0 Then
                    Using cmd As New SqlCommand("FoodServiceUpdate")
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@UserId", uid)
                        cmd.Parameters.AddWithValue("@ITEM_ID", fItems.Id)
                        cmd.Parameters.AddWithValue("@Quantity", fItems.Quantity)
                        cmd.Connection = con
                        If con.State = ConnectionState.Closed Then
                            con.Open()
                        End If

                        BillId = Convert.ToInt32(cmd.ExecuteScalar())
                    End Using

                End If
            Next fItems
            con.Close()

        End Using
    End Sub

    'Protected Sub btnFoodDelete_Click(sender As Object, e As EventArgs) Handles btnFoodDelete.Click
    '    Dim BillId As Integer
    '    Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
    '    Using con As New SqlConnection(constr)
    '        Using cmd As New SqlCommand("FoodServiceDelete")
    '            cmd.CommandType = CommandType.StoredProcedure
    '            cmd.Parameters.AddWithValue("@UserId", hdnUserId.Value)
    '            cmd.Connection = con
    '            con.Open()
    '            BillId = Convert.ToInt32(cmd.ExecuteScalar())

    '        End Using
    '        con.Close()
    '    End Using

    '    If BillId > 0 Then
    '        lblMessege.Text = "Existing items deleted successfully."
    '        Session("FlushMessage") = "Existing items deleted successfully."

    '    End If

    '    Response.Redirect("RoomView")

    'End Sub
End Class

Public Class FoodServiceSale
    Public Property Id As Integer
    Public Property Quantity As String
End Class
