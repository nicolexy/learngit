<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransferQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.TransferQuery" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>转账单查询</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );

        .style2 {
            COLOR: #000000;
        }

        .style3 {
            COLOR: #ff0000;
        }

        BODY {
            BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif);
        }

        TD {
            FONT-SIZE: 9pt;
        }

        .style4 {
            COLOR: #ff0000;
        }

        table {
            background-color: grey;
        }

        td, th {
            background-color: white;
            padding: 2px;
        }

        #tab_transfer_info td:nth-child(1), #tab_transfer_info td:nth-child(3) {
            width: 150px;
            text-align: right;
        }
    </style>
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <table id="table1" border="0" cellspacing="1" cellpadding="0" width="900" runat="server">
            <tr>
                <td width="15%">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16" alt="" />
                    <span class="style3">转账单查询</span>
                </td>
                <td colspan="2">
                    <label class="style3">操作员ID：</label>
                    <asp:Label ID="lb_operatorID" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">                    
                    <asp:RadioButton ID="check_tenpay" runat="server" GroupName="chekedType" Text="财付通订单" Checked="true" />
                </td>
                <td colspan="2">
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span>单号：</span>
                    <asp:TextBox ID="txt_order" runat="server" Width="250"></asp:TextBox>
                </td>
            </tr>
            <tr style="border: 0">
                <td>
                     <asp:RadioButton ID="check_sp" runat="server" GroupName="chekedType" Text="商户订单号" />
                </td>
                <td>
                    <span>商户订单号：</span>
                    <asp:TextBox ID="txt_SpOrder" runat="server" Width="250"></asp:TextBox>
                </td>
                 <td>
                    <span>财付通账户：</span>
                    <asp:TextBox ID="txt_CaiFuTongAccount" runat="server" Width="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="border: 0">
                    <span style="line-height:2em">功能说明：查询微信/手Q的红包、转账、AA的入账单详情。</span>
                    <asp:Button ID="Button1" runat="server" Text="查询" Style="margin-left: 100px;" OnClick="Button1_Click" />
                    <table style="width: 100%;" id="tab_transfer_info" cellspacing="1" cellpadding="0" runat="server"  visible="false">
                        <caption style="text-align: left; background: #b0c3d1; padding: 4px;">
                            转账单详情：
                        </caption>
                        <tr>
                            <td>交易单号：</td>
                            <td>
                                <asp:Label ID="lab_Flistid" runat="server" />
                            </td>
                            <td>单类型：</td>
                            <td>
                                <asp:Label ID="lab_Flist_type_str" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>本次交易总金额：</td>
                            <td>
                                <asp:Label ID="lab_Ftotalnum_str" runat="server" />
                            </td>
                            <td>业务侧重入检查标记</td>
                            <td>
                                <asp:Label ID="lab_Fbussredo_sign" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>买家财付通账号：</td>
                            <td>
                                <asp:Label ID="lab_Fbuy_uin" runat="server" />
                            </td>
                            <td>卖家财付通账号：</td>
                            <td>
                                <asp:Label ID="lab_Fsale_uin" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>买家内部账号：</td>
                            <td>
                                <asp:Label ID="lab_Fbuy_uid" runat="server" />
                            </td>
                            <td>卖家内部账号：</td>
                            <td>
                                <asp:Label ID="lab_Fsale_uid" runat="server" />
                            </td>
                        </tr>
                       <%-- <tr>
                            <td>买家真实姓名：</td>
                            <td>
                                <asp:Label ID="lab_" runat="server" />
                            </td>
                            <td>卖家真实姓名：</td>
                            <td>
                                <asp:Label ID="lab_" runat="server" />
                            </td>
                        </tr>--%>
                        <tr>
                            <td>原始支付单号：</td>
                            <td>
                                <asp:Label ID="lab_Forigin_listid" runat="server" />
                            </td>
                            <td>原始支付单商户订单号：</td>
                            <td>
                                <asp:Label ID="lab_Forigin_sp_billno" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>交易说明：</td>
                            <td>
                                <asp:Label ID="lab_Fmemo" runat="server" />
                            </td>
                            <td>备注：</td>
                            <td>
                                <asp:Label ID="lab_Fexplain" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>最后修改时间：</td>
                            <td>
                                <asp:Label ID="lab_Fmodify_time" runat="server" />
                            </td>
                            <td>事务管理器时间：</td>
                            <td>
                                <asp:Label ID="lab_Ftrans_time" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>渠道编号：</td>
                            <td>
                                <asp:Label ID="lab_Fchannel_id" runat="server" />
                            </td>
                            <td>资源管理器入库时间戳：</td>
                            <td>
                                <asp:Label ID="lab_Fsource_time_stamp" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>事务号：</td>
                            <td>
                                <asp:Label ID="lab_Ftrans_no" runat="server" />
                            </td>
                            <td>子事务号：</td>
                            <td>
                                <asp:Label ID="lab_Fsubtrans_no" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>请求序列号：</td>
                            <td>
                                <asp:Label ID="lab_Fsequence_no" runat="server" />
                            </td>
                            <td><%--唯一标识一次请求：--%></td>
                            <td>
                                <%--<asp:Label ID="lab_Fsequence_no_only" runat="server" />--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
