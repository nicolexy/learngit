<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WechatMerchantTrans.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.WebchatPay.WechatMerchantTrans" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
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
					<td style="width: 100%" bgcolor="#e4e5f7" colspan="2"><font color="red"><img src="../images/page/post.gif" width="20" height="16">微信/财付通商户信息互查</font>
						</td>
				</tr>
				<tr>
                    <td>账号:<asp:TextBox ID="txtAccId" Width="400px" runat="server"></asp:TextBox></td>
					<td align="center"><asp:button id="btnQuery" runat="server" width="80px" text="查 询" 
                            onclick="btnQuery_Click"></asp:button></td>
				</tr>
                <tr>
                    <td align="left" colspan="2">
                       财付通商户号：
                       <asp:label id="lbSpid" runat="server"></asp:label>
                       
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                       微信支付商户号：
                       <asp:label id="lbMchid" runat="server"></asp:label>
                    </td>
                </tr>
                

			</table>
            
    </div>
    </form>
</body>
</html>
