using System;
using System.Collections;

namespace TENCENT.OSS.CFT.KF.DataAccess
{
	/// <summary>
	/// 业务命令
	/// </summary>
	public class YWCommandCode
	{
		public static int 修改用户信息 = 310020;
		public static int 查询用户信息 = 310000; //S_CMD_QUERY_USER 

		public static int 查询交易单信息 = 400000; //S_CMD_QUERY_TRAN_LIST
		public static int 修改交易单信息 = 400020; //S_CMD_ALTER_TRAN_LIST
		public static int 退款3中介到买家=10042 ; //T_CMD_REFUND3_MEDI_TO_BUYER

		public static int 查询商户信息 = 100000;//S_CMD_QUERY_MEDI
		public static int 创建商户 = 100010; //S_CMD_CREATE_MEDI
		public static int 修改商户信息 = 100020; //S_CMD_ALTER_MEDI


		public static int 提现请求 = 10002; //T_CMD_FETCH	
		public static int 提现取消 = 10005; //T_CMD_FETCH_CANCEL
		public static int 提现直接成功 = 10006; //T_CMD_FETCH_DIRECT_SUCC
		public static int 修改提现备注 = 140020; //S_CMD_SIM_ALTER_TCPAY_LIST

		public static int 收货确认 = 10032; //T_CMD_PAY32_MEDI_TO_SALER_NOSALER

		public static int 核心退款 = 10040; //T_CMD_REFUND1_MEDI_TO_REFUNDER

		public static int 冻结用户帐户 = 10006;
		public static int 解冻用户帐户 = 10007;

		public static int 查询核心充值单信息 = 130000;//S_CMD_QUERY_TCBANKROLL_LIST

		public static int 查询银行用户信息 = 110000;//S_CMD_QUERY_BANK
		public static int 增加银行用户信息 = 110010;//S_CMD_CREATE_BANK 
		public static int 修改银行用户信息 = 110020;//S_CMD_ALTER_BANK  

		public static int 子帐户转帐 = 10010;//T_CMD_PAY1_BUYER_TO_SALER  
	}

	/// <summary>
	/// 业务资源类型
	/// </summary>
	public class YWSourceType
	{
		public static int 用户资源 = 1; //SOURCE_DB_T_USER
		public static int 交易单资源 = 2;  //SOURCE_DB_T_TRAN_LIST
		public static int 商户资源 = 6;  //  SOURCE_DB_T_MIDDLE_USER
		public static int 提现单资源 = 4; //SOURCE_DB_T_TCPAY_LIST

		public static int 充值单资源 = 3;//SOURCE_DB_T_TCBANKROLLLIST

		public static int 银行资源 = 7;//SOURCE_DB_T_BANK 

	}

	/// <summary>
	/// 订单锁操作类型
	/// </summary>
	public class OrderSubType
	{
		public static int 请求锁 = 1; //SOURCE_DB_T_USER
		public static int 提交锁 = 2;  //SOURCE_DB_T_TRAN_LIST
		public static int 回滚锁 = 3;  //  SOURCE_DB_T_MIDDLE_USER
	}

	/// <summary>
	/// 错误说明
	/// </summary>
	public class YWCommandResult
	{

		private static Hashtable ywresult = null;

		#region 把错误信息导入。
		
		static void YWCommandCode()
		{
			ywresult = new Hashtable();
			ywresult.Add("60120000","未知应用错误");
			ywresult.Add("60120001","操作类型失败");
			ywresult.Add("60120002","db结构定义错误");
			ywresult.Add("60120101","没有找到相应的数据");
			ywresult.Add("60120102","子事务数量错误");
			ywresult.Add("60120103","重复锁定单");
			ywresult.Add("60120104","重复执行命令");
			ywresult.Add("60120105","重复插入信息");
			ywresult.Add("60121001","解密错误");
			ywresult.Add("60121002","参数缺失");
			ywresult.Add("60121003","参数类型错误");
			ywresult.Add("60121004","主键不存在");
			ywresult.Add("60121005","分库分表字段参数错误");
			ywresult.Add("60121006","参数不符合");
			ywresult.Add("60121007","参数过长");
			ywresult.Add("60121008","数据库超过限制");
			ywresult.Add("60121009","uid类型错误");
			ywresult.Add("60121010","两个银行");
			ywresult.Add("60121101","未知的步骤类型");
			ywresult.Add("60121102","命令方式非法改命令字可能不支持回滚等操作");
			ywresult.Add("60121103","命令字和处理类不对应");
			ywresult.Add("60121104","事务前置状态不符合");
			ywresult.Add("60121201","查询状态命令返回未执行commit");
			ywresult.Add("60121202","查询状态命令返回未执行rollback");
			ywresult.Add("60121203","查询状态命令返回未执行prepare");
			ywresult.Add("60122001","用户冻结");
			ywresult.Add("60122002","用户余额不足");
			ywresult.Add("60122003","用户冻结余额不足");
			ywresult.Add("60123000","订单不可预计错误,比如状态不正常等");
			ywresult.Add("60123001","用户角色数量超过最大限制");
			ywresult.Add("60123002","订单金额错误");
			ywresult.Add("60123003","用户参与方序号错误");
			ywresult.Add("60123004","用户参与方uid与以前不一致");
			ywresult.Add("60123005","没有买卖家");
			ywresult.Add("60123006","用户数量错误");
			ywresult.Add("60123007","定单前置状态修改");
			ywresult.Add("60123008","定单状态错误,被锁定,或者作废");
			ywresult.Add("60123009","新增的是买家");
			ywresult.Add("60124000","商户未知错误");
			ywresult.Add("60124001","商户冻结");
			ywresult.Add("60124002","商户余额不足");
			ywresult.Add("60124003","商户冻结余额不足");
			ywresult.Add("60124004","商户流水的对方列表数量不一致");
			ywresult.Add("60124005","商户交易对方列表中的金额信息和外面的总金额信息不一样");
			ywresult.Add("60110000","系统异常(内存不足等)");
			ywresult.Add("60110101","分配数据库连接失败");
			ywresult.Add("60110102","连接数据库失败");
			ywresult.Add("60110103","和数据库断开连接");
			ywresult.Add("60110104","获取到空数据库结果集");
			ywresult.Add("60110105","获取到空数据库结果集");
			ywresult.Add("60110106","设置自动提交模式失败");
			ywresult.Add("60110107","开始事务失败");
			ywresult.Add("60110108","提交事务失败");
			ywresult.Add("60110109","回滚事务失败");
			ywresult.Add("60110110","没有取到任何结果");
			ywresult.Add("60110111","取到结果多余一行");
			ywresult.Add("60110112","取结果行错误");
			ywresult.Add("60110113","意外错误");
			ywresult.Add("60110114","结果行数过长");
			ywresult.Add("60110115","拼接SQL语句错误,没有值");
			ywresult.Add("60110116","重复开始事务");
			ywresult.Add("60110117","不在事务中,调用事务处理语");
			ywresult.Add("60111001","无可用节点");
			ywresult.Add("60111002","共享文件操作错误    ");
			ywresult.Add("60111003","共享文件节点状态不对");
			ywresult.Add("60111004","文件中节点状态不对");
			ywresult.Add("60111101","对内存索引操作失败");
			ywresult.Add("60111201","订单加解锁出错");
			ywresult.Add("60111302","没有打包返回值");
			ywresult.Add("60011100","不是事务，不需要发送到差错服务");
			ywresult.Add("60011101","无效的业务命令号");
			ywresult.Add("60011102","未找到对应的配置项");
			ywresult.Add("60011103","参数无效");
			ywresult.Add("60011104","命令字调用顺序错");
			ywresult.Add("60011105","取重入信息错误");
			ywresult.Add("60011106","用户不存在");
			ywresult.Add("60011107","MEDI不相同");
			ywresult.Add("60011108","相同用户");
			ywresult.Add("60011109","冻结");
			ywresult.Add("60011110","余额不足");
			ywresult.Add("60011111","单已经存在");
			ywresult.Add("60011112","单不存在");
			ywresult.Add("60011113","用户类型错误");
			ywresult.Add("60011114","单状态错");
			ywresult.Add("60025000","db处理错误,此种返回会导致回滚发生");
			ywresult.Add("60025001","db处理网络错误,此种返回会导致回滚发生");
			ywresult.Add("60025002","db查询错误");
			ywresult.Add("60025003","db查询网络错误");
			ywresult.Add("60025004","文件处理错误");
			ywresult.Add("60026000","系统维护中");
			ywresult.Add("60027000","已经处理过,给前面返回成功");
		}
		#endregion

		public static string GetResultDetailInfo(string resultcode)
		{
			if(ywresult == null)
				return "";

			if(ywresult.ContainsKey(resultcode))
			{
				return ywresult[resultcode].ToString();
			}
			else
				return "";

		}
	}
}
