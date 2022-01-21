Imports System.Data.SqlClient
Imports System.IO

Public Class AddExtraBed
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


            If (Not (Session("FlushMessage")) Is Nothing) Then
                lblMessege.Text = Session("FlushMessage").ToString()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "showModal();", True)
                Session("FlushMessage") = Nothing
            End If
        End If
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        ExecuteSqlQuery("UPDATE GuestInformation SET Extra_Beds = Extra_Beds + " & ConvertingNumbers(txtBed.Text) & "  WHERE    (GUEST_ID = " & ConvertingNumbers(txtGuestID.Text) & ") ")
        Using con As New SqlConnection(CnString)
            Using cmd As New SqlCommand(" INSERT INTO  ExtraBed (GUEST_ID, SOLD_DATE, NO_OF_BED, Comment) VALUES (@GUEST_ID, '" + Format(Now, "MM/dd/yyyy") + "', @NO_OF_BED, @Comment)  ")
                cmd.Parameters.AddWithValue("@GUEST_ID", ConvertingNumbers(txtGuestID.Text))
                cmd.Parameters.AddWithValue("@NO_OF_BED", ConvertingNumbers(txtBed.Text))
                cmd.Parameters.AddWithValue("@Comment", txtComment.Text)
                cmd.Connection = con
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        End Using
        HOTEL_SERVICE_CHARGE_CALCULATION(ConvertingNumbers(txtGuestID.Text))
        Session("FlushMessage") = "Data has been saved successfully."
        Response.Redirect("AddExtraBed.aspx")
    End Sub
End Class