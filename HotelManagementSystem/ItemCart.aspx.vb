Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.Services
Imports System.IO

Public Class ItemCart
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then

            If Not Me.Page.User.Identity.IsAuthenticated Then
                FormsAuthentication.RedirectToLoginPage()
            End If
            Dim currentPageFileName As String = New FileInfo(Me.Request.Url.LocalPath).Name
            Try
                CheckPermissions(currentPageFileName.Split(".aspx")(0).ToString, ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)))
            Catch ex As Exception

            End Try

            If IsNumeric(Request.QueryString("bill")) Then
                txtBillNo.Text = ConvertingNumbers(Request.QueryString("bill"))
                Dim dt_bill As DataTable = QueryDataTable(" SELECT        FoodServices.BILL_NO, FoodServices.BILLING_DATE, GuestInformation.GuestName, FoodServices.GUEST_ID, FoodServices.PAYMENT, FoodServices.DISCOUNT
                                                            FROM     FoodServices LEFT OUTER JOIN GuestInformation ON FoodServices.GUEST_ID = GuestInformation.GUEST_ID  WHERE    (FoodServices.BILL_NO = " & ConvertingNumbers(txtBillNo.Text) & ") ")
                If dt_bill.Rows.Count > 0 Then
                    txtBillDate.Text = Format(dt_bill.Rows(0)("BILLING_DATE"), "dd-MMM-yyyy")
                    txtGuestID.Text = dt_bill.Rows(0)("GUEST_ID").ToString()
                    txtGuestName.Text = dt_bill.Rows(0)("GuestName").ToString()
                    txtDiscount.Text = dt_bill.Rows(0)("DISCOUNT").ToString()
                    txtPayment.Text = dt_bill.Rows(0)("PAYMENT").ToString()
                End If

            Else
                Response.Redirect("FoodServices.aspx")
            End If


            Dim dt_tax As DataTable = QueryDataTable("SELECT * FROM TAXInfo")
            For Each chk_row As DataRow In dt_tax.Rows
                lblTAX1.Text = chk_row("TAX_Name_1").ToString() & " - " & chk_row("Rate_1").ToString() & " %"
                lblTAX2.Text = chk_row("TAX_Name_2").ToString() & " - " & chk_row("Rate_2").ToString() & " %"
                lblTAX3.Text = chk_row("TAX_Name_3").ToString() & " - " & chk_row("Rate_3").ToString() & " %"
            Next


            ddlFoodItem.DataSource = FetchDataFromTable("SELECT   FOOD_ITEM_ID, FOOD_ITEM_NAME  FROM     FoodItem")
            ddlFoodItem.DataTextField = "FOOD_ITEM_NAME"
            ddlFoodItem.DataValueField = "FOOD_ITEM_ID"
            ddlFoodItem.DataBind()
            ddlFoodItem.Items.Insert(0, New ListItem("Please select"))

            Me.BindDummyRow()

        End If
    End Sub
    Private Sub BindDummyRow()
        Dim dummy As New DataTable()
        dummy.Columns.Add("ROW_ID")
        dummy.Columns.Add("FOOD_ITEM_NAME")
        dummy.Columns.Add("Quantity")
        dummy.Columns.Add("UnitPrice")
        dummy.Columns.Add("TotalPrice")
        dummy.Rows.Add()
        gvItemCart.DataSource = dummy
        gvItemCart.DataBind()
    End Sub

    <WebMethod()>
    Public Shared Function GetSoldCartItem(BILL_NO As Double) As String
        BILL_CALCULATOR(BILL_NO)
        Dim query As String = " SELECT        FoodServiceDetails.ROW_ID, FoodItem.FOOD_ITEM_NAME, FoodServiceDetails.Quantity, FoodServiceDetails.UnitPrice, FoodServiceDetails.TotalPrice, FoodServiceDetails.BILL_NO
                               FROM            FoodServiceDetails LEFT OUTER JOIN FoodItem ON FoodServiceDetails.FOOD_ITEM_ID = FoodItem.FOOD_ITEM_ID  WHERE        (FoodServiceDetails.BILL_NO = " & ConvertingNumbers(BILL_NO) & ") "
        Dim dt_Item_all As DataTable = QueryDataTable(query)
        If dt_Item_all.Rows.Count > 0 Then
            Return FetchDataFromTable(query).GetXml()
        Else
            Return False
        End If
    End Function

    <WebMethod()>
    Public Shared Sub DeleteItem(ROW_ID As Double)
        Dim xDt_Bill As DataTable = QueryDataTable("Select  *  FROM     FoodServiceDetails   WHERE    (ROW_ID = " & ConvertingNumbers(ROW_ID) & ") ")
        If xDt_Bill.Rows.Count > 0 Then
            xBILL_NO = xDt_Bill.Rows(0)("BILL_NO")
        Else
            xBILL_NO = 0
        End If

        Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("DELETE FROM FoodServiceDetails WHERE ROW_ID = @ROW_ID")
                cmd.Parameters.AddWithValue("@ROW_ID", ROW_ID)
                cmd.Connection = con
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                BILL_CALCULATOR(xBILL_NO)
            End Using
        End Using

    End Sub

    <WebMethod()>
    Public Shared Function InsertItemAddCart(BILL_NO As Double, FOOD_ITEM_ID As Double, Quantity As Double) As String
        Dim FOOD_ITEM_PRICE As Double
        Dim dt_bill As DataTable = QueryDataTable(" SELECT *  FROM    FoodItem  WHERE  (FOOD_ITEM_ID = " & FOOD_ITEM_ID & ") ")
        If dt_bill.Rows.Count > 0 Then
            FOOD_ITEM_PRICE = dt_bill.Rows(0)("FOOD_ITEM_PRICE").ToString()
        End If
        Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("INSERT INTO FoodServiceDetails (FOOD_ITEM_ID, Quantity, BILL_NO, TotalPrice, UnitPrice) VALUES (@FOOD_ITEM_ID, @Quantity, @BILL_NO, @TotalPrice, @UnitPrice) SELECT SCOPE_IDENTITY()")
                cmd.Parameters.AddWithValue("@FOOD_ITEM_ID", FOOD_ITEM_ID)
                cmd.Parameters.AddWithValue("@Quantity", Quantity)
                cmd.Parameters.AddWithValue("@BILL_NO", BILL_NO)
                cmd.Parameters.AddWithValue("@TotalPrice", (Quantity * FOOD_ITEM_PRICE))
                cmd.Parameters.AddWithValue("@UnitPrice", FOOD_ITEM_PRICE)
                cmd.Connection = con
                con.Open()
                Dim ROW_ID As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                con.Close()
                BILL_CALCULATOR(BILL_NO)
                Dim query As String = " SELECT        FoodServiceDetails.ROW_ID, FoodItem.FOOD_ITEM_NAME, FoodServiceDetails.Quantity, FoodServiceDetails.UnitPrice, FoodServiceDetails.TotalPrice, FoodServiceDetails.BILL_NO
                                       FROM            FoodServiceDetails LEFT OUTER JOIN FoodItem ON FoodServiceDetails.FOOD_ITEM_ID = FoodItem.FOOD_ITEM_ID  WHERE        (FoodServiceDetails.ROW_ID = " & ConvertingNumbers(ROW_ID) & ") "
                Return FetchDataFromTable(query).GetXml()
            End Using
        End Using
    End Function
    <WebMethod()>
    Public Shared Function TransactionDetails(BILL_NO As Double) As String
        Dim query As String = " SELECT  *  FROM   FoodServices   WHERE   (BILL_NO = " & ConvertingNumbers(BILL_NO) & ") "
        Return FetchDataFromTable(query).GetXml()
    End Function
    <WebMethod()>
    Public Shared Function SaveItemCart(BILL_NO As Double, GUEST_ID As Double, DISCOUNT As Double, PAYMENT As Double) As String
        ExecuteSqlQuery(" UPDATE FoodServices   SET DISCOUNT=" & DISCOUNT & ", PAYMENT=" & PAYMENT & ", GUEST_ID=" & GUEST_ID & "  WHERE    (BILL_NO = " & BILL_NO & ") ")
        BILL_CALCULATOR(BILL_NO)
        Return 0
    End Function

End Class