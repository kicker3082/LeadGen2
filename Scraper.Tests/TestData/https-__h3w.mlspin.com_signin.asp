<!-- Latest compiled and minified CSS -->
<LINK href="/css/Signin.css" type=text/css rel=stylesheet>

<script language='javascript'> window.sessionStorage.clear();</script>
<html>
<head>
    <meta name="viewport" content="width=device-width,initial-scale=1,maximum-scale=1,user-scalable=no, shrink-to-fit=no">
    <meta http-equiv="CACHE-CONTROL" content="NO-CACHE">
    <meta http-equiv="expires" content="0">
    <title>pinergy - Sign In</title>
    <link href="/style.asp" type="text/css" rel="stylesheet">

    <script language="JavaScript">
        
        var ath; ath = {};
        var isMobile = function() {
            return /(iphone|ipod|android|blackberry|windows ce|palm|symbian|nexus 7|xoom|windows phone)/i.test(navigator.userAgent);
        };

        var isIPad = function(){
            return /(ipad)/i.test(navigator.userAgent);
        }
        function CheckSavePassword() {
            if (document.loginform.SavePassword.checked) {
                document.loginform.SavePassword.checked = false;
            } else {
                document.loginform.SavePassword.checked = true;
            }
        }
        function mobileRedirect(param){
            var strUrl;
            strUrl = 'http://mobile.mlspin.com/login.aspx';
            if(param){
                if(document.getElementById("RemMe").checked == true){
                    try {
                        localStorage.setItem('gomobile','true');
                        ath.hasLocalStorage = true;
                    } catch (e) {
                        // we are most likely in private mode
                        ath.hasLocalStorage = false;
                    }
                }
                window.location.href=strUrl;
            }
            else{
                if(document.getElementById("RemMe").checked == true){
                    try {
                        localStorage.setItem('gomobile','false');
                        ath.hasLocalStorage = true;
                    } catch (e) {
                        // we are most likely in private mode
                        ath.hasLocalStorage = false;
                    }
                }
                document.getElementById("mobile").style.display="none";
            }
        }

        function parseQueryString(queryString) {
            var QueryString = {};
            queryString = queryString.slice(queryString.indexOf("?") + 1);
            var qsArray = queryString.split("&");
            for (var i = 0; i < qsArray.length; i++) {
                var arr = qsArray[i].split("=");
                QueryString[arr[0]] = arr[1];
            }
            return QueryString;
        }

        if (window != top) {
            top.location.href = location.href;
        }
    </script>
    <style>
        body {
            padding:0;
        }
        INPUT.login {
            height: 22px;
            border: 1px solid #808080;
            padding: 2px 4px;
            background-image: url('images/bg_input.gif');
        }
        
       
        .mobile {
            padding-top: 15px;
            padding-bottom: 15px;
            padding-left: 15px;
            padding-right:15px;
            align-content: center;
            align-self: center;
            width: 90%;
            height: auto;
            display: none;
            text-align: center;
            border:4px solid #E7E7E7;
            border-radius: 15px;
          
            color:#444;
            margin: auto;
            background: -moz-linear-gradient(top, #FFFFFF, #E7E7E7);
            background: -ms-linear-gradient(#FFFFFF, #E7E7E7);
            background: -webkit-gradient(linear, left top, left bottom, from(#FFFFFF), to(#E7E7E7));
        }
     
        .mobileButton {
	     
	        font-weight:bold;
	        
	        border-top: 1px outset grey;
	        border-left: 1px outset grey;
	       
    	     -moz-border-radius:10px 10px 10px 10px;
	        -webkit-border-radius:10px 10px 10px 10px;
            border-radius:10px 10px 10px 10px;
	        -webkit-appearance: none;
	        -moz-appearance:none;
	        min-height:2rem;
	        /*width:90px;
	        
	        height:54px;
	        font-family:Arial;
            font-size:x-large;*/
           

        }
        .yes{   background-color:#FBAF41;}
        .no {
               background-color:#BEBEBE;
        }

        .mobileCheckTable {
            font-family: -apple-system,BlinkMacSystemFont,Segoe UI,Roboto,Helvetica Neue,Arial,sans-serif !important;
        }

        .mobileCheckTable td
        {
             vertical-align:top;
        }
        .textContent {
            font-size: .9rem;
            text-align:center;
            font-weight:500;
        }
        .textContent1Child
        {
            white-space:nowrap;
            margin-bottom:1rem;
        }
        .mobilecheck {
            height: 1.2rem;
            width: 1.2rem;
            vertical-align: top;
            padding: 0;
            border: 1px solid rgba(0, 0, 0, 0.3);
        }


        .mobilequest
        {
            margin:0 auto;
            max-width:20rem;
            box-sizing:border-box;
            
        }

        div#remember {
            font-size:0.88rem;
        }
        @media only screen and (min-width: 768px) {
            .mobilequest.textContent {
                font-size: 1.5rem !important;
            }
            .textContent1Child {
                font-size: 1.2rem !important;
            }
        }
        @media only screen and (min-width: 1024px) {
            .mobilequest.textContent {
                font-size: 1.6rem !important;
            }
            .textContent1Child {
                font-size: 1.2rem !important;
            }
        }
    </style>
    <link rel="icon" href="favicon.ico" type="image/x-icon" />
    <link rel="shortcut icon" href="favicon.ico" type="image/x-icon" />

</head>
<body marginheight="0" marginwidth="0" topmargin="0" leftmargin="0"  >

    <table height="100%" width="100%" cellpadding="0" cellspacing="0" border="0" >
        <tr>
            <td style="padding-top: 10px;">
                <div id="mobile" class="mobile">
                    <table width="100%" border="0" class="mobileCheckTable">
                        <tr>
                            <td width="20%" class="textContent"><img src="images/pinergy_mobile_app_icon_redirect.png" style="width:100%" /></td>
                            <td style="max-width:10rem;"><div class="mobilequest textContent">Would you like to be redirected to Pinergy Mobile?</div>
                            </td>
                            <td width="20%">
                                <div class="textContent">
                                    <div class="textContent1Child">
                                        <input type="button" value="Yes" onclick="mobileRedirect(true)" class="mobileButton yes" />&nbsp;
                                        <input type="button" value="No" onclick="mobileRedirect(false)" class="mobileButton no" />
                                    </div>
                                    <br />
                                    
                                    
                                </div>
                                
                                <div style="clear:both;"/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align:center;"><div id="remember" style="nowrap"><input type="checkbox" id="RemMe" name="RemMe" class="mobilecheck" /> Remember my choice</div></td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td width="100%" height="100%" >
                <form name="loginform" method="POST" action="validate_new.asp" style="margin: 0px">
                    <input name="cmwtnsxjvalidcmjgqfnv" value="DT: 9/2/2019 11:00:30 AM" type="hidden">
                    <input name="Page_Loaded" value="DT: 9/2/2019 11:00:30 AM" type="hidden">
                    <table border="0" cellspacing="0" cellpadding="0" class="mls-login">
                        
                        <tr>
                            <td ><img src="images/MLSPIN_Logo.jpg" width="240" height="60" /> </td>
                        </tr>
                        <tr>
                            <td class="text-left pl-4"><span class="h4">Sign In to</span>
                                <img src="images/pinergy-logo.jpg" width="90" height="30" /></td>
                        </tr>
                        <tr>
                            
                            <td class="pl-2">
                                <input class="form-control" type="text" style="width: 220px;" name="user_name" maxlength="8" 
                                    value="" placeholder="Enter Your Agent ID"></td>
                        </tr>
                        <tr>
                            <td class="pl-2" >
                                <input class="form-control" type="password" style="width: 220px" name="pass" maxlength="20" 
                                     placeholder="Password">
                            </td>
                        </tr>
                         <tr>
                            <td class="text-left pl-2">
                                <a href="/MLS.Pinergy/auth/password/forgotpassword">Forgot your password?</a>
                            </td>
                        </tr>
                        <tr>
                            
                            <td class="text-left pl-2 mls-login-rem-me">
                                <input type="checkbox" name="SavePassword" value="Y" ><span onclick="CheckSavePassword();"> Remember My Password</span></td>
                        </tr>
                        <tr>
                            
                            <td>
                                <button class="btn btn-sm btn-primary" type="submit">Sign In</button>
                                <!--<input class="btn btn-sm btn-primary" type="submit" value="Sign In" name="signin"></td>-->
                        </tr>
                    </table>
                </form>
            </td>
        </tr>
        <tr>
            <td>
                

<footer class="mls-site-footer">
    <div class="footer-content">

        <div class="footer-icon MLSPINlogo mr-1"></div>

        <div class="mb-1">&copy; <span>MLS Property Information Network, Inc.</span></div>

        <div class="vert-bar">|</div>
        <div>
            904 Hartford Turnpike, Shrewsbury, MA 01545

        </div>
        <div class="vert-bar">|</div>
        <div>
            800-695-3000 
           
        </div>
        
        <div class="vert-bar">|</div>
        <div class="footer-content-group">
            <div class="d-inline">
                <a href="http://www.mlspin.com/downloads/WebsiteAccessNoticeForm.pdf" target="_blank">Access Notice</a>
            </div>
            <div class="vert-bar d-inline">|</div>
            <div class="d-inline">
                <a href="http://www.mlspin.com/privacy_policy.aspx" target="_blank">Privacy Policy</a>
            </div>
            <div class="vert-bar d-inline">|</div>
            <div class="d-inline">
                <a href="http://www.mlspin.com/copyright_policy.aspx" target="_blank">Copyright Policy</a>
            </div>
        </div>

        <div class="vert-bar">|</div>
        <div class="footer-user-count">1724 users online right now!</div>
    </div>
</footer>

            </td>
        </tr>
    </table>
    <script language="JavaScript">
        document.loginform.user_name.focus();
        
    </script>
    <script>
   if(isMobile() || isIPad()){
      if(localStorage.getItem("gomobile") == null ){
          document.getElementById("mobile").style.display="block";
          document.getElementById("remember").style.display="block";
       }
       else{
           if(localStorage.getItem("gomobile") == "true"){
              var QueryString,strUrl;
              QueryString = parseQueryString(window.location.search);
              if(QueryString.fs != null){
                 localStorage.removeItem("gomobile");
                 document.getElementById("mobile").style.display="block";
                 document.getElementById("remember").style.display="block";
              }
              else{
                strUrl = 'http://mobile.mlspin.com/login.aspx';
                window.location.href=strUrl;
              }
           } 
       }
   }
   if(isIPad()){
       document.getElementById("remember").style.display="none";
   }
    </script>
</body>
</html>
