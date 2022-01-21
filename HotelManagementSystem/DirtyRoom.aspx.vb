Imports System.Data.SqlClient
Imports System.IO

Public Class DirtyRoom
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

            Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
            Using con As New SqlConnection(constr)
                Using sqlComm As New SqlCommand("GetCheckOutRoomDetails")
                    sqlComm.CommandType = CommandType.StoredProcedure
                    sqlComm.Connection = con
                    Dim billTbl As DataSet = New DataSet
                    Dim sqlDA As New SqlDataAdapter(sqlComm)
                    Dim sqlCB As New SqlCommandBuilder(sqlDA)
                    sqlDA.Fill(billTbl)
                    If billTbl IsNot Nothing Then
                        If billTbl.Tables(0).Rows.Count > 0 Then
                            Dim dv As DataView = billTbl.Tables(0).DefaultView
                            dv.Sort = "TimeDirty DESC"
                            'GridView1.DataSource = billTbl.Tables(0)
                            GridView1.DataSource = dv
                            GridView1.DataBind()
                        End If
                    End If
                End Using
            End Using


            'GridView1.DataSource = FetchDataFromTable(" SELECT        GuestInformation.GUEST_ID, GuestInformation.GuestName, GuestInformation.Status, GuestInformation.Address, GuestInformation.PhoneNo, RoomType.ROOM_TYPE, RoomDetails.Room_No, GuestInformation.Check_In_Date,  GuestInformation.Check_Out_Date FROM            RoomType INNER JOIN
            '                                            RoomDetails ON RoomType.ROOM_TYPE_ID = RoomDetails.ROOM_TYPE_ID RIGHT OUTER JOIN GuestInformation ON RoomDetails.ROOM_ID = GuestInformation.ROOM_ID WHERE        (GuestInformation.DirtyRoom = 'Y') ")
            'GridView1.DataBind()

            Dim rows = GridView1.Rows

            For Each row As GridViewRow In rows
                Dim RoomId As HiddenField = CType(row.FindControl("ROOM_ID"), HiddenField)
                Dim GuestId As HiddenField = CType(row.FindControl("GUEST_ID"), HiddenField)


                sqlStr = "select distinct UserInformation.USER_ID from  RoomDetails  INNER JOIN

                         GuestInformation ON RoomDetails.ROOM_ID = GuestInformation.ROOM_ID
						 Left Outer JOIN UserInformation ON GuestInformation.USER_ID=UserInformation.USER_ID where RoomDetails.ROOM_ID= " & RoomId.Value & " and GuestInformation.GUEST_ID=" & GuestId.Value & "  and UserInformation.ROLE_ID=3 "
                Dim SelectedMaid As DataSet = FetchDataFromTable(sqlStr)



                Dim ddlMaidNames As DropDownList = CType(row.FindControl("ddlMaidNames"), DropDownList)
                ddlMaidNames.DataSource = FetchDataFromTable("SELECT  *,(FirstName +' '+ LastName) as FullName  FROM  UserInformation where ROLE_ID=3")
                ddlMaidNames.DataTextField = "FullName"
                ddlMaidNames.DataValueField = "USER_ID"
                If SelectedMaid IsNot Nothing Then
                    If SelectedMaid.Tables(0).Rows.Count > 0 Then
                        ddlMaidNames.SelectedValue = SelectedMaid.Tables(0).Rows(0)("USER_ID").ToString
                    End If

                End If
                ddlMaidNames.DataBind()

                ddlMaidNames.Items.Insert(0, New ListItem("Seleccionar una recamarera"))

                'Dim txtMaidComments As TextBox = CType(row.FindControl("txtMaidComments"), TextBox)
                'txtMaidName.Text = "Some Value"
                'txtMaidComments.Text = "Some Value"
            Next

            If (Not (Session("FlushCleanRoomMessage")) Is Nothing) Then
                lblMessege.Text = Session("FlushCleanRoomMessage").ToString()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "showModal();", True)
                Session("FlushCleanRoomMessage") = Nothing
            End If
        End If
    End Sub
    Protected Sub CleanDone(ByVal sender As Object, ByVal e As EventArgs)
        Dim maidId As Integer = 0
        Dim btnSend As Button = CType(sender, Button)
        Dim row As GridViewRow = CType(btnSend.NamingContainer, GridViewRow)
        Dim ddlMaidNames As DropDownList = CType(row.FindControl("ddlMaidNames"), DropDownList)
        Dim lblMaidErr As Label = CType(row.FindControl("lblMaidErr"), Label)
        If IsNumeric(ddlMaidNames.SelectedValue) Then
            If ddlMaidNames.SelectedValue > 0 Then
                maidId = ConvertingNumbers(ddlMaidNames.SelectedValue)
                lblMaidErr.Text = ""
                lblMaidErr.ForeColor = System.Drawing.Color.White
            Else
                lblMaidErr.Text = "Por favor seleccionar una Recamarera"
                lblMaidErr.ForeColor = System.Drawing.Color.Red
                Return
            End If
        Else
            lblMaidErr.Text = "Por favor seleccionar una Recamarera"
            lblMaidErr.ForeColor = System.Drawing.Color.Red
            Return
        End If
        Dim txtMaidComments As TextBox = CType(row.FindControl("txtMaidComments"), TextBox)
        Dim hdnRoom_ID As HiddenField = CType(row.FindControl("ROOM_ID"), HiddenField)
        Dim hdnGUEST_ID As HiddenField = CType(row.FindControl("GUEST_ID"), HiddenField)

        Dim constr As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString

        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("DirtyCleanedRoom_Insert")
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@ROOM_ID", hdnRoom_ID.Value)
                cmd.Parameters.AddWithValue("@GUEST_ID", ConvertingNumbers(hdnGUEST_ID.Value))
                cmd.Connection = con
                con.Open()
                Dim result As Integer = Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        End Using




        ExecuteSqlQuery("UPDATE GuestInformation SET  USER_ID=" & maidId & ",DirtyRoom = 'N',Arrival_n_Departure_Date_Time='" & txtMaidComments.Text & "'  WHERE  (GUEST_ID = " & ConvertingNumbers(hdnGUEST_ID.Value) & ") ")
        ExecuteSqlQuery("UPDATE    RoomDetails   SET    Occupied='N',  GUEST_ID =  0,Status_ID=1  WHERE   (ROOM_ID = " & ConvertingNumbers(hdnRoom_ID.Value) & ")")
        ExecuteSqlQuery("Delete from    RoomLastInfo   WHERE   (ROOM_ID = " & ConvertingNumbers(hdnRoom_ID.Value) & ") AND (GUEST_ID = " & ConvertingNumbers(hdnGUEST_ID.Value) & ")")
        'Session("FlushCleanRoomMessage") = "Room cleaned successfully."
        Response.Redirect("DirtyRoom.aspx")
    End Sub

    Protected Sub txtMaidComments_TextChanged(sender As Object, e As EventArgs)
        Dim maidId As Integer = 0
        Dim btnSend As TextBox = CType(sender, TextBox)
        Dim row As GridViewRow = CType(btnSend.NamingContainer, GridViewRow)
        Dim ddlMaidNames As DropDownList = CType(row.FindControl("ddlMaidNames"), DropDownList)
        If IsNumeric(ddlMaidNames.SelectedValue) Then
            maidId = ConvertingNumbers(ddlMaidNames.SelectedValue)
        Else
            Return
        End If
        Dim txtMaidComments As TextBox = CType(row.FindControl("txtMaidComments"), TextBox)
        Dim hdnRoom_ID As HiddenField = CType(row.FindControl("ROOM_ID"), HiddenField)
        Dim hdnGUEST_ID As HiddenField = CType(row.FindControl("GUEST_ID"), HiddenField)
        ExecuteSqlQuery("UPDATE GuestInformation SET  USER_ID=" & maidId & ",DirtyRoom = 'N',Arrival_n_Departure_Date_Time='" & txtMaidComments.Text & "'  WHERE  (GUEST_ID = " & ConvertingNumbers(hdnGUEST_ID.Value) & ") ")

    End Sub

    Protected Sub ddlMaidNames_SelectedIndexChanged(sender As Object, e As EventArgs)

        Dim maidId As Integer = 0
        Dim btnSend As DropDownList = CType(sender, DropDownList)
        Dim row As GridViewRow = CType(btnSend.NamingContainer, GridViewRow)
        Dim ddlMaidNames As DropDownList = CType(row.FindControl("ddlMaidNames"), DropDownList)
        If IsNumeric(ddlMaidNames.SelectedValue) Then
            maidId = ConvertingNumbers(ddlMaidNames.SelectedValue)
        Else
            Return
        End If
        Dim txtMaidComments As TextBox = CType(row.FindControl("txtMaidComments"), TextBox)
        Dim hdnRoom_ID As HiddenField = CType(row.FindControl("ROOM_ID"), HiddenField)
        Dim hdnGUEST_ID As HiddenField = CType(row.FindControl("GUEST_ID"), HiddenField)
        ExecuteSqlQuery("UPDATE GuestInformation SET  USER_ID=" & maidId & ",DirtyRoom = 'N',Arrival_n_Departure_Date_Time='" & txtMaidComments.Text & "'  WHERE  (GUEST_ID = " & ConvertingNumbers(hdnGUEST_ID.Value) & ") ")

    End Sub
End Class