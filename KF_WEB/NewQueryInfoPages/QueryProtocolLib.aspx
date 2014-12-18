<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="QueryProtocolLib.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryProtocolLib" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ComplainBussinessInput</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script language="javascript">
					function openModeBegin()
					{
						var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxBeginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
						if(returnValue != null) Form1.TextBoxBeginDate.value=returnValue;
		            }
		            function openModeEnd() {
		                var returnValue = window.showModalDialog("../Control/CalendarForm2.aspx", Form1.TextBoxEndDate.value, 'dialogWidth:375px;DialogHeight=260px;status:no');
		                if (returnValue != null) Form1.TextBoxEndDate.value = returnValue;
		            }
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="4"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;代扣协议库查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
                    <TD align="right"><asp:label id="Label2" runat="server">商户号：</asp:label></TD>
                    <TD><asp:textbox id="bussId" style="WIDTH: 180px;" runat="server"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label5" runat="server">起始日期：</asp:label></TD>
                    <TD><asp:textbox id="TextBoxBeginDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton>
                    </TD>
				</TR>
                <TR>
                    <TD align="right"><asp:label id="Label3" runat="server">商户名称：</asp:label></TD>
                    <TD><asp:textbox id="bussName" style="WIDTH: 180px;" runat="server"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label4" runat="server">结束日期：</asp:label></TD>
                    <TD><asp:textbox id="TextBoxEndDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonEndDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton>
                    </TD>
				</TR>
				<TR>
                    <TD align="center" colspan="4"><asp:button id="btnQuery" runat="server" Width="80px" Text="查 询" onclick="btnQuery_Click"></asp:button>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 154px; HEIGHT: 35%"
				cellSpacing="1" cellPadding="1" width="808" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" OnItemDataBound="DataGrid1_ItemDataBound">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fbatchid" HeaderText="批次号"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fsp_batchid" HeaderText="商户批次号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fspid" HeaderText="商户号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ftotal_count" HeaderText="总笔数"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcreate_time" HeaderText="导入时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmemo" HeaderText="批次说明"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="操作">
                                    <ItemTemplate>
                                        <asp:LinkButton id="lbDetail" runat="server" CommandName="DETAIL">显示明细</asp:LinkButton>&nbsp;
                                        <asp:LinkButton id="lbChange" Visible="false" runat="server" CommandName="CHANGE">审核通过</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="Ffname" HeaderText="Ffname"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="Fverify_way" HeaderText="Fverify_way"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="Fstate" HeaderText="Fstate"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="Fop_type" HeaderText="Fop_type"></asp:BoundColumn>
							</Columns>
                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
                <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage"
							SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
            <TABLE id="tbDetail" cellSpacing="1" cellPadding="1" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 478px;" border="1">
                <TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid2" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fseq" HeaderText="序号"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fbuss_proto_flag" HeaderText="商户侧协议标识"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbuss_user_flag" HeaderText="商户侧用户标识"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Facc_att" HeaderText="账户属性"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fname" HeaderText="姓名/公司名称"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fphone" HeaderText="手机号码"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fopenacc_cer_type" HeaderText="开户证件类型"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fopenacc_cer_id" HeaderText="开户证件号码"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbank_name" HeaderText="银行名称"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Facc_type" HeaderText="账号类型"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbank_acc" HeaderText="银行账号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fprovince" HeaderText="省市名称"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbranch_name" HeaderText="支行名称"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcre_card_valid" HeaderText="信用卡有效期"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcre_card_cvv2" HeaderText="信用卡cvv2"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcft_no" HeaderText="财付通账号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbuss_code" HeaderText="业务代码"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fpay_type" HeaderText="支付方式"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fproto_start_time" HeaderText="协议起始日"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fproto_end_time" HeaderText="协议结束日"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fproto_brief_desc" HeaderText="协议简要说明"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fproto_detail_desc" HeaderText="协议详细说明"></asp:BoundColumn>
                                
							</Columns>
                            
						</asp:datagrid></TD>
				</TR>
                
            </TABLE>
		</form>
	</body>
</HTML>
