Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO


Public Class rptCheckedIn
    Inherits System.Web.UI.Page
    Public billTbl As DataSet = New DataSet
    Public dtvw As DataView = New DataView
    Public Date_From As Date
    Public Date_To As Date

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
                'Dim Rep_User As String = Request.QueryString("cUser").ToString()
                'Dim uid As String = Request.QueryString("uid").ToString()
                Dim guestId As String = Request.QueryString("guestId").ToString()
                Dim roomId As String = Request.QueryString("roomId").ToString()

                Dim invName As String = Request.QueryString("invName").ToString()
                Dim invId As String = Request.QueryString("invId").ToString()
                Dim IsAsGuest As String = Request.QueryString("IsAsGuest").ToString()

                lblReportName.Text = invName
                lblInvId.Text = invId

                hdnGuestId.Value = guestId
                hdnRoomId.Value = roomId

                If IsAsGuest = "false" Then
                    lblGestId.Visible = True
                    lblGuestName.Visible = True
                End If
                'hfDateFrom.Value = Date_From
                'hfDateTo.Value = Date_To
                'hfUser.Value = uid.ToString
                'hrptType.Value = rptType.ToString
                'hgType.Value = gType.ToString
                'lblReportType.Text = "From :" & Format(Date_From, "dd-MMM-yyy").ToString() & ", To : " & Format(Date_To, "dd-MMM-yyy").ToString() & ""
                'lblUser.Text = Rep_User

                CheckInReportBind()

            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub CheckInReportBind()
        gvReport.DataSource = Nothing
        gvReport.DataBind()
        gvReport.ShowFooter = True
        Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
        Using con As New SqlConnection(constr)
            If IsNumeric(hdnGuestId.Value) Then


                Using sqlComm As New SqlCommand("GetGuestInfoByGuestID")
                    sqlComm.CommandType = CommandType.StoredProcedure
                    sqlComm.Parameters.AddWithValue("@GuestID", ConvertingNumbers(hdnGuestId.Value))
                    sqlComm.Connection = con
                    con.Open()
                    Dim sqlReader As SqlDataReader = sqlComm.ExecuteReader()

                    If sqlReader.HasRows Then

                        While (sqlReader.Read())
                            lblGestId.Text = sqlReader.GetValue(0)
                            hdnGuestId.Value = sqlReader.GetValue(1)
                            lblGuestName.Text = sqlReader.GetValue(2)
                            hdnRoomId.Value = sqlReader.GetValue(17)
                            lblTarjeta.Text = "Tarjeta No.: " + sqlReader.GetValue(8)
                            lblCheckInDateTime.Text = "Fecha de entrada: " + sqlReader.GetValue(13) + " " + sqlReader.GetValue(14)
                            lblCheckOutDateTime.Text = "Fecha de salida: " + sqlReader.GetValue(15) + " " + sqlReader.GetValue(16)
                            lblRoomNo.Text = " Habitación:" + sqlReader.GetValue(18)
                        End While


                    End If

                    sqlReader.Close()

                End Using

                Using sqlComm2 As New SqlCommand("rptGetBillInfoByGuestIDRoomID")
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

                            'Dim firstView As DataView
                            'Dim firstTbl As DataTable
                            'firstView = billTbl.Tables(0).DefaultView
                            'firstView.RowFilter = "ReportType!='Sales'"
                            'firstView.Sort = "Bill_ID desc"
                            'firstTbl = firstView.ToTable
                            'Dim SecondView As DataView
                            'Dim SecondTbl As DataTable
                            'SecondView = billTbl.Tables(0).DefaultView
                            'SecondView.RowFilter = "ReportType='Sales'"
                            'SecondView.Sort = "Date desc,Time desc"
                            'SecondTbl = SecondView.ToTable

                            'Dim finalDs As DataSet
                            'finalDs.Tables.Add(firstTbl)
                            'finalDs.Tables.Add(SecondTbl)








                            gvReport.DataSource = billTbl.Tables(0)
                            gvReport.DataBind()
                            'gvReport.HeaderRow.Cells(8).Visible = False
                            'gvReport.HeaderRow.Cells(9).Visible = False
                            'gvReport.HeaderRow.Cells(10).Visible = False
                            'gvReport.HeaderRow.Cells(11).Visible = False
                            'gvReport.HeaderRow.Cells(12).Visible = False
                            'gvReport.HeaderRow.Cells(13).Visible = False
                            'gvReport.HeaderRow.Cells(14).Visible = False
                            'gvReport.HeaderRow.Cells(15).Visible = False
                            'gvReport.HeaderRow.Cells(16).Visible = False
                            'gvReport.HeaderRow.Cells(17).Visible = False
                            'gvReport.HeaderRow.Cells(18).Visible = False
                            'gvReport.HeaderRow.Cells(19).Visible = False
                            'gvReport.HeaderRow.Cells(20).Visible = False
                            'gvReport.HeaderRow.Cells(21).Visible = False

                            'gvReport.FooterRow.Cells(8).Visible = False
                            'gvReport.FooterRow.Cells(9).Visible = False
                            'gvReport.FooterRow.Cells(10).Visible = False
                            'gvReport.FooterRow.Cells(11).Visible = False
                            'gvReport.FooterRow.Cells(12).Visible = False
                            'gvReport.FooterRow.Cells(13).Visible = False
                            'gvReport.FooterRow.Cells(14).Visible = False
                            'gvReport.FooterRow.Cells(15).Visible = False
                            'gvReport.FooterRow.Cells(16).Visible = False
                            'gvReport.FooterRow.Cells(17).Visible = False
                            'gvReport.FooterRow.Cells(18).Visible = False
                            'gvReport.FooterRow.Cells(19).Visible = False
                            'gvReport.FooterRow.Cells(20).Visible = False
                            'gvReport.FooterRow.Cells(21).Visible = False


                            'For i As Integer = 0 To gvReport.Rows.Count - 1
                            '    'gvReport.Rows(i).Cells(8).Visible = False
                            '    gvReport.Rows(i).Cells(3).Text = gvReport.Rows(i).Cells(18).Text
                            '    gvReport.Rows(i).Cells(9).Visible = False
                            '    gvReport.Rows(i).Cells(10).Visible = False
                            '    gvReport.Rows(i).Cells(11).Visible = False
                            '    gvReport.Rows(i).Cells(12).Visible = False
                            '    gvReport.Rows(i).Cells(13).Visible = False
                            '    gvReport.Rows(i).Cells(14).Visible = False
                            '    gvReport.Rows(i).Cells(15).Visible = False
                            '    gvReport.Rows(i).Cells(16).Visible = False
                            '    gvReport.Rows(i).Cells(17).Visible = False
                            '    gvReport.Rows(i).Cells(18).Visible = False
                            '    gvReport.Rows(i).Cells(19).Visible = False
                            '    gvReport.Rows(i).Cells(20).Visible = False
                            '    gvReport.Rows(i).Cells(21).Visible = False

                            'Next
                            gvReport.FooterRow.Visible = False

                            Dim total As Decimal = billTbl.Tables(0).AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("Saldo"))
                            gvReport.FooterRow.Cells(4).Text = "Total"
                            gvReport.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right
                            gvReport.FooterRow.Cells(5).Text = "$" + total.ToString("N2")
                        End If
                    End If
                End Using
            End If
        End Using

    End Sub




End Class