Imports System.Data.SqlClient
Imports System.IO

Public Class TAXInfo
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

            Dim dt As DataTable = QueryDataTable("SELECT * FROM TAXInfo")
            For Each row As DataRow In dt.Rows
                txtTAX_Name_1.Text = row("TAX_Name_1").ToString()
                txtTAX_Name_2.Text = row("TAX_Name_2").ToString()
                txtTAX_Name_3.Text = row("TAX_Name_3").ToString()
                txtRate_1.Text = row("Rate_1").ToString()
                txtRate_2.Text = row("Rate_2").ToString()
                txtRate_3.Text = row("Rate_3").ToString()
            Next

            If (Not (Session("FlushMessage")) Is Nothing) Then
                lblMessege.Text = Session("FlushMessage").ToString()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "showModal();", True)
                Session("FlushMessage") = Nothing
            End If
        End If
    End Sub
    Private Sub ExecuteSQLQuery(ByVal sql As String)
        Using con As New SqlConnection(CnString)
            Using cmd As New SqlCommand(sql)
                cmd.Parameters.AddWithValue("@TAX_Name_1", txtTAX_Name_1.Text)
                cmd.Parameters.AddWithValue("@TAX_Name_2", txtTAX_Name_2.Text)
                cmd.Parameters.AddWithValue("@TAX_Name_3", txtTAX_Name_3.Text)
                cmd.Parameters.AddWithValue("@Rate_1", ConvertingNumbers(txtRate_1.Text))
                cmd.Parameters.AddWithValue("@Rate_2", ConvertingNumbers(txtRate_2.Text))
                cmd.Parameters.AddWithValue("@Rate_3", ConvertingNumbers(txtRate_3.Text))
                cmd.Connection = con
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        End Using
    End Sub
    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim dt As DataTable = QueryDataTable("SELECT * FROM TAXInfo")
        If dt.Rows.Count > 0 Then
            ExecuteSQLQuery("UPDATE   TAXInfo SET TAX_Name_1=@TAX_Name_1, TAX_Name_2=@TAX_Name_2, TAX_Name_3=@TAX_Name_3, Rate_1=@Rate_1, Rate_2=@Rate_2, Rate_3=@Rate_3 ")
            Session("FlushMessage") = "Data has been updated successfully."
            Response.Redirect("TAXInfo.aspx")
        Else
            ExecuteSQLQuery("INSERT INTO  TAXInfo (TAX_Name_1, TAX_Name_2, TAX_Name_3, Rate_1, Rate_2, Rate_3) VALUES (@TAX_Name_1, @TAX_Name_2, @TAX_Name_3, @Rate_1, @Rate_2, @Rate_3)")
            Session("FlushMessage") = "Data has been saved successfully."
            Response.Redirect("TAXInfo.aspx")
        End If
    End Sub
End Class