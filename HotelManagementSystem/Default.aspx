<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="HotelManagementSystem._Default" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Log In</title>
    <link rel="icon" href="Assets/Img/favicon.png" type="image/gif" sizes="16x16" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <link rel="stylesheet" href="~/Assets/bower_components/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/Assets/bower_components/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="~/Assets/bower_components/Ionicons/css/ionicons.min.css" />
    <link rel="stylesheet" href="~/Assets/dist/css/AdminLTE.min.css" />
    <link rel="stylesheet" href="~/Assets/plugins/iCheck/square/blue.css" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic">
</head>
<body class="hold-transition login-page">
    <form id="form1" runat="server">
          
        <div class="login-box">
          <div class="login-logo">
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Default.aspx">
                    <%--<b>Hotel</b>Management--%><img src="Assets/Img/logo1.png" width="360" height="80" />
                    
                </asp:HyperLink>
            </div>
            <!-- /.login-logo -->
            <div class="login-box-body">
                <p class="login-box-msg">
                    Inicia sesión con tu usuario. La contraseña distingue mayúsculas y minúsculas.<br />
                    <asp:Label ID="lblMessege" ForeColor="Red" Font-Size="Small" runat="server" Text="" />
                </p>
                <div class="form-group has-feedback">
                    <label for="txtEmail">Introduce tu usuario y contraseña</label>
                    <asp:TextBox ID="txtEmail" CssClass="form-control validate[required,custom[email]]" runat="server" placeholder="Usuario" />
                    <span class="glyphicon glyphicon-envelope form-control-feedback"></span>
                </div>
                <div class="form-group has-feedback">
                    <asp:TextBox ID="txtPassword" TextMode="Password" CssClass="form-control validate[required, minSize[5]]" placeholder="Contraseña" runat="server" />
                    <span class="glyphicon glyphicon-lock form-control-feedback"></span>
                </div>
                <div class="row">
                    <div class="col-xs-8 text-left">
                    </div>
                    <!-- /.col -->
                    <div class="col-xs-4">
                        <asp:Button ID="btnSubmit" OnClick="ValidateUser" CssClass="btn btn-info btn-block btn-flat" runat="server" Text="Iniciar" />
                    </div>
                    <!-- /.col -->
                </div>
                <!-- /.social-auth-links -->
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/ForgotPassword.aspx">¿Olvidaste tu contraseña?</asp:HyperLink>
            </div>
            <!-- /.login-box-body -->
        </div>
    </form>
    <!-- jQuery 3 -->
    <script src="Assets/bower_components/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap 3.3.7 -->
    <link rel="stylesheet" href="Assets/jQuery_Validation/css/validationEngine.jquery.css" type="text/css" />
    <script src="Assets/bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="Assets/jQuery_Validation/js/languages/jquery.validationEngine-en.js" charset="utf-8"></script>
    <script type="text/javascript" src="Assets/jQuery_Validation/js/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript">
        $(function () {
            $("#form1").validationEngine();
        });
    </script>
</body>
</html>
