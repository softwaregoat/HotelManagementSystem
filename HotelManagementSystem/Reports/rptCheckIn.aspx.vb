Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO

Public Class rptCheckIn
    Inherits System.Web.UI.Page
    Public billTbl As DataSet = New DataSet
    Public dtvw As DataView = New DataView
    Public Date_From As DateTime
    Public lastexception As Exception
    Public Date_To As DateTime

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Not Me.Page.User.Identity.IsAuthenticated Then
                FormsAuthentication.RedirectToLoginPage()
            End If
            Dim currentPageFileName As String = New FileInfo(Me.Request.Url.LocalPath).Name
            Try
                If Session("LOGGED_USER_ID") Is Nothing Then
                    Response.Redirect("Default")
                End If
                'CheckPermissions(currentPageFileName.Split(".aspx")(0).ToString, ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)))
                'Date_From = DateTime.ParseExact(Request.QueryString("dFrom").ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
                'Date_To = DateTime.ParseExact(Request.QueryString("dTo").ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)

                Dim _originFromDate = DateTime.ParseExact(Request.QueryString("dFrom").ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)
                Dim _originToDate = DateTime.ParseExact(Request.QueryString("dTo").ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)





                Dim originFromDate = Convert.ToDateTime(_originFromDate).ToString("yyyy-MM-dd HH:mm:ss")
                Dim originToDate = Convert.ToDateTime(_originToDate).ToString("yyyy-MM-dd HH:mm:ss")



                Dim Rep_User As String = Request.QueryString("cUser").ToString()
                Dim uid As String = Request.QueryString("uid").ToString()
                Dim rptType As String = Request.QueryString("rptType").ToString()
                Dim gType As String = Request.QueryString("gType").ToString()

                hfDateFrom.Value = originFromDate
                hfDateTo.Value = originToDate
                hfUser.Value = uid.ToString
                hrptType.Value = rptType.ToString
                hgType.Value = gType.ToString
                lblReportType.Text = "Desde:" & originFromDate & " hasta: " & originToDate & ""
                lblUser.Text = "Recepcionista:" + Rep_User
                If Rep_User.Contains("Please select") Then
                    lblUser.Text = ""
                End If

                If hrptType.Value = "1" Then
                    CheckInReportBind(hgType.Value)
                ElseIf hrptType.Value = "2" Then
                    DirtyReportBind()
                ElseIf hrptType.Value = "3" Then
                    GuestReportBind()
                End If
            Catch ex As Exception
                lastexception = ex
            End Try
        End If
    End Sub

    Private Sub CheckInReportBind(ByVal GuestType As String)
        gvReport.DataSource = Nothing
        gvReport.DataBind()
        gvReport.ShowFooter = True
        'Dim _originFromDate = DateTime.ParseExact(Request.QueryString("dFrom").ToString(), "dd/MM/yyyy HH:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None)
        'Dim _originToDate = DateTime.ParseExact(Request.QueryString("dTo").ToString(), "dd/MM/yyyy HH:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None)





        'Dim originFromDate = Convert.ToDateTime(_originFromDate).ToString("yyyy-MM-dd HH:mm:ss:fff")
        'Dim originToDate = Convert.ToDateTime(_originToDate).ToString("yyyy-MM-dd HH:mm:ss:fff")


        Dim _originFromDate = DateTime.ParseExact(Request.QueryString("dFrom").ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)
        Dim _originToDate = DateTime.ParseExact(Request.QueryString("dTo").ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)


        Dim originFromDate = Convert.ToDateTime(_originFromDate).ToString("yyyy-MM-dd HH:mm:ss:fff")
        Dim originToDate = Convert.ToDateTime(_originToDate).ToString("yyyy-MM-dd HH:mm:ss:fff")

        Dim From_Date As Date = DateTime.ParseExact(originFromDate, "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture, DateTimeStyles.None)
        Dim To_Date As Date = DateTime.ParseExact(originToDate, "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture, DateTimeStyles.None)





        Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
        If IsNumeric(hfUser.Value) Then
            Using con As New SqlConnection(constr)
                Using sqlComm2 As New SqlCommand("GetCheckInReportExport")
                    sqlComm2.CommandType = CommandType.StoredProcedure
                    sqlComm2.Parameters.AddWithValue("@FromDate", originFromDate)
                    sqlComm2.Parameters.AddWithValue("@ToDate", originToDate)
                    sqlComm2.Parameters.AddWithValue("@USER_ID", hfUser.Value)
                    sqlComm2.Parameters.AddWithValue("@Guest_Type", GuestType)
                    sqlComm2.Connection = con
                    Dim sqlDA As New SqlDataAdapter(sqlComm2)
                    Dim sqlCB As New SqlCommandBuilder(sqlDA)
                    sqlDA.Fill(billTbl)
                    If billTbl IsNot Nothing Then
                        If billTbl.Tables(0).Rows.Count > 0 Then
                            billTbl.Tables(0).Columns("ID No.").ColumnName = "Número de Tarjeta"
                            billTbl.Tables(0).Columns("Date").ColumnName = "Fecha"
                            billTbl.Tables(0).Columns("Time").ColumnName = "Hora"
                            billTbl.Tables(0).Columns("Room").ColumnName = "Hab"
                            billTbl.Tables(0).Columns("GuestName").ColumnName = "Huésped"
                            billTbl.Tables(0).Columns("Rate").ColumnName = "Horas"
                            dtvw = billTbl.Tables(0).DefaultView
                            'If GuestType <> "ALL" Then
                            '    If GuestType Is "Normal" Then
                            '        dtvw.RowFilter = "Guest_Type='" & GuestType & "' OR Guest_Type=''"
                            '    Else
                            '        dtvw.RowFilter = "Guest_Type='" & GuestType & "'"
                            '    End If
                            'End If
                            lblReportName.Text = "Reporte de Habitaciones"





                            gvReport.DataSource = dtvw.ToTable()
                            gvReport.DataBind()
                            gvReport.HeaderRow.Cells(10).Visible = False
                            gvReport.HeaderRow.Cells(11).Visible = False
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
                            'gvReport.HeaderRow.Cells(24).Visible = False
                            'gvReport.HeaderRow.Cells(25).Visible = False

                            gvReport.FooterRow.Cells(10).Visible = False
                            gvReport.FooterRow.Cells(11).Visible = False
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
                            'gvReport.FooterRow.Cells(24).Visible = False
                            'gvReport.FooterRow.Cells(25).Visible = False
                            For i As Integer = 0 To gvReport.Rows.Count - 1

                                gvReport.Rows(i).Cells(10).Visible = False
                                gvReport.Rows(i).Cells(11).Visible = False
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
                                'gvReport.Rows(i).Cells(24).Visible = False
                                'gvReport.Rows(i).Cells(25).Visible = False
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
                            gvReport.FooterRow.Cells(6).Text = "Total"
                            gvReport.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                            gvReport.FooterRow.Cells(7).Text = "$" + total.ToString("N2")
                            'gvReport.FooterRow.Cells(12).HorizontalAlign = HorizontalAlign.Right
                            ' gvReport.FooterRow.Cells(12).Text = "$" + Visatotal.ToString("N2")
                            gvReport.FooterRow.Cells(8).HorizontalAlign = HorizontalAlign.Right
                            gvReport.FooterRow.Cells(8).Text = "$" + Efectivototal.ToString("N2")
                            gvReport.FooterRow.Cells(9).HorizontalAlign = HorizontalAlign.Right
                            gvReport.FooterRow.Cells(9).Text = "$" + MasterCardtotal.ToString("N2")
                            gvReport.ShowFooter = True
                        End If
                    End If
                End Using
            End Using
        End If
    End Sub

    Private Sub DirtyReportBind()
        gvReport.DataSource = Nothing
        gvReport.DataBind()



        Dim _originFromDate = DateTime.ParseExact(Request.QueryString("dFrom").ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)
        Dim _originToDate = DateTime.ParseExact(Request.QueryString("dTo").ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)





        Dim originFromDate = Convert.ToDateTime(_originFromDate).ToString("yyyy-MM-dd HH:mm:ss:fff")
        Dim originToDate = Convert.ToDateTime(_originToDate).ToString("yyyy-MM-dd HH:mm:ss:fff")










        Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
        If IsNumeric(hfUser.Value) Then
            Using con As New SqlConnection(constr)
                Using sqlComm2 As New SqlCommand("GetDirtyRoomReport")
                    sqlComm2.CommandType = CommandType.StoredProcedure
                    sqlComm2.Parameters.AddWithValue("@FromDate", originFromDate)
                    sqlComm2.Parameters.AddWithValue("@ToDate", originToDate)
                    sqlComm2.Parameters.AddWithValue("@USER_ID", hfUser.Value)
                    sqlComm2.Connection = con
                    Dim sqlDA As New SqlDataAdapter(sqlComm2)
                    Dim sqlCB As New SqlCommandBuilder(sqlDA)
                    sqlDA.Fill(billTbl)
                    If billTbl IsNot Nothing Then
                        If billTbl.Tables(0).Rows.Count > 0 Then
                            dtvw = billTbl.Tables(0).DefaultView
                            lblReportName.Text = "Dirty Report"
                            gvReport.DataSource = dtvw.ToTable()
                            gvReport.DataBind()
                            gvReport.HeaderRow.Cells(5).Visible = False
                            gvReport.HeaderRow.Cells(6).Visible = False
                            gvReport.HeaderRow.Cells(7).Visible = False


                            gvReport.FooterRow.Cells(5).Visible = False
                            gvReport.FooterRow.Cells(6).Visible = False
                            gvReport.FooterRow.Cells(7).Visible = False
                            For i As Integer = 0 To gvReport.Rows.Count - 1


                                gvReport.Rows(i).Cells(5).Visible = False
                                gvReport.Rows(i).Cells(6).Visible = False
                                gvReport.Rows(i).Cells(7).Visible = False
                            Next
                            gvReport.ShowFooter = False
                        End If
                    End If
                End Using
            End Using
        End If
    End Sub

    Private Sub GuestReportBind()
        gvReport.DataSource = Nothing
        gvReport.DataBind()


        Dim _originFromDate = DateTime.ParseExact(Request.QueryString("dFrom").ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)
        Dim _originToDate = DateTime.ParseExact(Request.QueryString("dTo").ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)





        Dim originFromDate = Convert.ToDateTime(_originFromDate).ToString("yyyy-MM-dd HH:mm:ss:fff")
        Dim originToDate = Convert.ToDateTime(_originToDate).ToString("yyyy-MM-dd HH:mm:ss:fff")


        Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
        If IsNumeric(hfUser.Value) Then
            Using con As New SqlConnection(constr)
                Using sqlComm2 As New SqlCommand("GetGuestReport")
                    sqlComm2.CommandType = CommandType.StoredProcedure
                    sqlComm2.Parameters.AddWithValue("@FromDate", originFromDate)
                    sqlComm2.Parameters.AddWithValue("@ToDate", originToDate)
                    sqlComm2.Parameters.AddWithValue("@USER_ID", hfUser.Value)
                    sqlComm2.Connection = con

                    Dim sqlDA As New SqlDataAdapter(sqlComm2)
                    Dim sqlCB As New SqlCommandBuilder(sqlDA)
                    sqlDA.Fill(billTbl)
                    If billTbl IsNot Nothing Then
                        If billTbl.Tables(0).Rows.Count > 0 Then
                            dtvw = billTbl.Tables(0).DefaultView
                            lblReportName.Text = "Guest Report"
                            gvReport.DataSource = dtvw.ToTable()
                            gvReport.DataBind()

                            gvReport.HeaderRow.Cells(9).Visible = False
                            gvReport.HeaderRow.Cells(10).Visible = False
                            gvReport.HeaderRow.Cells(11).Visible = False
                            gvReport.HeaderRow.Cells(12).Visible = False
                            gvReport.HeaderRow.Cells(13).Visible = False
                            gvReport.HeaderRow.Cells(14).Visible = False
                            gvReport.HeaderRow.Cells(15).Visible = False
                            gvReport.HeaderRow.Cells(16).Visible = False
                            gvReport.HeaderRow.Cells(17).Visible = False
                            gvReport.HeaderRow.Cells(18).Visible = False
                            'gvReport.HeaderRow.Cells(19).Visible = False
                            'gvReport.HeaderRow.Cells(20).Visible = False


                            gvReport.FooterRow.Cells(9).Visible = False
                            gvReport.FooterRow.Cells(10).Visible = False
                            gvReport.FooterRow.Cells(11).Visible = False
                            gvReport.FooterRow.Cells(12).Visible = False
                            gvReport.FooterRow.Cells(13).Visible = False
                            gvReport.FooterRow.Cells(14).Visible = False
                            gvReport.FooterRow.Cells(15).Visible = False
                            gvReport.FooterRow.Cells(16).Visible = False
                            gvReport.FooterRow.Cells(17).Visible = False
                            gvReport.FooterRow.Cells(18).Visible = False
                            'gvReport.FooterRow.Cells(19).Visible = False
                            'gvReport.FooterRow.Cells(20).Visible = False
                            For i As Integer = 0 To gvReport.Rows.Count - 1



                                gvReport.Rows(i).Cells(9).Visible = False
                                gvReport.Rows(i).Cells(10).Visible = False
                                gvReport.Rows(i).Cells(11).Visible = False
                                gvReport.Rows(i).Cells(12).Visible = False
                                gvReport.Rows(i).Cells(13).Visible = False
                                gvReport.Rows(i).Cells(14).Visible = False
                                gvReport.Rows(i).Cells(15).Visible = False
                                gvReport.Rows(i).Cells(16).Visible = False
                                gvReport.Rows(i).Cells(17).Visible = False
                                gvReport.Rows(i).Cells(18).Visible = False
                                'gvReport.Rows(i).Cells(19).Visible = False
                                'gvReport.Rows(i).Cells(20).Visible = False
                            Next
                            gvReport.ShowFooter = False
                        End If
                    End If
                End Using
            End Using
        End If
    End Sub
End Class