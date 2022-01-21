Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Web.Services

Public Class check_out
    Inherits System.Web.UI.Page
    Protected strRoomId As String
    Protected guestIdChangeEvent As Boolean
    Protected TaxInfo As DataSet
    Protected StatusId As Integer
    Protected strRate As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            ddlHours.AutoPostBack = True
            ddlPromotion.AutoPostBack = True
            If Not Me.Page.User.Identity.IsAuthenticated Or Session("LOGGED_USER_ID") Is Nothing Then
                FormsAuthentication.RedirectToLoginPage()
            End If
            Dim currentPageFileName As String = New FileInfo(Me.Request.Url.LocalPath).Name
            Try
                'CheckPermissions(currentPageFileName.Split(".aspx")(0).ToString, ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)))
                ddlUsers.DataSource = FetchDataFromTable("SELECT  *,(FirstName +' '+ LastName) as FullName  FROM  UserInformation where ROLE_ID=2")
                ddlUsers.DataTextField = "FullName"
                ddlUsers.DataValueField = "USER_ID"
                ddlUsers.DataBind()
                ddlUsers.Items.Insert(0, New ListItem("Please select", 0))

                TaxInfo = FetchDataFromTable("SELECT * FROM TAXInfo")
                hdnTaxPercent.Value = Convert.ToDouble(TaxInfo.Tables(0).Rows(0)("Rate_1").ToString())

                ddlRoomNo.DataSource = FetchDataFromTable("SELECT ROOM_ID, Occupied, Room_No AS RoomType FROM RoomDetails")
                ddlRoomNo.DataTextField = "RoomType"
                ddlRoomNo.DataValueField = "ROOM_ID"
                ddlRoomNo.DataBind()
                ddlRoomNo.Items.Insert(0, New ListItem("Please select"))


                ddlPromotion.DataSource = FetchDataFromTable("SELECT * FROM [Promotions]")
                ddlPromotion.DataTextField = "Promotion"
                ddlPromotion.DataValueField = "Promotion_ID"
                ddlPromotion.DataBind()
                ddlPromotion.Items.Insert(0, New ListItem("Please select Promoción"))

                strRoomId = Request.QueryString("room_id")
                strRate = Request.QueryString("rate")
                If Request.QueryString("room_id") IsNot Nothing Then
                    If IsNumeric(strRoomId) Then
                        sqlStr = " SELECT RoomDetails.ROOM_ID, RoomDetails.Room_No, RoomType.ROOM_TYPE, RoomDetails.Room_Rent, RoomDetails.Occupied, CASE WHEN RoomDetails.Occupied = 'Y' THEN 'Ocupada' ELSE 'Disponible' END AS OldStatus, 
                         RoomStatus.Status AS Status, RoomStatus.En_Status, RoomStatus.Status_Color,RoomDetails.Status_ID
FROM            RoomDetails INNER JOIN
                         RoomStatus ON RoomDetails.Status_ID = RoomStatus.Status_ID LEFT OUTER JOIN
                         RoomType ON RoomDetails.ROOM_TYPE_ID = RoomType.ROOM_TYPE_ID
  WHERE        (RoomDetails.ROOM_ID = " & strRoomId & ") "
                        Dim SelectedRoomInfo As DataSet = FetchDataFromTable(sqlStr)
                        If SelectedRoomInfo IsNot Nothing Then
                            ddlRoomNo.SelectedValue = SelectedRoomInfo.Tables(0).Rows(0)("ROOM_ID").ToString
                            'roomstatus.Text = "-" + SelectedRoomInfo.Tables(0).Rows(0)("Status").ToString 
                            'roomstatus.Text = "-" + "<b><span style='color:" + SelectedRoomInfo.Tables(0).Rows(0)("Status_Color").ToString + ";'>" + SelectedRoomInfo.Tables(0).Rows(0)("Status").ToString + "</span></b>"
                            StatusId = ConvertingNumbers(SelectedRoomInfo.Tables(0).Rows(0)("Status_ID").ToString)
                            hdnStatusId.Value = StatusId

                        End If

                        'roomstatus.ForeColor = System.Drawing.Color.FromArgb(SelectedRoomInfo.Tables(0).Rows(0)("Status_Color").ToString)
                        hdnRoomId.Value = strRoomId
                        ddlPromotion.DataSource = FetchDataFromTable("SELECT * FROM [Promotions]")
                        ddlPromotion.DataTextField = "Promotion"
                        ddlPromotion.DataValueField = "Promotion_ID"
                        ddlPromotion.DataBind()
                        ddlPromotion.Items.Insert(0, New ListItem("Please select Promoción"))

                        ddlHours.DataSource = FetchDataFromTable("SELECT [RENT_ID], [ROOM_ID],Room_Hours,Room_Rent FROM [RoomRent] where (ROOM_ID = " & strRoomId & ")")
                        ddlHours.DataTextField = "Room_Hours"
                        ddlHours.DataValueField = "RENT_ID"
                        ddlHours.DataBind()
                        ddlHours.Items.Insert(0, New ListItem("Please select hour(s)"))


                        sqlStr = " select top(1) Arrival_n_Departure_Date_Time as Comments from GuestInformation  
                        WHERE  ROOM_ID = " & strRoomId & " and (Arrival_n_Departure_Date_Time!='' or Arrival_n_Departure_Date_Time!='-') order by Check_Out_Date desc,Check_Out_Time desc"
                        Dim SelectedMaidComment As DataSet = FetchDataFromTable(sqlStr)

                        If SelectedMaidComment IsNot Nothing Then
                            If SelectedMaidComment.Tables(0).Rows.Count > 0 Then

                                txtArrivalDeparture.Text = SelectedMaidComment.Tables(0).Rows(0)("Comments").ToString.Trim()
                                If txtArrivalDeparture.Text IsNot Nothing And txtArrivalDeparture.Text IsNot "" Then
                                    Image1.Visible = True
                                    Image1.ToolTip = txtArrivalDeparture.Text
                                Else
                                    Image1.Visible = False
                                End If

                            End If
                        End If


                        Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
                        Using con As New SqlConnection(constr)
                            If Request.QueryString("rate") IsNot Nothing Then
                                If IsNumeric(strRate) Then
                                    Dim IdNo = Request.QueryString("idno").ToString()
                                    Using sqlCommand As New SqlCommand("GetGuestInfoByRoomID_Edit")
                                        sqlCommand.CommandType = CommandType.StoredProcedure
                                        sqlCommand.Parameters.AddWithValue("@RoomID", strRoomId)
                                        sqlCommand.Parameters.AddWithValue("@Rate", strRate)
                                        sqlCommand.Parameters.AddWithValue("@IdNo", IdNo)
                                        sqlCommand.Connection = con
                                        con.Open()
                                        Dim sqlReader As SqlDataReader = sqlCommand.ExecuteReader()

                                        If sqlReader.HasRows Then

                                            While (sqlReader.Read())
                                                hdnGuestId.Value = sqlReader.GetValue(0)
                                                txtGuestID.Text = sqlReader.GetValue(1)
                                                txtGuestName.Text = sqlReader.GetValue(2)
                                                txtAddress.Text = sqlReader.GetValue(3)

                                                If sqlReader.GetValue(4) IsNot "" Then
                                                    CheckBoxList1.SelectedValue = CheckBoxList1.Items.FindByText("Lista " + sqlReader.GetValue(4)).Value
                                                End If

                                                If sqlReader.GetValue(5) IsNot "" Then
                                                    ddlNormalSpecial.SelectedValue = ddlNormalSpecial.Items.FindByText(sqlReader.GetValue(5)).Value
                                                End If

                                                txtNote.Value = sqlReader.GetValue(6)

                                                ddlIDType.SelectedValue = ddlIDType.Items.FindByText(sqlReader.GetValue(7)).Value
                                                txtIDNo.Text = sqlReader.GetValue(8)
                                                ddlGender.SelectedValue = ddlGender.Items.FindByText(sqlReader.GetValue(9)).Value
                                                txtPhoneNo.Text = sqlReader.GetValue(10)
                                                txtArrivalDeparture.Text = sqlReader.GetValue(11)
                                                txtPurpose.Text = sqlReader.GetValue(12)
                                                txtCheckInDate.Text = sqlReader.GetValue(13) + " " + sqlReader.GetValue(14)
                                                txtCheckInTime.Text = sqlReader.GetValue(14)
                                                txtCheckOutDate.Text = sqlReader.GetValue(15) + " " + sqlReader.GetValue(16)
                                                txtCheckOutTime.Text = sqlReader.GetValue(16)
                                                lblremaintime.Text = sqlReader.GetValue(17) + " " + sqlReader.GetValue(18)
                                                If lblremaintime.Text.Trim IsNot "" Then
                                                    remaintext.Visible = True
                                                End If

                                            End While


                                        End If

                                        sqlReader.Close()

                                    End Using
                                    If IsNumeric(hdnGuestId.Value) Then

                                        Using sqlComm2 As New SqlCommand("GetBillInfoByGuestIDRoomID_Edit")
                                            sqlComm2.CommandType = CommandType.StoredProcedure
                                            sqlComm2.Parameters.AddWithValue("@RoomID", strRoomId)
                                            sqlComm2.Parameters.AddWithValue("@GuestID", ConvertingNumbers(hdnGuestId.Value))
                                            sqlComm2.Parameters.AddWithValue("@Rate", strRate)
                                            sqlComm2.Connection = con
                                            Dim billTbl As DataSet = New DataSet
                                            Dim sqlDA As New SqlDataAdapter(sqlComm2)
                                            Dim sqlCB As New SqlCommandBuilder(sqlDA)
                                            sqlDA.Fill(billTbl)
                                            If billTbl IsNot Nothing Then
                                                If billTbl.Tables(0).Rows.Count > 0 Then

                                                    btnCheckInSubmit.Text = "Actualizar"
                                                    'lnkChangeRoom.NavigateUrl = "RoomView.aspx?croom_id=" + strRoomId
                                                    'lnkChangeRoom.Enabled = True
                                                    'lnkChangeRoom.Visible = True
                                                    'lnkBlockRoom.Enabled = True
                                                    gvBill.DataSource = billTbl.Tables(0)
                                                    gvBill.DataBind()
                                                    'ddlHours.SelectedValue = ConvertingNumbers(billTbl.Tables(0).Rows(0)(11).ToString)
                                                    If billTbl.Tables(0).Rows(0)(7).ToString IsNot "" Then
                                                        Dim pv As String = ddlPromotion.Items.FindByText(billTbl.Tables(0).Rows(0)(7).ToString).Value
                                                        ddlPromotion.SelectedValue = ddlPromotion.Items.FindByText(billTbl.Tables(0).Rows(0)(7).ToString).Value
                                                    End If

                                                    ddlPayType.SelectedValue = ddlPayType.Items.FindByText(billTbl.Tables(0).Rows(0)(4).ToString).Value
                                                    txtRoomTax.Text = billTbl.Tables(0).Rows(0)(5).ToString
                                                    txtRoomDiscount.Text = billTbl.Tables(0).Rows(0)(6).ToString
                                                    txtRentTotal.Text = billTbl.Tables(0).Rows(0)(8).ToString
                                                    txtRoomRent.Text = billTbl.Tables(0).Rows(0)(18).ToString


                                                    gvBill.HeaderRow.Cells(4).Visible = False
                                                    gvBill.HeaderRow.Cells(11).Visible = False
                                                    gvBill.HeaderRow.Cells(12).Visible = False
                                                    gvBill.HeaderRow.Cells(13).Visible = False
                                                    gvBill.HeaderRow.Cells(14).Visible = False
                                                    gvBill.HeaderRow.Cells(15).Visible = False
                                                    gvBill.HeaderRow.Cells(16).Visible = False
                                                    gvBill.HeaderRow.Cells(17).Visible = False
                                                    gvBill.HeaderRow.Cells(18).Visible = False
                                                    gvBill.HeaderRow.Cells(19).Visible = False
                                                    gvBill.HeaderRow.Cells(20).Visible = False
                                                    gvBill.HeaderRow.Cells(21).Visible = False
                                                    gvBill.HeaderRow.Cells(22).Visible = False
                                                    gvBill.HeaderRow.Cells(23).Visible = False

                                                    gvBill.FooterRow.Cells(4).Visible = False
                                                    gvBill.FooterRow.Cells(11).Visible = False
                                                    gvBill.FooterRow.Cells(12).Visible = False
                                                    gvBill.FooterRow.Cells(13).Visible = False
                                                    gvBill.FooterRow.Cells(14).Visible = False
                                                    gvBill.FooterRow.Cells(15).Visible = False
                                                    gvBill.FooterRow.Cells(16).Visible = False
                                                    gvBill.FooterRow.Cells(17).Visible = False
                                                    gvBill.FooterRow.Cells(18).Visible = False
                                                    gvBill.FooterRow.Cells(19).Visible = False
                                                    gvBill.FooterRow.Cells(20).Visible = False
                                                    gvBill.FooterRow.Cells(21).Visible = False
                                                    gvBill.FooterRow.Cells(22).Visible = False
                                                    gvBill.FooterRow.Cells(23).Visible = False
                                                    For i As Integer = 0 To gvBill.Rows.Count - 1



                                                        gvBill.Rows(i).Cells(4).Visible = False
                                                        gvBill.Rows(i).Cells(11).Visible = False
                                                        gvBill.Rows(i).Cells(12).Visible = False
                                                        gvBill.Rows(i).Cells(13).Visible = False
                                                        gvBill.Rows(i).Cells(14).Visible = False
                                                        gvBill.Rows(i).Cells(15).Visible = False
                                                        gvBill.Rows(i).Cells(16).Visible = False
                                                        gvBill.Rows(i).Cells(17).Visible = False
                                                        gvBill.Rows(i).Cells(18).Visible = False
                                                        gvBill.Rows(i).Cells(19).Visible = False
                                                        gvBill.Rows(i).Cells(20).Visible = False
                                                        gvBill.Rows(i).Cells(21).Visible = False
                                                        gvBill.Rows(i).Cells(22).Visible = False
                                                        gvBill.Rows(i).Cells(23).Visible = False
                                                    Next
                                                End If
                                            End If

                                        End Using
                                    End If
                                End If
                            Else
                                Using sqlComm As New SqlCommand("GetGuestInfoByRoomID_CkOutEdit")
                                    sqlComm.CommandType = CommandType.StoredProcedure
                                    sqlComm.Parameters.AddWithValue("@RoomID", strRoomId)
                                    sqlComm.Parameters.AddWithValue("@ID_No", txtIDNo.Text.Trim)



                                    sqlComm.Connection = con
                                    con.Open()
                                    Dim sqlReader As SqlDataReader = sqlComm.ExecuteReader()

                                    If sqlReader.HasRows Then

                                        While (sqlReader.Read())
                                            hdnGuestId.Value = sqlReader.GetValue(0)
                                            txtGuestID.Text = sqlReader.GetValue(1)
                                            txtGuestName.Text = sqlReader.GetValue(2)
                                            txtAddress.Text = sqlReader.GetValue(3)

                                            If sqlReader.GetValue(4) IsNot "" Then
                                                CheckBoxList1.SelectedValue = CheckBoxList1.Items.FindByText(sqlReader.GetValue(4)).Value
                                            End If

                                            If sqlReader.GetValue(5) IsNot "" Then
                                                ddlNormalSpecial.SelectedValue = ddlNormalSpecial.Items.FindByText(sqlReader.GetValue(5)).Value
                                            End If

                                            txtNote.Value = sqlReader.GetValue(6)



                                            ddlIDType.SelectedValue = ddlIDType.Items.FindByText(sqlReader.GetValue(7)).Value
                                            txtIDNo.Text = sqlReader.GetValue(8)
                                            If sqlReader.GetValue(9) IsNot "" Then
                                                ddlGender.SelectedValue = ddlGender.Items.FindByText(sqlReader.GetValue(9)).Value
                                            End If

                                            txtPhoneNo.Text = sqlReader.GetValue(10)
                                            txtArrivalDeparture.Text = sqlReader.GetValue(11)
                                            txtPurpose.Text = sqlReader.GetValue(12)
                                            txtCheckInDate.Text = sqlReader.GetValue(13) + " " + sqlReader.GetValue(14)
                                            txtCheckInTime.Text = sqlReader.GetValue(14)
                                            txtCheckOutDate.Text = sqlReader.GetValue(15) + " " + sqlReader.GetValue(16)
                                            txtCheckOutTime.Text = sqlReader.GetValue(16)
                                            lblremaintime.Text = sqlReader.GetValue(17) + " " + sqlReader.GetValue(18)
                                            If lblremaintime.Text.Trim IsNot "" Then
                                                remaintext.Visible = True
                                            End If



                                        End While


                                    End If

                                    sqlReader.Close()

                                End Using
                                If IsNumeric(hdnGuestId.Value) Then

                                    Using sqlComm2 As New SqlCommand("GetBillInfoByGuestIDRoomID")
                                        sqlComm2.CommandType = CommandType.StoredProcedure
                                        sqlComm2.Parameters.AddWithValue("@RoomID", strRoomId)
                                        sqlComm2.Parameters.AddWithValue("@GuestID", ConvertingNumbers(hdnGuestId.Value))
                                        sqlComm2.Connection = con
                                        Dim billTbl As DataSet = New DataSet
                                        Dim sqlDA As New SqlDataAdapter(sqlComm2)
                                        Dim sqlCB As New SqlCommandBuilder(sqlDA)
                                        sqlDA.Fill(billTbl)
                                        If billTbl IsNot Nothing Then
                                            If billTbl.Tables(0).Rows.Count > 0 Then

                                                btnCheckInSubmit.Text = "Guardar"
                                                'lnkChangeRoom.NavigateUrl = "RoomView.aspx?croom_id=" + strRoomId
                                                'lnkChangeRoom.Enabled = True
                                                'lnkChangeRoom.Visible = True
                                                'lnkBlockRoom.Enabled = True
                                                gvBill.DataSource = billTbl.Tables(0)
                                                gvBill.DataBind()
                                                'ddlHours.SelectedValue = ConvertingNumbers(billTbl.Tables(0).Rows(0)(11).ToString)
                                                If billTbl.Tables(0).Rows(0)(7).ToString IsNot "" Then
                                                    ddlPromotion.SelectedValue = ddlPromotion.Items.FindByText(billTbl.Tables(0).Rows(0)(7).ToString).Value
                                                End If

                                                ddlPayType.SelectedValue = ddlPayType.Items.FindByText(billTbl.Tables(0).Rows(0)(4).ToString).Value
                                                txtRoomTax.Text = billTbl.Tables(0).Rows(0)(5).ToString
                                                txtRoomDiscount.Text = billTbl.Tables(0).Rows(0)(6).ToString
                                                txtRentTotal.Text = billTbl.Tables(0).Rows(0)(8).ToString
                                                txtRoomRent.Text = billTbl.Tables(0).Rows(0)(18).ToString


                                                gvBill.HeaderRow.Cells(4).Visible = False
                                                gvBill.HeaderRow.Cells(11).Visible = False
                                                gvBill.HeaderRow.Cells(12).Visible = False
                                                gvBill.HeaderRow.Cells(13).Visible = False
                                                gvBill.HeaderRow.Cells(14).Visible = False
                                                gvBill.HeaderRow.Cells(15).Visible = False
                                                gvBill.HeaderRow.Cells(16).Visible = False
                                                gvBill.HeaderRow.Cells(17).Visible = False
                                                gvBill.HeaderRow.Cells(18).Visible = False
                                                gvBill.HeaderRow.Cells(19).Visible = False
                                                gvBill.HeaderRow.Cells(20).Visible = False
                                                gvBill.HeaderRow.Cells(21).Visible = False
                                                gvBill.HeaderRow.Cells(22).Visible = False
                                                gvBill.HeaderRow.Cells(23).Visible = False

                                                gvBill.FooterRow.Cells(4).Visible = False
                                                gvBill.FooterRow.Cells(11).Visible = False
                                                gvBill.FooterRow.Cells(12).Visible = False
                                                gvBill.FooterRow.Cells(13).Visible = False
                                                gvBill.FooterRow.Cells(14).Visible = False
                                                gvBill.FooterRow.Cells(15).Visible = False
                                                gvBill.FooterRow.Cells(16).Visible = False
                                                gvBill.FooterRow.Cells(17).Visible = False
                                                gvBill.FooterRow.Cells(18).Visible = False
                                                gvBill.FooterRow.Cells(19).Visible = False
                                                gvBill.FooterRow.Cells(20).Visible = False
                                                gvBill.FooterRow.Cells(21).Visible = False
                                                gvBill.FooterRow.Cells(22).Visible = False
                                                gvBill.FooterRow.Cells(23).Visible = False
                                                For i As Integer = 0 To gvBill.Rows.Count - 1
                                                    gvBill.Rows(i).Cells(4).Visible = False
                                                    gvBill.Rows(i).Cells(11).Visible = False
                                                    gvBill.Rows(i).Cells(12).Visible = False
                                                    gvBill.Rows(i).Cells(13).Visible = False
                                                    gvBill.Rows(i).Cells(14).Visible = False
                                                    gvBill.Rows(i).Cells(15).Visible = False
                                                    gvBill.Rows(i).Cells(16).Visible = False
                                                    gvBill.Rows(i).Cells(17).Visible = False
                                                    gvBill.Rows(i).Cells(18).Visible = False
                                                    gvBill.Rows(i).Cells(19).Visible = False
                                                    gvBill.Rows(i).Cells(20).Visible = False
                                                    gvBill.Rows(i).Cells(21).Visible = False
                                                    gvBill.Rows(i).Cells(22).Visible = False
                                                    gvBill.Rows(i).Cells(23).Visible = False
                                                Next
                                            End If
                                        End If

                                        'Dim sqlReader2 As SqlDataReader = sqlComm2.ExecuteReader()

                                        'If sqlReader2.HasRows Then

                                        '    While (sqlReader2.Read())
                                        '        hdnGuestId.Value = sqlReader2.GetValue(0)
                                        '        txtGuestName.Text = sqlReader2.GetValue(1)
                                        '        txtAddress.Text = sqlReader2.GetValue(2)
                                        '        CheckBoxList1.SelectedValue = CheckBoxList1.Items.FindByText(sqlReader.GetValue(3)).Value
                                        '        ddlNormalSpecial.SelectedValue = ddlNormalSpecial.Items.FindByText(sqlReader.GetValue(4)).Value
                                        '        txtNote.Value = sqlReader.GetValue(5)
                                        '        ddlIDType.SelectedValue = ddlIDType.Items.FindByText(sqlReader.GetValue(6)).Value
                                        '        txtIDNo.Text = sqlReader.GetValue(7)
                                        '        ddlGender.SelectedValue = ddlGender.Items.FindByText(sqlReader.GetValue(8)).Value
                                        '        txtPhoneNo.Text = sqlReader.GetValue(9)
                                        '        txtArrivalDeparture.Text = sqlReader.GetValue(10)
                                        '        txtPurpose.Text = sqlReader.GetValue(11)
                                        '        txtCheckInDate.Text = sqlReader.GetValue(12)
                                        '        txtCheckInTime.Text = sqlReader.GetValue(13)
                                        '        txtCheckOutDate.Text = sqlReader.GetValue(14)
                                        '        txtCheckOutTime.Text = sqlReader.GetValue(15)
                                        '    End While


                                        'End If

                                    End Using
                                End If
                            End If



                            con.Close()
                        End Using

                    End If
                End If


            Catch ex As Exception

                Console.WriteLine(ex.Message)

            End Try

            'txtCheckInDate.Text = Now.AddHours(1).ToString("d/M/yyyy hh:mm tt")
            'txtCheckOutDate.Text = Now.AddHours(1).ToString("d/M/yyyy hh:mm tt")



            'If Request.QueryString("act") IsNot Nothing Then
            '    If Request.QueryString("act") = "edit" Then
            '        '---------------------------------------
            '        If IsNumeric(Request.QueryString("id")) Then
            '            btnCheckInSubmit.Text = "Guardar"
            '            hdnGuestId.Value = ConvertingNumbers(Request.QueryString("id"))
            '            Dim dt_cust As DataTable = QueryDataTable(" SELECT        GuestInformation.GUEST_ID, GuestInformation.GuestName, GuestInformation.Address, GuestInformation.PhoneNo, GuestInformation.ID_Type,
            '                                     GuestInformation.Gender, GuestInformation.Purpose, GuestInformation.Arrival_n_Departure_Date_Time, GuestInformation.Note, GuestInformation.Check_In_Date, GuestInformation.Check_In_Time, 
            '                                     GuestInformation.Check_Out_Date, GuestInformation.Check_Out_Time, GuestInformation.No_Of_Day, GuestInformation.ROOM_ID, GuestInformation.Rent_Day, GuestInformation.Total_Charges, 
            '                                     GuestInformation.No_Of_Adult, GuestInformation.No_Of_Children, GuestInformation.Extra_Beds, GuestInformation.Per_Bed_Cost, GuestInformation.Total_Extra_Bed_Cost, RoomType.ROOM_TYPE
            '                                     FROM     RoomType INNER JOIN RoomDetails ON RoomType.ROOM_TYPE_ID = RoomDetails.ROOM_TYPE_ID RIGHT OUTER JOIN GuestInformation ON RoomDetails.ROOM_ID = GuestInformation.ROOM_ID
            '                                     WHERE    (GuestInformation.GUEST_ID = " & ConvertingNumbers(Request.QueryString("id")) & ") ")
            '            For Each chk_row As DataRow In dt_cust.Rows
            '                txtGuestName.Text = chk_row("GuestName").ToString()
            '                txtAddress.Text = chk_row("Address").ToString()
            '                txtPhoneNo.Text = chk_row("PhoneNo").ToString()
            '                ddlIDType.SelectedValue = chk_row("ID_Type").ToString()
            '                'txtIDNo.Text = chk_row("ID_No").ToString()
            '                ddlGender.SelectedValue = chk_row("Gender").ToString()
            '                txtPurpose.Text = chk_row("Purpose").ToString()
            '                txtNote.Value = chk_row("Note").ToString()
            '                txtArrivalDeparture.Text = chk_row("Arrival_n_Departure_Date_Time").ToString()
            '                txtCheckInDate.Text = Format(chk_row("Check_In_Date"), "dd/MM/yyyy") + " " + chk_row("Check_In_Time").ToString()
            '                txtCheckInTime.Text = chk_row("Check_In_Time").ToString()
            '                txtNoOfDay.Text = chk_row("No_Of_Day").ToString()
            '                txtCheckOutDate.Text = Format(chk_row("Check_Out_Date"), "dd/MM/yyyy") + " " + chk_row("Check_Out_Time").ToString()
            '                txtCheckOutTime.Text = chk_row("Check_Out_Time").ToString()
            '                ddlRoomNo.SelectedValue = chk_row("ROOM_ID").ToString()
            '                txtTotalCharges.Text = chk_row("Total_Charges").ToString()
            '                txtNoOfAdult.Text = chk_row("No_Of_Adult").ToString()
            '                txtRentnDay.Text = chk_row("Rent_Day").ToString()
            '                txtNoOfChildren.Text = chk_row("No_Of_Children").ToString()
            '                txtExtraBeds.Text = chk_row("Extra_Beds").ToString()
            '                txtPerBedCost.Text = chk_row("Per_Bed_Cost").ToString()
            '                txtRoomType.Text = chk_row("ROOM_TYPE").ToString()
            '                txtTotalExtraBedCost.Text = chk_row("Total_Extra_Bed_Cost").ToString()
            '            Next
            '        Else
            '            Response.Redirect("CheckIn.aspx")
            '        End If
            '        '---------------------------------------
            '    Else
            '        Response.Redirect("CheckIn.aspx")
            '    End If
            'End If


            If (Not (Session("FlushMessage")) Is Nothing) Then
                lblMessege.Text = Session("FlushMessage").ToString()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "showModal();", True)
                Session("FlushMessage") = Nothing
            End If
        End If
    End Sub
    Private Sub SetInitialRow()
        Dim dt As DataTable = New DataTable()
        Dim dr As DataRow = Nothing
        dt.Columns.Add(New DataColumn("Date/Hour", GetType(String)))
        dt.Columns.Add(New DataColumn("Room", GetType(String)))
        dt.Columns.Add(New DataColumn("Rate", GetType(String)))
        dt.Columns.Add(New DataColumn("Type", GetType(String)))
        dt.Columns.Add(New DataColumn("Tax", GetType(String)))
        dt.Columns.Add(New DataColumn("Discount", GetType(String)))
        dt.Columns.Add(New DataColumn("Promo", GetType(String)))
        dt.Columns.Add(New DataColumn("Payment", GetType(String)))
        dr = dt.NewRow()
        dt.Rows.Add(dr)
        ViewState("CurrentTable") = dt
    End Sub
    Private Sub btnCheckInSubmit_Click(sender As Object, e As EventArgs) Handles btnCheckInSubmit.Click

        guestIdChangeEvent = True

        If ConvertingNumbers(hdnGuestId.Value) Then
            'strRoomId = Request.QueryString("room_id")
            ''---------------------------------------
            'Dim dt_checkRoom As DataTable = QueryDataTable(" SELECT   *   FROM  RoomDetails  WHERE    (Occupied = 'Y') AND (ROOM_ID = " & ddlRoomNo.SelectedValue & ") ")
            'If dt_checkRoom.Rows.Count > 0 Then
            '    Dim current_room As DataTable = QueryDataTable("SELECT   * FROM            RoomDetails    WHERE        (GUEST_ID = " & ConvertingNumbers(hdnGuestId.Value) & ") AND (ROOM_ID = " & ConvertingNumbers(strRoomId) & ") AND (Occupied = 'Y')")
            '    If current_room.Rows.Count > 0 And gvBill.Rows.Count > 0 And hdnNewPayment.Value = "1" Then
            '        Update_Guest_AddBill()
            '    ElseIf current_room.Rows.Count > 0 And hdnNewPayment.Value = "0" Then
            '        Update_Guest_Information()
            '    Else
            '        ClientScript.RegisterStartupScript(Me.[GetType](), "myalert", "alert('Room has been already booked by another guest.');", True)
            '    End If
            'Else
            Update_Guest_Information()
            Session("FlushMessage") = "Data has been updated successfully."
            Response.Redirect("RoomView.aspx")
        End If
        '---------------------------------------
        'Else
        '    Dim dt_checkRoom As DataTable = QueryDataTable(" SELECT   *   FROM    RoomDetails  WHERE    (Occupied = 'Y') AND (ROOM_ID = " & ConvertingNumbers(ddlRoomNo.SelectedValue) & ") ")
        '    If dt_checkRoom.Rows.Count > 0 Then
        '        ClientScript.RegisterStartupScript(Me.[GetType](), "myalert", "alert('Room has been already booked.');", True)
        '    Else
        '        '-----------------------------------
        '        strRoomId = Request.QueryString("room_id")
        '        Dim datTimeseparate As String() = txtCheckInDate.Text.ToString.Split(" ")
        '        Dim datTimeseparate2 As String() = txtCheckOutDate.Text.ToString.Split(" ")

        '        Dim CheckIn_Date As Date = DateTime.ParseExact(datTimeseparate(0), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
        '        Dim CheckOut_Date As Date = DateTime.ParseExact(datTimeseparate2(0), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)

        '        Dim CheckIn_Time As String = datTimeseparate(1) '+ " " + datTimeseparate(2)
        '        Dim CheckOut_Time As String = datTimeseparate2(1) '+ " " + datTimeseparate2(2)
        '        Using con As New SqlConnection(CnString)
        '            If IsNumeric(strRoomId) Then
        '                If IsNumeric(hdnGuestId.Value) Then
        '                    Using cmd2 As New SqlCommand("UPDATE  GuestInformation SET  GuestName=@GuestName, Address=@Address, PhoneNo=@PhoneNo, ID_Type=@ID_Type, Gender=@Gender, Purpose=@Purpose, Arrival_n_Departure_Date_Time=@Arrival_n_Departure_Date_Time, Note=@Note, Check_In_Date=@Check_In_Date, Check_In_Time=@Check_In_Time, Check_Out_Date=@Check_Out_Date, Check_Out_Time=@Check_Out_Time, No_Of_Day=@No_Of_Day, ROOM_ID=@ROOM_ID, Rent_Day=@Rent_Day,Total_Charges=@Total_Charges, No_Of_Adult=@No_Of_Adult, No_Of_Children=@No_Of_Children, Extra_Beds=@Extra_Beds, Per_Bed_Cost=@Per_Bed_Cost, Total_Extra_Bed_Cost=@Total_Extra_Bed_Cost,  Social_Type=@Social_Type,USER_ID=@USER_ID where GUEST_ID=@GUEST_ID")

        '                        cmd2.Parameters.AddWithValue("@GUEST_ID", hdnGuestId.Value)
        '                        cmd2.Parameters.AddWithValue("@GuestName", txtGuestName.Text)
        '                        cmd2.Parameters.AddWithValue("@Address", txtAddress.Text)
        '                        cmd2.Parameters.AddWithValue("@PhoneNo", txtPhoneNo.Text)
        '                        cmd2.Parameters.AddWithValue("@ID_Type", ddlIDType.SelectedValue)
        '                        'cmd2.Parameters.AddWithValue("@ID_No", txtIDNo.Text)   ID_No=@ID_No,
        '                        cmd2.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue)
        '                        cmd2.Parameters.AddWithValue("@Purpose", txtPurpose.Text)
        '                        cmd2.Parameters.AddWithValue("@Arrival_n_Departure_Date_Time", txtArrivalDeparture.Text)
        '                        cmd2.Parameters.AddWithValue("@Note", txtNote.Value)
        '                        cmd2.Parameters.AddWithValue("@Check_In_Date", CheckIn_Date)
        '                        cmd2.Parameters.AddWithValue("@Check_In_Time", CheckIn_Time)
        '                        cmd2.Parameters.AddWithValue("@Check_Out_Date", CheckOut_Date)
        '                        cmd2.Parameters.AddWithValue("@Check_Out_Time", CheckOut_Time)
        '                        cmd2.Parameters.AddWithValue("@No_Of_Day", txtNoOfDay.Text)
        '                        cmd2.Parameters.AddWithValue("@ROOM_ID", ConvertingNumbers(strRoomId))
        '                        cmd2.Parameters.AddWithValue("@Rent_Day", txtRentnDay.Text)
        '                        cmd2.Parameters.AddWithValue("@Total_Charges", Val(txtRentnDay.Text) * Val(txtNoOfDay.Text))
        '                        cmd2.Parameters.AddWithValue("@No_Of_Adult", txtNoOfAdult.Text)
        '                        cmd2.Parameters.AddWithValue("@No_Of_Children", txtNoOfChildren.Text)
        '                        cmd2.Parameters.AddWithValue("@Extra_Beds", txtExtraBeds.Text)
        '                        cmd2.Parameters.AddWithValue("@Per_Bed_Cost", txtPerBedCost.Text)
        '                        cmd2.Parameters.AddWithValue("@Total_Extra_Bed_Cost", Val(txtPerBedCost.Text) * Val(txtExtraBeds.Text))
        '                        'cmd2.Parameters.AddWithValue("@Guest_Type", ddlNormalSpecial.SelectedValue) Guest_Type=@Guest_Type,
        '                        If IsNumeric(CheckBoxList1.SelectedValue) Then

        '                            cmd2.Parameters.AddWithValue("@Social_Type", CheckBoxList1.Items(CheckBoxList1.SelectedIndex).Text)
        '                        Else

        '                            cmd2.Parameters.AddWithValue("@Social_Type", "")
        '                        End If

        '                        cmd2.Parameters.AddWithValue("@USER_ID", ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)))


        '                        cmd2.Connection = con
        '                        con.Open()
        '                        cmd2.ExecuteNonQuery()
        '                    End Using

        '                    'Else
        '                    '    Using cmd As New SqlCommand("INSERT INTO  GuestInformation (GEST_ID,GuestName, Address, PhoneNo, ID_Type,  Gender, Purpose, Arrival_n_Departure_Date_Time, Note, Check_In_Date, Check_In_Time, Check_Out_Date, Check_Out_Time, No_Of_Day, ROOM_ID, Rent_Day,Total_Charges, No_Of_Adult, No_Of_Children, Extra_Beds, Per_Bed_Cost, Total_Extra_Bed_Cost, Status,Social_Type,USER_ID,Register_Date) VALUES (@GEST_ID,@GuestName, @Address, @PhoneNo, @ID_Type,  @Gender, @Purpose, @Arrival_n_Departure_Date_Time, @Note, @Check_In_Date, @Check_In_Time, @Check_Out_Date, @Check_Out_Time, @No_Of_Day, @ROOM_ID, @Rent_Day, @Total_Charges, @No_Of_Adult, @No_Of_Children, @Extra_Beds, @Per_Bed_Cost, @Total_Extra_Bed_Cost, 'CheckIn',@Social_Type," & ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)) & ",'" & Now & "') SELECT SCOPE_IDENTITY()")
        '                    '        cmd.Parameters.AddWithValue("@GEST_ID", txtGuestID.Text)
        '                    '        cmd.Parameters.AddWithValue("@GuestName", txtGuestName.Text)
        '                    '        cmd.Parameters.AddWithValue("@Address", txtAddress.Text)
        '                    '        cmd.Parameters.AddWithValue("@PhoneNo", txtPhoneNo.Text)
        '                    '        cmd.Parameters.AddWithValue("@ID_Type", ddlIDType.SelectedValue)
        '                    '        'cmd.Parameters.AddWithValue("@ID_No", txtIDNo.Text) , 
        '                    '        cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue)
        '                    '        cmd.Parameters.AddWithValue("@Purpose", txtPurpose.Text)
        '                    '        cmd.Parameters.AddWithValue("@Arrival_n_Departure_Date_Time", txtArrivalDeparture.Text)
        '                    '        cmd.Parameters.AddWithValue("@Note", txtNote.Value)
        '                    '        cmd.Parameters.AddWithValue("@Check_In_Date", CheckIn_Date)
        '                    '        cmd.Parameters.AddWithValue("@Check_In_Time", CheckIn_Time)
        '                    '        cmd.Parameters.AddWithValue("@Check_Out_Date", CheckOut_Date)
        '                    '        cmd.Parameters.AddWithValue("@Check_Out_Time", CheckOut_Time)
        '                    '        cmd.Parameters.AddWithValue("@No_Of_Day", txtNoOfDay.Text)
        '                    '        cmd.Parameters.AddWithValue("@ROOM_ID", ConvertingNumbers(strRoomId))
        '                    '        cmd.Parameters.AddWithValue("@Rent_Day", txtRentnDay.Text)
        '                    '        cmd.Parameters.AddWithValue("@Total_Charges", Val(txtRentnDay.Text) * Val(txtNoOfDay.Text))
        '                    '        cmd.Parameters.AddWithValue("@No_Of_Adult", txtNoOfAdult.Text)
        '                    '        cmd.Parameters.AddWithValue("@No_Of_Children", txtNoOfChildren.Text)
        '                    '        cmd.Parameters.AddWithValue("@Extra_Beds", txtExtraBeds.Text)
        '                    '        cmd.Parameters.AddWithValue("@Per_Bed_Cost", txtPerBedCost.Text)
        '                    '        cmd.Parameters.AddWithValue("@Total_Extra_Bed_Cost", Val(txtPerBedCost.Text) * Val(txtExtraBeds.Text))
        '                    '        'cmd.Parameters.AddWithValue("@Guest_Type", ddlNormalSpecial.SelectedValue) 
        '                    '        If IsNumeric(CheckBoxList1.SelectedValue) Then

        '                    '            cmd.Parameters.AddWithValue("@Social_Type", CheckBoxList1.Items(CheckBoxList1.SelectedIndex).Text)
        '                    '        Else

        '                    '            cmd.Parameters.AddWithValue("@Social_Type", "")
        '                    '        End If
        '                    '        cmd.Connection = con
        '                    '        con.Open()
        '                    '        Dim GUEST_ID As Integer = Convert.ToInt32(cmd.ExecuteScalar())

        '                    '        Dim CheckIn_Date2 As Date


        '                    '        Dim cmd2 As New SqlCommand("INSERT   INTO         BillDetails(Room_ID, Rent_ID, PayType, Tax, Discount, Promotion, Payment, Guest_ID, Check_In_Date, Check_In_Time, UserID,ID_No,Guest_Type) VALUES        (@Room_ID, @Rent_ID, @PayType, @Tax, @Discount, @Promotion, @Payment, @Guest_ID, @Check_In_Date, @Check_In_Time, @UserID, @ID_No,@Guest_Type)")
        '                    '        cmd2.CommandType = CommandType.Text
        '                    '        ' Dim adapFam As New SqlDataAdapter 
        '                    '        If gvBill.Rows.Count > 0 Then
        '                    '            For i As Integer = 0 To gvBill.Rows.Count - 1
        '                    '                Dim checkin_date_time As String()
        '                    '                checkin_date_time = gvBill.Rows(i).Cells(2).Text.Split(" ")
        '                    '                CheckIn_Date2 = DateTime.ParseExact(checkin_date_time(0), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
        '                    '                Dim rent_val As String()
        '                    '                rent_val = gvBill.Rows(i).Cells(4).Text.Split(" ")
        '                    '                cmd2.Parameters.Clear()
        '                    '                cmd2.Parameters.AddWithValue("@Room_ID", ConvertingNumbers(strRoomId))
        '                    '                cmd2.Parameters.AddWithValue("@Rent_ID", ddlHours.Items.FindByText(rent_val(0).Trim).Value)
        '                    '                cmd2.Parameters.AddWithValue("@PayType", gvBill.Rows(i).Cells(5).Text)
        '                    '                cmd2.Parameters.AddWithValue("@Tax", gvBill.Rows(i).Cells(6).Text)

        '                    '                Dim discont As String
        '                    '                If gvBill.Rows(i).Cells(7).Text Is "" Then
        '                    '                    discont = "0.00"
        '                    '                Else
        '                    '                    discont = gvBill.Rows(i).Cells(7).Text
        '                    '                End If
        '                    '                cmd2.Parameters.AddWithValue("@Discount", discont)

        '                    '                Dim promo As String
        '                    '                If IsNumeric(ddlPromotion.SelectedValue) Then
        '                    '                    promo = gvBill.Rows(i).Cells(8).Text
        '                    '                Else
        '                    '                    promo = ""
        '                    '                End If

        '                    '                cmd2.Parameters.AddWithValue("@Promotion", promo)
        '                    '                cmd2.Parameters.AddWithValue("@Payment", Convert.ToDecimal(gvBill.Rows(i).Cells(9).Text))
        '                    '                cmd2.Parameters.AddWithValue("@Guest_ID", GUEST_ID)
        '                    '                cmd2.Parameters.AddWithValue("@Check_In_Date", CheckIn_Date2)
        '                    '                cmd2.Parameters.AddWithValue("@Check_In_Time", checkin_date_time(1))
        '                    '                cmd2.Parameters.AddWithValue("@UserID", ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)))
        '                    '                cmd2.Parameters.AddWithValue("@ID_No", txtIDNo.Text)
        '                    '                cmd2.Parameters.AddWithValue("@Guest_Type", ddlNormalSpecial.SelectedValue)
        '                    '                cmd2.Connection = con
        '                    '                cmd2.ExecuteNonQuery()
        '                    '                'adapFam.InsertCommand.ExecuteNonQuery()
        '                    '            Next
        '                    '        Else
        '                    '            cmd2.Parameters.Clear()
        '                    '            cmd2.Parameters.AddWithValue("@Room_ID", ConvertingNumbers(strRoomId))
        '                    '            cmd2.Parameters.AddWithValue("@Rent_ID", ddlHours.SelectedValue)
        '                    '            cmd2.Parameters.AddWithValue("@PayType", ddlPayType.SelectedItem.Text)
        '                    '            cmd2.Parameters.AddWithValue("@Tax", txtRoomTax.Text)

        '                    '            If txtRoomDiscount.Text Is "" Then
        '                    '                txtRoomDiscount.Text = "0.00"
        '                    '            End If

        '                    '            cmd2.Parameters.AddWithValue("@Discount", txtRoomDiscount.Text)

        '                    '            Dim promo As String
        '                    '            If IsNumeric(ddlPromotion.SelectedValue) Then
        '                    '                promo = ddlPromotion.SelectedItem.Text
        '                    '            Else
        '                    '                promo = ""
        '                    '            End If

        '                    '            cmd2.Parameters.AddWithValue("@Promotion", promo)
        '                    '            cmd2.Parameters.AddWithValue("@Payment", Convert.ToDecimal(txtRentTotal.Text))
        '                    '            cmd2.Parameters.AddWithValue("@Guest_ID", GUEST_ID)
        '                    '            cmd2.Parameters.AddWithValue("@Check_In_Date", CheckIn_Date)
        '                    '            cmd2.Parameters.AddWithValue("@Check_In_Time", CheckIn_Time)
        '                    '            cmd2.Parameters.AddWithValue("@UserID", ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)))
        '                    '            cmd2.Parameters.AddWithValue("@ID_No", txtIDNo.Text)
        '                    '            cmd2.Parameters.AddWithValue("@Guest_Type", ddlNormalSpecial.SelectedValue)
        '                    '            cmd2.Connection = con
        '                    '            cmd2.ExecuteNonQuery()
        '                    '        End If

        '                    '        con.Close()
        '                    '        If IsNumeric(strRoomId) Then
        '                    '            RoomBooking(GUEST_ID, ConvertingNumbers(strRoomId))
        '                    '        ElseIf IsNumeric(ddlRoomNo.SelectedValue) Then
        '                    '            If ddlRoomNo.SelectedValue > 0 Then
        '                    '                RoomBooking(GUEST_ID, ddlRoomNo.SelectedValue)
        '                    '            End If

        '                    '        End If
        '                    '        HOTEL_SERVICE_CHARGE_CALCULATION(GUEST_ID)

        '                    '    End Using

        '                End If
        '            End If

        '        End Using
        '        Session("FlushMessage") = "Data has been saved successfully."
        '        Response.Redirect("RoomView.aspx")
        '        '-----------------------------------
        '    End If
        'End If
    End Sub

    Public Sub Update_Guest_Information()

        Dim datTimeseparate As String() = txtCheckInDate.Text.ToString.Split(" ")
        Dim datTimeseparate2 As String() = txtCheckOutDate.Text.ToString.Split(" ")

        Dim CheckIn_Date As Date = DateTime.ParseExact(datTimeseparate(0), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
        Dim CheckOut_Date As Date = DateTime.ParseExact(datTimeseparate2(0), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)


        Dim CheckIn_Time As String = datTimeseparate(1)
        Dim CheckOut_Time As String = datTimeseparate2(1)
        'Dim CheckIn_Time As String = datTimeseparate(1) + " " + datTimeseparate(2)
        'Dim CheckOut_Time As String = datTimeseparate2(1) + " " + datTimeseparate2(2)
        'Dim CheckIn_Date As Date = DateTime.ParseExact(txtCheckInDate.Text.ToString, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
        'Dim CheckOut_Date As Date = DateTime.ParseExact(txtCheckOutDate.Text.ToString, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
        'Dim CheckIn_Time As String = DateTime.ParseExact(txtCheckInDate.Text.ToString, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None)
        'Dim CheckOut_Time As String = DateTime.ParseExact(txtCheckOutDate.Text.ToString, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None)

        Using con As New SqlConnection(CnString)



            Using cmd As New SqlCommand("UPDATE  GuestInformation SET  GuestName=@GuestName, Address=@Address, PhoneNo=@PhoneNo, ID_Type=@ID_Type,  Gender=@Gender, Purpose=@Purpose, Arrival_n_Departure_Date_Time=@Arrival_n_Departure_Date_Time, Note=@Note, Check_In_Date=@Check_In_Date, Check_In_Time=@Check_In_Time, Check_Out_Date=@Check_Out_Date, Check_Out_Time=@Check_Out_Time, No_Of_Day=@No_Of_Day, ROOM_ID=@ROOM_ID, Rent_Day=@Rent_Day,Total_Charges=@Total_Charges, No_Of_Adult=@No_Of_Adult, No_Of_Children=@No_Of_Children, Extra_Beds=@Extra_Beds, Per_Bed_Cost=@Per_Bed_Cost, Total_Extra_Bed_Cost=@Total_Extra_Bed_Cost,Social_Type=@Social_Type,USER_ID=@USER_ID where GUEST_ID=@GUEST_ID")

                cmd.Parameters.AddWithValue("@GUEST_ID", hdnGuestId.Value)
                cmd.Parameters.AddWithValue("@GuestName", txtGuestName.Text)
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text)
                cmd.Parameters.AddWithValue("@PhoneNo", txtPhoneNo.Text)
                cmd.Parameters.AddWithValue("@ID_Type", ddlIDType.SelectedValue)
                'cmd.Parameters.AddWithValue("@ID_No", txtIDNo.Text)
                cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue)
                cmd.Parameters.AddWithValue("@Purpose", txtPurpose.Text)
                cmd.Parameters.AddWithValue("@Arrival_n_Departure_Date_Time", txtArrivalDeparture.Text)
                cmd.Parameters.AddWithValue("@Note", txtNote.Value)
                cmd.Parameters.AddWithValue("@Check_In_Date", CheckIn_Date)
                cmd.Parameters.AddWithValue("@Check_In_Time", CheckIn_Time)
                cmd.Parameters.AddWithValue("@Check_Out_Date", CheckOut_Date)
                cmd.Parameters.AddWithValue("@Check_Out_Time", CheckOut_Time)
                cmd.Parameters.AddWithValue("@No_Of_Day", txtNoOfDay.Text)
                cmd.Parameters.AddWithValue("@ROOM_ID", ddlRoomNo.SelectedValue)
                cmd.Parameters.AddWithValue("@Rent_Day", txtRentnDay.Text)
                cmd.Parameters.AddWithValue("@Total_Charges", Val(txtRentnDay.Text) * Val(txtNoOfDay.Text))
                cmd.Parameters.AddWithValue("@No_Of_Adult", txtNoOfAdult.Text)
                cmd.Parameters.AddWithValue("@No_Of_Children", txtNoOfChildren.Text)
                cmd.Parameters.AddWithValue("@Extra_Beds", txtExtraBeds.Text)
                cmd.Parameters.AddWithValue("@Per_Bed_Cost", txtPerBedCost.Text)
                cmd.Parameters.AddWithValue("@Total_Extra_Bed_Cost", Val(txtPerBedCost.Text) * Val(txtExtraBeds.Text))
                If IsNumeric(CheckBoxList1.SelectedValue) Then
                    cmd.Parameters.AddWithValue("@Social_Type", CheckBoxList1.Items(CheckBoxList1.SelectedIndex).Text)
                Else
                    cmd.Parameters.AddWithValue("@Social_Type", "")
                End If

                cmd.Parameters.AddWithValue("@USER_ID", ddlUsers.SelectedValue)
                cmd.Connection = con
                If con.State = ConnectionState.Closed Then
                    con.Open()
                End If

                cmd.ExecuteNonQuery()


                'Dim dt_rentinfo As DataTable = QueryDataTable(" SELECT   *   FROM    BillDetails  where  (ROOM_ID = " & ConvertingNumbers(strRoomId) & ") And  (Guest_ID= " & ConvertingNumbers(hdnGuestId.Value) & ") and (Rent_ID=" & ddlHours.SelectedValue & ")")
                'If dt_rentinfo.Rows.Count = 0 Then
                '    Dim cmd2 As New SqlCommand("INSERT   INTO         BillDetails(Room_ID, Rent_ID, PayType, Tax, Discount, Promotion, Payment, Guest_ID, Check_In_Date, Check_In_Time, UserID,ID_No, Guest_Type) VALUES        (@Room_ID, @Rent_ID, @PayType, @Tax, @Discount, @Promotion, @Payment, @Guest_ID, @Check_In_Date, @Check_In_Time, @UserID,@ID_No,@Guest_Type)")
                '    cmd2.CommandType = CommandType.Text
                '    ' Dim adapFam As New SqlDataAdapter 

                '    cmd2.Parameters.Clear()
                '    cmd2.Parameters.AddWithValue("@Room_ID", ConvertingNumbers(hdnRoomId.Value))
                '    cmd2.Parameters.AddWithValue("@Rent_ID", ddlHours.SelectedValue)
                '    cmd2.Parameters.AddWithValue("@PayType", ddlPayType.SelectedItem.Text)
                '    cmd2.Parameters.AddWithValue("@Tax", txtRoomTax.Text)

                '    If txtRoomDiscount.Text Is "" Then
                '        txtRoomDiscount.Text = "0.00"
                '    End If

                '    cmd2.Parameters.AddWithValue("@Discount", txtRoomDiscount.Text)

                '    Dim promo As String
                '    If IsNumeric(ddlPromotion.SelectedValue) Then
                '        promo = ddlPromotion.SelectedItem.Text
                '    Else
                '        promo = ""
                '    End If

                '    cmd2.Parameters.AddWithValue("@Promotion", promo)
                '    cmd2.Parameters.AddWithValue("@Payment", Convert.ToDecimal(txtRentTotal.Text))
                '    cmd2.Parameters.AddWithValue("@Guest_ID", hdnGuestId.Value)
                '    cmd2.Parameters.AddWithValue("@Check_In_Date", CheckIn_Date)
                '    cmd2.Parameters.AddWithValue("@Check_In_Time", CheckIn_Time)
                '    cmd2.Parameters.AddWithValue("@UserID", ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)))
                '    cmd2.Parameters.AddWithValue("@ID_No", txtIDNo.Text)
                '    cmd2.Parameters.AddWithValue("@Guest_Type", ddlNormalSpecial.SelectedValue)
                '    cmd2.Connection = con
                '    If con.State = ConnectionState.Closed Then

                '        con.Open()
                '    End If
                '    cmd2.ExecuteNonQuery()
                'Else
                'Dim GUEST_ID As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                Dim cmd2 As New SqlCommand("update BillDetails Set UserID=@UserID,Guest_Type=@Guest_Type, Room_ID=@Room_ID where Guest_ID=@Guest_ID and BillDetails.ID_No=@ID_No")
                cmd2.CommandType = CommandType.Text
                ' Dim adapFam As New SqlDataAdapter 

                cmd2.Parameters.Clear()
                cmd2.Parameters.AddWithValue("@GUEST_ID", hdnGuestId.Value)
                cmd2.Parameters.AddWithValue("@Room_ID", ddlRoomNo.SelectedValue)
                'cmd2.Parameters.AddWithValue("@Rent_ID", ddlHours.SelectedValue)
                'cmd2.Parameters.AddWithValue("@PayType", ddlPayType.SelectedItem.Text)
                'cmd2.Parameters.AddWithValue("@Tax", txtRoomTax.Text)

                'If txtRoomDiscount.Text Is "" Then
                '    txtRoomDiscount.Text = "0.00"
                'End If

                'cmd2.Parameters.AddWithValue("@Discount", txtRoomDiscount.Text)

                'Dim promo As String
                'If IsNumeric(ddlPromotion.SelectedValue) Then
                '    promo = ddlPromotion.SelectedItem.Text
                'Else
                '    promo = ""
                'End If

                'cmd2.Parameters.AddWithValue("@Promotion", promo)
                'cmd2.Parameters.AddWithValue("@Payment", Convert.ToDecimal(txtRentTotal.Text))
                'cmd2.Parameters.AddWithValue("@Check_In_Date", CheckIn_Date)
                'cmd2.Parameters.AddWithValue("@Check_In_Time", CheckIn_Time)

                Dim userID = ddlUsers.SelectedValue
                If userID = 0 Then
                    cmd2.Parameters.AddWithValue("@UserID", ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)))
                Else
                    cmd2.Parameters.AddWithValue("@UserID", userID)
                End If


                cmd2.Parameters.AddWithValue("@ID_No", txtIDNo.Text)
                cmd2.Parameters.AddWithValue("@Guest_Type", ddlNormalSpecial.SelectedValue)
                cmd2.Connection = con
                If con.State = ConnectionState.Closed Then
                    con.Open()
                End If
                cmd2.ExecuteNonQuery()
                'End If


                Dim cmd3 As New SqlCommand("UPDATE RoomDetails SET Occupied='N',Status_ID='1',GUEST_ID='0' WHERE GUEST_ID=@GUEST_ID AND Occupied='Y' AND Status_ID='2'")
                cmd3.CommandType = CommandType.Text
                ' Dim adapFam As New SqlDataAdapter 

                cmd3.Parameters.Clear()
                cmd3.Parameters.AddWithValue("@GUEST_ID", hdnGuestId.Value)
                cmd3.Connection = con
                If con.State = ConnectionState.Closed Then
                    con.Open()
                End If
                cmd3.ExecuteNonQuery()

                Dim cmd4 As New SqlCommand("UPDATE RoomDetails SET Occupied='Y',Status_ID='2',GUEST_ID=@GUEST_ID WHERE Room_ID=@Room_ID")
                cmd4.CommandType = CommandType.Text
                ' Dim adapFam As New SqlDataAdapter 

                cmd4.Parameters.Clear()
                cmd4.Parameters.AddWithValue("@GUEST_ID", hdnGuestId.Value)
                cmd4.Parameters.AddWithValue("@Room_ID", ddlRoomNo.SelectedValue)
                cmd4.Connection = con
                If con.State = ConnectionState.Closed Then
                    con.Open()
                End If
                cmd4.ExecuteNonQuery()

                If con.State = ConnectionState.Open Then
                    con.Close()
                End If


                HOTEL_SERVICE_CHARGE_CALCULATION(ConvertingNumbers(hdnGuestId.Value))
                'RoomBooking(ConvertingNumbers(hdnGuestId.Value), ddlRoomNo.SelectedValue)

            End Using
        End Using

    End Sub

    Public Sub Update_Guest_AddBill()

        Dim datTimeseparate As String() = txtCheckInDate.Text.ToString.Split(" ")
        Dim datTimeseparate2 As String() = txtCheckOutDate.Text.ToString.Split(" ")

        Dim CheckIn_Date As Date = DateTime.ParseExact(datTimeseparate(0), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
        Dim CheckOut_Date As Date = DateTime.ParseExact(datTimeseparate2(0), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)


        Dim CheckIn_Time As String = datTimeseparate(1)
        Dim CheckOut_Time As String = datTimeseparate2(1)

        'Dim CheckIn_Time As String = datTimeseparate(1) + " " + datTimeseparate(2)
        'Dim CheckOut_Time As String = datTimeseparate2(1) + " " + datTimeseparate2(2)
        'Dim CheckIn_Date As Date = DateTime.ParseExact(txtCheckInDate.Text.ToString, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
        'Dim CheckOut_Date As Date = DateTime.ParseExact(txtCheckOutDate.Text.ToString, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)

        'Dim CheckIn_Time As String = DateTime.ParseExact(txtCheckInDate.Text.ToString, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None)
        'Dim CheckOut_Time As String = DateTime.ParseExact(txtCheckOutDate.Text.ToString, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None)

        Using con As New SqlConnection(CnString)

            Using cmd2 As New SqlCommand("UPDATE  GuestInformation SET  GuestName=@GuestName, Address=@Address, PhoneNo=@PhoneNo, ID_Type=@ID_Type, Gender=@Gender, Purpose=@Purpose, Arrival_n_Departure_Date_Time=@Arrival_n_Departure_Date_Time, Note=@Note, Check_In_Date=@Check_In_Date, Check_In_Time=@Check_In_Time, Check_Out_Date=@Check_Out_Date, Check_Out_Time=@Check_Out_Time, No_Of_Day=@No_Of_Day, ROOM_ID=@ROOM_ID, Rent_Day=@Rent_Day,Total_Charges=@Total_Charges, No_Of_Adult=@No_Of_Adult, No_Of_Children=@No_Of_Children, Extra_Beds=@Extra_Beds, Per_Bed_Cost=@Per_Bed_Cost, Total_Extra_Bed_Cost=@Total_Extra_Bed_Cost,  Social_Type=@Social_Type,USER_ID=@USER_ID where GUEST_ID=@GUEST_ID")

                'Dim GUEST_ID As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                Dim CheckIn_Date2 As Date

                Dim current_room As DataTable = QueryDataTable("SELECT   * FROM  RoomDetails    WHERE        (GUEST_ID = " & ConvertingNumbers(hdnGuestId.Value) & ") AND (ROOM_ID = " & ddlRoomNo.SelectedValue & ") AND (Occupied = 'Y')")
                If current_room.Rows.Count > 0 And gvBill.Rows.Count > 0 And hdnNewPayment.Value = "1" Then
                    Update_Guest_AddBill()
                ElseIf current_room.Rows.Count > 0 And hdnNewPayment.Value = "0" Then
                    Update_Guest_Information()
                Else
                    ClientScript.RegisterStartupScript(Me.[GetType](), "myalert", "alert('Room has been already booked by another guest.');", True)
                End If

                '"INSERT   INTO BillDetails(Room_ID, Rent_ID, PayType, Tax, Discount, Promotion, Payment, Guest_ID, Check_In_Date, Check_In_Time, UserID,ID_No, Guest_Type) VALUES        (@Room_ID, @Rent_ID, @PayType, @Tax, @Discount, @Promotion, @Payment, @Guest_ID, @Check_In_Date, @Check_In_Time, @UserID,@ID_No,@Guest_Type)"
                cmd2.CommandType = CommandType.Text

                If gvBill.Rows.Count > 0 And hdnNewPayment.Value = "1" Then
                    For i As Integer = 0 To gvBill.Rows.Count - 1
                        Dim checkin_date_time As String()
                        checkin_date_time = gvBill.Rows(i).Cells(2).Text.Split(" ")
                        CheckIn_Date2 = DateTime.ParseExact(checkin_date_time(0), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
                        Dim rent_val As String()
                        rent_val = gvBill.Rows(i).Cells(4).Text.Split(" ")
                        cmd2.Parameters.Clear()
                        cmd2.Parameters.AddWithValue("@Room_ID", ConvertingNumbers(hdnRoomId.Value))
                        cmd2.Parameters.AddWithValue("@Rent_ID", ddlHours.Items.FindByText(rent_val(0).Trim).Value)
                        cmd2.Parameters.AddWithValue("@PayType", gvBill.Rows(i).Cells(5).Text)
                        cmd2.Parameters.AddWithValue("@Tax", gvBill.Rows(i).Cells(6).Text)

                        Dim discont As String
                        If gvBill.Rows(i).Cells(7).Text Is "" Then
                            discont = "0.00"
                        Else
                            discont = gvBill.Rows(i).Cells(7).Text
                        End If
                        cmd2.Parameters.AddWithValue("@Discount", discont)

                        Dim promo As String
                        If IsNumeric(ddlPromotion.SelectedValue) Then
                            promo = gvBill.Rows(i).Cells(8).Text
                        Else
                            promo = ""
                        End If

                        cmd2.Parameters.AddWithValue("@Promotion", promo)
                        cmd2.Parameters.AddWithValue("@Payment", Convert.ToDecimal(gvBill.Rows(i).Cells(9).Text))
                        cmd2.Parameters.AddWithValue("@Guest_ID", hdnGuestId.Value)
                        cmd2.Parameters.AddWithValue("@Check_In_Date", CheckIn_Date2)
                        cmd2.Parameters.AddWithValue("@Check_In_Time", checkin_date_time(1))
                        cmd2.Parameters.AddWithValue("@UserID", ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)))
                        cmd2.Parameters.AddWithValue("@ID_No", txtIDNo.Text)
                        cmd2.Parameters.AddWithValue("@Guest_Type", ddlNormalSpecial.SelectedValue)
                        cmd2.Connection = con
                        cmd2.ExecuteNonQuery()
                        'adapFam.InsertCommand.ExecuteNonQuery()
                    Next
                    'Else
                    '    cmd2.Parameters.Clear()
                    '    cmd2.Parameters.AddWithValue("@Room_ID", ConvertingNumbers(hdnRoomId.Value))
                    '    cmd2.Parameters.AddWithValue("@Rent_ID", ddlHours.SelectedValue)
                    '    cmd2.Parameters.AddWithValue("@PayType", ddlPayType.SelectedItem.Text)
                    '    cmd2.Parameters.AddWithValue("@Tax", txtRoomTax.Text)

                    '    If txtRoomDiscount.Text Is "" Then
                    '        txtRoomDiscount.Text = "0.00"
                    '    End If

                    '    cmd2.Parameters.AddWithValue("@Discount", txtRoomDiscount.Text)

                    '    Dim promo As String
                    '    If IsNumeric(ddlPromotion.SelectedValue) Then
                    '        promo = ddlPromotion.SelectedItem.Text
                    '    Else
                    '        promo = ""
                    '    End If

                    '    cmd2.Parameters.AddWithValue("@Promotion", promo)
                    '    cmd2.Parameters.AddWithValue("@Payment", Convert.ToDecimal(txtRentTotal.Text))
                    '    cmd2.Parameters.AddWithValue("@Guest_ID", hdnGuestId.Value)
                    '    cmd2.Parameters.AddWithValue("@Check_In_Date", CheckIn_Date)
                    '    cmd2.Parameters.AddWithValue("@Check_In_Time", CheckIn_Time)
                    '    cmd2.Parameters.AddWithValue("@UserID", ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)))
                    '    cmd2.Parameters.AddWithValue("@ID_No", txtIDNo.Text)
                    '    cmd2.Parameters.AddWithValue("@Guest_Type", ddlNormalSpecial.SelectedValue)
                    '    cmd2.Connection = con
                    '    cmd2.ExecuteNonQuery()
                End If

                con.Close()
                strRoomId = Request.QueryString("room_id")
                If IsNumeric(strRoomId) Then
                    RoomBooking(hdnGuestId.Value, ConvertingNumbers(hdnRoomId.Value))
                ElseIf IsNumeric(ddlRoomNo.SelectedValue) Then
                    If ddlRoomNo.SelectedValue > 0 Then
                        RoomBooking(hdnGuestId.Value, ddlRoomNo.SelectedValue)
                    End If
                End If
                HOTEL_SERVICE_CHARGE_CALCULATION(hdnGuestId.Value)
                'RoomBooking(ConvertingNumbers(hdnGuestId.Value), ddlRoomNo.SelectedValue)

            End Using
        End Using

    End Sub


    Protected Sub ddlHours_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlHours.SelectedIndexChanged
        guestIdChangeEvent = True

        Dim CheckIn_Date As Date = DateTime.ParseExact(txtCheckInDate.Text, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)
        Dim CheckOut_Date As Date = DateTime.ParseExact(txtCheckOutDate.Text, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)

        Dim promotionType As String
        Dim disCountVal As String
        Dim RentInfo As DataSet
        Dim Promotion As DataSet
        If IsNumeric(ddlHours.SelectedValue) Then
            RentInfo = FetchDataFromTable("SELECT [RENT_ID], [ROOM_ID],Room_Hours,Room_Rent FROM [RoomRent] where (RENT_ID = " & ddlHours.SelectedValue & ")")
        Else
            totalhours.Value = "0"
            txtRoomRent.Text = ""
            txtRoomTax.Text = ""
            txtRentTotal.Text = ""
            txtRoomDiscount.Text = ""
            ddlPromotion.SelectedIndex = 0
            Return
        End If
        If IsNumeric(ddlPromotion.SelectedValue) Then
            Promotion = FetchDataFromTable("SELECT * FROM Promotions where Promotion_ID=" & ddlPromotion.SelectedValue & "")
        End If
        If Promotion IsNot Nothing Then
            If Promotion.Tables(0).Rows.Count > 0 Then
                promotionType = Promotion.Tables(0).Rows(0)("Promotion_Type").ToString
            End If
        End If
        If RentInfo IsNot Nothing Then
            If RentInfo.Tables(0).Rows.Count > 0 Then
                totalhours.Value = RentInfo.Tables(0).Rows(0)("Room_Hours").ToString
                txtRoomRent.Text = Format(Val(Convert.ToDouble(RentInfo.Tables(0).Rows(0)("Room_Rent").ToString)), "0.00")
                '* Convert.ToDouble(RentInfo.Tables(0).Rows(0)("Room_Hours").ToString)), "0.00")
                txtRoomTax.Text = Format(Val(Convert.ToDouble(txtRoomRent.Text) * hdnTaxPercent.Value / 100), "0.00")
                txtRentTotal.Text = Format(Val(Convert.ToDouble(txtRoomRent.Text) + Convert.ToDouble(txtRoomTax.Text)), "0.00")
                If RentInfo.Tables(0).Rows(0)("Room_Hours").ToString = "24" Then
                    txtCheckOutDate.Text = Format(CheckIn_Date.AddDays(1), "dd/MM/yyyy") + " 14:00:00"
                    txtCheckOutTime.Text = "14:00:00"
                Else
                    If hdnRowUpdate.Value > 0 And IsNumeric(hdnGuestId.Value) And gvBill.Rows.Count > 0 Then
                        txtCheckOutDate.Text = Format(CheckIn_Date.AddHours(RentInfo.Tables(0).Rows(0)("Room_Hours").ToString), "dd/MM/yyyy HH:mm:ss")
                        txtCheckOutTime.Text = Format(CheckIn_Date.AddHours(RentInfo.Tables(0).Rows(0)("Room_Hours").ToString), "HH:mm:ss")
                    ElseIf IsNumeric(hdnGuestId.Value) And gvBill.Rows.Count > 0 Then
                        txtCheckOutDate.Text = Format(CheckOut_Date.AddHours(RentInfo.Tables(0).Rows(0)("Room_Hours").ToString), "dd/MM/yyyy HH:mm:ss")
                        txtCheckOutTime.Text = Format(CheckOut_Date.AddHours(RentInfo.Tables(0).Rows(0)("Room_Hours").ToString), "HH:mm:ss")
                    Else
                        txtCheckOutDate.Text = Format(CheckIn_Date.AddHours(RentInfo.Tables(0).Rows(0)("Room_Hours").ToString), "dd/MM/yyyy HH:mm:ss")
                        txtCheckOutTime.Text = Format(CheckIn_Date.AddHours(RentInfo.Tables(0).Rows(0)("Room_Hours").ToString), "HH:mm:ss")
                    End If
                    'txtCheckOutDate.Text = Format(CheckIn_Date.AddHours(ConvertingNumbers(totalhours.Value)), "dd/MM/yyyy HH:mm:ss")

                End If
                If promotionType = "Extra" Or promotionType = "Extra Hour" Or promotionType = "Extra Hours" Then
                    totalhours.Value = Convert.ToDouble(RentInfo.Tables(0).Rows(0)("Room_Hours").ToString) + Convert.ToDouble(Promotion.Tables(0).Rows(0)("Promotion_Value").ToString)

                    txtCheckOutDate.Text = Format(CheckIn_Date.AddHours(ConvertingNumbers(totalhours.Value)), "dd/MM/yyyy HH:mm:ss")
                    txtRoomRent.Text = Format(Val(Convert.ToDouble(RentInfo.Tables(0).Rows(0)("Room_Rent").ToString)), "0.00")
                ElseIf promotionType = "%" Then
                    disCountVal = Promotion.Tables(0).Rows(0)("Promotion_Value").ToString
                    txtRoomDiscount.Text = Format(Val(Convert.ToDouble(txtRoomRent.Text) * disCountVal / 100), "0.00")
                End If
                txtRoomTax.Text = Format(Val(Convert.ToDouble(txtRoomRent.Text) * hdnTaxPercent.Value / 100), "0.00")
                If IsNumeric(txtRoomDiscount.Text) Then
                    txtRentTotal.Text = Format(Val(Convert.ToDouble(txtRoomRent.Text) + Convert.ToDouble(txtRoomTax.Text) - Convert.ToDouble(txtRoomDiscount.Text)), "0.00")
                Else
                    txtRentTotal.Text = Format(Val((Convert.ToDouble(txtRoomRent.Text) + Convert.ToDouble(txtRoomTax.Text))), "0.00")
                End If

            End If
        End If

    End Sub

    Protected Sub ddlPromotion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPromotion.SelectedIndexChanged
        guestIdChangeEvent = True
        Dim datTimeseparate2 As String() = txtCheckOutDate.Text.ToString.Split(" ")

        Dim CheckIn_Date As Date = DateTime.ParseExact(txtCheckInDate.Text, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)
        Dim CheckOut_Date As Date = DateTime.ParseExact(txtCheckOutDate.Text, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)
        Dim CheckOut_Time As Date = DateTime.ParseExact(datTimeseparate2(1), "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)
        'Dim CheckOut_Time As Date = DateTime.ParseExact(datTimeseparate2(1) + " " + datTimeseparate2(2), "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)


        Dim promotionType As String
        Dim disCountVal As String
        Dim RentInfo As DataSet
        Dim TaxInfo As DataSet
        Dim Promotion As DataSet
        Dim TaxPercent As Decimal
        TaxInfo = FetchDataFromTable("SELECT * FROM TAXInfo")
        TaxPercent = Convert.ToDouble(TaxInfo.Tables(0).Rows(0)("Rate_1").ToString)
        If IsNumeric(ddlHours.SelectedValue) Then
            RentInfo = FetchDataFromTable("SELECT [RENT_ID], [ROOM_ID],Room_Hours,Room_Rent FROM [RoomRent] where (RENT_ID = " & ddlHours.SelectedValue & ")")

            If RentInfo IsNot Nothing Then
                If RentInfo.Tables(0).Rows.Count > 0 Then
                    totalhours.Value = RentInfo.Tables(0).Rows(0)("Room_Hours").ToString
                    txtRoomRent.Text = Format(Val(Convert.ToDouble(RentInfo.Tables(0).Rows(0)("Room_Rent").ToString) * Convert.ToDouble(RentInfo.Tables(0).Rows(0)("Room_Hours").ToString)), "0.00")
                    txtRoomTax.Text = Format(Val(Convert.ToDouble(txtRoomRent.Text) * TaxPercent / 100), "0.00")
                    txtRentTotal.Text = Format(Val(Convert.ToDouble(txtRoomRent.Text) + Convert.ToDouble(txtRoomTax.Text)), "0.00")
                    If IsNumeric(ddlPromotion.SelectedValue) Then
                        Promotion = FetchDataFromTable("SELECT * FROM Promotions where Promotion_ID=" & ddlPromotion.SelectedValue & "")
                    End If
                    If Promotion IsNot Nothing Then
                        If Promotion.Tables(0).Rows.Count > 0 Then
                            promotionType = Promotion.Tables(0).Rows(0)("Promotion_Type").ToString
                        End If
                    End If

                    If promotionType = "Extra" Or promotionType = "Extra Hour" Or promotionType = "Extra Hours" Then
                        totalhours.Value = Convert.ToDouble(RentInfo.Tables(0).Rows(0)("Room_Hours").ToString) + Convert.ToDouble(Promotion.Tables(0).Rows(0)("Promotion_Value").ToString)

                        txtCheckOutDate.Text = Format(CheckIn_Date.AddHours(ConvertingNumbers(totalhours.Value)), "dd/MM/yyyy HH:mm:ss")
                        txtRoomRent.Text = Format(Val(Convert.ToDouble(RentInfo.Tables(0).Rows(0)("Room_Rent").ToString)), "0.00")
                        '* Convert.ToDouble(totalhours.Value)
                        txtRoomDiscount.Text = ""
                    ElseIf promotionType = "%" Then
                        disCountVal = Promotion.Tables(0).Rows(0)("Promotion_Value").ToString
                        totalhours.Value = Format(Val(Convert.ToDouble(RentInfo.Tables(0).Rows(0)("Room_Hours").ToString)), "0.00")
                        txtRoomRent.Text = Format(Val(Convert.ToDouble(RentInfo.Tables(0).Rows(0)("Room_Rent").ToString)), "0.00")
                        '* Convert.ToDouble(totalhours.Value)
                        txtRoomDiscount.Text = Format(Val(Convert.ToDouble(txtRoomRent.Text) * disCountVal / 100), "0.00")
                    Else
                        txtRoomDiscount.Text = ""
                    End If
                    txtRoomTax.Text = Format(Val(Convert.ToDouble(txtRoomRent.Text) * TaxPercent / 100), "0.00")


                    If IsNumeric(txtRoomDiscount.Text) Then
                        txtRentTotal.Text = Format(Val(Convert.ToDouble(txtRoomRent.Text) + Convert.ToDouble(txtRoomTax.Text) - Convert.ToDouble(txtRoomDiscount.Text)), "0.00")
                    Else
                        txtRentTotal.Text = Format(Val(Convert.ToDouble(txtRoomRent.Text) + Convert.ToDouble(txtRoomTax.Text)), "0.00")
                    End If

                End If
            End If


        End If




    End Sub
    Private Sub AddNewRowToGrid()
        If ViewState("CurrentTable") IsNot Nothing Then
            Dim dtCurrentTable As DataTable = CType(ViewState("CurrentTable"), DataTable)
            Dim drCurrentRow As DataRow = Nothing

            Dim rowCount As Integer = dtCurrentTable.Rows.Count
            If dtCurrentTable.Rows.Count > 0 Then
                drCurrentRow = dtCurrentTable.NewRow()
                'drCurrentRow("Date/Hour") = txtCheckInDate.Text
                If IsNumeric(hdnGuestId.Value) And gvBill.Rows.Count > 0 Then
                    drCurrentRow("Date/Hour") = DateTime.Now.AddHours(2).ToString("dd/MM/yyyy HH:mm:ss")
                    hdnNewPayment.Value = "1"
                Else
                    drCurrentRow("Date/Hour") = txtCheckInDate.Text
                    hdnNewPayment.Value = "0"
                End If

                If ddlRoomNo.SelectedValue <> "Please select" Then
                    drCurrentRow("Room") = ddlRoomNo.SelectedItem.Text
                End If

                drCurrentRow("Rate") = ddlHours.Items(ddlHours.SelectedIndex).Text + " hours"
                drCurrentRow("Type") = ddlPayType.Items(ddlPayType.SelectedIndex).Text
                drCurrentRow("Tax") = txtRoomTax.Text
                If IsNumeric(txtRoomDiscount.Text) Then
                    drCurrentRow("Discount") = txtRoomDiscount.Text
                Else
                    drCurrentRow("Discount") = "0.00"
                End If
                If ddlPromotion.SelectedIndex > 0 Then
                    drCurrentRow("Promo") = ddlPromotion.Items(ddlPromotion.SelectedIndex).Text
                Else
                    drCurrentRow("Promo") = ""
                End If

                drCurrentRow("Payment") = txtRentTotal.Text
                dtCurrentTable.Rows.Add(drCurrentRow)
                gvBill.DataSource = dtCurrentTable
                gvBill.DataBind()

                Dim total As Decimal = dtCurrentTable.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("Payment"))
                gvBill.FooterRow.Cells(8).Text = "Total"
                gvBill.FooterRow.Cells(8).HorizontalAlign = HorizontalAlign.Right
                gvBill.FooterRow.Cells(9).Text = "$" + total.ToString("N2")
                ViewState("CurrentTable") = dtCurrentTable
            End If
        Else
            Dim dtCurrentTable As DataTable = New DataTable()
            dtCurrentTable.Columns.Add(New DataColumn("Date/Hour", GetType(String)))
            dtCurrentTable.Columns.Add(New DataColumn("Room", GetType(String)))
            dtCurrentTable.Columns.Add(New DataColumn("Rate", GetType(String)))
            dtCurrentTable.Columns.Add(New DataColumn("Type", GetType(String)))
            dtCurrentTable.Columns.Add(New DataColumn("Tax", GetType(String)))
            dtCurrentTable.Columns.Add(New DataColumn("Discount", GetType(String)))
            dtCurrentTable.Columns.Add(New DataColumn("Promo", GetType(String)))
            dtCurrentTable.Columns.Add(New DataColumn("Payment", GetType(Decimal)))
            Dim drCurrentRow As DataRow = Nothing

            Dim rowCount As Integer = dtCurrentTable.Rows.Count

            drCurrentRow = dtCurrentTable.NewRow()
            'drCurrentRow("Date/Hour") = txtCheckInDate.Text
            If IsNumeric(hdnGuestId.Value) And gvBill.Rows.Count > 0 Then
                drCurrentRow("Date/Hour") = DateTime.Now.AddHours(2).ToString("dd/MM/yyyy HH:mm:ss")
                hdnNewPayment.Value = "1"
            Else
                drCurrentRow("Date/Hour") = txtCheckInDate.Text
                hdnNewPayment.Value = "0"
            End If
            '+ "-" + txtCheckInTime.Text
            If ddlRoomNo.SelectedValue <> "Please select" Then
                drCurrentRow("Room") = ddlRoomNo.SelectedItem.Text
            End If


            drCurrentRow("Rate") = ddlHours.Items(ddlHours.SelectedIndex).Text + " hours"
            drCurrentRow("Type") = ddlPayType.Items(ddlPayType.SelectedIndex).Text
            drCurrentRow("Tax") = txtRoomTax.Text
            If IsNumeric(txtRoomDiscount.Text) Then
                drCurrentRow("Discount") = txtRoomDiscount.Text
            Else
                drCurrentRow("Discount") = "0.00"
            End If
            If ddlPromotion.SelectedIndex > 0 Then
                drCurrentRow("Promo") = ddlPromotion.Items(ddlPromotion.SelectedIndex).Text
            Else
                drCurrentRow("Promo") = ""
            End If
            drCurrentRow("Payment") = txtRentTotal.Text
            dtCurrentTable.Rows.Add(drCurrentRow)
            gvBill.DataSource = dtCurrentTable
            gvBill.DataBind()
            Dim total As Decimal = dtCurrentTable.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("Payment"))
            gvBill.FooterRow.Cells(8).Text = "Total"
            gvBill.FooterRow.Cells(8).HorizontalAlign = HorizontalAlign.Right
            gvBill.FooterRow.Cells(9).Text = "$" + total.ToString("N2")
            ViewState("CurrentTable") = dtCurrentTable

        End If

        'totalhours.Value = 0
        'txtRoomRent.Text = ""
        'txtRoomTax.Text = ""
        'txtRentTotal.Text = ""
        'txtRoomDiscount.Text = ""
        'ddlPayType.SelectedIndex = 0
    End Sub


    Protected Sub gvBill_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvBill.SelectedIndexChanged

        Dim errortext As String = ""
        Dim row As GridViewRow = gvBill.SelectedRow
        If row.RowType = DataControlRowType.DataRow Then

            Dim btn As Button = TryCast(row.FindControl("btngvselect"), Button)

            If btn.Text = "actualizar" Then
                hdnRowUpdate.Value = 0
                Dim index As Integer = Convert.ToInt32(row.RowIndex)
                Dim dt As DataTable = TryCast(ViewState("CurrentTable"), DataTable)
                Dim total As Decimal
                Try

                    If ConvertingNumbers(hdnGuestId.Value) Then


                        Dim datTimeseparate As String() = txtCheckInDate.Text.ToString.Split(" ")
                        Dim datTimeseparate2 As String() = txtCheckOutDate.Text.ToString.Split(" ")

                        Dim CheckIn_Date As Date = DateTime.ParseExact(datTimeseparate(0), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
                        Dim CheckOut_Date As Date = DateTime.ParseExact(datTimeseparate2(0), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)


                        Dim CheckIn_Time As String = datTimeseparate(1)
                        Dim CheckOut_Time As String = datTimeseparate2(1)

                        Using con As New SqlConnection(CnString)

                            Dim BillID As Integer = Convert.ToInt32(gvBill.Rows(index).Cells(11).Text)
                            Dim cmd2 As New SqlCommand("update       BillDetails Set   Rent_ID=@Rent_ID, PayType=@PayType, Tax=@Tax, Discount=@Discount, Promotion=@Promotion, Payment=@Payment, Check_In_Date=@Check_In_Date, Check_In_Time=@Check_In_Time, UserID=@UserID,ID_No=@ID_No,Guest_Type=@Guest_Type where  BILL_ID=@BILL_ID") 'Room_ID=@Room_ID and Guest_ID=@Guest_ID and
                            cmd2.CommandType = CommandType.Text
                            ' Dim adapFam As New SqlDataAdapter 

                            cmd2.Parameters.Clear()
                            'cmd2.Parameters.AddWithValue("@GUEST_ID", hdnGuestId.Value)
                            'cmd2.Parameters.AddWithValue("@Room_ID", ConvertingNumbers(strRoomId))
                            cmd2.Parameters.AddWithValue("@Rent_ID", ddlHours.SelectedValue)
                            cmd2.Parameters.AddWithValue("@PayType", ddlPayType.SelectedItem.Text)
                            cmd2.Parameters.AddWithValue("@Tax", txtRoomTax.Text)

                            If txtRoomDiscount.Text Is "" Then
                                txtRoomDiscount.Text = "0.00"
                            End If

                            cmd2.Parameters.AddWithValue("@Discount", txtRoomDiscount.Text)

                            Dim promo As String
                            If IsNumeric(ddlPromotion.SelectedValue) Then
                                promo = ddlPromotion.SelectedItem.Text
                            Else
                                promo = ""
                            End If

                            cmd2.Parameters.AddWithValue("@Promotion", promo)
                            cmd2.Parameters.AddWithValue("@Payment", Convert.ToDecimal(txtRentTotal.Text))
                            cmd2.Parameters.AddWithValue("@Check_In_Date", CheckIn_Date)
                            cmd2.Parameters.AddWithValue("@Check_In_Time", CheckIn_Time)
                            cmd2.Parameters.AddWithValue("@UserID", ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)))
                            cmd2.Parameters.AddWithValue("@ID_No", txtIDNo.Text)
                            cmd2.Parameters.AddWithValue("@Guest_Type", ddlNormalSpecial.SelectedValue)
                            cmd2.Parameters.AddWithValue("@BILL_ID", BillID)
                            cmd2.Connection = con
                            con.Open()

                            cmd2.ExecuteNonQuery()
                            con.Close()
                            'gvBill.EditIndex = -1
                            'gvBill.DataBind()
                        End Using

                    End If

                Catch ex As Exception
                    errortext = ex.Message
                End Try
                SearchByIDNO()

            Else
                hdnRowUpdate.Value = 1
                Dim hourval As String()
                If row IsNot Nothing Then
                    'datehour
                    'room
                    hourval = row.Cells(5).Text.Split(" ")
                    If hourval.Length > 1 Then
                        ddlHours.SelectedValue = ddlHours.Items.FindByText(hourval(0)).Value
                    Else
                        ddlHours.SelectedValue = ddlHours.Items.FindByText(row.Cells(5).Text).Value
                    End If
                    ddlPayType.SelectedValue = ddlPayType.Items.FindByText(row.Cells(6).Text).Value
                    txtRoomTax.Text = row.Cells(7).Text
                    txtRoomDiscount.Text = row.Cells(8).Text

                    Dim promo As String
                    promo = row.Cells(9).Text.Replace("&nbsp;", "")

                    If promo IsNot "" Then
                        If ddlPromotion.Items.FindByText(promo).Value IsNot Nothing Then

                            ddlPromotion.SelectedValue = ddlPromotion.Items.FindByText(promo).Value
                        End If
                    End If


                    txtRentTotal.Text = row.Cells(10).Text
                    txtRoomRent.Text = (Convert.ToDecimal(row.Cells(10).Text) + Convert.ToDecimal(row.Cells(8).Text)) - Convert.ToDecimal(row.Cells(7).Text)
                End If

                btn.Text = "actualizar"

            End If

        End If

        guestIdChangeEvent = True

    End Sub

    Protected Sub txtRoomDiscount_TextChanged(sender As Object, e As EventArgs) Handles txtRoomDiscount.TextChanged
        guestIdChangeEvent = True

        If IsNumeric(txtRoomDiscount.Text) Then
            txtRentTotal.Text = Convert.ToDouble(txtRoomRent.Text) + Convert.ToDouble(txtRoomTax.Text) - Convert.ToDouble(txtRoomDiscount.Text)
        Else
            txtRentTotal.Text = Convert.ToDouble(txtRoomRent.Text) + Convert.ToDouble(txtRoomTax.Text)
        End If
    End Sub
    Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim item As String = e.Row.Cells(0).Text
            For Each button As Button In e.Row.Cells(1).Controls.OfType(Of Button)()
                If button.CommandName = "Delete" Then
                    button.Attributes("onclick") = "if(!confirm('Do you want to delete " + item + "?')){ return false; };"
                End If
            Next
        End If
    End Sub
    Protected Sub OnRowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim index As Integer = Convert.ToInt32(e.RowIndex)
        Dim dt As DataTable = TryCast(ViewState("CurrentTable"), DataTable)
        Dim total As Decimal
        If ConvertingNumbers(hdnGuestId.Value) Then


            Dim BillID As Integer = Convert.ToInt32(gvBill.Rows(index).Cells(11).Text)
            ExecuteSqlQuery("delete from BillDetails where (BILL_ID = " & BillID & ")")

            If dt Is Nothing Then
                Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
                Using con As New SqlConnection(constr)
                    If IsNumeric(hdnRoomId.Value) Then
                        Using sqlComm2 As New SqlCommand("GetBillInfoByGuestIDRoomID")
                            sqlComm2.CommandType = CommandType.StoredProcedure
                            sqlComm2.Parameters.AddWithValue("@RoomID", ConvertingNumbers(hdnRoomId.Value))
                            sqlComm2.Parameters.AddWithValue("@GuestID", ConvertingNumbers(hdnGuestId.Value))
                            sqlComm2.Connection = con
                            Dim billTbl As DataSet = New DataSet
                            Dim sqlDA As New SqlDataAdapter(sqlComm2)
                            Dim sqlCB As New SqlCommandBuilder(sqlDA)
                            sqlDA.Fill(billTbl)
                            If billTbl IsNot Nothing Then
                                If billTbl.Tables(0).Rows.Count > 0 Then
                                    gvBill.DataSource = billTbl.Tables(0)
                                    gvBill.DataBind()
                                    total = billTbl.Tables(0).AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("Payment"))
                                    gvBill.HeaderRow.Cells(3).Visible = False
                                    gvBill.HeaderRow.Cells(11).Visible = False
                                    gvBill.HeaderRow.Cells(12).Visible = False
                                    gvBill.HeaderRow.Cells(13).Visible = False
                                    gvBill.HeaderRow.Cells(14).Visible = False
                                    gvBill.HeaderRow.Cells(15).Visible = False
                                    gvBill.HeaderRow.Cells(16).Visible = False
                                    gvBill.HeaderRow.Cells(17).Visible = False
                                    gvBill.HeaderRow.Cells(18).Visible = False
                                    gvBill.HeaderRow.Cells(19).Visible = False
                                    gvBill.HeaderRow.Cells(20).Visible = False
                                    gvBill.HeaderRow.Cells(21).Visible = False
                                    gvBill.HeaderRow.Cells(22).Visible = False
                                    gvBill.HeaderRow.Cells(23).Visible = False


                                    gvBill.FooterRow.Cells(2).Visible = False
                                    gvBill.FooterRow.Cells(11).Visible = False
                                    gvBill.FooterRow.Cells(12).Visible = False
                                    gvBill.FooterRow.Cells(13).Visible = False
                                    gvBill.FooterRow.Cells(14).Visible = False
                                    gvBill.FooterRow.Cells(15).Visible = False
                                    gvBill.FooterRow.Cells(16).Visible = False
                                    gvBill.FooterRow.Cells(17).Visible = False
                                    gvBill.FooterRow.Cells(18).Visible = False
                                    gvBill.FooterRow.Cells(19).Visible = False
                                    gvBill.FooterRow.Cells(20).Visible = False
                                    gvBill.FooterRow.Cells(21).Visible = False
                                    gvBill.FooterRow.Cells(22).Visible = False
                                    gvBill.FooterRow.Cells(23).Visible = False

                                    For i As Integer = 0 To gvBill.Rows.Count - 1


                                        gvBill.Rows(i).Cells(2).Visible = False
                                        gvBill.Rows(i).Cells(11).Visible = False
                                        gvBill.Rows(i).Cells(12).Visible = False
                                        gvBill.Rows(i).Cells(13).Visible = False
                                        gvBill.Rows(i).Cells(14).Visible = False
                                        gvBill.Rows(i).Cells(15).Visible = False
                                        gvBill.Rows(i).Cells(16).Visible = False
                                        gvBill.Rows(i).Cells(17).Visible = False
                                        gvBill.Rows(i).Cells(18).Visible = False
                                        gvBill.Rows(i).Cells(19).Visible = False
                                        gvBill.Rows(i).Cells(20).Visible = False
                                        gvBill.Rows(i).Cells(21).Visible = False
                                        gvBill.Rows(i).Cells(22).Visible = False
                                        gvBill.Rows(i).Cells(23).Visible = False
                                    Next

                                End If
                            End If
                        End Using
                    End If
                End Using

            End If

        Else

            dt.Rows(index).Delete()
            ViewState("CurrentTable") = dt
            gvBill.DataSource = dt
            gvBill.DataBind()
            total = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("Payment"))
        End If

        If gvBill.Rows.Count > 0 Then
            gvBill.FooterRow.Cells(8).Text = "Total"
            gvBill.FooterRow.Cells(8).HorizontalAlign = HorizontalAlign.Right
            gvBill.FooterRow.Cells(9).Text = "$" + total.ToString("N2")
        End If

        Response.Redirect("RoomView")

    End Sub
    Protected Sub txtGuestID_TextChanged(sender As Object, e As EventArgs) Handles txtGuestID.TextChanged
        guestIdChangeEvent = True

        If txtGuestID.Text IsNot "" Then

            Using conn As New SqlConnection()
                conn.ConnectionString = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
                Using cmd As New SqlCommand()
                    cmd.CommandText = "SELECT        GUEST_ID, GEST_ID, GuestName, Address, PhoneNo,Social_Type,  ID_Type, Gender, Purpose, Arrival_n_Departure_Date_Time, Note, Check_In_Date, Check_In_Time, Check_Out_Date, Check_Out_Time from      GuestInformation where GEST_ID = @SearchText"
                    cmd.Parameters.AddWithValue("@SearchText", txtGuestID.Text)
                    cmd.Connection = conn
                    conn.Open()
                    Dim sqlReader As SqlDataReader = cmd.ExecuteReader()

                    If sqlReader.HasRows Then

                        While (sqlReader.Read())
                            hdnGuestId.Value = sqlReader.GetValue(0)

                            txtGuestName.Text = sqlReader.GetValue(2)
                            txtAddress.Text = sqlReader.GetValue(3)
                            txtPhoneNo.Text = sqlReader.GetValue(4)

                            'If sqlReader.GetValue(5) IsNot "" Then
                            '    ddlNormalSpecial.SelectedValue = ddlNormalSpecial.Items.FindByText(sqlReader.GetValue(5)).Value
                            'End If
                            If sqlReader.GetValue(5) IsNot "" Then
                                CheckBoxList1.SelectedValue = CheckBoxList1.Items.FindByText(sqlReader.GetValue(5)).Value
                            End If

                            ddlIDType.SelectedValue = ddlIDType.Items.FindByText(sqlReader.GetValue(6)).Value

                            'txtIDNo.Text = sqlReader.GetValue(8)
                            If sqlReader.GetValue(7) IsNot "" Then
                                ddlGender.SelectedValue = ddlGender.Items.FindByText(sqlReader.GetValue(7)).Value

                            End If
                            txtPurpose.Text = sqlReader.GetValue(8)
                            txtArrivalDeparture.Text = sqlReader.GetValue(9)


                            txtNote.Value = sqlReader.GetValue(10)



                            'txtCheckInDate.Text = sqlReader.GetValue(13) + " " + sqlReader.GetValue(14)
                            'txtCheckInTime.Text = sqlReader.GetValue(14)
                            'txtCheckOutDate.Text = sqlReader.GetValue(15) + " " + sqlReader.GetValue(16)
                            'txtCheckOutTime.Text = sqlReader.GetValue(16)
                        End While

                    Else
                        hdnGuestId.Value = 0

                        txtGuestName.Text = ""
                        txtAddress.Text = ""
                        txtPhoneNo.Text = ""
                        CheckBoxList1.SelectedValue = -1


                        ddlIDType.SelectedValue = 0
                        ddlGender.SelectedValue = 0
                        txtPurpose.Text = ""
                        txtArrivalDeparture.Text = ""
                        txtNote.Value = ""
                        ddlRoomNo.DataSource = FetchDataFromTable("SELECT        ROOM_ID, Room_No + '  - [Booked -' + Occupied + ']' AS RoomType  FROM            RoomDetails")
                        ddlRoomNo.DataTextField = "RoomType"
                        ddlRoomNo.DataValueField = "ROOM_ID"
                        ddlRoomNo.DataBind()
                        ddlRoomNo.Items.Insert(0, New ListItem("Please select"))


                        ddlPromotion.DataSource = FetchDataFromTable("SELECT * FROM [Promotions]")
                        ddlPromotion.DataTextField = "Promotion"
                        ddlPromotion.DataValueField = "Promotion_ID"
                        ddlPromotion.DataBind()
                        ddlPromotion.Items.Insert(0, New ListItem("Please select Promoción"))



                    End If

                    sqlReader.Close()
                    conn.Close()
                End Using
            End Using

        End If
    End Sub
    'Protected Sub txtCheckInDate_TextChanged(sender As Object, e As EventArgs)
    '    Dim CheckIn_Date As Date = DateTime.ParseExact(txtCheckInDate.Text, "d/M/yyyy h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None)
    '    txtCheckOutDate.Text = Format(CheckIn_Date.AddHours(ConvertingNumbers(totalhours.Value)), "d/M/yyyy h:mm tt")
    'End Sub

    'Protected Sub txtCheckInDate_Disposed(sender As Object, e As EventArgs)
    '    Dim CheckIn_Date As Date = DateTime.ParseExact(txtCheckInDate.Text, "d/M/yyyy h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None)
    '    txtCheckOutDate.Text = Format(CheckIn_Date.AddHours(ConvertingNumbers(totalhours.Value)), "d/M/yyyy h:mm tt")
    'End Sub
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        SearchByIDNO()
    End Sub

    Private Sub SearchByIDNO()
        guestIdChangeEvent = True

        If txtIDNo.Text IsNot "" Then

            ddlPromotion.DataSource = FetchDataFromTable("SELECT * FROM [Promotions]")
            ddlPromotion.DataTextField = "Promotion"
            ddlPromotion.DataValueField = "Promotion_ID"
            ddlPromotion.DataBind()
            ddlPromotion.Items.Insert(0, New ListItem("Please select Promoción"))

            Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
            Using con As New SqlConnection(constr)
                Using sqlComm As New SqlCommand("GetGuestInfoByID_No")
                    sqlComm.CommandType = CommandType.StoredProcedure
                    sqlComm.Parameters.AddWithValue("@ID_No", txtIDNo.Text)
                    sqlComm.Connection = con
                    con.Open()
                    Dim sqlReader As SqlDataReader = sqlComm.ExecuteReader()

                    If sqlReader.HasRows Then

                        While (sqlReader.Read())
                            hdnGuestId.Value = sqlReader.GetValue(0)
                            txtGuestID.Text = sqlReader.GetValue(1)
                            txtGuestName.Text = sqlReader.GetValue(2)
                            txtAddress.Text = sqlReader.GetValue(3)

                            If sqlReader.GetValue(4) IsNot "" Then
                                CheckBoxList1.SelectedValue = CheckBoxList1.Items.FindByText(sqlReader.GetValue(4)).Value
                            End If

                            If sqlReader.GetValue(5) IsNot "" Then
                                ddlNormalSpecial.SelectedValue = ddlNormalSpecial.Items.FindByText(sqlReader.GetValue(5)).Value
                            End If

                            txtNote.Value = sqlReader.GetValue(6)



                            ddlIDType.SelectedValue = ddlIDType.Items.FindByText(sqlReader.GetValue(7)).Value
                            txtIDNo.Text = sqlReader.GetValue(8)
                            ddlGender.SelectedValue = ddlGender.Items.FindByText(sqlReader.GetValue(9)).Value
                            txtPhoneNo.Text = sqlReader.GetValue(10)
                            txtArrivalDeparture.Text = sqlReader.GetValue(11)
                            txtPurpose.Text = sqlReader.GetValue(12)
                            txtCheckInDate.Text = sqlReader.GetValue(13) + " " + sqlReader.GetValue(14)
                            txtCheckInTime.Text = sqlReader.GetValue(14)
                            txtCheckOutDate.Text = sqlReader.GetValue(15) + " " + sqlReader.GetValue(16)
                            txtCheckOutTime.Text = sqlReader.GetValue(16)
                            lblremaintime.Text = sqlReader.GetValue(17) + " " + sqlReader.GetValue(18)
                            If lblremaintime.Text.Trim IsNot "" Then
                                remaintext.Visible = True
                            End If
                            hdnRoomId.Value = sqlReader.GetValue(21)

                            ddlRoomNo.SelectedValue = sqlReader.GetValue(21)


                        End While


                    End If

                    sqlReader.Close()

                End Using
                If IsNumeric(hdnGuestId.Value) Then


                    ddlHours.DataSource = FetchDataFromTable("SELECT [RENT_ID], [ROOM_ID],Room_Hours,Room_Rent FROM [RoomRent] where (ROOM_ID = " & hdnRoomId.Value & ")")
                    ddlHours.DataTextField = "Room_Hours"
                    ddlHours.DataValueField = "RENT_ID"
                    ddlHours.DataBind()
                    ddlHours.Items.Insert(0, New ListItem("Please select hour(s)"))

                    Using sqlComm2 As New SqlCommand("GetBillInfoByGuestIDRoomID")
                        sqlComm2.CommandType = CommandType.StoredProcedure
                        sqlComm2.Parameters.AddWithValue("@RoomID", hdnRoomId.Value)
                        sqlComm2.Parameters.AddWithValue("@GuestID", ConvertingNumbers(hdnGuestId.Value))
                        sqlComm2.Connection = con
                        Dim billTbl As DataSet = New DataSet
                        Dim sqlDA As New SqlDataAdapter(sqlComm2)
                        Dim sqlCB As New SqlCommandBuilder(sqlDA)
                        sqlDA.Fill(billTbl)

                        If billTbl IsNot Nothing Then
                            If billTbl.Tables(0).Rows.Count > 0 Then

                                btnCheckInSubmit.Text = "Guardar"
                                'lnkChangeRoom.NavigateUrl = "RoomView.aspx?croom_id=" + strRoomId
                                'lnkChangeRoom.Enabled = True
                                'lnkChangeRoom.Visible = True
                                'lnkBlockRoom.Enabled = True
                                gvBill.DataSource = billTbl.Tables(0)
                                gvBill.DataBind()
                                'ddlHours.SelectedValue = ConvertingNumbers(billTbl.Tables(0).Rows(0)(11).ToString)
                                If billTbl.Tables(0).Rows(0)(7).ToString IsNot "" Then
                                    ddlPromotion.SelectedValue = ddlPromotion.Items.FindByText(billTbl.Tables(0).Rows(0)(7).ToString).Value
                                End If

                                Dim userID = billTbl.Tables(0).Rows(0)(15)

                                If ddlUsers.Items.FindByValue(userID.ToString()) IsNot Nothing Then

                                    ddlUsers.SelectedValue = userID

                                End If

                                ddlPayType.SelectedValue = ddlPayType.Items.FindByText(billTbl.Tables(0).Rows(0)(4).ToString).Value
                                txtRoomTax.Text = billTbl.Tables(0).Rows(0)(5).ToString
                                txtRoomDiscount.Text = billTbl.Tables(0).Rows(0)(6).ToString
                                txtRentTotal.Text = billTbl.Tables(0).Rows(0)(8).ToString
                                txtRoomRent.Text = billTbl.Tables(0).Rows(0)(18).ToString
                                If billTbl.Tables(0).Rows(0)(22) IsNot "" Then
                                    ddlNormalSpecial.SelectedValue = ddlNormalSpecial.Items.FindByText(billTbl.Tables(0).Rows(0)(22)).Value
                                End If

                                gvBill.HeaderRow.Cells(4).Visible = False
                                gvBill.HeaderRow.Cells(11).Visible = False
                                gvBill.HeaderRow.Cells(12).Visible = False
                                gvBill.HeaderRow.Cells(13).Visible = False
                                gvBill.HeaderRow.Cells(14).Visible = False
                                gvBill.HeaderRow.Cells(15).Visible = False
                                gvBill.HeaderRow.Cells(16).Visible = False
                                gvBill.HeaderRow.Cells(17).Visible = False
                                gvBill.HeaderRow.Cells(18).Visible = False
                                gvBill.HeaderRow.Cells(19).Visible = False
                                gvBill.HeaderRow.Cells(20).Visible = False
                                gvBill.HeaderRow.Cells(21).Visible = False
                                gvBill.HeaderRow.Cells(22).Visible = False
                                gvBill.HeaderRow.Cells(23).Visible = False
                                gvBill.HeaderRow.Cells(24).Visible = False
                                gvBill.HeaderRow.Cells(25).Visible = False

                                gvBill.FooterRow.Cells(4).Visible = False
                                gvBill.FooterRow.Cells(11).Visible = False
                                gvBill.FooterRow.Cells(12).Visible = False
                                gvBill.FooterRow.Cells(13).Visible = False
                                gvBill.FooterRow.Cells(14).Visible = False
                                gvBill.FooterRow.Cells(15).Visible = False
                                gvBill.FooterRow.Cells(16).Visible = False
                                gvBill.FooterRow.Cells(17).Visible = False
                                gvBill.FooterRow.Cells(18).Visible = False
                                gvBill.FooterRow.Cells(19).Visible = False
                                gvBill.FooterRow.Cells(20).Visible = False
                                gvBill.FooterRow.Cells(21).Visible = False
                                gvBill.FooterRow.Cells(22).Visible = False
                                gvBill.FooterRow.Cells(23).Visible = False
                                gvBill.FooterRow.Cells(24).Visible = False
                                gvBill.FooterRow.Cells(25).Visible = False

                                For i As Integer = 0 To gvBill.Rows.Count - 1

                                    gvBill.Rows(i).Cells(4).Visible = False
                                    gvBill.Rows(i).Cells(11).Visible = False
                                    gvBill.Rows(i).Cells(12).Visible = False
                                    gvBill.Rows(i).Cells(13).Visible = False
                                    gvBill.Rows(i).Cells(14).Visible = False
                                    gvBill.Rows(i).Cells(15).Visible = False
                                    gvBill.Rows(i).Cells(16).Visible = False
                                    gvBill.Rows(i).Cells(17).Visible = False
                                    gvBill.Rows(i).Cells(18).Visible = False
                                    gvBill.Rows(i).Cells(19).Visible = False
                                    gvBill.Rows(i).Cells(20).Visible = False
                                    gvBill.Rows(i).Cells(21).Visible = False
                                    gvBill.Rows(i).Cells(22).Visible = False
                                    gvBill.Rows(i).Cells(23).Visible = False
                                    gvBill.Rows(i).Cells(24).Visible = False
                                    gvBill.Rows(i).Cells(25).Visible = False
                                Next
                            Else
                                gvBill.DataSource = Nothing
                                gvBill.DataBind()

                                ddlUsers.SelectedValue = 0
                                ddlPayType.SelectedValue = "Seleccione forma"
                                txtRoomTax.Text = ""
                                txtRoomDiscount.Text = ""
                                txtRentTotal.Text = ""
                                txtRoomRent.Text = ""
                            End If
                        End If

                        'Dim sqlReader2 As SqlDataReader = sqlComm2.ExecuteReader()

                        'If sqlReader2.HasRows Then

                        '    While (sqlReader2.Read())
                        '        hdnGuestId.Value = sqlReader2.GetValue(0)
                        '        txtGuestName.Text = sqlReader2.GetValue(1)
                        '        txtAddress.Text = sqlReader2.GetValue(2)
                        '        CheckBoxList1.SelectedValue = CheckBoxList1.Items.FindByText(sqlReader.GetValue(3)).Value
                        '        ddlNormalSpecial.SelectedValue = ddlNormalSpecial.Items.FindByText(sqlReader.GetValue(4)).Value
                        '        txtNote.Value = sqlReader.GetValue(5)
                        '        ddlIDType.SelectedValue = ddlIDType.Items.FindByText(sqlReader.GetValue(6)).Value
                        '        txtIDNo.Text = sqlReader.GetValue(7)
                        '        ddlGender.SelectedValue = ddlGender.Items.FindByText(sqlReader.GetValue(8)).Value
                        '        txtPhoneNo.Text = sqlReader.GetValue(9)
                        '        txtArrivalDeparture.Text = sqlReader.GetValue(10)
                        '        txtPurpose.Text = sqlReader.GetValue(11)
                        '        txtCheckInDate.Text = sqlReader.GetValue(12)
                        '        txtCheckInTime.Text = sqlReader.GetValue(13)
                        '        txtCheckOutDate.Text = sqlReader.GetValue(14)
                        '        txtCheckOutTime.Text = sqlReader.GetValue(15)
                        '    End While


                        'End If

                    End Using
                End If
                con.Close()
            End Using


        End If
    End Sub

    Protected Sub ddlRoomNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRoomNo.SelectedIndexChanged
        If ddlRoomNo.SelectedIndex > 0 Then
            ddlHours.DataSource = FetchDataFromTable("SELECT [RENT_ID], [ROOM_ID],Room_Hours,Room_Rent FROM [RoomRent] where (ROOM_ID = " & ddlRoomNo.SelectedValue & ")")
            ddlHours.DataTextField = "Room_Hours"
            ddlHours.DataValueField = "RENT_ID"
            ddlHours.DataBind()
            ddlHours.Items.Insert(0, New ListItem("Please select hour(s)"))

        End If
    End Sub

End Class