<%@ Page language="c#" Codebehind="OrderDetail2.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.OrderDetail2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
    <HEAD>
        <title>batPayReturn</title>
        
        
        
        
        <style type="text/css">@import url( ../Resources/css/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
    BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
        </style>
    </HEAD>
    <BODY>
        <form id="Form1" method="post" encType="multipart/form-data" runat="server">
            &nbsp;
            <TABLE id="Table2" style="Z-INDEX: 101; LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1"
                cellPadding="1" width="90%" border="1">
                <TR>
                    <TD bgColor="#666666">
                        <TABLE height="391" cellSpacing="1" cellPadding="0" width="100%" align="center" border="0">
                            <TR bgColor="#e4e5f7">
                                <TD style="HEIGHT: 15px" colSpan="5" height="18"><STRONG></STRONG>
                                    <table cellSpacing="0" cellPadding="1" width="100%" border="0">
                                        <tr>
                                            <td background="../IMAGES/Page/bg_bl.gif" height="20"><strong><font color="#ff0000"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
                                                    </font></strong><font color="#ff0000">交易单资料</font>
                                            </td>
                                            <td width="10%" background="../IMAGES/Page/bg_bl.gif"><!--<div align="center"><A href="../ACCOUNTMANAGE/AdjustDepositMoney.aspx?id=<%=LB_Flistid.Text.Trim()%>" ><font color="red">分析</font></A>|</div> --><FONT face="宋体"><asp:hyperlink id="hlOrder" runat="server">订单详细信息</asp:hyperlink></FONT></td>
                                            <td width="5%" background="../IMAGES/Page/bg_bl.gif">
                                                <div align="left"><A href="tradeLogQuery.aspx?id=<%=LB_Flistid.Text.Trim()%>" target=_blank ><font color="red"><font color="red">全屏</font></font></A></div>
                                            </td>
                                        </tr>
                                    </table>
                                    <DIV align="center"><FONT face="宋体"></FONT></DIV>
                                </TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px" width="154" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="24">&nbsp; 交易单号:</TD>
                                <TD style="WIDTH: 129px; HEIGHT: 2px" width="243" bgColor="#ffffff" height="24">&nbsp;<span class="style4">
                                        <asp:label id="LB_Flistid" runat="server" Width="194px" ForeColor="Black"></asp:label></span></TD>
                                <TD width="152" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="24">&nbsp;交易状态:</TD>
                                <TD bgColor="#ffffff" colSpan="2" height="24">&nbsp;
                                    <asp:dropdownlist id="DropDownList2_tradeState" runat="server" ForeColor="Black" Enabled="False">
                                        <asp:ListItem Value="1" Selected="True">支付中</asp:ListItem>
                                        <asp:ListItem Value="2">支付成功</asp:ListItem>
                                        <asp:ListItem Value="3">确认收货</asp:ListItem>
                                        <asp:ListItem Value="4">转入退款</asp:ListItem>
                                        <asp:ListItem Value="5">5</asp:ListItem>
                                        <asp:ListItem Value="6">6</asp:ListItem>
                                        <asp:ListItem Value="7">7</asp:ListItem>
                                        <asp:ListItem Value="8">8</asp:ListItem>
                                        <asp:ListItem Value="9">9</asp:ListItem>
                                        <asp:ListItem Value="10">10</asp:ListItem>
                                        <asp:ListItem Value="11">11</asp:ListItem>
                                        <asp:ListItem Value="12">12</asp:ListItem>
                                        <asp:ListItem Value="13">未定义</asp:ListItem>
                                        <asp:ListItem Value="14">未定义</asp:ListItem>
                                        <asp:ListItem Value="99">作废</asp:ListItem>
                                    </asp:dropdownlist></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="宋体">&nbsp;是否调帐标志:</FONT></TD>
                                <TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp; </FONT>
                                    <asp:label id="lbAdjustFlag" runat="server" ForeColor="Red"></asp:label></TD>
                                <TD style="HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18">
                                    <P><FONT face="宋体">&nbsp;交易类型:</FONT></P>
                                </TD>
                                <TD style="HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="18"><FONT face="宋体">&nbsp;
                                        <asp:label id="lbTradeType" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="宋体">&nbsp;给银行订单号:</FONT></TD>
                                <TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Fbank_listid" runat="server"></asp:label></FONT></TD>
                                <TD style="HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="宋体">&nbsp;银行返回订单号:</FONT></TD>
                                <TD style="HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="18"><FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Fbank_backid" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 17px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="17"><FONT face="宋体">&nbsp;机构代码名称(发起):</FONT></TD>
                                <TD style="HEIGHT: 17px" bgColor="#ffffff" height="17"><FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Fspid" runat="server"></asp:label></FONT></TD>
                                <TD style="HEIGHT: 17px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="17"><FONT face="宋体">&nbsp;交易单的状态:</FONT></TD>
                                <TD style="HEIGHT: 17px" width="132" bgColor="#ffffff" height="17" colSpan="2"><FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Flstate" runat="server" ForeColor="Red"></asp:label></FONT></TD>
                                <%-- <TD style="HEIGHT: 17px" width="135" background="../IMAGES/Page/bg_bl.gif" bgColor="#e4e5f7"
                                    height="17">&nbsp;
                                   <asp:linkbutton id="LinkButton3_action" runat="server" onclick="LinkButton3_action_Click">冻结</asp:linkbutton></TD>--%>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18">&nbsp;币种代码:</TD>
                                <TD style="WIDTH: 129px; HEIGHT: 18px" bgColor="#ffffff" height="18">&nbsp;
                                    <asp:label id="LB_Fcurtype" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18">&nbsp;支付类型:</TD>
                                <TD style="HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">
                                        <asp:label id="LB_Fpay_type" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="宋体">&nbsp;最后修改交易单的IP:</FONT></TD>
                                <TD style="WIDTH: 129px; HEIGHT: 15px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Fip" runat="server"></asp:label></FONT></TD>
                                <TD style="HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="宋体">&nbsp;最后修改时间(本地)</FONT></TD>
                                <TD style="HEIGHT: 15px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Fmodify_time" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="宋体">&nbsp;创建时间:</FONT></TD>
                                <TD style="WIDTH: 129px; HEIGHT: 15px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Fcreate_time" runat="server"></asp:label></FONT></TD>
                                <TD style="HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="宋体">&nbsp;渠道编号</FONT></TD>
                                <TD style="HEIGHT: 15px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Fchannel_id" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;中介ID:</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
                                    <asp:label id="LB_Fmediuid" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;中介余额:</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Fmedinum" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;手续费账户:</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
                                    <asp:label id="LB_Fchargeuid" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;手续费:</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Fchargenum" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;总金额:</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
                                    <asp:label id="LB_Ftotalnum" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;买家已支付总金额:</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Fbuyerpaytotal" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;买家已退款总金额:</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
                                    <asp:label id="LB_Fbuyerrefundtotal" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;卖家已收到总金额:</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Fsellerpaytotal" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;卖家已退款总金额:</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
                                    <asp:label id="LB_Fsellerrefundtotal" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;参与方数量:</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Frolenum" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;内部ID(0):</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
                                    <asp:label id="LB_Fuid0" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;预计金额(0):</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Fplanpaynum0" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;实际金额(0):</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
                                    <asp:label id="LB_Fpaynum0" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;退款金额(0):</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Frefund0" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;内部ID(1):</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
                                    <asp:label id="LB_Fuid1" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;预计金额(1):</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Fplanpaynum1" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;实际金额(1):</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
                                    <asp:label id="LB_Fpaynum1" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;退款金额(1):</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Frefund1" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;内部ID(2):</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
                                    <asp:label id="LB_Fuid2" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;预计金额(2):</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Fplanpaynum2" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;实际金额(2):</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
                                    <asp:label id="LB_Fpaynum2" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;退款金额(2):</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Frefund2" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;内部ID(3):</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
                                    <asp:label id="LB_Fuid3" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;预计金额(3):</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Fplanpaynum3" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;实际金额(3):</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
                                    <asp:label id="LB_Fpaynum3" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;退款金额(3):</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Frefund3" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="宋体">&nbsp;后台备注:</FONT></TD>
                                <TD bgColor="#ffffff" colSpan="4" height="18"><FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Fexplain" runat="server"></asp:label></FONT><FONT face="宋体">&nbsp;</FONT><FONT face="宋体">&nbsp;</FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="宋体">&nbsp;备注:</FONT></TD>
                                <TD bgColor="#ffffff" colSpan="4" height="18"><FONT face="宋体">&nbsp;
                                        <asp:label id="LB_FMemo" runat="server"></asp:label></FONT><FONT face="宋体">&nbsp;</FONT><FONT face="宋体">&nbsp;</FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="宋体">&nbsp;业务处理描述:</FONT></TD>
                                <TD bgColor="#ffffff" colSpan="4" height="18"><FONT face="宋体">&nbsp;
                                        <asp:label id="LB_Fcatch_desc" runat="server"></asp:label></FONT><FONT face="宋体">&nbsp;</FONT><FONT face="宋体">&nbsp;</FONT></TD>
                            </TR>
                        </TABLE>
                    </TD>
                </TR>
                <TR>
                    <TD><asp:label id="Label37" runat="server" ForeColor="Blue">交易流水</asp:label></TD>
                </TR>
                <TR width="100%">
                    <TD><asp:datagrid id="DataGrid2" runat="server" Width="100%" CellPadding="3" BackColor="White" BorderWidth="1px"
                            BorderStyle="None" BorderColor="#CCCCCC" AutoGenerateColumns="False">
                            <FooterStyle ForeColor="#000066" BackColor="White"></FooterStyle>
                            <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#669999"></SelectedItemStyle>
                            <ItemStyle ForeColor="#000066"></ItemStyle>
                            <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#006699"></HeaderStyle>
                            <Columns>
                                <asp:BoundColumn DataField="Fqqid" HeaderText="用户帐号"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="FtypeName" HeaderText="交易类型"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FsubjectName" HeaderText="请求类别"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ffromid" HeaderText="对方帐号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FpaynumName" HeaderText="金额"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FpaybuyName" HeaderText="退买家/现金"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FpaysaleName" HeaderText="退卖家/现金券"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcreate_time" HeaderText="创建时间"></asp:BoundColumn>
                            </Columns>
                            <PagerStyle HorizontalAlign="Left" ForeColor="#000066" BackColor="White" Mode="NumericPages"></PagerStyle>
                        </asp:datagrid></TD>
                </TR>
                <%--<TR>
                    <TD><asp:label id="Label43" runat="server" ForeColor="Blue">物流信息</asp:label></TD>
                </TR>
                <TR>
                    <TD><asp:datagrid id="Datagrid3" runat="server" Width="100%" CellPadding="3" BackColor="White" BorderWidth="1px"
                            BorderStyle="None" BorderColor="#CCCCCC" AutoGenerateColumns="False">
                            <FooterStyle ForeColor="#000066" BackColor="White"></FooterStyle>
                            <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#669999"></SelectedItemStyle>
                            <ItemStyle ForeColor="#000066"></ItemStyle>
                            <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#006699"></HeaderStyle>
                            <Columns>
                                <asp:BoundColumn DataField="Fqqid" HeaderText="帐号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ftransport_typeName" HeaderText="物流类型"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fgoods_typeName" HeaderText="物品类型"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ftran_typeName" HeaderText="发货物流类型"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ftransport_id" HeaderText="物流单号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Frecv_truename" HeaderText="收货姓名"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fsend_truename" HeaderText="发货姓名"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FstateName" HeaderText="物流单状态"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcreate_time" HeaderText="创建时间"></asp:BoundColumn>
                            </Columns>
                            <PagerStyle HorizontalAlign="Left" ForeColor="#000066" BackColor="White" Mode="NumericPages"></PagerStyle>
                        </asp:datagrid></TD>
                </TR>--%>
                <TR>
                    <TD><asp:label id="Label29" runat="server" ForeColor="Blue">投诉单信息</asp:label></TD>
                </TR>
                <TR>
                    <TD><asp:datagrid id="DataGrid1" runat="server" Width="100%" CellPadding="3" BackColor="White" BorderWidth="1px"
                            BorderStyle="None" BorderColor="#CCCCCC" AutoGenerateColumns="False">
                            <FooterStyle ForeColor="#000066" BackColor="White"></FooterStyle>
                            <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#669999"></SelectedItemStyle>
                            <ItemStyle ForeColor="#000066"></ItemStyle>
                            <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#006699"></HeaderStyle>
                            <Columns>
                                <asp:HyperLinkColumn DataNavigateUrlField="Fappealid" DataNavigateUrlFormatString="AppealDetail.aspx?appealid={0}"
                                    DataTextField="Fappealid" HeaderText="投诉单号"></asp:HyperLinkColumn>
                                <asp:BoundColumn DataField="Fqqid" HeaderText="帐户号码"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fvs_qqid" HeaderText="对方帐号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FstateName" HeaderText="投诉单状态"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fpunish_flagName" HeaderText="处罚标记"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcheck_stateName" HeaderText="用服审核标计"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fappeal_typeName" HeaderText="投诉类别"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fappeal_time" HeaderText="投诉时间"></asp:BoundColumn>
                            </Columns>
                            <PagerStyle HorizontalAlign="Left" ForeColor="#000066" BackColor="White" Mode="NumericPages"></PagerStyle>
                        </asp:datagrid></TD>
                </TR>
            </TABLE>
        </form>
    </BODY>
</HTML>
