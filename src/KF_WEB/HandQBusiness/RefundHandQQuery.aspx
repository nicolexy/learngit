<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RefundHandQQuery.aspx.cs"
    Inherits="TENCENT.OSS.CFT.KF.KF_Web.HandQBusiness.RefundHandQQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>手Q还款查询</title>
    <style type="text/css">@import url( ../STYLES/ossstyle.css ); 
        UNKNOWN { COLOR: #000000 }
	    .style3 { COLOR: #ff0000 }
	    BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		.auto-style1
        {
            width: 84px;
        }
		</style>
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body>
    <form id="formMain" runat="server">
    <div>
        <table border="1" cellspacing="1" cellpadding="1" width="1000">
            <tr>
                <td bgcolor="#e4e5f7" colspan="5">
                    <font color="red">
                        <img src="../images/page/post.gif" width="20" height="16">手Q还款查询</font>
                </td>
            </tr>
            <tr>
			  <TD><asp:dropdownlist id="ddlType" runat="server" Width="100px">
							<asp:ListItem Value="1">财付通账号</asp:ListItem>
							<asp:ListItem Value="2">卡号</asp:ListItem>
							<asp:ListItem Value="3">手Q交易单号</asp:ListItem>
							<asp:ListItem Value="4">商户订单号</asp:ListItem>
						</asp:dropdownlist></TD>
                <td>
                   <asp:TextBox ID="txtInput" runat="server" Width="150px"></asp:TextBox>                  
                   
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;
                    开始日期：                   
                    <asp:TextBox ID="textBoxBeginDate" runat="server" Width="150px" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"  CssClass="Wdate"></asp:TextBox>
                   <img onclick="textBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
                    结束日期：
                        <asp:TextBox ID="textBoxEndDate" runat="server" Width="150px" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"  CssClass="Wdate"></asp:TextBox> 
                    <img onclick="textBoxEndDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />  
                     <asp:Button ID="btnQuery" runat="server" Width="80px" Text="查 询" OnClick="btnQuery_Click">
                    </asp:Button>             
                </td>
            </tr>
            <%-- <tr>
                <td colspan="5" align="center">
                    <font color="red">时间跨度只支持按自然月查询，不支持跨月查询</font>
                </td>
            </tr>--%>
        </table>
        <br />
        <table border="1" cellspacing="0" cellpadding="0" width="1000">
            <tr>
                <td valign="top">
                    <asp:GridView ID="DatagridList" runat="server" Width="1000px" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                        GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                        BorderColor="#E7E7FF" OnRowCommand="gvReceiveHQList_RowCommand">
                        <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                        </HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="card_name" HeaderText="开户名称">
                                <HeaderStyle Width="150px" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="num_str" HeaderText="还款金额">
                                <HeaderStyle Width="200px"></HeaderStyle> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="bank_type_str" HeaderText="还款银行">
                                <HeaderStyle Width="80px"></HeaderStyle> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField> 
                            <asp:BoundField DataField="card_id" HeaderText="银行账号">
                                <HeaderStyle Width="110px"></HeaderStyle> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                           <asp:BoundField DataField="create_time" HeaderText="还款时间">
                                <HeaderStyle Width="110px"></HeaderStyle> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="state_str" HeaderText="还款状态">
                                <HeaderStyle Width="80px"></HeaderStyle> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                              <asp:BoundField DataField="isTP" HeaderText="退票">
                                <HeaderStyle Width="110px"></HeaderStyle> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:TemplateField ShowHeader="False">
                                <HeaderStyle Width="200px"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnView1" runat="server" CausesValidation="False" CommandArgument='<%#string.Format("{0}",((System.Data.DataRowView)Container.DataItem)["wx_fetch_no"].ToString())%>'
                                        CommandName="ViewDetail" Text="详情"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <webdiyer:aspnetpager id="pager" runat="server" horizontalalign="right"
                        numericbuttoncount="5" pagingbuttonspacing="0" showinputbox="always" cssclass="mypager"
                        submitbuttontext="转到" numericbuttontextformatstring="[{0}]" alwaysshow="True"
                        onpagechanged="PageChanged"></webdiyer:aspnetpager>
                </td>
            </tr>
        </table>
        <br />
        	<div id="divInfo" style="LEFT: 5%; WIDTH: 820px; "
				runat="server">
				<table cellSpacing="1" cellPadding="1" width="820" align="center" border="1">
					<tr>
						<TD align="left" width="150"><asp:label id="Label16" runat="server">还款人ID</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbopenid" runat="server"></asp:label></td>
						<TD align="left" width="150"><asp:label id="Label5" runat="server">卡号</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbcard_id" runat="server"></asp:label></td>
					</tr>
					<tr>
						<TD align="left"><asp:label id="Label6" runat="server">商户订单号</asp:label></TD>
						<td align="left"><asp:label id="lbwx_fetch_no" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label7" runat="server">开户名称</asp:label></TD>
						<td align="left"><asp:label id="lbcard_name" runat="server"></asp:label></td>
					</tr>
					<tr>
						<TD align="left"><asp:label id="Label17" runat="server">还款金额</asp:label></TD>
						<td align="left"><asp:label id="lbnum" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label20" runat="server">还款银行</asp:label></TD>
						<td align="left"><asp:label id="lbbank_name" runat="server"></asp:label></td>
					</tr>
					<tr>
						<TD align="left"><asp:label id="Label9" runat="server">还款时间</asp:label></TD>
						<td align="left"><asp:label id="lbcreate_time" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label18" runat="server">还款状态</asp:label></TD>
						<td align="left"><asp:label id="lbstate" runat="server"></asp:label></td>
					</tr>

                    <tr>
						<TD align="left"><asp:label id="Label22" runat="server">最后修改时间</asp:label></TD>
						<td align="left"><asp:label id="lbmodify_time" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label25" runat="server">财付通提现单号</asp:label></TD>
						<td align="left"><asp:label id="lbcft_fetch_no" runat="server"></asp:label></td>
					</tr>
                     <tr>
						<TD align="left"><asp:label id="Label23" runat="server">手Q支付单号</asp:label></TD>
						<td align="left"><asp:label id="lbtrade_no" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label27" runat="server"></asp:label></TD>
						<td align="left"><asp:label id="lb" runat="server"></asp:label></td>
					</tr>
				</table>
			</div>
    </div>
    </form>
</body>
</html>
