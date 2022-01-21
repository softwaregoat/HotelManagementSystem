<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NotFound.aspx.vb" Inherits="HotelManagementSystem.NotFound" %>

<!DOCTYPE html>

<html lang="en">
<head runat="server">
    <meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<meta name="viewport" content="width=device-width, initial-scale=1">
	<title>oOopS!</title>
    <link rel="icon" href="Assets/Img/favicon.ico" type="image/gif" sizes="16x16">
	<link href="https://fonts.googleapis.com/css?family=Maven+Pro:400,900" rel="stylesheet">
    <link rel="stylesheet" href="Assets/ErrorPage.css">
</head>
<body>
    <form id="form1" runat="server">
        <div id="notfound">
            <div class="notfound">
                <div class="notfound-404">
                    <h1>404</h1>
                </div>
                <h2>We are sorry, Page not found!</h2>
                <p>The page you are looking for might have been removed had its name changed or is temporarily unavailable.</p>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Default.aspx">Back</asp:HyperLink>
            </div>
        </div>
    </form>
</body>
</html>
