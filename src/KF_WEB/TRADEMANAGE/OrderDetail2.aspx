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
                                                    </font></strong><font color="#ff0000">���׵�����</font>
                                            </td>
                                            <td width="10%" background="../IMAGES/Page/bg_bl.gif"><!--<div align="center"><A href="../ACCOUNTMANAGE/AdjustDepositMoney.aspx?id=<%=LB_Flistid.Text.Trim()%>" ><font color="red">����</font></A>|</div> --><FONT face="����"><asp:hyperlink id="hlOrder" runat="server">������ϸ��Ϣ</asp:hyperlink></FONT></td>
                                            <td width="5%" background="../IMAGES/Page/bg_bl.gif">
                                                <div align="left"><A href="tradeLogQuery.aspx?id=<%=LB_Flistid.Text.Trim()%>" target=_blank ><font color="red"><font color="red">ȫ��</font></font></A></div>
                                            </td>
                                        </tr>
                                    </table>
                                    <DIV align="center"><FONT face="����"></FONT></DIV>
                                </TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px" width="154" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="24">&nbsp; ���׵���:</TD>
                                <TD style="WIDTH: 129px; HEIGHT: 2px" width="243" bgColor="#ffffff" height="24">&nbsp;<span class="style4">
                                        <asp:label id="LB_Flistid" runat="server" Width="194px" ForeColor="Black"></asp:label></span></TD>
                                <TD width="152" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="24">&nbsp;����״̬:</TD>
                                <TD bgColor="#ffffff" colSpan="2" height="24">&nbsp;
                                    <asp:dropdownlist id="DropDownList2_tradeState" runat="server" ForeColor="Black" Enabled="False">
                                        <asp:ListItem Value="1" Selected="True">֧����</asp:ListItem>
                                        <asp:ListItem Value="2">֧���ɹ�</asp:ListItem>
                                        <asp:ListItem Value="3">ȷ���ջ�</asp:ListItem>
                                        <asp:ListItem Value="4">ת���˿�</asp:ListItem>
                                        <asp:ListItem Value="5">5</asp:ListItem>
                                        <asp:ListItem Value="6">6</asp:ListItem>
                                        <asp:ListItem Value="7">7</asp:ListItem>
                                        <asp:ListItem Value="8">8</asp:ListItem>
                                        <asp:ListItem Value="9">9</asp:ListItem>
                                        <asp:ListItem Value="10">10</asp:ListItem>
                                        <asp:ListItem Value="11">11</asp:ListItem>
                                        <asp:ListItem Value="12">12</asp:ListItem>
                                        <asp:ListItem Value="13">δ����</asp:ListItem>
                                        <asp:ListItem Value="14">δ����</asp:ListItem>
                                        <asp:ListItem Value="99">����</asp:ListItem>
                                    </asp:dropdownlist></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="����">&nbsp;�Ƿ���ʱ�־:</FONT></TD>
                                <TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp; </FONT>
                                    <asp:label id="lbAdjustFlag" runat="server" ForeColor="Red"></asp:label></TD>
                                <TD style="HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18">
                                    <P><FONT face="����">&nbsp;��������:</FONT></P>
                                </TD>
                                <TD style="HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="18"><FONT face="����">&nbsp;
                                        <asp:label id="lbTradeType" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="����">&nbsp;�����ж�����:</FONT></TD>
                                <TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
                                        <asp:label id="LB_Fbank_listid" runat="server"></asp:label></FONT></TD>
                                <TD style="HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="����">&nbsp;���з��ض�����:</FONT></TD>
                                <TD style="HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="18"><FONT face="����">&nbsp;
                                        <asp:label id="LB_Fbank_backid" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 17px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="17"><FONT face="����">&nbsp;������������(����):</FONT></TD>
                                <TD style="HEIGHT: 17px" bgColor="#ffffff" height="17"><FONT face="����">&nbsp;
                                        <asp:label id="LB_Fspid" runat="server"></asp:label></FONT></TD>
                                <TD style="HEIGHT: 17px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="17"><FONT face="����">&nbsp;���׵���״̬:</FONT></TD>
                                <TD style="HEIGHT: 17px" width="132" bgColor="#ffffff" height="17" colSpan="2"><FONT face="����">&nbsp;
                                        <asp:label id="LB_Flstate" runat="server" ForeColor="Red"></asp:label></FONT></TD>
                                <%-- <TD style="HEIGHT: 17px" width="135" background="../IMAGES/Page/bg_bl.gif" bgColor="#e4e5f7"
                                    height="17">&nbsp;
                                   <asp:linkbutton id="LinkButton3_action" runat="server" onclick="LinkButton3_action_Click">����</asp:linkbutton></TD>--%>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18">&nbsp;���ִ���:</TD>
                                <TD style="WIDTH: 129px; HEIGHT: 18px" bgColor="#ffffff" height="18">&nbsp;
                                    <asp:label id="LB_Fcurtype" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18">&nbsp;֧������:</TD>
                                <TD style="HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="����">
                                        <asp:label id="LB_Fpay_type" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="����">&nbsp;����޸Ľ��׵���IP:</FONT></TD>
                                <TD style="WIDTH: 129px; HEIGHT: 15px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
                                        <asp:label id="LB_Fip" runat="server"></asp:label></FONT></TD>
                                <TD style="HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="����">&nbsp;����޸�ʱ��(����)</FONT></TD>
                                <TD style="HEIGHT: 15px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="����">&nbsp;
                                        <asp:label id="LB_Fmodify_time" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="����">&nbsp;����ʱ��:</FONT></TD>
                                <TD style="WIDTH: 129px; HEIGHT: 15px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
                                        <asp:label id="LB_Fcreate_time" runat="server"></asp:label></FONT></TD>
                                <TD style="HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="����">&nbsp;�������</FONT></TD>
                                <TD style="HEIGHT: 15px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="����">&nbsp;
                                        <asp:label id="LB_Fchannel_id" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;�н�ID:</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="����">&nbsp;</FONT>
                                    <asp:label id="LB_Fmediuid" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;�н����:</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="����">&nbsp;
                                        <asp:label id="LB_Fmedinum" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;�������˻�:</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="����">&nbsp;</FONT>
                                    <asp:label id="LB_Fchargeuid" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;������:</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="����">&nbsp;
                                        <asp:label id="LB_Fchargenum" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;�ܽ��:</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="����">&nbsp;</FONT>
                                    <asp:label id="LB_Ftotalnum" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;�����֧���ܽ��:</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="����">&nbsp;
                                        <asp:label id="LB_Fbuyerpaytotal" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;������˿��ܽ��:</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="����">&nbsp;</FONT>
                                    <asp:label id="LB_Fbuyerrefundtotal" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;�������յ��ܽ��:</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="����">&nbsp;
                                        <asp:label id="LB_Fsellerpaytotal" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;�������˿��ܽ��:</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="����">&nbsp;</FONT>
                                    <asp:label id="LB_Fsellerrefundtotal" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;���뷽����:</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="����">&nbsp;
                                        <asp:label id="LB_Frolenum" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;�ڲ�ID(0):</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="����">&nbsp;</FONT>
                                    <asp:label id="LB_Fuid0" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;Ԥ�ƽ��(0):</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="����">&nbsp;
                                        <asp:label id="LB_Fplanpaynum0" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;ʵ�ʽ��(0):</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="����">&nbsp;</FONT>
                                    <asp:label id="LB_Fpaynum0" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;�˿���(0):</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="����">&nbsp;
                                        <asp:label id="LB_Frefund0" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;�ڲ�ID(1):</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="����">&nbsp;</FONT>
                                    <asp:label id="LB_Fuid1" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;Ԥ�ƽ��(1):</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="����">&nbsp;
                                        <asp:label id="LB_Fplanpaynum1" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;ʵ�ʽ��(1):</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="����">&nbsp;</FONT>
                                    <asp:label id="LB_Fpaynum1" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;�˿���(1):</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="����">&nbsp;
                                        <asp:label id="LB_Frefund1" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;�ڲ�ID(2):</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="����">&nbsp;</FONT>
                                    <asp:label id="LB_Fuid2" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;Ԥ�ƽ��(2):</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="����">&nbsp;
                                        <asp:label id="LB_Fplanpaynum2" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;ʵ�ʽ��(2):</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="����">&nbsp;</FONT>
                                    <asp:label id="LB_Fpaynum2" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;�˿���(2):</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="����">&nbsp;
                                        <asp:label id="LB_Frefund2" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;�ڲ�ID(3):</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="����">&nbsp;</FONT>
                                    <asp:label id="LB_Fuid3" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;Ԥ�ƽ��(3):</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="����">&nbsp;
                                        <asp:label id="LB_Fplanpaynum3" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;ʵ�ʽ��(3):</TD>
                                <TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="����">&nbsp;</FONT>
                                    <asp:label id="LB_Fpaynum3" runat="server"></asp:label></TD>
                                <TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="19">&nbsp;�˿���(3):</TD>
                                <TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="����">&nbsp;
                                        <asp:label id="LB_Frefund3" runat="server"></asp:label></FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="����">&nbsp;��̨��ע:</FONT></TD>
                                <TD bgColor="#ffffff" colSpan="4" height="18"><FONT face="����">&nbsp;
                                        <asp:label id="LB_Fexplain" runat="server"></asp:label></FONT><FONT face="����">&nbsp;</FONT><FONT face="����">&nbsp;</FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="����">&nbsp;��ע:</FONT></TD>
                                <TD bgColor="#ffffff" colSpan="4" height="18"><FONT face="����">&nbsp;
                                        <asp:label id="LB_FMemo" runat="server"></asp:label></FONT><FONT face="����">&nbsp;</FONT><FONT face="����">&nbsp;</FONT></TD>
                            </TR>
                            <TR>
                                <TD style="WIDTH: 154px; HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
                                    height="18"><FONT face="����">&nbsp;ҵ��������:</FONT></TD>
                                <TD bgColor="#ffffff" colSpan="4" height="18"><FONT face="����">&nbsp;
                                        <asp:label id="LB_Fcatch_desc" runat="server"></asp:label></FONT><FONT face="����">&nbsp;</FONT><FONT face="����">&nbsp;</FONT></TD>
                            </TR>
                        </TABLE>
                    </TD>
                </TR>
                <TR>
                    <TD><asp:label id="Label37" runat="server" ForeColor="Blue">������ˮ</asp:label></TD>
                </TR>
                <TR width="100%">
                    <TD><asp:datagrid id="DataGrid2" runat="server" Width="100%" CellPadding="3" BackColor="White" BorderWidth="1px"
                            BorderStyle="None" BorderColor="#CCCCCC" AutoGenerateColumns="False">
                            <FooterStyle ForeColor="#000066" BackColor="White"></FooterStyle>
                            <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#669999"></SelectedItemStyle>
                            <ItemStyle ForeColor="#000066"></ItemStyle>
                            <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#006699"></HeaderStyle>
                            <Columns>
                                <asp:BoundColumn DataField="Fqqid" HeaderText="�û��ʺ�"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="FtypeName" HeaderText="��������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FsubjectName" HeaderText="�������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ffromid" HeaderText="�Է��ʺ�"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FpaynumName" HeaderText="���"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FpaybuyName" HeaderText="�����/�ֽ�"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FpaysaleName" HeaderText="������/�ֽ�ȯ"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcreate_time" HeaderText="����ʱ��"></asp:BoundColumn>
                            </Columns>
                            <PagerStyle HorizontalAlign="Left" ForeColor="#000066" BackColor="White" Mode="NumericPages"></PagerStyle>
                        </asp:datagrid></TD>
                </TR>
                <%--<TR>
                    <TD><asp:label id="Label43" runat="server" ForeColor="Blue">������Ϣ</asp:label></TD>
                </TR>
                <TR>
                    <TD><asp:datagrid id="Datagrid3" runat="server" Width="100%" CellPadding="3" BackColor="White" BorderWidth="1px"
                            BorderStyle="None" BorderColor="#CCCCCC" AutoGenerateColumns="False">
                            <FooterStyle ForeColor="#000066" BackColor="White"></FooterStyle>
                            <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#669999"></SelectedItemStyle>
                            <ItemStyle ForeColor="#000066"></ItemStyle>
                            <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#006699"></HeaderStyle>
                            <Columns>
                                <asp:BoundColumn DataField="Fqqid" HeaderText="�ʺ�"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ftransport_typeName" HeaderText="��������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fgoods_typeName" HeaderText="��Ʒ����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ftran_typeName" HeaderText="������������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ftransport_id" HeaderText="��������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Frecv_truename" HeaderText="�ջ�����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fsend_truename" HeaderText="��������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FstateName" HeaderText="������״̬"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcreate_time" HeaderText="����ʱ��"></asp:BoundColumn>
                            </Columns>
                            <PagerStyle HorizontalAlign="Left" ForeColor="#000066" BackColor="White" Mode="NumericPages"></PagerStyle>
                        </asp:datagrid></TD>
                </TR>--%>
                <TR>
                    <TD><asp:label id="Label29" runat="server" ForeColor="Blue">Ͷ�ߵ���Ϣ</asp:label></TD>
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
                                    DataTextField="Fappealid" HeaderText="Ͷ�ߵ���"></asp:HyperLinkColumn>
                                <asp:BoundColumn DataField="Fqqid" HeaderText="�ʻ�����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fvs_qqid" HeaderText="�Է��ʺ�"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FstateName" HeaderText="Ͷ�ߵ�״̬"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fpunish_flagName" HeaderText="�������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcheck_stateName" HeaderText="�÷���˱��"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fappeal_typeName" HeaderText="Ͷ�����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fappeal_time" HeaderText="Ͷ��ʱ��"></asp:BoundColumn>
                            </Columns>
                            <PagerStyle HorizontalAlign="Left" ForeColor="#000066" BackColor="White" Mode="NumericPages"></PagerStyle>
                        </asp:datagrid></TD>
                </TR>
            </TABLE>
        </form>
    </BODY>
</HTML>
