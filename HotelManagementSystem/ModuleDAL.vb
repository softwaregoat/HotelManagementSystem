Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Security.Cryptography
Imports System.IO
Module ModuleDAL
    Public sqlStr As String
    Public mHasException As Boolean
    Public mLastException As Exception
    Public CnString As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
    Public xBILL_NO As Double
    Public Function QueryDataTable(ByVal sql As String) As DataTable
        Dim da As New SqlDataAdapter(sql, CnString)
        Dim ds As New DataSet
        da.Fill(ds, "result")
        Return ds.Tables("result")
    End Function
    Public Function FetchDataFromTable(query As String) As DataSet
        Dim conString As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
        Dim cmd As New SqlCommand(query)
        Using con As New SqlConnection(conString)
            Using sda As New SqlDataAdapter()
                cmd.Connection = con
                sda.SelectCommand = cmd
                Using ds As New DataSet()
                    sda.Fill(ds)
                    Return ds
                End Using
            End Using
        End Using
    End Function
    Public Function FetchDataByProcedureTable(procName As String) As DataSet
        Dim conString As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString
        Dim cmd As New SqlCommand(procName)
        cmd.CommandType = CommandType.StoredProcedure
        Using con As New SqlConnection(conString)
            Using sda As New SqlDataAdapter()
                cmd.Connection = con
                sda.SelectCommand = cmd
                Using ds As New DataSet()
                    sda.Fill(ds)
                    Return ds
                End Using
            End Using
        End Using
    End Function
    Public Function FetchDataByProcedure(procName As String, parameter As String) As DataTable
        mHasException = False
        Dim Guestid As Integer
        Guestid = 0
        If IsNumeric(parameter) Then
            Guestid = ConvertingNumbers(parameter)
        End If
        Dim dt = New DataTable()

        Try
            Dim conString As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString

            Using cn = New SqlConnection With {.ConnectionString = conString}
                Using cmd = New SqlCommand With {
                    .Connection = cn,
                    .CommandType = CommandType.StoredProcedure
                }

                    cmd.CommandText = procName
                    cmd.Parameters.AddWithValue("@Guest_ID", Guestid)
                    cmd.Parameters.AddWithValue("@Gest_ID", parameter)
                    cmd.Parameters.AddWithValue("@GuestName", parameter)


                    cn.Open()

                    dt.Load(cmd.ExecuteReader())

                End Using
            End Using

        Catch e As Exception
            mHasException = True
            mLastException = e
        End Try

        Return dt
    End Function
    Public Function FetchDataFromProcedure(procName As String) As DataTable
        mHasException = False
        Dim dt = New DataTable()

        Try
            Dim conString As String = ConfigurationManager.ConnectionStrings("ConStringHotelMngSys").ConnectionString

            Using cn = New SqlConnection With {.ConnectionString = conString}
                Using cmd = New SqlCommand With {
                    .Connection = cn,
                    .CommandType = CommandType.StoredProcedure
                }

                    cmd.CommandText = procName


                    cn.Open()

                    dt.Load(cmd.ExecuteReader())

                End Using
            End Using

        Catch e As Exception
            mHasException = True
            mLastException = e
        End Try

        Return dt
    End Function
    Public Function ConvertingNumbers(ByVal value As String)
        If Not IsNumeric(value) Then
            Return 0
        Else
            Return value
        End If
    End Function
    Public Function Encrypt(clearText As String) As String
        Dim EncryptionKey As String = "34rAUGYjbxfF3"
        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
             &H65, &H64, &H76, &H65, &H64, &H65,
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                    cs.Close()
                End Using
                clearText = Convert.ToBase64String(ms.ToArray())
            End Using
        End Using
        Return clearText
    End Function
    Public Function Decrypt(cipherText As String) As String
        Dim EncryptionKey As String = "34rAUGYjbxfF3"
        cipherText = cipherText.Replace(" ", "+")
        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
             &H65, &H64, &H76, &H65, &H64, &H65,
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        Return cipherText
    End Function
    Public Sub RoomBooking(ByVal GUEST_ID As Double, ByVal ROOM_ID As Double)
        ExecuteSqlQuery("UPDATE  RoomDetails SET Occupied='N',Status_ID=1, GUEST_ID = 0  WHERE   (GUEST_ID =" & GUEST_ID & ") ")
        ExecuteSqlQuery("UPDATE  RoomDetails SET Occupied='Y',Status_ID=2, GUEST_ID = " & GUEST_ID & "  WHERE   (ROOM_ID = " & ROOM_ID & ") ")
    End Sub
    Public Sub ExecuteSqlQuery(ByVal sql As String)
        Using con As New SqlConnection(CnString)
            Using cmd As New SqlCommand(sql)
                cmd.Connection = con
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        End Using
    End Sub
    Public Function ExecuteSqlScalerQuery(ByVal sql As String)
        Dim statusId As Int32 = 0
        Using con As New SqlConnection(CnString)
            Using cmd As New SqlCommand(sql)
                cmd.Connection = con
                con.Open()
                statusId = Convert.ToInt32(cmd.ExecuteScalar())
                con.Close()
            End Using
        End Using
        Return statusId
    End Function
    Public Sub Insert_Default_Permissions(ByVal USER_ID As Double)
        Using con As New SqlConnection(CnString)
            Using cmd As New SqlCommand(" INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'HotelInformation', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'EmployeeDetails', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'RoomType', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'RoomDetails', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'TAXInfo', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'CheckIn', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'ExtraServices', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'CheckOut', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'GuestList', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'Reservation', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'ChangeRoom', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'AddExtraBed', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'RoomView', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'DirtyRoom', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'AddFoodItem', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'FoodServices', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'ItemCart', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'ServiceReport', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'FoodServiceReport', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'UserInformation', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'UsersList', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'Permission', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'ChangePassword', 0) " &
                                        " INSERT INTO Permissions (USER_ID, WebForm, AllowUser) VALUES (@USER_ID, 'SMTP', 0) ")
                cmd.Parameters.AddWithValue("@USER_ID", USER_ID)
                cmd.Connection = con
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        End Using
    End Sub
    Public Sub BILL_CALCULATOR(ByVal BILL_NO As Double)
        Dim TAX1, TAX2, TAX3, ITEM_COST, BALANCES As Double
        Dim dt_tax As DataTable = QueryDataTable("SELECT * FROM TAXInfo")
        If dt_tax.Rows.Count > 0 Then
            TAX1 = dt_tax.Rows(0)("Rate_1")
            TAX2 = dt_tax.Rows(0)("Rate_2")
            TAX3 = dt_tax.Rows(0)("Rate_3")
        Else
            TAX1 = 0
            TAX2 = 0
            TAX3 = 0
        End If

        Dim item_inv As DataTable = QueryDataTable(" SELECT  *  FROM    FoodServiceDetails   WHERE   (BILL_NO = " & ConvertingNumbers(BILL_NO) & ") ")
        If item_inv.Rows.Count > 0 Then
            Dim item_inv_total As DataTable = QueryDataTable(" SELECT SUM(TotalPrice) AS Expr1  FROM    FoodServiceDetails   WHERE   (BILL_NO = " & ConvertingNumbers(BILL_NO) & ") ")
            ITEM_COST = item_inv_total.Rows(0)("Expr1")
        Else
            ITEM_COST = 0
        End If

        Dim dt_balnc As DataTable = QueryDataTable("SELECT  G_TOTAL - DISCOUNT - PAYMENT AS  Expr001  FROM     FoodServices  WHERE    (BILL_NO = " & ConvertingNumbers(BILL_NO) & ")")
        If dt_balnc.Rows.Count > 0 Then
            BALANCES = dt_balnc.Rows(0)("Expr001")
        Else
            BALANCES = 0
        End If

        ExecuteSqlQuery(" UPDATE FoodServices SET BALANCE=" & BALANCES & ", TAX_1= " & (ITEM_COST * TAX1 / 100) & " , TAX_2=" & (ITEM_COST * TAX2 / 100) & ", TAX_3=" & (ITEM_COST * TAX3 / 100) & ", ITEM_COST=" & ITEM_COST & ", TOTAL_TAX=" & (ITEM_COST * TAX1 / 100) + (ITEM_COST * TAX2 / 100) + (ITEM_COST * TAX3 / 100) & ", G_TOTAL= " & ITEM_COST + ((ITEM_COST * TAX1 / 100) + (ITEM_COST * TAX2 / 100) + (ITEM_COST * TAX3 / 100)) & "  WHERE   (BILL_NO = " & ConvertingNumbers(BILL_NO) & ") ")
    End Sub

    Public Sub HOTEL_SERVICE_CHARGE_CALCULATION(ByVal GUEST_ID As Double)
        Dim ROOM_ID, PerDay_Room_Rent, No_Of_Day, Total_Cost, TAX1, TAX2, TAX3 As Double

        ExecuteSqlQuery("UPDATE    GuestInformation   SET      Total_Extra_Bed_Cost =  Per_Bed_Cost * Extra_Beds  WHERE   (GUEST_ID = " & GUEST_ID & ")")

        Dim dt_guestinfo As DataTable = QueryDataTable(" SELECT    *  FROM     GuestInformation  WHERE    (GUEST_ID = " & GUEST_ID & ") ")
        If dt_guestinfo.Rows.Count > 0 Then
            ROOM_ID = dt_guestinfo.Rows(0)("ROOM_ID")
            No_Of_Day = dt_guestinfo.Rows(0)("No_Of_Day")
        End If

        Dim dt_RoomCharge As DataTable = QueryDataTable(" SELECT  *  FROM    RoomDetails  WHERE   (ROOM_ID = " & ROOM_ID & ") ")
        If dt_RoomCharge.Rows.Count > 0 Then
            PerDay_Room_Rent = dt_RoomCharge.Rows(0)("Room_Rent")
        Else
            PerDay_Room_Rent = 0
        End If

        ExecuteSqlQuery("UPDATE GuestInformation SET  No_Of_Day=" & No_Of_Day & ", Rent_Day=" & PerDay_Room_Rent & ", Total_Charges=" & (No_Of_Day * PerDay_Room_Rent) & "  WHERE    (GUEST_ID = " & GUEST_ID & ") ")

        Dim dt_totalCharge As DataTable = QueryDataTable("SELECT   (Boarding + Food + Laundry + Telephone + OtherCharges + BAR + DINNER + SPA + BanquetDinner + Cleaning + ServiceCharges + Breakfat + Total_Extra_Bed_Cost + Total_Charges) AS Expr01  FROM     GuestInformation  WHERE    (GUEST_ID = " & GUEST_ID & ") ")
        If dt_totalCharge.Rows.Count > 0 Then
            Total_Cost = dt_totalCharge.Rows(0)("Expr01")
        Else
            Total_Cost = 0
        End If

        ExecuteSqlQuery("UPDATE GuestInformation SET  Total=" & Total_Cost & "  WHERE    (GUEST_ID = " & GUEST_ID & ") ")

        Dim dt_tax As DataTable = QueryDataTable("SELECT * FROM TAXInfo")
        If dt_tax.Rows.Count > 0 Then
            TAX1 = dt_tax.Rows(0)("Rate_1")
            TAX2 = dt_tax.Rows(0)("Rate_2")
            TAX3 = dt_tax.Rows(0)("Rate_3")
        Else
            TAX1 = 0
            TAX2 = 0
            TAX3 = 0
        End If

        ExecuteSqlQuery("UPDATE GuestInformation SET  TAX_1=" & Total_Cost * TAX1 / 100 & ", TAX_2=" & Total_Cost * TAX2 / 100 & ", TAX_3=" & Total_Cost * TAX3 / 100 & ", TotalTAX=" & (Total_Cost * TAX1 / 100) + (Total_Cost * TAX2 / 100) + (Total_Cost * TAX3 / 100) & ", GrandTotal=" & Total_Cost + ((Total_Cost * TAX1 / 100) + (Total_Cost * TAX2 / 100) + (Total_Cost * TAX3 / 100)) & "  WHERE    (GUEST_ID = " & GUEST_ID & ") ")
        ExecuteSqlQuery("UPDATE    GuestInformation   SET      NetAmount = GrandTotal - DiscountAmount  WHERE   (GUEST_ID = " & GUEST_ID & ")")
        ExecuteSqlQuery("UPDATE    GuestInformation   SET      BalanceAmount = NetAmount - PaidAmount  WHERE   (GUEST_ID = " & GUEST_ID & ")")
    End Sub
    Public Sub CheckPermissions(ByVal PageName As String, ByVal userID As Double)
        Dim chkPermission As DataTable = QueryDataTable("Select u.*,p.WebForm,p.AllowUser from UserInformation as u inner join UserRoles as r on r.ROLE_ID=u.ROLE_ID  join PermissionsRoleBased as p on p.ROLE_ID=u.ROLE_ID where (u.USER_ID = " & ConvertingNumbers(userID) & ") AND (p.WebForm = '" + PageName + "') AND (p.AllowUser = 1)")
        If Not chkPermission.Rows.Count > 0 Then
            HttpContext.Current.Session("FlushMessage") = "Access prohibited. Page : " + PageName.ToString
            HttpContext.Current.Response.Redirect("Dashboard.aspx")
        End If
    End Sub
End Module
