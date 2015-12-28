<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="ContractQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.ContractQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ContractQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">
		    @import url( ../STYLES/ossstyle.css );

		    UNKNOWN {
		        COLOR: #000000;
		    }

		    .style3 {
		        COLOR: #ff0000;
		    }

		    BODY {
		        BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif);
		    }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" align="center" 
				 Width="90%"  border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;合同查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">合同供应商名/他方主体：</asp:label></TD>
					<TD colspan="3"><asp:textbox id="txtVendorName" runat="server" Width="250px"></asp:textbox></TD>
				</TR
                	<TR>
					<TD align="right"><asp:label id="Label3" runat="server">合同客户名/他方主体：</asp:label></TD>
					<TD><asp:textbox id="txtCustomerName" runat="server"  Width="250px"></asp:textbox></TD>
                    <TD align="right"><asp:label id="Label4" runat="server">合同号：</asp:label></TD>
                    <TD><asp:textbox id="txtContractNo" runat="server"  Width="250px"></asp:textbox></TD>
				</TR>
				
                <TR>
                    <TD align="right"><asp:label id="Label36" runat="server">合同创建时间：</asp:label></TD>
				    <TD><asp:textbox id="TextBoxStartCreatedTime" runat="server" onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>至
                        <asp:textbox id="TextBoxEndCreatedTime" runat="server" onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
				    </TD>
                    <TD align="right"><asp:label id="Label32" runat="server">合同归档时间：</asp:label></TD>
					 <TD><asp:textbox id="TextBoxStartArchiveDay" runat="server" onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>至
                        <asp:textbox id="TextBoxEndArchiveDay" runat="server" onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
				    </TD>
				</TR>
                <TR>
					<TD align="right"><asp:label id="Label21" runat="server">有效期起：</asp:label></TD>
					 <TD><asp:textbox id="TextBoxStartBeginDate" runat="server" onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>至
                        <asp:textbox id="TextBoxEndBeginDate" runat="server" onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
				    </TD>
					<TD align="right"><asp:label id="Label24" runat="server">有效期止：</asp:label></TD>
					 <TD><asp:textbox id="TextBoxStartEndDate" runat="server" onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>至
                        <asp:textbox id="TextBoxEndEndDate" runat="server" onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
				    </TD>
				</TR>
                <TR>
                    <TD align="center" colspan="6"><FONT face="宋体"><asp:button id="btnSearch" runat="server" Width="80px" Text="查 询" onclick="btnSearch_Click"></asp:button></FONT></TD>
				</TR>
			</TABLE>
		<%--	<div style="LEFT: 5%; OVERFLOW: auto; WIDTH:820px; POSITION: absolute; TOP: 200px; HEIGHT: 300px">--%>
				<table cellSpacing="0" cellPadding="0" border="0" Width="90%" align="center" >
					<tr>
						<TD vAlign="top" align="left"><asp:datagrid id="dgList" runat="server" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
								HorizontalAlign="left" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1" BackColor="White"
								BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF"  Width="100%">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="CONTRACTNO" HeaderText="合同号">
										<HeaderStyle Width="150px" HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle Width="150px" HorizontalAlign="Center"/>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="VENDORNAME" HeaderText="供应商名/他方主体">
									    <HeaderStyle Width="100px" HorizontalAlign="Center"></HeaderStyle>
                                         <ItemStyle Width="100px"  HorizontalAlign="Center"/>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="CUSTOMERNAME" HeaderText="客户名/他方主体"> 
									    <HeaderStyle Width="100px" HorizontalAlign="Center"></HeaderStyle>
                                         <ItemStyle Width="100px"  HorizontalAlign="Center"/>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ORGFULLNAME" HeaderText="我方主体">
										<HeaderStyle Width="100px" HorizontalAlign="Center"></HeaderStyle>
                                         <ItemStyle   Width="100px" HorizontalAlign="Center"/>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="CATEGORYNAME" HeaderText="合同类别">
										<HeaderStyle Width="100px"></HeaderStyle>
                                         <ItemStyle  Width="100px" HorizontalAlign="Center"/>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="DEPTNAME" HeaderText="经办部门">
										<HeaderStyle Width="100px"></HeaderStyle>
                                         <ItemStyle  Width="100px" HorizontalAlign="Center"/>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="STAFFNAME" HeaderText="经办人">
										<HeaderStyle Width="50px"></HeaderStyle>
                                         <ItemStyle  Width="50px" HorizontalAlign="Center"/>
									</asp:BoundColumn>
									<asp:ButtonColumn Text="查看" CommandName="Select">
										<HeaderStyle Width="30px"></HeaderStyle>
                                         <ItemStyle Width="30px"  HorizontalAlign="Center"/>
									</asp:ButtonColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</tr>
                    <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" PageSize="5"
							SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				   </TR>
				</table>
		<%--	</div>--%>
			<div id="divInfo" runat="server">
				<table cellSpacing="1" cellPadding="1"  Width="90%"  align="center" border="1" frame="box">
<tr>
    <TD align="left" width="150"><asp:label id="Label12" runat="server">合同ID</asp:label></TD>
      <td align="left" width="250"><asp:label id="lbCONTRACTID" runat="server"></asp:label></td>
<TD align="left" width="151"><asp:label id="Label45" runat="server">合同状态</asp:label></TD>
      <td align="left" width="251"><asp:label id="lbContractState" runat="server"></asp:label></td>
</tr>
<tr>
    <TD align="left" width="150"><asp:label id="Label5" runat="server">我方主体全称</asp:label></TD>
      <td align="left" width="250"><asp:label id="lbORGFULLNAME" runat="server"></asp:label></td>
<TD align="left" width="151"><asp:label id="Label6" runat="server">供应商名</asp:label></TD>
      <td align="left" width="251"><asp:label id="lbVENDORNAME" runat="server"></asp:label></td>
</tr>
<tr>
<TD align="left" width="152"><asp:label id="Label7" runat="server">客户名</asp:label></TD>
      <td align="left" width="252"><asp:label id="lbCUSTOMERNAME" runat="server"></asp:label></td>
<TD align="left" width="153"><asp:label id="Label8" runat="server">经办部门名</asp:label></TD>
      <td align="left" width="253"><asp:label id="lbDEPTNAME" runat="server"></asp:label></td>
    </tr>
<tr>
<TD align="left" width="154"><asp:label id="Label9" runat="server">合同类别名</asp:label></TD>
      <td align="left" width="254"><asp:label id="lbCATEGORYNAME" runat="server"></asp:label></td>
<TD align="left" width="155"><asp:label id="Label10" runat="server">合同类别全称</asp:label></TD>
      <td align="left" width="255"><asp:label id="lbFULLPATHCATEGORYNAME" runat="server"></asp:label></td>
    </tr>
<tr>
<TD align="left" width="156"><asp:label id="Label11" runat="server">是否重要合同</asp:label></TD>
      <td align="left" width="256"><asp:label id="lbISIMPORTANT" runat="server"></asp:label></td>
<TD align="left" width="157"><asp:label id="Label13" runat="server">是否采购合同</asp:label></TD>
      <td align="left" width="257"><asp:label id="lbISPURCHASE" runat="server"></asp:label></td>
    </tr>
<tr>
<TD align="left" width="158"><asp:label id="Label14" runat="server">合同号</asp:label></TD>
      <td align="left" width="258"><asp:label id="lbCONTRACTNO" runat="server"></asp:label></td>
<TD align="left" width="159"><asp:label id="Label15" runat="server">是否授权合同</asp:label></TD>
      <td align="left" width="259"><asp:label id="lbISWARRANT" runat="server"></asp:label></td>
    </tr>
<tr>
<TD align="left" width="160"><asp:label id="Label16" runat="server">是否片区合同</asp:label></TD>
      <td align="left" width="260"><asp:label id="lbISAREA" runat="server"></asp:label></td>
<TD align="left" width="161"><asp:label id="Label17" runat="server">片区名</asp:label></TD>
      <td align="left" width="261"><asp:label id="lbAREANAME" runat="server"></asp:label></td>
    </tr>
<tr>
<TD align="left" width="162"><asp:label id="Label18" runat="server">合同经办人名</asp:label></TD>
      <td align="left" width="262"><asp:label id="lbSTAFFNAME" runat="server"></asp:label></td>
<TD align="left" width="163"><asp:label id="Label19" runat="server">合同录入人名</asp:label></TD>
      <td align="left" width="263"><asp:label id="lbWRITERNAME" runat="server"></asp:label></td>
    </tr>
<tr>
<TD align="left" width="164"><asp:label id="Label20" runat="server">是否约定有效期</asp:label></TD>
      <td align="left" width="264"><asp:label id="lbISDATEPROMISE" runat="server"></asp:label></td>
<TD align="left" width="165"><asp:label id="Label22" runat="server">是否标准合同</asp:label></TD>
      <td align="left" width="265"><asp:label id="lbISSTANDARD" runat="server"></asp:label></td>
    </tr>
<tr>
<TD align="left" width="166"><asp:label id="Label23" runat="server">合同有效期开始时间</asp:label></TD>
      <td align="left" width="266"><asp:label id="lbSTARTDATE" runat="server"></asp:label></td>
<TD align="left" width="167"><asp:label id="Label25" runat="server">合同有效期结束时间</asp:label></TD>
      <td align="left" width="267"><asp:label id="lbENDDATE" runat="server"></asp:label></td>
    </tr>
<tr>
<TD align="left" width="168"><asp:label id="Label26" runat="server">合同合作内容描述</asp:label></TD>
      <td align="left" width="268"><asp:label id="lbCONTENT" runat="server"></asp:label></td>
<TD align="left" width="169"><asp:label id="Label27" runat="server">结算条款描述</asp:label></TD>
      <td align="left" width="269"><asp:label id="lbBALANCETERM" runat="server"></asp:label></td>
    </tr>
<tr>
<TD align="left" width="170"><asp:label id="Label28" runat="server">结算方式</asp:label></TD>
      <td align="left" width="270"><asp:label id="lbSETTLEMODENAME" runat="server"></asp:label></td>
<TD align="left" width="171"><asp:label id="Label29" runat="server">结算描述</asp:label></TD>
      <td align="left" width="271"><asp:label id="lbSETTLEMODERATIO" runat="server"></asp:label></td>
    </tr>
<tr>
<TD align="left" width="172"><asp:label id="Label30" runat="server">合同金额</asp:label></TD>
      <td align="left" width="272"><asp:label id="lbTOTALPRICE" runat="server"></asp:label></td>
<TD align="left" width="173"><asp:label id="Label31" runat="server">币种名</asp:label></TD>
      <td align="left" width="273"><asp:label id="lbCURRENCYNAME" runat="server"></asp:label></td>
    </tr>
<tr>
<TD align="left" width="174"><asp:label id="Label33" runat="server">币种代码</asp:label></TD>
      <td align="left" width="274"><asp:label id="lbCURRENCYCODE" runat="server"></asp:label></td>
<TD align="left" width="175"><asp:label id="Label34" runat="server">汇率</asp:label></TD>
      <td align="left" width="275"><asp:label id="lbCURRENCYRATIO" runat="server"></asp:label></td>
    </tr>
<tr>
<TD align="left" width="176"><asp:label id="Label35" runat="server">合同总金额</asp:label></TD>
      <td align="left" width="276"><asp:label id="lbTOTALAMOUNT" runat="server"></asp:label></td>
<TD align="left" width="177"><asp:label id="Label37" runat="server">合同状态</asp:label></TD>
      <td align="left" width="277"><asp:label id="lbSTATE" runat="server"></asp:label></td>
    </tr>
<tr>
<TD align="left" width="178"><asp:label id="Label38" runat="server">归档文件夹号</asp:label></TD>
      <td align="left" width="278"><asp:label id="lbFOLDERNO" runat="server"></asp:label></td>
<TD align="left" width="179"><asp:label id="Label39" runat="server">合同归档时间</asp:label></TD>
      <td align="left" width="279"><asp:label id="lbARCHIVEDAY" runat="server"></asp:label></td>
    </tr>
<tr>
<TD align="left" width="180"><asp:label id="Label40" runat="server">合同创建时间</asp:label></TD>
      <td align="left" width="280"><asp:label id="lbCREATEDTIME" runat="server"></asp:label></td>
<TD align="left" width="181"><asp:label id="Label41" runat="server">是否框架合同</asp:label></TD>
      <td align="left" width="281"><asp:label id="lbISSTRUCTURE" runat="server"></asp:label></td>
    </tr>
<tr>
<TD align="left" width="182"><asp:label id="Label42" runat="server">经办BG名</asp:label></TD>
      <td align="left" width="282"><asp:label id="lbBUNAME" runat="server"></asp:label></td>
<TD align="left" width="183"><asp:label id="Label43" runat="server">是否关闭付款</asp:label></TD>
      <td align="left" width="283"><asp:label id="lbISCLOSEDPAYMENT" runat="server"></asp:label></td>
    </tr>
<tr>      
     </tr>
<tr>
     </tr>
<tr>
     </tr>

				</table>
			</div>
		</form>
	</body>
</HTML>
