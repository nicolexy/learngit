<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="defaultNotice.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.defaultNotice" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!--<script src="https://code.jquery.com/jquery-3.0.0.min.js"></script>-->
    <script src="../SCRIPTS/jquery-3.0.0.min.js"></script>
    <!--<script src="../SCRIPTS/jquery-1.11.3.min.js"></script>-->
    <style type="text/css">
        .black_overlay
        {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
            min-height: 100%;
            min-width: 100%;
            background-color: gray;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: .80;
            filter: alpha(opacity=80);
        }

        .white_content
        {
            display: none;
            position: absolute;
            top: 25%;
            left: 33%;
            width: 600px;
            height: 400px;
            border: 1px solid lightblue;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }

        .white_content_small
        {
            display: none;
            position: absolute;
            top: 20%;
            left: 30%;
            width: 40%;
            height: 50%;
            border: 1px solid lightblue;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }

        .yuanjiao
        {
            font-family: 'Microsoft YaHei';
            border: 2px solid red;
            border-radius: 20px;
            padding: 30px 30px;
            width: 600px;
        }

        .btn
        {
            line-height: 31px;
            height: 31px;
            width: 76px;
            color: #ffffff;
            background-color: #ededed;
            font-size: 16px;
            font-weight: normal;
            font-family: Arial;
            background: -webkit-gradient(linear, left top, left bottom, color-start(0.05, #c62d1f), color-stop(1, #f24437));
            background: -moz-linear-gradient(top, #c62d1f 5%, #f24437 100%);
            background: -o-linear-gradient(top, #c62d1f 5%, #f24437 100%);
            background: -ms-linear-gradient(top, #c62d1f 5%, #f24437 100%);
            background: linear-gradient(to bottom, #c62d1f 5%, #f24437 100%);
            background: -webkit-linear-gradient(top, #c62d1f 5%, #f24437 100%);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#c62d1f', endColorstr='#f24437',GradientType=0);
            border: 1px solid #d02718;
            -webkit-border-top-left-radius: 18px;
            -moz-border-radius-topleft: 18px;
            border-top-left-radius: 18px;
            -webkit-border-top-right-radius: 18px;
            -moz-border-radius-topright: 18px;
            border-top-right-radius: 18px;
            -webkit-border-bottom-left-radius: 18px;
            -moz-border-radius-bottomleft: 18px;
            border-bottom-left-radius: 18px;
            -webkit-border-bottom-right-radius: 18px;
            -moz-border-radius-bottomright: 18px;
            border-bottom-right-radius: 18px;
            -moz-box-shadow: 3px 4px 0px 0px #8a2a21;
            -webkit-box-shadow: 3px 4px 0px 0px #8a2a21;
            box-shadow: 3px 4px 0px 0px #8a2a21;
            text-align: center;
            display: inline-block;
            text-decoration: none;
        }

            .btn:hover
            {
                background-color: #f5f5f5;
                background: -webkit-gradient(linear, left top, left bottom, color-start(0.05, #f24437), color-stop(1, #c62d1f));
                background: -moz-linear-gradient(top, #f24437 5%, #c62d1f 100%);
                background: -o-linear-gradient(top, #f24437 5%, #c62d1f 100%);
                background: -ms-linear-gradient(top, #f24437 5%, #c62d1f 100%);
                background: linear-gradient(to bottom, #f24437 5%, #c62d1f 100%);
                background: -webkit-linear-gradient(top, #f24437 5%, #c62d1f 100%);
                filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#f24437', endColorstr='#c62d1f',GradientType=0);
            }
    </style>
    <script type="text/javascript">      
        $(function () {           
            $("#btn_Sure").css("display", "none");
            $.ajax({
                type: 'get',
                url: "defaultNotice.aspx?getAction=GetCookie",
                dataType: "text",
                success: function (data) {
                    if (data == "True") {
                        return;
                    }
                    else {
                        ShowDiv("div_Notice", "back");
                    }
                }
            });
            $("#cb_CheckNotice").click(function () {

                if ($("#cb_CheckNotice").is(':checked')) {
                    $("#btn_Sure").css("display", "block");
                    //$("#btn_Sure").removeAttr("disabled");
                }
                else {
                    $("#btn_Sure").css("display", "none");
                }
            })

            $("#btn_Sure").click(function () {
                $.ajax({
                    type: 'get',
                    url: "defaultNotice.aspx?getAction=SetCookie",
                    dataType: "text",
                    success: function (data) {
                        var dataObj = eval('(' + data + ')');                      
                        $.each(dataObj, function (idx, item) {
                            var cookie = item.cookie;
                            var requestUrl = item.requestUrl;                            
                            if (cookie != null)
                            {
                                CloseDiv("div_Notice", "back");
                                if (requestUrl != null)
                                {
                                    location.href = "http://kf.cf.com"+requestUrl;
                                }
                            }                            
                        })
                    }
                });
                //CloseDiv("div_Notice", "back");
                //register("xiaolin");
            });
        })
        //弹出隐藏层
        function ShowDiv(show_div, bg_div) {
            document.getElementById(show_div).style.display = 'block';
            document.getElementById(bg_div).style.display = 'block';
            //$("#" + show_div).css("display", "block");
            //$("#" + bg_div).css("display", "block");
            var bgdiv = document.getElementById(bg_div);
            bgdiv.style.width = document.body.scrollWidth;
            bgdiv.style.height = $(document).height();
            //$("#" + bg_div).width($(document).body.scrollWidth());           
            $("#" + bg_div).height($(document).height());

        };
        //关闭弹出层
        function CloseDiv(show_div, bg_div) {
            document.getElementById(show_div).style.display = 'none';
            document.getElementById(bg_div).style.display = 'none';
        };
    </script>
</head>
<body style="margin:0px">
    <div style="overflow-y:hidden">
        <iframe id="ifr_Main" frameborder="0" src="default.aspx" style="width: 100%; height: 100%;"></iframe>
    </div>
    <div id="back" class="black_overlay"></div>
    <div id="div_Notice" class="white_content  yuanjiao" style="font-family: 'Microsoft YaHei'">
        <span style="color: red; font-size: 250%; font-weight: bolder">警告</span>
        <hr style="height: 1px; border: none; border-top: 1px solid  #cccccc;" />
        <p style="padding: 3px 20px 3px 20px">您正在访问的kf.cf.com页面上的任何操作（包括数据和资金的增删改查）都是敏感操作，请仔细阅读如下内容：</p>
        <p style="padding: 3px 20px 3px 20px">1.禁止任何非业务需要的滥用本站点，违者<span style="color: red; font-size: x-large">高压线</span>处置！</p>
        <p style="padding: 3px 20px 3px 20px">2.禁止任何形式（如：截图外传，微信QQ同事等途径暴露传播数据，与他人分享或炫耀等）的外传和泄漏，违者<span style="color: red; font-size: x-large">高压线</span>处置！</p>
        <p style="padding: 3px 20px 3px 20px">3.所有操作都会有<span style="color: red; font-size: x-large">记录</span>和<span style="color: red; font-size: x-large">审计</span>，请勿触犯<span style="color: red; font-size: x-large">高压线</span>！</p>

        <div style="text-align: center">
            <input type="checkbox" id="cb_CheckNotice" />
            <span>同意以上内容</span>
            <br />
            <br />
            <button id="btn_Sure" class="btn" style="margin-left: 270px">确定</button>
            <%--background-color: #cc4125; --%>
        </div>
    </div>
</body>
</html>
