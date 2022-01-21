Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO

Public Class ServiceReport
    Inherits System.Web.UI.Page
    Public billTbl As DataSet = New DataSet
    Public dtvw As DataView = New DataView
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Not Me.Page.User.Identity.IsAuthenticated Or Session("LOGGED_USER_ID") Is Nothing Then
                FormsAuthentication.RedirectToLoginPage()
            End If
            Dim currentPageFileName As String = New FileInfo(Me.Request.Url.LocalPath).Name
            Try

                CheckPermissions(currentPageFileName.Split(".aspx")(0).ToString, ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)))
                ddlNormalSpecial.AutoPostBack = True
                Dim RoleId As Integer = 0
                Dim UId As String
                UId = Decrypt(Session("LOGGED_USER_ID").ToString)
                ddlUsers.DataSource = FetchDataFromTable("SELECT  *,(FirstName +' '+ LastName) as FullName  FROM  UserInformation where ROLE_ID=2")
                ddlUsers.DataTextField = "FullName"
                ddlUsers.DataValueField = "USER_ID"
                ddlUsers.DataBind()
                ddlUsers.Items.Insert(0, New ListItem("Please select", 0))
                Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
                Using con As New SqlConnection(constr)
                    Using cmd As New SqlCommand("GetRoleName")
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@Username", UId)
                        cmd.Connection = con
                        con.Open()
                        RoleId = Convert.ToInt32(cmd.ExecuteScalar())
                    End Using
                    con.Close()
                End Using
                If RoleId = 3 Then
                    ddlUsers.SelectedValue = ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString))
                End If
            Catch ex As Exception

            End Try
            gvReport.ShowFooter = True
            'txtDateFrom.Text = Format(Now, "dd/MM/yyyy")
            'txtDateTo.Text = Format(Now, "dd/MM/yyyy")
        End If
    End Sub

    Protected Sub btnCheckInRpt_Click(sender As Object, e As EventArgs) Handles btnCheckInRpt.Click
        hdnreporttype.Value = "1"
        CheckInReportBind("ALL")

        reportTitle.Text = "Reporte de Habitaciones"
    End Sub

    Private Sub CheckInReportBind(ByVal GuestType As String)
        gvReport.DataSource = Nothing
        gvReport.DataBind()
        pnlFilter.Visible = True
        gvReport.ShowFooter = True
        Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString

        Dim _originFromDate = DateTime.ParseExact(txtDateFrom.Text, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)
        Dim _originToDate = DateTime.ParseExact(txtDateTo.Text, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)



        Dim originFromDate = Convert.ToDateTime(_originFromDate).ToString("yyyy-MM-dd HH:mm:ss:fff")
        Dim originToDate = Convert.ToDateTime(_originToDate).ToString("yyyy-MM-dd HH:mm:ss:fff")

        Dim From_Date As Date = DateTime.ParseExact(originFromDate, "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture, DateTimeStyles.None)
        Dim To_Date As Date = DateTime.ParseExact(originToDate, "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture, DateTimeStyles.None)
        'If IsNumeric(ddlUsers.SelectedValue) Then
        Using con As New SqlConnection(constr)
            Using sqlComm2 As New SqlCommand("GetCheckInReportExport")
                sqlComm2.CommandType = CommandType.StoredProcedure
                sqlComm2.Parameters.AddWithValue("@FromDate", originFromDate)
                sqlComm2.Parameters.AddWithValue("@ToDate", originToDate)
                sqlComm2.Parameters.AddWithValue("@USER_ID", ddlUsers.SelectedValue)
                sqlComm2.Parameters.AddWithValue("@Guest_Type", GuestType)
                sqlComm2.Connection = con
                Dim sqlDA As New SqlDataAdapter(sqlComm2)
                Dim sqlCB As New SqlCommandBuilder(sqlDA)
                sqlDA.Fill(billTbl)
                If billTbl Is Nothing Or billTbl.Tables.Count = 0 Then
                    Console.WriteLine("Dataset is null")
                Else
                    If billTbl.Tables(0).Rows.Count > 0 Then
                        pnlExport.Visible = True
                        billTbl.Tables(0).Columns("ID No.").ColumnName = "Número de Tarjeta"
                        billTbl.Tables(0).Columns("Date").ColumnName = "Fecha"
                        billTbl.Tables(0).Columns("Time").ColumnName = "Hora"
                        billTbl.Tables(0).Columns("Room").ColumnName = "Hab"
                        billTbl.Tables(0).Columns("GuestName").ColumnName = "Huésped"
                        billTbl.Tables(0).Columns("Rate").ColumnName = "Horas"
                        dtvw = billTbl.Tables(0).DefaultView
                        'If GuestType IsNot "ALL" Then
                        '    If GuestType Is "Normal" Then
                        '        dtvw.RowFilter = "Guest_Type='" & GuestType & "' OR Guest_Type=''"
                        '    Else
                        '        dtvw.RowFilter = "Guest_Type='" & GuestType & "'"
                        '    End If
                        'End If

                        gvReport.DataSource = dtvw.ToTable()
                        gvReport.DataBind()
                        'gvReport.HeaderRow.Cells(11).Visible = False
                        gvReport.HeaderRow.Cells(12).Visible = False
                        gvReport.HeaderRow.Cells(13).Visible = False
                        gvReport.HeaderRow.Cells(14).Visible = False
                        gvReport.HeaderRow.Cells(15).Visible = False
                        gvReport.HeaderRow.Cells(16).Visible = False
                        gvReport.HeaderRow.Cells(17).Visible = False
                        gvReport.HeaderRow.Cells(18).Visible = False
                        gvReport.HeaderRow.Cells(19).Visible = False
                        gvReport.HeaderRow.Cells(20).Visible = False
                        gvReport.HeaderRow.Cells(21).Visible = False
                        gvReport.HeaderRow.Cells(22).Visible = False
                        gvReport.HeaderRow.Cells(23).Visible = False
                        gvReport.HeaderRow.Cells(24).Visible = False
                        gvReport.HeaderRow.Cells(25).Visible = False

                        'gvReport.FooterRow.Cells(11).Visible = False
                        gvReport.FooterRow.Cells(12).Visible = False
                        gvReport.FooterRow.Cells(13).Visible = False
                        gvReport.FooterRow.Cells(14).Visible = False
                        gvReport.FooterRow.Cells(15).Visible = False
                        gvReport.FooterRow.Cells(16).Visible = False
                        gvReport.FooterRow.Cells(17).Visible = False
                        gvReport.FooterRow.Cells(18).Visible = False
                        gvReport.FooterRow.Cells(19).Visible = False
                        gvReport.FooterRow.Cells(20).Visible = False
                        gvReport.FooterRow.Cells(21).Visible = False
                        gvReport.FooterRow.Cells(22).Visible = False
                        gvReport.FooterRow.Cells(23).Visible = False
                        gvReport.FooterRow.Cells(24).Visible = False
                        gvReport.FooterRow.Cells(25).Visible = False
                        For i As Integer = 0 To gvReport.Rows.Count - 1

                            'gvReport.Rows(i).Cells(11).Visible = False
                            gvReport.Rows(i).Cells(12).Visible = False
                            gvReport.Rows(i).Cells(13).Visible = False
                            gvReport.Rows(i).Cells(14).Visible = False
                            gvReport.Rows(i).Cells(15).Visible = False
                            gvReport.Rows(i).Cells(16).Visible = False
                            gvReport.Rows(i).Cells(17).Visible = False
                            gvReport.Rows(i).Cells(18).Visible = False
                            gvReport.Rows(i).Cells(19).Visible = False
                            gvReport.Rows(i).Cells(20).Visible = False
                            gvReport.Rows(i).Cells(21).Visible = False
                            gvReport.Rows(i).Cells(22).Visible = False
                            gvReport.Rows(i).Cells(23).Visible = False
                            gvReport.Rows(i).Cells(24).Visible = False
                            gvReport.Rows(i).Cells(25).Visible = False
                        Next


                        Dim total As Decimal = dtvw.ToTable().AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("Total"))
                        '  Dim Visatotal As Decimal = dtvw.ToTable().AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("Otro"))
                        Dim Efectivototal As Decimal = dtvw.ToTable().AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("Efectivo"))
                        Dim MasterCardtotal As Decimal = dtvw.ToTable().AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("Tarjeta"))



                        Dim dvsubtotNormal As DataView = dtvw.ToTable().DefaultView
                        Dim dvsubtotSpecial As DataView = dtvw.ToTable().DefaultView
                        Dim dvsubtotVentas As DataView = dtvw.ToTable().DefaultView
                        dvsubtotNormal.RowFilter = ("Promoción='Sub Total (Normal)'")
                        dvsubtotSpecial.RowFilter = ("Promoción='Sub Total (Especial)'")
                        dvsubtotVentas.RowFilter = ("Promoción='Sub Total (Ventas)'")


                        Dim sbVentasTotal As Decimal = 0.00
                        Dim sbVentasEffectivo As Decimal = 0.00
                        Dim sbVentasTarjeta As Decimal = 0.00

                        If dvsubtotVentas IsNot Nothing Then
                            If dvsubtotVentas.ToTable().Rows.Count > 0 Then
                                sbVentasTotal = dvsubtotVentas.ToTable().Rows(0).Field(Of Decimal)("Total")
                                sbVentasEffectivo = dvsubtotVentas.ToTable().Rows(0).Field(Of Decimal)("Efectivo")
                                sbVentasTarjeta = dvsubtotVentas.ToTable().Rows(0).Field(Of Decimal)("Tarjeta")
                            End If
                        End If





                        Dim sbNormalTotal As Decimal = 0.00
                        Dim sbNormalEffectivo As Decimal = 0.00
                        Dim sbNormalTarjeta As Decimal = 0.00

                        If dvsubtotNormal IsNot Nothing Then
                            If dvsubtotNormal.ToTable().Rows.Count > 0 Then
                                sbNormalTotal = dvsubtotNormal.ToTable().Rows(0).Field(Of Decimal)("Total")
                                sbNormalEffectivo = dvsubtotNormal.ToTable().Rows(0).Field(Of Decimal)("Efectivo")
                                sbNormalTarjeta = dvsubtotNormal.ToTable().Rows(0).Field(Of Decimal)("Tarjeta")
                            End If
                        End If


                        Dim sbSpecialTotal As Decimal = 0.00
                        Dim sbSpecialEffectivo As Decimal = 0.00
                        Dim sbSpecialTarjeta As Decimal = 0.00
                        If dvsubtotSpecial IsNot Nothing Then
                            If dvsubtotSpecial.ToTable().Rows.Count > 0 Then
                                sbSpecialTotal = dvsubtotSpecial.ToTable().Rows(0).Field(Of Decimal)("Total")
                                sbSpecialEffectivo = dvsubtotSpecial.ToTable().Rows(0).Field(Of Decimal)("Efectivo")
                                sbSpecialTarjeta = dvsubtotSpecial.ToTable().Rows(0).Field(Of Decimal)("Tarjeta")
                            End If
                        End If
                        total = total - (sbNormalTotal + sbSpecialTotal + sbVentasTotal)
                        Efectivototal = Efectivototal - (sbNormalEffectivo + sbSpecialEffectivo + sbVentasEffectivo)
                        MasterCardtotal = MasterCardtotal - (sbNormalTarjeta + sbSpecialTarjeta + sbVentasTarjeta)
                        gvReport.FooterRow.Cells(8).Text = "Total"
                        gvReport.FooterRow.Cells(9).HorizontalAlign = HorizontalAlign.Right
                                gvReport.FooterRow.Cells(9).Text = "$" + total.ToString("N2")
                                'gvReport.FooterRow.Cells(12).HorizontalAlign = HorizontalAlign.Right
                                ' gvReport.FooterRow.Cells(12).Text = "$" + Visatotal.ToString("N2")
                                gvReport.FooterRow.Cells(10).HorizontalAlign = HorizontalAlign.Right
                                gvReport.FooterRow.Cells(10).Text = "$" + Efectivototal.ToString("N2")
                                gvReport.FooterRow.Cells(11).HorizontalAlign = HorizontalAlign.Right
                                gvReport.FooterRow.Cells(11).Text = "$" + MasterCardtotal.ToString("N2")
                                gvReport.ShowFooter = True
                            End If
                        End If
            End Using
        End Using
        'End If
    End Sub
    Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs)

        If hdnreporttype.Value = "1" Then
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
        Else
            For Each editbutton As Button In e.Row.Cells(0).Controls.OfType(Of Button)()
                editbutton.Visible = False
            Next

            For Each delbutton As Button In e.Row.Cells(1).Controls.OfType(Of Button)()
                delbutton.Visible = False
            Next
        End If

    End Sub

    Protected Sub gvReport_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvReport.SelectedIndexChanged

        If hdnreporttype.Value = "1" Then
            Dim row As GridViewRow = gvReport.SelectedRow
            Dim roomId As Int32
            Dim statusId As Int32
            Dim IDNo As String
            Dim Rate As String()
            If row IsNot Nothing Then
                'datehour
                'room
                Rate = row.Cells(7).Text.Split(" ")
                IDNo = row.Cells(2).Text
                roomId = ConvertingNumbers(row.Cells(24).Text)

                statusId = ExecuteSqlScalerQuery("select Status_ID from RoomDetails  where ROOM_ID=" & roomId)

                If roomId > 0 Then
                    If (statusId = 2 Or statusId = 3) Then
                        Response.Redirect("CheckInEdit.aspx?room_id=" & roomId & "&rate=" & Rate(0) & "&idno=" & IDNo & "")
                    ElseIf (statusId = 1 Or statusId = 4) Then
                        Response.Redirect("check-out.aspx?room_id=" & roomId & "&rate=" & Rate(0) & "&idno=" & IDNo & "")
                    End If
                End If


                End If
            End If
    End Sub

    Protected Sub OnRowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim index As Integer = Convert.ToInt32(e.RowIndex)
        Dim roomId As Int32
        Dim IdNo As String
        Dim rentId As Int32
        If hdnreporttype.Value = "1" Then
            IdNo = gvReport.Rows(e.RowIndex).Cells(2).Text
            roomId = ConvertingNumbers(gvReport.Rows(e.RowIndex).Cells(24).Text)
            rentId = ConvertingNumbers(gvReport.Rows(e.RowIndex).Cells(25).Text)
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

            CheckInReportBind("ALL")
            Session("FlushMessage") = "Room deleted successfully."
        End If
    End Sub


    Protected Sub btnDirtyRpt_Click(sender As Object, e As EventArgs) Handles btnDirtyRpt.Click
        hdnreporttype.Value = "2"
        DirtyReportBind()
        reportTitle.Text = "Reporte de Habitaciones Sucias"
    End Sub

    Private Sub DirtyReportBind()
        gvReport.DataSource = Nothing
        gvReport.DataBind()
        pnlFilter.Visible = False
        Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
        Dim _originFromDate = DateTime.ParseExact(txtDateFrom.Text, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)
        Dim _originToDate = DateTime.ParseExact(txtDateTo.Text, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)



        Dim originFromDate = Convert.ToDateTime(_originFromDate).ToString("yyyy-MM-dd HH:mm:ss:fff")
        Dim originToDate = Convert.ToDateTime(_originToDate).ToString("yyyy-MM-dd HH:mm:ss:fff")

        Dim From_Date As Date = DateTime.ParseExact(originFromDate, "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture, DateTimeStyles.None)
        Dim To_Date As Date = DateTime.ParseExact(originToDate, "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture, DateTimeStyles.None)


        If IsNumeric(ddlUsers.SelectedValue) Then
            Using con As New SqlConnection(constr)
                Using sqlComm2 As New SqlCommand("GetDirtyRoomReport")
                    sqlComm2.CommandType = CommandType.StoredProcedure
                    sqlComm2.Parameters.AddWithValue("@FromDate", originFromDate)
                    sqlComm2.Parameters.AddWithValue("@ToDate", originToDate)
                    sqlComm2.Parameters.AddWithValue("@USER_ID", ddlUsers.SelectedValue)
                    sqlComm2.Connection = con
                    Dim sqlDA As New SqlDataAdapter(sqlComm2)
                    Dim sqlCB As New SqlCommandBuilder(sqlDA)
                    sqlDA.Fill(billTbl)
                    If billTbl IsNot Nothing Then
                        If billTbl.Tables(0).Rows.Count > 0 Then
                            pnlExport.Visible = True
                            dtvw = billTbl.Tables(0).DefaultView
                            gvReport.DataSource = dtvw.ToTable()
                            gvReport.DataBind()

                            gvReport.HeaderRow.Cells(0).Visible = False
                            gvReport.HeaderRow.Cells(1).Visible = False
                            'gvReport.HeaderRow.Cells(5).Visible = False
                            'gvReport.HeaderRow.Cells(6).Visible = False
                            gvReport.HeaderRow.Cells(7).Visible = False
                            gvReport.HeaderRow.Cells(8).Visible = False
                            gvReport.HeaderRow.Cells(9).Visible = False

                            gvReport.FooterRow.Cells(0).Visible = False
                            gvReport.FooterRow.Cells(1).Visible = False
                            'gvReport.FooterRow.Cells(5).Visible = False
                            'gvReport.FooterRow.Cells(6).Visible = False
                            gvReport.FooterRow.Cells(7).Visible = False
                            gvReport.FooterRow.Cells(8).Visible = False
                            gvReport.FooterRow.Cells(9).Visible = False
                            For i As Integer = 0 To gvReport.Rows.Count - 1

                                gvReport.Rows(i).Cells(0).Visible = False
                                gvReport.Rows(i).Cells(1).Visible = False
                                'gvReport.Rows(i).Cells(5).Visible = False
                                'gvReport.Rows(i).Cells(6).Visible = False
                                gvReport.Rows(i).Cells(7).Visible = False
                                gvReport.Rows(i).Cells(8).Visible = False
                                gvReport.Rows(i).Cells(9).Visible = False
                            Next
                            gvReport.ShowFooter = False
                        End If
                    End If
                End Using
            End Using
        End If
    End Sub

    Protected Sub btnGuestRpt_Click(sender As Object, e As EventArgs) Handles btnGuestRpt.Click
        hdnreporttype.Value = "3"

        ddlNormalSpecial.Items.Clear()
        ddlNormalSpecial.Items.Insert(0, New ListItem("Todas", 0))
        ddlNormalSpecial.Items.Insert(1, New ListItem("Nuevos", 1))
        GuestReportBind()
        reportTitle.Text = "Reporte de Huéspedes"
    End Sub

    Private Sub GuestReportBind()


        gvReport.DataSource = Nothing
        gvReport.DataBind()
        pnlFilter.Visible = True
        Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString

        Dim _originFromDate = DateTime.ParseExact(txtDateFrom.Text, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)
        Dim _originToDate = DateTime.ParseExact(txtDateTo.Text, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)



        Dim originFromDate = Convert.ToDateTime(_originFromDate).ToString("yyyy-MM-dd HH:mm:ss:fff")
        Dim originToDate = Convert.ToDateTime(_originToDate).ToString("yyyy-MM-dd HH:mm:ss:fff")

        Dim From_Date As Date = DateTime.ParseExact(originFromDate, "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture, DateTimeStyles.None)
        Dim To_Date As Date = DateTime.ParseExact(originToDate, "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture, DateTimeStyles.None)




        'If IsNumeric(ddlUsers.SelectedValue) Then
        Using con As New SqlConnection(constr)
            Using sqlComm2 As New SqlCommand("GetGuestReport")
                If ddlNormalSpecial.SelectedValue = 1 Then
                    sqlComm2.CommandText = "GetGuestReport2"
                End If
                sqlComm2.CommandType = CommandType.StoredProcedure
                sqlComm2.Parameters.AddWithValue("@FromDate", originFromDate)
                sqlComm2.Parameters.AddWithValue("@ToDate", originToDate)
                sqlComm2.Parameters.AddWithValue("@USER_ID", ddlUsers.SelectedValue)
                sqlComm2.Connection = con

                Dim sqlDA As New SqlDataAdapter(sqlComm2)
                Dim sqlCB As New SqlCommandBuilder(sqlDA)
                sqlDA.Fill(billTbl)
                If billTbl IsNot Nothing Then
                    If billTbl.Tables(0).Rows.Count > 0 Then
                        pnlExport.Visible = True
                        dtvw = billTbl.Tables(0).DefaultView
                        'If ddlNormalSpecial.SelectedValue = 1 Then
                        '    dtvw.RowFilter = ("ReportType='Nuevos'")
                        'End If



                        gvReport.DataSource = dtvw.ToTable()
                        gvReport.DataBind()

                        gvReport.HeaderRow.Cells(0).Visible = False
                        gvReport.HeaderRow.Cells(1).Visible = False

                        'gvReport.HeaderRow.Cells(9).Visible = False
                        'gvReport.HeaderRow.Cells(10).Visible = False
                        'gvReport.HeaderRow.Cells(11).Visible = False
                        'gvReport.HeaderRow.Cells(12).Visible = False
                        'gvReport.HeaderRow.Cells(13).Visible = False
                        gvReport.HeaderRow.Cells(14).Visible = False
                        gvReport.HeaderRow.Cells(15).Visible = False
                        gvReport.HeaderRow.Cells(16).Visible = False
                        gvReport.HeaderRow.Cells(17).Visible = False
                        gvReport.HeaderRow.Cells(18).Visible = False
                        gvReport.HeaderRow.Cells(19).Visible = False
                        gvReport.HeaderRow.Cells(20).Visible = False
                        '    gvReport.HeaderRow.Cells(18).Visible = False
                        'gvReport.HeaderRow.Cells(19).Visible = False
                        'gvReport.HeaderRow.Cells(20).Visible = False
                        'gvReport.HeaderRow.Cells(21).Visible = False

                        gvReport.FooterRow.Cells(0).Visible = False
                        gvReport.FooterRow.Cells(1).Visible = False
                        'gvReport.FooterRow.Cells(9).Visible = False
                        'gvReport.FooterRow.Cells(10).Visible = False
                        'gvReport.FooterRow.Cells(11).Visible = False
                        'gvReport.FooterRow.Cells(12).Visible = False
                        'gvReport.FooterRow.Cells(13).Visible = False
                        gvReport.FooterRow.Cells(14).Visible = False
                        gvReport.FooterRow.Cells(15).Visible = False
                        gvReport.FooterRow.Cells(16).Visible = False
                        gvReport.FooterRow.Cells(17).Visible = False
                        gvReport.FooterRow.Cells(18).Visible = False
                        gvReport.FooterRow.Cells(19).Visible = False
                        gvReport.FooterRow.Cells(20).Visible = False
                        '    gvReport.FooterRow.Cells(18).Visible = False
                        'gvReport.FooterRow.Cells(19).Visible = False
                        'gvReport.FooterRow.Cells(20).Visible = False
                        'gvReport.FooterRow.Cells(21).Visible = False
                        For i As Integer = 0 To gvReport.Rows.Count - 1


                            gvReport.Rows(i).Cells(0).Visible = False
                            gvReport.Rows(i).Cells(1).Visible = False
                            'gvReport.Rows(i).Cells(9).Visible = False
                            'gvReport.Rows(i).Cells(10).Visible = False
                            'gvReport.Rows(i).Cells(11).Visible = False
                            'gvReport.Rows(i).Cells(12).Visible = False
                            'gvReport.Rows(i).Cells(13).Visible = False
                            gvReport.Rows(i).Cells(14).Visible = False
                            gvReport.Rows(i).Cells(15).Visible = False
                            gvReport.Rows(i).Cells(16).Visible = False
                            gvReport.Rows(i).Cells(17).Visible = False

                            gvReport.Rows(i).Cells(18).Visible = False
                            gvReport.Rows(i).Cells(19).Visible = False
                            gvReport.Rows(i).Cells(20).Visible = False
                            'gvReport.Rows(i).Cells(21).Visible = False
                        Next
                        gvReport.ShowFooter = False
                    End If
                End If
            End Using
        End Using
        'End If
    End Sub

    Private Sub ExportGridToCSV()

        Dim rowremove As Integer
        Response.Clear()
        Response.Buffer = True
        Response.Charset = ""
        Response.ContentType = "text/csv"

        Dim sb As StringBuilder = New StringBuilder()
        If hdnreporttype.Value = "1" Then
            CheckInReportBind(ddlNormalSpecial.SelectedValue)
            Response.AddHeader("content-disposition", "attachment;filename=checkinreport.csv")
            rowremove = 10
        ElseIf hdnreporttype.Value = "2" Then
            DirtyReportBind()
            Response.AddHeader("content-disposition", "attachment;filename=dirtyreport.csv")
            rowremove = 5
        ElseIf hdnreporttype.Value = "3" Then
            GuestReportBind()
            Response.AddHeader("content-disposition", "attachment;filename=guestreport.csv")
            rowremove = 10
        End If

        Dim rownum As Integer
        rownum = dtvw.ToTable().Rows.Count
        Dim colnum As Integer
        colnum = dtvw.ToTable().Columns.Count
        Dim temptbl As DataTable = dtvw.ToTable().Copy
        Dim j As Integer = rowremove

        While j <= colnum - 1
            temptbl.Columns.RemoveAt(j)
            temptbl.AcceptChanges()
            colnum = colnum - 1
        End While

        gvReport.DataSource = temptbl
        gvReport.DataBind()

        'For Each gr As GridViewRow In gvReport.Rows
        '    Dim editbutton As Button = gr.Cells(0).Controls.OfType(Of Button)()
        '    editbutton.Visible = False
        '    Dim delbutton As Button = gr.Cells(1).Controls.OfType(Of Button)()
        '    delbutton.Visible = False
        'Next



        If hdnreporttype.Value = "1" Then



            Dim total As Decimal = dtvw.ToTable().AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("Total"))
            ' Dim Visatotal As Decimal = dtvw.ToTable().AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("Otro"))
            Dim Efectivototal As Decimal = dtvw.ToTable().AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("Efectivo"))
            Dim MasterCardtotal As Decimal = dtvw.ToTable().AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("Tarjeta"))











            Dim dvsubtotNormal As DataView = dtvw.ToTable().DefaultView
            Dim dvsubtotSpecial As DataView = dtvw.ToTable().DefaultView

            Dim dvsubtotVentas As DataView = dtvw.ToTable().DefaultView
            dvsubtotNormal.RowFilter = ("Promoción='Sub Total (Normal)'")
            dvsubtotSpecial.RowFilter = ("Promoción='Sub Total (Especial)'")
            dvsubtotVentas.RowFilter = ("Promoción='Sub Total (Ventas)'")

            Dim sbNormalTotal As Decimal = 0.00
            Dim sbNormalEffectivo As Decimal = 0.00
            Dim sbNormalTarjeta As Decimal = 0.00

            If dvsubtotNormal.ToTable().Rows.Count > 0 Then
                sbNormalTotal = dvsubtotNormal.ToTable().Rows(0).Field(Of Decimal)("Total")
                sbNormalEffectivo = dvsubtotNormal.ToTable().Rows(0).Field(Of Decimal)("Efectivo")
                sbNormalTarjeta = dvsubtotNormal.ToTable().Rows(0).Field(Of Decimal)("Tarjeta")
            End If




            Dim sbSpecialTotal As Decimal = 0.00
            Dim sbSpecialEffectivo As Decimal = 0.00
            Dim sbSpecialTarjeta As Decimal = 0.00

            If dvsubtotSpecial.ToTable().Rows.Count > 0 Then
                sbSpecialTotal = dvsubtotSpecial.ToTable().Rows(0).Field(Of Decimal)("Total")
                sbSpecialEffectivo = dvsubtotSpecial.ToTable().Rows(0).Field(Of Decimal)("Efectivo")
                sbSpecialTarjeta = dvsubtotSpecial.ToTable().Rows(0).Field(Of Decimal)("Tarjeta")

            End If


            Dim sbVentasTotal As Decimal = 0.00
            Dim sbVentasEffectivo As Decimal = 0.00
            Dim sbVentasTarjeta As Decimal = 0.00

            If dvsubtotVentas IsNot Nothing Then
                If dvsubtotVentas.ToTable().Rows.Count > 0 Then
                    sbVentasTotal = dvsubtotVentas.ToTable().Rows(0).Field(Of Decimal)("Total")
                    sbVentasEffectivo = dvsubtotVentas.ToTable().Rows(0).Field(Of Decimal)("Efectivo")
                    sbVentasTarjeta = dvsubtotVentas.ToTable().Rows(0).Field(Of Decimal)("Tarjeta")
                End If
            End If





            total = total - (sbNormalTotal + sbSpecialTotal + sbVentasTotal)
            Efectivototal = Efectivototal - (sbNormalEffectivo + sbSpecialEffectivo + sbVentasEffectivo)
            MasterCardtotal = MasterCardtotal - (sbNormalTarjeta + sbSpecialTarjeta + sbVentasTarjeta)


            gvReport.FooterRow.Cells(8).Text = "Total"
            gvReport.FooterRow.Cells(9).HorizontalAlign = HorizontalAlign.Right
            gvReport.FooterRow.Cells(9).Text = "$" + total.ToString("")
            'gvReport.FooterRow.Cells(8).HorizontalAlign = HorizontalAlign.Right
            'gvReport.FooterRow.Cells(8).Text = "$" + Visatotal.ToString("")
            gvReport.FooterRow.Cells(10).HorizontalAlign = HorizontalAlign.Right
            gvReport.FooterRow.Cells(10).Text = "$" + Efectivototal.ToString("")
            gvReport.FooterRow.Cells(11).HorizontalAlign = HorizontalAlign.Right
            gvReport.FooterRow.Cells(11).Text = "$" + MasterCardtotal.ToString("")
            gvReport.ShowFooter = True
            End If






            gvReport.AllowPaging = False
        Dim txt As String
        For Each cell As TableCell In gvReport.HeaderRow.Cells
            'Append data with separator.

            If cell.Text IsNot "" Then

                txt = cell.Text.Trim().Replace("&nbsp;", "").Replace("ú", "u").Replace("é", "e").Replace("ó", "o")
                sb.Append(txt & ",")
            End If

        Next

        'Append new line character.
        sb.Append(vbCr & vbLf)

        For Each row As GridViewRow In gvReport.Rows

            For Each cell As TableCell In row.Cells
                'Append data with separator.

                txt = cell.Text.Trim().Replace("&nbsp;", "").Replace("&#233;", "e")
                sb.Append(txt & ",")


            Next

            'Append new line character.
            sb.Append(vbCr & vbLf)
        Next

        If hdnreporttype.Value = "1" Then

            'Append new line character.
            sb.Append(vbCr & vbLf)
            For Each cell As TableCell In gvReport.FooterRow.Cells
                'Append data with separator.
                txt = cell.Text.Trim().Replace("&nbsp;", "")
                sb.Append(txt & ",")
            Next
        End If

        Response.Output.Write(sb.ToString())
        Response.Flush()
        Response.End()
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        ExportGridToCSV()
    End Sub

    Protected Sub ddlNormalSpecial_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlNormalSpecial.SelectedIndexChanged
        If hdnreporttype.Value = "3" Then
            GuestReportBind()
        Else

            CheckInReportBind(ddlNormalSpecial.SelectedValue)
        End If
    End Sub
End Class