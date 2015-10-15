// **********************************************************************
//
// Copyright (c) 2003-2005 ZeroC, Inc. All rights reserved.
//
// This copy of Ice is licensed to you under the terms described in the
// ICE_LICENSE file included in this distribution.
//
// **********************************************************************

// Ice version 3.1.1
// Generated from file `BackProcessInvoke.ice'

using _System = System;
using _Microsoft = Microsoft;

namespace BackProcessInvoke
{
	public interface IProcessInvoke : Ice.Object, IProcessInvokeOperations_, IProcessInvokeOperationsNC_
	{
	}
}

namespace BackProcessInvoke
{
	public interface IProcessInvokePrx : Ice.ObjectPrx
	{
		bool Invoke(string strCommand, out int iResult, out string strResInfo);
		bool Invoke(string strCommand, out int iResult, out string strResInfo, Ice.Context context__);

		bool MiddleInvoke(string strServiceName, string strReqMsg, int iServerFlag, int iTimeout, string strUserName, string strUserPwd, out string strAnsMsg, out int iResult, out string strResInfo);
		bool MiddleInvoke(string strServiceName, string strReqMsg, int iServerFlag, int iTimeout, string strUserName, string strUserPwd, out string strAnsMsg, out int iResult, out string strResInfo, Ice.Context context__);

		int InvokeV30(string transListNo, string sequenceNo, int cmdNo, int middleNo, string request, out string respond);
		int InvokeV30(string transListNo, string sequenceNo, int cmdNo, int middleNo, string request, out string respond, Ice.Context context__);

		int InvokeQuery(int SourceType, int SourceCmd, string szKey, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp);
		int InvokeQuery(int SourceType, int SourceCmd, string szKey, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp, Ice.Context context__);


        #region 增加参数setID
        int InvokeQuery_SetID(int SourceType, int SourceCmd, string szKey, string setID, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp);
        int InvokeQuery_SetID(int SourceType, int SourceCmd, string szKey, string setID, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp, Ice.Context context__);
        #endregion

	}
}

namespace BackProcessInvoke
{
	public interface IProcessInvokeOperations_
	{
		bool Invoke(string strCommand, out int iResult, out string strResInfo, Ice.Current current__);

		bool MiddleInvoke(string strServiceName, string strReqMsg, int iServerFlag, int iTimeout, string strUserName, string strUserPwd, out string strAnsMsg, out int iResult, out string strResInfo, Ice.Current current__);

		int InvokeV30(string transListNo, string sequenceNo, int cmdNo, int middleNo, string request, out string respond, Ice.Current current__);

		int InvokeQuery(int SourceType, int SourceCmd, string szKey, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp, Ice.Current current__);

        #region 增加参数setID
        int InvokeQuery_SetID(int SourceType, int SourceCmd, string szKey, string setID, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp, Ice.Current current__);
        #endregion
	}

	public interface IProcessInvokeOperationsNC_
	{
		bool Invoke(string strCommand, out int iResult, out string strResInfo);

		bool MiddleInvoke(string strServiceName, string strReqMsg, int iServerFlag, int iTimeout, string strUserName, string strUserPwd, out string strAnsMsg, out int iResult, out string strResInfo);

		int InvokeV30(string transListNo, string sequenceNo, int cmdNo, int middleNo, string request, out string respond);

		int InvokeQuery(int SourceType, int SourceCmd, string szKey, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp);

        #region 增加参数setID
        int InvokeQuery_SetID(int SourceType, int SourceCmd, string szKey, string setID, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp);
        #endregion
	}
}

namespace BackProcessInvoke
{
	public sealed class IProcessInvokePrxHelper : Ice.ObjectPrxHelperBase, IProcessInvokePrx
	{
		#region Synchronous operations

		public bool Invoke(string strCommand, out int iResult, out string strResInfo)
		{
			return Invoke(strCommand, out iResult, out strResInfo, defaultContext__());
		}

		public bool Invoke(string strCommand, out int iResult, out string strResInfo, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("Invoke");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IProcessInvokeDel_ del__ = (IProcessInvokeDel_)delBase__;
					return del__.Invoke(strCommand, out iResult, out strResInfo, context__);
				}
				catch(IceInternal.LocalExceptionWrapper ex__)
				{
					handleExceptionWrapper__(ex__);
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public int InvokeQuery(int SourceType, int SourceCmd, string szKey, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp)
		{
			return InvokeQuery(SourceType, SourceCmd, szKey, middleNo, ParaLen, szPara, out RespLen, out szResp, defaultContext__());
		}

		public int InvokeQuery(int SourceType, int SourceCmd, string szKey, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("InvokeQuery");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IProcessInvokeDel_ del__ = (IProcessInvokeDel_)delBase__;
					return del__.InvokeQuery(SourceType, SourceCmd, szKey, middleNo, ParaLen, szPara, out RespLen, out szResp, context__);
				}
				catch(IceInternal.LocalExceptionWrapper ex__)
				{
					handleExceptionWrapper__(ex__);
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

        #region  增加参数setID
        public int InvokeQuery_SetID(int SourceType, int SourceCmd, string szKey, string setID, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp)
        {
            return InvokeQuery_SetID(SourceType, SourceCmd, szKey, setID, middleNo, ParaLen, szPara, out RespLen, out szResp, defaultContext__());
        }

        public int InvokeQuery_SetID(int SourceType, int SourceCmd, string szKey, string setID, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp, Ice.Context context__)
        {
            int cnt__ = 0;
            while (true)
            {
                try
                {
                    checkTwowayOnly__("InvokeQuery");
                    Ice.ObjectDel_ delBase__ = getDelegate__();
                    IProcessInvokeDel_ del__ = (IProcessInvokeDel_)delBase__;
                    return del__.InvokeQuery_SetID(SourceType, SourceCmd, szKey, setID, middleNo, ParaLen, szPara, out RespLen, out szResp, context__);
                }
                catch (IceInternal.LocalExceptionWrapper ex__)
                {
                    handleExceptionWrapper__(ex__);
                }
                catch (Ice.LocalException ex__)
                {
                    cnt__ = handleException__(ex__, cnt__);
                }
            }
        }
        #endregion

		public int InvokeV30(string transListNo, string sequenceNo, int cmdNo, int middleNo, string request, out string respond)
		{
			return InvokeV30(transListNo, sequenceNo, cmdNo, middleNo, request, out respond, defaultContext__());
		}

		public int InvokeV30(string transListNo, string sequenceNo, int cmdNo, int middleNo, string request, out string respond, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("InvokeV30");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IProcessInvokeDel_ del__ = (IProcessInvokeDel_)delBase__;
					return del__.InvokeV30(transListNo, sequenceNo, cmdNo, middleNo, request, out respond, context__);
				}
				catch(IceInternal.LocalExceptionWrapper ex__)
				{
					handleExceptionWrapper__(ex__);
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool MiddleInvoke(string strServiceName, string strReqMsg, int iServerFlag, int iTimeout, string strUserName, string strUserPwd, out string strAnsMsg, out int iResult, out string strResInfo)
		{
			return MiddleInvoke(strServiceName, strReqMsg, iServerFlag, iTimeout, strUserName, strUserPwd, out strAnsMsg, out iResult, out strResInfo, defaultContext__());
		}

		public bool MiddleInvoke(string strServiceName, string strReqMsg, int iServerFlag, int iTimeout, string strUserName, string strUserPwd, out string strAnsMsg, out int iResult, out string strResInfo, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("MiddleInvoke");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IProcessInvokeDel_ del__ = (IProcessInvokeDel_)delBase__;
					return del__.MiddleInvoke(strServiceName, strReqMsg, iServerFlag, iTimeout, strUserName, strUserPwd, out strAnsMsg, out iResult, out strResInfo, context__);
				}
				catch(IceInternal.LocalExceptionWrapper ex__)
				{
					handleExceptionWrapper__(ex__);
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		#endregion

		#region Checked and unchecked cast operations

		public static IProcessInvokePrx checkedCast(Ice.ObjectPrx b)
		{
			if(b == null)
			{
				return null;
			}
			if(b is IProcessInvokePrx)
			{
				return (IProcessInvokePrx)b;
			}
			if(b.ice_isA("::BackProcessInvoke::IProcessInvoke"))
			{
				IProcessInvokePrxHelper h = new IProcessInvokePrxHelper();
				h.copyFrom__(b);
				return h;
			}
			return null;
		}

		public static IProcessInvokePrx checkedCast(Ice.ObjectPrx b, Ice.Context ctx)
		{
			if(b == null)
			{
				return null;
			}
			if(b is IProcessInvokePrx)
			{
				return (IProcessInvokePrx)b;
			}
			if(b.ice_isA("::BackProcessInvoke::IProcessInvoke", ctx))
			{
				IProcessInvokePrxHelper h = new IProcessInvokePrxHelper();
				h.copyFrom__(b);
				return h;
			}
			return null;
		}

		public static IProcessInvokePrx checkedCast(Ice.ObjectPrx b, string f)
		{
			if(b == null)
			{
				return null;
			}
			Ice.ObjectPrx bb = b.ice_facet(f);
			try
			{
				if(bb.ice_isA("::BackProcessInvoke::IProcessInvoke"))
				{
					IProcessInvokePrxHelper h = new IProcessInvokePrxHelper();
					h.copyFrom__(bb);
					return h;
				}
			}
			catch(Ice.FacetNotExistException)
			{
			}
			return null;
		}

		public static IProcessInvokePrx checkedCast(Ice.ObjectPrx b, string f, Ice.Context ctx)
		{
			if(b == null)
			{
				return null;
			}
			Ice.ObjectPrx bb = b.ice_facet(f);
			try
			{
				if(bb.ice_isA("::BackProcessInvoke::IProcessInvoke", ctx))
				{
					IProcessInvokePrxHelper h = new IProcessInvokePrxHelper();
					h.copyFrom__(bb);
					return h;
				}
			}
			catch(Ice.FacetNotExistException)
			{
			}
			return null;
		}

		public static IProcessInvokePrx uncheckedCast(Ice.ObjectPrx b)
		{
			if(b == null)
			{
				return null;
			}
			IProcessInvokePrxHelper h = new IProcessInvokePrxHelper();
			h.copyFrom__(b);
			return h;
		}

		public static IProcessInvokePrx uncheckedCast(Ice.ObjectPrx b, string f)
		{
			if(b == null)
			{
				return null;
			}
			Ice.ObjectPrx bb = b.ice_facet(f);
			IProcessInvokePrxHelper h = new IProcessInvokePrxHelper();
			h.copyFrom__(bb);
			return h;
		}

		#endregion

		#region Marshaling support

		protected override Ice.ObjectDelM_ createDelegateM__()
		{
			return new IProcessInvokeDelM_();
		}

		protected override Ice.ObjectDelD_ createDelegateD__()
		{
			return new IProcessInvokeDelD_();
		}

		public static void write__(IceInternal.BasicStream os__, IProcessInvokePrx v__)
		{
			os__.writeProxy(v__);
		}

		public static IProcessInvokePrx read__(IceInternal.BasicStream is__)
		{
			Ice.ObjectPrx proxy = is__.readProxy();
			if(proxy != null)
			{
				IProcessInvokePrxHelper result = new IProcessInvokePrxHelper();
				result.copyFrom__(proxy);
				return result;
			}
			return null;
		}

		#endregion
	}
}

namespace BackProcessInvoke
{
	public interface IProcessInvokeDel_ : Ice.ObjectDel_
	{
		bool Invoke(string strCommand, out int iResult, out string strResInfo, Ice.Context context__);

		bool MiddleInvoke(string strServiceName, string strReqMsg, int iServerFlag, int iTimeout, string strUserName, string strUserPwd, out string strAnsMsg, out int iResult, out string strResInfo, Ice.Context context__);

		int InvokeV30(string transListNo, string sequenceNo, int cmdNo, int middleNo, string request, out string respond, Ice.Context context__);

		int InvokeQuery(int SourceType, int SourceCmd, string szKey, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp, Ice.Context context__);

        #region 增加参数setID
        int InvokeQuery_SetID(int SourceType, int SourceCmd, string szKey, string setID, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp, Ice.Context context__);
        #endregion
	}
}

namespace BackProcessInvoke
{
	public sealed class IProcessInvokeDelM_ : Ice.ObjectDelM_, IProcessInvokeDel_
	{
		public bool Invoke(string strCommand, out int iResult, out string strResInfo, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("Invoke", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strCommand);
				}
				catch(Ice.LocalException ex__)
				{
					og__.abort(ex__);
				}
				bool ok__ = og__.invoke();
				try
				{
					IceInternal.BasicStream is__ = og__.istr();
					if(!ok__)
					{
						try
						{
							is__.throwException();
						}
						catch(Ice.UserException ex)
						{
							throw new Ice.UnknownUserException(ex);
						}
					}
					iResult = is__.readInt();
					strResInfo = is__.readString();
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.LocalExceptionWrapper(ex__, false);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public int InvokeQuery(int SourceType, int SourceCmd, string szKey, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("InvokeQuery", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeInt(SourceType);
					os__.writeInt(SourceCmd);
					os__.writeString(szKey);
					os__.writeInt(middleNo);
					os__.writeInt(ParaLen);
					os__.writeString(szPara);
				}
				catch(Ice.LocalException ex__)
				{
					og__.abort(ex__);
				}
				bool ok__ = og__.invoke();
				try
				{
					IceInternal.BasicStream is__ = og__.istr();
					if(!ok__)
					{
						try
						{
							is__.throwException();
						}
						catch(Ice.UserException ex)
						{
							throw new Ice.UnknownUserException(ex);
						}
					}
					RespLen = is__.readInt();
					szResp = is__.readString();
					int ret__;
					ret__ = is__.readInt();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.LocalExceptionWrapper(ex__, false);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}


        #region  增加参数setID
        public int InvokeQuery_SetID(int SourceType, int SourceCmd, string szKey, string setID, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp, Ice.Context context__)
        {
            IceInternal.Outgoing og__ = getOutgoing("InvokeQuery_SetID", Ice.OperationMode.Normal, context__);
            try
            {
                try
                {
                    IceInternal.BasicStream os__ = og__.ostr();
                    os__.writeInt(SourceType);
                    os__.writeInt(SourceCmd);
                    os__.writeString(szKey);
                    os__.writeString(setID);
                    os__.writeInt(middleNo);
                    os__.writeInt(ParaLen);
                    os__.writeString(szPara);
                }
                catch (Ice.LocalException ex__)
                {
                    og__.abort(ex__);
                }
                bool ok__ = og__.invoke();
                try
                {
                    IceInternal.BasicStream is__ = og__.istr();
                    if (!ok__)
                    {
                        try
                        {
                            is__.throwException();
                        }
                        catch (Ice.UserException ex)
                        {
                            throw new Ice.UnknownUserException(ex);
                        }
                    }
                    RespLen = is__.readInt();
                    szResp = is__.readString();
                    int ret__;
                    ret__ = is__.readInt();
                    return ret__;
                }
                catch (Ice.LocalException ex__)
                {
                    throw new IceInternal.LocalExceptionWrapper(ex__, false);
                }
            }
            finally
            {
                reclaimOutgoing(og__);
            }
        }
        #endregion

		public int InvokeV30(string transListNo, string sequenceNo, int cmdNo, int middleNo, string request, out string respond, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("InvokeV30", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(transListNo);
					os__.writeString(sequenceNo);
					os__.writeInt(cmdNo);
					os__.writeInt(middleNo);
					os__.writeString(request);
				}
				catch(Ice.LocalException ex__)
				{
					og__.abort(ex__);
				}
				bool ok__ = og__.invoke();
				try
				{
					IceInternal.BasicStream is__ = og__.istr();
					if(!ok__)
					{
						try
						{
							is__.throwException();
						}
						catch(Ice.UserException ex)
						{
							throw new Ice.UnknownUserException(ex);
						}
					}
					respond = is__.readString();
					int ret__;
					ret__ = is__.readInt();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.LocalExceptionWrapper(ex__, false);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool MiddleInvoke(string strServiceName, string strReqMsg, int iServerFlag, int iTimeout, string strUserName, string strUserPwd, out string strAnsMsg, out int iResult, out string strResInfo, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("MiddleInvoke", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strServiceName);
					os__.writeString(strReqMsg);
					os__.writeInt(iServerFlag);
					os__.writeInt(iTimeout);
					os__.writeString(strUserName);
					os__.writeString(strUserPwd);
				}
				catch(Ice.LocalException ex__)
				{
					og__.abort(ex__);
				}
				bool ok__ = og__.invoke();
				try
				{
					IceInternal.BasicStream is__ = og__.istr();
					if(!ok__)
					{
						try
						{
							is__.throwException();
						}
						catch(Ice.UserException ex)
						{
							throw new Ice.UnknownUserException(ex);
						}
					}
					strAnsMsg = is__.readString();
					iResult = is__.readInt();
					strResInfo = is__.readString();
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.LocalExceptionWrapper(ex__, false);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}
	}
}

namespace BackProcessInvoke
{
	public sealed class IProcessInvokeDelD_ : Ice.ObjectDelD_, IProcessInvokeDel_
	{
		public bool Invoke(string strCommand, out int iResult, out string strResInfo, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "Invoke", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IProcessInvoke)
				{
					try
					{
						return ((BackProcessInvoke.IProcessInvoke)servant__).Invoke(strCommand, out iResult, out strResInfo, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.LocalExceptionWrapper(ex__, false);
					}
					finally
					{
						direct__.destroy();
					}
				}
				else
				{
					direct__.destroy();
					Ice.OperationNotExistException opEx__ = new Ice.OperationNotExistException();
					opEx__.id = current__.id;
					opEx__.facet = current__.facet;
					opEx__.operation = current__.operation;
					throw opEx__;
				}
			}
		}


		public int InvokeQuery(int SourceType, int SourceCmd, string szKey, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "InvokeQuery", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IProcessInvoke)
				{
					try
					{
						return ((BackProcessInvoke.IProcessInvoke)servant__).InvokeQuery(SourceType, SourceCmd, szKey, middleNo, ParaLen, szPara, out RespLen, out szResp, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.LocalExceptionWrapper(ex__, false);
					}
					finally
					{
						direct__.destroy();
					}
				}
				else
				{
					direct__.destroy();
					Ice.OperationNotExistException opEx__ = new Ice.OperationNotExistException();
					opEx__.id = current__.id;
					opEx__.facet = current__.facet;
					opEx__.operation = current__.operation;
					throw opEx__;
				}
			}
		}

        #region 增加参数setID
        public int InvokeQuery_SetID(int SourceType, int SourceCmd, string szKey, string setID, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp, Ice.Context context__)
        {
            Ice.Current current__ = new Ice.Current();
            initCurrent__(ref current__, "InvokeQuery_SetID", Ice.OperationMode.Normal, context__);
            while (true)
            {
                IceInternal.Direct direct__ = new IceInternal.Direct(current__);
                object servant__ = direct__.servant();
                if (servant__ is IProcessInvoke)
                {
                    try
                    {
                        return ((BackProcessInvoke.IProcessInvoke)servant__).InvokeQuery_SetID(SourceType, SourceCmd, szKey, setID, middleNo, ParaLen, szPara, out RespLen, out szResp, current__);
                    }
                    catch (Ice.LocalException ex__)
                    {
                        throw new IceInternal.LocalExceptionWrapper(ex__, false);
                    }
                    finally
                    {
                        direct__.destroy();
                    }
                }
                else
                {
                    direct__.destroy();
                    Ice.OperationNotExistException opEx__ = new Ice.OperationNotExistException();
                    opEx__.id = current__.id;
                    opEx__.facet = current__.facet;
                    opEx__.operation = current__.operation;
                    throw opEx__;
                }
            }
        }
        #endregion

		public int InvokeV30(string transListNo, string sequenceNo, int cmdNo, int middleNo, string request, out string respond, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "InvokeV30", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IProcessInvoke)
				{
					try
					{
						return ((BackProcessInvoke.IProcessInvoke)servant__).InvokeV30(transListNo, sequenceNo, cmdNo, middleNo, request, out respond, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.LocalExceptionWrapper(ex__, false);
					}
					finally
					{
						direct__.destroy();
					}
				}
				else
				{
					direct__.destroy();
					Ice.OperationNotExistException opEx__ = new Ice.OperationNotExistException();
					opEx__.id = current__.id;
					opEx__.facet = current__.facet;
					opEx__.operation = current__.operation;
					throw opEx__;
				}
			}
		}

		public bool MiddleInvoke(string strServiceName, string strReqMsg, int iServerFlag, int iTimeout, string strUserName, string strUserPwd, out string strAnsMsg, out int iResult, out string strResInfo, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "MiddleInvoke", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IProcessInvoke)
				{
					try
					{
						return ((BackProcessInvoke.IProcessInvoke)servant__).MiddleInvoke(strServiceName, strReqMsg, iServerFlag, iTimeout, strUserName, strUserPwd, out strAnsMsg, out iResult, out strResInfo, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.LocalExceptionWrapper(ex__, false);
					}
					finally
					{
						direct__.destroy();
					}
				}
				else
				{
					direct__.destroy();
					Ice.OperationNotExistException opEx__ = new Ice.OperationNotExistException();
					opEx__.id = current__.id;
					opEx__.facet = current__.facet;
					opEx__.operation = current__.operation;
					throw opEx__;
				}
			}
		}
	}
}

namespace BackProcessInvoke
{
	public abstract class IProcessInvokeDisp_ : Ice.ObjectImpl, IProcessInvoke
	{
		#region Slice operations

		public bool Invoke(string strCommand, out int iResult, out string strResInfo)
		{
			return Invoke(strCommand, out iResult, out strResInfo, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool Invoke(string strCommand, out int iResult, out string strResInfo, Ice.Current current__);

		public bool MiddleInvoke(string strServiceName, string strReqMsg, int iServerFlag, int iTimeout, string strUserName, string strUserPwd, out string strAnsMsg, out int iResult, out string strResInfo)
		{
			return MiddleInvoke(strServiceName, strReqMsg, iServerFlag, iTimeout, strUserName, strUserPwd, out strAnsMsg, out iResult, out strResInfo, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool MiddleInvoke(string strServiceName, string strReqMsg, int iServerFlag, int iTimeout, string strUserName, string strUserPwd, out string strAnsMsg, out int iResult, out string strResInfo, Ice.Current current__);

		public int InvokeV30(string transListNo, string sequenceNo, int cmdNo, int middleNo, string request, out string respond)
		{
			return InvokeV30(transListNo, sequenceNo, cmdNo, middleNo, request, out respond, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract int InvokeV30(string transListNo, string sequenceNo, int cmdNo, int middleNo, string request, out string respond, Ice.Current current__);

		public int InvokeQuery(int SourceType, int SourceCmd, string szKey, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp)
		{
			return InvokeQuery(SourceType, SourceCmd, szKey, middleNo, ParaLen, szPara, out RespLen, out szResp, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract int InvokeQuery(int SourceType, int SourceCmd, string szKey, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp, Ice.Current current__);


        #region 增加参数setID
        public int InvokeQuery_SetID(int SourceType, int SourceCmd, string szKey, string setID, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp)
        {
            return InvokeQuery_SetID(SourceType, SourceCmd, szKey, setID, middleNo, ParaLen, szPara, out RespLen, out szResp, Ice.ObjectImpl.defaultCurrent);
        }

        public abstract int InvokeQuery_SetID(int SourceType, int SourceCmd, string szKey, string setID, int middleNo, int ParaLen, string szPara, out int RespLen, out string szResp, Ice.Current current__);
        #endregion

		#endregion

		#region Slice type-related members

		public static new string[] ids__ = 
	{
		"::BackProcessInvoke::IProcessInvoke",
		"::Ice::Object"
	};

		public override bool ice_isA(string s)
		{
			if(IceInternal.AssemblyUtil.runtime_ == IceInternal.AssemblyUtil.Runtime.Mono)
			{
				// Mono bug: System.Array.BinarySearch() uses the wrong collation sequence,
				// so we do a linear search for the time being
				int pos = 0;
				while(pos < ids__.Length)
				{
					if(ids__[pos] == s)
					{
						break;
					}
					++pos;
				}
				if(pos == ids__.Length)
				{
					pos = -1;
				}
				return pos >= 0;
			}
			else
			{
				return _System.Array.BinarySearch(ids__, s, _System.Collections.Comparer.DefaultInvariant) >= 0;
			}
		}

		public override bool ice_isA(string s, Ice.Current current__)
		{
			if(IceInternal.AssemblyUtil.runtime_ == IceInternal.AssemblyUtil.Runtime.Mono)
			{
				// Mono bug: System.Array.BinarySearch() uses the wrong collation sequence,
				// so we do a linear search for the time being
				int pos = 0;
				while(pos < ids__.Length)
				{
					if(ids__[pos] == s)
					{
						break;
					}
					++pos;
				}
				if(pos == ids__.Length)
				{
					pos = -1;
				}
				return pos >= 0;
			}
			else
			{
				return _System.Array.BinarySearch(ids__, s, _System.Collections.Comparer.DefaultInvariant) >= 0;
			}
		}

		public override string[] ice_ids()
		{
			return ids__;
		}

		public override string[] ice_ids(Ice.Current current__)
		{
			return ids__;
		}

		public override string ice_id()
		{
			return ids__[0];
		}

		public override string ice_id(Ice.Current current__)
		{
			return ids__[0];
		}

		public static new string ice_staticId()
		{
			return ids__[0];
		}

		#endregion

		#region Operation dispatch

		public static IceInternal.DispatchStatus Invoke___(IProcessInvoke obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strCommand;
			strCommand = is__.readString();
			int iResult;
			string strResInfo;
			bool ret__ = obj__.Invoke(strCommand, out iResult, out strResInfo, current__);
			os__.writeInt(iResult);
			os__.writeString(strResInfo);
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus MiddleInvoke___(IProcessInvoke obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strServiceName;
			strServiceName = is__.readString();
			string strReqMsg;
			strReqMsg = is__.readString();
			int iServerFlag;
			iServerFlag = is__.readInt();
			int iTimeout;
			iTimeout = is__.readInt();
			string strUserName;
			strUserName = is__.readString();
			string strUserPwd;
			strUserPwd = is__.readString();
			string strAnsMsg;
			int iResult;
			string strResInfo;
			bool ret__ = obj__.MiddleInvoke(strServiceName, strReqMsg, iServerFlag, iTimeout, strUserName, strUserPwd, out strAnsMsg, out iResult, out strResInfo, current__);
			os__.writeString(strAnsMsg);
			os__.writeInt(iResult);
			os__.writeString(strResInfo);
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus InvokeV30___(IProcessInvoke obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string transListNo;
			transListNo = is__.readString();
			string sequenceNo;
			sequenceNo = is__.readString();
			int cmdNo;
			cmdNo = is__.readInt();
			int middleNo;
			middleNo = is__.readInt();
			string request;
			request = is__.readString();
			string respond;
			int ret__ = obj__.InvokeV30(transListNo, sequenceNo, cmdNo, middleNo, request, out respond, current__);
			os__.writeString(respond);
			os__.writeInt(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus InvokeQuery___(IProcessInvoke obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			int SourceType;
			SourceType = is__.readInt();
			int SourceCmd;
			SourceCmd = is__.readInt();
			string szKey;
			szKey = is__.readString();
			int middleNo;
			middleNo = is__.readInt();
			int ParaLen;
			ParaLen = is__.readInt();
			string szPara;
			szPara = is__.readString();
			int RespLen;
			string szResp;
			int ret__ = obj__.InvokeQuery(SourceType, SourceCmd, szKey, middleNo, ParaLen, szPara, out RespLen, out szResp, current__);
			os__.writeInt(RespLen);
			os__.writeString(szResp);
			os__.writeInt(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

        #region 增加参数setID
        public static IceInternal.DispatchStatus InvokeQuery_SetID___(IProcessInvoke obj__, IceInternal.Incoming inS__, Ice.Current current__)
        {
            checkMode__(Ice.OperationMode.Normal, current__.mode);
            IceInternal.BasicStream is__ = inS__.istr();
            IceInternal.BasicStream os__ = inS__.ostr();
            int SourceType;
            SourceType = is__.readInt();
            int SourceCmd;
            SourceCmd = is__.readInt();
            string szKey;
            szKey = is__.readString();
            string setID; //增加参数setID
            setID = is__.readString();
            int middleNo;
            middleNo = is__.readInt();
            int ParaLen;
            ParaLen = is__.readInt();
            string szPara;
            szPara = is__.readString();
            int RespLen;
            string szResp;
            int ret__ = obj__.InvokeQuery_SetID(SourceType, SourceCmd, szKey, setID, middleNo, ParaLen, szPara, out RespLen, out szResp, current__);
            os__.writeInt(RespLen);
            os__.writeString(szResp);
            os__.writeInt(ret__);
            return IceInternal.DispatchStatus.DispatchOK;
        }
        #endregion

		private static string[] all__ =
	{
		"ice_id",
		"ice_ids",
		"ice_isA",
		"ice_ping",
		"Invoke",
		"InvokeQuery",
		"InvokeV30",
		"MiddleInvoke",
        "InvokeQuery_SetID" //增加参数setID
	};

		public override IceInternal.DispatchStatus dispatch__(IceInternal.Incoming inS__, Ice.Current current__)
		{
			int pos;
			if(IceInternal.AssemblyUtil.runtime_ == IceInternal.AssemblyUtil.Runtime.Mono)
			{
				// Mono bug: System.Array.BinarySearch() uses the wrong collation sequence,
				// so we do a linear search for the time being
				pos = 0;
				while(pos < all__.Length)
				{
					if(all__[pos] == current__.operation)
					{
						break;
					}
					++pos;
				}
				if(pos == all__.Length)
				{
					pos = -1;
				}
			}
			else
			{
				pos = _System.Array.BinarySearch(all__, current__.operation, _System.Collections.Comparer.DefaultInvariant);
			}
			if(pos < 0)
			{
				return IceInternal.DispatchStatus.DispatchOperationNotExist;
			}

			switch(pos)
			{
				case 0:
				{
					return ice_id___(this, inS__, current__);
				}
				case 1:
				{
					return ice_ids___(this, inS__, current__);
				}
				case 2:
				{
					return ice_isA___(this, inS__, current__);
				}
				case 3:
				{
					return ice_ping___(this, inS__, current__);
				}
				case 4:
				{
					return Invoke___(this, inS__, current__);
				}
				case 5:
				{
					return InvokeQuery___(this, inS__, current__);
				}
				case 6:
				{
					return InvokeV30___(this, inS__, current__);
				}
				case 7:
				{
					return MiddleInvoke___(this, inS__, current__);
				}
                case 8: //增加参数setID
                {
                    return InvokeQuery_SetID___(this, inS__, current__);
                }
			}

			_System.Diagnostics.Debug.Assert(false);
			return IceInternal.DispatchStatus.DispatchOperationNotExist;
		}

		#endregion
	}
}
