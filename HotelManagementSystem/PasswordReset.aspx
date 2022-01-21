<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PasswordReset.aspx.vb" Inherits="HotelManagementSystem.PasswordReset" %>

<!DOCTYPE html>
<html>
<head runat="server">
  <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge" />
  <title>Password Reset</title> 
  <link rel="icon" href="Assets/Img/favicon.ico" type="image/gif" sizes="16x16">
  <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport"/>
  <link rel="stylesheet" href="~/Assets/bower_components/bootstrap/dist/css/bootstrap.min.css"/>
  <link rel="stylesheet" href="~/Assets/bower_components/font-awesome/css/font-awesome.min.css"/>
  <link rel="stylesheet" href="~/Assets/bower_components/Ionicons/css/ionicons.min.css"/>
  <link rel="stylesheet" href="~/Assets/dist/css/AdminLTE.min.css"/>
  <link rel="stylesheet" href="~/Assets/plugins/iCheck/square/blue.css"/>
  <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.3/jquery.min.js"></script>
</head>
<body class="hold-transition login-page">
    <form id="form1" runat="server">
        <div class="login-box">
      <div class="login-logo">
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Default.aspx"><b>Hotel</b>Management</asp:HyperLink>
      </div>
  <!-- /.login-logo -->
  <div class="login-box-body">
    <p class="login-box-msg">Enter your credentials to change password.<br />
            <asp:Label ID="lblMessege" ForeColor="Red" Font-Size="Small" runat="server" Text="" />               
    </p>
      <div class="form-group has-feedback">
         <asp:TextBox ID="txtEmail" CssClass="form-control validate[required,custom[email]]" runat="server" placeholder="Email" TextMode="Email" />
        <span class="glyphicon glyphicon-envelope form-control-feedback"></span>
      </div>
      <div class="form-group has-feedback">
         <asp:TextBox ID="txtResetCode" class="form-control validate[required,minSize[14],custom[integer]]" data-errormessage-custom-error="Please enter your 4 digit Reset Code." runat="server" placeholder="Reset Code" MaxLength="14" />
        <span class="fa fa-comment form-control-feedback"></span>
      </div>

      <div class="form-group has-feedback">
          <asp:TextBox ID="txtPassword"  CssClass="form-control validate[required, minSize[5]]" TextMode="Password" placeholder="Password" runat="server"></asp:TextBox>
        <span class="glyphicon glyphicon-lock form-control-feedback"></span>
      </div>

      <div class="form-group has-feedback">
         <asp:TextBox ID="txtRePassword" TextMode="Password" CssClass="form-control validate[required,equals[txtPassword]]" placeholder="Re Password" runat="server"></asp:TextBox>
        <span class="glyphicon glyphicon-lock form-control-feedback"></span>
      </div>

      <div class="row">
        <div class="col-xs-6 text-left">
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Default.aspx"> <i class="fa fa-arrow-left"></i> Back to the Login</asp:HyperLink>
        </div>
        <!-- /.col -->
        <div class="col-xs-6">
        <asp:Button ID="btnSubmit" CssClass="btn btn-info btn-block btn-flat" runat="server" Text="Submit" />  
        </div>          
        <!-- /.col -->
      </div>  
  </div>
  <!-- /.login-box-body -->
</div>
    </form>
    
    <script src="https://netdna.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="Assets/jQuery_Validation/css/validationEngine.jquery.css" type="text/css"/>
    <script type="text/javascript" src="Assets/jQuery_Validation/js/languages/jquery.validationEngine-en.js" charset="utf-8"></script>
    <script type="text/javascript" src="Assets/jQuery_Validation/js/jquery.validationEngine.js" charset="utf-8"></script>
    <script src="Assets/jquery.toaster.js"></script>
    <script src="Assets/Master.js"></script>
        <script type="text/javascript">
        $(function () {
            $("#form1").validationEngine();
        });
    </script>
</body>
</html>