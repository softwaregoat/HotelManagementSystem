Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Web.Services

Public Class GuestInformation
    Inherits System.Web.UI.Page
    Protected strGuestId As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            'SetInitialRow()
            If Not Me.Page.User.Identity.IsAuthenticated Or Session("LOGGED_USER_ID") Is Nothing Then
                FormsAuthentication.RedirectToLoginPage()
            End If
            Dim currentPageFileName As String = New FileInfo(Me.Request.Url.LocalPath).Name
            Try
                CheckPermissions(currentPageFileName.Split(".aspx")(0).ToString, ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)))
                txtGuestID.Focus()
                Dim strGuestId = Request.QueryString("id")
                If Request.QueryString("id") IsNot Nothing Then
                    If IsNumeric(strGuestId) Then
                        hdnGuestId.Value = ConvertingNumbers(strGuestId)
                        SearchGuestById()
                        SearchGuest()
                    End If
                End If

            Catch ex As Exception
            End Try
        End If
        Me.txtGuestID.Focus()
        If (Not (Session("FlushMessage")) Is Nothing) Then
            lblMessege.Text = Session("FlushMessage").ToString()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "showModal();", True)
            Session("FlushMessage") = Nothing
        End If
    End Sub
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        SearchGuest()

    End Sub

    Private Sub SearchGuestById()
        Using conn As New SqlConnection()
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
            Using cmd As New SqlCommand()
                cmd.CommandText = "SELECT        GUEST_ID, GEST_ID, GuestName, Address, PhoneNo, Social_Type,  ID_Type, Gender, Purpose, Arrival_n_Departure_Date_Time, Note, Check_In_Date, Check_In_Time, Check_Out_Date, Check_Out_Time from      GuestInformation where GUEST_ID = @SearchText"
                cmd.Parameters.AddWithValue("@SearchText", hdnGuestId.Value)
                cmd.Connection = conn
                conn.Open()
                Dim sqlReader As SqlDataReader = cmd.ExecuteReader()

                If sqlReader.HasRows Then

                    While (sqlReader.Read())
                        hdnGuestId.Value = sqlReader.GetValue(0)
                        txtGuestID.Text = sqlReader.GetValue(1)
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


                        btnSave.Text = "Guardar"
                        'txtCheckInDate.Text = sqlReader.GetValue(13) + " " + sqlReader.GetValue(14)
                        'txtCheckInTime.Text = sqlReader.GetValue(14)
                        'txtCheckOutDate.Text = sqlReader.GetValue(15) + " " + sqlReader.GetValue(16)
                        'txtCheckOutTime.Text = sqlReader.GetValue(16)
                    End While


                End If

                sqlReader.Close()

                conn.Close()
            End Using

            Using cmd2 As New SqlCommand("GetFirstLastCheckInByGuest")
                    cmd2.CommandType = CommandType.StoredProcedure

                    cmd2.Parameters.AddWithValue("@Guest_ID", hdnGuestId.Value)
                cmd2.Connection = conn
                conn.Open()
                Dim sqlReader2 As SqlDataReader = cmd2.ExecuteReader()

                    If sqlReader2.HasRows Then

                        While (sqlReader2.Read())
                            firstVisitDate.Text = sqlReader2.GetValue(0)
                            lastVisitDate.Text = sqlReader2.GetValue(1)



                        End While
                    End If


                    conn.Close()
                End Using
            End Using
    End Sub



    Private Sub SearchGuest()
        Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
        Using con As New SqlConnection(constr)
            If IsNumeric(hdnGuestId.Value) Then


                'Using sqlComm As New SqlCommand("GetGuestInfoByGuestID")
                Using sqlComm As New SqlCommand("select  GEST_ID,GUEST_ID,GuestName,Address,Social_Type,Note,ID_Type,Gender,PhoneNo,Arrival_n_Departure_Date_Time,Purpose,ROOM_ID from GuestInformation where GUEST_ID=" & hdnGuestId.Value & "")
                    sqlComm.CommandType = CommandType.Text
                    'sqlComm.Parameters.AddWithValue("@GuestID", ConvertingNumbers(hdnGuestId.Value))
                    sqlComm.Connection = con
                    con.Open()
                    Dim sqlReader As SqlDataReader = sqlComm.ExecuteReader()

                    If sqlReader.HasRows Then

                        While (sqlReader.Read())
                            hdnGuestId.Value = sqlReader.GetValue(1)
                            txtGuestName.Text = sqlReader.GetValue(2)
                            txtAddress.Text = sqlReader.GetValue(3)
                            If sqlReader.GetValue(4) IsNot "" Then
                                CheckBoxList1.SelectedValue = CheckBoxList1.Items.FindByText(sqlReader.GetValue(4)).Value
                            End If
                            'If sqlReader.GetValue(4) IsNot "" Then
                            '    ddlNormalSpecial.SelectedValue = ddlNormalSpecial.Items.FindByText(sqlReader.GetValue(4)).Value
                            'End If
                            txtNote.Value = sqlReader.GetValue(5)
                            ddlIDType.SelectedValue = ddlIDType.Items.FindByText(sqlReader.GetValue(6)).Value
                            'txtIDNo.Text = sqlReader.GetValue(7)
                            If sqlReader.GetValue(7) IsNot "" Then
                                ddlGender.SelectedValue = ddlGender.Items.FindByText(sqlReader.GetValue(7)).Value
                            End If

                            txtPhoneNo.Text = sqlReader.GetValue(8)
                            txtArrivalDeparture.Text = sqlReader.GetValue(9)
                            txtPurpose.Text = sqlReader.GetValue(10)
                            'txtCheckInDate.Text = sqlReader.GetValue(12)
                            'txtCheckInTime.Text = sqlReader.GetValue(13)
                            'txtCheckOutDate.Text = sqlReader.GetValue(14)
                            'txtCheckOutTime.Text = sqlReader.GetValue(15)
                            hdnRoomId.Value = sqlReader.GetValue(11)
                            btnSave.Text = "Guardar"
                        End While


                    End If

                    sqlReader.Close()

                End Using

                Using sqlComm2 As New SqlCommand("GetBillHistoryByGuestIDRoomID")
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
                            btnSave.Text = "Guardar"
                            gvBill.DataSource = billTbl.Tables(0)
                            gvBill.DataBind()
                            'ddlHours.SelectedValue = ConvertingNumbers(billTbl.Tables(0).Rows(0)(10).ToString)
                            'ddlPromotion.SelectedValue = ddlPromotion.Items.FindByText(billTbl.Tables(0).Rows(0)(6).ToString).Value
                            'ddlPayType.SelectedValue = ddlPayType.Items.FindByText(billTbl.Tables(0).Rows(0)(3).ToString).Value
                            'gvBill.HeaderRow.Cells(8).Visible = False
                            'gvBill.HeaderRow.Cells(9).Visible = False
                            'gvBill.HeaderRow.Cells(10).Visible = False
                            'gvBill.HeaderRow.Cells(11).Visible = False

                            'gvBill.HeaderRow.Cells(12).Visible = False
                            'gvBill.HeaderRow.Cells(13).Visible = False
                            'gvBill.HeaderRow.Cells(14).Visible = False
                            'gvBill.HeaderRow.Cells(15).Visible = False
                            'gvBill.HeaderRow.Cells(16).Visible = False
                            'gvBill.HeaderRow.Cells(17).Visible = False
                            'gvBill.HeaderRow.Cells(18).Visible = False
                            'gvBill.HeaderRow.Cells(19).Visible = False
                            'gvBill.HeaderRow.Cells(20).Visible = False
                            'gvBill.HeaderRow.Cells(21).Visible = False
                            'gvBill.HeaderRow.Cells(22).Visible = False
                            'gvBill.HeaderRow.Cells(23).Visible = False
                            'gvBill.HeaderRow.Cells(24).Visible = False
                            ''gvBill.FooterRow.Cells(8).Visible = False
                            ''gvBill.FooterRow.Cells(9).Visible = False
                            ''gvBill.FooterRow.Cells(10).Visible = False
                            ''gvBill.FooterRow.Cells(11).Visible = False
                            'gvBill.FooterRow.Cells(12).Visible = False
                            'gvBill.FooterRow.Cells(13).Visible = False
                            'gvBill.FooterRow.Cells(14).Visible = False
                            'gvBill.FooterRow.Cells(15).Visible = False
                            'gvBill.FooterRow.Cells(16).Visible = False
                            'gvBill.FooterRow.Cells(17).Visible = False
                            'gvBill.FooterRow.Cells(18).Visible = False
                            'gvBill.FooterRow.Cells(19).Visible = False
                            'gvBill.FooterRow.Cells(20).Visible = False
                            'gvBill.FooterRow.Cells(21).Visible = False
                            'gvBill.FooterRow.Cells(22).Visible = False
                            'gvBill.FooterRow.Cells(23).Visible = False
                            'gvBill.FooterRow.Cells(24).Visible = False
                            'For i As Integer = 0 To gvBill.Rows.Count - 1
                            '    'gvBill.Rows(i).Cells(8).Visible = False
                            '    'gvBill.Rows(i).Cells(9).Visible = False
                            '    'gvBill.Rows(i).Cells(10).Visible = False
                            '    'gvBill.Rows(i).Cells(11).Visible = False
                            '    gvBill.Rows(i).Cells(12).Visible = False
                            '    gvBill.Rows(i).Cells(13).Visible = False
                            '    gvBill.Rows(i).Cells(14).Visible = False
                            '    gvBill.Rows(i).Cells(15).Visible = False
                            '    gvBill.Rows(i).Cells(16).Visible = False
                            '    gvBill.Rows(i).Cells(17).Visible = False
                            '    gvBill.Rows(i).Cells(18).Visible = False
                            '    gvBill.Rows(i).Cells(19).Visible = False
                            '    gvBill.Rows(i).Cells(20).Visible = False
                            '    gvBill.Rows(i).Cells(21).Visible = False
                            '    gvBill.Rows(i).Cells(22).Visible = False
                            '    gvBill.Rows(i).Cells(23).Visible = False
                            '    gvBill.Rows(i).Cells(24).Visible = False
                            'Next

                            Dim total As Decimal = billTbl.Tables(0).AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("Payment"))
                            gvBill.FooterRow.Cells(10).Text = "Total"
                            gvBill.FooterRow.Cells(10).HorizontalAlign = HorizontalAlign.Right
                            gvBill.FooterRow.Cells(11).Text = "$" + total.ToString("N2")
                        End If
                    End If
                End Using
                Using cmd2 As New SqlCommand("GetFirstLastCheckInByGuest")
                    cmd2.CommandType = CommandType.StoredProcedure

                    cmd2.Parameters.AddWithValue("@Guest_ID", hdnGuestId.Value)
                    cmd2.Connection = con
                    Dim sqlReader2 As SqlDataReader = cmd2.ExecuteReader()

                    If sqlReader2.HasRows Then

                        While (sqlReader2.Read())
                            If sqlReader2.GetValue(0) IsNot Nothing And IsDBNull(sqlReader2.GetValue(0)) = False Then

                                firstVisitDate.Text = sqlReader2.GetValue(0)
                            End If
                            If sqlReader2.GetValue(1) IsNot Nothing And IsDBNull(sqlReader2.GetValue(1)) = False Then

                                lastVisitDate.Text = sqlReader2.GetValue(1)

                            End If



                        End While
                    End If


                    con.Close()
                End Using


            Else
                Response.Redirect("GuestInformation")
            End If
        End Using
    End Sub

    Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then
                Dim item As String = e.Row.Cells(0).Text
                Dim itemIdNo As String = e.Row.Cells(2).Text
                Dim itemRoomNo As String = e.Row.Cells(5).Text

                Dim itemType As String = e.Row.Cells(11).Text

                If itemType = "Ventas" Or (itemIdNo = "&nbsp;" And itemRoomNo = "&nbsp;") Then

                    For Each editbutton As Button In e.Row.Cells(0).Controls.OfType(Of Button)()
                        editbutton.Visible = False
                    Next

                    For Each delbutton As Button In e.Row.Cells(1).Controls.OfType(Of Button)()
                        delbutton.Visible = False
                    Next

                End If
                For Each button As Button In e.Row.Cells(1).Controls.OfType(Of Button)()
                    If button.CommandName = "Delete" Then
                        button.Attributes("onclick") = "if(!confirm('¿Estás seguro de que quieres eliminar la habitación?')){ return false; };"
                    End If
                Next
            End If

    End Sub


    Protected Sub gvBill_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvBill.SelectedIndexChanged

        Dim row As GridViewRow = gvBill.SelectedRow
        Dim roomId As Int32
        Dim statusId As Int32
        Dim Rate As String()
        Dim cardNumber As String
        Dim room_id As HiddenField = TryCast(row.FindControl("Room_ID"), HiddenField)

        If row IsNot Nothing Then
            'datehour
            'room
            Rate = row.Cells(6).Text.Split(" ")

            cardNumber = (TryCast(row.FindControl("CardNumber"), Label)).Text
            ' roomId = ConvertingNumbers(row.Cells(12).Text)

            statusId = ExecuteSqlScalerQuery("select Status_ID from RoomDetails  where ROOM_ID=" & room_id.Value)

            If room_id.Value > 0 Then
                If (statusId = 2 Or statusId = 3) Then
                    Response.Redirect("CheckInEdit.aspx?room_id=" & room_id.Value & "&rate=" & Rate(0) & "&idno=" & cardNumber)
                ElseIf (statusId = 1 Or statusId = 4) Then
                    Response.Redirect("check-out.aspx?room_id=" & room_id.Value & "&rate=" & Rate(0) & "&idno=" & cardNumber)
                End If
            End If


        End If
    End Sub

    Protected Sub OnRowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim index As Integer = Convert.ToInt32(e.RowIndex)
        Dim roomId As Int32
        Dim IdNo As String
        Dim rentId As Int32
        IdNo = gvBill.Rows(e.RowIndex).Cells(2).Text
        roomId = ConvertingNumbers(gvBill.Rows(e.RowIndex).Cells(13).Text)
        rentId = ConvertingNumbers(gvBill.Rows(e.RowIndex).Cells(14).Text)
        ' ExecuteSqlQuery("UPDATE    RoomDetails   SET    Occupied='N',  GUEST_ID =  0,Status_ID=1  WHERE   (ROOM_ID = " & ConvertingNumbers(roomId) & ")")

        'ExecuteSqlQuery("delete from BillDetails where    (ROOM_ID = " & ConvertingNumbers(roomId) & ") and (ID_No=" & IdNo & ")")

        Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString

            Using con As New SqlConnection(constr)
                Using sqlComm2 As New SqlCommand("DeleteBillInfoByRoomID_IDNo")
                    sqlComm2.CommandType = CommandType.StoredProcedure
                    sqlComm2.Parameters.AddWithValue("@RoomID", ConvertingNumbers(roomId))
                    sqlComm2.Parameters.AddWithValue("@ID_No", IdNo)
                    sqlComm2.Parameters.AddWithValue("@RentID", rentId)
                    sqlComm2.Connection = con
                    con.Open()
                    Dim billId As Int32 = Convert.ToInt32(sqlComm2.ExecuteScalar())
                End Using
            End Using

        Session("FlushMessage") = "Room deleted successfully."
    End Sub


    <WebMethod()>
    Public Shared Function GetGuests(prefix As String) As String()
        Dim customers As New List(Of String)()
        Using conn As New SqlConnection()
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
            Using cmd As New SqlCommand()
                cmd.CommandText = "select top(50) GuestName from GuestInformation where GuestName like '%' + @SearchText + '%'"
                cmd.Parameters.AddWithValue("@SearchText", prefix)
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        customers.Add(sdr("GuestName"))
                    End While
                End Using
                conn.Close()
            End Using
        End Using
        Return customers.ToArray()
    End Function
    <WebMethod()>
    Public Shared Function GetGuestsIds(prefix As String) As Object()
        Dim blankcustomers As New List(Of String)()
        'Dim customers As New List(Of String)()
        Using conn As New SqlConnection()
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
            If prefix.Length >= 3 Then
                Using cmd As New SqlCommand()
                    cmd.CommandText = "select top(50) GEST_ID from GuestInformation where GEST_ID like '" & prefix & "%'"
                    'cmd.Parameters.AddWithValue("@SearchText", prefix)
                    cmd.Connection = conn
                    conn.Open()
                    Dim billTbl As DataSet = New DataSet
                    Dim sqlDA As New SqlDataAdapter(cmd)
                    sqlDA.Fill(billTbl)
                    Dim customers = (From rw As DataRow In billTbl.Tables(0).Rows Where Not String.IsNullOrEmpty(rw.Item("GEST_ID")) Select rw.Item("GEST_ID")).ToArray()
                    'Dim rowAsString = String.Join(", ", billTbl.Tables(0).Rows(0).ItemArray())
                    If customers.Count > 0 Then
                        Return customers
                    End If
                    'While sdr.Read()
                    '    customers.Add(sdr("GEST_ID"))
                    'End While
                End Using
                conn.Close()

            End If


        End Using
        Return blankcustomers.ToArray()

    End Function

    <WebMethod()>
    Public Shared Function CheckGuests(GuestId As String) As Integer
        Dim isexist As New Integer

        Using conn As New SqlConnection()
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
            If GuestId.Length >= 3 Then
                Using cmd As New SqlCommand()
                    cmd.CommandText = "select count(GEST_ID) as existval from GuestInformation where GEST_ID = @SearchText"
                    cmd.Parameters.AddWithValue("@SearchText", GuestId)
                    cmd.Connection = conn
                    conn.Open()
                    Using sdr As SqlDataReader = cmd.ExecuteReader()
                        While sdr.Read()
                            isexist = (sdr("existval"))
                        End While
                    End Using
                    conn.Close()
                End Using
            End If

        End Using
        Return isexist
    End Function

    <WebMethod()>
    Public Shared Function CheckGuestIDNo(IdNO As String) As Integer
        Dim isexist As New Integer

        Using conn As New SqlConnection()
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
            Using cmd As New SqlCommand()
                cmd.CommandText = "select count(ID_NO) as existval from BillDetails where ID_NO = @SearchText"
                cmd.Parameters.AddWithValue("@SearchText", IdNO)
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        isexist = (sdr("existval"))
                        If isexist > 0 Then
                            isexist = 1
                        End If
                    End While
                End Using
                conn.Close()
            End Using
        End Using
        Return isexist
    End Function

    <WebMethod()>
    Public Shared Function CountGuestByDat(sDat As String) As Integer
        Dim tlGust As New Integer
        Dim regdate As Date = DateTime.ParseExact(sDat, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)

        Using conn As New SqlConnection()
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
            Using cmd As New SqlCommand()
                cmd.CommandText = "select count(*) as totalGuest from GuestInformation where Convert(date,Register_Date) >= @SearchText"
                cmd.Parameters.AddWithValue("@SearchText", regdate)
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        tlGust = (sdr("totalGuest"))
                    End While
                End Using
                conn.Close()
            End Using
        End Using
        Return tlGust
    End Function

    <WebMethod()>
    Public Shared Function updateMaidComment(maidId As Integer, maidComments As String, guestId As Integer) As Integer
        ExecuteSqlQuery("UPDATE GuestInformation SET  USER_ID=" & maidId & ",DirtyRoom = 'N',Arrival_n_Departure_Date_Time='" & maidComments & "'  WHERE  (GUEST_ID = " & ConvertingNumbers(guestId) & ") ")
        Return 1
    End Function

    Protected Sub txtGuestID_TextChanged(sender As Object, e As EventArgs) Handles txtGuestID.TextChanged

        If txtGuestID.Text IsNot "" Then

            If txtGuestID.Text.Length >= 3 Then
                errGuestId.Text = ""
                errGuestId.CssClass = ""

                Using conn As New SqlConnection()
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
                    Using cmd As New SqlCommand()
                        cmd.CommandText = "SELECT        GUEST_ID, GEST_ID, GuestName, Address, PhoneNo, Social_Type,  ID_Type, Gender, Purpose, Arrival_n_Departure_Date_Time, Note, Check_In_Date, Check_In_Time, Check_Out_Date, Check_Out_Time from      GuestInformation where GEST_ID = @SearchText"
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


                                btnSave.Text = "Guardar"
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
                            CheckBoxList1.SelectedValue = 0
                            ddlIDType.SelectedValue = 0
                            ddlGender.SelectedValue = 0
                            txtPurpose.Text = ""
                            txtArrivalDeparture.Text = ""
                            txtNote.Value = ""
                        End If

                        sqlReader.Close()
                        conn.Close()
                    End Using

                    Using cmd2 As New SqlCommand("GetFirstLastCheckInByGuest")
                        cmd2.CommandType = CommandType.StoredProcedure

                        cmd2.Parameters.AddWithValue("@Guest_ID", hdnGuestId.Value)
                        cmd2.Connection = conn
                        conn.Open()
                        Dim sqlReader2 As SqlDataReader = cmd2.ExecuteReader()

                        If sqlReader2.HasRows Then

                            While (sqlReader2.Read())
                                If sqlReader2.GetValue(0) IsNot Nothing And IsDBNull(sqlReader2.GetValue(0)) = False Then

                                    firstVisitDate.Text = sqlReader2.GetValue(0)
                                End If
                                If sqlReader2.GetValue(1) IsNot Nothing And IsDBNull(sqlReader2.GetValue(1)) = False Then

                                    lastVisitDate.Text = sqlReader2.GetValue(1)

                                End If

                            End While
                        End If


                        conn.Close()
                    End Using

                End Using
            End If
        End If
    End Sub

    Protected Sub txtGuestName_TextChanged(sender As Object, e As EventArgs) Handles txtGuestName.TextChanged
        If txtGuestName.Text IsNot "" Then
            If txtGuestName.Text.Length >= 3 Then
                errGuest.Text = ""
                errGuest.CssClass = ""
                Using conn As New SqlConnection()
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
                    Using cmd As New SqlCommand()
                        cmd.CommandText = "SELECT         GUEST_ID, GEST_ID, GuestName, Address, PhoneNo, Social_Type,  ID_Type, Gender, Purpose, Arrival_n_Departure_Date_Time, Note, Check_In_Date, Check_In_Time, Check_Out_Date, Check_Out_Time from      GuestInformation where GuestName = '" & txtGuestName.Text & "'"
                        'cmd.Parameters.AddWithValue("@SearchText", txtGuestName.Text)
                        cmd.Connection = conn
                        conn.Open()
                        Dim sqlReader As SqlDataReader = cmd.ExecuteReader()

                        If sqlReader.HasRows Then

                            While (sqlReader.Read())
                                hdnGuestId.Value = sqlReader.GetValue(0)
                                txtGuestID.Text = sqlReader.GetValue(1)
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


                                btnSave.Text = "Guardar"
                                'txtCheckInDate.Text = sqlReader.GetValue(13) + " " + sqlReader.GetValue(14)
                                'txtCheckInTime.Text = sqlReader.GetValue(14)
                                'txtCheckOutDate.Text = sqlReader.GetValue(15) + " " + sqlReader.GetValue(16)
                                'txtCheckOutTime.Text = sqlReader.GetValue(16)
                            End While
                        Else
                            'hdnGuestId.Value = 0
                            'txtGuestName.Text = ""
                            'txtAddress.Text = ""
                            'txtPhoneNo.Text = ""
                            'CheckBoxList1.SelectedValue = 0
                            'ddlIDType.SelectedValue = 0
                            'ddlGender.SelectedValue = 0
                            'txtPurpose.Text = ""
                            'txtArrivalDeparture.Text = ""
                            'txtNote.Value = ""

                        End If

                        sqlReader.Close()
                        conn.Close()
                    End Using

                    Using cmd2 As New SqlCommand("GetFirstLastCheckInByGuest")
                        cmd2.CommandType = CommandType.StoredProcedure

                        cmd2.Parameters.AddWithValue("@Guest_ID", hdnGuestId.Value)
                        cmd2.Connection = conn
                        Dim sqlReader2 As SqlDataReader = cmd2.ExecuteReader()

                        If sqlReader2.HasRows Then

                            While (sqlReader2.Read())
                                firstVisitDate.Text = sqlReader2.GetValue(0)
                                lastVisitDate.Text = sqlReader2.GetValue(1)



                            End While
                        End If


                        conn.Close()
                    End Using

                End Using
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If txtGuestID.Text = "" Then
            errGuestId.Text = "ID del Huésped es requerido"
            txtGuestID.Focus()
            errGuestId.CssClass = "label label-danger"
            Return
        Else
            errGuestId.Text = ""
            errGuestId.CssClass = ""
        End If
        If txtGuestName.Text = "" Then
            errGuest.Text = "Nombre del huésped es requerido"
            txtGuestName.Focus()
            errGuest.CssClass = "label label-danger"
            Return
        Else
            errGuest.Text = ""
            errGuest.CssClass = ""
        End If

        If txtAddress.Text = "" Then
            errAddress.Text = "Nacionalidad es requeridad"
            txtAddress.Focus()
            errAddress.CssClass = "label label-danger"
            Return
        Else
            errAddress.Text = ""
            errAddress.CssClass = ""
        End If
        If IsNumeric(hdnGuestId.Value) Then
            If ConvertingNumbers(hdnGuestId.Value) > 0 Then
                Using con As New SqlConnection(CnString)
                    Using cmd2 As New SqlCommand("UPDATE  GuestInformation SET  GuestName=@GuestName, Address=@Address, PhoneNo=@PhoneNo, ID_Type=@ID_Type, Gender=@Gender, Purpose=@Purpose, Arrival_n_Departure_Date_Time=@Arrival_n_Departure_Date_Time, Note=@Note,  Social_Type=@Social_Type,USER_ID=@USER_ID where GUEST_ID=@GUEST_ID")

                        cmd2.Parameters.AddWithValue("@GUEST_ID", hdnGuestId.Value)
                        cmd2.Parameters.AddWithValue("@GuestName", txtGuestName.Text)
                        cmd2.Parameters.AddWithValue("@Address", txtAddress.Text)
                        cmd2.Parameters.AddWithValue("@PhoneNo", txtPhoneNo.Text)
                        cmd2.Parameters.AddWithValue("@ID_Type", ddlIDType.SelectedValue)
                        'cmd2.Parameters.AddWithValue("@ID_No", txtIDNo.Text)   ID_No=@ID_No,
                        cmd2.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue)
                        cmd2.Parameters.AddWithValue("@Purpose", txtPurpose.Text)
                        cmd2.Parameters.AddWithValue("@Arrival_n_Departure_Date_Time", txtArrivalDeparture.Text)
                        cmd2.Parameters.AddWithValue("@Note", txtNote.Value)
                        If IsNumeric(CheckBoxList1.SelectedValue) Then

                            cmd2.Parameters.AddWithValue("@Social_Type", CheckBoxList1.Items(CheckBoxList1.SelectedIndex).Text)
                        Else

                            cmd2.Parameters.AddWithValue("@Social_Type", "")
                        End If

                        cmd2.Parameters.AddWithValue("@USER_ID", ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)))


                        cmd2.Connection = con
                        con.Open()
                        cmd2.ExecuteNonQuery()
                        Session("FlushMessage") = "Data has been updated successfully."
                        con.Close()
                    End Using
                End Using
            Else
                Using con As New SqlConnection(CnString)
                    Using cmd As New SqlCommand("INSERT INTO  GuestInformation (GEST_ID,GuestName, Address, PhoneNo, ID_Type,  Gender, Purpose, Arrival_n_Departure_Date_Time, Note, Social_Type,USER_ID,Register_Date) Values (@GEST_ID,@GuestName, @Address, @PhoneNo, @ID_Type,  @Gender, @Purpose, @Arrival_n_Departure_Date_Time, @Note, @Social_Type,@USER_ID,@Register_Date) SELECT SCOPE_IDENTITY()")
                        cmd.Parameters.AddWithValue("@GEST_ID", txtGuestID.Text)
                        cmd.Parameters.AddWithValue("@GuestName", txtGuestName.Text)
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text)
                        cmd.Parameters.AddWithValue("@PhoneNo", txtPhoneNo.Text)
                        cmd.Parameters.AddWithValue("@ID_Type", ddlIDType.SelectedValue)
                        'cmd.Parameters.AddWithValue("@ID_No", txtIDNo.Text) , 
                        cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue)
                        cmd.Parameters.AddWithValue("@Purpose", txtPurpose.Text)
                        cmd.Parameters.AddWithValue("@Arrival_n_Departure_Date_Time", txtArrivalDeparture.Text)
                        cmd.Parameters.AddWithValue("@Note", txtNote.Value)
                        If IsNumeric(CheckBoxList1.SelectedValue) Then

                            cmd.Parameters.AddWithValue("@Social_Type", CheckBoxList1.Items(CheckBoxList1.SelectedIndex).Text)
                        Else

                            cmd.Parameters.AddWithValue("@Social_Type", "")
                        End If

                        cmd.Parameters.AddWithValue("@USER_ID", ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)))
                        cmd.Parameters.AddWithValue("@Register_Date", Now.ToString("yyyy-MM-dd HH:mm:ss"))
                        '" & ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)) & ",'" & Now & "'
                        cmd.Connection = con
                        con.Open()
                        Dim GUEST_ID As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                        Session("FlushMessage") = "Data has been saved successfully."
                        con.Close()

                    End Using
                End Using
            End If
        End If
        Response.Redirect("GuestInformation")
    End Sub

    Protected Sub txtAddress_TextChanged(sender As Object, e As EventArgs) Handles txtAddress.TextChanged
        If txtAddress.Text IsNot "" Then
            errAddress.Text = ""
            errAddress.CssClass = ""

        End If
    End Sub
End Class