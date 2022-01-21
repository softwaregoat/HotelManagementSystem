Imports System.Data.SqlClient
Imports System.IO

Public Class RoomDetails
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then

            If Not Me.Page.User.Identity.IsAuthenticated Or Session("LOGGED_USER_ID") Is Nothing Then
                FormsAuthentication.RedirectToLoginPage()
            End If
            Dim currentPageFileName As String = New FileInfo(Me.Request.Url.LocalPath).Name
            Try
                CheckPermissions(currentPageFileName.Split(".aspx")(0).ToString, ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)))
                SetInitialRow()
            Catch ex As Exception

            End Try

            ddlRoomType.DataSource = FetchDataFromTable("SELECT   *  FROM    RoomType")
            ddlRoomType.DataTextField = "ROOM_TYPE"
            ddlRoomType.DataValueField = "ROOM_TYPE_ID"
            ddlRoomType.DataBind()
            ddlRoomType.Items.Insert(0, New ListItem("Please select"))


            If Request.QueryString("act") IsNot Nothing Then
                If Request.QueryString("act") = "edit" Then
                    '---------------------------------------
                    If IsNumeric(Request.QueryString("id")) Then
                        btnSubmit.Text = "Update"
                        hfldID.Value = ConvertingNumbers(Request.QueryString("id"))
                        Dim dt_cust As DataTable = QueryDataTable("SELECT * FROM RoomDetails WHERE ROOM_ID= " & ConvertingNumbers(hfldID.Value) & " ")
                        For Each custrow As DataRow In dt_cust.Rows
                            'txtRent.Text = custrow("Room_Rent").ToString()
                            txtRoomNo.Text = custrow("Room_No").ToString()
                            ddlRoomType.SelectedValue = custrow("ROOM_TYPE_ID")
                        Next
                        sqlStr = " SELECT Room_Hours as Column1,Room_Rent as Column2,RENT_ID as Column3 FROM RoomRent WHERE ROOM_ID= " & ConvertingNumbers(hfldID.Value) & "  "
                        Dim currentDs As DataSet
                        currentDs = FetchDataFromTable(sqlStr)
                        'Dim newrow As DataRow
                        'newrow = currentDs.Tables(0).NewRow
                        'currentDs.Tables(0).Rows.Add(newrow)
                        ViewState("CurrentTable") = currentDs.Tables(0)
                        SetPreviousData2()
                    Else
                        Response.Redirect("RoomDetails.aspx")
                    End If
                    '---------------------------------------
                ElseIf Request.QueryString("act") = "del" Then
                    '---------------------------------------
                    If IsNumeric(Request.QueryString("id")) Then
                        hfldID.Value = ConvertingNumbers(Request.QueryString("id"))
                        QueryDataTable("DELETE RoomDetails WHERE ROOM_ID= " & ConvertingNumbers(hfldID.Value) & " ")
                        Session("FlushMessage") = "Data has been Deleted successfully."
                        Response.Redirect("RoomDetails.aspx")
                    Else
                        Response.Redirect("RoomDetails.aspx")
                    End If
                    '---------------------------------------
                Else
                    Response.Redirect("RoomDetails.aspx")
                End If
            End If


            sqlStr = " SELECT        RoomDetails.ROOM_ID, RoomType.ROOM_TYPE, RoomDetails.Room_No, RoomDetails.Room_Rent, RoomDetails.Occupied " &
                     " FROM            RoomDetails LEFT OUTER JOIN  RoomType ON RoomDetails.ROOM_TYPE_ID = RoomType.ROOM_TYPE_ID "
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
            Dim RoomId As Integer = 0
            Using con As New SqlConnection(CnString)
                Using cmd2 As New SqlCommand("CheckRoomDuplicate")
                    cmd2.CommandType = CommandType.StoredProcedure
                    cmd2.Parameters.AddWithValue("@RoomNo", txtRoomNo.Text)
                    cmd2.Connection = con
                    con.Open()
                    RoomId = Convert.ToInt32(cmd2.ExecuteScalar())
                    con.Close()
                End Using
                If RoomId = 0 Then
                    Using cmd As New SqlCommand("INSERT INTO  RoomDetails (ROOM_TYPE_ID, Room_No, Room_Rent, Occupied) VALUES (@ROOM_TYPE_ID, @Room_No, @Room_Rent, 'N') ")
                        cmd.Parameters.AddWithValue("@ROOM_TYPE_ID", ConvertingNumbers(ddlRoomType.SelectedValue))
                        cmd.Parameters.AddWithValue("@Room_No", txtRoomNo.Text)
                        cmd.Parameters.AddWithValue("@Room_Rent", 0)
                        cmd.Connection = con
                        con.Open()
                        cmd.ExecuteNonQuery()
                        Dim cmd2 As New SqlCommand("INSERT INTO RoomRent (ROOM_ID,Room_Hours,Room_Rent) VALUES((select top 1 ROOM_ID from RoomDetails order by ROOM_ID desc),@Room_Hours,@Room_Rent)")
                        cmd2.CommandType = CommandType.Text
                        ' Dim adapFam As New SqlDataAdapter 
                        For i As Integer = 0 To GridView2.Rows.Count - 1
                            Dim box3 As TextBox = CType(GridView2.Rows(i).Cells(0).FindControl("TextBox1"), TextBox)
                            Dim box4 As TextBox = CType(GridView2.Rows(i).Cells(1).FindControl("TextBox2"), TextBox)
                            cmd2.Parameters.Clear()
                            cmd2.Parameters.AddWithValue("@Room_Hours", box3.Text)
                            cmd2.Parameters.AddWithValue("@Room_Rent", box4.Text)
                            cmd2.Connection = con
                            cmd2.ExecuteNonQuery()
                            'adapFam.InsertCommand.ExecuteNonQuery()

                        Next
                        con.Close()
                    End Using
                    Session("FlushMessage") = "Data has been saved successfully."
                    Response.Redirect("RoomDetails.aspx")
                Else
                    Session("FlushMessage") = txtRoomNo.Text + " already Exists!"
                    lblerror.CssClass = "label label-danger"
                    lblMessege.CssClass = "label label-danger"
                    lblerror.Text = txtRoomNo.Text + " already Exists!"


                End If

            End Using

        ElseIf btnSubmit.Text = "Update" Then
            Using con As New SqlConnection(CnString)
                Using cmd As New SqlCommand("UPDATE   RoomDetails SET ROOM_TYPE_ID= @ROOM_TYPE_ID, Room_No=@Room_No, Room_Rent=@Room_Rent  WHERE  ROOM_ID= @ROOM_ID")
                    cmd.Parameters.AddWithValue("@ROOM_ID", ConvertingNumbers(hfldID.Value))
                    cmd.Parameters.AddWithValue("@ROOM_TYPE_ID", ConvertingNumbers(ddlRoomType.SelectedValue))
                    cmd.Parameters.AddWithValue("@Room_No", txtRoomNo.Text)
                    cmd.Parameters.AddWithValue("@Room_Rent", 0)
                    cmd.Connection = con
                    con.Open()
                    cmd.ExecuteNonQuery()
                    Dim cmd2 As New SqlCommand("Update RoomRent Set Room_Hours=@Room_Hours,Room_Rent=@Room_Rent where RENT_ID=@RENT_ID")
                    cmd2.CommandType = CommandType.Text
                    ' Dim adapFam As New SqlDataAdapter 
                    Dim lasthours As String
                    For i As Integer = 0 To GridView2.Rows.Count - 1
                        Dim box3 As TextBox = CType(GridView2.Rows(i).Cells(0).FindControl("TextBox1"), TextBox)
                        Dim box4 As TextBox = CType(GridView2.Rows(i).Cells(1).FindControl("TextBox2"), TextBox)
                        Dim rentId As HiddenField = CType(GridView2.Rows(i).Cells(2).FindControl("RentId"), HiddenField)
                        If lasthours = box3.Text Then
                            lasthours = String.Empty
                        Else
                            lasthours = box3.Text
                            If rentId.Value = 0 Then
                                Dim cmd3 As New SqlCommand("INSERT INTO RoomRent (ROOM_ID,Room_Hours,Room_Rent) VALUES(" & ConvertingNumbers(hfldID.Value) & ",@Room_Hours,@Room_Rent)")
                                cmd3.CommandType = CommandType.Text
                                cmd3.Parameters.Clear()
                                cmd3.Parameters.AddWithValue("@Room_Hours", box3.Text)
                                cmd3.Parameters.AddWithValue("@Room_Rent", box4.Text)
                                cmd3.Connection = con
                                cmd3.ExecuteNonQuery()
                            Else
                                cmd2.Parameters.Clear()
                                cmd2.Parameters.AddWithValue("@Room_Hours", box3.Text)
                                cmd2.Parameters.AddWithValue("@Room_Rent", box4.Text)
                                cmd2.Parameters.AddWithValue("@RENT_ID", rentId.Value)
                                cmd2.Connection = con
                                cmd2.ExecuteNonQuery()
                            End If
                        End If



                        'adapFam.InsertCommand.ExecuteNonQuery()

                    Next



                    con.Close()
                End Using
            End Using
            Session("FlushMessage") = "Data has been updated successfully."
            Response.Redirect("RoomDetails.aspx")
        End If
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        AddNewRowToGrid()
    End Sub

    Private Sub SetInitialRow()
        Dim dt As DataTable = New DataTable()
        Dim dr As DataRow = Nothing
        dt.Columns.Add(New DataColumn("Column1", GetType(String)))
        dt.Columns.Add(New DataColumn("Column2", GetType(String)))
        dt.Columns.Add(New DataColumn("Column3", GetType(Integer)))
        dr = dt.NewRow()
        dt.Rows.Add(dr)
        ViewState("CurrentTable") = dt
        GridView2.DataSource = dt
        GridView2.DataBind()
    End Sub

    Private Sub AddNewRowToGrid()
        Dim rowIndex As Integer = 0
        Dim box1 As TextBox
        Dim box2 As TextBox
        Dim box5 As TextBox
        Dim box6 As TextBox
        Dim rentId As HiddenField
        If ViewState("CurrentTable") IsNot Nothing Then
            Dim dtCurrentTable As DataTable = CType(ViewState("CurrentTable"), DataTable)
            Dim drCurrentRow As DataRow = Nothing
            Dim rowCount As Integer = dtCurrentTable.Rows.Count
            If dtCurrentTable.Rows.Count > 0 Then
                Dim index As Integer = 0



                If Request.QueryString("act") = "edit" Then
                    For i As Integer = 0 To dtCurrentTable.Rows.Count - 1
                        box1 = CType(GridView2.Rows(index).Cells(0).FindControl("TextBox1"), TextBox)
                        box2 = CType(GridView2.Rows(index).Cells(1).FindControl("TextBox2"), TextBox)
                        rentId = CType(GridView2.Rows(index).Cells(2).FindControl("RentId"), HiddenField)
                        index += 1
                    Next
                    drCurrentRow = dtCurrentTable.Rows(dtCurrentTable.Rows.Count - 1)
                    drCurrentRow("Column1") = box1.Text
                    drCurrentRow("Column2") = box2.Text
                    drCurrentRow("Column3") = rentId.Value
                    drCurrentRow = dtCurrentTable.NewRow()

                    'drCurrentRow("Column1") = String.Empty
                    'drCurrentRow("Column2") = String.Empty
                    'drCurrentRow("Column3") = 0
                    dtCurrentTable.Rows.Add(drCurrentRow)
                    GridView2.DataSource = dtCurrentTable
                    GridView2.DataBind()

                Else
                    For i As Integer = 1 To dtCurrentTable.Rows.Count
                        box1 = CType(GridView2.Rows(index).Cells(0).FindControl("TextBox1"), TextBox)
                        box2 = CType(GridView2.Rows(index).Cells(1).FindControl("TextBox2"), TextBox)
                        rentId = CType(GridView2.Rows(index).Cells(2).FindControl("RentId"), HiddenField)
                        index += 1
                    Next
                    drCurrentRow = dtCurrentTable.NewRow()
                    drCurrentRow("Column1") = box1.Text
                    drCurrentRow("Column2") = box2.Text
                    drCurrentRow("Column3") = 0
                    dtCurrentTable.Rows.Add(drCurrentRow)
                    GridView2.DataSource = dtCurrentTable
                    GridView2.DataBind()
                End If
                'drCurrentRow = dtCurrentTable.NewRow()
                'drCurrentRow("Column1") = box1.Text
                'drCurrentRow("Column2") = box2.Text
                'drCurrentRow("Column3") = 0
                'dtCurrentTable.Rows.Add(drCurrentRow)
                'GridView2.DataSource = dtCurrentTable
                'GridView2.DataBind()

                ViewState("CurrentTable") = dtCurrentTable
            End If
        Else
            Response.Write("ViewState is null")
        End If
        If Request.QueryString("act") = "edit" Then
            SetPreviousData2()
        Else
            SetPreviousData()
        End If


    End Sub

    Private Sub SetPreviousData()
        Dim rowIndex As Integer = 0

        If ViewState("CurrentTable") IsNot Nothing Then
            Dim dt As DataTable = CType(ViewState("CurrentTable"), DataTable)
            If dt.Rows.Count > 0 Then
                For i As Integer = 1 To dt.Rows.Count - 1
                    Dim box1 As TextBox = CType(GridView2.Rows(rowIndex).Cells(0).FindControl("TextBox1"), TextBox)
                    Dim box2 As TextBox = CType(GridView2.Rows(rowIndex).Cells(1).FindControl("TextBox2"), TextBox)
                    Dim rentId As HiddenField = CType(GridView2.Rows(rowIndex).Cells(2).FindControl("RentId"), HiddenField)
                    box1.Text = dt.Rows(i)("Column1").ToString()
                    box2.Text = dt.Rows(i)("Column2").ToString()
                    rentId.Value = ConvertingNumbers(dt.Rows(i)("Column3").ToString())
                    rowIndex += 1
                Next
            End If
        End If
    End Sub
    Private Sub SetPreviousData2()
        Dim rowIndex As Integer = 0

        If ViewState("CurrentTable") IsNot Nothing Then
            Dim dt As DataTable = CType(ViewState("CurrentTable"), DataTable)
            If dt.Rows.Count > 0 Then
                GridView2.DataSource = dt
                GridView2.DataBind()
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim box1 As TextBox = CType(GridView2.Rows(rowIndex).Cells(0).FindControl("TextBox1"), TextBox)
                    Dim box2 As TextBox = CType(GridView2.Rows(rowIndex).Cells(1).FindControl("TextBox2"), TextBox)
                    Dim rentId As HiddenField = CType(GridView2.Rows(rowIndex).Cells(2).FindControl("RentId"), HiddenField)
                    box1.Text = dt.Rows(i)("Column1").ToString()
                    box2.Text = dt.Rows(i)("Column2").ToString()
                    rentId.Value = ConvertingNumbers(dt.Rows(i)("Column3").ToString())
                    rowIndex += 1
                Next

            End If
        End If
    End Sub
End Class