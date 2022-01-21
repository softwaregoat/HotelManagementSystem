Imports System.Data.SqlClient

Public Class Site
    Inherits System.Web.UI.MasterPage
    Public sqlDTxMainMenu As New DataTable
    Public sqlDTx As New DataTable
    Public sqlDTview As New DataView
    Public sqlDTxSubMenu As New DataTable
    Public UserId As New Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Session("LOGGED_USER_ID") Is Nothing Then
                FormsAuthentication.RedirectToLoginPage()
                Return

            End If
            Dim dt As DataTable = QueryDataTable("SELECT * FROM UserInformation   WHERE  (USER_ID = " & ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString)) & ")   ")
            For Each row As DataRow In dt.Rows
                lblUserFullName.Text = row("FirstName").ToString() & " " & row("LastName").ToString()
            Next

        End If
    End Sub
    Public Function ExecQuery(ByVal SQLQuery As String) As DataTable
        Try
            Dim sqlCon As New SqlConnection(CnString)
            Dim sqlDA As New SqlDataAdapter(SQLQuery, sqlCon)
            Dim sqlCB As New SqlCommandBuilder(sqlDA)
            sqlDTx.Reset()
            sqlDA.Fill(sqlDTx)
            'sqlDTview = sqlDTx.DefaultView
        Catch ex As Exception
        End Try
        Return sqlDTx
    End Function
    Public Function ExecQuery2(ByVal SQLQuery As String) As DataTable
        Try
            Dim sqlCon As New SqlConnection(CnString)
            Dim sqlDA As New SqlDataAdapter(SQLQuery, sqlCon)
            Dim sqlCB As New SqlCommandBuilder(sqlDA)
            sqlDTxMainMenu.Reset()
            sqlDA.Fill(sqlDTxMainMenu)
        Catch ex As Exception
        End Try
        Return sqlDTxMainMenu
    End Function
    Public Function ExecQuery3(ByVal SQLQuery As Integer) As DataTable
        Try
            Dim copyDt As DataView = sqlDTx.DefaultView
            copyDt.RowFilter = "MenuID=" & SQLQuery
            sqlDTxSubMenu.Reset()
            sqlDTxSubMenu = copyDt.ToTable.Copy
        Catch ex As Exception
        End Try
        Return sqlDTxSubMenu
    End Function
    Public Function GetUserId() As Integer
        Try
            If Session("LOGGED_USER_ID") Is Nothing Then
                FormsAuthentication.RedirectToLoginPage()
            Else
                UserId = ConvertingNumbers(Decrypt(Session("LOGGED_USER_ID").ToString))
            End If
        Catch ex As Exception
        End Try
        Return UserId
    End Function
End Class