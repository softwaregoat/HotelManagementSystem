Imports System.Data.SqlClient
Imports System.IO

Public Class Promotion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then

            If Not Me.Page.User.Identity.IsAuthenticated Or Session("LOGGED_USER_ID") Is Nothing Then
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
                        Dim dt_cust As DataTable = QueryDataTable("SELECT * FROM Promotions WHERE Promotion_ID= " & ConvertingNumbers(hfldID.Value) & " ")
                        Dim Ptypeval As Integer

                        For Each custrow As DataRow In dt_cust.Rows
                            If custrow("Promotion_Type").ToString() = "Extra" Then
                                Ptypeval = 1
                            Else
                                Ptypeval = 2
                            End If
                            txtPromotion.Text = custrow("Promotion").ToString()
                            ddlPromotionType.SelectedValue = Ptypeval
                            txtPromotionValue.Text = custrow("Promotion_Value").ToString()
                        Next
                    Else
                        Response.Redirect("Promotion.aspx")
                    End If
                    '---------------------------------------
                ElseIf Request.QueryString("act") = "del" Then
                    '---------------------------------------
                    If IsNumeric(Request.QueryString("id")) Then
                        hfldID.Value = ConvertingNumbers(Request.QueryString("id"))
                        QueryDataTable("DELETE Promotions WHERE Promotion_ID= " & ConvertingNumbers(hfldID.Value) & " ")
                        Session("FlushMessage") = "Data has been Deleted successfully."
                        Response.Redirect("Promotion.aspx")
                    Else
                        Response.Redirect("Promotion.aspx")
                    End If
                    '---------------------------------------
                Else
                    Response.Redirect("Promotion.aspx")
                End If
            End If

            Dim dt As DataTable = QueryDataTable("SELECT * FROM Promotions")
            For Each row As DataRow In dt.Rows
                GridView1.DataSource = dt
                GridView1.DataBind()
            Next

            If (Not (Session("FlushMessage")) Is Nothing) Then
                lblMessege.Text = Session("FlushMessage").ToString()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "showModal();", True)
                Session("FlushMessage") = Nothing
            End If

        End If
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If btnSubmit.Text = "Confirmar" Then
            Dim Ptype As String
            If ddlPromotionType.SelectedValue = 1 Then
                Ptype = "Extra"
            Else
                Ptype = "%"
            End If

            Using con As New SqlConnection(CnString)
                Using cmd As New SqlCommand("INSERT INTO  Promotions (Promotion,Promotion_Type,Promotion_Value) VALUES (@Promotion,@Promotion_Type,@Promotion_Value)")
                    cmd.Parameters.AddWithValue("@Promotion", txtPromotion.Text)
                    cmd.Parameters.AddWithValue("@Promotion_Type", Ptype)
                    cmd.Parameters.AddWithValue("@Promotion_Value", txtPromotionValue.Text)
                    cmd.Connection = con
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                End Using
            End Using
            Session("FlushMessage") = "Data has been saved successfully."
            Response.Redirect("Promotion.aspx")
        ElseIf btnSubmit.Text = "Update" Then
            Dim Ptype As String
            If ddlPromotionType.SelectedValue = 1 Then
                Ptype = "Extra"
            Else
                Ptype = "%"
            End If
            Using con As New SqlConnection(CnString)
                Using cmd As New SqlCommand("UPDATE   Promotions SET Promotion= @Promotion,Promotion_Type=@PromotionType,Promotion_Value=@PromotionValue  WHERE  Promotion_ID= @Promotion_ID")
                    cmd.Parameters.AddWithValue("@Promotion_ID", ConvertingNumbers(hfldID.Value))
                    cmd.Parameters.AddWithValue("@Promotion", txtPromotion.Text)
                    cmd.Parameters.AddWithValue("@PromotionType", Ptype)
                    cmd.Parameters.AddWithValue("@PromotionValue", txtPromotionValue.Text)
                    cmd.Connection = con
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                End Using
            End Using
            Session("FlushMessage") = "Data has been updated successfully."
            Response.Redirect("Promotion.aspx")
        End If
    End Sub
End Class