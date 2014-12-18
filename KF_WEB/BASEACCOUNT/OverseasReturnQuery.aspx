<%@ Page Language="c#" CodeBehind="OverseasReturnQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.OverseasReturnQuery" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>OverseasReturnQuery</title>
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );
        .style3
        {
            color: #ff0000;
        }
        BODY
        {
            background-image: url(../IMAGES/Page/bg01.gif);
        }
        .style4
        {
            width: 920px;
        }
        .tdfull
        {
            _height:expression(this.offsetParent.offsetHeight+"px");
        }
    </style>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <table cellspacing="1" cellpadding="0" align="center" bgcolor="#666666" border="0" width="95%">
        <tr bgcolor="#e4e5f7" style="background-image: ../IMAGES/Page/bg_bl.gif">
            <td valign="middle" colspan="2">
                <table style="height: 90% width:100%" cellspacing="0" cellpadding="1" border="0" class="tdfull">
                    <tr>
                        <td width="80%" style="background-image: ../IMAGES/Page/bg_bl.gif" height="18">
                            <font color="#ff0000"><strong><font color="#ff0000">&nbsp;</font></strong><img height="16"
                                src="../IMAGES/Page/post.gif" width="20">
                                境外收单退款</font>
                        </td>
                        <td width="20%" style="background-image: ../IMAGES/Page/bg_bl.gif">
                            操作员代码: <span class="style3">
                                <asp:Label ID="Label_uid" runat="server" Width="73px"></asp:Label></span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr bgcolor="#ffffff">
            <td colspan="2">
                <table  cellspacing="0" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td style="padding-left: 100px">
                            交易单号：&nbsp;
                            <asp:TextBox ID="TextTransactionId" runat="server"></asp:TextBox>
                        </td>
                        <td style="padding-left: 100px">
                            退款单号：&nbsp;
                            <asp:TextBox ID="TextDrawId" runat="server"></asp:TextBox>
                        </td>
                        <td style="padding-left: 100px">
                        <asp:button id="Button" runat="server" Width="80px" Text="查 询" onclick="Button_Click"></asp:button>
                         </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
      <br />
    <table id="table3" cellspacing="1" cellpadding="0" width="95%" align="center" border="0" frame="box" runat="server">
         <tr>
           <td  width="100%">
                <table width="100%" cellspacing="0" cellpadding="0" align="left" border="1" frame="box" rules="all">
                    <tr bgcolor="#e4e5f7" background="../IMAGES/Page/bg_bl.gif">
                        <td background="../IMAGES/Page/bg_bl.gif" height="20" class="style4">
                            <strong><font color="#ff0000">&nbsp;<img height="16" src="../IMAGES/Page/post.gif"
                                width="20" />
                            </font></strong><font color="#ff0000">境外商户退款详情表</font>
                            <div align="center">
                                <font face="宋体"></font>
                            </div>
                        </td>
                    </tr>
                     <tr width="100%">
						<TD vAlign="top" align="center" class="style4"><asp:datagrid id="Datagrid1" runat="server" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
								HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1" BackColor="White"  OnItemDataBound="DataGrid1_ItemDataBound"
								BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" AllowPaging="false" width="100%" ShowFooter="false" >
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="Ftransaction_id" HeaderText="交易单号">
										<HeaderStyle Width="200px"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Fdraw_id" HeaderText="退款单号">
									    <HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fspid" HeaderText="商户号">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fcur_type_str" HeaderText="账户类型">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
                                    	<asp:BoundColumn DataField="Ffee_type_str" HeaderText="外币类型">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Frefund_req_fee_str" HeaderText="退款金额(外币)">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="Frefund_req_fee_rmb_str" HeaderText="退款金额(人民币)">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="Fmemo" HeaderText="备注">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
                                     <asp:BoundColumn DataField="Fmodify_time" HeaderText="修改时间(本地)">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
                                     <asp:BoundColumn DataField="Fcreate_time" HeaderText="创建时间(本地)">
										<HeaderStyle Width="100px"></HeaderStyle>
                                     </asp:BoundColumn>
                                    <%-- <asp:BoundColumn DataField="order_exist" HeaderText="核心订单">
										<HeaderStyle Width="100px"></HeaderStyle>
                                     </asp:BoundColumn>--%>
                                       <asp:BoundColumn DataField="Fcore_order_exist"  Visible="false" HeaderText="核心订单是否存在">
										<HeaderStyle Width="100px"></HeaderStyle>
                                     </asp:BoundColumn>
								    <asp:TemplateColumn>
									    <ItemTemplate>
										    <asp:Button id="queryButton" Visible="false" runat="server" CommandName="query" Text="核心交易退款详情"></asp:Button>
									    </ItemTemplate>
								   </asp:TemplateColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
				    	</tr>
                    <tr>
                        <td class="style4" Width="100%" height="30">
                         <asp:button id="Detail" runat="server" Width="120px" Text="核心交易退款详情" onclick="Detail_Click"></asp:button>
                        </td>
                     </tr>
                   
                     <tr width="100%">
                     <td class="style4" >
                      <table cellSpacing="0" cellPadding="0" width="100%" border="0">
					    <tr>
						<TD vAlign="top" align="center"><asp:datagrid id="dgList" runat="server" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
								HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1" BackColor="White"
								BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" AllowPaging="false" width="100%">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="Ftransaction_id" HeaderText="交易单号">
										<HeaderStyle Width="200px"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Frp_feeName" HeaderText="退给买家费用">
									    <HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Frb_feeName" HeaderText="退给卖家费用">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FstatusName" HeaderText="业务状态">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
                                    	<asp:BoundColumn DataField="Fcreate_time" HeaderText="生成时间">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fmodify_time" HeaderText="最后修改时间">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="Frefund_typeName" HeaderText="退款类型">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="Fdraw_id" HeaderText="退单ID">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
				    	</tr>
			    	</table>
                     </td>
                   </tr>
                    </table>
           </td>
     </tr>
    </table>
          
      
    </form>
</body>
</html>
