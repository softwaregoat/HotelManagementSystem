Imports System.IO

Public Class GuestList
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

            Dim ds As DataSet = Nothing
            If Request.QueryString("act") IsNot Nothing Then
                If Request.QueryString("act") = "cnl" Then
                    If IsNumeric(Request.QueryString("id")) Then
                        Dim GUEST_ID As Double = ConvertingNumbers(Request.QueryString("id"))
                        ExecuteSqlQuery("UPDATE GuestInformation SET Status='Cancelled' , ROOM_ID = 0, Rent_Day = 0, Total_Charges = 0 WHERE  (GUEST_ID = " & GUEST_ID & ") ")
                        HOTEL_SERVICE_CHARGE_CALCULATION(GUEST_ID)
                        RoomBooking(GUEST_ID, 0)
                        Session("FlushMessage") = "Guest information successfully cancelled."
                        Response.Redirect("GuestList.aspx")
                    End If
                    '---------------------------------------
                Else
                    Response.Redirect("GuestList.aspx")
                End If
            End If

            sqlStr = " SELECT  Top(100)   GuestInformation.GUEST_ID,GuestInformation.GEST_ID, GuestInformation.GuestName, GuestInformation.Gender, GuestInformation.Address , GuestInformation.PhoneNo, 

REPLACE(GuestInformation.Social_Type,'List','') as Social_Type,GuestInformation.Address as Nationality, GuestInformation.Note,

GuestInformation.Check_In_Date, 
                      RoomType.ROOM_TYPE, RoomDetails.Room_No, GuestInformation.Status, (select count(*) from GuestInformation) as totalGuest
                      FROM     RoomType RIGHT OUTER JOIN RoomDetails ON RoomType.ROOM_TYPE_ID = RoomDetails.ROOM_TYPE_ID RIGHT OUTER JOIN
                      GuestInformation ON RoomDetails.ROOM_ID = GuestInformation.ROOM_ID ORDER BY GuestInformation.GUEST_ID DESC  "

            ds = FetchDataFromTable(sqlStr)
            If ds.Tables(0).Rows.Count > 0 Then
                lblCount.Text = "Clientes a la fecha: " + ds.Tables(0).Rows(0)("totalGuest").ToString
            End If

            GridView1.DataSource = ds
            GridView1.DataBind()


            If (Not (Session("FlushMessage")) Is Nothing) Then
                lblMessege.Text = Session("FlushMessage").ToString()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "showModal();", True)
                Session("FlushMessage") = Nothing
            End If

        End If
    End Sub

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
        Dim ds As DataSet = Nothing
        If txtSearch.Text IsNot "" Then
            If IsNumeric(txtSearch.Text) Then
                sqlStr = " SELECT  Top(100)   GuestInformation.GUEST_ID,GuestInformation.GEST_ID, GuestInformation.GuestName, GuestInformation.Gender, GuestInformation.Address , GuestInformation.PhoneNo, 

GuestInformation.Social_Type,GuestInformation.Address as Nationality, GuestInformation.Note,

GuestInformation.Check_In_Date, 
                      RoomType.ROOM_TYPE, RoomDetails.Room_No, GuestInformation.Status
                      FROM     RoomType RIGHT OUTER JOIN RoomDetails ON RoomType.ROOM_TYPE_ID = RoomDetails.ROOM_TYPE_ID RIGHT OUTER JOIN
                      GuestInformation ON RoomDetails.ROOM_ID = GuestInformation.ROOM_ID where GuestInformation.GUEST_ID=" & txtSearch.Text.Trim & " ORDER BY GuestInformation.GUEST_ID DESC "
                ds = FetchDataFromTable(sqlStr)

            Else
                If txtSearch.Text.Length >= 3 Then
                    sqlStr = " SELECT  Top(100)   GuestInformation.GUEST_ID,GuestInformation.GEST_ID, GuestInformation.GuestName, GuestInformation.Gender, GuestInformation.Address , GuestInformation.PhoneNo, 

GuestInformation.Social_Type,GuestInformation.Address as Nationality, GuestInformation.Note,

GuestInformation.Check_In_Date, 
                      RoomType.ROOM_TYPE, RoomDetails.Room_No, GuestInformation.Status
                      FROM     RoomType RIGHT OUTER JOIN RoomDetails ON RoomType.ROOM_TYPE_ID = RoomDetails.ROOM_TYPE_ID RIGHT OUTER JOIN
                      GuestInformation ON RoomDetails.ROOM_ID = GuestInformation.ROOM_ID where GuestInformation.GEST_ID Like '%" & txtSearch.Text.Trim & "%' ORDER BY GuestInformation.GUEST_ID DESC "
                    ds = FetchDataFromTable(sqlStr)
                    If ds.Tables(0).Rows.Count = 0 Then
                        sqlStr = " SELECT  Top(100)   GuestInformation.GUEST_ID,GuestInformation.GEST_ID, GuestInformation.GuestName, GuestInformation.Gender, GuestInformation.Address , GuestInformation.PhoneNo, 

GuestInformation.Social_Type,GuestInformation.Address as Nationality, GuestInformation.Note,

GuestInformation.Check_In_Date, 
                      RoomType.ROOM_TYPE, RoomDetails.Room_No, GuestInformation.Status
                      FROM     RoomType RIGHT OUTER JOIN RoomDetails ON RoomType.ROOM_TYPE_ID = RoomDetails.ROOM_TYPE_ID RIGHT OUTER JOIN
                      GuestInformation ON RoomDetails.ROOM_ID = GuestInformation.ROOM_ID where GuestInformation.GuestName Like'%" & txtSearch.Text.Trim & "%' ORDER BY GuestInformation.GUEST_ID DESC "
                        ds = FetchDataFromTable(sqlStr)
                    End If
                End If
            End If
        Else
            sqlStr = " SELECT  Top(100)   GuestInformation.GUEST_ID,GuestInformation.GEST_ID, GuestInformation.GuestName, GuestInformation.Gender, GuestInformation.Address , GuestInformation.PhoneNo, 

GuestInformation.Social_Type,GuestInformation.Address as Nationality, GuestInformation.Note,

GuestInformation.Check_In_Date, 
                      RoomType.ROOM_TYPE, RoomDetails.Room_No, GuestInformation.Status
                      FROM     RoomType RIGHT OUTER JOIN RoomDetails ON RoomType.ROOM_TYPE_ID = RoomDetails.ROOM_TYPE_ID RIGHT OUTER JOIN
                      GuestInformation ON RoomDetails.ROOM_ID = GuestInformation.ROOM_ID ORDER BY GuestInformation.GUEST_ID DESC "
            ds = FetchDataFromTable(sqlStr)
            txtSearch.Text = ""
        End If
        'If ds.Tables(0).Rows.Count > 0 Then
        '    lblCount.Text = "Clientes a la fecha: " + ds.Tables(0).Rows.Count.ToString
        'End If

        GridView1.DataSource = ds
        GridView1.DataBind()

    End Sub

    'Protected Sub txtDate_TextChanged(sender As Object, e As EventArgs) Handles txtDate.TextChanged

    '    sqlStr = " select count(*) as totalGuest from GuestInformation where Convert(date,Register_Date)='" & txtDate.Text & "'   "

    '    Dim ds As DataSet = FetchDataFromTable(sqlStr)
    '    If ds.Tables(0).Rows.Count > 0 Then
    '        lblCount2.Text = ds.Tables(0).Rows(0)("totalGuest").ToString
    '    End If
    'End Sub

End Class