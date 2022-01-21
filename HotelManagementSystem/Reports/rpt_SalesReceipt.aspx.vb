Public Class rpt_SalesReceipt
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Not Me.Page.User.Identity.IsAuthenticated Then
                FormsAuthentication.RedirectToLoginPage()
            End If
            Dim tax1, tax2, tax3 As String

            Dim dt_cust As DataTable = QueryDataTable("SELECT * FROM TAXInfo")
            For Each chk_row As DataRow In dt_cust.Rows
                tax1 = chk_row("TAX_Name_1").ToString() & " - " & chk_row("Rate_1").ToString() & " % ,"
                tax2 = chk_row("TAX_Name_2").ToString() & " - " & chk_row("Rate_2").ToString() & " % ,"
                tax3 = chk_row("TAX_Name_3").ToString() & " - " & chk_row("Rate_3").ToString() & " % "
            Next

            lblTaxInfo.Text = tax1 & tax2 & tax3

            If IsNumeric(Request.QueryString("id")) Then
                HiddenField1.Value = ConvertingNumbers(Request.QueryString("id"))
            Else
                Response.Redirect("~/DataNotFound.aspx")
            End If
        End If
    End Sub

End Class