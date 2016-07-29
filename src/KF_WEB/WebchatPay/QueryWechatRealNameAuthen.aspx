<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryWechatRealNameAuthen.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.WebchatPay.QueryWechatRealNameAuthen" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
<head runat="server">
    <title>微信/财付通商户信息互查</title>
    <style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
</head>
<body>
    <form id="formMain" runat="server">
    <div>
        <table border="1" cellspacing="1" cellpadding="1" width="800px">
				<tr>
					<td style="width: 100%" bgcolor="#e4e5f7" colspan="3"><font color="red"><img src="../images/page/post.gif" width="20" height="16">微信支付实名认证查询</font>
						</td>
				</tr>
				<tr>
                    <td>微信财付通账号:<asp:TextBox ID="txtAccId" Width="200px" runat="server"></asp:TextBox></td>
                    <td>快捷绑定序列号:<asp:TextBox ID="txtSerialno" Width="200px" runat="server"></asp:TextBox></td>
					<td align="center"><asp:button id="btnQuery" runat="server" width="80px" text="查 询" 
                            onclick="btnQuery_Click"></asp:button></td>
				</tr>
                <tr>
                    <td align="left" colspan="3">
                       认证状态：
                       <asp:label id="lbAuthenState" runat="server"></asp:label>
                       
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="3">
                       失败原因：
                       <asp:label id="lbFailInfo" runat="server"></asp:label>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="3">
                       认证账号：
                       <asp:label id="lbAuthenId" runat="server"></asp:label>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="3">
                       银行类型：
                       <asp:label id="lbBankType" runat="server"></asp:label>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="3">
                       身份证号码：
                       <asp:label id="lbCerid" runat="server"></asp:label>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="3">
                       认证通过时间：
                       <asp:label id="lbPassTime" runat="server"></asp:label>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="3">
                       最后修改时间：
                       <asp:label id="lbModifyTime" runat="server"></asp:label>
                    </td>
                </tr>

			</table>
            
    </div>
    </form>
</body>
</html>
