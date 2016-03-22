<%@ Page Language="c#" CodeBehind="InfoCenter.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.InfoCenter" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>InfoCenter</title>
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );
        .style2
        {
            color: #000000;
        }
        .style3
        {
            color: #ff0000;
        }
        BODY
        {
            background-image: url(../IMAGES/Page/bg01.gif);
        }
    </style>
    <script language="javascript" type="text/javascript">

        function IsNumber(string, sign) {
            var number;
            if (string == null)
                return false;

            number = new Number(string);
            if (isNaN(number)) {
                return false;
            }
            else {
                return true;
            }
        }

        function checkvlid() {
            with (Form1) {
                if (TextBox1_InputQQ.value == "") {
                    alert("�������û��ʻ���!!");
                    TextBox1_InputQQ.focus();
                    return false;
                }
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <table cellspacing="1" cellpadding="0" width="95%" align="center" bgcolor="#666666"
        border="0">
        <tr bgcolor="#e4e5f7" style="background-image: ../IMAGES/Page/bg_bl.gif">
            <td valign="middle" colspan="2" height="20">
                <table style="height: 90%" cellspacing="0" cellpadding="1" width="97%" border="0">
                    <tr>
                        <td width="80%" style="background-image: ../IMAGES/Page/bg_bl.gif" height="18">
                            <font color="#ff0000"><strong><font color="#ff0000">&nbsp;</font></strong><img height="16"
                                src="../IMAGES/Page/post.gif" width="20">
                                �û���ѯ</font>
                        </td>
                        <td width="20%" style="background-image: ../IMAGES/Page/bg_bl.gif">
                            ����Ա����: <span class="style3">
                                <asp:Label ID="Label_uid" runat="server" Width="73px"></asp:Label></span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr bgcolor="#ffffff">
            <td>
                <table style="height: 100%" cellspacing="0" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td style="padding-left: 100px">
                            ���룺&nbsp;
                            <asp:TextBox ID="TextBox1_InputQQ" runat="server"></asp:TextBox>
                            &nbsp;
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Width="150px"
                                Display="Dynamic" ErrorMessage="RequiredFieldValidator" ControlToValidate="TextBox1_InputQQ">�������û��ʺ�</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <input id="CFT" name="IDType" runat="server" type="radio" checked /><label for="CFT">C�˺�</label>
                            <input id="InternalID" name="IDType" runat="server" type="radio" /><label for="InternalID">�ڲ��˺�</label>
                            
                        </td>
                    </tr>
                </table>
            </td>
            <td width="25%">
                <div align="center">
                    <asp:CheckBox ID="CheckBox1" runat="server" Text="��ʷ��¼"></asp:CheckBox>&nbsp;
                    <asp:Button ID="Button1" runat="server" Text="�� ѯ" OnClick="Button1_Click"></asp:Button></div>
            </td>
        </tr>
    </table>
    <br />
    <table height="127" cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
        <tr>
            <td bgcolor="#666666">
                <table height="192" cellspacing="1" cellpadding="0" width="100%" align="center" border="0">
                    <tr bgcolor="#e4e5f7" background="../IMAGES/Page/bg_bl.gif">
                        <td background="../IMAGES/Page/bg_bl.gif" colspan="5" height="20">
                            <strong><font color="#ff0000">&nbsp;<img height="16" src="../IMAGES/Page/post.gif"
                                width="20">
                            </font></strong><font color="#ff0000">�û��˻�����</font>
                            <div align="center">
                                <font face="����"></font>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 3px" width="20%" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="3">
                            &nbsp;QQ �˺�:
                        </td>
                        <td style="height: 3px" bgcolor="#ffffff" height="3">
                            &nbsp;<span class="style2">
                                <asp:Label ID="Label1_Acc" runat="server">22000254</asp:Label></span>
                        </td>
                        <td style="height: 8px" width="11%" background="../IMAGES/Page/bg_bl.gif" bgcolor="#e4e5f7"
                            height="8">
                            <asp:Label ID="labQQstate" runat="server"></asp:Label>
                        </td>
                        <td style="height: 3px" width="24%" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="3">
                            &nbsp;<font face="����">��ʵ����:</font>
                        </td>
                        <td style="height: 3px" bgcolor="#ffffff" height="3">
                            &nbsp;<font face="����">
                                <asp:Label ID="Label14_Ftruename" runat="server">����</asp:Label></font> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton runat="server" ID="lbtn_synName" ForeColor="blue" OnClick="SyncUserNameClick">ͬ������</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 3px" width="20%" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="3">
                               EMAIL�˺�:
                        </td>
                        <td style="height: 3px" bgcolor="#ffffff" height="3">
                            &nbsp;<span class="style2">
                                <asp:Label ID="labEmail" runat="server">at126@126.com</asp:Label></span>
                        </td>
                        <td style="height: 8px" width="11%" background="../IMAGES/Page/bg_bl.gif" bgcolor="#e4e5f7"
                            height="8">
                            <asp:Label ID="labEmailState" runat="server"></asp:Label>
                        </td>
                        <td style="height: 3px" width="24%" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="3">
                            &nbsp;<font face="����">�ڲ�ID:</font>
                        </td>
                        <td style="height: 3px" bgcolor="#ffffff" height="3">
                            &nbsp;<font face="����">
                                <asp:Label ID="lbInnerID" runat="server"></asp:Label></font>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 13px" width="20%" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="13">
                            <font face="����">&nbsp;�ֻ��ʺ�:</font>
                        </td>
                        <td style="height: 13px" bgcolor="#ffffff" height="13">
                            <font face="����">&nbsp;
                                <asp:Label ID="labMobile" runat="server"></asp:Label></font>
                        </td>
                        <td style="height: 8px" width="11%" background="../IMAGES/Page/bg_bl.gif" bgcolor="#e4e5f7"
                            height="8">
                            <asp:Label ID="labMobileState" runat="server"></asp:Label>
                        </td>
                        <td style="height: 13px" width="24%" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="13">
                            <font face="����">&nbsp;���֧��״̬:</font>
                        </td>
                        <td style="height: 13px" bgcolor="#ffffff" height="13">
                            <font face="����">&nbsp;</font>
                            <asp:Label ID="lbLeftPay" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 8px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="8">
                            <font face="����">&nbsp;�ʻ�״̬:</font>
                        </td>
                        <td style="height: 8px" width="18%" bgcolor="#ffffff" height="8">
                            <font face="����">&nbsp;
                                <asp:Label ID="Label12_Fstate" runat="server">����</asp:Label></font><font face="����"></font>
                        </td>
                        <td style="height: 8px" width="11%" background="../IMAGES/Page/bg_bl.gif" bgcolor="#e4e5f7"
                            height="8">
                            <div align="center">
                                <asp:LinkButton ID="LinkButton3" runat="server" OnClick="LinkButton3_Click"></asp:LinkButton></div>
                        </td>
                        <td style="height: 8px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="8">
                            <font face="����">&nbsp;�ʻ�����:</font>
                        </td>
                        <td style="height: 8px" bgcolor="#ffffff" height="8">
                            <font face="����">&nbsp;
                                <asp:Label ID="Label13_Fuser_type" runat="server">����</asp:Label></font><font face="����"></font>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 15px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="15">
                            &nbsp;<font face="����">�������</font>:
                        </td>
                        <td style="height: 15px" bgcolor="#ffffff" height="15">
                            &nbsp;
                            <asp:Label ID="Label15_Useable" runat="server" Width="100px" ForeColor="Red"></asp:Label><font
                                face="����"></font>
                        </td>
                         <td style="height: 8px" bgcolor="#ffffff" height="15">
                            &nbsp;
                            <asp:Label ID="Label19_OpenOrNot" runat="server" Width="100px" ForeColor="Red"></asp:Label><font
                                face="����"></font>
                        </td>
                        <td style="height: 15px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="15">
                            &nbsp;������:
                        </td>
                        <td style="height: 15px" bgcolor="#ffffff" height="15">
                            &nbsp;
                            <asp:Label ID="Label4_Freeze" runat="server" Width="120px"></asp:Label>
                            <span style="margin-left:20px;">
                                <span>���˶�����:</span>
                                <asp:Label ID="lb_Freeze_amt" runat="server" Width="120px">0</asp:Label>
                            </span>
                        </td>
                    </tr>
                    <tr>
                       <%-- <td style="height: 4px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="4">
                            &nbsp;�������:
                        </td>
                        <td style="height: 4px" bgcolor="#ffffff" colspan="2" height="4">
                            &nbsp;
                            <asp:Label ID="Label5_YestodayLeft" runat="server">10</asp:Label>
                        </td>--%>
                        <td style="height: 4px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="4">
                            &nbsp;�ʻ����<font face="����">:</font>
                        </td>
                        <td style="height: 4px" bgcolor="#ffffff" height="4">
                            &nbsp;<font face="����">
                                <asp:Label ID="Label3_LeftAcc" runat="server" Width="180px">3000</asp:Label></font>
                        </td>
                        <td style="height: 4px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="4"></td>
                        <td style="height: 4px" bgcolor="#ffffff" colspan="2" height="4"></td>
                    </tr>
                    <tr>
                        <td style="height: 4px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="4">
                            <font face="����">&nbsp;��������:</font>
                        </td>
                        <td style="height: 4px" bgcolor="#ffffff" colspan="2" height="4">
                            <font face="����">&nbsp;</font>
                            <asp:Label ID="Label2_Type" runat="server">����ȯ</asp:Label>
                        </td>
                        <td style="height: 4px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="4">
                            <font face="����">&nbsp;ע��ʱ��:</font>
                        </td>
                        <td style="height: 4px" bgcolor="#ffffff" height="4">
                            <font face="����">&nbsp;
                                <asp:Label ID="lblLoginTime" runat="server">10</asp:Label></font>
                        </td>
                    </tr>
          <%--          <tr>
                        <td style="height: 4px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="4">
                            <font face="����">&nbsp;�����Ѹ�:</font>
                        </td>
                        <td style="height: 4px" bgcolor="#ffffff" height="4" colspan="2">
                            <font face="����">&nbsp;
                                <asp:Label ID="Label16_Fapay" runat="server">10</asp:Label></font>
                        </td>
                        <td style="height: 13px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee">
                            &nbsp;���ʽ����޶�:
                        </td>
                        <td style="height: 13px" bgcolor="#ffffff" height="13">
                            &nbsp;
                            <asp:Label ID="Label7_SingleMax" runat="server">2000</asp:Label>
                        </td>
                    </tr>--%>
               <%--     <tr>
                        <td style="height: 13px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="13">
                            &nbsp;����֧���޶�:
                        </td>
                        <td style="height: 13px" bgcolor="#ffffff" height="13" colspan="2">
                            &nbsp;
                            <asp:Label ID="Label8_PerDayLmt" runat="server">5000</asp:Label>
                        </td>
                        <td style="height: 13px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee">
                            <font face="����">&nbsp;�������ֽ��:</font>
                        </td>
                        <td style="height: 13px" bgcolor="#ffffff" height="13">
                            <font face="����">&nbsp;
                                <asp:Label ID="lbFetchMoney" runat="server"></asp:Label></font>
                        </td>
                    </tr>--%>
                   <%-- <tr>
                        <td style="height: 13px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="13">
                            <font face="����">&nbsp;�����ѳ�ֵ���:</font>
                        </td>
                        <td style="height: 13px" bgcolor="#ffffff" height="13" colspan="2">
                            <font face="����">&nbsp;
                                <asp:Label ID="lbSave" runat="server"></asp:Label></font>
                        </td>
                        <td style="height: 6px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="6">
                            &nbsp;����������:
                        </td>
                        <td style="height: 6px" bgcolor="#ffffff" height="6">
                            &nbsp;
                            <asp:Label ID="Label9_LastSaveDate" runat="server">2005-03-01</asp:Label>
                        </td>
                    </tr>--%>
                    <tr>
                       <%-- <td style="height: 6px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="6">
                            &nbsp;����������:
                        </td>
                        <td style="height: 6px" bgcolor="#ffffff" height="6" colspan="2">
                            &nbsp;
                            <asp:Label ID="Label10_Drawing" runat="server">2005-04-15</asp:Label>
                        </td>--%>
                        <td style="height: 14px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="14">
                            <font face="����">&nbsp;����½IP��ַ:</font>
                        </td>
                        <td style="height: 14px" bgcolor="#ffffff" height="14">
                            <font face="����">&nbsp;
                                <asp:Label ID="Label17_Flogin_ip" runat="server">202.103.24.68</asp:Label></font>
                        </td>
                        <td style="height: 4px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="4"></td>
                        <td style="height: 4px" bgcolor="#ffffff" colspan="2" height="4"></td>
                    </tr>
                    <tr>
                        <td style="height: 14px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="14">
                            <font face="����">&nbsp;����޸�ʱ��:</font>
                        </td>
                        <td style="height: 14px" bgcolor="#ffffff" height="14" colspan="2">
                            <font face="����">&nbsp;</font>
                            <asp:Label ID="Label6_LastModify" runat="server">2005-05-01</asp:Label>
                        </td>
                        <td style="height: 14px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="14">
                            <font face="����">&nbsp;��Ʒ����:</font>
                        </td>
                        <td style="height: 14px" bgcolor="#ffffff" height="14">
                            <font face="����">&nbsp;</font>
                            <asp:Label ID="Label18_Attid" runat="server">BB</asp:Label>
                        </td>
                    </tr>
                   <%-- <tr>
                        <td style="height: 14px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="14">
                            <font face="����">&nbsp;�Ƹ�ֵ:</font>
                        </td>
                        <td style="height: 14px" bgcolor="#ffffff" height="14" colspan="2">
                            <font face="����">&nbsp;</font>
                            <asp:Label ID="vip_value" runat="server"></asp:Label>
                        </td>
                        <td style="height: 14px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="14">
                            <font face="����">&nbsp;��Ա����:</font>
                        </td>
                        <td style="height: 14px" bgcolor="#ffffff" height="14">
                            <font face="����">&nbsp;</font>
                            <asp:Label ID="vip_flag" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 14px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="14">
                            <font face="����">&nbsp;��Ա�ȼ�:</font>
                        </td>
                        <td style="height: 14px" bgcolor="#ffffff" height="14" colspan="2">
                            <font face="����">&nbsp;</font>
                            <asp:Label ID="vip_level" runat="server"></asp:Label>
                        </td>
                        <td style="height: 14px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="14">
                            <font face="����">&nbsp;��Ա��ͨ��ʽ:</font>
                        </td>
                        <td style="height: 14px" bgcolor="#ffffff" height="14">
                            <font face="����">&nbsp;</font>
                            <asp:Label ID="vip_channel" runat="server"></asp:Label>
                        </td>
                    </tr>--%>
                    <tr>
                        <%--<td style="height: 14px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="14">
                            <font face="����">&nbsp;��Ա�ر�ʱ��:</font>
                        </td>
                        <td style="height: 14px" bgcolor="#ffffff" height="14" colspan="2">
                            <font face="����">&nbsp;</font>
                            <asp:Label ID="vip_exp_date" runat="server"></asp:Label>
                        </td>--%>

                        
                        <td background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee" height="10">
                            &nbsp;��ע:
                        </td>
                        <td bgcolor="#ffffff" height="10" colspan="2">
                            &nbsp;
                            <asp:Label ID="Label11_Remark" runat="server">����һ������ʲô��û����������</asp:Label>
                        </td>
                        
                        <td style="height: 14px" background="../IMAGES/Page/bk_white.gif" bgcolor="#eeeeee"
                            height="14">
                            <font face="����">&nbsp;ʵ����֤:</font>
                        </td>
                        <td style="height: 14px" bgcolor="#ffffff" height="14" colspan="3">
                            <font face="����">&nbsp;</font>
                            <asp:Label ID="labUserClassInfo" runat="server"></asp:Label>
                        </td>
                        
                    </tr>
                    <tr>
                        <td  style="height: 14px" bgcolor="#ffffff" height="14" colspan="4"></td>
                        <td style="height: 16px" bgcolor="#ffffff" height="16">
                            <font face="����">
                                <asp:Button ID="btnDelClass" runat="server" Text="ɾ����֤" Visible="False" OnClick="btnDelClass_Click">
                                </asp:Button></font>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br>
    <table cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
        <tr>
            <td valign="top" align="center">
                <asp:DataGrid ID="dgList" runat="server" Width="1150px" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                    GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                    BorderColor="#E7E7FF" AllowPaging="True" OnPageIndexChanged="dgList_PageIndexChanged">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                    </HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="Fqqid" HeaderText="�ʺ�"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fmemo" HeaderText="��ע"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fauthen_operator" HeaderText="������Ա"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fmodify_time" HeaderText="����޸�ʱ��"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <table height="35" cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
        <tr>
            <td bgcolor="#666666">
                <table height="35" cellspacing="1" cellpadding="0" width="100%" align="center" border="0">
                    <tr bgcolor="#e4e5f7">
                        <td background="../IMAGES/Page/bg_bl.gif" bgcolor="#e4e5f7" colspan="3" height="20">
                            <font color="#ff0000">&nbsp;<img height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;
                                <asp:LinkButton ID="LKBT_TradeLog" runat="server" ForeColor="Red" OnClick="LKBT_TradeLog_Click">��ҽ��׵�</asp:LinkButton></font>|
                            <span class="style2">
                                    <asp:LinkButton ID="LKBT_TradeLog_Sale" runat="server" ForeColor="Black" OnClick="LKBT_TradeLog_Sale_Click">���ҽ��׵�</asp:LinkButton>|
                                    <%--<asp:LinkButton ID="LKBT_TradeLog_Unfinished" runat="server" ForeColor="Black" OnClick="LKBT_TradeLog_Unfinished_Click">���δ��ɽ��׵�</asp:LinkButton>|--%>
                                    <%--<asp:LinkButton ID="LKBT_TradeLog_Sale_Unfinished" runat="server" ForeColor="Black" OnClick="LKBT_TradeLog_Sale_Unfinished_Click">����δ��ɽ��׵�</asp:LinkButton>|--%>
                                    <asp:LinkButton ID="LKBT_bankrollLog" runat="server" ForeColor="Black" OnClick="LKBT_bankrollLog_Click">�û��ʽ���ˮ</asp:LinkButton>|
                                    <asp:LinkButton ID="LKBT_GatheringLog" runat="server" ForeColor="Black" OnClick="LKBT_GatheringLog_Click">��ֵ��¼</asp:LinkButton>|
                                    <asp:LinkButton ID="LkBT_PaymentLog" runat="server" ForeColor="Black" OnClick="LkBT_PaymentLog_Click">���ּ�¼</asp:LinkButton>&nbsp;|
                                    <%--<asp:LinkButton ID="LkBT_Refund" runat="server" ForeColor="Black" OnClick="LkBT_Refund_Click">����˿</asp:LinkButton>&nbsp;|--%>
                                    <%--<asp:LinkButton ID="LkBT_Refund_Sale" runat="server" ForeColor="Black" OnClick="LkBT_Refund_Sale_Click">�����˿</asp:LinkButton>&nbsp;|--%>
                                    <%--<asp:LinkButton ID="LkBT_ButtonInfo" runat="server" ForeColor="Black" OnClick="LkBT_ButtonInfo_Click">�̼ҹ��߰�ť</asp:LinkButton>&nbsp;|--%>
                                    <%--<asp:LinkButton ID="LkBT_Gwq" runat="server" ForeColor="Black" OnClick="LkBT_Gwq_Click">�Ƹ�ȯ</asp:LinkButton>&nbsp;|--%>
                                    <%--<asp:LinkButton ID="LkBT_mediOrder" runat="server" ForeColor="Black" OnClick="LkBT_mediOrder_Click">�н齻��</asp:LinkButton>--%>
                            </span>
                            <div align="center">
                                <font face="Tahoma"></font>
                            </div>
                        </td>
                        <td width="30" background="../IMAGES/Page/bg_bl.gif" height="20">
                            &nbsp;
                        </td>
                        <td align="right" width="30" background="../IMAGES/Page/bg_bl.gif">
                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="../Images/Page/down.gif">
                            </asp:ImageButton>
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="#ffffff" colspan="5" height="12">
                            <iframe id="iframe0" name="iframe0" marginwidth="0" marginheight="0" src="<%=iFramePath%>"
                                frameborder="0" width="100%" height="20"></iframe>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
