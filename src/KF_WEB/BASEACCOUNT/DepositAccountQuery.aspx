<%@ Page language="c#" Codebehind="DepositAccountQuery.aspx.cs" AutoEventWireup="false" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.DepositAccountQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
    <HEAD>
        <title>��֤���˻���ѯ</title>
        <style type="text/css">
           @import url( ../styles/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); 
           .style2 { COLOR: #ff0000; FONT-WEIGHT: bold }
           .style3 { COLOR: #000000 }
           .style4 { COLOR: #ff0000 }
           BODY { BACKGROUND-IMAGE: url(../images/page/bg01.gif) }
           </style>
        <script language="javascript" src="../js/common.js"></script>
    </HEAD>
    <body>
        <form id="form1" method="post" runat="server">
            <table cellspacing="1" cellpadding="0" width="95%" align="center" bgcolor="#666666" border="0">
                <tr bgcolor="#e4e5f7" background="../images/page/bg_bl.gif">
                    <td valign="middle" colspan="2" height="20">
                        <table height="90%" cellspacing="0" cellpadding="1" width="100%" background="../images/page/bg_bl.gif"
                            border="0">
                            <tr background="../images/page/bg_bl.gif">
                                <td width="80%" height="18"><font color="#ff0000"><strong><font color="#ff0000">&nbsp;</font></strong><img height="16" src="../images/page/post.gif" width="20">
                                        ��֤���̻���ѯ</font>
                                    <div align="right"></div>
                                </td>
                                <td width="20%">����Ա����:
                                    <span class="style4">
                                        <asp:label id="lblOperatorId" runat="server">0755688</asp:label>
                                    </span></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr bgcolor="#ffffff">
                    <td width="842">
                        <div align="center"></div>
                        <div align="left">
                            <table height="100%" cellspacing="0" cellpadding="1" width="100%" border="0">
                                <tr>
                                    <td width="19%">&nbsp;</td>
                                    <td width="542"><span class="need">*</span>�̻��ţ�&nbsp;
                                        <asp:textbox id="txtSPId" runat="server" borderwidth="1px" borderstyle="solid"></asp:textbox>&nbsp;</td>
                                    <td width="542">��֤���ʺţ�&nbsp;
                                        <asp:textbox id="txtDepositAccountId" runat="server" borderwidth="1px" borderstyle="solid" Width="300px"></asp:textbox>&nbsp;</td>
                                </tr>
                            </table>
                        </div>
                        <div align="left"></div>
                        <div align="center"></div>
                    </td>
                    <td width="25%">
                        <div align="center">&nbsp;
                            <asp:button id="btnQuery" runat="server" text="�� ѯ"></asp:button></div>
                    </td>
                </tr>
            </table>
            <br>
            <table cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
                <tr>
                    <td bgcolor="#666666">
                        <table cellspacing="1" cellpadding="0" width="100%" align="center" border="0">
                            <tr bgcolor="#e4e5f7">
                                <td background="../images/page/bg_bl.gif" height="20"><font color="#ff0000"><img height="16" src="../images/page/post.gif" width="20">&nbsp;&nbsp;<font face="����">��ϸ</font>
                                    </font>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#ffffff" height="12">
                                    <table class="nomal" cellpadding="3">
                                        <asp:Repeater ID="rptAccountList" Runat="server">
                                            <HeaderTemplate>
                                                <tr>
                                                    <th>�̻���SPID</td>
                                                    <th>�̻��ڲ�UID</td>
                                                    <th>��֤������</th>
                                                    <th>���ɷ�ʽ</th>
                                                    <th>��֤���ʺ�</th>
                                                    <th>��֤���˻��ڲ�UID</th>
                                                    <th>��ʼ��֤��Ԫ��</th>
                                                    <th>��֤���˻���Ԫ��</th>
                                                    <th>������</th>
                                                    <th>�ʽ���ˮ</th>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%#((System.Data.DataRowView)Container.DataItem).Row["Fuin"]%></td>
                                                    <td><%#((System.Data.DataRowView)Container.DataItem).Row["Fuid"]%></td>
                                                    <td><%#((System.Data.DataRowView)Container.DataItem).Row["Finsure_type_str"]%></td>
                                                    <td><%#((System.Data.DataRowView)Container.DataItem).Row["Finsure_add_type_str"]%></td>
                                                    <td><%#((System.Data.DataRowView)Container.DataItem).Row["Finsure_uin"]%></td>
                                                    <td><%#((System.Data.DataRowView)Container.DataItem).Row["Finsure_uid"]%></td>
                                                    <td><%#((System.Data.DataRowView)Container.DataItem).Row["Finsure_conf_amount"]%></td>
                                                    <td><%#((System.Data.DataRowView)Container.DataItem).Row["balance"]%></td>
                                                    <td><%#((System.Data.DataRowView)Container.DataItem).Row["ratio"]%></td>
                                                    <td><a href="bankrollLog.aspx?type=QQID&currtype=1&qqid=<%#((System.Data.DataRowView)Container.DataItem).Row["Finsure_uin"]%>" target="iframeList">�鿴</a></td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                               <tr class="alter">
                                                    <td><%#((System.Data.DataRowView)Container.DataItem).Row["Fuin"]%></td>
                                                    <td><%#((System.Data.DataRowView)Container.DataItem).Row["Fuid"]%></td>
                                                    <td><%#((System.Data.DataRowView)Container.DataItem).Row["Finsure_type_str"]%></td>
                                                    <td><%#((System.Data.DataRowView)Container.DataItem).Row["Finsure_add_type_str"]%></td>
                                                    <td><%#((System.Data.DataRowView)Container.DataItem).Row["Finsure_uin"]%></td>
                                                    <td><%#((System.Data.DataRowView)Container.DataItem).Row["Finsure_uid"]%></td>
                                                    <td><%#((System.Data.DataRowView)Container.DataItem).Row["Finsure_conf_amount"]%></td>
                                                    <td><%#((System.Data.DataRowView)Container.DataItem).Row["balance"]%></td>
                                                    <td><%#((System.Data.DataRowView)Container.DataItem).Row["ratio"]%></td>
                                                    <td><a href="bankrollLog.aspx?type=QQID&currtype=1&qqid=<%#((System.Data.DataRowView)Container.DataItem).Row["Finsure_uin"]%>" target="iframeList">�鿴</a></td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br/>
            <table width="95%" align="center" border="1">
               <tr bgcolor="#e4e5f7">
                  <td background="../images/page/bg_bl.gif" height="20"><font color="#ff0000"><img height="16" src="../images/page/post.gif" width="20">&nbsp;&nbsp;��ˮ��ϸ</font>
                  </td>
              </tr>
               <tr>
                  <td bgcolor="#ffffff"><iframe id="iframe0" name="iframeList" marginwidth="0" marginheight="0"  frameborder="0" width="100%" height="300"></iframe>
                  </td>
              </tr>
            </table>
        </form>
    </body>
</HTML>
