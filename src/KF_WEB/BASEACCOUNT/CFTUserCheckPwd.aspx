<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CFTUserCheckPwd.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.CFTUserCheckPwd" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>CFTUserCheck</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }

        .watermark_wrap span {
            z-index: 999;
            position: absolute;
            color:#808080;
            font-size:14px;
            -moz-transform: rotate(0deg) scale(1.00,1.00) translate(0px,0px) skew(0deg,-12deg);
            -moz-transform-origin: 0% 0%;
            -webkit-transform: rotate(0deg) scale(1.00,1.00) translate(0px,0px) skew(0deg,-12deg);
            -webkit-transform-origin: 0% 0%;
            -o-transform: rotate(0deg) scale(1.00,1.00) translate(0px,0px) skew(0deg,-12deg);
            -o-transform-origin: 0% 0%;
            transform: rotate(0deg) scale(1.00,1.00) translate(0px,0px) skew(0deg,-12deg);
            transform-origin: 0% 0%;
            -moz-user-select: none;
            -webkit-user-select: none;
            -ms-user-select: none;
            -khtml-user-select: none;
            user-select: none;
            -moz-opacity:0.5;
			-khtml-opacity: 0.5;
			opacity: 0.5;
        }
		</style>
    <script type="text/javascript">
        function showTips_2() {
            if (document.getElementById("cb_2").checked) {
                document.getElementById("div_cbxl2").style.display = "";
            }
            else {
                document.getElementById("div_cbxl2").style.display = "none";
            }
        }

        function shotTips_3() {
            if (document.getElementById("cb_3").checked) {
                document.getElementById("div_cbx_3").style.display = "";
            }
            else {
                document.getElementById("div_cbx_3").style.display = "none";
            }
        }

        function Init() {
            showTips_2();
            shotTips_3();
            watermark("watermark_wrap", "<%=uid%>");
            }

            function watermark(wrapClass, text) {
                var wrap = document.createElement("div");
                wrap.className = wrapClass;
                var page_width = Math.max(document.body.scrollWidth, document.body.clientWidth);
                var page_height = Math.max(document.body.scrollHeight, document.body.clientHeight);
                for (var i = 20; i < page_width; i += 300) {
                    for (var j = 20; j < page_height; j += 100) {
                        var node = document.createElement("span");
                        node.style.top = j;
                        node.style.left = i;
                        node.textContent = node.innerText = text;
                        wrap.appendChild(node);
                    }
                }
                document.body.appendChild(wrap);
                window.onresize = window.onload = function () {
                    wrap.parentElement.removeChild(wrap);
                    watermark(wrapClass, text);
                };
            }


    </script>
</head>
<body onload="Init()" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <table border="0" cellspacing="1" cellpadding="0" width="100%" bgcolor="#000000">
            <tr bgcolor="#ffffff">
                <td bgcolor="#eeeeee" height="20" background="IMAGES/Page/bk_white.gif" width="50%"
                    colspan="2" align="center">后台资料(
						<asp:Label ID="lblfid" runat="server" ForeColor="Red"></asp:Label>)</td>
                <td bgcolor="#eeeeee" background="IMAGES/Page/bk_white.gif" width="50%" colspan="2"
                    align="center">用户提交资料(
						<asp:Label ID="labFTypeName" runat="server" ForeColor="Red"></asp:Label>)</td>
            </tr>
            <tr bgcolor="#ffffff">
                <td bgcolor="#eeeeee" height="20" width="25%">&nbsp;&nbsp;&nbsp;&nbsp;用户帐号</td>
                <td width="25%">&nbsp;
						<asp:Label ID="labFQQid" runat="server" ForeColor="#8080FF"></asp:Label></td>
                <td width="25%">&nbsp;
						<asp:Label ID="labIsAnswer" runat="server" ForeColor="#00C000"></asp:Label></td>
                <td bgcolor="#eeeeee" width="25%">&nbsp;&nbsp;&nbsp;&nbsp;原密保答案</td>
            </tr>
            <tr bgcolor="#ffffff">
                <td bgcolor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;证件号码</td>
                <td>&nbsp;
						<asp:Label ID="labFcreid" runat="server" ForeColor="#8080FF"></asp:Label></td>
                <td>&nbsp;
						<asp:Label ID="cre_id" runat="server" ForeColor="#00C000"></asp:Label></td>
                <td bgcolor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;证件号码</td>
            </tr>
            <tr bgcolor="#ffffff">
                <td bgcolor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;证件类型</td>
                <td>&nbsp;
						<asp:Label ID="labFcre_type" runat="server" ForeColor="#8080FF"></asp:Label></td>
                <td>&nbsp;
						<asp:Label ID="cre_type" runat="server" ForeColor="#00C000"></asp:Label></td>
                <td bgcolor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;证件类型</td>
            </tr>
            <tr bgcolor="#ffffff">
                <td bgcolor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;邮箱地址</td>
                <td>&nbsp;
						<asp:Label ID="labFEmail" runat="server" ForeColor="#8080FF"></asp:Label></td>
                <td>&nbsp;
						<asp:Label ID="email" runat="server" ForeColor="#00C000"></asp:Label></td>
                <td bgcolor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;邮箱地址</td>
            </tr>
            <tr bgcolor="#ffffff">
                <td bgcolor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;名称</td>
                <td>&nbsp;
						<asp:Label ID="labFtruename" runat="server" ForeColor="#8080FF"></asp:Label></td>
                <td>&nbsp;
						<asp:Label ID="old_name" runat="server" ForeColor="#00C000"></asp:Label></td>
                <td bgcolor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;名称</td>
            </tr>
            <tr bgcolor="#ffffff">
                <td bgcolor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;总金额</td>
                <td>&nbsp;
						<asp:Label ID="labFbalance" runat="server" ForeColor="#8080FF"></asp:Label></td>
                <td>&nbsp;
						<asp:Label ID="new_name" runat="server" ForeColor="#00C000"></asp:Label></td>
                <td bgcolor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;新名称</td>
            </tr>
            <tr bgcolor="#ffffff">
                <td bgcolor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;冻结金额</td>
                <td>&nbsp;
						<asp:Label ID="labFCon" runat="server" ForeColor="#8080FF"></asp:Label></td>
                <td>&nbsp;
						<asp:Label ID="clear_pps" runat="server" ForeColor="#00C000"></asp:Label></td>
                <td bgcolor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;是否清空密保资料</td>
            </tr>
            <tr bgcolor="#ffffff">
                <td bgcolor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;银行帐号</td>
                <td>&nbsp;
						<asp:Label ID="labFBankAcc" runat="server" ForeColor="#8080FF"></asp:Label></td>
                <td>&nbsp;
						<asp:Label ID="lblBindMobileUser" runat="server" ForeColor="#00C000"></asp:Label></td>
                <td bgcolor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;手机</td>
            </tr>
            <tr bgcolor="#ffffff">
                <td bgcolor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;免审核标准分</td>
                <td>&nbsp;
						<asp:Label ID="lblstandard_score" runat="server" ForeColor="#8080FF"></asp:Label></td>
                <td>&nbsp;
						<asp:Label ID="lblBindMailUser" runat="server" ForeColor="#00C000"></asp:Label></td>
                <td bgcolor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;绑定邮箱</td>
            </tr>
            <tr bgcolor="#ffffff">
                <td bgcolor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;实际得分</td>
                <td>&nbsp;
						<asp:Label ID="lblscore" runat="server" ForeColor="#8080FF"></asp:Label></td>
                <td>&nbsp;
						<asp:Label ID="labFstatename" runat="server" ForeColor="#00C000"></asp:Label></td>
                <td bgcolor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;审批当前状态</td>
            </tr>
            <tr bgcolor="#ffffff">
                <td bgcolor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;得分明细</td>
                <td>&nbsp;
						<asp:Label ID="lbldetail_score" runat="server" ForeColor="#8080FF"></asp:Label></td>
                <td>&nbsp;
						<asp:Label ID="new_cre_id" runat="server" ForeColor="#00C000"></asp:Label></td>
                <td bgcolor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;新证件号码</td>
            </tr>
            <tr bgcolor="#ffffff">
                <td bgcolor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;风控标记</td>
                <td>&nbsp;
						<asp:Label ID="lblrisk_result" runat="server" ForeColor="#8080FF"></asp:Label></td>
                <td>&nbsp;
						<asp:Label ID="lbauthenState" runat="server" ForeColor="#00C000"></asp:Label></td>
                <td bgcolor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;实名认证</td>
            </tr>
            <tr bgcolor="#ffffff">
                <td bgcolor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;IVR呼叫结果</td>
                <td colspan="3">&nbsp;
						<asp:Label ID="lblivrresult" runat="server" ForeColor="#8080FF"></asp:Label></td>
            </tr>
        </table>
        <hr size="1">
        <table border="0" cellspacing="1" cellpadding="0" width="100%" bgcolor="#000000">
            <tr bgcolor="#ffffff">
                <td bgcolor="#eeeeee" height="20" background="IMAGES/Page/bk_white.gif" width="50%"
                    align="center">申诉原因</td>
                <td bgcolor="#eeeeee" background="IMAGES/Page/bk_white.gif" width="50%" align="center">拒绝原因</td>
            </tr>
            <tr bgcolor="#ffffff">
                <td valign="top">                
                        <asp:TextBox ID="tbReason" runat="server" Width="100%" Height="74px" TextMode="MultiLine" Enabled="False"
                            BorderStyle="Groove" ReadOnly="True"></asp:TextBox></td>            
                <td valign="top">
                    <asp:TextBox ID="tbFCheckInfo" runat="server" Width="100%" Height="74px" TextMode="MultiLine"></asp:TextBox></td>
            </tr>
            <tr bgcolor="#ffffff">
                <td bgcolor="#eeeeee" background="IMAGES/Page/bk_white.gif" width="100%" colspan="2"
                    align="center">备注</td>
            </tr>
            <tr bgcolor="#ffffff">
                <td width="100%" colspan="2">
                    <asp:TextBox ID="tbComment" runat="server" Width="100%" Height="74px" TextMode="MultiLine" BorderStyle="Groove"></asp:TextBox></td>
            </tr>
        </table>
        <table border="0" cellspacing="0" cellpadding="0" width="100%">
            <tr>
                <td height="30" align="center">
                    <asp:Button ID="btSetRealName" Width="85px" Text="通过，置实名" runat="server" Visible="False" OnClick="btSetRealName_Click"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:Button ID="btOK" runat="server" Width="85px" Text="通过" Visible="False" OnClick="btOK_Click"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:Button ID="btCancel" runat="server" Width="85px" Text="拒绝" OnClick="btCancel_Click"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:Button ID="btnDel" runat="server" Width="85px" Text="删除" Visible="False" OnClick="btnDel_Click"></asp:Button></td>
            </tr>
        </table>
    </form>
</body>
</html>
