<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DataNotFound.aspx.vb" Inherits="HotelManagementSystem.DataNotFound" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Data Not Found</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
    <link href="https://cdn.bootcss.com/bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            background: #2E95EF;
            background-image: -moz-radial-gradient(center 45deg,circle cover, #EAF5FF, #D8EBFC);
            background-image: -webkit-gradient(radial, 50% 50%, 0, 50% 50%,800, from(#EAF5FF), to(#D8EBFC));
            padding-top: 15%;
        }

        .img-error {
            width: 220px;
            height: 220px;
        }
    </style>
</head>
<body>
    <div class="container bootstrap snippet">
        <div class="row">
            <div class="col-md-12">
                <div class="pull-right" style="margin-top: 10px;">
                    <div class="col-md-10 col-md-offset-1 pull-right">
                        <img class="img-error" src="Assets/Img/sqlmicrosoft.svg" />
                        <h2>Data Not Found</h2>
                        <p>Sorry, Value does not exist or does not match filter criteria!</p>
                        <form id="form1" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="https://cdn.bootcss.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
</body>
</html>
