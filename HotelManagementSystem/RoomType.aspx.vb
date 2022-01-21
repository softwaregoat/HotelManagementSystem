Imports System.Data.SqlClient
Imports System.IO

Public Class RoomType
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
                        Dim dt_cust As DataTable = QueryDataTable("SELECT * FROM RoomType WHERE ROOM_TYPE_ID= " & ConvertingNumbers(hfldID.Value) & " ")
                        For Each custrow As DataRow In dt_cust.Rows
                            txtRoomType.Text = custrow("ROOM_TYPE").ToString()
                        Next
                    Else
                        Response.Redirect("RoomType.aspx")
                    End If
                    '---------------------------------------
                ElseIf Request.QueryString("act") = "del" Then
                    '---------------------------------------
                    If IsNumeric(Request.QueryString("id")) Then
                        hfldID.Value = ConvertingNumbers(Request.QueryString("id"))
                        QueryDataTable("DELETE RoomType WHERE ROOM_TYPE_ID= " & ConvertingNumbers(hfldID.Value) & " ")
                        Session("FlushMessage") = "Data has been Deleted successfully."
                        Response.Redirect("RoomType.aspx")
                    Else
                        Response.Redirect("RoomType.aspx")
                    End If
                    '---------------------------------------
                Else
                    Response.Redirect("RoomType.aspx")
                End If
            End If

            Dim dt As DataTable = QueryDataTable("SELECT * FROM RoomType")
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
        If btnSubmit.Text = "Submit" Then
            Using con As New SqlConnection(CnString)
                Using cmd As New SqlCommand("INSERT INTO  RoomType (ROOM_TYPE) VALUES (@ROOM_TYPE)")
                    cmd.Parameters.AddWithValue("@ROOM_TYPE", txtRoomType.Text)
                    cmd.Connection = con
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                End Using
            End Using
            Session("FlushMessage") = "Data has been saved successfully."
            Response.Redirect("RoomType.aspx")
        ElseIf btnSubmit.Text = "Update" Then
            Using con As New SqlConnection(CnString)
                Using cmd As New SqlCommand("UPDATE   RoomType SET ROOM_TYPE= @ROOM_TYPE  WHERE  ROOM_TYPE_ID= @ROOM_TYPE_ID")
                    cmd.Parameters.AddWithValue("@ROOM_TYPE_ID", ConvertingNumbers(hfldID.Value))
                    cmd.Parameters.AddWithValue("@ROOM_TYPE", txtRoomType.Text)
                    cmd.Connection = con
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                End Using
            End Using
            Session("FlushMessage") = "Data has been updated successfully."
            Response.Redirect("RoomType.aspx")
        End If
    End Sub
End Class