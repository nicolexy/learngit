<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WechatAACollection.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.WebchatPay.WechatAACollection" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AA收款</title>
    <style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
</head>
<body>
    <form id="formMain" runat="server">
    <div>
        <table border="1" cellspacing="1" cellpadding="1" width="1100">
				<tr>
					<td style="width: 100%" bgcolor="#e4e5f7" colspan="5"><font color="red"><img src="../images/page/post.gif" width="20" height="16">AA收款查询</font>
						</td>
				</tr>
				<tr>
                    <td>微信号:<asp:TextBox ID="txtWechatName" runat="server"></asp:TextBox></td>
					<td align="center"><asp:button id="btnQuery" runat="server" width="80px" text="查 询" 
                            onclick="btnQuery_Click"></asp:button></td>
				</tr>
                <tr>
                    <td align="left" >
                       微信支付财付通帐号：
                       <asp:label id="lblacctUin" runat="server"></asp:label>
                       
                    </td>
                    <td align="left" >
                       微信AA财付通帐号：
                       <asp:label id="lblAAUin" runat="server"></asp:label>
                    </td>
                <tr>
                    <td align="left" colspan="2" >
                       账户余额：
                       <asp:label id="lblBanlance" runat="server"></asp:label>
                    </td>
                </tr>

			</TABLE>
            <br/>
			<table border="1" cellSpacing="0" cellPadding="0" width="1100">
                <TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">AA记录</FONT>
				    </TD>
				</TR>
				<TR>
					<TD vAlign="top">
                        <asp:GridView id="gvAACollections" runat="server" Width="1100px" ItemStyle-HorizontalAlign="Center"
							HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
							GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None" 
                            BorderColor="#E7E7FF" onrowcommand="gvAACollections_RowCommand">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundField DataField="Fcreate_time" HeaderText="日期">
									<HeaderStyle Width="150px" HorizontalAlign="Center"></HeaderStyle> <ItemStyle HorizontalAlign="Center" />
								</asp:BoundField>
                                <asp:BoundField DataField="Freason" HeaderText="主题">
									<HeaderStyle Width="200px"></HeaderStyle> <ItemStyle HorizontalAlign="Center" />
								</asp:BoundField>
								<asp:TemplateField HeaderText="参与人数">
                                    <ItemTemplate>
                                        <%#string.Format("{0}/{1}", ((System.Data.DataRowView)Container.DataItem)["Ftotal_paid_num"].ToString(), ((System.Data.DataRowView)Container.DataItem)["Fplan_paid_num"].ToString())%>
                                    </ItemTemplate><ItemStyle HorizontalAlign="Center"/>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Ftotal_paid_amount_text" HeaderText="金额">
									<HeaderStyle Width="80px"></HeaderStyle> <ItemStyle HorizontalAlign="Center" />
								</asp:BoundField>
								<asp:BoundField DataField="Fstatus_text" HeaderText="状态">
									<HeaderStyle Width="110px"></HeaderStyle> <ItemStyle HorizontalAlign="Center"/>
								</asp:BoundField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnView1" runat="server" CausesValidation="False" 
                                            CommandArgument='<%#string.Format("{0},{1}",((System.Data.DataRowView)Container.DataItem)["Faa_collection_no"].ToString(), ((System.Data.DataRowView)Container.DataItem)["Fcreate_time"].ToString())%>' CommandName="ViewDetail" Text="详情"></asp:LinkButton>
                                    </ItemTemplate> 
                                </asp:TemplateField>
							</Columns>
						</asp:GridView><webdiyer:aspnetpager id="aaCollectionPager" runat="server" 
                            HorizontalAlign="right" NumericButtonCount="5" PagingButtonSpacing="0"
							ShowInputBox="always" CssClass="mypager" SubmitButtonText="转到" 
                            NumericButtonTextFormatString="[{0}]" AlwaysShow="True" 
                            onpagechanged="aaCollectionPager_PageChanged"></webdiyer:aspnetpager></TD>
				</TR>
			</table>
            <br/>
            <table border="1" cellSpacing="0" cellPadding="0" width="1100">
                <tr>
					<td style="width: 100%" bgcolor="#e4e5f7" colspan="5"><font color="red"><img src="../images/page/post.gif" width="20" height="16">AA收款明细</font>
						</td>
				</tr>
				<tr>
					<td valign="top">
                        <asp:HiddenField ID="hfCurrentCollectionNo" runat="server" />
                        <asp:HiddenField ID="hfCurrentCollectionCreateTime" runat="server" />
                        <asp:gridview id="gvaacollectiondetails" runat="server" width="1100px" itemstyle-horizontalalign="center"
							headerstyle-horizontalalign="center" horizontalalign="center" pagesize="5" autogeneratecolumns="false"
							gridlines="horizontal" cellpadding="1" backcolor="white" borderwidth="1px" borderstyle="none" bordercolor="#e7e7ff">
							<footerstyle forecolor="#4a3c8c" backcolor="#b5c7de"></footerstyle>
							<headerstyle font-bold="true" horizontalalign="center" forecolor="#f7f7f7" backcolor="#4a3c8c"></headerstyle>
							<columns>
								<asp:boundfield datafield="Fmemo" headertext="说明">
									<headerstyle width="150px" horizontalalign="center"></headerstyle> <ItemStyle HorizontalAlign="Center" />
								</asp:boundfield>
                                <asp:boundfield datafield="Fnum_text" headertext="每人收取金额">
									<headerstyle width="100px"></headerstyle> <ItemStyle HorizontalAlign="Center" />
								</asp:boundfield>
								<asp:boundfield datafield="Fpay_nickname" headertext="付款方昵称">
									<headerstyle width="70px"></headerstyle> <ItemStyle HorizontalAlign="Center" />
								</asp:boundfield>
                                	<asp:boundfield datafield="receive_name" headertext="收款方账户姓名">
									<headerstyle width="70px"></headerstyle> <ItemStyle HorizontalAlign="Center" />
								</asp:boundfield>
                                <asp:boundfield datafield="Fpay_openid" headertext="付款方微信财付通账号">
									<headerstyle width="200px"></headerstyle> <ItemStyle HorizontalAlign="Center" />
								</asp:boundfield>
                                <asp:boundfield datafield="Fpay_aaopenid" headertext="付款方微信AA财付通账号">
									<headerstyle width="200px"></headerstyle> <ItemStyle HorizontalAlign="Center" />
								</asp:boundfield>
                                  <asp:boundfield datafield="Freceive_openid" headertext="收款方微信财付通账号">
									<headerstyle width="200px"></headerstyle> <ItemStyle HorizontalAlign="Center" />
								</asp:boundfield>
                                <asp:boundfield datafield="receive_aaopenid" headertext="收款方微信AA财付通账号">
									<headerstyle width="200px"></headerstyle> <ItemStyle HorizontalAlign="Center" />
								</asp:boundfield>
                                <asp:boundfield datafield="Fstate_text" headertext="付款状态">
									<headerstyle width="80px"></headerstyle>
                                    <ItemStyle HorizontalAlign="Center"/>
								</asp:boundfield>
								<asp:boundfield datafield="Fpay_memo" headertext="付款备注">
									<headerstyle width="200px"></headerstyle>
                                     <ItemStyle HorizontalAlign="Center" />
								</asp:boundfield>
							</columns>
						</asp:gridview>
                        <webdiyer:aspnetpager id="aaCollectionDetails" runat="server" 
                            horizontalalign="right" numericbuttoncount="5" pagingbuttonspacing="0"
							showinputbox="always" cssclass="mypager" submitbuttontext="转到" 
                            numericbuttontextformatstring="[{0}]" alwaysshow="true" 
                            onpagechanged="aaCollectionDetails_pagechanged"></webdiyer:aspnetpager></td>
				</tr>
			</table>
    </div>
    </form>
</body>
</html>
