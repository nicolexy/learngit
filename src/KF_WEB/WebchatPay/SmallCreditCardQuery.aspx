<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SmallCreditCardQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.WebchatPay.SmallCreditCardQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>小额刷卡查询</title>
    <style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
</head>
<body>
    <form id="formMain" runat="server">
        <table border="1" cellspacing="1" cellpadding="1" width="1100">
				<tr>
					<td style="width: 100%" bgcolor="#e4e5f7" colspan="5"><font color="red"><img src="../images/page/post.gif" width="20" height="16">小额刷卡查询</font>
						</td>
				</tr>
				<tr>
                    <td>微信号码：<asp:TextBox ID="txtWechatName" Width="200px" runat="server"></asp:TextBox></td>
                    <td>交易单号：<asp:TextBox ID="txtTradeId" Width="200px" runat="server"></asp:TextBox></td>
					<td align="center"><asp:button id="btnQuery" runat="server" width="80px" text="查 询" 
                            onclick="btnQuery_Click"></asp:button></td>
				</tr>
                <tr>
                    <td align="left" >
                       开户时间：
                       <asp:label id="lblCreateTime" runat="server"></asp:label>
                       
                    </td>
                    <td align="left" >
                       刷卡账号：
                       <asp:label id="lblAccountNo" runat="server"></asp:label>
                    </td>
                    <td align="left" >
                       账户状态：
                       <asp:label id="lblAccountState" runat="server"></asp:label>
                    </td>
                <tr>
                    <td align="left" colspan="3" >
                       当前额度：
                       <asp:label id="lblBanlance" runat="server"></asp:label>
                    </td>
                </tr>

			</TABLE>
            <br/>
            <table border="1" cellSpacing="0" cellPadding="0" width="1100">
                <tr>
					<td style="width: 100%" bgcolor="#e4e5f7" colspan="5"><font color="red"><img src="../images/page/post.gif" width="20" height="16">交易详情</font>
						</td>
				</tr>
				<tr>
					<td valign="top">
                        <asp:HiddenField ID="hfCurrentCollectionNo" runat="server" />
                        <asp:HiddenField ID="hfCurrentCollectionCreateTime" runat="server" />
                        <asp:gridview id="DataGrid1" runat="server" width="1100px" itemstyle-horizontalalign="center"
							headerstyle-horizontalalign="center" horizontalalign="center" pagesize="5" autogeneratecolumns="false"
							gridlines="horizontal" cellpadding="1" backcolor="white" borderwidth="1px" borderstyle="none" bordercolor="#e7e7ff">
							<footerstyle forecolor="#4a3c8c" backcolor="#b5c7de"></footerstyle>
							<headerstyle font-bold="true" horizontalalign="center" forecolor="#f7f7f7" backcolor="#4a3c8c"></headerstyle>
							<columns>
								<asp:boundfield datafield="Fcreate_time" headertext="日期">
									<headerstyle width="150px" horizontalalign="center"></headerstyle>
								</asp:boundfield>
                                <asp:boundfield datafield="Flistid" headertext="订单号">
									<headerstyle width="200px"></headerstyle>
								</asp:boundfield>
								<asp:boundfield datafield="Fpaynum_str" headertext="扣费金额">
									<headerstyle width="100px"></headerstyle>
								</asp:boundfield>
                                <asp:boundfield datafield="Ftrade_state_str" headertext="状态">
									<headerstyle width="80px"></headerstyle>
								</asp:boundfield>
								<asp:boundfield datafield="Fsale_name" headertext="商户名称">
									<headerstyle width="150px"></headerstyle>
								</asp:boundfield>
                                <%--<asp:boundfield datafield="Fmemo" headertext="商品备注">
									<headerstyle width="200px"></headerstyle>
								</asp:boundfield>--%>
                                <asp:boundfield datafield="Fbank_id_str" headertext="银行卡号">
									<headerstyle width="50px"></headerstyle>
								</asp:boundfield>
                                <asp:boundfield datafield="Fbank_type_str" headertext="银行类型">
									<headerstyle width="100px"></headerstyle>
								</asp:boundfield>
							</columns>
						</asp:gridview>
                        <webdiyer:aspnetpager id="pager" runat="server" 
                            horizontalalign="right" numericbuttoncount="5" pagingbuttonspacing="0"
							showinputbox="always" cssclass="mypager" submitbuttontext="转到" 
                            numericbuttontextformatstring="[{0}]" alwaysshow="true" 
                            onpagechanged="ChangePage"></webdiyer:aspnetpager></td>
				</tr>
			</table>
    </form>
</body>
</html>
