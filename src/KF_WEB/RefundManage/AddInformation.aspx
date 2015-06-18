<%@ Page language="c#" Codebehind="AddInformation.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RefundManage.AddInformation" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>AddInformation</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); 
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
	.style5 { COLOR: #ff0000 }
		    .style6
            {
                height: 20px;
            }
            .lbStyle
            {
                width:250px;
            }
		</style>
		<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT>
			<br>
			<table height="108" cellSpacing="1" cellPadding="0" width="860" align="center" bgColor="#666666"
				border="0">
				<tr background="../IMAGES/Page/bg_bl.gif">
					<td style="HEIGHT: 16px" vAlign="middle" colSpan="4" height="16">
						<table height="25" cellSpacing="0" cellPadding="1" width="859" border="0">
							<tr>
								<td style="HEIGHT: 20px" width="80%" background="../IMAGES/Page/bg_bl.gif"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
										<asp:label id="lbHeadID" runat="server">特殊退款信息登记</asp:label></font>
								</td>
								<td style="HEIGHT: 20px" width="20%" background="../IMAGES/Page/bg_bl.gif">操作员代码: <span class="style5">
										<asp:label id="Label1" runat="server">Label</asp:label></span></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr bgColor="#ffffff" >
                    <td style="HEIGHT: 20px" bgColor="#eeeeee" nowrap><FONT face="宋体">财付通订单号：</FONT></td>
					<td colSpan="2">&nbsp;
						<asp:textbox id="tbUinID" runat="server" Width="700px" height="50px" BorderWidth="1px"  TextMode="MultiLine"  Enabled ="false"></asp:textbox>
					</td>
		
				</tr>
				<TR bgColor="#ffffff">
					<TD style="HEIGHT: 20px" bgColor="#eeeeee" nowrap><FONT face="宋体">退款类型：</FONT></TD>
					<TD style="HEIGHT: 20px" colSpan="2">&nbsp;
						<asp:textbox id="tbRefundType" runat="server" Width="700px" height="50px" BorderWidth="1px"  TextMode="MultiLine" Enabled ="false"></asp:textbox>
                    </TD>
				</TR>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee"><FONT face="宋体" nowrap>银行订单号：</FONT></td>
					<td colSpan="2">&nbsp;
					<asp:textbox id="tbBankListId" runat="server" Width="700px" height="50px" BorderWidth="1px"  TextMode="MultiLine" Enabled ="false"></asp:textbox>	
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee"><FONT face="宋体" nowrap>身份证：</FONT></td>
					<td colSpan="2">&nbsp;
						<asp:textbox id="txtIdentity" runat="server" Width="250px" BorderWidth="1px" ></asp:textbox>
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td style="HEIGHT: 20px" bgColor="#eeeeee" nowrap><FONT face="宋体">原卡号：</FONT></td>
					<td style="HEIGHT: 20px" colSpan="2">&nbsp;
						<asp:textbox id="txtInitBankAccNo" runat="server" Width="250px" BorderWidth="1px"></asp:textbox>&nbsp;	
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" class="style6" nowrap><FONT face="宋体">原银行类型：</FONT></td>
					<td colSpan="2" class="style6">&nbsp;						
                        
                        <asp:DropDownList ID="DropOldBankType" runat="server" Width = "250px"  borderwidth = "2px" BorderStyle = "Ridge" >
                        <asp:ListItem Value="1001" Selected="True">招商银行</asp:ListItem>
                        <asp:ListItem Value="1027">广东发展银行</asp:ListItem>
                        <asp:ListItem Value="1026">中国银行</asp:ListItem>
                        <asp:ListItem Value="1022">中国光大银行</asp:ListItem>
                        <asp:ListItem Value="1021">中信实业银行</asp:ListItem>
                        <asp:ListItem Value="1020">中国交通银行</asp:ListItem>
                        <asp:ListItem Value="1010">平安银行</asp:ListItem>
                        <asp:ListItem Value="1009">兴业银行</asp:ListItem>
                        <asp:ListItem Value="1006">民生银行</asp:ListItem> 
                        <asp:ListItem Value="1005">农业银行</asp:ListItem>  
                        <asp:ListItem Value="1004">浦发银行</asp:ListItem>
                        <asp:ListItem Value="1003">建设银行</asp:ListItem>
                        <asp:ListItem Value="1002">工商银行</asp:ListItem>
                                                           		
						<asp:ListItem Value="1050">工行信用卡</asp:ListItem>                   
                        <asp:ListItem Value="3017">招商银行信用卡</asp:ListItem>
                        <asp:ListItem Value="3019">交通银行信用卡</asp:ListItem>
                        <asp:ListItem Value="3022">建设银行信用卡</asp:ListItem>                        
                        <asp:ListItem Value="3018">广发银行信用卡</asp:ListItem>
                        <asp:ListItem Value="3027">中国银行信用卡</asp:ListItem>                                                                                     
                        </asp:DropDownList>
		            </td>
				</tr>
                <tr bgColor="#ffffff" >
					<td style="HEIGHT: 21px" bgColor="#eeeeee" nowrap><FONT face="宋体">联系邮箱：</FONT></td>
					<td style="HEIGHT: 21px" colSpan="2">&nbsp;
						<asp:textbox id="txtMail" runat="server" Width="250px" BorderWidth="1px"></asp:textbox>                 
		            </td>
				</tr>
                <tr bgColor="#ffffff">
					<td style="HEIGHT: 21px" bgColor="#eeeeee" nowrap><FONT face="宋体">收款帐号类型：</FONT></td>
					<td style="HEIGHT: 21px" colSpan="2">&nbsp;
						<asp:DropDownList id="ddlUserFlag"  runat="server" Width = "250px" >
          	                    <asp:ListItem Value="1">个人</asp:ListItem>
								<asp:ListItem Value="2">公司</asp:ListItem>
                        </asp:DropDownList>                 
		            </td>
				</tr>
                </tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" nowrap><FONT face="宋体">新银行卡号：</FONT></td>
					<td colSpan="2">&nbsp;
						<asp:textbox id="txtNewBankAccNo" runat="server" Width="250px" BorderWidth="1px"></asp:textbox>
					</td>
				</tr>
                <tr bgColor="#ffffff">
					<td bgColor="#eeeeee" nowrap><FONT face="宋体">银行户名：</FONT></td>
					<td colSpan="2">&nbsp;
						<asp:textbox id="txtBankUserName" runat="server" Width="250px" BorderWidth="1px"></asp:textbox>
					</td>
				</tr>
                 <tr bgColor="#ffffff">
					<td bgColor="#eeeeee" nowrap><FONT face="宋体">新银行类型：</FONT></td>
					<td colSpan="2">&nbsp;						
					    <asp:DropDownList ID="DropNewBankType" runat="server" Width = "250px" onselectedindexchanged="DdlNewBankTypeSelectChanged" 
                            AutoPostBack ="true" >                     
                         <asp:ListItem Value="1001" Selected="True">招商银行</asp:ListItem>
                        <asp:ListItem Value="1027">广东发展银行</asp:ListItem>
                        <asp:ListItem Value="1026">中国银行</asp:ListItem>
                        <asp:ListItem Value="1022">中国光大银行</asp:ListItem>
                        <asp:ListItem Value="1021">中信实业银行</asp:ListItem>
                        <asp:ListItem Value="1020">中国交通银行</asp:ListItem>
                        <asp:ListItem Value="1010">平安银行</asp:ListItem>
                        <asp:ListItem Value="1009">兴业银行</asp:ListItem>
                        <asp:ListItem Value="1006">民生银行</asp:ListItem> 
                        <asp:ListItem Value="1005">农业银行</asp:ListItem>  
                        <asp:ListItem Value="1004">浦发银行</asp:ListItem>
                        <asp:ListItem Value="1003">建设银行</asp:ListItem>
                        <asp:ListItem Value="1002">工商银行</asp:ListItem>
                                                           		
						<asp:ListItem Value="1050">工行信用卡</asp:ListItem>                   
                        <asp:ListItem Value="3017">招商银行信用卡</asp:ListItem>
                        <asp:ListItem Value="3019">交通银行信用卡</asp:ListItem>
                        <asp:ListItem Value="3022">建设银行信用卡</asp:ListItem>                        
                        <asp:ListItem Value="3018">广发银行信用卡</asp:ListItem>
                        <asp:ListItem Value="3027">中国银行信用卡</asp:ListItem>      	                        
                        </asp:DropDownList>
                    </td>
				</tr>
                <tr bgColor="#ffffff">
					<td style="HEIGHT: 21px" bgColor="#eeeeee" nowrap><FONT face="宋体">收款人卡类型：</FONT></td>
					<td style="HEIGHT: 21px" colSpan="2">&nbsp;
						<asp:DropDownList id="ddlCardType"  runat="server" Width = "250px" AutoPostBack ="true" >
          	                    <asp:ListItem Value="1">借记卡</asp:ListItem>
								<asp:ListItem Value="2">信用卡</asp:ListItem>
                        </asp:DropDownList>                 
		            </td>
				</tr>
                <tr bgColor="#ffffff">
					<td bgColor="#eeeeee" nowrap><FONT face="宋体">开户行支行名称：</FONT></td>
					<td colSpan="2">&nbsp;
						<asp:textbox id="tbBankName" runat="server" Width="250px" BorderWidth="1px"></asp:textbox>
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td style="HEIGHT: 23px" bgColor="#eeeeee"  Width="180px BorderWidth="1px" nowrap>&nbsp;<FONT face="宋体">承诺函：</FONT></td>
					<td style="HEIGHT: 23px" colSpan="2">&nbsp;<FONT face="宋体"> </FONT><INPUT id="commitmentFile"  style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="commitmentFile"
							runat="server">
					</td>
				</tr>
                <tr bgColor="#ffffff">
					<td style="HEIGHT: 23px" bgColor="#eeeeee"  Width="180px BorderWidth="1px" nowrap >&nbsp;<FONT face="宋体">身份证：</FONT></td>
					<td style="HEIGHT: 23px" colSpan="2">&nbsp;<FONT face="宋体"> </FONT><INPUT id="identityCardFile"  style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="identityCardFile"
							runat="server">
					</td>
				</tr>
                 <tr bgColor="#ffffff">
					<td style="HEIGHT: 23px" bgColor="#eeeeee"  Width="180px BorderWidth="1px" nowrap>&nbsp;<FONT face="宋体">银行流水：</FONT></td>
					<td style="HEIGHT: 23px" colSpan="2">&nbsp;<FONT face="宋体"> </FONT><INPUT id="bankWaterFile" style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="bankWaterFile"
							runat="server">
					</td>
				</tr>
                 <tr bgColor="#ffffff">
					<td style="HEIGHT: 23px" bgColor="#eeeeee"  Width="180px BorderWidth="1px" nowrap >&nbsp;<FONT face="宋体">销户证明：</FONT></td>
					<td style="HEIGHT: 23px" colSpan="2">&nbsp;<FONT face="宋体"> </FONT><INPUT id="cancellationFile" style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="cancellationFile"
							runat="server">
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee"  Width="250px BorderWidth="1px" nowrap><FONT face="宋体">备注：</FONT></td>
					<td colSpan="2"><FONT face="宋体">&nbsp;</FONT>
						<asp:textbox id="txtRemark" runat="server" Width="338px" BorderWidth="1px" Height="69px" TextMode="MultiLine" ></asp:textbox>&nbsp;</td>
				</tr>
			</table>
			<P align="center"><FONT face="宋体">&nbsp;&nbsp;&nbsp; </FONT>
				<asp:button id="btnForm" runat="server" Width="130px" Height="30px" BorderStyle="Groove"
					Text="生成申请单" onclick="Button_Update_Click"></asp:button>
			</P>
		</form>
		<DIV align="center"><FONT face="宋体"></FONT>&nbsp;</DIV>
	</body>
</HTML>
