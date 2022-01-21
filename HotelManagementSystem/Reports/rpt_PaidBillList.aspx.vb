Imports System.Globalization

Public Class rpt_PaidBillList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Not Me.Page.User.Identity.IsAuthenticated Then
                FormsAuthentication.RedirectToLoginPage()
            End If

            Try
                Dim Date_From As Date = DateTime.ParseExact(Request.QueryString("dFrom").ToString(), "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
                Dim Date_To As Date = DateTime.ParseExact(Request.QueryString("dTo").ToString(), "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
                hfDateFrom.Value = Date_From
                hfDateTo.Value = Date_To
                lblReportType.Text = "Paid Bill List, From :" & Format(Date_From, "dd-MMM-yyy").ToString() & ", To : " & Format(Date_To, "dd-MMM-yyy").ToString()
            Catch ex As Exception
                Response.Redirect("~/DataNotFound.aspx")
            End Try

        End If
    End Sub

End Class