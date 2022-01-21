Imports System.Globalization

Public Class rpt_FoodServiceUnPaidSales
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Not Me.Page.User.Identity.IsAuthenticated Then
                FormsAuthentication.RedirectToLoginPage()
            End If

            Try
                Dim Date_From As Date = DateTime.ParseExact(Request.QueryString("dFrom").ToString(), "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
                Dim Date_To As Date = DateTime.ParseExact(Request.QueryString("dTo").ToString(), "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)

                lblReportType.Text = "UnPaid Sales List, From :" & Format(Date_From, "dd-MMM-yyy").ToString() & ", To : " & Format(Date_To, "dd-MMM-yyy").ToString()

                GridView1.DataSource = FetchDataFromTable(" SELECT        BILL_NO, BILLING_DATE, USER_ID, GUEST_ID, G_TOTAL, TAX_1, TAX_2, TAX_3, ITEM_COST, TOTAL_TAX, PAYABLE, PAYMENT, BALANCE, DISCOUNT FROM   FoodServices WHERE      (BALANCE > 0) AND ((BILLING_DATE>= '" + Date_From + "') AND (BILLING_DATE <= '" + Date_To + "'))")
                GridView1.DataBind()

                Dim dt_sum_check As DataTable = QueryDataTable("SELECT      *  FROM     FoodServices   WHERE      (BALANCE > 0) AND BILLING_DATE >= '" & Date_From & "' AND BILLING_DATE <= '" & Date_To & "' ")
                If dt_sum_check.Rows.Count > 0 Then
                    Dim dt_sum As DataTable = QueryDataTable(" SELECT   SUM(G_TOTAL) AS ExprG_TOTAL, SUM(TAX_1) AS ExprTAX_1, SUM(TAX_2) AS ExprTAX_2, SUM(TAX_3) AS ExprTAX_3, SUM(ITEM_COST) AS ExprITEM_COST, SUM(TOTAL_TAX) AS ExprTOTAL_TAX, SUM(PAYMENT) AS ExprPAYMENT, SUM(BALANCE) AS ExprBALANCE, SUM(DISCOUNT) 
                                  AS ExprDISCOUNT  FROM     FoodServices   WHERE      (BALANCE > 0) AND  BILLING_DATE >= '" + Date_From + "' AND BILLING_DATE <= '" + Date_To + "'  ")
                    GridView1.FooterRow.Cells(3).Text = Math.Round(dt_sum.Rows(0)("ExprITEM_COST"), 2)
                    GridView1.FooterRow.Cells(4).Text = Math.Round(dt_sum.Rows(0)("ExprTAX_1"), 2)
                    GridView1.FooterRow.Cells(5).Text = Math.Round(dt_sum.Rows(0)("ExprTAX_2"), 2)
                    GridView1.FooterRow.Cells(6).Text = Math.Round(dt_sum.Rows(0)("ExprTAX_3"), 2)
                    GridView1.FooterRow.Cells(7).Text = Math.Round(dt_sum.Rows(0)("ExprTOTAL_TAX"), 2)
                    GridView1.FooterRow.Cells(8).Text = Math.Round(dt_sum.Rows(0)("ExprG_TOTAL"), 2)
                    GridView1.FooterRow.Cells(9).Text = Math.Round(dt_sum.Rows(0)("ExprDISCOUNT"), 2)
                    GridView1.FooterRow.Cells(10).Text = Math.Round(dt_sum.Rows(0)("ExprPAYMENT"), 2)
                    GridView1.FooterRow.Cells(11).Text = Math.Round(dt_sum.Rows(0)("ExprBALANCE"), 2)
                End If

            Catch ex As Exception
                Response.Redirect("~/DataNotFound.aspx")
            End Try

        End If
    End Sub

End Class