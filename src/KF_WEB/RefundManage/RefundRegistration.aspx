<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RefundRegistration.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RefundManage.RefundRegistration" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <title>RefundRegistration</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
	    .mycss table tr td span 
        {
            BORDER-RIGHT: #000099 1px solid; PADDING-RIGHT: 5px; BORDER-TOP: #000099 1px solid; PADDING-LEFT: 5px; FONT-WEIGHT: bold; PADDING-BOTTOM: 2px; MARGIN: 2px; BORDER-LEFT: #000099 1px solid; COLOR: #fff; PADDING-TOP: 2px; BORDER-BOTTOM: #000099 1px solid; BACKGROUND-COLOR: #000099
        }
        .mycss table tr td 
        {
         PADDING-RIGHT: 3px; PADDING-LEFT: 3px; PADDING-BOTTOM: 3px; MARGIN: 3px; PADDING-TOP: 3px; TEXT-ALIGN: center
        }
        div.mycss  table tr td a
        {
         BORDER-RIGHT: #aaaadd 1px solid; PADDING-RIGHT: 5px; BORDER-TOP: #aaaadd 1px solid; PADDING-LEFT: 5px; PADDING-BOTTOM: 2px; MARGIN: 2px; BORDER-LEFT: #aaaadd 1px solid; COLOR: #000099; PADDING-TOP: 2px; BORDER-BOTTOM: #aaaadd 1px solid; TEXT-DECORATION: none
        }
        div.mycss table tr td A:hover {
         BORDER-RIGHT: #FF0000 1px solid; BORDER-TOP: #FF0000 1px solid; BORDER-LEFT: #FF0000 1px solid; COLOR: #000; BORDER-BOTTOM: #FF0000 1px solid
        }
        div.mycss table tr td A:active {
         BORDER-RIGHT: #0000FF 1px solid; BORDER-TOP: #0000FF 1px solid; BORDER-LEFT: #0000FF 1px solid; COLOR: #000; BORDER-BOTTOM: #0000FF 1px solid
        }
		    .auto-style1
            {
                width: 344px;
            }
		</style>		
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body >
    <form id="form1" method="post" runat="server">
    
			<table id="Table4" style="Z-INDEX: 101; LEFT: 16px; WIDTH: 1040px" cellSpacing="1" cellPadding="1" TOP: 10%; HEIGHT: 80%"
				width="900" align="center" border="1">
				<tr>
					<td colspan="2"><IMG height="16" src="../IMAGES/Page/post.gif" width="15">&nbsp;<asp:label id="lbTitle" runat="server" ForeColor="Red">异常退款单查询</asp:label>
                    </td>
					<td  colspan="2">	
						</FONT>&nbsp;&nbsp; 操作员代码: </FONT><SPAN class="style3"><asp:label id="operaterName" runat="server" Width="136px" ForeColor="Red"></asp:label></SPAN>
					</td>
				</tr>
				<tr>
                    <td nowrap>
                    <asp:label id="Label2" runat="server">财付通订单号：</asp:label>
					<asp:textbox id="txtUinID" runat="server" Width = "200px"></asp:textbox>
                    </td nowrap>
                    <td class="auto-style1">
                    <asp:label id="Label6" runat="server">银行订单：</asp:label>
					<asp:textbox id="txtBankID" runat="server" Width = "200px"></asp:textbox>
                     </td>
                    <td nowrap>
                    <asp:label id="Label7" runat="server">退款单号：</asp:label>
					<asp:textbox id="txtOldID" runat="server" Width = "200px"></asp:textbox>
                    </td>
                    <td nowrap>
                    <asp:label id="Label5" runat="server">提交人：</asp:label>
					<asp:textbox id="txtOperator" runat="server" Width = "200px"></asp:textbox>
                 </td>
    
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:label id="Label10" runat="server">商户号：(注各商户号间以;作为分隔符)</asp:label>
                        <asp:TextBox ID="txtSPID" runat="server" Width = "1400px" Height="50px" TextMode="MultiLine">  </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:TextBox ID="txtContext" runat="server" Width = "1400px" Height="30px" Visible="false" TextMode="MultiLine">  </asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnText" runat="server" Width ="80px" Visible="false" Text="设置" onclick="OnBtnYonghua_Click" />
                    </td>
                </tr>
                <tr>
                <td nowrap>
                    
                    开始日期：                   
                    <asp:TextBox ID="textBoxBeginDate" runat="server" Width="130px" onClick="WdatePicker()"  CssClass="Wdate"></asp:TextBox>
                    
                   
                    结束日期：
                        <asp:TextBox ID="textBoxEndDate" runat="server" Width="130px" onClick="WdatePicker()"  CssClass="Wdate"></asp:TextBox>                
                </td>
                <td nowrap>
                    金额范围：
					<asp:textbox id="txtMin" runat="server" Width = "50px"></asp:textbox>
                    至
                    <asp:textbox id="txtMax" runat="server" Width = "80px"></asp:textbox>元
                 </td>
                  <td nowrap>
					    <asp:Label ID="Label3" runat="server" Text="支付场景:"></asp:Label>&nbsp;&nbsp;
                        <asp:DropDownList ID="ddDoneID" runat="server" Height="20px" Width="97px">
                        <asp:ListItem Value="0" Selected="True">全部</asp:ListItem>
						<asp:ListItem Value="1">微信</asp:ListItem>
						<asp:ListItem Value="2">非微信</asp:ListItem>
                        </asp:DropDownList>
                 </td>
                <td nowrap>
					    <asp:Label ID="Label9" runat="server" Text="支付渠道:"></asp:Label>&nbsp;&nbsp;
                        <asp:DropDownList ID="ddPayID" runat="server" Height="20px" Width="97px">
                        <asp:ListItem Value="0" Selected="True">全部</asp:ListItem>
						<asp:ListItem Value="1">快捷</asp:ListItem>
						<asp:ListItem Value="2">非快捷</asp:ListItem>
                        </asp:DropDownList>
                 </td>

         

				</tr>
               <tr>
                    <td>
                        &nbsp;&nbsp;
                        <asp:Label id="Label4" runat="server">审核状态:</asp:Label>&nbsp;&nbsp;
                        <asp:DropDownList ID="checkStateListID" runat="server" Height="20px"  Width="200px" >
                        <asp:ListItem Value="-1" Selected="True">全部</asp:ListItem>
                        <asp:ListItem Value="-2">挂起</asp:ListItem>     
                        <asp:ListItem Value="0">等待客服处理</asp:ListItem>
						<asp:ListItem Value="1">已通知用户</asp:ListItem>
						<asp:ListItem Value="2">收集用户卡号，等待客服审批</asp:ListItem>						
						<asp:ListItem Value="3">客服通过等待BG审核</asp:ListItem>
						<asp:ListItem Value="4">BG通过等待风控审核</asp:ListItem>						
						<asp:ListItem Value="5">风控通过，等待财务处理</asp:ListItem>
                        <asp:ListItem Value="6">财务处理完成</asp:ListItem>
                        <asp:ListItem Value="7">无法处理直接拒绝</asp:ListItem>                
                        </asp:DropDownList>
                    </td>
                   <td>    
                      
                        <asp:Label ID="Label8" runat="server" Text="退款状态:"></asp:Label>&nbsp;&nbsp;
                        <asp:DropDownList ID="tradeListID" runat="server" Height="20px" Width="97px">
                        <asp:ListItem Value="0" Selected="True">全部</asp:ListItem>
						<asp:ListItem Value="1">退款中</asp:ListItem>
						<asp:ListItem Value="2">退款成功</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    
                    <td align="center">                      
                        &nbsp;<asp:Button ID="btnCheckID" runat="server" Width ="80px"  Text="查询" onclick="OnBtnCheck_Click"  ViewStateMode="Disabled" />
                    </td>
                   
                   <td align="center">
                       <asp:Button ID="btnSelID" runat="server" Width ="80px" Text="全选" onclick="OnBtnChangeItem_Click" />
                    </td>
              </tr>
                <tr>
                       <td nowrap><asp:Button ID="btnInputID" runat="server" Text="客服补填资料" onclick="OnBtnInputInfor_Click" /></td>
                        <td nowrap><asp:Button ID="btnReceiveID" runat="server" Text="银行收件人名单管理"  onclick="OnBtnReceiveManager_Click" /></td>
                        <td nowrap><asp:Button ID="btnExecl" runat="server" Text="导出execl" onclick="OnBtnExecl_Click" />
                            <asp:Button ID="Button1" runat="server" Text="挂起" OnClick="Button1_Click" />
                       </td>
                        <td nowrap><asp:Button ID="btnCaiID" runat="server" Text="转财务处理" onclick="OnBtnCaiWu_Click" /></td>
                </tr>
               
           </table >
         <div class="mycss">
         <table ID = "tableView" style="Z-INDEX: 101; LEFT: 16px; WIDTH: 1500px" cellSpacing="1" cellPadding="1"  align="center" border="0">
             <tr>
                <td vAlign="top">
                <br><asp:GridView ID="gridInfor" runat="server" AutoGenerateColumns = "False" 
                        headerstyle-horizontalalign="center" horizontalalign="Center" PageSize="10"
                        AllowPaging ="True" AllowSorting = "True"
                        GridLines="Horizontal" CellPadding="3" BorderStyle="None" DataKeyNames = "FpayListid,FCardType,FbankListid,FbankName,FbankType,FcreateTime,FtrueName,FmodifyTime,FReturnAmt,FAmt,FbankAccNo,FbankTypeOld,FoldId,FrefundType,Fstate"
                        BorderWidth="1px">
                        <PagerSettings Visible="False" />
                        <PagerStyle BorderColor="#66FF66" Font-Names="宋体"  Font-Size="24px" />  
                        <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
                    <Columns>            
                         
                        <asp:BoundField HeaderText="财付通订单号" DataField="FpayListid"   ReadOnly="True" ShowHeader="False" ><HeaderStyle Width="150px"></HeaderStyle><ItemStyle HorizontalAlign="Center"/></asp:BoundField >
                        <%--<asp:BoundField HeaderText="交易状态"     DataField="FReturnstate" ReadOnly="True" ShowHeader="False" ><HeaderStyle Width="100px"></HeaderStyle><ItemStyle HorizontalAlign="Center"/></asp:BoundField >--%>
                        <asp:BoundField HeaderText="交易金额"     DataField="FAmtEx"       ReadOnly="True" ShowHeader="False" ><HeaderStyle Width="100px"></HeaderStyle><ItemStyle HorizontalAlign="Center"/></asp:BoundField >
                        <asp:BoundField HeaderText="退款金额"     DataField="FReturnAmtEx" ReadOnly="True" ShowHeader="False" ><HeaderStyle Width="100px"></HeaderStyle><ItemStyle HorizontalAlign="Center"/></asp:BoundField >
                        <asp:BoundField HeaderText="创建时间"     DataField="FcreateTime"   ReadOnly="True" ShowHeader="False" ><HeaderStyle Width="100px"></HeaderStyle><ItemStyle HorizontalAlign="Center"/></asp:BoundField >
                        <asp:BoundField HeaderText="邮箱地址"     DataField="FUserEmail"   ReadOnly="True" ShowHeader="False" ><HeaderStyle Width="100px"></HeaderStyle><ItemStyle HorizontalAlign="Center"/></asp:BoundField >
                        <asp:BoundField HeaderText="银行订单号"   DataField="FbankListid"  ReadOnly="True" ShowHeader="False"><HeaderStyle  Width="100px"></HeaderStyle><ItemStyle HorizontalAlign="Center"/></asp:BoundField >
                        <asp:BoundField HeaderText="审批状态"     DataField="FstateEx"     ReadOnly="True" ShowHeader="False" ><HeaderStyle Width="100px"></HeaderStyle><ItemStyle HorizontalAlign="Center"/></asp:BoundField >
                        <asp:BoundField HeaderText="数据来源"     DataField="FrefundType_str"     ReadOnly="True" ShowHeader="False" ><HeaderStyle Width="100px"></HeaderStyle><ItemStyle HorizontalAlign="Center"/></asp:BoundField >
                        <asp:BoundField HeaderText="退款状态"     DataField="TotalDB_Fstate_str"     ReadOnly="True" ShowHeader="False" ><HeaderStyle Width="100px"></HeaderStyle><ItemStyle HorizontalAlign="Center"/></asp:BoundField >
                        <asp:BoundField HeaderText="银行类型"     DataField="FbankTypeName" ReadOnly="True" ShowHeader="False" ><HeaderStyle Width="100px"></HeaderStyle><ItemStyle HorizontalAlign="Center"/></asp:BoundField >                                           
                             <asp:TemplateField HeaderText = "操作" >
                              <ItemTemplate>                                                           
                                        <asp:CheckBox ID="checkbox" runat="server" Text ="" /> &nbsp;                                          
                                        <asp:Button ID = "btnNotice" runat = "server"  Text = "通知" OnClick = "OnBtnClickNotice_Click"  Width = "50px" 
                                        CommandArgument='<%# Eval("FuserFlag")+","+Eval("FpayListid")+","+Eval("FoldId") %>'  >     
                                        </asp:Button>&nbsp;&nbsp; 
                                        <asp:Button ID = "btnBank" runat = "server"  Text = "通知银行"  OnClick = "OnBtnClickBank_Click" Width = "60px" 
                                        CommandArgument='<%# Eval("FbankType")+","+Eval("FbankAccNo")+","+Eval("FtrueName")+","+Eval("FmodifyTime")+","+Eval("FbankTypeOld") +","+Eval("FcreateTime")+","+Eval("FbankListid")+","+Eval("FReturnAmt")+","+Eval("FAmt") %>'  > 
                                        </asp:Button>&nbsp;&nbsp;
                                        <asp:LinkButton ID="linkDetail" runat="server" CommandName="detail"  CommandArgument ='<%# Eval("FoldId") %>' OnClick = "OnLinkBtnDetail_Click" Width = "40px">详情</asp:LinkButton>                                                                          
                             </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField HeaderText="银行类型(存放值的不参加显示)"  DataField="Fbuybanktype" Visible="false"></asp:BoundField>--%>                                        
                    </Columns>
                </asp:GridView>
                    <webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left" PageSize="10"
								PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage"
								SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager>
                    <span style="float:right; right:20px;">
                        记录总数:
                        <asp:Label runat="server" id="lb_RefundCountAll" Text="0" />
                    </span>
                </td>
             </tr>
          </table>
     </div>           
    </form>
</body>
</html>
