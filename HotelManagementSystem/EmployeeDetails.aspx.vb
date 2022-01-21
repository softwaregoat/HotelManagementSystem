Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO

Public Class EmployeeDetails
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

            txtJoiningDate.Text = Format(Now, "dd/MM/yyyy")

            If Request.QueryString("act") IsNot Nothing Then
                If Request.QueryString("act") = "edit" Then
                    '---------------------------------------
                    If IsNumeric(Request.QueryString("id")) Then
                        btnSubmit.Text = "Update"
                        hfldID.Value = ConvertingNumbers(Request.QueryString("id"))
                        Dim dt_cust As DataTable = QueryDataTable("SELECT * FROM EmployeeDetails WHERE EMPLOYEE_ID= " & ConvertingNumbers(hfldID.Value) & " ")
                        For Each custrow As DataRow In dt_cust.Rows
                            txtEmployeeName.Text = custrow("EmployeeName").ToString()
                            txtParentsNames.Text = custrow("ParentsNames").ToString()
                            txtAddress.Text = custrow("Address").ToString()
                            txtContactNumber.Text = custrow("ContactNumber").ToString()
                            txtPhotoIdCardNo.Text = custrow("PhotoIdCardNo").ToString()
                            txtDesignation.Text = custrow("Designation").ToString()
                            txtJoiningDate.Text = Format(custrow("JoiningDate"), "dd/MM/yyyy")
                        Next
                    Else
                        Response.Redirect("EmployeeDetails.aspx")
                    End If
                    '---------------------------------------
                ElseIf Request.QueryString("act") = "del" Then
                    '---------------------------------------
                    If IsNumeric(Request.QueryString("id")) Then
                        hfldID.Value = ConvertingNumbers(Request.QueryString("id"))
                        QueryDataTable("DELETE EmployeeDetails WHERE EMPLOYEE_ID= " & ConvertingNumbers(hfldID.Value) & " ")
                        Session("FlushMessage") = "Data has been Deleted successfully."
                        Response.Redirect("EmployeeDetails.aspx")
                    Else
                        Response.Redirect("EmployeeDetails.aspx")
                    End If
                    '---------------------------------------
                Else
                    Response.Redirect("EmployeeDetails.aspx")
                End If
            End If


            Dim dt As DataTable = QueryDataTable("SELECT * FROM EmployeeDetails")
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
        Dim Joining_Date As Date = DateTime.ParseExact(txtJoiningDate.Text.ToString, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
        If btnSubmit.Text = "Submit" Then
            Using con As New SqlConnection(CnString)
                Using cmd As New SqlCommand("INSERT INTO  EmployeeDetails (EmployeeName, ParentsNames, Address, ContactNumber, PhotoIdCardNo, Designation, JoiningDate) VALUES (@EmployeeName, @ParentsNames, @Address, @ContactNumber, @PhotoIdCardNo, @Designation, @JoiningDate)")
                    cmd.Parameters.AddWithValue("@EmployeeName", txtEmployeeName.Text)
                    cmd.Parameters.AddWithValue("@ParentsNames", txtParentsNames.Text)
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text)
                    cmd.Parameters.AddWithValue("@ContactNumber", txtContactNumber.Text)
                    cmd.Parameters.AddWithValue("@PhotoIdCardNo", txtPhotoIdCardNo.Text)
                    cmd.Parameters.AddWithValue("@Designation", txtDesignation.Text)
                    cmd.Parameters.AddWithValue("@JoiningDate", Joining_Date)
                    cmd.Connection = con
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                End Using
            End Using
            Session("FlushMessage") = "Data has been saved successfully."
            Response.Redirect("EmployeeDetails.aspx")
        ElseIf btnSubmit.Text = "Update" Then
            Using con As New SqlConnection(CnString)
                Using cmd As New SqlCommand("UPDATE   EmployeeDetails SET  EmployeeName=@EmployeeName, ParentsNames=@ParentsNames, Address=@Address, ContactNumber=@ContactNumber, PhotoIdCardNo=@PhotoIdCardNo, Designation=@Designation, JoiningDate=@JoiningDate  WHERE  EMPLOYEE_ID= @EMPLOYEE_ID")
                    cmd.Parameters.AddWithValue("@EMPLOYEE_ID", ConvertingNumbers(hfldID.Value))
                    cmd.Parameters.AddWithValue("@EmployeeName", txtEmployeeName.Text)
                    cmd.Parameters.AddWithValue("@ParentsNames", txtParentsNames.Text)
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text)
                    cmd.Parameters.AddWithValue("@ContactNumber", txtContactNumber.Text)
                    cmd.Parameters.AddWithValue("@PhotoIdCardNo", txtPhotoIdCardNo.Text)
                    cmd.Parameters.AddWithValue("@Designation", txtDesignation.Text)
                    cmd.Parameters.AddWithValue("@JoiningDate", Joining_Date)
                    cmd.Connection = con
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                End Using
            End Using
            Session("FlushMessage") = "Data has been updated successfully."
            Response.Redirect("EmployeeDetails.aspx")
        End If
    End Sub
End Class