Imports System.Data.SqlClient
Imports System.IO

Public Class AddFoodItem
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

            If Request.QueryString("act") IsNot Nothing Then
                If Request.QueryString("act") = "edit" Then
                    '---------------------------------------
                    If IsNumeric(Request.QueryString("id")) Then
                        btnSubmit.Text = "Update"
                        hfldID.Value = ConvertingNumbers(Request.QueryString("id"))
                        Dim dt_cust As DataTable = QueryDataTable("SELECT * FROM FoodItem WHERE FOOD_ITEM_ID= " & ConvertingNumbers(hfldID.Value) & " ")
                        For Each Item_ROW As DataRow In dt_cust.Rows
                            txtItemName.Text = Item_ROW("FOOD_ITEM_NAME").ToString()
                            txtPrice.Text = Item_ROW("FOOD_ITEM_PRICE").ToString()
                        Next
                    Else
                        Response.Redirect("AddFoodItem.aspx")
                    End If
                    '---------------------------------------
                ElseIf Request.QueryString("act") = "del" Then
                    '---------------------------------------
                    If IsNumeric(Request.QueryString("id")) Then
                        hfldID.Value = ConvertingNumbers(Request.QueryString("id"))
                        QueryDataTable("DELETE FoodItem WHERE FOOD_ITEM_ID= " & ConvertingNumbers(hfldID.Value) & " ")
                        Session("FlushMessage") = "Data has been Deleted successfully."
                        Response.Redirect("AddFoodItem.aspx")
                    Else
                        Response.Redirect("AddFoodItem.aspx")
                    End If
                    '---------------------------------------
                Else
                    Response.Redirect("AddFoodItem.aspx")
                End If
            End If

            sqlStr = " SELECT   *   FROM  FoodItem"
            GridView1.DataSource = FetchDataFromTable(sqlStr)
            GridView1.DataBind()

            If (Not (Session("FlushMessage")) Is Nothing) Then
                lblMessege.Text = Session("FlushMessage").ToString()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "showModal();", True)
                Session("FlushMessage") = Nothing
            End If

        End If
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If btnSubmit.Text = "Submit" Then
            Using con As New SqlConnection(CnString)
                Using cmd As New SqlCommand("INSERT INTO  FoodItem (FOOD_ITEM_NAME, FOOD_ITEM_PRICE) VALUES (@FOOD_ITEM_NAME, @FOOD_ITEM_PRICE)")
                    cmd.Parameters.AddWithValue("@FOOD_ITEM_NAME", txtItemName.Text)
                    cmd.Parameters.AddWithValue("@FOOD_ITEM_PRICE", ConvertingNumbers(txtPrice.Text))
                    cmd.Connection = con
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                End Using
            End Using
            Session("FlushMessage") = "Data has been saved successfully."
            Response.Redirect("AddFoodItem.aspx")
        ElseIf btnSubmit.Text = "Update" Then
            Using con As New SqlConnection(CnString)
                Using cmd As New SqlCommand("UPDATE   FoodItem SET FOOD_ITEM_NAME= @FOOD_ITEM_NAME, FOOD_ITEM_PRICE=@FOOD_ITEM_PRICE  WHERE  FOOD_ITEM_ID= @FOOD_ITEM_ID")
                    cmd.Parameters.AddWithValue("@FOOD_ITEM_ID", ConvertingNumbers(hfldID.Value))
                    cmd.Parameters.AddWithValue("@FOOD_ITEM_NAME", txtItemName.Text)
                    cmd.Parameters.AddWithValue("@FOOD_ITEM_PRICE", ConvertingNumbers(txtPrice.Text))
                    cmd.Connection = con
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                End Using
            End Using
            Session("FlushMessage") = "Data has been updated successfully."
            Response.Redirect("AddFoodItem.aspx")
        End If
    End Sub
End Class