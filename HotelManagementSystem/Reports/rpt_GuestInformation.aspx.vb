Imports QRCoder
Imports System.IO
Imports System.Drawing

Public Class rpt_GuestInformation
    Inherits System.Web.UI.Page
    Dim tax1, tax2, tax3 As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Not Me.Page.User.Identity.IsAuthenticated Then
                FormsAuthentication.RedirectToLoginPage()
            End If

            If IsNumeric(Request.QueryString("id")) Then
                HiddenField1.Value = ConvertingNumbers(Request.QueryString("id"))

                Dim qrCodeData As String = " "

                Dim dt_gstid As DataTable = QueryDataTable(" SELECT   GUEST_ID, GuestName, Address, PhoneNo  FROM     GuestInformation WHERE   (GUEST_ID = " & ConvertingNumbers(Request.QueryString("id")) & ") ")
                For Each gsv_row As DataRow In dt_gstid.Rows
                    qrCodeData = "INV# : " & gsv_row("GUEST_ID").ToString & ",  Date : " & gsv_row("GuestName").ToString & ", Customer Name : " & gsv_row("Address").ToString & ", Address : " & gsv_row("PhoneNo").ToString
                Next

                Dim dt_tax As DataTable = QueryDataTable("SELECT * FROM TAXInfo")
                If dt_tax.Rows.Count > 0 Then
                    tax1 = dt_tax.Rows(0)("TAX_Name_1").ToString() & " - " & dt_tax.Rows(0)("Rate_1") & " %"
                    tax2 = dt_tax.Rows(0)("TAX_Name_2").ToString() & " - " & dt_tax.Rows(0)("Rate_2") & " %"
                    tax3 = dt_tax.Rows(0)("TAX_Name_3").ToString() & " - " & dt_tax.Rows(0)("Rate_3") & " %"
                Else
                    tax1 = "TAX 1"
                    tax2 = "TAX 2"
                    tax3 = "TAX 3"
                End If




                Dim qrGenerator As New QRCodeGenerator()
                Dim qrCode As QRCodeGenerator.QRCode = qrGenerator.CreateQrCode(qrCodeData, QRCodeGenerator.ECCLevel.Q)
                Dim imgBarCode As New System.Web.UI.WebControls.Image()
                imgBarCode.Height = 150
                imgBarCode.Width = 150
                Using bitMap As Bitmap = qrCode.GetGraphic(20)
                    Using ms As New MemoryStream()
                        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
                        Dim byteImage As Byte() = ms.ToArray()
                        imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage)
                    End Using
                    phQrCode.Controls.Add(imgBarCode)
                End Using
            Else
                Response.Redirect("~/DataNotFound.aspx")
            End If
        End If
    End Sub
    Private Sub DetailsView1_DataBound(sender As Object, e As EventArgs) Handles DetailsView1.DataBound
        DetailsView1.Rows(37).Cells(0).Text = tax1
        DetailsView1.Rows(38).Cells(0).Text = tax2
        DetailsView1.Rows(39).Cells(0).Text = tax3
    End Sub
End Class