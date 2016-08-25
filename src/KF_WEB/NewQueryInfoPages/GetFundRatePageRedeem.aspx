<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetFundRatePageRedeem.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.GetFundRatePageRedeem" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
    <title>跨境汇款</title>
    <style type="text/css">
        @import url( ../STYLES/common.css );
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );
        BODY {
            background-image: url(../IMAGES/Page/bg01.gif);
        }
    </style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>

</head>
<body>
    <form id="formMain" runat="server">
        <div class="container">
             
            <asp:Panel runat="server" ID="plan_NonMonetaryFundRedeem"  Visible="false" >
                <table cellspacing="1" cellpadding="1">
                   <tr>
                       <td colspan="4"  class="style_title" style="text-align:center">非货币基金强赎 </td>
                   </tr>
                    <tr>
                        <td class="tb_query_title" style="width:25%">财付通账号：</td>
                        <td style="width:25%">
                            <asp:Label runat="server" ID="lbl_uin"></asp:Label>
                        </td>
                        <td class="tb_query_title" style="width:25%">基金编码：</td>
                        <td style="width:25%">
                            <asp:Label runat="server" ID="lbl_fund_code"> </asp:Label></td>
                    </tr>
                        <tr>
                        <td class="tb_query_title">商户号：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_spid"></asp:Label>
                        </td>
                        <td class="tb_query_title">币种类型：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_cur_type"> </asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tb_query_title">提现金额：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_total_fee"></asp:Label>
                        </td>
                        <td class="tb_query_title">账户类型：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_acct_type"> </asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tb_query_title">渠道号：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_channel_id"></asp:Label>
                        </td>
                        <td class="tb_query_title">绑卡序列号：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_bind_serialno"> </asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tb_query_title">卡尾号：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_card_tail"></asp:Label>
                        </td>
                        <td class="tb_query_title">提现银行类型：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_bank_type"> </asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tb_query_title"> 本地IP：</td>
                        <td>
                          
                            <asp:Label ID="lbl_client_ip" runat="server"></asp:Label>
                          
                        </td>
                        <td class="tb_query_title">赎回用途：</td>
                        <td>
                            <asp:DropDownList ID="ddl_redem_type" runat="server" Width="180px">
                                <asp:ListItem Text="提现到银行卡" Value="1" /> 
                                 <asp:ListItem Text="提现到理财通余额" Value="2" /> 
                                 <asp:ListItem Text="提现到默认基金" Value="3" /> 
                                 <asp:ListItem Text="提现到微信余额" Value="4" /> 
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tb_query_title"><asp:Label runat="server" ID="title_close_id" Text="期次号(投连险)："></asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lbl_close_id"></asp:Label>
                        </td>
                        <td class="tb_query_title"><asp:Label runat="server" ID="title_opt_type" Text="赎回方式(投连险)："></asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lbl_opt_type"></asp:Label>
                        </td>
                    </tr>
                     <tr>
                       <td  colspan="2" style="color:red;font-size:20px;text-align:right;">基金公司凭证: </td>
                         <td colspan="2">
                             <INPUT id="fileNON" style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="fileNON" runat="server">
                              <asp:button id="upNON" runat="server"  Text="上传" OnClick="upNON_Click" />
                         </td>
                   </tr>
                     <tr>
                        <td colspan="4" style="text-align:center">
                            <asp:Image ID="imgNoN" runat="server" />

                        </td>
                   </tr>
                    <tr>
                        <td colspan="4" style="text-align:center">
                            <asp:button id="btnNON" runat="server" Width="200px" Text="确认无误后，提交申请" OnClick="btnNON_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

                <asp:Panel runat="server" ID="plan_MonetaryFundRedeem"   Visible="false" >
                <table cellspacing="1" cellpadding="1">
                   <tr>
                       <td colspan="4"  class="style_title" style="text-align:center">货币基金强赎 </td>
                   </tr>
                    <tr>
                        <td class="tb_query_title" style="width:25%">财付通账号：</td>
                        <td style="width:25%">
                            <asp:Label runat="server" ID="lblHB_uin"></asp:Label>
                        </td>
                        <td class="tb_query_title" style="width:25%">商户号：</td>
                        <td style="width:25%">
                            <asp:Label runat="server" ID="lblHB_spid"> </asp:Label></td>
                    </tr>
                       <tr>
                        <td class="tb_query_title" style="width:25%">基金编码：</td>
                        <td style="width:25%">
                            <asp:Label runat="server" ID="lblHB_fund_code"></asp:Label>
                        </td>
                        <td class="tb_query_title" style="width:25%">赎回金额：</td>
                        <td style="width:25%">
                            <asp:Label runat="server" ID="lblHB_total_fee"> </asp:Label></td>
                    </tr>
                      <tr>
                        <td class="tb_query_title" style="width:25%">渠道号：</td>
                        <td style="width:25%">
                            <asp:Label runat="server" ID="lblHB_channel_id"></asp:Label>
                        </td>
                        <td class="tb_query_title" style="width:25%">绑定序列号：</td>
                        <td style="width:25%">
                            <asp:Label runat="server" ID="lblHB_bind_serialno"> </asp:Label></td>
                    </tr>
                       <tr>
                        <td class="tb_query_title" style="width:25%">银行类型：</td>
                        <td style="width:25%">
                             <asp:Label ID="lblHB_bank_type" runat="server"></asp:Label>
                        </td>
                        <td class="tb_query_title" style="width:25%">卡尾号：</td>
                        <td style="width:25%">
                            <asp:Label runat="server" ID="lblHB_card_tail"> </asp:Label></td>
                    </tr>
                      <tr>
                        <td class="tb_query_title" style="width:25%">币种类型：</td>
                        <td style="width:25%">
                            <asp:Label ID="lblHB_cur_type" runat="server"></asp:Label>
                        </td>
                        <td class="tb_query_title" style="width:25%">&nbsp;</td>
                        <td style="width:25%">
                            &nbsp;</td>
                    </tr>
                    

                       <tr>
                        <td class="tb_query_title" style="width:25%">银行卡账户名：</td>
                        <td style="width:25%">
                           
                            <asp:TextBox ID="txtHB_card_name" runat="server"  Width="180px"></asp:TextBox>
                           
                        </td>
                        <td class="tb_query_title" style="width:25%">开户行名称：</td>
                        <td style="width:25%">
                           
                            <asp:TextBox ID="txtHB_bank_name" runat="server"  Width="180px"></asp:TextBox>
                           
                        </td>
                    </tr>

                      <tr>
                        <td class="tb_query_title" style="width:25%">开户行城市：</td>
                        <td style="width:25%">
                           
                            <asp:TextBox ID="txtHB_bank_city" runat="server"  Width="180px"></asp:TextBox>
                           
                        </td>
                        <td class="tb_query_title" style="width:25%">开户行省份：</td>
                        <td style="width:25%">
                           
                            <asp:TextBox ID="txtHB_bank_area" runat="server"  Width="180px"></asp:TextBox>
                           
                        </td>
                    </tr>
                    
                     <tr>
                       <td  colspan="2" style="color:red;font-size:20px;text-align:right;">基金公司凭证: </td>
                         <td colspan="2">
                             <INPUT id="fileHB" style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="fileHB" runat="server">
                              <asp:button id="upHB" runat="server"  Text="上传" OnClick="upHB_Click" />
                         </td>
                   </tr>
                     <tr>
                        <td colspan="4" style="text-align:center">
                            <asp:Image ID="imgHB" runat="server" />
                        </td>
                   </tr>
                    <tr>
                        <td colspan="4" style="text-align:center">
                            <asp:button id="btnHB" runat="server" Width="200px" Text="确认无误后，提交申请" OnClick="btnHB_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <br />
    </form>
</body>
</html>