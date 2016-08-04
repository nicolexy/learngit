<%@ Page Language="c#" CodeBehind="ValueAddedTaxDetail.aspx.cs" AutoEventWireup="true"
    Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.ValueAddedTaxDetail" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>ValueAddedTaxDetail</title>
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );
        .style2
        {
            color: #ff0000;
            font-weight: bold;
        }
        BODY
        {
            background-image: url(../IMAGES/Page/bg01.gif);
        }
    </style>
</head>
<body>
    <form id="Form1" enctype="multipart/form-data" method="post" runat="server">
    &nbsp;
    <table style="z-index: 101; position: absolute; top: 5%; left: 5%" id="Table2" border="1"
        cellspacing="1" cellpadding="1" width="90%">
        <tr style="height: 24px" bgcolor="#eeeeee">
            <td colspan="4">
                <font color="#ff0000"><span class="style1">
                    <img src="../IMAGES/Page/post.gif" width="15" height="16" alt="" /><strong>&nbsp;<asp:Label
                        ID="lbTitle" runat="server">营改增详细信息</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server">操作员代码：</asp:Label><asp:Label ID="labUid" runat="server"
                            Width="64px"></asp:Label><span class="style3"></span></strong></span></font>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td width="20%" align="center">
                            <p align="right">
                                <asp:Label ID="Label10" runat="server">商户号：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td width="30%" align="center">
                            <p align="left">
                                <asp:Label ID="labSPID" runat="server" ForeColor="Blue"></asp:Label></p>
                        </td>
                        <td width="20%" align="center">
                            <p align="right">
                                <asp:Label ID="Label2" runat="server">申请类型：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td width="30%" align="center">
                            <p align="left">
                                <asp:Label ID="labApplyType" runat="server" ForeColor="Blue"></asp:Label></p>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="center">
                            <p align="right">
                                <asp:Label ID="Label26" runat="server">申请状态：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td colspan="3" align="center">
                            <p align="left">
                                <asp:Label ID="labFlag" runat="server" ForeColor="Blue"></asp:Label></p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
                <asp:Panel ID="OldInfo" runat="server" Visible="False">
                    <table style="width: 100%" id="Table3" border="0" cellspacing="1" cellpadding="1"
                        width="100%">
                        <tr bordercolor="#999999" bgcolor="#999999">
                            <td colspan="4" align="left">
                                商户旧信息
                            </td>
                        </tr>
                        <tr>
                            <td width="20%" align="center">
                                <p align="right">
                                    <asp:Label ID="Label5" runat="server">纳税人类型：</asp:Label><font face="宋体">:</font></p>
                            </td>
                            <td width="30%" align="center">
                                <p align="left">
                                    <asp:Label ID="labOldTaxerType" runat="server" ForeColor="Blue"></asp:Label></p>
                            </td>
                            <td width="20%" align="center">
                                <p align="right">
                                    <asp:Label ID="Label13" runat="server">开票类型：</asp:Label><font face="宋体">:</font></p>
                            </td>
                            <td width="30%" align="center">
                                <p align="left">
                                    <asp:Label ID="labOldTaxInvoiceType" runat="server" ForeColor="Blue"></asp:Label></p>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 17px" align="center">
                                <p align="right">
                                    <asp:Label ID="Label17" runat="server">公司名称（发票抬头）：</asp:Label><font face="宋体">:</font></p>
                            </td>
                            <td style="height: 17px" align="center">
                                <p align="left">
                                    <asp:Label ID="labOldCompanyName" runat="server" ForeColor="Blue"></asp:Label></p>
                            </td>
                            <td style="height: 17px" align="center">
                                <p align="right">
                                    <asp:Label ID="Label20" runat="server">纳税人识别号：</asp:Label><font face="宋体">:</font></p>
                            </td>
                            <td style="height: 17px" align="center">
                                <p align="left">
                                    <asp:Label ID="labOldTaxerID" runat="server" ForeColor="Blue"></asp:Label></p>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 17px" align="center">
                                <p align="right">
                                    <asp:Label ID="Label28" runat="server">公司地址：</asp:Label><font face="宋体">:</font></p>
                            </td>
                            <td style="height: 17px" align="center">
                                <p align="left">
                                    <asp:Label ID="labOldCompanyAddress" runat="server" ForeColor="Blue"></asp:Label></p>
                            </td>
                            <td style="height: 17px" align="center">
                                <p align="right">
                                    <asp:Label ID="Label32" runat="server">公司电话：</asp:Label><font face="宋体">:</font></p>
                            </td>
                            <td style="height: 17px" align="center">
                                <p align="left">
                                    <asp:Label ID="labOldCompanyPhone" runat="server" ForeColor="Blue"></asp:Label></p>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 17px" align="center">
                                <p align="right">
                                    <asp:Label ID="Label23" runat="server">公司基本户开户行：</asp:Label><font face="宋体">:</font></p>
                            </td>
                            <td style="height: 17px" align="center">
                                <p align="left">
                                    <asp:Label ID="labOldBasebankName" runat="server" ForeColor="Blue"></asp:Label></p>
                            </td>
                            <td style="height: 17px" align="center">
                                <p align="right">
                                    <asp:Label ID="Label25" runat="server">公司基本户银行账号：</asp:Label><font face="宋体">:</font></p>
                            </td>
                            <td style="height: 17px" align="center">
                                <p align="left">
                                    <asp:Label ID="labOldBaseBankAcct" runat="server" ForeColor="Blue"></asp:Label></p>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <p align="right">
                                    <asp:Label ID="Label27" runat="server">收件人姓名：</asp:Label><font face="宋体">:</font></p>
                            </td>
                            <td align="center">
                                <p align="left">
                                    <asp:Label ID="labOldReceiverName" runat="server" ForeColor="Blue"></asp:Label></p>
                            </td>
                            <td align="center">
                                <p align="right">
                                    <asp:Label ID="Label29" runat="server">收件人地址：</asp:Label><font face="宋体">:</font></p>
                            </td>
                            <td align="center">
                                <p align="left">
                                    <asp:Label ID="labOldReceiverAddr" runat="server" ForeColor="Blue"></asp:Label></p>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <p align="right">
                                    <asp:Label ID="Label31" runat="server">邮编：</asp:Label><font face="宋体">:</font></p>
                            </td>
                            <td align="center">
                                <p align="left">
                                    <asp:Label ID="labOldReceiverPostalCode" runat="server" ForeColor="Blue"></asp:Label></p>
                            </td>
                            <td align="center">
                                <p align="right">
                                    <asp:Label ID="Label33" runat="server">联系人电话：</asp:Label><font face="宋体">:</font></p>
                            </td>
                            <td align="center">
                                <p align="left">
                                    <asp:Label ID="labOldReceiverPhone" runat="server" ForeColor="Blue"></asp:Label></p>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <p align="right">
                                    <asp:Label ID="Label35" runat="server">商户类型：</asp:Label><font face="宋体">:</font></p>
                            </td>
                            <td align="center">
                                <p align="left">
                                    <asp:Label ID="labOldUserType" runat="server" ForeColor="Blue"></asp:Label></p>
                            </td>
                            <td align="center">
                                <p align="right">
                                    <asp:Label ID="Label37" runat="server">开具发票：</asp:Label><font face="宋体">:</font></p>
                            </td>
                            <td align="center">
                                <p align="left">
                                    <asp:Label ID="lbOldIsInvoice" runat="server" ForeColor="Blue"></asp:Label></p>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
                <table style="width: 100%" id="Table4" border="0" cellspacing="1" cellpadding="1"
                    width="100%">
                    <tr bordercolor="#999999" bgcolor="#999999">
                    </tr>
                    <tr bordercolor="#999999" bgcolor="#999999">
                        <td colspan="4" align="left">
                            商户新信息
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="center">
                            <p align="right">
                                <asp:Label ID="Label14" runat="server">纳税人类型：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td width="30%" align="center">
                            <p align="left">
                                <asp:Label ID="labNewTaxerType" runat="server" ForeColor="Blue"></asp:Label></p>
                        </td>
                        <td width="20%" align="center">
                            <p align="right">
                                <asp:Label ID="Label12" runat="server">开票类型：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td width="30%" align="center">
                            <p align="left">
                                <asp:Label ID="labNewTaxInvoiceType" runat="server" ForeColor="Blue"></asp:Label></p>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 17px" align="center">
                            <p align="right">
                                <asp:Label ID="Label3" runat="server">公司名称（发票抬头）：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td style="height: 17px" align="center">
                            <p align="left">
                                <asp:Label ID="labNewCompanyName" runat="server" ForeColor="Blue"></asp:Label></p>
                        </td>
                        <td style="height: 17px" align="center">
                            <p align="right">
                                <asp:Label ID="Label6" runat="server">纳税人识别号：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td style="height: 17px" align="center">
                            <p align="left">
                                <asp:Label ID="labNewTaxerID" runat="server" ForeColor="Blue"></asp:Label></p>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 17px" align="center">
                            <p align="right">
                                <asp:Label ID="Label30" runat="server">公司地址：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td style="height: 17px" align="center">
                            <p align="left">
                                <asp:Label ID="labNewCompanyAddress" runat="server" ForeColor="Blue"></asp:Label></p>
                        </td>
                        <td style="height: 17px" align="center">
                            <p align="right">
                                <asp:Label ID="Label36" runat="server">公司电话：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td style="height: 17px" align="center">
                            <p align="left">
                                <asp:Label ID="labNewCompanyPhone" runat="server" ForeColor="Blue"></asp:Label></p>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <p align="right">
                                <asp:Label ID="Label19" runat="server">公司基本户开户行：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td align="center">
                            <p align="left">
                                <asp:Label ID="labNewBasebankName" runat="server" ForeColor="Blue"></asp:Label></p>
                        </td>
                        <td align="center">
                            <p align="right">
                                <asp:Label ID="Label21" runat="server">公司基本户银行账号：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td align="center">
                            <p align="left">
                                <asp:Label ID="labNewBaseBankAcct" runat="server" ForeColor="Blue"></asp:Label></p>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <p align="right">
                                <asp:Label ID="Label9" runat="server">收件人姓名：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td align="center">
                            <p align="left">
                                <asp:Label ID="labNewReceiverName" runat="server" ForeColor="Blue"></asp:Label></p>
                        </td>
                        <td align="center">
                            <p align="right">
                                <asp:Label ID="Label4" runat="server">收件人地址：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td align="center">
                            <p align="left">
                                <asp:Label ID="labNewReceiverAddr" runat="server" ForeColor="Blue"></asp:Label></p>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <p align="right">
                                <asp:Label ID="Label8" runat="server">邮编：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td align="center">
                            <p align="left">
                                <asp:Label ID="labNewReceiverPostalCode" runat="server" ForeColor="Blue"></asp:Label></p>
                        </td>
                        <td align="center">
                            <p align="right">
                                <asp:Label ID="Label11" runat="server">联系人电话：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td align="center">
                            <p align="left">
                                <asp:Label ID="labNewReceiverPhone" runat="server" ForeColor="Blue"></asp:Label></p>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <p align="right">
                                <asp:Label ID="Label15" runat="server">商户类型：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td align="center">
                            <p align="left">
                                <asp:Label ID="labNewUserType" runat="server" ForeColor="Blue"></asp:Label></p>
                        </td>
                        <td align="center">
                            <p align="right">
                                <asp:Label ID="Label7" runat="server">申请时间：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td align="center">
                            <p align="left">
                                <asp:Label ID="lblApplyTime" runat="server" ForeColor="Blue"></asp:Label><font face="宋体"></font></p>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <p align="right">
                                <asp:Label ID="Label16" runat="server">营改增审核备注：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtMemo" Width="700px" runat="server" TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td align="center">
                            <p align="right">
                                <asp:Label ID="Label34" runat="server">开具发票：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td align="center">
                            <p align="left">
                                <asp:Label ID="lbNewIsInvoice" runat="server" ForeColor="Blue"></asp:Label><font face="宋体"></font></p>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <p align="right">
                                <asp:Label ID="Label18" runat="server">纳税人资格证书或税务登记证副本：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td colspan="3" align="left">
                            <asp:Image ID="imgTaxCert" runat="server" Width="370px" Height="300px"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <p align="right">
                                <asp:Label ID="Label22" runat="server">营业执照路径：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td colspan="3" align="left">
                            <asp:Image ID="imgBizLicenseCert" runat="server" Width="370px" Height="300px"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <p align="right">
                                <asp:Label ID="Label24" runat="server">授权书路径：</asp:Label><font face="宋体">:</font></p>
                        </td>
                        <td colspan="3" align="left">
                            <asp:Image ID="imgAuthorizationCert" Width="370px" Height="300px" runat="server">
                            </asp:Image>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnApprove" Width="64px" runat="server" Height="22" Text="审核通过" Visible="False" OnClick="btnApprove_Click">
                </asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnCancel" Width="64px" runat="server" Height="22" Text="审核拒绝" Visible="False" OnClick="btnCancel_Click">
                </asp:Button>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
