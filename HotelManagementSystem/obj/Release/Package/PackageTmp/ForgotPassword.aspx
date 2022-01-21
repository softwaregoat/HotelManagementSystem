<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ForgotPassword.aspx.vb" Inherits="HotelManagementSystem.ForgotPassword" %>

<!DOCTYPE html>
<html>
<head runat="server">
  <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge" />
  <title>Forgot Password</title>
  <link rel="icon" href="Assets/Img/favicon.ico" type="image/gif" sizes="16x16"/>
  <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport"/>
  <link rel="stylesheet" href="~/Assets/bower_components/bootstrap/dist/css/bootstrap.min.css"/>
  <link rel="stylesheet" href="~/Assets/bower_components/font-awesome/css/font-awesome.min.css"/>
  <link rel="stylesheet" href="~/Assets/bower_components/Ionicons/css/ionicons.min.css"/>
  <link rel="stylesheet" href="~/Assets/dist/css/AdminLTE.min.css"/>
  <link rel="stylesheet" href="~/Assets/plugins/iCheck/square/blue.css"/>
  <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic">
</head>
<body class="hold-transition login-page">
     <form id="form1" runat="server">
        <div class="login-box">
  <div class="login-logo">
<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Default.aspx"><b>Hotel</b>Management</asp:HyperLink>
  </div>
  <!-- /.login-logo -->
  <div class="login-box-body">
    <p class="login-box-msg">Enter your registered email. We'll mail instruction on how to reset your password.<br />
            <asp:Label ID="lblMessege" ForeColor="Red" Font-Size="Small" runat="server" Text="" />               
    </p>
      <div class="form-group has-feedback">
         <asp:TextBox ID="txtEmail" CssClass="form-control validate[required,custom[email]]" runat="server" placeholder="Email" TextMode="Email" />
        <span class="glyphicon glyphicon-envelope form-control-feedback"></span>
      </div>
      <div class="row">
        <div class="col-xs-8 text-left">
         
        </div>
        <!-- /.col -->
        <div class="col-xs-4">
        <asp:Button ID="btnSubmit" CssClass="btn btn-info btn-block btn-flat" runat="server" Text="Submit" />
        </div>
        <!-- /.col -->
      </div>
    <!-- /.social-auth-links -->
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Default.aspx">Back to Login</asp:HyperLink>
  </div>
  <!-- /.login-box-body -->
</div>
    </form>
    <!-- jQuery 3 -->
    <script src="Assets/bower_components/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap 3.3.7 -->
    <link rel="stylesheet" href="Assets/jQuery_Validation/css/validationEngine.jquery.css" type="text/css"/>
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