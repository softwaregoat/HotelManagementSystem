Public Class rpt_CheckOut
    Inherits System.Web.UI.Page
    Dim tax1, tax2, tax3 As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Not Me.Page.User.Identity.IsAuthenticated Then
                FormsAuthentication.RedirectToLoginPage()
            End If
            If IsNumeric(Request.QueryString("id")) Then
                HiddenField1.Value = ConvertingNumbers(Request.QueryString("id"))
            Else
                Response.Redirect("~/DataNotFound.aspx")
            End If

            Dim dt_cust As DataTable = QueryDataTable("SELECT * FROM TAXInfo")
            For Each chk_row As DataRow In dt_cust.Rows
                tax1 = chk_row("TAX_Name_1").ToString() & " - " & chk_row("Rate_1").ToString() & " %"
                tax2 = chk_row("TAX_Name_2").ToString() & " - " & chk_row("Rate_2").ToString() & " %"
                tax3 = chk_row("TAX_Name_3").ToString() & " - " & chk_row("Rate_3").ToString() & " %"
            Next

        End If
    End Sub

    Private Sub DetailsView1_DataBound(sender As Object, e As EventArgs) Handles DetailsView1.DataBound
        DetailsView1.Rows(30).Cells(0).Text = tax1
        DetailsView1.Rows(31).Cells(0).Text = tax2
        DetailsView1.Rows(32).Cells(0).Text = tax3
    End Sub
End Class