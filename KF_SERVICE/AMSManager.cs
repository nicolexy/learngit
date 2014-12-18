// **********************************************************************
//
// Copyright (c) 2003-2005 ZeroC, Inc. All rights reserved.
//
// This copy of Ice is licensed to you under the terms described in the
// ICE_LICENSE file included in this distribution.
//
// **********************************************************************

// Ice version 3.0.1
// Generated from file `C:\Documents and Settings\bruceliao\×ÀÃæ\AMSManager.ice'

using _System = System;
using _Microsoft = Microsoft;

namespace AMSManager
{
	public class StrMap : _System.Collections.DictionaryBase, _System.ICloneable
	{
		#region StrMap members

		public void AddRange(StrMap d__)
		{
			foreach(_System.Collections.DictionaryEntry e in d__)
			{
				try
				{
					InnerHashtable.Add(e.Key, e.Value);
				}
				catch(_System.ArgumentException)
				{
					// ignore
				}
			}
		}

		#endregion

		#region IDictionary members

		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public _System.Collections.ICollection Keys
		{
			get
			{
				return InnerHashtable.Keys;
			}
		}

		public _System.Collections.ICollection Values
		{
			get
			{
				return InnerHashtable.Values;
			}
		}

		#region Indexer

		public string this[string key]
		{
			get
			{
				return (string)InnerHashtable[key];
			}
			set
			{
				InnerHashtable[key] = value;
			}
		}

		#endregion

		public void Add(string key, string value)
		{
			InnerHashtable.Add(key, value);
		}

		public void Remove(string key)
		{
			InnerHashtable.Remove(key);
		}

		public bool Contains(string key)
		{
			return InnerHashtable.Contains(key);
		}

		#endregion

		#region ICollection members

		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			StrMap d = new StrMap();
			foreach(_System.Collections.DictionaryEntry e in InnerHashtable)
			{
				d.InnerHashtable.Add(e.Key, e.Value);
			}
			return d;
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int hash = 0;
			foreach(_System.Collections.DictionaryEntry e in InnerHashtable)
			{
				hash = 5 * hash + e.Key.GetHashCode();
				if(e.Value != null)
				{
					hash = 5 * hash + e.Value.GetHashCode();
				}
			}
			return hash;
		}

		public override bool Equals(object other)
		{
			if(object.ReferenceEquals(this, other))
			{
				return true;
			}
			if(!(other is StrMap))
			{
				return false;
			}
			if(Count != ((StrMap)other).Count)
			{
				return false;
			}
			string[] klhs__ = new string[Count];
			Keys.CopyTo(klhs__, 0);
			_System.Array.Sort(klhs__);
			string[] krhs__ = new string[((StrMap)other).Count];
			((StrMap)other).Keys.CopyTo(krhs__, 0);
			_System.Array.Sort(krhs__);
			for(int i = 0; i < Count; ++i)
			{
				if(!klhs__[i].Equals(krhs__[i]))
				{
					return false;
				}
			}
			string[] vlhs__ = new string[Count];
			Values.CopyTo(vlhs__, 0);
			_System.Array.Sort(vlhs__);
			string[] vrhs__ = new string[((StrMap)other).Count];
			((StrMap)other).Values.CopyTo(vrhs__, 0);
			_System.Array.Sort(vrhs__);
			for(int i = 0; i < Count; ++i)
			{
				if(vlhs__[i] == null)
				{
					if(vrhs__[i] != null)
					{
						return false;
					}
				}
				else if(!vlhs__[i].Equals(vrhs__[i]))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(StrMap lhs__, StrMap rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(StrMap lhs__, StrMap rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion
	}

	public class STRUMerchantInfo : _System.ICloneable
	{
		#region Slice data members

		public string strFSpid;

		public string strFName;

		public int lFUid;

		public int lFUidMiddle;

		public int lFFeeContract;

		public string strFContract;

		public string strFspecial;

		public string strFuserId;

		public string strFModifyTime;

		public short iFrecordStatus;

		public string strFmerKey;

		public short iFstate;

		public short iFlistState;

		public int lFstandby1;

		public int lFstandby2;

		public int lFstandby3;

		public int lFstandby4;

		public int lFstandby5;

		public string strFstandby6;

		public string strFstandby7;

		public string strFstandby8;

		public string strFstandby9;

		public string strFstandby10;

		public string strFmemo;

		public string strFip;

		#endregion

		#region Constructors

		public STRUMerchantInfo()
		{
		}

		public STRUMerchantInfo(string strFSpid, string strFName, int lFUid, int lFUidMiddle, int lFFeeContract, string strFContract, string strFspecial, string strFuserId, string strFModifyTime, short iFrecordStatus, string strFmerKey, short iFstate, short iFlistState, int lFstandby1, int lFstandby2, int lFstandby3, int lFstandby4, int lFstandby5, string strFstandby6, string strFstandby7, string strFstandby8, string strFstandby9, string strFstandby10, string strFmemo, string strFip)
		{
			this.strFSpid = strFSpid;
			this.strFName = strFName;
			this.lFUid = lFUid;
			this.lFUidMiddle = lFUidMiddle;
			this.lFFeeContract = lFFeeContract;
			this.strFContract = strFContract;
			this.strFspecial = strFspecial;
			this.strFuserId = strFuserId;
			this.strFModifyTime = strFModifyTime;
			this.iFrecordStatus = iFrecordStatus;
			this.strFmerKey = strFmerKey;
			this.iFstate = iFstate;
			this.iFlistState = iFlistState;
			this.lFstandby1 = lFstandby1;
			this.lFstandby2 = lFstandby2;
			this.lFstandby3 = lFstandby3;
			this.lFstandby4 = lFstandby4;
			this.lFstandby5 = lFstandby5;
			this.strFstandby6 = strFstandby6;
			this.strFstandby7 = strFstandby7;
			this.strFstandby8 = strFstandby8;
			this.strFstandby9 = strFstandby9;
			this.strFstandby10 = strFstandby10;
			this.strFmemo = strFmemo;
			this.strFip = strFip;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			if(strFSpid != null)
			{
				h__ = 5 * h__ + strFSpid.GetHashCode();
			}
			if(strFName != null)
			{
				h__ = 5 * h__ + strFName.GetHashCode();
			}
			h__ = 5 * h__ + lFUid.GetHashCode();
			h__ = 5 * h__ + lFUidMiddle.GetHashCode();
			h__ = 5 * h__ + lFFeeContract.GetHashCode();
			if(strFContract != null)
			{
				h__ = 5 * h__ + strFContract.GetHashCode();
			}
			if(strFspecial != null)
			{
				h__ = 5 * h__ + strFspecial.GetHashCode();
			}
			if(strFuserId != null)
			{
				h__ = 5 * h__ + strFuserId.GetHashCode();
			}
			if(strFModifyTime != null)
			{
				h__ = 5 * h__ + strFModifyTime.GetHashCode();
			}
			h__ = 5 * h__ + iFrecordStatus.GetHashCode();
			if(strFmerKey != null)
			{
				h__ = 5 * h__ + strFmerKey.GetHashCode();
			}
			h__ = 5 * h__ + iFstate.GetHashCode();
			h__ = 5 * h__ + iFlistState.GetHashCode();
			h__ = 5 * h__ + lFstandby1.GetHashCode();
			h__ = 5 * h__ + lFstandby2.GetHashCode();
			h__ = 5 * h__ + lFstandby3.GetHashCode();
			h__ = 5 * h__ + lFstandby4.GetHashCode();
			h__ = 5 * h__ + lFstandby5.GetHashCode();
			if(strFstandby6 != null)
			{
				h__ = 5 * h__ + strFstandby6.GetHashCode();
			}
			if(strFstandby7 != null)
			{
				h__ = 5 * h__ + strFstandby7.GetHashCode();
			}
			if(strFstandby8 != null)
			{
				h__ = 5 * h__ + strFstandby8.GetHashCode();
			}
			if(strFstandby9 != null)
			{
				h__ = 5 * h__ + strFstandby9.GetHashCode();
			}
			if(strFstandby10 != null)
			{
				h__ = 5 * h__ + strFstandby10.GetHashCode();
			}
			if(strFmemo != null)
			{
				h__ = 5 * h__ + strFmemo.GetHashCode();
			}
			if(strFip != null)
			{
				h__ = 5 * h__ + strFip.GetHashCode();
			}
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			STRUMerchantInfo o__ = (STRUMerchantInfo)other__;
			if(strFSpid == null)
			{
				if(o__.strFSpid != null)
				{
					return false;
				}
			}
			else
			{
				if(!strFSpid.Equals(o__.strFSpid))
				{
					return false;
				}
			}
			if(strFName == null)
			{
				if(o__.strFName != null)
				{
					return false;
				}
			}
			else
			{
				if(!strFName.Equals(o__.strFName))
				{
					return false;
				}
			}
			if(!lFUid.Equals(o__.lFUid))
			{
				return false;
			}
			if(!lFUidMiddle.Equals(o__.lFUidMiddle))
			{
				return false;
			}
			if(!lFFeeContract.Equals(o__.lFFeeContract))
			{
				return false;
			}
			if(strFContract == null)
			{
				if(o__.strFContract != null)
				{
					return false;
				}
			}
			else
			{
				if(!strFContract.Equals(o__.strFContract))
				{
					return false;
				}
			}
			if(strFspecial == null)
			{
				if(o__.strFspecial != null)
				{
					return false;
				}
			}
			else
			{
				if(!strFspecial.Equals(o__.strFspecial))
				{
					return false;
				}
			}
			if(strFuserId == null)
			{
				if(o__.strFuserId != null)
				{
					return false;
				}
			}
			else
			{
				if(!strFuserId.Equals(o__.strFuserId))
				{
					return false;
				}
			}
			if(strFModifyTime == null)
			{
				if(o__.strFModifyTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!strFModifyTime.Equals(o__.strFModifyTime))
				{
					return false;
				}
			}
			if(!iFrecordStatus.Equals(o__.iFrecordStatus))
			{
				return false;
			}
			if(strFmerKey == null)
			{
				if(o__.strFmerKey != null)
				{
					return false;
				}
			}
			else
			{
				if(!strFmerKey.Equals(o__.strFmerKey))
				{
					return false;
				}
			}
			if(!iFstate.Equals(o__.iFstate))
			{
				return false;
			}
			if(!iFlistState.Equals(o__.iFlistState))
			{
				return false;
			}
			if(!lFstandby1.Equals(o__.lFstandby1))
			{
				return false;
			}
			if(!lFstandby2.Equals(o__.lFstandby2))
			{
				return false;
			}
			if(!lFstandby3.Equals(o__.lFstandby3))
			{
				return false;
			}
			if(!lFstandby4.Equals(o__.lFstandby4))
			{
				return false;
			}
			if(!lFstandby5.Equals(o__.lFstandby5))
			{
				return false;
			}
			if(strFstandby6 == null)
			{
				if(o__.strFstandby6 != null)
				{
					return false;
				}
			}
			else
			{
				if(!strFstandby6.Equals(o__.strFstandby6))
				{
					return false;
				}
			}
			if(strFstandby7 == null)
			{
				if(o__.strFstandby7 != null)
				{
					return false;
				}
			}
			else
			{
				if(!strFstandby7.Equals(o__.strFstandby7))
				{
					return false;
				}
			}
			if(strFstandby8 == null)
			{
				if(o__.strFstandby8 != null)
				{
					return false;
				}
			}
			else
			{
				if(!strFstandby8.Equals(o__.strFstandby8))
				{
					return false;
				}
			}
			if(strFstandby9 == null)
			{
				if(o__.strFstandby9 != null)
				{
					return false;
				}
			}
			else
			{
				if(!strFstandby9.Equals(o__.strFstandby9))
				{
					return false;
				}
			}
			if(strFstandby10 == null)
			{
				if(o__.strFstandby10 != null)
				{
					return false;
				}
			}
			else
			{
				if(!strFstandby10.Equals(o__.strFstandby10))
				{
					return false;
				}
			}
			if(strFmemo == null)
			{
				if(o__.strFmemo != null)
				{
					return false;
				}
			}
			else
			{
				if(!strFmemo.Equals(o__.strFmemo))
				{
					return false;
				}
			}
			if(strFip == null)
			{
				if(o__.strFip != null)
				{
					return false;
				}
			}
			else
			{
				if(!strFip.Equals(o__.strFip))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(STRUMerchantInfo lhs__, STRUMerchantInfo rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(STRUMerchantInfo lhs__, STRUMerchantInfo rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeString(strFSpid);
			os__.writeString(strFName);
			os__.writeInt(lFUid);
			os__.writeInt(lFUidMiddle);
			os__.writeInt(lFFeeContract);
			os__.writeString(strFContract);
			os__.writeString(strFspecial);
			os__.writeString(strFuserId);
			os__.writeString(strFModifyTime);
			os__.writeShort(iFrecordStatus);
			os__.writeString(strFmerKey);
			os__.writeShort(iFstate);
			os__.writeShort(iFlistState);
			os__.writeInt(lFstandby1);
			os__.writeInt(lFstandby2);
			os__.writeInt(lFstandby3);
			os__.writeInt(lFstandby4);
			os__.writeInt(lFstandby5);
			os__.writeString(strFstandby6);
			os__.writeString(strFstandby7);
			os__.writeString(strFstandby8);
			os__.writeString(strFstandby9);
			os__.writeString(strFstandby10);
			os__.writeString(strFmemo);
			os__.writeString(strFip);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			strFSpid = is__.readString();
			strFName = is__.readString();
			lFUid = is__.readInt();
			lFUidMiddle = is__.readInt();
			lFFeeContract = is__.readInt();
			strFContract = is__.readString();
			strFspecial = is__.readString();
			strFuserId = is__.readString();
			strFModifyTime = is__.readString();
			iFrecordStatus = is__.readShort();
			strFmerKey = is__.readString();
			iFstate = is__.readShort();
			iFlistState = is__.readShort();
			lFstandby1 = is__.readInt();
			lFstandby2 = is__.readInt();
			lFstandby3 = is__.readInt();
			lFstandby4 = is__.readInt();
			lFstandby5 = is__.readInt();
			strFstandby6 = is__.readString();
			strFstandby7 = is__.readString();
			strFstandby8 = is__.readString();
			strFstandby9 = is__.readString();
			strFstandby10 = is__.readString();
			strFmemo = is__.readString();
			strFip = is__.readString();
		}

		#endregion
	}

	public class STRUOperInfo : _System.ICloneable
	{
		#region Slice data members

		public string strMid;

		public string strMin;

		public string strOpname;

		public string strMchname;

		public int iLevel;

		#endregion

		#region Constructors

		public STRUOperInfo()
		{
		}

		public STRUOperInfo(string strMid, string strMin, string strOpname, string strMchname, int iLevel)
		{
			this.strMid = strMid;
			this.strMin = strMin;
			this.strOpname = strOpname;
			this.strMchname = strMchname;
			this.iLevel = iLevel;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			if(strMid != null)
			{
				h__ = 5 * h__ + strMid.GetHashCode();
			}
			if(strMin != null)
			{
				h__ = 5 * h__ + strMin.GetHashCode();
			}
			if(strOpname != null)
			{
				h__ = 5 * h__ + strOpname.GetHashCode();
			}
			if(strMchname != null)
			{
				h__ = 5 * h__ + strMchname.GetHashCode();
			}
			h__ = 5 * h__ + iLevel.GetHashCode();
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			STRUOperInfo o__ = (STRUOperInfo)other__;
			if(strMid == null)
			{
				if(o__.strMid != null)
				{
					return false;
				}
			}
			else
			{
				if(!strMid.Equals(o__.strMid))
				{
					return false;
				}
			}
			if(strMin == null)
			{
				if(o__.strMin != null)
				{
					return false;
				}
			}
			else
			{
				if(!strMin.Equals(o__.strMin))
				{
					return false;
				}
			}
			if(strOpname == null)
			{
				if(o__.strOpname != null)
				{
					return false;
				}
			}
			else
			{
				if(!strOpname.Equals(o__.strOpname))
				{
					return false;
				}
			}
			if(strMchname == null)
			{
				if(o__.strMchname != null)
				{
					return false;
				}
			}
			else
			{
				if(!strMchname.Equals(o__.strMchname))
				{
					return false;
				}
			}
			if(!iLevel.Equals(o__.iLevel))
			{
				return false;
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(STRUOperInfo lhs__, STRUOperInfo rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(STRUOperInfo lhs__, STRUOperInfo rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeString(strMid);
			os__.writeString(strMin);
			os__.writeString(strOpname);
			os__.writeString(strMchname);
			os__.writeInt(iLevel);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			strMid = is__.readString();
			strMin = is__.readString();
			strOpname = is__.readString();
			strMchname = is__.readString();
			iLevel = is__.readInt();
		}

		#endregion
	}

	public class STRUTranInfo : _System.ICloneable
	{
		#region Slice data members

		public string strTranTime;

		public string strTransactionID;

		public string strOrderNO;

		public int iBankType;

		public string strBankOrderNO;

		public string strOppQQID;

		public int iStatus;

		public int iFee;

		public string strProductDesc;

		public int iOrderFlag;

		#endregion

		#region Constructors

		public STRUTranInfo()
		{
		}

		public STRUTranInfo(string strTranTime, string strTransactionID, string strOrderNO, int iBankType, string strBankOrderNO, string strOppQQID, int iStatus, int iFee, string strProductDesc, int iOrderFlag)
		{
			this.strTranTime = strTranTime;
			this.strTransactionID = strTransactionID;
			this.strOrderNO = strOrderNO;
			this.iBankType = iBankType;
			this.strBankOrderNO = strBankOrderNO;
			this.strOppQQID = strOppQQID;
			this.iStatus = iStatus;
			this.iFee = iFee;
			this.strProductDesc = strProductDesc;
			this.iOrderFlag = iOrderFlag;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			if(strTranTime != null)
			{
				h__ = 5 * h__ + strTranTime.GetHashCode();
			}
			if(strTransactionID != null)
			{
				h__ = 5 * h__ + strTransactionID.GetHashCode();
			}
			if(strOrderNO != null)
			{
				h__ = 5 * h__ + strOrderNO.GetHashCode();
			}
			h__ = 5 * h__ + iBankType.GetHashCode();
			if(strBankOrderNO != null)
			{
				h__ = 5 * h__ + strBankOrderNO.GetHashCode();
			}
			if(strOppQQID != null)
			{
				h__ = 5 * h__ + strOppQQID.GetHashCode();
			}
			h__ = 5 * h__ + iStatus.GetHashCode();
			h__ = 5 * h__ + iFee.GetHashCode();
			if(strProductDesc != null)
			{
				h__ = 5 * h__ + strProductDesc.GetHashCode();
			}
			h__ = 5 * h__ + iOrderFlag.GetHashCode();
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			STRUTranInfo o__ = (STRUTranInfo)other__;
			if(strTranTime == null)
			{
				if(o__.strTranTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!strTranTime.Equals(o__.strTranTime))
				{
					return false;
				}
			}
			if(strTransactionID == null)
			{
				if(o__.strTransactionID != null)
				{
					return false;
				}
			}
			else
			{
				if(!strTransactionID.Equals(o__.strTransactionID))
				{
					return false;
				}
			}
			if(strOrderNO == null)
			{
				if(o__.strOrderNO != null)
				{
					return false;
				}
			}
			else
			{
				if(!strOrderNO.Equals(o__.strOrderNO))
				{
					return false;
				}
			}
			if(!iBankType.Equals(o__.iBankType))
			{
				return false;
			}
			if(strBankOrderNO == null)
			{
				if(o__.strBankOrderNO != null)
				{
					return false;
				}
			}
			else
			{
				if(!strBankOrderNO.Equals(o__.strBankOrderNO))
				{
					return false;
				}
			}
			if(strOppQQID == null)
			{
				if(o__.strOppQQID != null)
				{
					return false;
				}
			}
			else
			{
				if(!strOppQQID.Equals(o__.strOppQQID))
				{
					return false;
				}
			}
			if(!iStatus.Equals(o__.iStatus))
			{
				return false;
			}
			if(!iFee.Equals(o__.iFee))
			{
				return false;
			}
			if(strProductDesc == null)
			{
				if(o__.strProductDesc != null)
				{
					return false;
				}
			}
			else
			{
				if(!strProductDesc.Equals(o__.strProductDesc))
				{
					return false;
				}
			}
			if(!iOrderFlag.Equals(o__.iOrderFlag))
			{
				return false;
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(STRUTranInfo lhs__, STRUTranInfo rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(STRUTranInfo lhs__, STRUTranInfo rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeString(strTranTime);
			os__.writeString(strTransactionID);
			os__.writeString(strOrderNO);
			os__.writeInt(iBankType);
			os__.writeString(strBankOrderNO);
			os__.writeString(strOppQQID);
			os__.writeInt(iStatus);
			os__.writeInt(iFee);
			os__.writeString(strProductDesc);
			os__.writeInt(iOrderFlag);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			strTranTime = is__.readString();
			strTransactionID = is__.readString();
			strOrderNO = is__.readString();
			iBankType = is__.readInt();
			strBankOrderNO = is__.readString();
			strOppQQID = is__.readString();
			iStatus = is__.readInt();
			iFee = is__.readInt();
			strProductDesc = is__.readString();
			iOrderFlag = is__.readInt();
		}

		#endregion
	}

	public class STRUOrderInfo : _System.ICloneable
	{
		#region Slice data members

		public string strTransactionID;

		public string strOrderNO;

		public int iBankType;

		public string strBankOrderNO;

		public string strOppQQID;

		public string strOppName;

		public int iType;

		public int iStatus;

		public string strTradeTime;

		public int iPrice;

		public int iTransportFee;

		public int iFee;

		public string strProductDesc;

		public int iOrderFlag;

		#endregion

		#region Constructors

		public STRUOrderInfo()
		{
		}

		public STRUOrderInfo(string strTransactionID, string strOrderNO, int iBankType, string strBankOrderNO, string strOppQQID, string strOppName, int iType, int iStatus, string strTradeTime, int iPrice, int iTransportFee, int iFee, string strProductDesc, int iOrderFlag)
		{
			this.strTransactionID = strTransactionID;
			this.strOrderNO = strOrderNO;
			this.iBankType = iBankType;
			this.strBankOrderNO = strBankOrderNO;
			this.strOppQQID = strOppQQID;
			this.strOppName = strOppName;
			this.iType = iType;
			this.iStatus = iStatus;
			this.strTradeTime = strTradeTime;
			this.iPrice = iPrice;
			this.iTransportFee = iTransportFee;
			this.iFee = iFee;
			this.strProductDesc = strProductDesc;
			this.iOrderFlag = iOrderFlag;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			if(strTransactionID != null)
			{
				h__ = 5 * h__ + strTransactionID.GetHashCode();
			}
			if(strOrderNO != null)
			{
				h__ = 5 * h__ + strOrderNO.GetHashCode();
			}
			h__ = 5 * h__ + iBankType.GetHashCode();
			if(strBankOrderNO != null)
			{
				h__ = 5 * h__ + strBankOrderNO.GetHashCode();
			}
			if(strOppQQID != null)
			{
				h__ = 5 * h__ + strOppQQID.GetHashCode();
			}
			if(strOppName != null)
			{
				h__ = 5 * h__ + strOppName.GetHashCode();
			}
			h__ = 5 * h__ + iType.GetHashCode();
			h__ = 5 * h__ + iStatus.GetHashCode();
			if(strTradeTime != null)
			{
				h__ = 5 * h__ + strTradeTime.GetHashCode();
			}
			h__ = 5 * h__ + iPrice.GetHashCode();
			h__ = 5 * h__ + iTransportFee.GetHashCode();
			h__ = 5 * h__ + iFee.GetHashCode();
			if(strProductDesc != null)
			{
				h__ = 5 * h__ + strProductDesc.GetHashCode();
			}
			h__ = 5 * h__ + iOrderFlag.GetHashCode();
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			STRUOrderInfo o__ = (STRUOrderInfo)other__;
			if(strTransactionID == null)
			{
				if(o__.strTransactionID != null)
				{
					return false;
				}
			}
			else
			{
				if(!strTransactionID.Equals(o__.strTransactionID))
				{
					return false;
				}
			}
			if(strOrderNO == null)
			{
				if(o__.strOrderNO != null)
				{
					return false;
				}
			}
			else
			{
				if(!strOrderNO.Equals(o__.strOrderNO))
				{
					return false;
				}
			}
			if(!iBankType.Equals(o__.iBankType))
			{
				return false;
			}
			if(strBankOrderNO == null)
			{
				if(o__.strBankOrderNO != null)
				{
					return false;
				}
			}
			else
			{
				if(!strBankOrderNO.Equals(o__.strBankOrderNO))
				{
					return false;
				}
			}
			if(strOppQQID == null)
			{
				if(o__.strOppQQID != null)
				{
					return false;
				}
			}
			else
			{
				if(!strOppQQID.Equals(o__.strOppQQID))
				{
					return false;
				}
			}
			if(strOppName == null)
			{
				if(o__.strOppName != null)
				{
					return false;
				}
			}
			else
			{
				if(!strOppName.Equals(o__.strOppName))
				{
					return false;
				}
			}
			if(!iType.Equals(o__.iType))
			{
				return false;
			}
			if(!iStatus.Equals(o__.iStatus))
			{
				return false;
			}
			if(strTradeTime == null)
			{
				if(o__.strTradeTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!strTradeTime.Equals(o__.strTradeTime))
				{
					return false;
				}
			}
			if(!iPrice.Equals(o__.iPrice))
			{
				return false;
			}
			if(!iTransportFee.Equals(o__.iTransportFee))
			{
				return false;
			}
			if(!iFee.Equals(o__.iFee))
			{
				return false;
			}
			if(strProductDesc == null)
			{
				if(o__.strProductDesc != null)
				{
					return false;
				}
			}
			else
			{
				if(!strProductDesc.Equals(o__.strProductDesc))
				{
					return false;
				}
			}
			if(!iOrderFlag.Equals(o__.iOrderFlag))
			{
				return false;
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(STRUOrderInfo lhs__, STRUOrderInfo rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(STRUOrderInfo lhs__, STRUOrderInfo rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeString(strTransactionID);
			os__.writeString(strOrderNO);
			os__.writeInt(iBankType);
			os__.writeString(strBankOrderNO);
			os__.writeString(strOppQQID);
			os__.writeString(strOppName);
			os__.writeInt(iType);
			os__.writeInt(iStatus);
			os__.writeString(strTradeTime);
			os__.writeInt(iPrice);
			os__.writeInt(iTransportFee);
			os__.writeInt(iFee);
			os__.writeString(strProductDesc);
			os__.writeInt(iOrderFlag);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			strTransactionID = is__.readString();
			strOrderNO = is__.readString();
			iBankType = is__.readInt();
			strBankOrderNO = is__.readString();
			strOppQQID = is__.readString();
			strOppName = is__.readString();
			iType = is__.readInt();
			iStatus = is__.readInt();
			strTradeTime = is__.readString();
			iPrice = is__.readInt();
			iTransportFee = is__.readInt();
			iFee = is__.readInt();
			strProductDesc = is__.readString();
			iOrderFlag = is__.readInt();
		}

		#endregion
	}

	public class STRUStatOrderInfo : _System.ICloneable
	{
		#region Slice data members

		public int iTotalCount;

		public int iSuccessCount;

		public int iRefundCount;

		public string totalAmount;

		public string successAmount;

		public string refundAmount;

		#endregion

		#region Constructors

		public STRUStatOrderInfo()
		{
		}

		public STRUStatOrderInfo(int iTotalCount, int iSuccessCount, int iRefundCount, string totalAmount, string successAmount, string refundAmount)
		{
			this.iTotalCount = iTotalCount;
			this.iSuccessCount = iSuccessCount;
			this.iRefundCount = iRefundCount;
			this.totalAmount = totalAmount;
			this.successAmount = successAmount;
			this.refundAmount = refundAmount;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			h__ = 5 * h__ + iTotalCount.GetHashCode();
			h__ = 5 * h__ + iSuccessCount.GetHashCode();
			h__ = 5 * h__ + iRefundCount.GetHashCode();
			if(totalAmount != null)
			{
				h__ = 5 * h__ + totalAmount.GetHashCode();
			}
			if(successAmount != null)
			{
				h__ = 5 * h__ + successAmount.GetHashCode();
			}
			if(refundAmount != null)
			{
				h__ = 5 * h__ + refundAmount.GetHashCode();
			}
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			STRUStatOrderInfo o__ = (STRUStatOrderInfo)other__;
			if(!iTotalCount.Equals(o__.iTotalCount))
			{
				return false;
			}
			if(!iSuccessCount.Equals(o__.iSuccessCount))
			{
				return false;
			}
			if(!iRefundCount.Equals(o__.iRefundCount))
			{
				return false;
			}
			if(totalAmount == null)
			{
				if(o__.totalAmount != null)
				{
					return false;
				}
			}
			else
			{
				if(!totalAmount.Equals(o__.totalAmount))
				{
					return false;
				}
			}
			if(successAmount == null)
			{
				if(o__.successAmount != null)
				{
					return false;
				}
			}
			else
			{
				if(!successAmount.Equals(o__.successAmount))
				{
					return false;
				}
			}
			if(refundAmount == null)
			{
				if(o__.refundAmount != null)
				{
					return false;
				}
			}
			else
			{
				if(!refundAmount.Equals(o__.refundAmount))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(STRUStatOrderInfo lhs__, STRUStatOrderInfo rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(STRUStatOrderInfo lhs__, STRUStatOrderInfo rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeInt(iTotalCount);
			os__.writeInt(iSuccessCount);
			os__.writeInt(iRefundCount);
			os__.writeString(totalAmount);
			os__.writeString(successAmount);
			os__.writeString(refundAmount);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			iTotalCount = is__.readInt();
			iSuccessCount = is__.readInt();
			iRefundCount = is__.readInt();
			totalAmount = is__.readString();
			successAmount = is__.readString();
			refundAmount = is__.readString();
		}

		#endregion
	}

	public class STRUSettlement : _System.ICloneable
	{
		#region Slice data members

		public string strSpid;

		public string strMin;

		public int iOffset;

		public int iLimit;

		public int iState;

		public string strCreateTimeBegin;

		public string strCreateTimeEnd;

		#endregion

		#region Constructors

		public STRUSettlement()
		{
		}

		public STRUSettlement(string strSpid, string strMin, int iOffset, int iLimit, int iState, string strCreateTimeBegin, string strCreateTimeEnd)
		{
			this.strSpid = strSpid;
			this.strMin = strMin;
			this.iOffset = iOffset;
			this.iLimit = iLimit;
			this.iState = iState;
			this.strCreateTimeBegin = strCreateTimeBegin;
			this.strCreateTimeEnd = strCreateTimeEnd;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			if(strSpid != null)
			{
				h__ = 5 * h__ + strSpid.GetHashCode();
			}
			if(strMin != null)
			{
				h__ = 5 * h__ + strMin.GetHashCode();
			}
			h__ = 5 * h__ + iOffset.GetHashCode();
			h__ = 5 * h__ + iLimit.GetHashCode();
			h__ = 5 * h__ + iState.GetHashCode();
			if(strCreateTimeBegin != null)
			{
				h__ = 5 * h__ + strCreateTimeBegin.GetHashCode();
			}
			if(strCreateTimeEnd != null)
			{
				h__ = 5 * h__ + strCreateTimeEnd.GetHashCode();
			}
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			STRUSettlement o__ = (STRUSettlement)other__;
			if(strSpid == null)
			{
				if(o__.strSpid != null)
				{
					return false;
				}
			}
			else
			{
				if(!strSpid.Equals(o__.strSpid))
				{
					return false;
				}
			}
			if(strMin == null)
			{
				if(o__.strMin != null)
				{
					return false;
				}
			}
			else
			{
				if(!strMin.Equals(o__.strMin))
				{
					return false;
				}
			}
			if(!iOffset.Equals(o__.iOffset))
			{
				return false;
			}
			if(!iLimit.Equals(o__.iLimit))
			{
				return false;
			}
			if(!iState.Equals(o__.iState))
			{
				return false;
			}
			if(strCreateTimeBegin == null)
			{
				if(o__.strCreateTimeBegin != null)
				{
					return false;
				}
			}
			else
			{
				if(!strCreateTimeBegin.Equals(o__.strCreateTimeBegin))
				{
					return false;
				}
			}
			if(strCreateTimeEnd == null)
			{
				if(o__.strCreateTimeEnd != null)
				{
					return false;
				}
			}
			else
			{
				if(!strCreateTimeEnd.Equals(o__.strCreateTimeEnd))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(STRUSettlement lhs__, STRUSettlement rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(STRUSettlement lhs__, STRUSettlement rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeString(strSpid);
			os__.writeString(strMin);
			os__.writeInt(iOffset);
			os__.writeInt(iLimit);
			os__.writeInt(iState);
			os__.writeString(strCreateTimeBegin);
			os__.writeString(strCreateTimeEnd);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			strSpid = is__.readString();
			strMin = is__.readString();
			iOffset = is__.readInt();
			iLimit = is__.readInt();
			iState = is__.readInt();
			strCreateTimeBegin = is__.readString();
			strCreateTimeEnd = is__.readString();
		}

		#endregion
	}

	public class STRUSettlementDetail : _System.ICloneable
	{
		#region Slice data members

		public string iNum;

		public string sChannel;

		public string sStime;

		public string sEtime;

		public string lAmt;

		public int lFeerate;

		public string lFee;

		public string lMchamt;

		public string sCashtime;

		public int lFeeNo;

		#endregion

		#region Constructors

		public STRUSettlementDetail()
		{
		}

		public STRUSettlementDetail(string iNum, string sChannel, string sStime, string sEtime, string lAmt, int lFeerate, string lFee, string lMchamt, string sCashtime, int lFeeNo)
		{
			this.iNum = iNum;
			this.sChannel = sChannel;
			this.sStime = sStime;
			this.sEtime = sEtime;
			this.lAmt = lAmt;
			this.lFeerate = lFeerate;
			this.lFee = lFee;
			this.lMchamt = lMchamt;
			this.sCashtime = sCashtime;
			this.lFeeNo = lFeeNo;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			if(iNum != null)
			{
				h__ = 5 * h__ + iNum.GetHashCode();
			}
			if(sChannel != null)
			{
				h__ = 5 * h__ + sChannel.GetHashCode();
			}
			if(sStime != null)
			{
				h__ = 5 * h__ + sStime.GetHashCode();
			}
			if(sEtime != null)
			{
				h__ = 5 * h__ + sEtime.GetHashCode();
			}
			if(lAmt != null)
			{
				h__ = 5 * h__ + lAmt.GetHashCode();
			}
			h__ = 5 * h__ + lFeerate.GetHashCode();
			if(lFee != null)
			{
				h__ = 5 * h__ + lFee.GetHashCode();
			}
			if(lMchamt != null)
			{
				h__ = 5 * h__ + lMchamt.GetHashCode();
			}
			if(sCashtime != null)
			{
				h__ = 5 * h__ + sCashtime.GetHashCode();
			}
			h__ = 5 * h__ + lFeeNo.GetHashCode();
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			STRUSettlementDetail o__ = (STRUSettlementDetail)other__;
			if(iNum == null)
			{
				if(o__.iNum != null)
				{
					return false;
				}
			}
			else
			{
				if(!iNum.Equals(o__.iNum))
				{
					return false;
				}
			}
			if(sChannel == null)
			{
				if(o__.sChannel != null)
				{
					return false;
				}
			}
			else
			{
				if(!sChannel.Equals(o__.sChannel))
				{
					return false;
				}
			}
			if(sStime == null)
			{
				if(o__.sStime != null)
				{
					return false;
				}
			}
			else
			{
				if(!sStime.Equals(o__.sStime))
				{
					return false;
				}
			}
			if(sEtime == null)
			{
				if(o__.sEtime != null)
				{
					return false;
				}
			}
			else
			{
				if(!sEtime.Equals(o__.sEtime))
				{
					return false;
				}
			}
			if(lAmt == null)
			{
				if(o__.lAmt != null)
				{
					return false;
				}
			}
			else
			{
				if(!lAmt.Equals(o__.lAmt))
				{
					return false;
				}
			}
			if(!lFeerate.Equals(o__.lFeerate))
			{
				return false;
			}
			if(lFee == null)
			{
				if(o__.lFee != null)
				{
					return false;
				}
			}
			else
			{
				if(!lFee.Equals(o__.lFee))
				{
					return false;
				}
			}
			if(lMchamt == null)
			{
				if(o__.lMchamt != null)
				{
					return false;
				}
			}
			else
			{
				if(!lMchamt.Equals(o__.lMchamt))
				{
					return false;
				}
			}
			if(sCashtime == null)
			{
				if(o__.sCashtime != null)
				{
					return false;
				}
			}
			else
			{
				if(!sCashtime.Equals(o__.sCashtime))
				{
					return false;
				}
			}
			if(!lFeeNo.Equals(o__.lFeeNo))
			{
				return false;
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(STRUSettlementDetail lhs__, STRUSettlementDetail rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(STRUSettlementDetail lhs__, STRUSettlementDetail rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeString(iNum);
			os__.writeString(sChannel);
			os__.writeString(sStime);
			os__.writeString(sEtime);
			os__.writeString(lAmt);
			os__.writeInt(lFeerate);
			os__.writeString(lFee);
			os__.writeString(lMchamt);
			os__.writeString(sCashtime);
			os__.writeInt(lFeeNo);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			iNum = is__.readString();
			sChannel = is__.readString();
			sStime = is__.readString();
			sEtime = is__.readString();
			lAmt = is__.readString();
			lFeerate = is__.readInt();
			lFee = is__.readString();
			lMchamt = is__.readString();
			sCashtime = is__.readString();
			lFeeNo = is__.readInt();
		}

		#endregion
	}

	public class STRUBulletinDetail : _System.ICloneable
	{
		#region Slice data members

		public int iIsNew;

		public string sTitle;

		public string sUrl;

		public string sIssueTime;

		public string sModifyTime;

		#endregion

		#region Constructors

		public STRUBulletinDetail()
		{
		}

		public STRUBulletinDetail(int iIsNew, string sTitle, string sUrl, string sIssueTime, string sModifyTime)
		{
			this.iIsNew = iIsNew;
			this.sTitle = sTitle;
			this.sUrl = sUrl;
			this.sIssueTime = sIssueTime;
			this.sModifyTime = sModifyTime;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			h__ = 5 * h__ + iIsNew.GetHashCode();
			if(sTitle != null)
			{
				h__ = 5 * h__ + sTitle.GetHashCode();
			}
			if(sUrl != null)
			{
				h__ = 5 * h__ + sUrl.GetHashCode();
			}
			if(sIssueTime != null)
			{
				h__ = 5 * h__ + sIssueTime.GetHashCode();
			}
			if(sModifyTime != null)
			{
				h__ = 5 * h__ + sModifyTime.GetHashCode();
			}
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			STRUBulletinDetail o__ = (STRUBulletinDetail)other__;
			if(!iIsNew.Equals(o__.iIsNew))
			{
				return false;
			}
			if(sTitle == null)
			{
				if(o__.sTitle != null)
				{
					return false;
				}
			}
			else
			{
				if(!sTitle.Equals(o__.sTitle))
				{
					return false;
				}
			}
			if(sUrl == null)
			{
				if(o__.sUrl != null)
				{
					return false;
				}
			}
			else
			{
				if(!sUrl.Equals(o__.sUrl))
				{
					return false;
				}
			}
			if(sIssueTime == null)
			{
				if(o__.sIssueTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!sIssueTime.Equals(o__.sIssueTime))
				{
					return false;
				}
			}
			if(sModifyTime == null)
			{
				if(o__.sModifyTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!sModifyTime.Equals(o__.sModifyTime))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(STRUBulletinDetail lhs__, STRUBulletinDetail rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(STRUBulletinDetail lhs__, STRUBulletinDetail rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeInt(iIsNew);
			os__.writeString(sTitle);
			os__.writeString(sUrl);
			os__.writeString(sIssueTime);
			os__.writeString(sModifyTime);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			iIsNew = is__.readInt();
			sTitle = is__.readString();
			sUrl = is__.readString();
			sIssueTime = is__.readString();
			sModifyTime = is__.readString();
		}

		#endregion
	}

	public class STRUTranInfoEx : _System.ICloneable
	{
		#region Slice data members

		public string strTranTime;

		public string strTransactionID;

		public string strOrderNO;

		public int iBankType;

		public string strBankOrderNO;

		public string strOppQQID;

		public int iStatus;

		public int iFee;

		public string strProductDesc;

		public int iOrderFlag;

		public int refundStatus;

		public int retoBuyerFee;

		public int retoSellerFee;

		public string refundListid;

		public string refundApplyTime;

		public string refundTime;

		public int iCashFee;

		public int iToken;

		public int iVFee;

		public string strTicketId;

		#endregion

		#region Constructors

		public STRUTranInfoEx()
		{
		}

		public STRUTranInfoEx(string strTranTime, string strTransactionID, string strOrderNO, int iBankType, string strBankOrderNO, string strOppQQID, int iStatus, int iFee, string strProductDesc, int iOrderFlag, int refundStatus, int retoBuyerFee, int retoSellerFee, string refundListid, string refundApplyTime, string refundTime, int iCashFee, int iToken, int iVFee, string strTicketId)
		{
			this.strTranTime = strTranTime;
			this.strTransactionID = strTransactionID;
			this.strOrderNO = strOrderNO;
			this.iBankType = iBankType;
			this.strBankOrderNO = strBankOrderNO;
			this.strOppQQID = strOppQQID;
			this.iStatus = iStatus;
			this.iFee = iFee;
			this.strProductDesc = strProductDesc;
			this.iOrderFlag = iOrderFlag;
			this.refundStatus = refundStatus;
			this.retoBuyerFee = retoBuyerFee;
			this.retoSellerFee = retoSellerFee;
			this.refundListid = refundListid;
			this.refundApplyTime = refundApplyTime;
			this.refundTime = refundTime;
			this.iCashFee = iCashFee;
			this.iToken = iToken;
			this.iVFee = iVFee;
			this.strTicketId = strTicketId;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			if(strTranTime != null)
			{
				h__ = 5 * h__ + strTranTime.GetHashCode();
			}
			if(strTransactionID != null)
			{
				h__ = 5 * h__ + strTransactionID.GetHashCode();
			}
			if(strOrderNO != null)
			{
				h__ = 5 * h__ + strOrderNO.GetHashCode();
			}
			h__ = 5 * h__ + iBankType.GetHashCode();
			if(strBankOrderNO != null)
			{
				h__ = 5 * h__ + strBankOrderNO.GetHashCode();
			}
			if(strOppQQID != null)
			{
				h__ = 5 * h__ + strOppQQID.GetHashCode();
			}
			h__ = 5 * h__ + iStatus.GetHashCode();
			h__ = 5 * h__ + iFee.GetHashCode();
			if(strProductDesc != null)
			{
				h__ = 5 * h__ + strProductDesc.GetHashCode();
			}
			h__ = 5 * h__ + iOrderFlag.GetHashCode();
			h__ = 5 * h__ + refundStatus.GetHashCode();
			h__ = 5 * h__ + retoBuyerFee.GetHashCode();
			h__ = 5 * h__ + retoSellerFee.GetHashCode();
			if(refundListid != null)
			{
				h__ = 5 * h__ + refundListid.GetHashCode();
			}
			if(refundApplyTime != null)
			{
				h__ = 5 * h__ + refundApplyTime.GetHashCode();
			}
			if(refundTime != null)
			{
				h__ = 5 * h__ + refundTime.GetHashCode();
			}
			h__ = 5 * h__ + iCashFee.GetHashCode();
			h__ = 5 * h__ + iToken.GetHashCode();
			h__ = 5 * h__ + iVFee.GetHashCode();
			if(strTicketId != null)
			{
				h__ = 5 * h__ + strTicketId.GetHashCode();
			}
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			STRUTranInfoEx o__ = (STRUTranInfoEx)other__;
			if(strTranTime == null)
			{
				if(o__.strTranTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!strTranTime.Equals(o__.strTranTime))
				{
					return false;
				}
			}
			if(strTransactionID == null)
			{
				if(o__.strTransactionID != null)
				{
					return false;
				}
			}
			else
			{
				if(!strTransactionID.Equals(o__.strTransactionID))
				{
					return false;
				}
			}
			if(strOrderNO == null)
			{
				if(o__.strOrderNO != null)
				{
					return false;
				}
			}
			else
			{
				if(!strOrderNO.Equals(o__.strOrderNO))
				{
					return false;
				}
			}
			if(!iBankType.Equals(o__.iBankType))
			{
				return false;
			}
			if(strBankOrderNO == null)
			{
				if(o__.strBankOrderNO != null)
				{
					return false;
				}
			}
			else
			{
				if(!strBankOrderNO.Equals(o__.strBankOrderNO))
				{
					return false;
				}
			}
			if(strOppQQID == null)
			{
				if(o__.strOppQQID != null)
				{
					return false;
				}
			}
			else
			{
				if(!strOppQQID.Equals(o__.strOppQQID))
				{
					return false;
				}
			}
			if(!iStatus.Equals(o__.iStatus))
			{
				return false;
			}
			if(!iFee.Equals(o__.iFee))
			{
				return false;
			}
			if(strProductDesc == null)
			{
				if(o__.strProductDesc != null)
				{
					return false;
				}
			}
			else
			{
				if(!strProductDesc.Equals(o__.strProductDesc))
				{
					return false;
				}
			}
			if(!iOrderFlag.Equals(o__.iOrderFlag))
			{
				return false;
			}
			if(!refundStatus.Equals(o__.refundStatus))
			{
				return false;
			}
			if(!retoBuyerFee.Equals(o__.retoBuyerFee))
			{
				return false;
			}
			if(!retoSellerFee.Equals(o__.retoSellerFee))
			{
				return false;
			}
			if(refundListid == null)
			{
				if(o__.refundListid != null)
				{
					return false;
				}
			}
			else
			{
				if(!refundListid.Equals(o__.refundListid))
				{
					return false;
				}
			}
			if(refundApplyTime == null)
			{
				if(o__.refundApplyTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!refundApplyTime.Equals(o__.refundApplyTime))
				{
					return false;
				}
			}
			if(refundTime == null)
			{
				if(o__.refundTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!refundTime.Equals(o__.refundTime))
				{
					return false;
				}
			}
			if(!iCashFee.Equals(o__.iCashFee))
			{
				return false;
			}
			if(!iToken.Equals(o__.iToken))
			{
				return false;
			}
			if(!iVFee.Equals(o__.iVFee))
			{
				return false;
			}
			if(strTicketId == null)
			{
				if(o__.strTicketId != null)
				{
					return false;
				}
			}
			else
			{
				if(!strTicketId.Equals(o__.strTicketId))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(STRUTranInfoEx lhs__, STRUTranInfoEx rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(STRUTranInfoEx lhs__, STRUTranInfoEx rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeString(strTranTime);
			os__.writeString(strTransactionID);
			os__.writeString(strOrderNO);
			os__.writeInt(iBankType);
			os__.writeString(strBankOrderNO);
			os__.writeString(strOppQQID);
			os__.writeInt(iStatus);
			os__.writeInt(iFee);
			os__.writeString(strProductDesc);
			os__.writeInt(iOrderFlag);
			os__.writeInt(refundStatus);
			os__.writeInt(retoBuyerFee);
			os__.writeInt(retoSellerFee);
			os__.writeString(refundListid);
			os__.writeString(refundApplyTime);
			os__.writeString(refundTime);
			os__.writeInt(iCashFee);
			os__.writeInt(iToken);
			os__.writeInt(iVFee);
			os__.writeString(strTicketId);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			strTranTime = is__.readString();
			strTransactionID = is__.readString();
			strOrderNO = is__.readString();
			iBankType = is__.readInt();
			strBankOrderNO = is__.readString();
			strOppQQID = is__.readString();
			iStatus = is__.readInt();
			iFee = is__.readInt();
			strProductDesc = is__.readString();
			iOrderFlag = is__.readInt();
			refundStatus = is__.readInt();
			retoBuyerFee = is__.readInt();
			retoSellerFee = is__.readInt();
			refundListid = is__.readString();
			refundApplyTime = is__.readString();
			refundTime = is__.readString();
			iCashFee = is__.readInt();
			iToken = is__.readInt();
			iVFee = is__.readInt();
			strTicketId = is__.readString();
		}

		#endregion
	}

	public class AccRollRecord : _System.ICloneable
	{
		#region Slice data members

		public string listid;

		public int type;

		public int subject;

		public string paynum;

		public int bankType;

		public int sign;

		public int actionType;

		public string fromId;

		public string fromName;

		public string balance;

		public string memo;

		public string modifyTime;

		#endregion

		#region Constructors

		public AccRollRecord()
		{
		}

		public AccRollRecord(string listid, int type, int subject, string paynum, int bankType, int sign, int actionType, string fromId, string fromName, string balance, string memo, string modifyTime)
		{
			this.listid = listid;
			this.type = type;
			this.subject = subject;
			this.paynum = paynum;
			this.bankType = bankType;
			this.sign = sign;
			this.actionType = actionType;
			this.fromId = fromId;
			this.fromName = fromName;
			this.balance = balance;
			this.memo = memo;
			this.modifyTime = modifyTime;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			if(listid != null)
			{
				h__ = 5 * h__ + listid.GetHashCode();
			}
			h__ = 5 * h__ + type.GetHashCode();
			h__ = 5 * h__ + subject.GetHashCode();
			if(paynum != null)
			{
				h__ = 5 * h__ + paynum.GetHashCode();
			}
			h__ = 5 * h__ + bankType.GetHashCode();
			h__ = 5 * h__ + sign.GetHashCode();
			h__ = 5 * h__ + actionType.GetHashCode();
			if(fromId != null)
			{
				h__ = 5 * h__ + fromId.GetHashCode();
			}
			if(fromName != null)
			{
				h__ = 5 * h__ + fromName.GetHashCode();
			}
			if(balance != null)
			{
				h__ = 5 * h__ + balance.GetHashCode();
			}
			if(memo != null)
			{
				h__ = 5 * h__ + memo.GetHashCode();
			}
			if(modifyTime != null)
			{
				h__ = 5 * h__ + modifyTime.GetHashCode();
			}
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			AccRollRecord o__ = (AccRollRecord)other__;
			if(listid == null)
			{
				if(o__.listid != null)
				{
					return false;
				}
			}
			else
			{
				if(!listid.Equals(o__.listid))
				{
					return false;
				}
			}
			if(!type.Equals(o__.type))
			{
				return false;
			}
			if(!subject.Equals(o__.subject))
			{
				return false;
			}
			if(paynum == null)
			{
				if(o__.paynum != null)
				{
					return false;
				}
			}
			else
			{
				if(!paynum.Equals(o__.paynum))
				{
					return false;
				}
			}
			if(!bankType.Equals(o__.bankType))
			{
				return false;
			}
			if(!sign.Equals(o__.sign))
			{
				return false;
			}
			if(!actionType.Equals(o__.actionType))
			{
				return false;
			}
			if(fromId == null)
			{
				if(o__.fromId != null)
				{
					return false;
				}
			}
			else
			{
				if(!fromId.Equals(o__.fromId))
				{
					return false;
				}
			}
			if(fromName == null)
			{
				if(o__.fromName != null)
				{
					return false;
				}
			}
			else
			{
				if(!fromName.Equals(o__.fromName))
				{
					return false;
				}
			}
			if(balance == null)
			{
				if(o__.balance != null)
				{
					return false;
				}
			}
			else
			{
				if(!balance.Equals(o__.balance))
				{
					return false;
				}
			}
			if(memo == null)
			{
				if(o__.memo != null)
				{
					return false;
				}
			}
			else
			{
				if(!memo.Equals(o__.memo))
				{
					return false;
				}
			}
			if(modifyTime == null)
			{
				if(o__.modifyTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!modifyTime.Equals(o__.modifyTime))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(AccRollRecord lhs__, AccRollRecord rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(AccRollRecord lhs__, AccRollRecord rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeString(listid);
			os__.writeInt(type);
			os__.writeInt(subject);
			os__.writeString(paynum);
			os__.writeInt(bankType);
			os__.writeInt(sign);
			os__.writeInt(actionType);
			os__.writeString(fromId);
			os__.writeString(fromName);
			os__.writeString(balance);
			os__.writeString(memo);
			os__.writeString(modifyTime);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			
			System.Text.Encoding gb2312 = System.Text.Encoding.GetEncoding("gb2312");
			
			byte[] buffer = is__.readByteSeq();
			listid = gb2312.GetString(buffer);


			//listid = is__.readString();
			type = is__.readInt();
			subject = is__.readInt();
			paynum = is__.readString();
			bankType = is__.readInt();
			sign = is__.readInt();
			actionType = is__.readInt();

			buffer = is__.readByteSeq();
			fromId = gb2312.GetString(buffer);

			buffer = is__.readByteSeq();
			fromName = gb2312.GetString(buffer);

			buffer = is__.readByteSeq();
			balance = gb2312.GetString(buffer);

			buffer = is__.readByteSeq();
			memo = gb2312.GetString(buffer);

			buffer = is__.readByteSeq();
//			buffer = System.Text.Encoding.Convert(gbk,utf8,buffer);
			modifyTime = gb2312.GetString(buffer);

//			fromId = is__.readString();
//			fromName = is__.readString();
//			balance = is__.readString();
//			memo = is__.readString();
//			modifyTime = is__.readString();
			
		}

		#endregion
	}

	public class AccRollRecord2 : _System.ICloneable
	{
		#region Slice data members

		public string listid;

		public int type;

		public int subject;

		public string paynum;

		public int bankType;

		public int sign;

		public int actionType;

		public string fromId;

		public string fromName;

		public string balance;

		public string memo;

		public string explain;

		public string modifyTime;

		#endregion

		#region Constructors

		public AccRollRecord2()
		{
		}

		public AccRollRecord2(string listid, int type, int subject, string paynum, int bankType, int sign, int actionType, string fromId, string fromName, string balance, string memo, string explain, string modifyTime)
		{
			this.listid = listid;
			this.type = type;
			this.subject = subject;
			this.paynum = paynum;
			this.bankType = bankType;
			this.sign = sign;
			this.actionType = actionType;
			this.fromId = fromId;
			this.fromName = fromName;
			this.balance = balance;
			this.memo = memo;
			this.explain = explain;
			this.modifyTime = modifyTime;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			if(listid != null)
			{
				h__ = 5 * h__ + listid.GetHashCode();
			}
			h__ = 5 * h__ + type.GetHashCode();
			h__ = 5 * h__ + subject.GetHashCode();
			if(paynum != null)
			{
				h__ = 5 * h__ + paynum.GetHashCode();
			}
			h__ = 5 * h__ + bankType.GetHashCode();
			h__ = 5 * h__ + sign.GetHashCode();
			h__ = 5 * h__ + actionType.GetHashCode();
			if(fromId != null)
			{
				h__ = 5 * h__ + fromId.GetHashCode();
			}
			if(fromName != null)
			{
				h__ = 5 * h__ + fromName.GetHashCode();
			}
			if(balance != null)
			{
				h__ = 5 * h__ + balance.GetHashCode();
			}
			if(memo != null)
			{
				h__ = 5 * h__ + memo.GetHashCode();
			}
			if(explain != null)
			{
				h__ = 5 * h__ + explain.GetHashCode();
			}
			if(modifyTime != null)
			{
				h__ = 5 * h__ + modifyTime.GetHashCode();
			}
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			AccRollRecord2 o__ = (AccRollRecord2)other__;
			if(listid == null)
			{
				if(o__.listid != null)
				{
					return false;
				}
			}
			else
			{
				if(!listid.Equals(o__.listid))
				{
					return false;
				}
			}
			if(!type.Equals(o__.type))
			{
				return false;
			}
			if(!subject.Equals(o__.subject))
			{
				return false;
			}
			if(paynum == null)
			{
				if(o__.paynum != null)
				{
					return false;
				}
			}
			else
			{
				if(!paynum.Equals(o__.paynum))
				{
					return false;
				}
			}
			if(!bankType.Equals(o__.bankType))
			{
				return false;
			}
			if(!sign.Equals(o__.sign))
			{
				return false;
			}
			if(!actionType.Equals(o__.actionType))
			{
				return false;
			}
			if(fromId == null)
			{
				if(o__.fromId != null)
				{
					return false;
				}
			}
			else
			{
				if(!fromId.Equals(o__.fromId))
				{
					return false;
				}
			}
			if(fromName == null)
			{
				if(o__.fromName != null)
				{
					return false;
				}
			}
			else
			{
				if(!fromName.Equals(o__.fromName))
				{
					return false;
				}
			}
			if(balance == null)
			{
				if(o__.balance != null)
				{
					return false;
				}
			}
			else
			{
				if(!balance.Equals(o__.balance))
				{
					return false;
				}
			}
			if(memo == null)
			{
				if(o__.memo != null)
				{
					return false;
				}
			}
			else
			{
				if(!memo.Equals(o__.memo))
				{
					return false;
				}
			}
			if(explain == null)
			{
				if(o__.explain != null)
				{
					return false;
				}
			}
			else
			{
				if(!explain.Equals(o__.explain))
				{
					return false;
				}
			}
			if(modifyTime == null)
			{
				if(o__.modifyTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!modifyTime.Equals(o__.modifyTime))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(AccRollRecord2 lhs__, AccRollRecord2 rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(AccRollRecord2 lhs__, AccRollRecord2 rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeString(listid);
			os__.writeInt(type);
			os__.writeInt(subject);
			os__.writeString(paynum);
			os__.writeInt(bankType);
			os__.writeInt(sign);
			os__.writeInt(actionType);
			os__.writeString(fromId);
			os__.writeString(fromName);
			os__.writeString(balance);
			os__.writeString(memo);
			os__.writeString(explain);
			os__.writeString(modifyTime);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			listid = is__.readString();
			type = is__.readInt();
			subject = is__.readInt();
			paynum = is__.readString();
			bankType = is__.readInt();
			sign = is__.readInt();
			actionType = is__.readInt();
			fromId = is__.readString();
			fromName = is__.readString();
			balance = is__.readString();
			memo = is__.readString();
			explain = is__.readString();
			modifyTime = is__.readString();
		}

		#endregion
	}

	public class AccRollRecordEx : _System.ICloneable
	{
		#region Slice data members

		public string listid;

		public int type;

		public int subject;

		public string paynum;

		public int bankType;

		public int sign;

		public int actionType;

		public string fromId;

		public string fromName;

		public string balance;

		public string memo;

		public string modifyTime;

		public string explain;

		public string spBillno;

		public AMSManager.StrMap attrs;

		#endregion

		#region Constructors

		public AccRollRecordEx()
		{
		}

		public AccRollRecordEx(string listid, int type, int subject, string paynum, int bankType, int sign, int actionType, string fromId, string fromName, string balance, string memo, string modifyTime, string explain, string spBillno, AMSManager.StrMap attrs)
		{
			this.listid = listid;
			this.type = type;
			this.subject = subject;
			this.paynum = paynum;
			this.bankType = bankType;
			this.sign = sign;
			this.actionType = actionType;
			this.fromId = fromId;
			this.fromName = fromName;
			this.balance = balance;
			this.memo = memo;
			this.modifyTime = modifyTime;
			this.explain = explain;
			this.spBillno = spBillno;
			this.attrs = attrs;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			if(listid != null)
			{
				h__ = 5 * h__ + listid.GetHashCode();
			}
			h__ = 5 * h__ + type.GetHashCode();
			h__ = 5 * h__ + subject.GetHashCode();
			if(paynum != null)
			{
				h__ = 5 * h__ + paynum.GetHashCode();
			}
			h__ = 5 * h__ + bankType.GetHashCode();
			h__ = 5 * h__ + sign.GetHashCode();
			h__ = 5 * h__ + actionType.GetHashCode();
			if(fromId != null)
			{
				h__ = 5 * h__ + fromId.GetHashCode();
			}
			if(fromName != null)
			{
				h__ = 5 * h__ + fromName.GetHashCode();
			}
			if(balance != null)
			{
				h__ = 5 * h__ + balance.GetHashCode();
			}
			if(memo != null)
			{
				h__ = 5 * h__ + memo.GetHashCode();
			}
			if(modifyTime != null)
			{
				h__ = 5 * h__ + modifyTime.GetHashCode();
			}
			if(explain != null)
			{
				h__ = 5 * h__ + explain.GetHashCode();
			}
			if(spBillno != null)
			{
				h__ = 5 * h__ + spBillno.GetHashCode();
			}
			if(attrs != null)
			{
				h__ = 5 * h__ + attrs.GetHashCode();
			}
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			AccRollRecordEx o__ = (AccRollRecordEx)other__;
			if(listid == null)
			{
				if(o__.listid != null)
				{
					return false;
				}
			}
			else
			{
				if(!listid.Equals(o__.listid))
				{
					return false;
				}
			}
			if(!type.Equals(o__.type))
			{
				return false;
			}
			if(!subject.Equals(o__.subject))
			{
				return false;
			}
			if(paynum == null)
			{
				if(o__.paynum != null)
				{
					return false;
				}
			}
			else
			{
				if(!paynum.Equals(o__.paynum))
				{
					return false;
				}
			}
			if(!bankType.Equals(o__.bankType))
			{
				return false;
			}
			if(!sign.Equals(o__.sign))
			{
				return false;
			}
			if(!actionType.Equals(o__.actionType))
			{
				return false;
			}
			if(fromId == null)
			{
				if(o__.fromId != null)
				{
					return false;
				}
			}
			else
			{
				if(!fromId.Equals(o__.fromId))
				{
					return false;
				}
			}
			if(fromName == null)
			{
				if(o__.fromName != null)
				{
					return false;
				}
			}
			else
			{
				if(!fromName.Equals(o__.fromName))
				{
					return false;
				}
			}
			if(balance == null)
			{
				if(o__.balance != null)
				{
					return false;
				}
			}
			else
			{
				if(!balance.Equals(o__.balance))
				{
					return false;
				}
			}
			if(memo == null)
			{
				if(o__.memo != null)
				{
					return false;
				}
			}
			else
			{
				if(!memo.Equals(o__.memo))
				{
					return false;
				}
			}
			if(modifyTime == null)
			{
				if(o__.modifyTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!modifyTime.Equals(o__.modifyTime))
				{
					return false;
				}
			}
			if(explain == null)
			{
				if(o__.explain != null)
				{
					return false;
				}
			}
			else
			{
				if(!explain.Equals(o__.explain))
				{
					return false;
				}
			}
			if(spBillno == null)
			{
				if(o__.spBillno != null)
				{
					return false;
				}
			}
			else
			{
				if(!spBillno.Equals(o__.spBillno))
				{
					return false;
				}
			}
			if(attrs == null)
			{
				if(o__.attrs != null)
				{
					return false;
				}
			}
			else
			{
				if(!attrs.Equals(o__.attrs))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(AccRollRecordEx lhs__, AccRollRecordEx rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(AccRollRecordEx lhs__, AccRollRecordEx rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeString(listid);
			os__.writeInt(type);
			os__.writeInt(subject);
			os__.writeString(paynum);
			os__.writeInt(bankType);
			os__.writeInt(sign);
			os__.writeInt(actionType);
			os__.writeString(fromId);
			os__.writeString(fromName);
			os__.writeString(balance);
			os__.writeString(memo);
			os__.writeString(modifyTime);
			os__.writeString(explain);
			os__.writeString(spBillno);
			AMSManager.StrMapHelper.write(os__, attrs);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			listid = is__.readString();
			type = is__.readInt();
			subject = is__.readInt();
			paynum = is__.readString();
			bankType = is__.readInt();
			sign = is__.readInt();
			actionType = is__.readInt();
			fromId = is__.readString();
			fromName = is__.readString();
			balance = is__.readString();
			memo = is__.readString();
			modifyTime = is__.readString();
			explain = is__.readString();
			spBillno = is__.readString();
			attrs = AMSManager.StrMapHelper.read(is__);
		}

		#endregion
	}

	public class STRUSettlementDetailEx : _System.ICloneable
	{
		#region Slice data members

		public string iNum;

		public string sChannel;

		public string sStime;

		public string sEtime;

		public string lAmt;

		public int lFeerate;

		public string lFee;

		public string lMchamt;

		public string sCashtime;

		public int lFeeNo;

		public string agentId;

		#endregion

		#region Constructors

		public STRUSettlementDetailEx()
		{
		}

		public STRUSettlementDetailEx(string iNum, string sChannel, string sStime, string sEtime, string lAmt, int lFeerate, string lFee, string lMchamt, string sCashtime, int lFeeNo, string agentId)
		{
			this.iNum = iNum;
			this.sChannel = sChannel;
			this.sStime = sStime;
			this.sEtime = sEtime;
			this.lAmt = lAmt;
			this.lFeerate = lFeerate;
			this.lFee = lFee;
			this.lMchamt = lMchamt;
			this.sCashtime = sCashtime;
			this.lFeeNo = lFeeNo;
			this.agentId = agentId;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			if(iNum != null)
			{
				h__ = 5 * h__ + iNum.GetHashCode();
			}
			if(sChannel != null)
			{
				h__ = 5 * h__ + sChannel.GetHashCode();
			}
			if(sStime != null)
			{
				h__ = 5 * h__ + sStime.GetHashCode();
			}
			if(sEtime != null)
			{
				h__ = 5 * h__ + sEtime.GetHashCode();
			}
			if(lAmt != null)
			{
				h__ = 5 * h__ + lAmt.GetHashCode();
			}
			h__ = 5 * h__ + lFeerate.GetHashCode();
			if(lFee != null)
			{
				h__ = 5 * h__ + lFee.GetHashCode();
			}
			if(lMchamt != null)
			{
				h__ = 5 * h__ + lMchamt.GetHashCode();
			}
			if(sCashtime != null)
			{
				h__ = 5 * h__ + sCashtime.GetHashCode();
			}
			h__ = 5 * h__ + lFeeNo.GetHashCode();
			if(agentId != null)
			{
				h__ = 5 * h__ + agentId.GetHashCode();
			}
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			STRUSettlementDetailEx o__ = (STRUSettlementDetailEx)other__;
			if(iNum == null)
			{
				if(o__.iNum != null)
				{
					return false;
				}
			}
			else
			{
				if(!iNum.Equals(o__.iNum))
				{
					return false;
				}
			}
			if(sChannel == null)
			{
				if(o__.sChannel != null)
				{
					return false;
				}
			}
			else
			{
				if(!sChannel.Equals(o__.sChannel))
				{
					return false;
				}
			}
			if(sStime == null)
			{
				if(o__.sStime != null)
				{
					return false;
				}
			}
			else
			{
				if(!sStime.Equals(o__.sStime))
				{
					return false;
				}
			}
			if(sEtime == null)
			{
				if(o__.sEtime != null)
				{
					return false;
				}
			}
			else
			{
				if(!sEtime.Equals(o__.sEtime))
				{
					return false;
				}
			}
			if(lAmt == null)
			{
				if(o__.lAmt != null)
				{
					return false;
				}
			}
			else
			{
				if(!lAmt.Equals(o__.lAmt))
				{
					return false;
				}
			}
			if(!lFeerate.Equals(o__.lFeerate))
			{
				return false;
			}
			if(lFee == null)
			{
				if(o__.lFee != null)
				{
					return false;
				}
			}
			else
			{
				if(!lFee.Equals(o__.lFee))
				{
					return false;
				}
			}
			if(lMchamt == null)
			{
				if(o__.lMchamt != null)
				{
					return false;
				}
			}
			else
			{
				if(!lMchamt.Equals(o__.lMchamt))
				{
					return false;
				}
			}
			if(sCashtime == null)
			{
				if(o__.sCashtime != null)
				{
					return false;
				}
			}
			else
			{
				if(!sCashtime.Equals(o__.sCashtime))
				{
					return false;
				}
			}
			if(!lFeeNo.Equals(o__.lFeeNo))
			{
				return false;
			}
			if(agentId == null)
			{
				if(o__.agentId != null)
				{
					return false;
				}
			}
			else
			{
				if(!agentId.Equals(o__.agentId))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(STRUSettlementDetailEx lhs__, STRUSettlementDetailEx rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(STRUSettlementDetailEx lhs__, STRUSettlementDetailEx rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeString(iNum);
			os__.writeString(sChannel);
			os__.writeString(sStime);
			os__.writeString(sEtime);
			os__.writeString(lAmt);
			os__.writeInt(lFeerate);
			os__.writeString(lFee);
			os__.writeString(lMchamt);
			os__.writeString(sCashtime);
			os__.writeInt(lFeeNo);
			os__.writeString(agentId);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			iNum = is__.readString();
			sChannel = is__.readString();
			sStime = is__.readString();
			sEtime = is__.readString();
			lAmt = is__.readString();
			lFeerate = is__.readInt();
			lFee = is__.readString();
			lMchamt = is__.readString();
			sCashtime = is__.readString();
			lFeeNo = is__.readInt();
			agentId = is__.readString();
		}

		#endregion
	}

	public class STRAirOrderinfo : _System.ICloneable
	{
		#region Slice data members

		public string strListid;

		public string strSpBillno;

		public int iTotalFee;

		public int iSttleFee;

		public int iSettleBackFee;

		public int iRefundFee;

		public string strPayTime;

		public int iOrderBalance;

		public long lAdjustInFee;

		public long lAdjustOutFee;

		#endregion

		#region Constructors

		public STRAirOrderinfo()
		{
		}

		public STRAirOrderinfo(string strListid, string strSpBillno, int iTotalFee, int iSttleFee, int iSettleBackFee, int iRefundFee, string strPayTime, int iOrderBalance, long lAdjustInFee, long lAdjustOutFee)
		{
			this.strListid = strListid;
			this.strSpBillno = strSpBillno;
			this.iTotalFee = iTotalFee;
			this.iSttleFee = iSttleFee;
			this.iSettleBackFee = iSettleBackFee;
			this.iRefundFee = iRefundFee;
			this.strPayTime = strPayTime;
			this.iOrderBalance = iOrderBalance;
			this.lAdjustInFee = lAdjustInFee;
			this.lAdjustOutFee = lAdjustOutFee;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			if(strListid != null)
			{
				h__ = 5 * h__ + strListid.GetHashCode();
			}
			if(strSpBillno != null)
			{
				h__ = 5 * h__ + strSpBillno.GetHashCode();
			}
			h__ = 5 * h__ + iTotalFee.GetHashCode();
			h__ = 5 * h__ + iSttleFee.GetHashCode();
			h__ = 5 * h__ + iSettleBackFee.GetHashCode();
			h__ = 5 * h__ + iRefundFee.GetHashCode();
			if(strPayTime != null)
			{
				h__ = 5 * h__ + strPayTime.GetHashCode();
			}
			h__ = 5 * h__ + iOrderBalance.GetHashCode();
			h__ = 5 * h__ + lAdjustInFee.GetHashCode();
			h__ = 5 * h__ + lAdjustOutFee.GetHashCode();
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			STRAirOrderinfo o__ = (STRAirOrderinfo)other__;
			if(strListid == null)
			{
				if(o__.strListid != null)
				{
					return false;
				}
			}
			else
			{
				if(!strListid.Equals(o__.strListid))
				{
					return false;
				}
			}
			if(strSpBillno == null)
			{
				if(o__.strSpBillno != null)
				{
					return false;
				}
			}
			else
			{
				if(!strSpBillno.Equals(o__.strSpBillno))
				{
					return false;
				}
			}
			if(!iTotalFee.Equals(o__.iTotalFee))
			{
				return false;
			}
			if(!iSttleFee.Equals(o__.iSttleFee))
			{
				return false;
			}
			if(!iSettleBackFee.Equals(o__.iSettleBackFee))
			{
				return false;
			}
			if(!iRefundFee.Equals(o__.iRefundFee))
			{
				return false;
			}
			if(strPayTime == null)
			{
				if(o__.strPayTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!strPayTime.Equals(o__.strPayTime))
				{
					return false;
				}
			}
			if(!iOrderBalance.Equals(o__.iOrderBalance))
			{
				return false;
			}
			if(!lAdjustInFee.Equals(o__.lAdjustInFee))
			{
				return false;
			}
			if(!lAdjustOutFee.Equals(o__.lAdjustOutFee))
			{
				return false;
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(STRAirOrderinfo lhs__, STRAirOrderinfo rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(STRAirOrderinfo lhs__, STRAirOrderinfo rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeString(strListid);
			os__.writeString(strSpBillno);
			os__.writeInt(iTotalFee);
			os__.writeInt(iSttleFee);
			os__.writeInt(iSettleBackFee);
			os__.writeInt(iRefundFee);
			os__.writeString(strPayTime);
			os__.writeInt(iOrderBalance);
			os__.writeLong(lAdjustInFee);
			os__.writeLong(lAdjustOutFee);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			strListid = is__.readString();
			strSpBillno = is__.readString();
			iTotalFee = is__.readInt();
			iSttleFee = is__.readInt();
			iSettleBackFee = is__.readInt();
			iRefundFee = is__.readInt();
			strPayTime = is__.readString();
			iOrderBalance = is__.readInt();
			lAdjustInFee = is__.readLong();
			lAdjustOutFee = is__.readLong();
		}

		#endregion
	}

	public class STRUDrawDetail : _System.ICloneable
	{
		#region Slice data members

		public int iNum;

		public string sCashtime;

		public string lMchamt;

		public string lAmt;

		public string lFee;

		public int iT;

		#endregion

		#region Constructors

		public STRUDrawDetail()
		{
		}

		public STRUDrawDetail(int iNum, string sCashtime, string lMchamt, string lAmt, string lFee, int iT)
		{
			this.iNum = iNum;
			this.sCashtime = sCashtime;
			this.lMchamt = lMchamt;
			this.lAmt = lAmt;
			this.lFee = lFee;
			this.iT = iT;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			h__ = 5 * h__ + iNum.GetHashCode();
			if(sCashtime != null)
			{
				h__ = 5 * h__ + sCashtime.GetHashCode();
			}
			if(lMchamt != null)
			{
				h__ = 5 * h__ + lMchamt.GetHashCode();
			}
			if(lAmt != null)
			{
				h__ = 5 * h__ + lAmt.GetHashCode();
			}
			if(lFee != null)
			{
				h__ = 5 * h__ + lFee.GetHashCode();
			}
			h__ = 5 * h__ + iT.GetHashCode();
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			STRUDrawDetail o__ = (STRUDrawDetail)other__;
			if(!iNum.Equals(o__.iNum))
			{
				return false;
			}
			if(sCashtime == null)
			{
				if(o__.sCashtime != null)
				{
					return false;
				}
			}
			else
			{
				if(!sCashtime.Equals(o__.sCashtime))
				{
					return false;
				}
			}
			if(lMchamt == null)
			{
				if(o__.lMchamt != null)
				{
					return false;
				}
			}
			else
			{
				if(!lMchamt.Equals(o__.lMchamt))
				{
					return false;
				}
			}
			if(lAmt == null)
			{
				if(o__.lAmt != null)
				{
					return false;
				}
			}
			else
			{
				if(!lAmt.Equals(o__.lAmt))
				{
					return false;
				}
			}
			if(lFee == null)
			{
				if(o__.lFee != null)
				{
					return false;
				}
			}
			else
			{
				if(!lFee.Equals(o__.lFee))
				{
					return false;
				}
			}
			if(!iT.Equals(o__.iT))
			{
				return false;
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(STRUDrawDetail lhs__, STRUDrawDetail rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(STRUDrawDetail lhs__, STRUDrawDetail rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeInt(iNum);
			os__.writeString(sCashtime);
			os__.writeString(lMchamt);
			os__.writeString(lAmt);
			os__.writeString(lFee);
			os__.writeInt(iT);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			iNum = is__.readInt();
			sCashtime = is__.readString();
			lMchamt = is__.readString();
			lAmt = is__.readString();
			lFee = is__.readString();
			iT = is__.readInt();
		}

		#endregion
	}

	public class STRUSettlementDetailAgent : _System.ICloneable
	{
		#region Slice data members

		public int iNum;

		public string sChannel;

		public string sFeeItem;

		public string sStime;

		public string sEtime;

		public int lCount;

		public string lAmt;

		public string sFeeRate;

		public string lMchamt;

		public string sProductName;

		public int lProductNum;

		public string sAgentSpid;

		#endregion

		#region Constructors

		public STRUSettlementDetailAgent()
		{
		}

		public STRUSettlementDetailAgent(int iNum, string sChannel, string sFeeItem, string sStime, string sEtime, int lCount, string lAmt, string sFeeRate, string lMchamt, string sProductName, int lProductNum, string sAgentSpid)
		{
			this.iNum = iNum;
			this.sChannel = sChannel;
			this.sFeeItem = sFeeItem;
			this.sStime = sStime;
			this.sEtime = sEtime;
			this.lCount = lCount;
			this.lAmt = lAmt;
			this.sFeeRate = sFeeRate;
			this.lMchamt = lMchamt;
			this.sProductName = sProductName;
			this.lProductNum = lProductNum;
			this.sAgentSpid = sAgentSpid;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			h__ = 5 * h__ + iNum.GetHashCode();
			if(sChannel != null)
			{
				h__ = 5 * h__ + sChannel.GetHashCode();
			}
			if(sFeeItem != null)
			{
				h__ = 5 * h__ + sFeeItem.GetHashCode();
			}
			if(sStime != null)
			{
				h__ = 5 * h__ + sStime.GetHashCode();
			}
			if(sEtime != null)
			{
				h__ = 5 * h__ + sEtime.GetHashCode();
			}
			h__ = 5 * h__ + lCount.GetHashCode();
			if(lAmt != null)
			{
				h__ = 5 * h__ + lAmt.GetHashCode();
			}
			if(sFeeRate != null)
			{
				h__ = 5 * h__ + sFeeRate.GetHashCode();
			}
			if(lMchamt != null)
			{
				h__ = 5 * h__ + lMchamt.GetHashCode();
			}
			if(sProductName != null)
			{
				h__ = 5 * h__ + sProductName.GetHashCode();
			}
			h__ = 5 * h__ + lProductNum.GetHashCode();
			if(sAgentSpid != null)
			{
				h__ = 5 * h__ + sAgentSpid.GetHashCode();
			}
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			STRUSettlementDetailAgent o__ = (STRUSettlementDetailAgent)other__;
			if(!iNum.Equals(o__.iNum))
			{
				return false;
			}
			if(sChannel == null)
			{
				if(o__.sChannel != null)
				{
					return false;
				}
			}
			else
			{
				if(!sChannel.Equals(o__.sChannel))
				{
					return false;
				}
			}
			if(sFeeItem == null)
			{
				if(o__.sFeeItem != null)
				{
					return false;
				}
			}
			else
			{
				if(!sFeeItem.Equals(o__.sFeeItem))
				{
					return false;
				}
			}
			if(sStime == null)
			{
				if(o__.sStime != null)
				{
					return false;
				}
			}
			else
			{
				if(!sStime.Equals(o__.sStime))
				{
					return false;
				}
			}
			if(sEtime == null)
			{
				if(o__.sEtime != null)
				{
					return false;
				}
			}
			else
			{
				if(!sEtime.Equals(o__.sEtime))
				{
					return false;
				}
			}
			if(!lCount.Equals(o__.lCount))
			{
				return false;
			}
			if(lAmt == null)
			{
				if(o__.lAmt != null)
				{
					return false;
				}
			}
			else
			{
				if(!lAmt.Equals(o__.lAmt))
				{
					return false;
				}
			}
			if(sFeeRate == null)
			{
				if(o__.sFeeRate != null)
				{
					return false;
				}
			}
			else
			{
				if(!sFeeRate.Equals(o__.sFeeRate))
				{
					return false;
				}
			}
			if(lMchamt == null)
			{
				if(o__.lMchamt != null)
				{
					return false;
				}
			}
			else
			{
				if(!lMchamt.Equals(o__.lMchamt))
				{
					return false;
				}
			}
			if(sProductName == null)
			{
				if(o__.sProductName != null)
				{
					return false;
				}
			}
			else
			{
				if(!sProductName.Equals(o__.sProductName))
				{
					return false;
				}
			}
			if(!lProductNum.Equals(o__.lProductNum))
			{
				return false;
			}
			if(sAgentSpid == null)
			{
				if(o__.sAgentSpid != null)
				{
					return false;
				}
			}
			else
			{
				if(!sAgentSpid.Equals(o__.sAgentSpid))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(STRUSettlementDetailAgent lhs__, STRUSettlementDetailAgent rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(STRUSettlementDetailAgent lhs__, STRUSettlementDetailAgent rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeInt(iNum);
			os__.writeString(sChannel);
			os__.writeString(sFeeItem);
			os__.writeString(sStime);
			os__.writeString(sEtime);
			os__.writeInt(lCount);
			os__.writeString(lAmt);
			os__.writeString(sFeeRate);
			os__.writeString(lMchamt);
			os__.writeString(sProductName);
			os__.writeInt(lProductNum);
			os__.writeString(sAgentSpid);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			iNum = is__.readInt();
			sChannel = is__.readString();
			sFeeItem = is__.readString();
			sStime = is__.readString();
			sEtime = is__.readString();
			lCount = is__.readInt();
			lAmt = is__.readString();
			sFeeRate = is__.readString();
			lMchamt = is__.readString();
			sProductName = is__.readString();
			lProductNum = is__.readInt();
			sAgentSpid = is__.readString();
		}

		#endregion
	}

	public class STRUUserOrder : _System.ICloneable
	{
		#region Slice data members

		public string sTransactionId;

		public string sMerchantListId;

		public string sTransTime;

		public string sModifyTime;

		public int lTransType;

		public string sPeerQqid;

		public int lAmount;

		public string sTicketId;

		public int lTransStatus;

		public string sTransInfo;

		#endregion

		#region Constructors

		public STRUUserOrder()
		{
		}

		public STRUUserOrder(string sTransactionId, string sMerchantListId, string sTransTime, string sModifyTime, int lTransType, string sPeerQqid, int lAmount, string sTicketId, int lTransStatus, string sTransInfo)
		{
			this.sTransactionId = sTransactionId;
			this.sMerchantListId = sMerchantListId;
			this.sTransTime = sTransTime;
			this.sModifyTime = sModifyTime;
			this.lTransType = lTransType;
			this.sPeerQqid = sPeerQqid;
			this.lAmount = lAmount;
			this.sTicketId = sTicketId;
			this.lTransStatus = lTransStatus;
			this.sTransInfo = sTransInfo;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			if(sTransactionId != null)
			{
				h__ = 5 * h__ + sTransactionId.GetHashCode();
			}
			if(sMerchantListId != null)
			{
				h__ = 5 * h__ + sMerchantListId.GetHashCode();
			}
			if(sTransTime != null)
			{
				h__ = 5 * h__ + sTransTime.GetHashCode();
			}
			if(sModifyTime != null)
			{
				h__ = 5 * h__ + sModifyTime.GetHashCode();
			}
			h__ = 5 * h__ + lTransType.GetHashCode();
			if(sPeerQqid != null)
			{
				h__ = 5 * h__ + sPeerQqid.GetHashCode();
			}
			h__ = 5 * h__ + lAmount.GetHashCode();
			if(sTicketId != null)
			{
				h__ = 5 * h__ + sTicketId.GetHashCode();
			}
			h__ = 5 * h__ + lTransStatus.GetHashCode();
			if(sTransInfo != null)
			{
				h__ = 5 * h__ + sTransInfo.GetHashCode();
			}
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			STRUUserOrder o__ = (STRUUserOrder)other__;
			if(sTransactionId == null)
			{
				if(o__.sTransactionId != null)
				{
					return false;
				}
			}
			else
			{
				if(!sTransactionId.Equals(o__.sTransactionId))
				{
					return false;
				}
			}
			if(sMerchantListId == null)
			{
				if(o__.sMerchantListId != null)
				{
					return false;
				}
			}
			else
			{
				if(!sMerchantListId.Equals(o__.sMerchantListId))
				{
					return false;
				}
			}
			if(sTransTime == null)
			{
				if(o__.sTransTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!sTransTime.Equals(o__.sTransTime))
				{
					return false;
				}
			}
			if(sModifyTime == null)
			{
				if(o__.sModifyTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!sModifyTime.Equals(o__.sModifyTime))
				{
					return false;
				}
			}
			if(!lTransType.Equals(o__.lTransType))
			{
				return false;
			}
			if(sPeerQqid == null)
			{
				if(o__.sPeerQqid != null)
				{
					return false;
				}
			}
			else
			{
				if(!sPeerQqid.Equals(o__.sPeerQqid))
				{
					return false;
				}
			}
			if(!lAmount.Equals(o__.lAmount))
			{
				return false;
			}
			if(sTicketId == null)
			{
				if(o__.sTicketId != null)
				{
					return false;
				}
			}
			else
			{
				if(!sTicketId.Equals(o__.sTicketId))
				{
					return false;
				}
			}
			if(!lTransStatus.Equals(o__.lTransStatus))
			{
				return false;
			}
			if(sTransInfo == null)
			{
				if(o__.sTransInfo != null)
				{
					return false;
				}
			}
			else
			{
				if(!sTransInfo.Equals(o__.sTransInfo))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(STRUUserOrder lhs__, STRUUserOrder rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(STRUUserOrder lhs__, STRUUserOrder rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeString(sTransactionId);
			os__.writeString(sMerchantListId);
			os__.writeString(sTransTime);
			os__.writeString(sModifyTime);
			os__.writeInt(lTransType);
			os__.writeString(sPeerQqid);
			os__.writeInt(lAmount);
			os__.writeString(sTicketId);
			os__.writeInt(lTransStatus);
			os__.writeString(sTransInfo);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			sTransactionId = is__.readString();
			sMerchantListId = is__.readString();
			sTransTime = is__.readString();
			sModifyTime = is__.readString();
			lTransType = is__.readInt();
			sPeerQqid = is__.readString();
			lAmount = is__.readInt();
			sTicketId = is__.readString();
			lTransStatus = is__.readInt();
			sTransInfo = is__.readString();
		}

		#endregion
	}

	public class STRUNewUserOrder : _System.ICloneable
	{
		#region Slice data members

		public int lUserType;

		public string sTransactionId;

		public string sMerchantListId;

		public string sTransTime;

		public string sModifyTime;

		public int lTransType;

		public string sPeerQqid;

		public int lAmount;

		public string sTicketId;

		public int lTransStatus;

		public string sTransInfo;

		public int lPayType;

		public int lMediSign;

		#endregion

		#region Constructors

		public STRUNewUserOrder()
		{
		}

		public STRUNewUserOrder(int lUserType, string sTransactionId, string sMerchantListId, string sTransTime, string sModifyTime, int lTransType, string sPeerQqid, int lAmount, string sTicketId, int lTransStatus, string sTransInfo, int lPayType, int lMediSign)
		{
			this.lUserType = lUserType;
			this.sTransactionId = sTransactionId;
			this.sMerchantListId = sMerchantListId;
			this.sTransTime = sTransTime;
			this.sModifyTime = sModifyTime;
			this.lTransType = lTransType;
			this.sPeerQqid = sPeerQqid;
			this.lAmount = lAmount;
			this.sTicketId = sTicketId;
			this.lTransStatus = lTransStatus;
			this.sTransInfo = sTransInfo;
			this.lPayType = lPayType;
			this.lMediSign = lMediSign;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			h__ = 5 * h__ + lUserType.GetHashCode();
			if(sTransactionId != null)
			{
				h__ = 5 * h__ + sTransactionId.GetHashCode();
			}
			if(sMerchantListId != null)
			{
				h__ = 5 * h__ + sMerchantListId.GetHashCode();
			}
			if(sTransTime != null)
			{
				h__ = 5 * h__ + sTransTime.GetHashCode();
			}
			if(sModifyTime != null)
			{
				h__ = 5 * h__ + sModifyTime.GetHashCode();
			}
			h__ = 5 * h__ + lTransType.GetHashCode();
			if(sPeerQqid != null)
			{
				h__ = 5 * h__ + sPeerQqid.GetHashCode();
			}
			h__ = 5 * h__ + lAmount.GetHashCode();
			if(sTicketId != null)
			{
				h__ = 5 * h__ + sTicketId.GetHashCode();
			}
			h__ = 5 * h__ + lTransStatus.GetHashCode();
			if(sTransInfo != null)
			{
				h__ = 5 * h__ + sTransInfo.GetHashCode();
			}
			h__ = 5 * h__ + lPayType.GetHashCode();
			h__ = 5 * h__ + lMediSign.GetHashCode();
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			STRUNewUserOrder o__ = (STRUNewUserOrder)other__;
			if(!lUserType.Equals(o__.lUserType))
			{
				return false;
			}
			if(sTransactionId == null)
			{
				if(o__.sTransactionId != null)
				{
					return false;
				}
			}
			else
			{
				if(!sTransactionId.Equals(o__.sTransactionId))
				{
					return false;
				}
			}
			if(sMerchantListId == null)
			{
				if(o__.sMerchantListId != null)
				{
					return false;
				}
			}
			else
			{
				if(!sMerchantListId.Equals(o__.sMerchantListId))
				{
					return false;
				}
			}
			if(sTransTime == null)
			{
				if(o__.sTransTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!sTransTime.Equals(o__.sTransTime))
				{
					return false;
				}
			}
			if(sModifyTime == null)
			{
				if(o__.sModifyTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!sModifyTime.Equals(o__.sModifyTime))
				{
					return false;
				}
			}
			if(!lTransType.Equals(o__.lTransType))
			{
				return false;
			}
			if(sPeerQqid == null)
			{
				if(o__.sPeerQqid != null)
				{
					return false;
				}
			}
			else
			{
				if(!sPeerQqid.Equals(o__.sPeerQqid))
				{
					return false;
				}
			}
			if(!lAmount.Equals(o__.lAmount))
			{
				return false;
			}
			if(sTicketId == null)
			{
				if(o__.sTicketId != null)
				{
					return false;
				}
			}
			else
			{
				if(!sTicketId.Equals(o__.sTicketId))
				{
					return false;
				}
			}
			if(!lTransStatus.Equals(o__.lTransStatus))
			{
				return false;
			}
			if(sTransInfo == null)
			{
				if(o__.sTransInfo != null)
				{
					return false;
				}
			}
			else
			{
				if(!sTransInfo.Equals(o__.sTransInfo))
				{
					return false;
				}
			}
			if(!lPayType.Equals(o__.lPayType))
			{
				return false;
			}
			if(!lMediSign.Equals(o__.lMediSign))
			{
				return false;
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(STRUNewUserOrder lhs__, STRUNewUserOrder rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(STRUNewUserOrder lhs__, STRUNewUserOrder rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeInt(lUserType);
			os__.writeString(sTransactionId);
			os__.writeString(sMerchantListId);
			os__.writeString(sTransTime);
			os__.writeString(sModifyTime);
			os__.writeInt(lTransType);
			os__.writeString(sPeerQqid);
			os__.writeInt(lAmount);
			os__.writeString(sTicketId);
			os__.writeInt(lTransStatus);
			os__.writeString(sTransInfo);
			os__.writeInt(lPayType);
			os__.writeInt(lMediSign);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			lUserType = is__.readInt();
			sTransactionId = is__.readString();
			sMerchantListId = is__.readString();
			sTransTime = is__.readString();
			sModifyTime = is__.readString();
			lTransType = is__.readInt();
			sPeerQqid = is__.readString();
			lAmount = is__.readInt();
			sTicketId = is__.readString();
			lTransStatus = is__.readInt();
			sTransInfo = is__.readString();
			lPayType = is__.readInt();
			lMediSign = is__.readInt();
		}

		#endregion
	}

	public class STRUAirTicket : _System.ICloneable
	{
		#region Slice data members

		public string sTransactionId;

		public string sMerchantListId;

		public string sPNR;

		public string sTransTime;

		public string sModifyTime;

		public int lTicketNum;

		public int lReTicketNum;

		public int lTransAmount;

		public int lInAmount;

		public int lTradeStatus;

		public string sAirPlaneInfo;

		public string sMerchantId;

		#endregion

		#region Constructors

		public STRUAirTicket()
		{
		}

		public STRUAirTicket(string sTransactionId, string sMerchantListId, string sPNR, string sTransTime, string sModifyTime, int lTicketNum, int lReTicketNum, int lTransAmount, int lInAmount, int lTradeStatus, string sAirPlaneInfo, string sMerchantId)
		{
			this.sTransactionId = sTransactionId;
			this.sMerchantListId = sMerchantListId;
			this.sPNR = sPNR;
			this.sTransTime = sTransTime;
			this.sModifyTime = sModifyTime;
			this.lTicketNum = lTicketNum;
			this.lReTicketNum = lReTicketNum;
			this.lTransAmount = lTransAmount;
			this.lInAmount = lInAmount;
			this.lTradeStatus = lTradeStatus;
			this.sAirPlaneInfo = sAirPlaneInfo;
			this.sMerchantId = sMerchantId;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			if(sTransactionId != null)
			{
				h__ = 5 * h__ + sTransactionId.GetHashCode();
			}
			if(sMerchantListId != null)
			{
				h__ = 5 * h__ + sMerchantListId.GetHashCode();
			}
			if(sPNR != null)
			{
				h__ = 5 * h__ + sPNR.GetHashCode();
			}
			if(sTransTime != null)
			{
				h__ = 5 * h__ + sTransTime.GetHashCode();
			}
			if(sModifyTime != null)
			{
				h__ = 5 * h__ + sModifyTime.GetHashCode();
			}
			h__ = 5 * h__ + lTicketNum.GetHashCode();
			h__ = 5 * h__ + lReTicketNum.GetHashCode();
			h__ = 5 * h__ + lTransAmount.GetHashCode();
			h__ = 5 * h__ + lInAmount.GetHashCode();
			h__ = 5 * h__ + lTradeStatus.GetHashCode();
			if(sAirPlaneInfo != null)
			{
				h__ = 5 * h__ + sAirPlaneInfo.GetHashCode();
			}
			if(sMerchantId != null)
			{
				h__ = 5 * h__ + sMerchantId.GetHashCode();
			}
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			STRUAirTicket o__ = (STRUAirTicket)other__;
			if(sTransactionId == null)
			{
				if(o__.sTransactionId != null)
				{
					return false;
				}
			}
			else
			{
				if(!sTransactionId.Equals(o__.sTransactionId))
				{
					return false;
				}
			}
			if(sMerchantListId == null)
			{
				if(o__.sMerchantListId != null)
				{
					return false;
				}
			}
			else
			{
				if(!sMerchantListId.Equals(o__.sMerchantListId))
				{
					return false;
				}
			}
			if(sPNR == null)
			{
				if(o__.sPNR != null)
				{
					return false;
				}
			}
			else
			{
				if(!sPNR.Equals(o__.sPNR))
				{
					return false;
				}
			}
			if(sTransTime == null)
			{
				if(o__.sTransTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!sTransTime.Equals(o__.sTransTime))
				{
					return false;
				}
			}
			if(sModifyTime == null)
			{
				if(o__.sModifyTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!sModifyTime.Equals(o__.sModifyTime))
				{
					return false;
				}
			}
			if(!lTicketNum.Equals(o__.lTicketNum))
			{
				return false;
			}
			if(!lReTicketNum.Equals(o__.lReTicketNum))
			{
				return false;
			}
			if(!lTransAmount.Equals(o__.lTransAmount))
			{
				return false;
			}
			if(!lInAmount.Equals(o__.lInAmount))
			{
				return false;
			}
			if(!lTradeStatus.Equals(o__.lTradeStatus))
			{
				return false;
			}
			if(sAirPlaneInfo == null)
			{
				if(o__.sAirPlaneInfo != null)
				{
					return false;
				}
			}
			else
			{
				if(!sAirPlaneInfo.Equals(o__.sAirPlaneInfo))
				{
					return false;
				}
			}
			if(sMerchantId == null)
			{
				if(o__.sMerchantId != null)
				{
					return false;
				}
			}
			else
			{
				if(!sMerchantId.Equals(o__.sMerchantId))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(STRUAirTicket lhs__, STRUAirTicket rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(STRUAirTicket lhs__, STRUAirTicket rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeString(sTransactionId);
			os__.writeString(sMerchantListId);
			os__.writeString(sPNR);
			os__.writeString(sTransTime);
			os__.writeString(sModifyTime);
			os__.writeInt(lTicketNum);
			os__.writeInt(lReTicketNum);
			os__.writeInt(lTransAmount);
			os__.writeInt(lInAmount);
			os__.writeInt(lTradeStatus);
			os__.writeString(sAirPlaneInfo);
			os__.writeString(sMerchantId);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			sTransactionId = is__.readString();
			sMerchantListId = is__.readString();
			sPNR = is__.readString();
			sTransTime = is__.readString();
			sModifyTime = is__.readString();
			lTicketNum = is__.readInt();
			lReTicketNum = is__.readInt();
			lTransAmount = is__.readInt();
			lInAmount = is__.readInt();
			lTradeStatus = is__.readInt();
			sAirPlaneInfo = is__.readString();
			sMerchantId = is__.readString();
		}

		#endregion
	}

	public class CompareCond : _System.ICloneable
	{
		#region Slice data members

		public string keyName;

		public string compareVal;

		public string op;

		#endregion

		#region Constructors

		public CompareCond()
		{
		}

		public CompareCond(string keyName, string compareVal, string op)
		{
			this.keyName = keyName;
			this.compareVal = compareVal;
			this.op = op;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			if(keyName != null)
			{
				h__ = 5 * h__ + keyName.GetHashCode();
			}
			if(compareVal != null)
			{
				h__ = 5 * h__ + compareVal.GetHashCode();
			}
			if(op != null)
			{
				h__ = 5 * h__ + op.GetHashCode();
			}
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			CompareCond o__ = (CompareCond)other__;
			if(keyName == null)
			{
				if(o__.keyName != null)
				{
					return false;
				}
			}
			else
			{
				if(!keyName.Equals(o__.keyName))
				{
					return false;
				}
			}
			if(compareVal == null)
			{
				if(o__.compareVal != null)
				{
					return false;
				}
			}
			else
			{
				if(!compareVal.Equals(o__.compareVal))
				{
					return false;
				}
			}
			if(op == null)
			{
				if(o__.op != null)
				{
					return false;
				}
			}
			else
			{
				if(!op.Equals(o__.op))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(CompareCond lhs__, CompareCond rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(CompareCond lhs__, CompareCond rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeString(keyName);
			os__.writeString(compareVal);
			os__.writeString(op);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			keyName = is__.readString();
			compareVal = is__.readString();
			op = is__.readString();
		}

		#endregion
	}

	public class STRUTranInfo2 : _System.ICloneable
	{
		#region Slice data members

		public string strCreateTime;

		public string strModifyTime;

		public string strTransactionID;

		public string strOrderNO;

		public int iBankType;

		public string strBankOrderNO;

		public string strOppQQID;

		public int iStatus;

		public int iFee;

		public string strProductDesc;

		public int iOrderFlag;

		#endregion

		#region Constructors

		public STRUTranInfo2()
		{
		}

		public STRUTranInfo2(string strCreateTime, string strModifyTime, string strTransactionID, string strOrderNO, int iBankType, string strBankOrderNO, string strOppQQID, int iStatus, int iFee, string strProductDesc, int iOrderFlag)
		{
			this.strCreateTime = strCreateTime;
			this.strModifyTime = strModifyTime;
			this.strTransactionID = strTransactionID;
			this.strOrderNO = strOrderNO;
			this.iBankType = iBankType;
			this.strBankOrderNO = strBankOrderNO;
			this.strOppQQID = strOppQQID;
			this.iStatus = iStatus;
			this.iFee = iFee;
			this.strProductDesc = strProductDesc;
			this.iOrderFlag = iOrderFlag;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			if(strCreateTime != null)
			{
				h__ = 5 * h__ + strCreateTime.GetHashCode();
			}
			if(strModifyTime != null)
			{
				h__ = 5 * h__ + strModifyTime.GetHashCode();
			}
			if(strTransactionID != null)
			{
				h__ = 5 * h__ + strTransactionID.GetHashCode();
			}
			if(strOrderNO != null)
			{
				h__ = 5 * h__ + strOrderNO.GetHashCode();
			}
			h__ = 5 * h__ + iBankType.GetHashCode();
			if(strBankOrderNO != null)
			{
				h__ = 5 * h__ + strBankOrderNO.GetHashCode();
			}
			if(strOppQQID != null)
			{
				h__ = 5 * h__ + strOppQQID.GetHashCode();
			}
			h__ = 5 * h__ + iStatus.GetHashCode();
			h__ = 5 * h__ + iFee.GetHashCode();
			if(strProductDesc != null)
			{
				h__ = 5 * h__ + strProductDesc.GetHashCode();
			}
			h__ = 5 * h__ + iOrderFlag.GetHashCode();
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			STRUTranInfo2 o__ = (STRUTranInfo2)other__;
			if(strCreateTime == null)
			{
				if(o__.strCreateTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!strCreateTime.Equals(o__.strCreateTime))
				{
					return false;
				}
			}
			if(strModifyTime == null)
			{
				if(o__.strModifyTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!strModifyTime.Equals(o__.strModifyTime))
				{
					return false;
				}
			}
			if(strTransactionID == null)
			{
				if(o__.strTransactionID != null)
				{
					return false;
				}
			}
			else
			{
				if(!strTransactionID.Equals(o__.strTransactionID))
				{
					return false;
				}
			}
			if(strOrderNO == null)
			{
				if(o__.strOrderNO != null)
				{
					return false;
				}
			}
			else
			{
				if(!strOrderNO.Equals(o__.strOrderNO))
				{
					return false;
				}
			}
			if(!iBankType.Equals(o__.iBankType))
			{
				return false;
			}
			if(strBankOrderNO == null)
			{
				if(o__.strBankOrderNO != null)
				{
					return false;
				}
			}
			else
			{
				if(!strBankOrderNO.Equals(o__.strBankOrderNO))
				{
					return false;
				}
			}
			if(strOppQQID == null)
			{
				if(o__.strOppQQID != null)
				{
					return false;
				}
			}
			else
			{
				if(!strOppQQID.Equals(o__.strOppQQID))
				{
					return false;
				}
			}
			if(!iStatus.Equals(o__.iStatus))
			{
				return false;
			}
			if(!iFee.Equals(o__.iFee))
			{
				return false;
			}
			if(strProductDesc == null)
			{
				if(o__.strProductDesc != null)
				{
					return false;
				}
			}
			else
			{
				if(!strProductDesc.Equals(o__.strProductDesc))
				{
					return false;
				}
			}
			if(!iOrderFlag.Equals(o__.iOrderFlag))
			{
				return false;
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(STRUTranInfo2 lhs__, STRUTranInfo2 rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(STRUTranInfo2 lhs__, STRUTranInfo2 rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeString(strCreateTime);
			os__.writeString(strModifyTime);
			os__.writeString(strTransactionID);
			os__.writeString(strOrderNO);
			os__.writeInt(iBankType);
			os__.writeString(strBankOrderNO);
			os__.writeString(strOppQQID);
			os__.writeInt(iStatus);
			os__.writeInt(iFee);
			os__.writeString(strProductDesc);
			os__.writeInt(iOrderFlag);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			strCreateTime = is__.readString();
			strModifyTime = is__.readString();
			strTransactionID = is__.readString();
			strOrderNO = is__.readString();
			iBankType = is__.readInt();
			strBankOrderNO = is__.readString();
			strOppQQID = is__.readString();
			iStatus = is__.readInt();
			iFee = is__.readInt();
			strProductDesc = is__.readString();
			iOrderFlag = is__.readInt();
		}

		#endregion
	}

	public interface IAMSManager : Ice.Object, IAMSManagerOperations_, IAMSManagerOperationsNC_
	{
	}
}

namespace MediCS
{
	public class OperationLog : _System.ICloneable
	{
		#region Slice data members

		public int id;

		public int state;

		public int opOrig;

		public int opType;

		public int reason;

		public string usrLoginId;

		public string operatorId;

		public string createTime;

		public string modifyTime;

		public string beginTime;

		public string endTime;

		public string memo;

		#endregion

		#region Constructors

		public OperationLog()
		{
		}

		public OperationLog(int id, int state, int opOrig, int opType, int reason, string usrLoginId, string operatorId, string createTime, string modifyTime, string beginTime, string endTime, string memo)
		{
			this.id = id;
			this.state = state;
			this.opOrig = opOrig;
			this.opType = opType;
			this.reason = reason;
			this.usrLoginId = usrLoginId;
			this.operatorId = operatorId;
			this.createTime = createTime;
			this.modifyTime = modifyTime;
			this.beginTime = beginTime;
			this.endTime = endTime;
			this.memo = memo;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			h__ = 5 * h__ + id.GetHashCode();
			h__ = 5 * h__ + state.GetHashCode();
			h__ = 5 * h__ + opOrig.GetHashCode();
			h__ = 5 * h__ + opType.GetHashCode();
			h__ = 5 * h__ + reason.GetHashCode();
			if(usrLoginId != null)
			{
				h__ = 5 * h__ + usrLoginId.GetHashCode();
			}
			if(operatorId != null)
			{
				h__ = 5 * h__ + operatorId.GetHashCode();
			}
			if(createTime != null)
			{
				h__ = 5 * h__ + createTime.GetHashCode();
			}
			if(modifyTime != null)
			{
				h__ = 5 * h__ + modifyTime.GetHashCode();
			}
			if(beginTime != null)
			{
				h__ = 5 * h__ + beginTime.GetHashCode();
			}
			if(endTime != null)
			{
				h__ = 5 * h__ + endTime.GetHashCode();
			}
			if(memo != null)
			{
				h__ = 5 * h__ + memo.GetHashCode();
			}
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			OperationLog o__ = (OperationLog)other__;
			if(!id.Equals(o__.id))
			{
				return false;
			}
			if(!state.Equals(o__.state))
			{
				return false;
			}
			if(!opOrig.Equals(o__.opOrig))
			{
				return false;
			}
			if(!opType.Equals(o__.opType))
			{
				return false;
			}
			if(!reason.Equals(o__.reason))
			{
				return false;
			}
			if(usrLoginId == null)
			{
				if(o__.usrLoginId != null)
				{
					return false;
				}
			}
			else
			{
				if(!usrLoginId.Equals(o__.usrLoginId))
				{
					return false;
				}
			}
			if(operatorId == null)
			{
				if(o__.operatorId != null)
				{
					return false;
				}
			}
			else
			{
				if(!operatorId.Equals(o__.operatorId))
				{
					return false;
				}
			}
			if(createTime == null)
			{
				if(o__.createTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!createTime.Equals(o__.createTime))
				{
					return false;
				}
			}
			if(modifyTime == null)
			{
				if(o__.modifyTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!modifyTime.Equals(o__.modifyTime))
				{
					return false;
				}
			}
			if(beginTime == null)
			{
				if(o__.beginTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!beginTime.Equals(o__.beginTime))
				{
					return false;
				}
			}
			if(endTime == null)
			{
				if(o__.endTime != null)
				{
					return false;
				}
			}
			else
			{
				if(!endTime.Equals(o__.endTime))
				{
					return false;
				}
			}
			if(memo == null)
			{
				if(o__.memo != null)
				{
					return false;
				}
			}
			else
			{
				if(!memo.Equals(o__.memo))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(OperationLog lhs__, OperationLog rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(OperationLog lhs__, OperationLog rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeInt(id);
			os__.writeInt(state);
			os__.writeInt(opOrig);
			os__.writeInt(opType);
			os__.writeInt(reason);
			os__.writeString(usrLoginId);
			os__.writeString(operatorId);
			os__.writeString(createTime);
			os__.writeString(modifyTime);
			os__.writeString(beginTime);
			os__.writeString(endTime);
			os__.writeString(memo);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			id = is__.readInt();
			state = is__.readInt();
			opOrig = is__.readInt();
			opType = is__.readInt();
			reason = is__.readInt();
			usrLoginId = is__.readString();
			operatorId = is__.readString();
			createTime = is__.readString();
			modifyTime = is__.readString();
			beginTime = is__.readString();
			endTime = is__.readString();
			memo = is__.readString();
		}

		#endregion
	}

	public class CompareCond : _System.ICloneable
	{
		#region Slice data members

		public string keyName;

		public string compareVal;

		public string op;

		#endregion

		#region Constructors

		public CompareCond()
		{
		}

		public CompareCond(string keyName, string compareVal, string op)
		{
			this.keyName = keyName;
			this.compareVal = compareVal;
			this.op = op;
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			if(keyName != null)
			{
				h__ = 5 * h__ + keyName.GetHashCode();
			}
			if(compareVal != null)
			{
				h__ = 5 * h__ + compareVal.GetHashCode();
			}
			if(op != null)
			{
				h__ = 5 * h__ + op.GetHashCode();
			}
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(other__ == null)
			{
				return false;
			}
			if(GetType() != other__.GetType())
			{
				return false;
			}
			CompareCond o__ = (CompareCond)other__;
			if(keyName == null)
			{
				if(o__.keyName != null)
				{
					return false;
				}
			}
			else
			{
				if(!keyName.Equals(o__.keyName))
				{
					return false;
				}
			}
			if(compareVal == null)
			{
				if(o__.compareVal != null)
				{
					return false;
				}
			}
			else
			{
				if(!compareVal.Equals(o__.compareVal))
				{
					return false;
				}
			}
			if(op == null)
			{
				if(o__.op != null)
				{
					return false;
				}
			}
			else
			{
				if(!op.Equals(o__.op))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(CompareCond lhs__, CompareCond rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(CompareCond lhs__, CompareCond rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshalling support

		public void write__(IceInternal.BasicStream os__)
		{
			os__.writeString(keyName);
			os__.writeString(compareVal);
			os__.writeString(op);
		}

		public void read__(IceInternal.BasicStream is__)
		{
			keyName = is__.readString();
			compareVal = is__.readString();
			op = is__.readString();
		}

		#endregion
	}

	public class KVDict : _System.Collections.DictionaryBase, _System.ICloneable
	{
		#region KVDict members

		public void AddRange(KVDict d__)
		{
			foreach(_System.Collections.DictionaryEntry e in d__)
			{
				try
				{
					InnerHashtable.Add(e.Key, e.Value);
				}
				catch(_System.ArgumentException)
				{
					// ignore
				}
			}
		}

		#endregion

		#region IDictionary members

		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public _System.Collections.ICollection Keys
		{
			get
			{
				return InnerHashtable.Keys;
			}
		}

		public _System.Collections.ICollection Values
		{
			get
			{
				return InnerHashtable.Values;
			}
		}

		#region Indexer

		public string this[string key]
		{
			get
			{
				return (string)InnerHashtable[key];
			}
			set
			{
				InnerHashtable[key] = value;
			}
		}

		#endregion

		public void Add(string key, string value)
		{
			InnerHashtable.Add(key, value);
		}

		public void Remove(string key)
		{
			InnerHashtable.Remove(key);
		}

		public bool Contains(string key)
		{
			return InnerHashtable.Contains(key);
		}

		#endregion

		#region ICollection members

		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		#endregion

		#region ICloneable members

		public object Clone()
		{
			KVDict d = new KVDict();
			foreach(_System.Collections.DictionaryEntry e in InnerHashtable)
			{
				d.InnerHashtable.Add(e.Key, e.Value);
			}
			return d;
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int hash = 0;
			foreach(_System.Collections.DictionaryEntry e in InnerHashtable)
			{
				hash = 5 * hash + e.Key.GetHashCode();
				if(e.Value != null)
				{
					hash = 5 * hash + e.Value.GetHashCode();
				}
			}
			return hash;
		}

		public override bool Equals(object other)
		{
			if(object.ReferenceEquals(this, other))
			{
				return true;
			}
			if(!(other is KVDict))
			{
				return false;
			}
			if(Count != ((KVDict)other).Count)
			{
				return false;
			}
			string[] klhs__ = new string[Count];
			Keys.CopyTo(klhs__, 0);
			_System.Array.Sort(klhs__);
			string[] krhs__ = new string[((KVDict)other).Count];
			((KVDict)other).Keys.CopyTo(krhs__, 0);
			_System.Array.Sort(krhs__);
			for(int i = 0; i < Count; ++i)
			{
				if(!klhs__[i].Equals(krhs__[i]))
				{
					return false;
				}
			}
			string[] vlhs__ = new string[Count];
			Values.CopyTo(vlhs__, 0);
			_System.Array.Sort(vlhs__);
			string[] vrhs__ = new string[((KVDict)other).Count];
			((KVDict)other).Values.CopyTo(vrhs__, 0);
			_System.Array.Sort(vrhs__);
			for(int i = 0; i < Count; ++i)
			{
				if(vlhs__[i] == null)
				{
					if(vrhs__[i] != null)
					{
						return false;
					}
				}
				else if(!vlhs__[i].Equals(vrhs__[i]))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(KVDict lhs__, KVDict rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(KVDict lhs__, KVDict rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion
	}

	public class ErrorBase : Ice.UserException
	{
		#region Slice data members

		public string ice_message_;

		#endregion

		#region Constructors

		private static readonly string _dflt = "ErrorBase";

		public ErrorBase() : base(_dflt)
		{
		}

		public ErrorBase(string m__) : base(m__)
		{
		}

		public ErrorBase(_System.Exception ex__) : base(_dflt, ex__)
		{
		}

		public ErrorBase(string m__, _System.Exception ex__) : base(m__, ex__)
		{
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			if((object)ice_message_ != null)
			{
				h__ = 5 * h__ + ice_message_.GetHashCode();
			}
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(other__ == null)
			{
				return false;
			}
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(!(other__ is ErrorBase))
			{
				return false;
			}
			if(ice_message_ == null)
			{
				if(((ErrorBase)other__).ice_message_ != null)
				{
					return false;
				}
			}
			else
			{
				if(!ice_message_.Equals(((ErrorBase)other__).ice_message_))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(ErrorBase lhs__, ErrorBase rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(ErrorBase lhs__, ErrorBase rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshaling support

		public override void write__(IceInternal.BasicStream os__)
		{
			os__.writeString("::MediCS::ErrorBase");
			os__.startWriteSlice();
			os__.writeString(ice_message_);
			os__.endWriteSlice();
		}

		public override void read__(IceInternal.BasicStream is__, bool rid__)
		{
			if(rid__)
			{
				/* string myId = */ is__.readString();
			}
			is__.startReadSlice();
			ice_message_ = is__.readString();
			is__.endReadSlice();
		}

		public override void write__(Ice.OutputStream outS__)
		{
			Ice.MarshalException ex = new Ice.MarshalException();
			ex.reason = "exception MediCS::ErrorBase was not generated with stream support";
			throw ex;
		}

		public override void read__(Ice.InputStream inS__, bool rid__)
		{
			Ice.MarshalException ex = new Ice.MarshalException();
			ex.reason = "exception MediCS::ErrorBase was not generated with stream support";
			throw ex;
		}

		public override bool usesClasses__()
		{
			return true;
		}

		#endregion
	}

	public class ServiceError : MediCS.ErrorBase
	{
		#region Slice data members

		public int errNo;

		public string serviceName;

		#endregion

		#region Constructors

		private static readonly string _dflt = "ServiceError";

		public ServiceError() : base(_dflt)
		{
		}

		public ServiceError(string m__) : base(m__)
		{
		}

		public ServiceError(_System.Exception ex__) : base(_dflt, ex__)
		{
		}

		public ServiceError(string m__, _System.Exception ex__) : base(m__, ex__)
		{
		}

		#endregion

		#region Object members

		public override int GetHashCode()
		{
			int h__ = 0;
			h__ = 5 * h__ + errNo.GetHashCode();
			if((object)serviceName != null)
			{
				h__ = 5 * h__ + serviceName.GetHashCode();
			}
			return h__;
		}

		public override bool Equals(object other__)
		{
			if(other__ == null)
			{
				return false;
			}
			if(object.ReferenceEquals(this, other__))
			{
				return true;
			}
			if(!(other__ is ServiceError))
			{
				return false;
			}
			if(!errNo.Equals(((ServiceError)other__).errNo))
			{
				return false;
			}
			if(serviceName == null)
			{
				if(((ServiceError)other__).serviceName != null)
				{
					return false;
				}
			}
			else
			{
				if(!serviceName.Equals(((ServiceError)other__).serviceName))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Comparison members

		public static bool operator==(ServiceError lhs__, ServiceError rhs__)
		{
			return Equals(lhs__, rhs__);
		}

		public static bool operator!=(ServiceError lhs__, ServiceError rhs__)
		{
			return !Equals(lhs__, rhs__);
		}

		#endregion

		#region Marshaling support

		public override void write__(IceInternal.BasicStream os__)
		{
			os__.writeString("::MediCS::ServiceError");
			os__.startWriteSlice();
			os__.writeInt(errNo);
			os__.writeString(serviceName);
			os__.endWriteSlice();
			base.write__(os__);
		}

		public override void read__(IceInternal.BasicStream is__, bool rid__)
		{
			if(rid__)
			{
				/* string myId = */ is__.readString();
			}
			is__.startReadSlice();
			errNo = is__.readInt();
			serviceName = is__.readString();
			is__.endReadSlice();
			base.read__(is__, true);
		}

		public override void write__(Ice.OutputStream outS__)
		{
			Ice.MarshalException ex = new Ice.MarshalException();
			ex.reason = "exception MediCS::ServiceError was not generated with stream support";
			throw ex;
		}

		public override void read__(Ice.InputStream inS__, bool rid__)
		{
			Ice.MarshalException ex = new Ice.MarshalException();
			ex.reason = "exception MediCS::ServiceError was not generated with stream support";
			throw ex;
		}

		public override bool usesClasses__()
		{
			return true;
		}

		#endregion
	}

	public interface IMediCustService : Ice.Object, IMediCustServiceOperations_, IMediCustServiceOperationsNC_
	{
	}
}

namespace AMSManager
{
	public interface IAMSManagerPrx : Ice.ObjectPrx
	{
		bool QuerySettlementDetail(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetail[] vecSettlementDetails);
		bool QuerySettlementDetail(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetail[] vecSettlementDetails, Ice.Context context__);

		bool QueryTranInfo(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo[] vecTranInfos);
		bool QueryTranInfo(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo[] vecTranInfos, Ice.Context context__);

		bool StatOrderInfo(string strSPID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUStatOrderInfo stStatOrderInfo);
		bool StatOrderInfo(string strSPID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUStatOrderInfo stStatOrderInfo, Ice.Context context__);

		bool QueryTransInfoCount(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int lDownloadNum);
		bool QueryTransInfoCount(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int lDownloadNum, Ice.Context context__);

		bool DownloadTransInfo(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUUserOrder[] UserOrderVec);
		bool DownloadTransInfo(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUUserOrder[] UserOrderVec, Ice.Context context__);

		bool QueryUserOrderCount(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int OrderNum);
		bool QueryUserOrderCount(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int OrderNum, Ice.Context context__);

		bool DownloadUserOrder(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUNewUserOrder[] UserOrderVec);
		bool DownloadUserOrder(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUNewUserOrder[] UserOrderVec, Ice.Context context__);

		bool QueryAirTicketCount(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, out int lDownloadNum, out string sResinfo);
		bool QueryAirTicketCount(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, out int lDownloadNum, out string sResinfo, Ice.Context context__);

		bool DownloadAirTicket(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, int offset, int limit, out int lDownloadNum, out string sResinfo, out AMSManager.STRUAirTicket[] AirTicketVec);
		bool DownloadAirTicket(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, int offset, int limit, out int lDownloadNum, out string sResinfo, out AMSManager.STRUAirTicket[] AirTicketVec, Ice.Context context__);

		bool QueryBulletinInfo(int iSystemID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUBulletinDetail[] vecBulletinDetail);
		bool QueryBulletinInfo(int iSystemID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUBulletinDetail[] vecBulletinDetail, Ice.Context context__);

		string GetString(string strInput);
		string GetString(string strInput, Ice.Context context__);

		bool QueryTranInfoEx(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfoEx[] tranList);
		bool QueryTranInfoEx(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfoEx[] tranList, Ice.Context context__);

		int QueryAccountRollCount(string uin, string begTime, string endTime, out int errCode, out string errInfo);
		int QueryAccountRollCount(string uin, string begTime, string endTime, out int errCode, out string errInfo, Ice.Context context__);

		void QueryAccountRollList(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo);
		void QueryAccountRollList(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__);

		void QueryAccountRollList2(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecord2[] rolls, out int errCode, out string errInfo);
		void QueryAccountRollList2(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecord2[] rolls, out int errCode, out string errInfo, Ice.Context context__);

		bool QuerySettDetailExBySpid(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails);
		bool QuerySettDetailExBySpid(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails, Ice.Context context__);

		bool QuerySettDetailExByAgentId(string agentId, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails);
		bool QuerySettDetailExByAgentId(string agentId, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails, Ice.Context context__);

		bool QueryTranReport(string sSPID, string sTranDate, out int iResult, out string strResInfo, out int iTotalCount, out int iPaidNum, out string paidMoney, out int iUnpaidNum, out string unpaidMoney, out AMSManager.STRUTranInfoEx[] tranList);
		bool QueryTranReport(string sSPID, string sTranDate, out int iResult, out string strResInfo, out int iTotalCount, out int iPaidNum, out string paidMoney, out int iUnpaidNum, out string unpaidMoney, out AMSManager.STRUTranInfoEx[] tranList, Ice.Context context__);

		int QueryAccountRollCountByCond(string uin, AMSManager.CompareCond[] conds, out int errCode, out string errInfo);
		int QueryAccountRollCountByCond(string uin, AMSManager.CompareCond[] conds, out int errCode, out string errInfo, Ice.Context context__);

		void QueryAccountRollListByCond(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo);
		void QueryAccountRollListByCond(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo, Ice.Context context__);

		void DownloadAccountDetails(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo);
		void DownloadAccountDetails(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__);

		int QuerySpAccountRollCount(string spid, string begTime, string endTime, out int errCode, out string errInfo);
		int QuerySpAccountRollCount(string spid, string begTime, string endTime, out int errCode, out string errInfo, Ice.Context context__);

		void QuerySpAccountRollList(string spid, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo);
		void QuerySpAccountRollList(string spid, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__);

		bool QueryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos);
		bool QueryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos, Ice.Context context__);

		bool QueryHistoryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos);
		bool QueryHistoryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos, Ice.Context context__);

		void QueryTransBankRollList(string spid, string listId, int offset, int limit, out int total, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo);
		void QueryTransBankRollList(string spid, string listId, int offset, int limit, out int total, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo, Ice.Context context__);

		bool QueryDrawDetail(string strSPID, string strMin, int iOffset, int iLimit, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUDrawDetail[] vecDrawDetails);
		bool QueryDrawDetail(string strSPID, string strMin, int iOffset, int iLimit, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUDrawDetail[] vecDrawDetails, Ice.Context context__);

		bool QueryOkSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, int iNo, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent);
		bool QueryOkSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, int iNo, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent, Ice.Context context__);

		bool QueryNoSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, out int iResult, out string strResInfo, out int iNoAllCount, out string iNoAllAmt, out int iNoTodayCount, out string iNoTodayAmt, out int iRefundCOunt, out string iRefundAmt, out int iNoHisCount, out string iNoHisAmt, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent);
		bool QueryNoSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, out int iResult, out string strResInfo, out int iNoAllCount, out string iNoAllAmt, out int iNoTodayCount, out string iNoTodayAmt, out int iRefundCOunt, out string iRefundAmt, out int iNoHisCount, out string iNoHisAmt, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent, Ice.Context context__);

		int QuerySubAccountRollCount(int uid, int curType, int usrType, int rollType, string begTime, string endTime, out int errCode, out string errInfo);
		int QuerySubAccountRollCount(int uid, int curType, int usrType, int rollType, string begTime, string endTime, out int errCode, out string errInfo, Ice.Context context__);

		void QuerySubAccountRollList(int uid, int curType, int usrType, int rollType, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo);
		void QuerySubAccountRollList(int uid, int curType, int usrType, int rollType, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__);

		bool QueryAirOrderBat(string strSpid, string strStime, string strEtime, int ioffset, int iLimit, out int iTotal, out int iReturnNum, out int iErrCode, out string strErrInfo, out AMSManager.STRAirOrderinfo[] SEQrolls);
		bool QueryAirOrderBat(string strSpid, string strStime, string strEtime, int ioffset, int iLimit, out int iTotal, out int iReturnNum, out int iErrCode, out string strErrInfo, out AMSManager.STRAirOrderinfo[] SEQrolls, Ice.Context context__);
	}
}

namespace MediCS
{
	public interface IMediCustServicePrx : Ice.ObjectPrx
	{
		long addOpLog(MediCS.OperationLog opLog);
		long addOpLog(MediCS.OperationLog opLog, Ice.Context context__);

		void queryOpLog(MediCS.CompareCond[] condition, int offset, int limit, out MediCS.OperationLog[] results);
		void queryOpLog(MediCS.CompareCond[] condition, int offset, int limit, out MediCS.OperationLog[] results, Ice.Context context__);

		int queryOpCount(MediCS.CompareCond[] condition);
		int queryOpCount(MediCS.CompareCond[] condition, Ice.Context context__);

		void updateAppealRecord(string transId, string appealId, MediCS.KVDict kvs);
		void updateAppealRecord(string transId, string appealId, MediCS.KVDict kvs, Ice.Context context__);
	}
}

namespace AMSManager
{
	public interface IAMSManagerOperations_
	{
		bool QuerySettlementDetail(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetail[] vecSettlementDetails, Ice.Current current__);

		bool QueryTranInfo(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo[] vecTranInfos, Ice.Current current__);

		bool StatOrderInfo(string strSPID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUStatOrderInfo stStatOrderInfo, Ice.Current current__);

		bool QueryTransInfoCount(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int lDownloadNum, Ice.Current current__);

		bool DownloadTransInfo(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUUserOrder[] UserOrderVec, Ice.Current current__);

		bool QueryUserOrderCount(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int OrderNum, Ice.Current current__);

		bool DownloadUserOrder(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUNewUserOrder[] UserOrderVec, Ice.Current current__);

		bool QueryAirTicketCount(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, out int lDownloadNum, out string sResinfo, Ice.Current current__);

		bool DownloadAirTicket(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, int offset, int limit, out int lDownloadNum, out string sResinfo, out AMSManager.STRUAirTicket[] AirTicketVec, Ice.Current current__);

		bool QueryBulletinInfo(int iSystemID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUBulletinDetail[] vecBulletinDetail, Ice.Current current__);

		string GetString(string strInput, Ice.Current current__);

		bool QueryTranInfoEx(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfoEx[] tranList, Ice.Current current__);

		int QueryAccountRollCount(string uin, string begTime, string endTime, out int errCode, out string errInfo, Ice.Current current__);

		void QueryAccountRollList(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Current current__);

		void QueryAccountRollList2(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecord2[] rolls, out int errCode, out string errInfo, Ice.Current current__);

		bool QuerySettDetailExBySpid(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails, Ice.Current current__);

		bool QuerySettDetailExByAgentId(string agentId, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails, Ice.Current current__);

		bool QueryTranReport(string sSPID, string sTranDate, out int iResult, out string strResInfo, out int iTotalCount, out int iPaidNum, out string paidMoney, out int iUnpaidNum, out string unpaidMoney, out AMSManager.STRUTranInfoEx[] tranList, Ice.Current current__);

		int QueryAccountRollCountByCond(string uin, AMSManager.CompareCond[] conds, out int errCode, out string errInfo, Ice.Current current__);

		void QueryAccountRollListByCond(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo, Ice.Current current__);

		void DownloadAccountDetails(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Current current__);

		int QuerySpAccountRollCount(string spid, string begTime, string endTime, out int errCode, out string errInfo, Ice.Current current__);

		void QuerySpAccountRollList(string spid, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Current current__);

		bool QueryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos, Ice.Current current__);

		bool QueryHistoryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos, Ice.Current current__);

		void QueryTransBankRollList(string spid, string listId, int offset, int limit, out int total, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo, Ice.Current current__);

		bool QueryDrawDetail(string strSPID, string strMin, int iOffset, int iLimit, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUDrawDetail[] vecDrawDetails, Ice.Current current__);

		bool QueryOkSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, int iNo, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent, Ice.Current current__);

		bool QueryNoSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, out int iResult, out string strResInfo, out int iNoAllCount, out string iNoAllAmt, out int iNoTodayCount, out string iNoTodayAmt, out int iRefundCOunt, out string iRefundAmt, out int iNoHisCount, out string iNoHisAmt, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent, Ice.Current current__);

		int QuerySubAccountRollCount(int uid, int curType, int usrType, int rollType, string begTime, string endTime, out int errCode, out string errInfo, Ice.Current current__);

		void QuerySubAccountRollList(int uid, int curType, int usrType, int rollType, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Current current__);

		bool QueryAirOrderBat(string strSpid, string strStime, string strEtime, int ioffset, int iLimit, out int iTotal, out int iReturnNum, out int iErrCode, out string strErrInfo, out AMSManager.STRAirOrderinfo[] SEQrolls, Ice.Current current__);
	}

	public interface IAMSManagerOperationsNC_
	{
		bool QuerySettlementDetail(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetail[] vecSettlementDetails);

		bool QueryTranInfo(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo[] vecTranInfos);

		bool StatOrderInfo(string strSPID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUStatOrderInfo stStatOrderInfo);

		bool QueryTransInfoCount(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int lDownloadNum);

		bool DownloadTransInfo(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUUserOrder[] UserOrderVec);

		bool QueryUserOrderCount(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int OrderNum);

		bool DownloadUserOrder(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUNewUserOrder[] UserOrderVec);

		bool QueryAirTicketCount(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, out int lDownloadNum, out string sResinfo);

		bool DownloadAirTicket(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, int offset, int limit, out int lDownloadNum, out string sResinfo, out AMSManager.STRUAirTicket[] AirTicketVec);

		bool QueryBulletinInfo(int iSystemID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUBulletinDetail[] vecBulletinDetail);

		string GetString(string strInput);

		bool QueryTranInfoEx(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfoEx[] tranList);

		int QueryAccountRollCount(string uin, string begTime, string endTime, out int errCode, out string errInfo);

		void QueryAccountRollList(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo);

		void QueryAccountRollList2(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecord2[] rolls, out int errCode, out string errInfo);

		bool QuerySettDetailExBySpid(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails);

		bool QuerySettDetailExByAgentId(string agentId, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails);

		bool QueryTranReport(string sSPID, string sTranDate, out int iResult, out string strResInfo, out int iTotalCount, out int iPaidNum, out string paidMoney, out int iUnpaidNum, out string unpaidMoney, out AMSManager.STRUTranInfoEx[] tranList);

		int QueryAccountRollCountByCond(string uin, AMSManager.CompareCond[] conds, out int errCode, out string errInfo);

		void QueryAccountRollListByCond(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo);

		void DownloadAccountDetails(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo);

		int QuerySpAccountRollCount(string spid, string begTime, string endTime, out int errCode, out string errInfo);

		void QuerySpAccountRollList(string spid, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo);

		bool QueryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos);

		bool QueryHistoryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos);

		void QueryTransBankRollList(string spid, string listId, int offset, int limit, out int total, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo);

		bool QueryDrawDetail(string strSPID, string strMin, int iOffset, int iLimit, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUDrawDetail[] vecDrawDetails);

		bool QueryOkSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, int iNo, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent);

		bool QueryNoSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, out int iResult, out string strResInfo, out int iNoAllCount, out string iNoAllAmt, out int iNoTodayCount, out string iNoTodayAmt, out int iRefundCOunt, out string iRefundAmt, out int iNoHisCount, out string iNoHisAmt, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent);

		int QuerySubAccountRollCount(int uid, int curType, int usrType, int rollType, string begTime, string endTime, out int errCode, out string errInfo);

		void QuerySubAccountRollList(int uid, int curType, int usrType, int rollType, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo);

		bool QueryAirOrderBat(string strSpid, string strStime, string strEtime, int ioffset, int iLimit, out int iTotal, out int iReturnNum, out int iErrCode, out string strErrInfo, out AMSManager.STRAirOrderinfo[] SEQrolls);
	}
}

namespace MediCS
{
	public interface IMediCustServiceOperations_
	{
		long addOpLog(MediCS.OperationLog opLog, Ice.Current current__);

		void queryOpLog(MediCS.CompareCond[] condition, int offset, int limit, out MediCS.OperationLog[] results, Ice.Current current__);

		int queryOpCount(MediCS.CompareCond[] condition, Ice.Current current__);

		void updateAppealRecord(string transId, string appealId, MediCS.KVDict kvs, Ice.Current current__);
	}

	public interface IMediCustServiceOperationsNC_
	{
		long addOpLog(MediCS.OperationLog opLog);

		void queryOpLog(MediCS.CompareCond[] condition, int offset, int limit, out MediCS.OperationLog[] results);

		int queryOpCount(MediCS.CompareCond[] condition);

		void updateAppealRecord(string transId, string appealId, MediCS.KVDict kvs);
	}
}

namespace AMSManager
{
	public sealed class StrMapHelper
	{
		public static void write(IceInternal.BasicStream os__, StrMap v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Count);
				foreach(_System.Collections.DictionaryEntry e__ in v__)
				{
					os__.writeString(((string)e__.Key));
					os__.writeString(((string)e__.Value));
				}
			}
		}

		public static StrMap read(IceInternal.BasicStream is__)
		{
			int sz__ = is__.readSize();
			StrMap r__ = new StrMap();
			for(int i__ = 0; i__ < sz__; ++i__)
			{
				string k__;
				k__ = is__.readString();
				string v__;
				v__ = is__.readString();
				r__[k__] = v__;
			}
			return r__;
		}
	}

	public sealed class SEQAirOrderInfoHelper
	{
		public static void write(IceInternal.BasicStream os__, AMSManager.STRAirOrderinfo[] v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Length);
				for(int ix__ = 0; ix__ < v__.Length; ++ix__)
				{
					v__[ix__].write__(os__);
				}
			}
		}

		public static AMSManager.STRAirOrderinfo[] read(IceInternal.BasicStream is__)
		{
			AMSManager.STRAirOrderinfo[] v__;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 39);
			v__ = new AMSManager.STRAirOrderinfo[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				v__[ix__] = new AMSManager.STRAirOrderinfo();
				v__[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			return v__;
		}
	}

	public sealed class SEQDrawDetailHelper
	{
		public static void write(IceInternal.BasicStream os__, AMSManager.STRUDrawDetail[] v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Length);
				for(int ix__ = 0; ix__ < v__.Length; ++ix__)
				{
					v__[ix__].write__(os__);
				}
			}
		}

		public static AMSManager.STRUDrawDetail[] read(IceInternal.BasicStream is__)
		{
			AMSManager.STRUDrawDetail[] v__;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 12);
			v__ = new AMSManager.STRUDrawDetail[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				v__[ix__] = new AMSManager.STRUDrawDetail();
				v__[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			return v__;
		}
	}

	public sealed class SEQSettlementDetailAgentHelper
	{
		public static void write(IceInternal.BasicStream os__, AMSManager.STRUSettlementDetailAgent[] v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Length);
				for(int ix__ = 0; ix__ < v__.Length; ++ix__)
				{
					v__[ix__].write__(os__);
				}
			}
		}

		public static AMSManager.STRUSettlementDetailAgent[] read(IceInternal.BasicStream is__)
		{
			AMSManager.STRUSettlementDetailAgent[] v__;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 21);
			v__ = new AMSManager.STRUSettlementDetailAgent[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				v__[ix__] = new AMSManager.STRUSettlementDetailAgent();
				v__[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			return v__;
		}
	}

	public sealed class SEQTranInfoHelper
	{
		public static void write(IceInternal.BasicStream os__, AMSManager.STRUTranInfo[] v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Length);
				for(int ix__ = 0; ix__ < v__.Length; ++ix__)
				{
					v__[ix__].write__(os__);
				}
			}
		}

		public static AMSManager.STRUTranInfo[] read(IceInternal.BasicStream is__)
		{
			AMSManager.STRUTranInfo[] v__;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 22);
			v__ = new AMSManager.STRUTranInfo[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				v__[ix__] = new AMSManager.STRUTranInfo();
				v__[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			return v__;
		}
	}

	public sealed class SEQMerchantInfoHelper
	{
		public static void write(IceInternal.BasicStream os__, AMSManager.STRUMerchantInfo[] v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Length);
				for(int ix__ = 0; ix__ < v__.Length; ++ix__)
				{
					v__[ix__].write__(os__);
				}
			}
		}

		public static AMSManager.STRUMerchantInfo[] read(IceInternal.BasicStream is__)
		{
			AMSManager.STRUMerchantInfo[] v__;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 52);
			v__ = new AMSManager.STRUMerchantInfo[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				v__[ix__] = new AMSManager.STRUMerchantInfo();
				v__[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			return v__;
		}
	}

	public sealed class SEQSettlementDetailHelper
	{
		public static void write(IceInternal.BasicStream os__, AMSManager.STRUSettlementDetail[] v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Length);
				for(int ix__ = 0; ix__ < v__.Length; ++ix__)
				{
					v__[ix__].write__(os__);
				}
			}
		}

		public static AMSManager.STRUSettlementDetail[] read(IceInternal.BasicStream is__)
		{
			AMSManager.STRUSettlementDetail[] v__;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 16);
			v__ = new AMSManager.STRUSettlementDetail[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				v__[ix__] = new AMSManager.STRUSettlementDetail();
				v__[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			return v__;
		}
	}

	public sealed class SEQBulletinDetailHelper
	{
		public static void write(IceInternal.BasicStream os__, AMSManager.STRUBulletinDetail[] v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Length);
				for(int ix__ = 0; ix__ < v__.Length; ++ix__)
				{
					v__[ix__].write__(os__);
				}
			}
		}

		public static AMSManager.STRUBulletinDetail[] read(IceInternal.BasicStream is__)
		{
			AMSManager.STRUBulletinDetail[] v__;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 8);
			v__ = new AMSManager.STRUBulletinDetail[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				v__[ix__] = new AMSManager.STRUBulletinDetail();
				v__[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			return v__;
		}
	}

	public sealed class SEQTranInfoExHelper
	{
		public static void write(IceInternal.BasicStream os__, AMSManager.STRUTranInfoEx[] v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Length);
				for(int ix__ = 0; ix__ < v__.Length; ++ix__)
				{
					v__[ix__].write__(os__);
				}
			}
		}

		public static AMSManager.STRUTranInfoEx[] read(IceInternal.BasicStream is__)
		{
			AMSManager.STRUTranInfoEx[] v__;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 50);
			v__ = new AMSManager.STRUTranInfoEx[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				v__[ix__] = new AMSManager.STRUTranInfoEx();
				v__[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			return v__;
		}
	}

	public sealed class AccRollListHelper
	{
		public static void write(IceInternal.BasicStream os__, AMSManager.AccRollRecord[] v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Length);
				for(int ix__ = 0; ix__ < v__.Length; ++ix__)
				{
					v__[ix__].write__(os__);
				}
			}
		}

		public static AMSManager.AccRollRecord[] read(IceInternal.BasicStream is__)
		{
			AMSManager.AccRollRecord[] v__;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 27);
			v__ = new AMSManager.AccRollRecord[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				v__[ix__] = new AMSManager.AccRollRecord();
				v__[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			return v__;
		}
	}

	public sealed class AccRollList2Helper
	{
		public static void write(IceInternal.BasicStream os__, AMSManager.AccRollRecord2[] v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Length);
				for(int ix__ = 0; ix__ < v__.Length; ++ix__)
				{
					v__[ix__].write__(os__);
				}
			}
		}

		public static AMSManager.AccRollRecord2[] read(IceInternal.BasicStream is__)
		{
			AMSManager.AccRollRecord2[] v__;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 28);
			v__ = new AMSManager.AccRollRecord2[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				v__[ix__] = new AMSManager.AccRollRecord2();
				v__[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			return v__;
		}
	}

	public sealed class AccRollListExHelper
	{
		public static void write(IceInternal.BasicStream os__, AMSManager.AccRollRecordEx[] v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Length);
				for(int ix__ = 0; ix__ < v__.Length; ++ix__)
				{
					v__[ix__].write__(os__);
				}
			}
		}

		public static AMSManager.AccRollRecordEx[] read(IceInternal.BasicStream is__)
		{
			AMSManager.AccRollRecordEx[] v__;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 30);
			v__ = new AMSManager.AccRollRecordEx[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				v__[ix__] = new AMSManager.AccRollRecordEx();
				v__[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			return v__;
		}
	}

	public sealed class SettDetailExListHelper
	{
		public static void write(IceInternal.BasicStream os__, AMSManager.STRUSettlementDetailEx[] v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Length);
				for(int ix__ = 0; ix__ < v__.Length; ++ix__)
				{
					v__[ix__].write__(os__);
				}
			}
		}

		public static AMSManager.STRUSettlementDetailEx[] read(IceInternal.BasicStream is__)
		{
			AMSManager.STRUSettlementDetailEx[] v__;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 17);
			v__ = new AMSManager.STRUSettlementDetailEx[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				v__[ix__] = new AMSManager.STRUSettlementDetailEx();
				v__[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			return v__;
		}
	}

	public sealed class SEQUserOrderHelper
	{
		public static void write(IceInternal.BasicStream os__, AMSManager.STRUUserOrder[] v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Length);
				for(int ix__ = 0; ix__ < v__.Length; ++ix__)
				{
					v__[ix__].write__(os__);
				}
			}
		}

		public static AMSManager.STRUUserOrder[] read(IceInternal.BasicStream is__)
		{
			AMSManager.STRUUserOrder[] v__;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 19);
			v__ = new AMSManager.STRUUserOrder[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				v__[ix__] = new AMSManager.STRUUserOrder();
				v__[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			return v__;
		}
	}

	public sealed class SEQNewUserOrderHelper
	{
		public static void write(IceInternal.BasicStream os__, AMSManager.STRUNewUserOrder[] v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Length);
				for(int ix__ = 0; ix__ < v__.Length; ++ix__)
				{
					v__[ix__].write__(os__);
				}
			}
		}

		public static AMSManager.STRUNewUserOrder[] read(IceInternal.BasicStream is__)
		{
			AMSManager.STRUNewUserOrder[] v__;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 31);
			v__ = new AMSManager.STRUNewUserOrder[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				v__[ix__] = new AMSManager.STRUNewUserOrder();
				v__[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			return v__;
		}
	}

	public sealed class SEQAirTicketHelper
	{
		public static void write(IceInternal.BasicStream os__, AMSManager.STRUAirTicket[] v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Length);
				for(int ix__ = 0; ix__ < v__.Length; ++ix__)
				{
					v__[ix__].write__(os__);
				}
			}
		}

		public static AMSManager.STRUAirTicket[] read(IceInternal.BasicStream is__)
		{
			AMSManager.STRUAirTicket[] v__;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 27);
			v__ = new AMSManager.STRUAirTicket[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				v__[ix__] = new AMSManager.STRUAirTicket();
				v__[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			return v__;
		}
	}

	public sealed class CompareCondListHelper
	{
		public static void write(IceInternal.BasicStream os__, AMSManager.CompareCond[] v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Length);
				for(int ix__ = 0; ix__ < v__.Length; ++ix__)
				{
					v__[ix__].write__(os__);
				}
			}
		}

		public static AMSManager.CompareCond[] read(IceInternal.BasicStream is__)
		{
			AMSManager.CompareCond[] v__;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 3);
			v__ = new AMSManager.CompareCond[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				v__[ix__] = new AMSManager.CompareCond();
				v__[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			return v__;
		}
	}

	public sealed class SEQTranInfo2Helper
	{
		public static void write(IceInternal.BasicStream os__, AMSManager.STRUTranInfo2[] v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Length);
				for(int ix__ = 0; ix__ < v__.Length; ++ix__)
				{
					v__[ix__].write__(os__);
				}
			}
		}

		public static AMSManager.STRUTranInfo2[] read(IceInternal.BasicStream is__)
		{
			AMSManager.STRUTranInfo2[] v__;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 23);
			v__ = new AMSManager.STRUTranInfo2[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				v__[ix__] = new AMSManager.STRUTranInfo2();
				v__[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			return v__;
		}
	}

	public sealed class IAMSManagerPrxHelper : Ice.ObjectPrxHelperBase, IAMSManagerPrx
	{
		#region Synchronous operations

		public void DownloadAccountDetails(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo)
		{
			DownloadAccountDetails(uin, conds, offset, limit, out rolls, out errCode, out errInfo, defaultContext__());
		}

		public void DownloadAccountDetails(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("DownloadAccountDetails");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					del__.DownloadAccountDetails(uin, conds, offset, limit, out rolls, out errCode, out errInfo, context__);
					return;
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool DownloadAirTicket(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, int offset, int limit, out int lDownloadNum, out string sResinfo, out AMSManager.STRUAirTicket[] AirTicketVec)
		{
			return DownloadAirTicket(strUin, strStartTime, strEndTime, lTransState, lAmountLowBound, lAmountHighBound, strPeerUin, offset, limit, out lDownloadNum, out sResinfo, out AirTicketVec, defaultContext__());
		}

		public bool DownloadAirTicket(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, int offset, int limit, out int lDownloadNum, out string sResinfo, out AMSManager.STRUAirTicket[] AirTicketVec, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("DownloadAirTicket");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.DownloadAirTicket(strUin, strStartTime, strEndTime, lTransState, lAmountLowBound, lAmountHighBound, strPeerUin, offset, limit, out lDownloadNum, out sResinfo, out AirTicketVec, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool DownloadTransInfo(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUUserOrder[] UserOrderVec)
		{
			return DownloadTransInfo(strUin, lRole, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, offset, limit, out strResinfo, out lDownloadNum, out UserOrderVec, defaultContext__());
		}

		public bool DownloadTransInfo(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUUserOrder[] UserOrderVec, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("DownloadTransInfo");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.DownloadTransInfo(strUin, lRole, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, offset, limit, out strResinfo, out lDownloadNum, out UserOrderVec, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool DownloadUserOrder(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUNewUserOrder[] UserOrderVec)
		{
			return DownloadUserOrder(strUin, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, offset, limit, out strResinfo, out lDownloadNum, out UserOrderVec, defaultContext__());
		}

		public bool DownloadUserOrder(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUNewUserOrder[] UserOrderVec, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("DownloadUserOrder");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.DownloadUserOrder(strUin, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, offset, limit, out strResinfo, out lDownloadNum, out UserOrderVec, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public string GetString(string strInput)
		{
			return GetString(strInput, defaultContext__());
		}

		public string GetString(string strInput, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("GetString");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.GetString(strInput, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public int QueryAccountRollCount(string uin, string begTime, string endTime, out int errCode, out string errInfo)
		{
			return QueryAccountRollCount(uin, begTime, endTime, out errCode, out errInfo, defaultContext__());
		}

		public int QueryAccountRollCount(string uin, string begTime, string endTime, out int errCode, out string errInfo, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QueryAccountRollCount");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QueryAccountRollCount(uin, begTime, endTime, out errCode, out errInfo, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public int QueryAccountRollCountByCond(string uin, AMSManager.CompareCond[] conds, out int errCode, out string errInfo)
		{
			return QueryAccountRollCountByCond(uin, conds, out errCode, out errInfo, defaultContext__());
		}

		public int QueryAccountRollCountByCond(string uin, AMSManager.CompareCond[] conds, out int errCode, out string errInfo, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QueryAccountRollCountByCond");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QueryAccountRollCountByCond(uin, conds, out errCode, out errInfo, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public void QueryAccountRollList(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo)
		{
			QueryAccountRollList(uin, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, defaultContext__());
		}

		public void QueryAccountRollList(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QueryAccountRollList");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					del__.QueryAccountRollList(uin, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, context__);
					return;
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public void QueryAccountRollList2(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecord2[] rolls, out int errCode, out string errInfo)
		{
			QueryAccountRollList2(uin, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, defaultContext__());
		}

		public void QueryAccountRollList2(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecord2[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QueryAccountRollList2");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					del__.QueryAccountRollList2(uin, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, context__);
					return;
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public void QueryAccountRollListByCond(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo)
		{
			QueryAccountRollListByCond(uin, conds, offset, limit, out rolls, out errCode, out errInfo, defaultContext__());
		}

		public void QueryAccountRollListByCond(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QueryAccountRollListByCond");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					del__.QueryAccountRollListByCond(uin, conds, offset, limit, out rolls, out errCode, out errInfo, context__);
					return;
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool QueryAirOrderBat(string strSpid, string strStime, string strEtime, int ioffset, int iLimit, out int iTotal, out int iReturnNum, out int iErrCode, out string strErrInfo, out AMSManager.STRAirOrderinfo[] SEQrolls)
		{
			return QueryAirOrderBat(strSpid, strStime, strEtime, ioffset, iLimit, out iTotal, out iReturnNum, out iErrCode, out strErrInfo, out SEQrolls, defaultContext__());
		}

		public bool QueryAirOrderBat(string strSpid, string strStime, string strEtime, int ioffset, int iLimit, out int iTotal, out int iReturnNum, out int iErrCode, out string strErrInfo, out AMSManager.STRAirOrderinfo[] SEQrolls, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QueryAirOrderBat");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QueryAirOrderBat(strSpid, strStime, strEtime, ioffset, iLimit, out iTotal, out iReturnNum, out iErrCode, out strErrInfo, out SEQrolls, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool QueryAirTicketCount(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, out int lDownloadNum, out string sResinfo)
		{
			return QueryAirTicketCount(strUin, strStartTime, strEndTime, lTransState, lAmountLowBound, lAmountHighBound, strPeerUin, out lDownloadNum, out sResinfo, defaultContext__());
		}

		public bool QueryAirTicketCount(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, out int lDownloadNum, out string sResinfo, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QueryAirTicketCount");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QueryAirTicketCount(strUin, strStartTime, strEndTime, lTransState, lAmountLowBound, lAmountHighBound, strPeerUin, out lDownloadNum, out sResinfo, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool QueryBulletinInfo(int iSystemID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUBulletinDetail[] vecBulletinDetail)
		{
			return QueryBulletinInfo(iSystemID, strStartTime, strEndTime, out iResult, out strResInfo, out vecBulletinDetail, defaultContext__());
		}

		public bool QueryBulletinInfo(int iSystemID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUBulletinDetail[] vecBulletinDetail, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QueryBulletinInfo");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QueryBulletinInfo(iSystemID, strStartTime, strEndTime, out iResult, out strResInfo, out vecBulletinDetail, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool QueryDrawDetail(string strSPID, string strMin, int iOffset, int iLimit, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUDrawDetail[] vecDrawDetails)
		{
			return QueryDrawDetail(strSPID, strMin, iOffset, iLimit, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out vecDrawDetails, defaultContext__());
		}

		public bool QueryDrawDetail(string strSPID, string strMin, int iOffset, int iLimit, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUDrawDetail[] vecDrawDetails, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QueryDrawDetail");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QueryDrawDetail(strSPID, strMin, iOffset, iLimit, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out vecDrawDetails, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool QueryHistoryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos)
		{
			return QueryHistoryTranInfo2(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, tmType, strBankID, iStartFee, iEndFee, iFlag, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out vecTranInfos, defaultContext__());
		}

		public bool QueryHistoryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QueryHistoryTranInfo2");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QueryHistoryTranInfo2(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, tmType, strBankID, iStartFee, iEndFee, iFlag, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out vecTranInfos, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool QueryNoSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, out int iResult, out string strResInfo, out int iNoAllCount, out string iNoAllAmt, out int iNoTodayCount, out string iNoTodayAmt, out int iRefundCOunt, out string iRefundAmt, out int iNoHisCount, out string iNoHisAmt, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent)
		{
			return QueryNoSettlementDetailAgent(strSPID, strMin, iOffset, iLimit, out iResult, out strResInfo, out iNoAllCount, out iNoAllAmt, out iNoTodayCount, out iNoTodayAmt, out iRefundCOunt, out iRefundAmt, out iNoHisCount, out iNoHisAmt, out iTotalCount, out vecSettlementDetailAgent, defaultContext__());
		}

		public bool QueryNoSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, out int iResult, out string strResInfo, out int iNoAllCount, out string iNoAllAmt, out int iNoTodayCount, out string iNoTodayAmt, out int iRefundCOunt, out string iRefundAmt, out int iNoHisCount, out string iNoHisAmt, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QueryNoSettlementDetailAgent");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QueryNoSettlementDetailAgent(strSPID, strMin, iOffset, iLimit, out iResult, out strResInfo, out iNoAllCount, out iNoAllAmt, out iNoTodayCount, out iNoTodayAmt, out iRefundCOunt, out iRefundAmt, out iNoHisCount, out iNoHisAmt, out iTotalCount, out vecSettlementDetailAgent, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool QueryOkSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, int iNo, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent)
		{
			return QueryOkSettlementDetailAgent(strSPID, strMin, iOffset, iLimit, iNo, out iResult, out strResInfo, out iTotalCount, out vecSettlementDetailAgent, defaultContext__());
		}

		public bool QueryOkSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, int iNo, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QueryOkSettlementDetailAgent");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QueryOkSettlementDetailAgent(strSPID, strMin, iOffset, iLimit, iNo, out iResult, out strResInfo, out iTotalCount, out vecSettlementDetailAgent, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool QuerySettDetailExByAgentId(string agentId, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails)
		{
			return QuerySettDetailExByAgentId(agentId, strMin, iOffset, iLimit, iStatus, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out mchamtAllsum, out mchamtBlance, out mchamtNonesum, out vecSettlementDetails, defaultContext__());
		}

		public bool QuerySettDetailExByAgentId(string agentId, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QuerySettDetailExByAgentId");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QuerySettDetailExByAgentId(agentId, strMin, iOffset, iLimit, iStatus, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out mchamtAllsum, out mchamtBlance, out mchamtNonesum, out vecSettlementDetails, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool QuerySettDetailExBySpid(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails)
		{
			return QuerySettDetailExBySpid(strSPID, strMin, iOffset, iLimit, iStatus, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out mchamtAllsum, out mchamtBlance, out mchamtNonesum, out vecSettlementDetails, defaultContext__());
		}

		public bool QuerySettDetailExBySpid(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QuerySettDetailExBySpid");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QuerySettDetailExBySpid(strSPID, strMin, iOffset, iLimit, iStatus, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out mchamtAllsum, out mchamtBlance, out mchamtNonesum, out vecSettlementDetails, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool QuerySettlementDetail(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetail[] vecSettlementDetails)
		{
			return QuerySettlementDetail(strSPID, strMin, iOffset, iLimit, iStatus, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out mchamtAllsum, out mchamtBlance, out mchamtNonesum, out vecSettlementDetails, defaultContext__());
		}

		public bool QuerySettlementDetail(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetail[] vecSettlementDetails, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QuerySettlementDetail");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QuerySettlementDetail(strSPID, strMin, iOffset, iLimit, iStatus, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out mchamtAllsum, out mchamtBlance, out mchamtNonesum, out vecSettlementDetails, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public int QuerySpAccountRollCount(string spid, string begTime, string endTime, out int errCode, out string errInfo)
		{
			return QuerySpAccountRollCount(spid, begTime, endTime, out errCode, out errInfo, defaultContext__());
		}

		public int QuerySpAccountRollCount(string spid, string begTime, string endTime, out int errCode, out string errInfo, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QuerySpAccountRollCount");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QuerySpAccountRollCount(spid, begTime, endTime, out errCode, out errInfo, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public void QuerySpAccountRollList(string spid, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo)
		{
			QuerySpAccountRollList(spid, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, defaultContext__());
		}

		public void QuerySpAccountRollList(string spid, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QuerySpAccountRollList");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					del__.QuerySpAccountRollList(spid, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, context__);
					return;
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public int QuerySubAccountRollCount(int uid, int curType, int usrType, int rollType, string begTime, string endTime, out int errCode, out string errInfo)
		{
			return QuerySubAccountRollCount(uid, curType, usrType, rollType, begTime, endTime, out errCode, out errInfo, defaultContext__());
		}

		public int QuerySubAccountRollCount(int uid, int curType, int usrType, int rollType, string begTime, string endTime, out int errCode, out string errInfo, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QuerySubAccountRollCount");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QuerySubAccountRollCount(uid, curType, usrType, rollType, begTime, endTime, out errCode, out errInfo, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public void QuerySubAccountRollList(int uid, int curType, int usrType, int rollType, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo)
		{
			QuerySubAccountRollList(uid, curType, usrType, rollType, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, defaultContext__());
		}

		public void QuerySubAccountRollList(int uid, int curType, int usrType, int rollType, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QuerySubAccountRollList");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					del__.QuerySubAccountRollList(uid, curType, usrType, rollType, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, context__);
					return;
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool QueryTranInfo(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo[] vecTranInfos)
		{
			return QueryTranInfo(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, strBankID, iStartFee, iEndFee, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out vecTranInfos, defaultContext__());
		}

		public bool QueryTranInfo(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo[] vecTranInfos, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QueryTranInfo");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QueryTranInfo(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, strBankID, iStartFee, iEndFee, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out vecTranInfos, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool QueryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos)
		{
			return QueryTranInfo2(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, tmType, strBankID, iStartFee, iEndFee, iFlag, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out vecTranInfos, defaultContext__());
		}

		public bool QueryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QueryTranInfo2");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QueryTranInfo2(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, tmType, strBankID, iStartFee, iEndFee, iFlag, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out vecTranInfos, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool QueryTranInfoEx(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfoEx[] tranList)
		{
			return QueryTranInfoEx(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, strBankID, iStartFee, iEndFee, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out tranList, defaultContext__());
		}

		public bool QueryTranInfoEx(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfoEx[] tranList, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QueryTranInfoEx");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QueryTranInfoEx(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, strBankID, iStartFee, iEndFee, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out tranList, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool QueryTranReport(string sSPID, string sTranDate, out int iResult, out string strResInfo, out int iTotalCount, out int iPaidNum, out string paidMoney, out int iUnpaidNum, out string unpaidMoney, out AMSManager.STRUTranInfoEx[] tranList)
		{
			return QueryTranReport(sSPID, sTranDate, out iResult, out strResInfo, out iTotalCount, out iPaidNum, out paidMoney, out iUnpaidNum, out unpaidMoney, out tranList, defaultContext__());
		}

		public bool QueryTranReport(string sSPID, string sTranDate, out int iResult, out string strResInfo, out int iTotalCount, out int iPaidNum, out string paidMoney, out int iUnpaidNum, out string unpaidMoney, out AMSManager.STRUTranInfoEx[] tranList, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QueryTranReport");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QueryTranReport(sSPID, sTranDate, out iResult, out strResInfo, out iTotalCount, out iPaidNum, out paidMoney, out iUnpaidNum, out unpaidMoney, out tranList, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public void QueryTransBankRollList(string spid, string listId, int offset, int limit, out int total, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo)
		{
			QueryTransBankRollList(spid, listId, offset, limit, out total, out rolls, out errCode, out errInfo, defaultContext__());
		}

		public void QueryTransBankRollList(string spid, string listId, int offset, int limit, out int total, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QueryTransBankRollList");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					del__.QueryTransBankRollList(spid, listId, offset, limit, out total, out rolls, out errCode, out errInfo, context__);
					return;
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool QueryTransInfoCount(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int lDownloadNum)
		{
			return QueryTransInfoCount(strUin, lRole, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, out strResinfo, out lDownloadNum, defaultContext__());
		}

		public bool QueryTransInfoCount(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int lDownloadNum, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QueryTransInfoCount");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QueryTransInfoCount(strUin, lRole, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, out strResinfo, out lDownloadNum, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool QueryUserOrderCount(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int OrderNum)
		{
			return QueryUserOrderCount(strUin, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, out strResinfo, out OrderNum, defaultContext__());
		}

		public bool QueryUserOrderCount(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int OrderNum, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("QueryUserOrderCount");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.QueryUserOrderCount(strUin, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, out strResinfo, out OrderNum, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public bool StatOrderInfo(string strSPID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUStatOrderInfo stStatOrderInfo)
		{
			return StatOrderInfo(strSPID, strStartTime, strEndTime, out iResult, out strResInfo, out stStatOrderInfo, defaultContext__());
		}

		public bool StatOrderInfo(string strSPID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUStatOrderInfo stStatOrderInfo, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("StatOrderInfo");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IAMSManagerDel_ del__ = (IAMSManagerDel_)delBase__;
					return del__.StatOrderInfo(strSPID, strStartTime, strEndTime, out iResult, out strResInfo, out stStatOrderInfo, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		#endregion

		#region Checked and unchecked cast operations

		public static IAMSManagerPrx checkedCast(Ice.ObjectPrx b)
		{
			if(b == null)
			{
				return null;
			}
			if(b is IAMSManagerPrx)
			{
				return (IAMSManagerPrx)b;
			}
			if(b.ice_isA("::AMSManager::IAMSManager"))
			{
				IAMSManagerPrxHelper h = new IAMSManagerPrxHelper();
				h.copyFrom__(b);
				return h;
			}
			return null;
		}

		public static IAMSManagerPrx checkedCast(Ice.ObjectPrx b, Ice.Context ctx)
		{
			if(b == null)
			{
				return null;
			}
			if(b is IAMSManagerPrx)
			{
				return (IAMSManagerPrx)b;
			}
			if(b.ice_isA("::AMSManager::IAMSManager", ctx))
			{
				IAMSManagerPrxHelper h = new IAMSManagerPrxHelper();
				h.copyFrom__(b);
				return h;
			}
			return null;
		}

		public static IAMSManagerPrx checkedCast(Ice.ObjectPrx b, string f)
		{
			if(b == null)
			{
				return null;
			}
			Ice.ObjectPrx bb = b.ice_newFacet(f);
			try
			{
				if(bb.ice_isA("::AMSManager::IAMSManager"))
				{
					IAMSManagerPrxHelper h = new IAMSManagerPrxHelper();
					h.copyFrom__(bb);
					return h;
				}
			}
			catch(Ice.FacetNotExistException)
			{
			}
			return null;
		}

		public static IAMSManagerPrx checkedCast(Ice.ObjectPrx b, string f, Ice.Context ctx)
		{
			if(b == null)
			{
				return null;
			}
			Ice.ObjectPrx bb = b.ice_newFacet(f);
			try
			{
				if(bb.ice_isA("::AMSManager::IAMSManager", ctx))
				{
					IAMSManagerPrxHelper h = new IAMSManagerPrxHelper();
					h.copyFrom__(bb);
					return h;
				}
			}
			catch(Ice.FacetNotExistException)
			{
			}
			return null;
		}

		public static IAMSManagerPrx uncheckedCast(Ice.ObjectPrx b)
		{
			if(b == null)
			{
				return null;
			}
			IAMSManagerPrxHelper h = new IAMSManagerPrxHelper();
			h.copyFrom__(b);
			return h;
		}

		public static IAMSManagerPrx uncheckedCast(Ice.ObjectPrx b, string f)
		{
			if(b == null)
			{
				return null;
			}
			Ice.ObjectPrx bb = b.ice_newFacet(f);
			IAMSManagerPrxHelper h = new IAMSManagerPrxHelper();
			h.copyFrom__(bb);
			return h;
		}

		#endregion

		#region Marshaling support

		protected override Ice.ObjectDelM_ createDelegateM__()
		{
			return new IAMSManagerDelM_();
		}

		protected override Ice.ObjectDelD_ createDelegateD__()
		{
			return new IAMSManagerDelD_();
		}

		public static void write__(IceInternal.BasicStream os__, IAMSManagerPrx v__)
		{
			os__.writeProxy(v__);
		}

		public static IAMSManagerPrx read__(IceInternal.BasicStream is__)
		{
			Ice.ObjectPrx proxy = is__.readProxy();
			if(proxy != null)
			{
				IAMSManagerPrxHelper result = new IAMSManagerPrxHelper();
				result.copyFrom__(proxy);
				return result;
			}
			return null;
		}

		#endregion
	}
}

namespace MediCS
{
	public sealed class OpLogListHelper
	{
		public static void write(IceInternal.BasicStream os__, MediCS.OperationLog[] v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Length);
				for(int ix__ = 0; ix__ < v__.Length; ++ix__)
				{
					v__[ix__].write__(os__);
				}
			}
		}

		public static MediCS.OperationLog[] read(IceInternal.BasicStream is__)
		{
			MediCS.OperationLog[] v__;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 27);
			v__ = new MediCS.OperationLog[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				v__[ix__] = new MediCS.OperationLog();
				v__[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			return v__;
		}
	}

	public sealed class CompareCondListHelper
	{
		public static void write(IceInternal.BasicStream os__, MediCS.CompareCond[] v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Length);
				for(int ix__ = 0; ix__ < v__.Length; ++ix__)
				{
					v__[ix__].write__(os__);
				}
			}
		}

		public static MediCS.CompareCond[] read(IceInternal.BasicStream is__)
		{
			MediCS.CompareCond[] v__;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 3);
			v__ = new MediCS.CompareCond[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				v__[ix__] = new MediCS.CompareCond();
				v__[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			return v__;
		}
	}

	public sealed class KVDictHelper
	{
		public static void write(IceInternal.BasicStream os__, KVDict v__)
		{
			if(v__ == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(v__.Count);
				foreach(_System.Collections.DictionaryEntry e__ in v__)
				{
					os__.writeString(((string)e__.Key));
					os__.writeString(((string)e__.Value));
				}
			}
		}

		public static KVDict read(IceInternal.BasicStream is__)
		{
			int sz__ = is__.readSize();
			KVDict r__ = new KVDict();
			for(int i__ = 0; i__ < sz__; ++i__)
			{
				string k__;
				k__ = is__.readString();
				string v__;
				v__ = is__.readString();
				r__[k__] = v__;
			}
			return r__;
		}
	}

	public sealed class IMediCustServicePrxHelper : Ice.ObjectPrxHelperBase, IMediCustServicePrx
	{
		#region Synchronous operations

		public long addOpLog(MediCS.OperationLog opLog)
		{
			return addOpLog(opLog, defaultContext__());
		}

		public long addOpLog(MediCS.OperationLog opLog, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("addOpLog");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IMediCustServiceDel_ del__ = (IMediCustServiceDel_)delBase__;
					return del__.addOpLog(opLog, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public int queryOpCount(MediCS.CompareCond[] condition)
		{
			return queryOpCount(condition, defaultContext__());
		}

		public int queryOpCount(MediCS.CompareCond[] condition, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("queryOpCount");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IMediCustServiceDel_ del__ = (IMediCustServiceDel_)delBase__;
					return del__.queryOpCount(condition, context__);
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public void queryOpLog(MediCS.CompareCond[] condition, int offset, int limit, out MediCS.OperationLog[] results)
		{
			queryOpLog(condition, offset, limit, out results, defaultContext__());
		}

		public void queryOpLog(MediCS.CompareCond[] condition, int offset, int limit, out MediCS.OperationLog[] results, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("queryOpLog");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IMediCustServiceDel_ del__ = (IMediCustServiceDel_)delBase__;
					del__.queryOpLog(condition, offset, limit, out results, context__);
					return;
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		public void updateAppealRecord(string transId, string appealId, MediCS.KVDict kvs)
		{
			updateAppealRecord(transId, appealId, kvs, defaultContext__());
		}

		public void updateAppealRecord(string transId, string appealId, MediCS.KVDict kvs, Ice.Context context__)
		{
			int cnt__ = 0;
			while(true)
			{
				try
				{
					checkTwowayOnly__("updateAppealRecord");
					Ice.ObjectDel_ delBase__ = getDelegate__();
					IMediCustServiceDel_ del__ = (IMediCustServiceDel_)delBase__;
					del__.updateAppealRecord(transId, appealId, kvs, context__);
					return;
				}
				catch(IceInternal.NonRepeatable ex__)
				{
					rethrowException__(ex__.get());
				}
				catch(Ice.LocalException ex__)
				{
					cnt__ = handleException__(ex__, cnt__);
				}
			}
		}

		#endregion

		#region Checked and unchecked cast operations

		public static IMediCustServicePrx checkedCast(Ice.ObjectPrx b)
		{
			if(b == null)
			{
				return null;
			}
			if(b is IMediCustServicePrx)
			{
				return (IMediCustServicePrx)b;
			}
			if(b.ice_isA("::MediCS::IMediCustService"))
			{
				IMediCustServicePrxHelper h = new IMediCustServicePrxHelper();
				h.copyFrom__(b);
				return h;
			}
			return null;
		}

		public static IMediCustServicePrx checkedCast(Ice.ObjectPrx b, Ice.Context ctx)
		{
			if(b == null)
			{
				return null;
			}
			if(b is IMediCustServicePrx)
			{
				return (IMediCustServicePrx)b;
			}
			if(b.ice_isA("::MediCS::IMediCustService", ctx))
			{
				IMediCustServicePrxHelper h = new IMediCustServicePrxHelper();
				h.copyFrom__(b);
				return h;
			}
			return null;
		}

		public static IMediCustServicePrx checkedCast(Ice.ObjectPrx b, string f)
		{
			if(b == null)
			{
				return null;
			}
			Ice.ObjectPrx bb = b.ice_newFacet(f);
			try
			{
				if(bb.ice_isA("::MediCS::IMediCustService"))
				{
					IMediCustServicePrxHelper h = new IMediCustServicePrxHelper();
					h.copyFrom__(bb);
					return h;
				}
			}
			catch(Ice.FacetNotExistException)
			{
			}
			return null;
		}

		public static IMediCustServicePrx checkedCast(Ice.ObjectPrx b, string f, Ice.Context ctx)
		{
			if(b == null)
			{
				return null;
			}
			Ice.ObjectPrx bb = b.ice_newFacet(f);
			try
			{
				if(bb.ice_isA("::MediCS::IMediCustService", ctx))
				{
					IMediCustServicePrxHelper h = new IMediCustServicePrxHelper();
					h.copyFrom__(bb);
					return h;
				}
			}
			catch(Ice.FacetNotExistException)
			{
			}
			return null;
		}

		public static IMediCustServicePrx uncheckedCast(Ice.ObjectPrx b)
		{
			if(b == null)
			{
				return null;
			}
			IMediCustServicePrxHelper h = new IMediCustServicePrxHelper();
			h.copyFrom__(b);
			return h;
		}

		public static IMediCustServicePrx uncheckedCast(Ice.ObjectPrx b, string f)
		{
			if(b == null)
			{
				return null;
			}
			Ice.ObjectPrx bb = b.ice_newFacet(f);
			IMediCustServicePrxHelper h = new IMediCustServicePrxHelper();
			h.copyFrom__(bb);
			return h;
		}

		#endregion

		#region Marshaling support

		protected override Ice.ObjectDelM_ createDelegateM__()
		{
			return new IMediCustServiceDelM_();
		}

		protected override Ice.ObjectDelD_ createDelegateD__()
		{
			return new IMediCustServiceDelD_();
		}

		public static void write__(IceInternal.BasicStream os__, IMediCustServicePrx v__)
		{
			os__.writeProxy(v__);
		}

		public static IMediCustServicePrx read__(IceInternal.BasicStream is__)
		{
			Ice.ObjectPrx proxy = is__.readProxy();
			if(proxy != null)
			{
				IMediCustServicePrxHelper result = new IMediCustServicePrxHelper();
				result.copyFrom__(proxy);
				return result;
			}
			return null;
		}

		#endregion
	}
}

namespace AMSManager
{
	public interface IAMSManagerDel_ : Ice.ObjectDel_
	{
		bool QuerySettlementDetail(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetail[] vecSettlementDetails, Ice.Context context__);

		bool QueryTranInfo(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo[] vecTranInfos, Ice.Context context__);

		bool StatOrderInfo(string strSPID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUStatOrderInfo stStatOrderInfo, Ice.Context context__);

		bool QueryTransInfoCount(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int lDownloadNum, Ice.Context context__);

		bool DownloadTransInfo(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUUserOrder[] UserOrderVec, Ice.Context context__);

		bool QueryUserOrderCount(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int OrderNum, Ice.Context context__);

		bool DownloadUserOrder(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUNewUserOrder[] UserOrderVec, Ice.Context context__);

		bool QueryAirTicketCount(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, out int lDownloadNum, out string sResinfo, Ice.Context context__);

		bool DownloadAirTicket(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, int offset, int limit, out int lDownloadNum, out string sResinfo, out AMSManager.STRUAirTicket[] AirTicketVec, Ice.Context context__);

		bool QueryBulletinInfo(int iSystemID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUBulletinDetail[] vecBulletinDetail, Ice.Context context__);

		string GetString(string strInput, Ice.Context context__);

		bool QueryTranInfoEx(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfoEx[] tranList, Ice.Context context__);

		int QueryAccountRollCount(string uin, string begTime, string endTime, out int errCode, out string errInfo, Ice.Context context__);

		void QueryAccountRollList(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__);

		void QueryAccountRollList2(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecord2[] rolls, out int errCode, out string errInfo, Ice.Context context__);

		bool QuerySettDetailExBySpid(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails, Ice.Context context__);

		bool QuerySettDetailExByAgentId(string agentId, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails, Ice.Context context__);

		bool QueryTranReport(string sSPID, string sTranDate, out int iResult, out string strResInfo, out int iTotalCount, out int iPaidNum, out string paidMoney, out int iUnpaidNum, out string unpaidMoney, out AMSManager.STRUTranInfoEx[] tranList, Ice.Context context__);

		int QueryAccountRollCountByCond(string uin, AMSManager.CompareCond[] conds, out int errCode, out string errInfo, Ice.Context context__);

		void QueryAccountRollListByCond(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo, Ice.Context context__);

		void DownloadAccountDetails(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__);

		int QuerySpAccountRollCount(string spid, string begTime, string endTime, out int errCode, out string errInfo, Ice.Context context__);

		void QuerySpAccountRollList(string spid, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__);

		bool QueryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos, Ice.Context context__);

		bool QueryHistoryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos, Ice.Context context__);

		void QueryTransBankRollList(string spid, string listId, int offset, int limit, out int total, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo, Ice.Context context__);

		bool QueryDrawDetail(string strSPID, string strMin, int iOffset, int iLimit, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUDrawDetail[] vecDrawDetails, Ice.Context context__);

		bool QueryOkSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, int iNo, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent, Ice.Context context__);

		bool QueryNoSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, out int iResult, out string strResInfo, out int iNoAllCount, out string iNoAllAmt, out int iNoTodayCount, out string iNoTodayAmt, out int iRefundCOunt, out string iRefundAmt, out int iNoHisCount, out string iNoHisAmt, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent, Ice.Context context__);

		int QuerySubAccountRollCount(int uid, int curType, int usrType, int rollType, string begTime, string endTime, out int errCode, out string errInfo, Ice.Context context__);

		void QuerySubAccountRollList(int uid, int curType, int usrType, int rollType, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__);

		bool QueryAirOrderBat(string strSpid, string strStime, string strEtime, int ioffset, int iLimit, out int iTotal, out int iReturnNum, out int iErrCode, out string strErrInfo, out AMSManager.STRAirOrderinfo[] SEQrolls, Ice.Context context__);
	}
}

namespace MediCS
{
	public interface IMediCustServiceDel_ : Ice.ObjectDel_
	{
		long addOpLog(MediCS.OperationLog opLog, Ice.Context context__);

		void queryOpLog(MediCS.CompareCond[] condition, int offset, int limit, out MediCS.OperationLog[] results, Ice.Context context__);

		int queryOpCount(MediCS.CompareCond[] condition, Ice.Context context__);

		void updateAppealRecord(string transId, string appealId, MediCS.KVDict kvs, Ice.Context context__);
	}
}

namespace AMSManager
{
	public sealed class IAMSManagerDelM_ : Ice.ObjectDelM_, IAMSManagerDel_
	{
		public void DownloadAccountDetails(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("DownloadAccountDetails", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(uin);
					if(conds == null)
					{
						os__.writeSize(0);
					}
					else
					{
						os__.writeSize(conds.Length);
						for(int ix__ = 0; ix__ < conds.Length; ++ix__)
						{
							conds[ix__].write__(os__);
						}
					}
					os__.writeInt(offset);
					os__.writeInt(limit);
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
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 30);
					rolls = new AMSManager.AccRollRecordEx[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						rolls[ix__] = new AMSManager.AccRollRecordEx();
						rolls[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					errCode = is__.readInt();
					errInfo = is__.readString();
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool DownloadAirTicket(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, int offset, int limit, out int lDownloadNum, out string sResinfo, out AMSManager.STRUAirTicket[] AirTicketVec, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("DownloadAirTicket", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strUin);
					os__.writeString(strStartTime);
					os__.writeString(strEndTime);
					os__.writeInt(lTransState);
					os__.writeInt(lAmountLowBound);
					os__.writeInt(lAmountHighBound);
					os__.writeString(strPeerUin);
					os__.writeInt(offset);
					os__.writeInt(limit);
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
					lDownloadNum = is__.readInt();
					sResinfo = is__.readString();
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 27);
					AirTicketVec = new AMSManager.STRUAirTicket[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						AirTicketVec[ix__] = new AMSManager.STRUAirTicket();
						AirTicketVec[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool DownloadTransInfo(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUUserOrder[] UserOrderVec, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("DownloadTransInfo", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strUin);
					os__.writeInt(lRole);
					os__.writeString(strStartTime);
					os__.writeString(strEndTime);
					os__.writeInt(lTransType);
					os__.writeString(strPeerUin);
					os__.writeInt(lTradeStatus);
					os__.writeInt(lAmountLowBound);
					os__.writeInt(lAmountHighBound);
					os__.writeInt(offset);
					os__.writeInt(limit);
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
					strResinfo = is__.readString();
					lDownloadNum = is__.readInt();
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 19);
					UserOrderVec = new AMSManager.STRUUserOrder[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						UserOrderVec[ix__] = new AMSManager.STRUUserOrder();
						UserOrderVec[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool DownloadUserOrder(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUNewUserOrder[] UserOrderVec, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("DownloadUserOrder", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strUin);
					os__.writeString(strStartTime);
					os__.writeString(strEndTime);
					os__.writeInt(lTransType);
					os__.writeString(strPeerUin);
					os__.writeInt(lTradeStatus);
					os__.writeInt(lAmountLowBound);
					os__.writeInt(lAmountHighBound);
					os__.writeInt(offset);
					os__.writeInt(limit);
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
					strResinfo = is__.readString();
					lDownloadNum = is__.readInt();
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 31);
					UserOrderVec = new AMSManager.STRUNewUserOrder[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						UserOrderVec[ix__] = new AMSManager.STRUNewUserOrder();
						UserOrderVec[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public string GetString(string strInput, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("GetString", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strInput);
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
					string ret__;
					ret__ = is__.readString();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public int QueryAccountRollCount(string uin, string begTime, string endTime, out int errCode, out string errInfo, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QueryAccountRollCount", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(uin);
					os__.writeString(begTime);
					os__.writeString(endTime);
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
					errCode = is__.readInt();
					errInfo = is__.readString();
					int ret__;
					ret__ = is__.readInt();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public int QueryAccountRollCountByCond(string uin, AMSManager.CompareCond[] conds, out int errCode, out string errInfo, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QueryAccountRollCountByCond", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(uin);
					if(conds == null)
					{
						os__.writeSize(0);
					}
					else
					{
						os__.writeSize(conds.Length);
						for(int ix__ = 0; ix__ < conds.Length; ++ix__)
						{
							conds[ix__].write__(os__);
						}
					}
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
					errCode = is__.readInt();
					errInfo = is__.readString();
					int ret__;
					ret__ = is__.readInt();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public void QueryAccountRollList(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QueryAccountRollList", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(uin);
					os__.writeString(begTime);
					os__.writeString(endTime);
					os__.writeInt(offset);
					os__.writeInt(limit);
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
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 30);
					rolls = new AMSManager.AccRollRecordEx[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						rolls[ix__] = new AMSManager.AccRollRecordEx();
						rolls[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					errCode = is__.readInt();
					errInfo = is__.readString();
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public void QueryAccountRollList2(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecord2[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QueryAccountRollList2", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(uin);
					os__.writeString(begTime);
					os__.writeString(endTime);
					os__.writeInt(offset);
					os__.writeInt(limit);
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
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 28);
					rolls = new AMSManager.AccRollRecord2[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						rolls[ix__] = new AMSManager.AccRollRecord2();
						rolls[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					errCode = is__.readInt();
					errInfo = is__.readString();
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public void QueryAccountRollListByCond(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QueryAccountRollListByCond", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(uin);
					if(conds == null)
					{
						os__.writeSize(0);
					}
					else
					{
						os__.writeSize(conds.Length);
						for(int ix__ = 0; ix__ < conds.Length; ++ix__)
						{
							conds[ix__].write__(os__);
						}
					}
					os__.writeInt(offset);
					os__.writeInt(limit);
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
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 27);
					rolls = new AMSManager.AccRollRecord[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						rolls[ix__] = new AMSManager.AccRollRecord();
						rolls[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					errCode = is__.readInt();
					errInfo = is__.readString();
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool QueryAirOrderBat(string strSpid, string strStime, string strEtime, int ioffset, int iLimit, out int iTotal, out int iReturnNum, out int iErrCode, out string strErrInfo, out AMSManager.STRAirOrderinfo[] SEQrolls, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QueryAirOrderBat", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strSpid);
					os__.writeString(strStime);
					os__.writeString(strEtime);
					os__.writeInt(ioffset);
					os__.writeInt(iLimit);
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
					iTotal = is__.readInt();
					iReturnNum = is__.readInt();
					iErrCode = is__.readInt();
					strErrInfo = is__.readString();
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 39);
					SEQrolls = new AMSManager.STRAirOrderinfo[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						SEQrolls[ix__] = new AMSManager.STRAirOrderinfo();
						SEQrolls[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool QueryAirTicketCount(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, out int lDownloadNum, out string sResinfo, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QueryAirTicketCount", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strUin);
					os__.writeString(strStartTime);
					os__.writeString(strEndTime);
					os__.writeInt(lTransState);
					os__.writeInt(lAmountLowBound);
					os__.writeInt(lAmountHighBound);
					os__.writeString(strPeerUin);
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
					lDownloadNum = is__.readInt();
					sResinfo = is__.readString();
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool QueryBulletinInfo(int iSystemID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUBulletinDetail[] vecBulletinDetail, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QueryBulletinInfo", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeInt(iSystemID);
					os__.writeString(strStartTime);
					os__.writeString(strEndTime);
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
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 8);
					vecBulletinDetail = new AMSManager.STRUBulletinDetail[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						vecBulletinDetail[ix__] = new AMSManager.STRUBulletinDetail();
						vecBulletinDetail[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool QueryDrawDetail(string strSPID, string strMin, int iOffset, int iLimit, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUDrawDetail[] vecDrawDetails, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QueryDrawDetail", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strSPID);
					os__.writeString(strMin);
					os__.writeInt(iOffset);
					os__.writeInt(iLimit);
					os__.writeString(strCreateTimeBegin);
					os__.writeString(strCreateTimeEnd);
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
					iTotalCount = is__.readInt();
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 12);
					vecDrawDetails = new AMSManager.STRUDrawDetail[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						vecDrawDetails[ix__] = new AMSManager.STRUDrawDetail();
						vecDrawDetails[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool QueryHistoryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QueryHistoryTranInfo2", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strSPID);
					os__.writeInt(iOffset);
					os__.writeInt(iLimit);
					os__.writeInt(iStatus);
					os__.writeString(strStartTime);
					os__.writeString(strEndTime);
					os__.writeInt(tmType);
					os__.writeString(strBankID);
					os__.writeInt(iStartFee);
					os__.writeInt(iEndFee);
					os__.writeInt(iFlag);
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
					iTotalCount = is__.readInt();
					iSuccNum = is__.readInt();
					succMoney = is__.readString();
					iUnsuccNum = is__.readInt();
					unsuccMoney = is__.readString();
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 23);
					vecTranInfos = new AMSManager.STRUTranInfo2[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						vecTranInfos[ix__] = new AMSManager.STRUTranInfo2();
						vecTranInfos[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool QueryNoSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, out int iResult, out string strResInfo, out int iNoAllCount, out string iNoAllAmt, out int iNoTodayCount, out string iNoTodayAmt, out int iRefundCOunt, out string iRefundAmt, out int iNoHisCount, out string iNoHisAmt, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QueryNoSettlementDetailAgent", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strSPID);
					os__.writeString(strMin);
					os__.writeInt(iOffset);
					os__.writeInt(iLimit);
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
					iNoAllCount = is__.readInt();
					iNoAllAmt = is__.readString();
					iNoTodayCount = is__.readInt();
					iNoTodayAmt = is__.readString();
					iRefundCOunt = is__.readInt();
					iRefundAmt = is__.readString();
					iNoHisCount = is__.readInt();
					iNoHisAmt = is__.readString();
					iTotalCount = is__.readInt();
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 21);
					vecSettlementDetailAgent = new AMSManager.STRUSettlementDetailAgent[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						vecSettlementDetailAgent[ix__] = new AMSManager.STRUSettlementDetailAgent();
						vecSettlementDetailAgent[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool QueryOkSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, int iNo, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QueryOkSettlementDetailAgent", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strSPID);
					os__.writeString(strMin);
					os__.writeInt(iOffset);
					os__.writeInt(iLimit);
					os__.writeInt(iNo);
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
					iTotalCount = is__.readInt();
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 21);
					vecSettlementDetailAgent = new AMSManager.STRUSettlementDetailAgent[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						vecSettlementDetailAgent[ix__] = new AMSManager.STRUSettlementDetailAgent();
						vecSettlementDetailAgent[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool QuerySettDetailExByAgentId(string agentId, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QuerySettDetailExByAgentId", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(agentId);
					os__.writeString(strMin);
					os__.writeInt(iOffset);
					os__.writeInt(iLimit);
					os__.writeInt(iStatus);
					os__.writeString(strCreateTimeBegin);
					os__.writeString(strCreateTimeEnd);
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
					iTotalCount = is__.readInt();
					mchamtAllsum = is__.readString();
					mchamtBlance = is__.readString();
					mchamtNonesum = is__.readString();
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 17);
					vecSettlementDetails = new AMSManager.STRUSettlementDetailEx[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						vecSettlementDetails[ix__] = new AMSManager.STRUSettlementDetailEx();
						vecSettlementDetails[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool QuerySettDetailExBySpid(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QuerySettDetailExBySpid", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strSPID);
					os__.writeString(strMin);
					os__.writeInt(iOffset);
					os__.writeInt(iLimit);
					os__.writeInt(iStatus);
					os__.writeString(strCreateTimeBegin);
					os__.writeString(strCreateTimeEnd);
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
					iTotalCount = is__.readInt();
					mchamtAllsum = is__.readString();
					mchamtBlance = is__.readString();
					mchamtNonesum = is__.readString();
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 17);
					vecSettlementDetails = new AMSManager.STRUSettlementDetailEx[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						vecSettlementDetails[ix__] = new AMSManager.STRUSettlementDetailEx();
						vecSettlementDetails[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool QuerySettlementDetail(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetail[] vecSettlementDetails, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QuerySettlementDetail", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strSPID);
					os__.writeString(strMin);
					os__.writeInt(iOffset);
					os__.writeInt(iLimit);
					os__.writeInt(iStatus);
					os__.writeString(strCreateTimeBegin);
					os__.writeString(strCreateTimeEnd);
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
					iTotalCount = is__.readInt();
					mchamtAllsum = is__.readString();
					mchamtBlance = is__.readString();
					mchamtNonesum = is__.readString();
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 16);
					vecSettlementDetails = new AMSManager.STRUSettlementDetail[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						vecSettlementDetails[ix__] = new AMSManager.STRUSettlementDetail();
						vecSettlementDetails[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public int QuerySpAccountRollCount(string spid, string begTime, string endTime, out int errCode, out string errInfo, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QuerySpAccountRollCount", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(spid);
					os__.writeString(begTime);
					os__.writeString(endTime);
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
					errCode = is__.readInt();
					errInfo = is__.readString();
					int ret__;
					ret__ = is__.readInt();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public void QuerySpAccountRollList(string spid, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QuerySpAccountRollList", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(spid);
					os__.writeString(begTime);
					os__.writeString(endTime);
					os__.writeInt(offset);
					os__.writeInt(limit);
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
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 30);
					rolls = new AMSManager.AccRollRecordEx[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						rolls[ix__] = new AMSManager.AccRollRecordEx();
						rolls[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					errCode = is__.readInt();
					errInfo = is__.readString();
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public int QuerySubAccountRollCount(int uid, int curType, int usrType, int rollType, string begTime, string endTime, out int errCode, out string errInfo, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QuerySubAccountRollCount", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeInt(uid);
					os__.writeInt(curType);
					os__.writeInt(usrType);
					os__.writeInt(rollType);
					os__.writeString(begTime);
					os__.writeString(endTime);
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
					errCode = is__.readInt();
					errInfo = is__.readString();
					int ret__;
					ret__ = is__.readInt();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public void QuerySubAccountRollList(int uid, int curType, int usrType, int rollType, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QuerySubAccountRollList", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeInt(uid);
					os__.writeInt(curType);
					os__.writeInt(usrType);
					os__.writeInt(rollType);
					os__.writeString(begTime);
					os__.writeString(endTime);
					os__.writeInt(offset);
					os__.writeInt(limit);
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
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 30);
					rolls = new AMSManager.AccRollRecordEx[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						rolls[ix__] = new AMSManager.AccRollRecordEx();
						rolls[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					errCode = is__.readInt();
					errInfo = is__.readString();
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool QueryTranInfo(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo[] vecTranInfos, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QueryTranInfo", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strSPID);
					os__.writeInt(iOffset);
					os__.writeInt(iLimit);
					os__.writeInt(iStatus);
					os__.writeString(strStartTime);
					os__.writeString(strEndTime);
					os__.writeString(strBankID);
					os__.writeInt(iStartFee);
					os__.writeInt(iEndFee);
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
					iTotalCount = is__.readInt();
					iSuccNum = is__.readInt();
					succMoney = is__.readString();
					iUnsuccNum = is__.readInt();
					unsuccMoney = is__.readString();
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 22);
					vecTranInfos = new AMSManager.STRUTranInfo[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						vecTranInfos[ix__] = new AMSManager.STRUTranInfo();
						vecTranInfos[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool QueryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QueryTranInfo2", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strSPID);
					os__.writeInt(iOffset);
					os__.writeInt(iLimit);
					os__.writeInt(iStatus);
					os__.writeString(strStartTime);
					os__.writeString(strEndTime);
					os__.writeInt(tmType);
					os__.writeString(strBankID);
					os__.writeInt(iStartFee);
					os__.writeInt(iEndFee);
					os__.writeInt(iFlag);
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
					iTotalCount = is__.readInt();
					iSuccNum = is__.readInt();
					succMoney = is__.readString();
					iUnsuccNum = is__.readInt();
					unsuccMoney = is__.readString();
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 23);
					vecTranInfos = new AMSManager.STRUTranInfo2[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						vecTranInfos[ix__] = new AMSManager.STRUTranInfo2();
						vecTranInfos[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool QueryTranInfoEx(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfoEx[] tranList, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QueryTranInfoEx", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strSPID);
					os__.writeInt(iOffset);
					os__.writeInt(iLimit);
					os__.writeInt(iStatus);
					os__.writeString(strStartTime);
					os__.writeString(strEndTime);
					os__.writeString(strBankID);
					os__.writeInt(iStartFee);
					os__.writeInt(iEndFee);
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
					iTotalCount = is__.readInt();
					iSuccNum = is__.readInt();
					succMoney = is__.readString();
					iUnsuccNum = is__.readInt();
					unsuccMoney = is__.readString();
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 50);
					tranList = new AMSManager.STRUTranInfoEx[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						tranList[ix__] = new AMSManager.STRUTranInfoEx();
						tranList[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool QueryTranReport(string sSPID, string sTranDate, out int iResult, out string strResInfo, out int iTotalCount, out int iPaidNum, out string paidMoney, out int iUnpaidNum, out string unpaidMoney, out AMSManager.STRUTranInfoEx[] tranList, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QueryTranReport", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(sSPID);
					os__.writeString(sTranDate);
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
					iTotalCount = is__.readInt();
					iPaidNum = is__.readInt();
					paidMoney = is__.readString();
					iUnpaidNum = is__.readInt();
					unpaidMoney = is__.readString();
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 50);
					tranList = new AMSManager.STRUTranInfoEx[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						tranList[ix__] = new AMSManager.STRUTranInfoEx();
						tranList[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public void QueryTransBankRollList(string spid, string listId, int offset, int limit, out int total, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QueryTransBankRollList", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(spid);
					os__.writeString(listId);
					os__.writeInt(offset);
					os__.writeInt(limit);
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
					total = is__.readInt();
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 27);
					rolls = new AMSManager.AccRollRecord[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						rolls[ix__] = new AMSManager.AccRollRecord();
						rolls[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
					errCode = is__.readInt();
					errInfo = "";//is__.readString();
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool QueryTransInfoCount(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int lDownloadNum, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QueryTransInfoCount", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strUin);
					os__.writeInt(lRole);
					os__.writeString(strStartTime);
					os__.writeString(strEndTime);
					os__.writeInt(lTransType);
					os__.writeString(strPeerUin);
					os__.writeInt(lTradeStatus);
					os__.writeInt(lAmountLowBound);
					os__.writeInt(lAmountHighBound);
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
					strResinfo = is__.readString();
					lDownloadNum = is__.readInt();
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool QueryUserOrderCount(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int OrderNum, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("QueryUserOrderCount", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strUin);
					os__.writeString(strStartTime);
					os__.writeString(strEndTime);
					os__.writeInt(lTransType);
					os__.writeString(strPeerUin);
					os__.writeInt(lTradeStatus);
					os__.writeInt(lAmountLowBound);
					os__.writeInt(lAmountHighBound);
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
					strResinfo = is__.readString();
					OrderNum = is__.readInt();
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public bool StatOrderInfo(string strSPID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUStatOrderInfo stStatOrderInfo, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("StatOrderInfo", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(strSPID);
					os__.writeString(strStartTime);
					os__.writeString(strEndTime);
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
					stStatOrderInfo = new AMSManager.STRUStatOrderInfo();
					stStatOrderInfo.read__(is__);
					bool ret__;
					ret__ = is__.readBool();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}
	}
}

namespace MediCS
{
	public sealed class IMediCustServiceDelM_ : Ice.ObjectDelM_, IMediCustServiceDel_
	{
		public long addOpLog(MediCS.OperationLog opLog, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("addOpLog", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					opLog.write__(os__);
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
						catch(MediCS.ErrorBase)
						{
							throw;
						}
						catch(Ice.UserException ex)
						{
							throw new Ice.UnknownUserException(ex);
						}
					}
					long ret__;
					ret__ = is__.readLong();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public int queryOpCount(MediCS.CompareCond[] condition, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("queryOpCount", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					if(condition == null)
					{
						os__.writeSize(0);
					}
					else
					{
						os__.writeSize(condition.Length);
						for(int ix__ = 0; ix__ < condition.Length; ++ix__)
						{
							condition[ix__].write__(os__);
						}
					}
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
						catch(MediCS.ErrorBase)
						{
							throw;
						}
						catch(Ice.UserException ex)
						{
							throw new Ice.UnknownUserException(ex);
						}
					}
					int ret__;
					ret__ = is__.readInt();
					return ret__;
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public void queryOpLog(MediCS.CompareCond[] condition, int offset, int limit, out MediCS.OperationLog[] results, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("queryOpLog", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					if(condition == null)
					{
						os__.writeSize(0);
					}
					else
					{
						os__.writeSize(condition.Length);
						for(int ix__ = 0; ix__ < condition.Length; ++ix__)
						{
							condition[ix__].write__(os__);
						}
					}
					os__.writeInt(offset);
					os__.writeInt(limit);
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
						catch(MediCS.ErrorBase)
						{
							throw;
						}
						catch(Ice.UserException ex)
						{
							throw new Ice.UnknownUserException(ex);
						}
					}
				{
					int szx__ = is__.readSize();
					is__.startSeq(szx__, 27);
					results = new MediCS.OperationLog[szx__];
					for(int ix__ = 0; ix__ < szx__; ++ix__)
					{
						results[ix__] = new MediCS.OperationLog();
						results[ix__].read__(is__);
						is__.checkSeq();
						is__.endElement();
					}
					is__.endSeq(szx__);
				}
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}

		public void updateAppealRecord(string transId, string appealId, MediCS.KVDict kvs, Ice.Context context__)
		{
			IceInternal.Outgoing og__ = getOutgoing("updateAppealRecord", Ice.OperationMode.Normal, context__);
			try
			{
				try
				{
					IceInternal.BasicStream os__ = og__.ostr();
					os__.writeString(transId);
					os__.writeString(appealId);
					MediCS.KVDictHelper.write(os__, kvs);
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
						catch(MediCS.ErrorBase)
						{
							throw;
						}
						catch(Ice.UserException ex)
						{
							throw new Ice.UnknownUserException(ex);
						}
					}
				}
				catch(Ice.LocalException ex__)
				{
					throw new IceInternal.NonRepeatable(ex__);
				}
			}
			finally
			{
				reclaimOutgoing(og__);
			}
		}
	}
}

namespace AMSManager
{
	public sealed class IAMSManagerDelD_ : Ice.ObjectDelD_, IAMSManagerDel_
	{
		public void DownloadAccountDetails(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "DownloadAccountDetails", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						((AMSManager.IAMSManager)servant__).DownloadAccountDetails(uin, conds, offset, limit, out rolls, out errCode, out errInfo, current__);
						return;
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool DownloadAirTicket(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, int offset, int limit, out int lDownloadNum, out string sResinfo, out AMSManager.STRUAirTicket[] AirTicketVec, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "DownloadAirTicket", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).DownloadAirTicket(strUin, strStartTime, strEndTime, lTransState, lAmountLowBound, lAmountHighBound, strPeerUin, offset, limit, out lDownloadNum, out sResinfo, out AirTicketVec, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool DownloadTransInfo(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUUserOrder[] UserOrderVec, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "DownloadTransInfo", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).DownloadTransInfo(strUin, lRole, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, offset, limit, out strResinfo, out lDownloadNum, out UserOrderVec, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool DownloadUserOrder(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUNewUserOrder[] UserOrderVec, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "DownloadUserOrder", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).DownloadUserOrder(strUin, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, offset, limit, out strResinfo, out lDownloadNum, out UserOrderVec, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public string GetString(string strInput, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "GetString", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).GetString(strInput, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public int QueryAccountRollCount(string uin, string begTime, string endTime, out int errCode, out string errInfo, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QueryAccountRollCount", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QueryAccountRollCount(uin, begTime, endTime, out errCode, out errInfo, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public int QueryAccountRollCountByCond(string uin, AMSManager.CompareCond[] conds, out int errCode, out string errInfo, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QueryAccountRollCountByCond", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QueryAccountRollCountByCond(uin, conds, out errCode, out errInfo, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public void QueryAccountRollList(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QueryAccountRollList", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						((AMSManager.IAMSManager)servant__).QueryAccountRollList(uin, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, current__);
						return;
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public void QueryAccountRollList2(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecord2[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QueryAccountRollList2", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						((AMSManager.IAMSManager)servant__).QueryAccountRollList2(uin, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, current__);
						return;
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public void QueryAccountRollListByCond(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QueryAccountRollListByCond", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						((AMSManager.IAMSManager)servant__).QueryAccountRollListByCond(uin, conds, offset, limit, out rolls, out errCode, out errInfo, current__);
						return;
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool QueryAirOrderBat(string strSpid, string strStime, string strEtime, int ioffset, int iLimit, out int iTotal, out int iReturnNum, out int iErrCode, out string strErrInfo, out AMSManager.STRAirOrderinfo[] SEQrolls, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QueryAirOrderBat", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QueryAirOrderBat(strSpid, strStime, strEtime, ioffset, iLimit, out iTotal, out iReturnNum, out iErrCode, out strErrInfo, out SEQrolls, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool QueryAirTicketCount(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, out int lDownloadNum, out string sResinfo, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QueryAirTicketCount", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QueryAirTicketCount(strUin, strStartTime, strEndTime, lTransState, lAmountLowBound, lAmountHighBound, strPeerUin, out lDownloadNum, out sResinfo, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool QueryBulletinInfo(int iSystemID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUBulletinDetail[] vecBulletinDetail, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QueryBulletinInfo", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QueryBulletinInfo(iSystemID, strStartTime, strEndTime, out iResult, out strResInfo, out vecBulletinDetail, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool QueryDrawDetail(string strSPID, string strMin, int iOffset, int iLimit, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUDrawDetail[] vecDrawDetails, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QueryDrawDetail", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QueryDrawDetail(strSPID, strMin, iOffset, iLimit, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out vecDrawDetails, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool QueryHistoryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QueryHistoryTranInfo2", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QueryHistoryTranInfo2(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, tmType, strBankID, iStartFee, iEndFee, iFlag, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out vecTranInfos, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool QueryNoSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, out int iResult, out string strResInfo, out int iNoAllCount, out string iNoAllAmt, out int iNoTodayCount, out string iNoTodayAmt, out int iRefundCOunt, out string iRefundAmt, out int iNoHisCount, out string iNoHisAmt, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QueryNoSettlementDetailAgent", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QueryNoSettlementDetailAgent(strSPID, strMin, iOffset, iLimit, out iResult, out strResInfo, out iNoAllCount, out iNoAllAmt, out iNoTodayCount, out iNoTodayAmt, out iRefundCOunt, out iRefundAmt, out iNoHisCount, out iNoHisAmt, out iTotalCount, out vecSettlementDetailAgent, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool QueryOkSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, int iNo, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QueryOkSettlementDetailAgent", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QueryOkSettlementDetailAgent(strSPID, strMin, iOffset, iLimit, iNo, out iResult, out strResInfo, out iTotalCount, out vecSettlementDetailAgent, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool QuerySettDetailExByAgentId(string agentId, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QuerySettDetailExByAgentId", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QuerySettDetailExByAgentId(agentId, strMin, iOffset, iLimit, iStatus, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out mchamtAllsum, out mchamtBlance, out mchamtNonesum, out vecSettlementDetails, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool QuerySettDetailExBySpid(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QuerySettDetailExBySpid", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QuerySettDetailExBySpid(strSPID, strMin, iOffset, iLimit, iStatus, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out mchamtAllsum, out mchamtBlance, out mchamtNonesum, out vecSettlementDetails, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool QuerySettlementDetail(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetail[] vecSettlementDetails, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QuerySettlementDetail", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QuerySettlementDetail(strSPID, strMin, iOffset, iLimit, iStatus, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out mchamtAllsum, out mchamtBlance, out mchamtNonesum, out vecSettlementDetails, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public int QuerySpAccountRollCount(string spid, string begTime, string endTime, out int errCode, out string errInfo, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QuerySpAccountRollCount", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QuerySpAccountRollCount(spid, begTime, endTime, out errCode, out errInfo, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public void QuerySpAccountRollList(string spid, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QuerySpAccountRollList", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						((AMSManager.IAMSManager)servant__).QuerySpAccountRollList(spid, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, current__);
						return;
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public int QuerySubAccountRollCount(int uid, int curType, int usrType, int rollType, string begTime, string endTime, out int errCode, out string errInfo, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QuerySubAccountRollCount", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QuerySubAccountRollCount(uid, curType, usrType, rollType, begTime, endTime, out errCode, out errInfo, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public void QuerySubAccountRollList(int uid, int curType, int usrType, int rollType, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QuerySubAccountRollList", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						((AMSManager.IAMSManager)servant__).QuerySubAccountRollList(uid, curType, usrType, rollType, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, current__);
						return;
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool QueryTranInfo(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo[] vecTranInfos, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QueryTranInfo", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QueryTranInfo(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, strBankID, iStartFee, iEndFee, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out vecTranInfos, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool QueryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QueryTranInfo2", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QueryTranInfo2(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, tmType, strBankID, iStartFee, iEndFee, iFlag, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out vecTranInfos, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool QueryTranInfoEx(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfoEx[] tranList, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QueryTranInfoEx", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QueryTranInfoEx(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, strBankID, iStartFee, iEndFee, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out tranList, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool QueryTranReport(string sSPID, string sTranDate, out int iResult, out string strResInfo, out int iTotalCount, out int iPaidNum, out string paidMoney, out int iUnpaidNum, out string unpaidMoney, out AMSManager.STRUTranInfoEx[] tranList, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QueryTranReport", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QueryTranReport(sSPID, sTranDate, out iResult, out strResInfo, out iTotalCount, out iPaidNum, out paidMoney, out iUnpaidNum, out unpaidMoney, out tranList, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public void QueryTransBankRollList(string spid, string listId, int offset, int limit, out int total, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QueryTransBankRollList", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						((AMSManager.IAMSManager)servant__).QueryTransBankRollList(spid, listId, offset, limit, out total, out rolls, out errCode, out errInfo, current__);
						return;
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool QueryTransInfoCount(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int lDownloadNum, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QueryTransInfoCount", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QueryTransInfoCount(strUin, lRole, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, out strResinfo, out lDownloadNum, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool QueryUserOrderCount(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int OrderNum, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "QueryUserOrderCount", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).QueryUserOrderCount(strUin, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, out strResinfo, out OrderNum, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public bool StatOrderInfo(string strSPID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUStatOrderInfo stStatOrderInfo, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "StatOrderInfo", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IAMSManager)
				{
					try
					{
						return ((AMSManager.IAMSManager)servant__).StatOrderInfo(strSPID, strStartTime, strEndTime, out iResult, out strResInfo, out stStatOrderInfo, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

namespace MediCS
{
	public sealed class IMediCustServiceDelD_ : Ice.ObjectDelD_, IMediCustServiceDel_
	{
		public long addOpLog(MediCS.OperationLog opLog, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "addOpLog", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IMediCustService)
				{
					try
					{
						return ((MediCS.IMediCustService)servant__).addOpLog(opLog, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public int queryOpCount(MediCS.CompareCond[] condition, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "queryOpCount", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IMediCustService)
				{
					try
					{
						return ((MediCS.IMediCustService)servant__).queryOpCount(condition, current__);
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public void queryOpLog(MediCS.CompareCond[] condition, int offset, int limit, out MediCS.OperationLog[] results, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "queryOpLog", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IMediCustService)
				{
					try
					{
						((MediCS.IMediCustService)servant__).queryOpLog(condition, offset, limit, out results, current__);
						return;
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

		public void updateAppealRecord(string transId, string appealId, MediCS.KVDict kvs, Ice.Context context__)
		{
			Ice.Current current__ = new Ice.Current();
			initCurrent__(ref current__, "updateAppealRecord", Ice.OperationMode.Normal, context__);
			while(true)
			{
				IceInternal.Direct direct__ = new IceInternal.Direct(current__);
				object servant__ = direct__.servant();
				if(servant__ is IMediCustService)
				{
					try
					{
						((MediCS.IMediCustService)servant__).updateAppealRecord(transId, appealId, kvs, current__);
						return;
					}
					catch(Ice.LocalException ex__)
					{
						throw new IceInternal.NonRepeatable(ex__);
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

namespace AMSManager
{
	public abstract class IAMSManagerDisp_ : Ice.ObjectImpl, IAMSManager
	{
		#region Slice operations

		public bool QuerySettlementDetail(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetail[] vecSettlementDetails)
		{
			return QuerySettlementDetail(strSPID, strMin, iOffset, iLimit, iStatus, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out mchamtAllsum, out mchamtBlance, out mchamtNonesum, out vecSettlementDetails, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool QuerySettlementDetail(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetail[] vecSettlementDetails, Ice.Current current__);

		public bool QueryTranInfo(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo[] vecTranInfos)
		{
			return QueryTranInfo(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, strBankID, iStartFee, iEndFee, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out vecTranInfos, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool QueryTranInfo(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo[] vecTranInfos, Ice.Current current__);

		public bool StatOrderInfo(string strSPID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUStatOrderInfo stStatOrderInfo)
		{
			return StatOrderInfo(strSPID, strStartTime, strEndTime, out iResult, out strResInfo, out stStatOrderInfo, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool StatOrderInfo(string strSPID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUStatOrderInfo stStatOrderInfo, Ice.Current current__);

		public bool QueryTransInfoCount(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int lDownloadNum)
		{
			return QueryTransInfoCount(strUin, lRole, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, out strResinfo, out lDownloadNum, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool QueryTransInfoCount(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int lDownloadNum, Ice.Current current__);

		public bool DownloadTransInfo(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUUserOrder[] UserOrderVec)
		{
			return DownloadTransInfo(strUin, lRole, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, offset, limit, out strResinfo, out lDownloadNum, out UserOrderVec, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool DownloadTransInfo(string strUin, int lRole, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUUserOrder[] UserOrderVec, Ice.Current current__);

		public bool QueryUserOrderCount(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int OrderNum)
		{
			return QueryUserOrderCount(strUin, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, out strResinfo, out OrderNum, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool QueryUserOrderCount(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, out string strResinfo, out int OrderNum, Ice.Current current__);

		public bool DownloadUserOrder(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUNewUserOrder[] UserOrderVec)
		{
			return DownloadUserOrder(strUin, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, offset, limit, out strResinfo, out lDownloadNum, out UserOrderVec, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool DownloadUserOrder(string strUin, string strStartTime, string strEndTime, int lTransType, string strPeerUin, int lTradeStatus, int lAmountLowBound, int lAmountHighBound, int offset, int limit, out string strResinfo, out int lDownloadNum, out AMSManager.STRUNewUserOrder[] UserOrderVec, Ice.Current current__);

		public bool QueryAirTicketCount(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, out int lDownloadNum, out string sResinfo)
		{
			return QueryAirTicketCount(strUin, strStartTime, strEndTime, lTransState, lAmountLowBound, lAmountHighBound, strPeerUin, out lDownloadNum, out sResinfo, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool QueryAirTicketCount(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, out int lDownloadNum, out string sResinfo, Ice.Current current__);

		public bool DownloadAirTicket(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, int offset, int limit, out int lDownloadNum, out string sResinfo, out AMSManager.STRUAirTicket[] AirTicketVec)
		{
			return DownloadAirTicket(strUin, strStartTime, strEndTime, lTransState, lAmountLowBound, lAmountHighBound, strPeerUin, offset, limit, out lDownloadNum, out sResinfo, out AirTicketVec, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool DownloadAirTicket(string strUin, string strStartTime, string strEndTime, int lTransState, int lAmountLowBound, int lAmountHighBound, string strPeerUin, int offset, int limit, out int lDownloadNum, out string sResinfo, out AMSManager.STRUAirTicket[] AirTicketVec, Ice.Current current__);

		public bool QueryBulletinInfo(int iSystemID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUBulletinDetail[] vecBulletinDetail)
		{
			return QueryBulletinInfo(iSystemID, strStartTime, strEndTime, out iResult, out strResInfo, out vecBulletinDetail, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool QueryBulletinInfo(int iSystemID, string strStartTime, string strEndTime, out int iResult, out string strResInfo, out AMSManager.STRUBulletinDetail[] vecBulletinDetail, Ice.Current current__);

		public string GetString(string strInput)
		{
			return GetString(strInput, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract string GetString(string strInput, Ice.Current current__);

		public bool QueryTranInfoEx(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfoEx[] tranList)
		{
			return QueryTranInfoEx(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, strBankID, iStartFee, iEndFee, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out tranList, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool QueryTranInfoEx(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, string strBankID, int iStartFee, int iEndFee, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfoEx[] tranList, Ice.Current current__);

		public int QueryAccountRollCount(string uin, string begTime, string endTime, out int errCode, out string errInfo)
		{
			return QueryAccountRollCount(uin, begTime, endTime, out errCode, out errInfo, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract int QueryAccountRollCount(string uin, string begTime, string endTime, out int errCode, out string errInfo, Ice.Current current__);

		public void QueryAccountRollList(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo)
		{
			QueryAccountRollList(uin, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract void QueryAccountRollList(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Current current__);

		public void QueryAccountRollList2(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecord2[] rolls, out int errCode, out string errInfo)
		{
			QueryAccountRollList2(uin, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract void QueryAccountRollList2(string uin, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecord2[] rolls, out int errCode, out string errInfo, Ice.Current current__);

		public bool QuerySettDetailExBySpid(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails)
		{
			return QuerySettDetailExBySpid(strSPID, strMin, iOffset, iLimit, iStatus, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out mchamtAllsum, out mchamtBlance, out mchamtNonesum, out vecSettlementDetails, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool QuerySettDetailExBySpid(string strSPID, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails, Ice.Current current__);

		public bool QuerySettDetailExByAgentId(string agentId, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails)
		{
			return QuerySettDetailExByAgentId(agentId, strMin, iOffset, iLimit, iStatus, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out mchamtAllsum, out mchamtBlance, out mchamtNonesum, out vecSettlementDetails, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool QuerySettDetailExByAgentId(string agentId, string strMin, int iOffset, int iLimit, int iStatus, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out string mchamtAllsum, out string mchamtBlance, out string mchamtNonesum, out AMSManager.STRUSettlementDetailEx[] vecSettlementDetails, Ice.Current current__);

		public bool QueryTranReport(string sSPID, string sTranDate, out int iResult, out string strResInfo, out int iTotalCount, out int iPaidNum, out string paidMoney, out int iUnpaidNum, out string unpaidMoney, out AMSManager.STRUTranInfoEx[] tranList)
		{
			return QueryTranReport(sSPID, sTranDate, out iResult, out strResInfo, out iTotalCount, out iPaidNum, out paidMoney, out iUnpaidNum, out unpaidMoney, out tranList, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool QueryTranReport(string sSPID, string sTranDate, out int iResult, out string strResInfo, out int iTotalCount, out int iPaidNum, out string paidMoney, out int iUnpaidNum, out string unpaidMoney, out AMSManager.STRUTranInfoEx[] tranList, Ice.Current current__);

		public int QueryAccountRollCountByCond(string uin, AMSManager.CompareCond[] conds, out int errCode, out string errInfo)
		{
			return QueryAccountRollCountByCond(uin, conds, out errCode, out errInfo, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract int QueryAccountRollCountByCond(string uin, AMSManager.CompareCond[] conds, out int errCode, out string errInfo, Ice.Current current__);

		public void QueryAccountRollListByCond(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo)
		{
			QueryAccountRollListByCond(uin, conds, offset, limit, out rolls, out errCode, out errInfo, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract void QueryAccountRollListByCond(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo, Ice.Current current__);

		public void DownloadAccountDetails(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo)
		{
			DownloadAccountDetails(uin, conds, offset, limit, out rolls, out errCode, out errInfo, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract void DownloadAccountDetails(string uin, AMSManager.CompareCond[] conds, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Current current__);

		public int QuerySpAccountRollCount(string spid, string begTime, string endTime, out int errCode, out string errInfo)
		{
			return QuerySpAccountRollCount(spid, begTime, endTime, out errCode, out errInfo, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract int QuerySpAccountRollCount(string spid, string begTime, string endTime, out int errCode, out string errInfo, Ice.Current current__);

		public void QuerySpAccountRollList(string spid, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo)
		{
			QuerySpAccountRollList(spid, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract void QuerySpAccountRollList(string spid, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Current current__);

		public bool QueryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos)
		{
			return QueryTranInfo2(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, tmType, strBankID, iStartFee, iEndFee, iFlag, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out vecTranInfos, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool QueryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos, Ice.Current current__);

		public bool QueryHistoryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos)
		{
			return QueryHistoryTranInfo2(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, tmType, strBankID, iStartFee, iEndFee, iFlag, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out vecTranInfos, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool QueryHistoryTranInfo2(string strSPID, int iOffset, int iLimit, int iStatus, string strStartTime, string strEndTime, int tmType, string strBankID, int iStartFee, int iEndFee, int iFlag, out int iResult, out string strResInfo, out int iTotalCount, out int iSuccNum, out string succMoney, out int iUnsuccNum, out string unsuccMoney, out AMSManager.STRUTranInfo2[] vecTranInfos, Ice.Current current__);

		public void QueryTransBankRollList(string spid, string listId, int offset, int limit, out int total, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo)
		{
			QueryTransBankRollList(spid, listId, offset, limit, out total, out rolls, out errCode, out errInfo, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract void QueryTransBankRollList(string spid, string listId, int offset, int limit, out int total, out AMSManager.AccRollRecord[] rolls, out int errCode, out string errInfo, Ice.Current current__);

		public bool QueryDrawDetail(string strSPID, string strMin, int iOffset, int iLimit, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUDrawDetail[] vecDrawDetails)
		{
			return QueryDrawDetail(strSPID, strMin, iOffset, iLimit, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out vecDrawDetails, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool QueryDrawDetail(string strSPID, string strMin, int iOffset, int iLimit, string strCreateTimeBegin, string strCreateTimeEnd, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUDrawDetail[] vecDrawDetails, Ice.Current current__);

		public bool QueryOkSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, int iNo, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent)
		{
			return QueryOkSettlementDetailAgent(strSPID, strMin, iOffset, iLimit, iNo, out iResult, out strResInfo, out iTotalCount, out vecSettlementDetailAgent, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool QueryOkSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, int iNo, out int iResult, out string strResInfo, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent, Ice.Current current__);

		public bool QueryNoSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, out int iResult, out string strResInfo, out int iNoAllCount, out string iNoAllAmt, out int iNoTodayCount, out string iNoTodayAmt, out int iRefundCOunt, out string iRefundAmt, out int iNoHisCount, out string iNoHisAmt, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent)
		{
			return QueryNoSettlementDetailAgent(strSPID, strMin, iOffset, iLimit, out iResult, out strResInfo, out iNoAllCount, out iNoAllAmt, out iNoTodayCount, out iNoTodayAmt, out iRefundCOunt, out iRefundAmt, out iNoHisCount, out iNoHisAmt, out iTotalCount, out vecSettlementDetailAgent, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool QueryNoSettlementDetailAgent(string strSPID, string strMin, int iOffset, int iLimit, out int iResult, out string strResInfo, out int iNoAllCount, out string iNoAllAmt, out int iNoTodayCount, out string iNoTodayAmt, out int iRefundCOunt, out string iRefundAmt, out int iNoHisCount, out string iNoHisAmt, out int iTotalCount, out AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent, Ice.Current current__);

		public int QuerySubAccountRollCount(int uid, int curType, int usrType, int rollType, string begTime, string endTime, out int errCode, out string errInfo)
		{
			return QuerySubAccountRollCount(uid, curType, usrType, rollType, begTime, endTime, out errCode, out errInfo, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract int QuerySubAccountRollCount(int uid, int curType, int usrType, int rollType, string begTime, string endTime, out int errCode, out string errInfo, Ice.Current current__);

		public void QuerySubAccountRollList(int uid, int curType, int usrType, int rollType, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo)
		{
			QuerySubAccountRollList(uid, curType, usrType, rollType, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract void QuerySubAccountRollList(int uid, int curType, int usrType, int rollType, string begTime, string endTime, int offset, int limit, out AMSManager.AccRollRecordEx[] rolls, out int errCode, out string errInfo, Ice.Current current__);

		public bool QueryAirOrderBat(string strSpid, string strStime, string strEtime, int ioffset, int iLimit, out int iTotal, out int iReturnNum, out int iErrCode, out string strErrInfo, out AMSManager.STRAirOrderinfo[] SEQrolls)
		{
			return QueryAirOrderBat(strSpid, strStime, strEtime, ioffset, iLimit, out iTotal, out iReturnNum, out iErrCode, out strErrInfo, out SEQrolls, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract bool QueryAirOrderBat(string strSpid, string strStime, string strEtime, int ioffset, int iLimit, out int iTotal, out int iReturnNum, out int iErrCode, out string strErrInfo, out AMSManager.STRAirOrderinfo[] SEQrolls, Ice.Current current__);

		#endregion

		#region Slice type-related members

		public static new string[] ids__ = 
	{
		"::AMSManager::IAMSManager",
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

		public static IceInternal.DispatchStatus QuerySettlementDetail___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strSPID;
			strSPID = is__.readString();
			string strMin;
			strMin = is__.readString();
			int iOffset;
			iOffset = is__.readInt();
			int iLimit;
			iLimit = is__.readInt();
			int iStatus;
			iStatus = is__.readInt();
			string strCreateTimeBegin;
			strCreateTimeBegin = is__.readString();
			string strCreateTimeEnd;
			strCreateTimeEnd = is__.readString();
			int iResult;
			string strResInfo;
			int iTotalCount;
			string mchamtAllsum;
			string mchamtBlance;
			string mchamtNonesum;
			AMSManager.STRUSettlementDetail[] vecSettlementDetails;
			bool ret__ = obj__.QuerySettlementDetail(strSPID, strMin, iOffset, iLimit, iStatus, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out mchamtAllsum, out mchamtBlance, out mchamtNonesum, out vecSettlementDetails, current__);
			os__.writeInt(iResult);
			os__.writeString(strResInfo);
			os__.writeInt(iTotalCount);
			os__.writeString(mchamtAllsum);
			os__.writeString(mchamtBlance);
			os__.writeString(mchamtNonesum);
			if(vecSettlementDetails == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(vecSettlementDetails.Length);
				for(int ix__ = 0; ix__ < vecSettlementDetails.Length; ++ix__)
				{
					vecSettlementDetails[ix__].write__(os__);
				}
			}
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QueryTranInfo___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strSPID;
			strSPID = is__.readString();
			int iOffset;
			iOffset = is__.readInt();
			int iLimit;
			iLimit = is__.readInt();
			int iStatus;
			iStatus = is__.readInt();
			string strStartTime;
			strStartTime = is__.readString();
			string strEndTime;
			strEndTime = is__.readString();
			string strBankID;
			strBankID = is__.readString();
			int iStartFee;
			iStartFee = is__.readInt();
			int iEndFee;
			iEndFee = is__.readInt();
			int iResult;
			string strResInfo;
			int iTotalCount;
			int iSuccNum;
			string succMoney;
			int iUnsuccNum;
			string unsuccMoney;
			AMSManager.STRUTranInfo[] vecTranInfos;
			bool ret__ = obj__.QueryTranInfo(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, strBankID, iStartFee, iEndFee, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out vecTranInfos, current__);
			os__.writeInt(iResult);
			os__.writeString(strResInfo);
			os__.writeInt(iTotalCount);
			os__.writeInt(iSuccNum);
			os__.writeString(succMoney);
			os__.writeInt(iUnsuccNum);
			os__.writeString(unsuccMoney);
			if(vecTranInfos == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(vecTranInfos.Length);
				for(int ix__ = 0; ix__ < vecTranInfos.Length; ++ix__)
				{
					vecTranInfos[ix__].write__(os__);
				}
			}
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus StatOrderInfo___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strSPID;
			strSPID = is__.readString();
			string strStartTime;
			strStartTime = is__.readString();
			string strEndTime;
			strEndTime = is__.readString();
			int iResult;
			string strResInfo;
			AMSManager.STRUStatOrderInfo stStatOrderInfo;
			bool ret__ = obj__.StatOrderInfo(strSPID, strStartTime, strEndTime, out iResult, out strResInfo, out stStatOrderInfo, current__);
			os__.writeInt(iResult);
			os__.writeString(strResInfo);
			stStatOrderInfo.write__(os__);
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QueryTransInfoCount___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strUin;
			strUin = is__.readString();
			int lRole;
			lRole = is__.readInt();
			string strStartTime;
			strStartTime = is__.readString();
			string strEndTime;
			strEndTime = is__.readString();
			int lTransType;
			lTransType = is__.readInt();
			string strPeerUin;
			strPeerUin = is__.readString();
			int lTradeStatus;
			lTradeStatus = is__.readInt();
			int lAmountLowBound;
			lAmountLowBound = is__.readInt();
			int lAmountHighBound;
			lAmountHighBound = is__.readInt();
			string strResinfo;
			int lDownloadNum;
			bool ret__ = obj__.QueryTransInfoCount(strUin, lRole, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, out strResinfo, out lDownloadNum, current__);
			os__.writeString(strResinfo);
			os__.writeInt(lDownloadNum);
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus DownloadTransInfo___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strUin;
			strUin = is__.readString();
			int lRole;
			lRole = is__.readInt();
			string strStartTime;
			strStartTime = is__.readString();
			string strEndTime;
			strEndTime = is__.readString();
			int lTransType;
			lTransType = is__.readInt();
			string strPeerUin;
			strPeerUin = is__.readString();
			int lTradeStatus;
			lTradeStatus = is__.readInt();
			int lAmountLowBound;
			lAmountLowBound = is__.readInt();
			int lAmountHighBound;
			lAmountHighBound = is__.readInt();
			int offset;
			offset = is__.readInt();
			int limit;
			limit = is__.readInt();
			string strResinfo;
			int lDownloadNum;
			AMSManager.STRUUserOrder[] UserOrderVec;
			bool ret__ = obj__.DownloadTransInfo(strUin, lRole, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, offset, limit, out strResinfo, out lDownloadNum, out UserOrderVec, current__);
			os__.writeString(strResinfo);
			os__.writeInt(lDownloadNum);
			if(UserOrderVec == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(UserOrderVec.Length);
				for(int ix__ = 0; ix__ < UserOrderVec.Length; ++ix__)
				{
					UserOrderVec[ix__].write__(os__);
				}
			}
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QueryUserOrderCount___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strUin;
			strUin = is__.readString();
			string strStartTime;
			strStartTime = is__.readString();
			string strEndTime;
			strEndTime = is__.readString();
			int lTransType;
			lTransType = is__.readInt();
			string strPeerUin;
			strPeerUin = is__.readString();
			int lTradeStatus;
			lTradeStatus = is__.readInt();
			int lAmountLowBound;
			lAmountLowBound = is__.readInt();
			int lAmountHighBound;
			lAmountHighBound = is__.readInt();
			string strResinfo;
			int OrderNum;
			bool ret__ = obj__.QueryUserOrderCount(strUin, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, out strResinfo, out OrderNum, current__);
			os__.writeString(strResinfo);
			os__.writeInt(OrderNum);
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus DownloadUserOrder___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strUin;
			strUin = is__.readString();
			string strStartTime;
			strStartTime = is__.readString();
			string strEndTime;
			strEndTime = is__.readString();
			int lTransType;
			lTransType = is__.readInt();
			string strPeerUin;
			strPeerUin = is__.readString();
			int lTradeStatus;
			lTradeStatus = is__.readInt();
			int lAmountLowBound;
			lAmountLowBound = is__.readInt();
			int lAmountHighBound;
			lAmountHighBound = is__.readInt();
			int offset;
			offset = is__.readInt();
			int limit;
			limit = is__.readInt();
			string strResinfo;
			int lDownloadNum;
			AMSManager.STRUNewUserOrder[] UserOrderVec;
			bool ret__ = obj__.DownloadUserOrder(strUin, strStartTime, strEndTime, lTransType, strPeerUin, lTradeStatus, lAmountLowBound, lAmountHighBound, offset, limit, out strResinfo, out lDownloadNum, out UserOrderVec, current__);
			os__.writeString(strResinfo);
			os__.writeInt(lDownloadNum);
			if(UserOrderVec == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(UserOrderVec.Length);
				for(int ix__ = 0; ix__ < UserOrderVec.Length; ++ix__)
				{
					UserOrderVec[ix__].write__(os__);
				}
			}
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QueryAirTicketCount___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strUin;
			strUin = is__.readString();
			string strStartTime;
			strStartTime = is__.readString();
			string strEndTime;
			strEndTime = is__.readString();
			int lTransState;
			lTransState = is__.readInt();
			int lAmountLowBound;
			lAmountLowBound = is__.readInt();
			int lAmountHighBound;
			lAmountHighBound = is__.readInt();
			string strPeerUin;
			strPeerUin = is__.readString();
			int lDownloadNum;
			string sResinfo;
			bool ret__ = obj__.QueryAirTicketCount(strUin, strStartTime, strEndTime, lTransState, lAmountLowBound, lAmountHighBound, strPeerUin, out lDownloadNum, out sResinfo, current__);
			os__.writeInt(lDownloadNum);
			os__.writeString(sResinfo);
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus DownloadAirTicket___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strUin;
			strUin = is__.readString();
			string strStartTime;
			strStartTime = is__.readString();
			string strEndTime;
			strEndTime = is__.readString();
			int lTransState;
			lTransState = is__.readInt();
			int lAmountLowBound;
			lAmountLowBound = is__.readInt();
			int lAmountHighBound;
			lAmountHighBound = is__.readInt();
			string strPeerUin;
			strPeerUin = is__.readString();
			int offset;
			offset = is__.readInt();
			int limit;
			limit = is__.readInt();
			int lDownloadNum;
			string sResinfo;
			AMSManager.STRUAirTicket[] AirTicketVec;
			bool ret__ = obj__.DownloadAirTicket(strUin, strStartTime, strEndTime, lTransState, lAmountLowBound, lAmountHighBound, strPeerUin, offset, limit, out lDownloadNum, out sResinfo, out AirTicketVec, current__);
			os__.writeInt(lDownloadNum);
			os__.writeString(sResinfo);
			if(AirTicketVec == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(AirTicketVec.Length);
				for(int ix__ = 0; ix__ < AirTicketVec.Length; ++ix__)
				{
					AirTicketVec[ix__].write__(os__);
				}
			}
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QueryBulletinInfo___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			int iSystemID;
			iSystemID = is__.readInt();
			string strStartTime;
			strStartTime = is__.readString();
			string strEndTime;
			strEndTime = is__.readString();
			int iResult;
			string strResInfo;
			AMSManager.STRUBulletinDetail[] vecBulletinDetail;
			bool ret__ = obj__.QueryBulletinInfo(iSystemID, strStartTime, strEndTime, out iResult, out strResInfo, out vecBulletinDetail, current__);
			os__.writeInt(iResult);
			os__.writeString(strResInfo);
			if(vecBulletinDetail == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(vecBulletinDetail.Length);
				for(int ix__ = 0; ix__ < vecBulletinDetail.Length; ++ix__)
				{
					vecBulletinDetail[ix__].write__(os__);
				}
			}
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus GetString___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strInput;
			strInput = is__.readString();
			string ret__ = obj__.GetString(strInput, current__);
			os__.writeString(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QueryTranInfoEx___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strSPID;
			strSPID = is__.readString();
			int iOffset;
			iOffset = is__.readInt();
			int iLimit;
			iLimit = is__.readInt();
			int iStatus;
			iStatus = is__.readInt();
			string strStartTime;
			strStartTime = is__.readString();
			string strEndTime;
			strEndTime = is__.readString();
			string strBankID;
			strBankID = is__.readString();
			int iStartFee;
			iStartFee = is__.readInt();
			int iEndFee;
			iEndFee = is__.readInt();
			int iResult;
			string strResInfo;
			int iTotalCount;
			int iSuccNum;
			string succMoney;
			int iUnsuccNum;
			string unsuccMoney;
			AMSManager.STRUTranInfoEx[] tranList;
			bool ret__ = obj__.QueryTranInfoEx(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, strBankID, iStartFee, iEndFee, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out tranList, current__);
			os__.writeInt(iResult);
			os__.writeString(strResInfo);
			os__.writeInt(iTotalCount);
			os__.writeInt(iSuccNum);
			os__.writeString(succMoney);
			os__.writeInt(iUnsuccNum);
			os__.writeString(unsuccMoney);
			if(tranList == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(tranList.Length);
				for(int ix__ = 0; ix__ < tranList.Length; ++ix__)
				{
					tranList[ix__].write__(os__);
				}
			}
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QueryAccountRollCount___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string uin;
			uin = is__.readString();
			string begTime;
			begTime = is__.readString();
			string endTime;
			endTime = is__.readString();
			int errCode;
			string errInfo;
			int ret__ = obj__.QueryAccountRollCount(uin, begTime, endTime, out errCode, out errInfo, current__);
			os__.writeInt(errCode);
			os__.writeString(errInfo);
			os__.writeInt(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QueryAccountRollList___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string uin;
			uin = is__.readString();
			string begTime;
			begTime = is__.readString();
			string endTime;
			endTime = is__.readString();
			int offset;
			offset = is__.readInt();
			int limit;
			limit = is__.readInt();
			AMSManager.AccRollRecordEx[] rolls;
			int errCode;
			string errInfo;
			obj__.QueryAccountRollList(uin, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, current__);
			if(rolls == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(rolls.Length);
				for(int ix__ = 0; ix__ < rolls.Length; ++ix__)
				{
					rolls[ix__].write__(os__);
				}
			}
			os__.writeInt(errCode);
			os__.writeString(errInfo);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QueryAccountRollList2___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string uin;
			uin = is__.readString();
			string begTime;
			begTime = is__.readString();
			string endTime;
			endTime = is__.readString();
			int offset;
			offset = is__.readInt();
			int limit;
			limit = is__.readInt();
			AMSManager.AccRollRecord2[] rolls;
			int errCode;
			string errInfo;
			obj__.QueryAccountRollList2(uin, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, current__);
			if(rolls == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(rolls.Length);
				for(int ix__ = 0; ix__ < rolls.Length; ++ix__)
				{
					rolls[ix__].write__(os__);
				}
			}
			os__.writeInt(errCode);
			os__.writeString(errInfo);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QuerySettDetailExBySpid___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strSPID;
			strSPID = is__.readString();
			string strMin;
			strMin = is__.readString();
			int iOffset;
			iOffset = is__.readInt();
			int iLimit;
			iLimit = is__.readInt();
			int iStatus;
			iStatus = is__.readInt();
			string strCreateTimeBegin;
			strCreateTimeBegin = is__.readString();
			string strCreateTimeEnd;
			strCreateTimeEnd = is__.readString();
			int iResult;
			string strResInfo;
			int iTotalCount;
			string mchamtAllsum;
			string mchamtBlance;
			string mchamtNonesum;
			AMSManager.STRUSettlementDetailEx[] vecSettlementDetails;
			bool ret__ = obj__.QuerySettDetailExBySpid(strSPID, strMin, iOffset, iLimit, iStatus, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out mchamtAllsum, out mchamtBlance, out mchamtNonesum, out vecSettlementDetails, current__);
			os__.writeInt(iResult);
			os__.writeString(strResInfo);
			os__.writeInt(iTotalCount);
			os__.writeString(mchamtAllsum);
			os__.writeString(mchamtBlance);
			os__.writeString(mchamtNonesum);
			if(vecSettlementDetails == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(vecSettlementDetails.Length);
				for(int ix__ = 0; ix__ < vecSettlementDetails.Length; ++ix__)
				{
					vecSettlementDetails[ix__].write__(os__);
				}
			}
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QuerySettDetailExByAgentId___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string agentId;
			agentId = is__.readString();
			string strMin;
			strMin = is__.readString();
			int iOffset;
			iOffset = is__.readInt();
			int iLimit;
			iLimit = is__.readInt();
			int iStatus;
			iStatus = is__.readInt();
			string strCreateTimeBegin;
			strCreateTimeBegin = is__.readString();
			string strCreateTimeEnd;
			strCreateTimeEnd = is__.readString();
			int iResult;
			string strResInfo;
			int iTotalCount;
			string mchamtAllsum;
			string mchamtBlance;
			string mchamtNonesum;
			AMSManager.STRUSettlementDetailEx[] vecSettlementDetails;
			bool ret__ = obj__.QuerySettDetailExByAgentId(agentId, strMin, iOffset, iLimit, iStatus, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out mchamtAllsum, out mchamtBlance, out mchamtNonesum, out vecSettlementDetails, current__);
			os__.writeInt(iResult);
			os__.writeString(strResInfo);
			os__.writeInt(iTotalCount);
			os__.writeString(mchamtAllsum);
			os__.writeString(mchamtBlance);
			os__.writeString(mchamtNonesum);
			if(vecSettlementDetails == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(vecSettlementDetails.Length);
				for(int ix__ = 0; ix__ < vecSettlementDetails.Length; ++ix__)
				{
					vecSettlementDetails[ix__].write__(os__);
				}
			}
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QueryTranReport___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string sSPID;
			sSPID = is__.readString();
			string sTranDate;
			sTranDate = is__.readString();
			int iResult;
			string strResInfo;
			int iTotalCount;
			int iPaidNum;
			string paidMoney;
			int iUnpaidNum;
			string unpaidMoney;
			AMSManager.STRUTranInfoEx[] tranList;
			bool ret__ = obj__.QueryTranReport(sSPID, sTranDate, out iResult, out strResInfo, out iTotalCount, out iPaidNum, out paidMoney, out iUnpaidNum, out unpaidMoney, out tranList, current__);
			os__.writeInt(iResult);
			os__.writeString(strResInfo);
			os__.writeInt(iTotalCount);
			os__.writeInt(iPaidNum);
			os__.writeString(paidMoney);
			os__.writeInt(iUnpaidNum);
			os__.writeString(unpaidMoney);
			if(tranList == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(tranList.Length);
				for(int ix__ = 0; ix__ < tranList.Length; ++ix__)
				{
					tranList[ix__].write__(os__);
				}
			}
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QueryAccountRollCountByCond___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string uin;
			uin = is__.readString();
			AMSManager.CompareCond[] conds;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 3);
			conds = new AMSManager.CompareCond[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				conds[ix__] = new AMSManager.CompareCond();
				conds[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			int errCode;
			string errInfo;
			int ret__ = obj__.QueryAccountRollCountByCond(uin, conds, out errCode, out errInfo, current__);
			os__.writeInt(errCode);
			os__.writeString(errInfo);
			os__.writeInt(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QueryAccountRollListByCond___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string uin;
			uin = is__.readString();
			AMSManager.CompareCond[] conds;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 3);
			conds = new AMSManager.CompareCond[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				conds[ix__] = new AMSManager.CompareCond();
				conds[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			int offset;
			offset = is__.readInt();
			int limit;
			limit = is__.readInt();
			AMSManager.AccRollRecord[] rolls;
			int errCode;
			string errInfo;
			obj__.QueryAccountRollListByCond(uin, conds, offset, limit, out rolls, out errCode, out errInfo, current__);
			if(rolls == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(rolls.Length);
				for(int ix__ = 0; ix__ < rolls.Length; ++ix__)
				{
					rolls[ix__].write__(os__);
				}
			}
			os__.writeInt(errCode);
			os__.writeString(errInfo);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus DownloadAccountDetails___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string uin;
			uin = is__.readString();
			AMSManager.CompareCond[] conds;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 3);
			conds = new AMSManager.CompareCond[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				conds[ix__] = new AMSManager.CompareCond();
				conds[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			int offset;
			offset = is__.readInt();
			int limit;
			limit = is__.readInt();
			AMSManager.AccRollRecordEx[] rolls;
			int errCode;
			string errInfo;
			obj__.DownloadAccountDetails(uin, conds, offset, limit, out rolls, out errCode, out errInfo, current__);
			if(rolls == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(rolls.Length);
				for(int ix__ = 0; ix__ < rolls.Length; ++ix__)
				{
					rolls[ix__].write__(os__);
				}
			}
			os__.writeInt(errCode);
			os__.writeString(errInfo);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QuerySpAccountRollCount___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string spid;
			spid = is__.readString();
			string begTime;
			begTime = is__.readString();
			string endTime;
			endTime = is__.readString();
			int errCode;
			string errInfo;
			int ret__ = obj__.QuerySpAccountRollCount(spid, begTime, endTime, out errCode, out errInfo, current__);
			os__.writeInt(errCode);
			os__.writeString(errInfo);
			os__.writeInt(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QuerySpAccountRollList___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string spid;
			spid = is__.readString();
			string begTime;
			begTime = is__.readString();
			string endTime;
			endTime = is__.readString();
			int offset;
			offset = is__.readInt();
			int limit;
			limit = is__.readInt();
			AMSManager.AccRollRecordEx[] rolls;
			int errCode;
			string errInfo;
			obj__.QuerySpAccountRollList(spid, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, current__);
			if(rolls == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(rolls.Length);
				for(int ix__ = 0; ix__ < rolls.Length; ++ix__)
				{
					rolls[ix__].write__(os__);
				}
			}
			os__.writeInt(errCode);
			os__.writeString(errInfo);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QueryTranInfo2___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strSPID;
			strSPID = is__.readString();
			int iOffset;
			iOffset = is__.readInt();
			int iLimit;
			iLimit = is__.readInt();
			int iStatus;
			iStatus = is__.readInt();
			string strStartTime;
			strStartTime = is__.readString();
			string strEndTime;
			strEndTime = is__.readString();
			int tmType;
			tmType = is__.readInt();
			string strBankID;
			strBankID = is__.readString();
			int iStartFee;
			iStartFee = is__.readInt();
			int iEndFee;
			iEndFee = is__.readInt();
			int iFlag;
			iFlag = is__.readInt();
			int iResult;
			string strResInfo;
			int iTotalCount;
			int iSuccNum;
			string succMoney;
			int iUnsuccNum;
			string unsuccMoney;
			AMSManager.STRUTranInfo2[] vecTranInfos;
			bool ret__ = obj__.QueryTranInfo2(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, tmType, strBankID, iStartFee, iEndFee, iFlag, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out vecTranInfos, current__);
			os__.writeInt(iResult);
			os__.writeString(strResInfo);
			os__.writeInt(iTotalCount);
			os__.writeInt(iSuccNum);
			os__.writeString(succMoney);
			os__.writeInt(iUnsuccNum);
			os__.writeString(unsuccMoney);
			if(vecTranInfos == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(vecTranInfos.Length);
				for(int ix__ = 0; ix__ < vecTranInfos.Length; ++ix__)
				{
					vecTranInfos[ix__].write__(os__);
				}
			}
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QueryHistoryTranInfo2___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strSPID;
			strSPID = is__.readString();
			int iOffset;
			iOffset = is__.readInt();
			int iLimit;
			iLimit = is__.readInt();
			int iStatus;
			iStatus = is__.readInt();
			string strStartTime;
			strStartTime = is__.readString();
			string strEndTime;
			strEndTime = is__.readString();
			int tmType;
			tmType = is__.readInt();
			string strBankID;
			strBankID = is__.readString();
			int iStartFee;
			iStartFee = is__.readInt();
			int iEndFee;
			iEndFee = is__.readInt();
			int iFlag;
			iFlag = is__.readInt();
			int iResult;
			string strResInfo;
			int iTotalCount;
			int iSuccNum;
			string succMoney;
			int iUnsuccNum;
			string unsuccMoney;
			AMSManager.STRUTranInfo2[] vecTranInfos;
			bool ret__ = obj__.QueryHistoryTranInfo2(strSPID, iOffset, iLimit, iStatus, strStartTime, strEndTime, tmType, strBankID, iStartFee, iEndFee, iFlag, out iResult, out strResInfo, out iTotalCount, out iSuccNum, out succMoney, out iUnsuccNum, out unsuccMoney, out vecTranInfos, current__);
			os__.writeInt(iResult);
			os__.writeString(strResInfo);
			os__.writeInt(iTotalCount);
			os__.writeInt(iSuccNum);
			os__.writeString(succMoney);
			os__.writeInt(iUnsuccNum);
			os__.writeString(unsuccMoney);
			if(vecTranInfos == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(vecTranInfos.Length);
				for(int ix__ = 0; ix__ < vecTranInfos.Length; ++ix__)
				{
					vecTranInfos[ix__].write__(os__);
				}
			}
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QueryTransBankRollList___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string spid;
			spid = is__.readString();
			string listId;
			listId = is__.readString();
			int offset;
			offset = is__.readInt();
			int limit;
			limit = is__.readInt();
			int total;
			AMSManager.AccRollRecord[] rolls;
			int errCode;
			string errInfo;
			obj__.QueryTransBankRollList(spid, listId, offset, limit, out total, out rolls, out errCode, out errInfo, current__);
			os__.writeInt(total);
			if(rolls == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(rolls.Length);
				for(int ix__ = 0; ix__ < rolls.Length; ++ix__)
				{
					rolls[ix__].write__(os__);
				}
			}
			os__.writeInt(errCode);
			os__.writeString(errInfo);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QueryDrawDetail___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strSPID;
			strSPID = is__.readString();
			string strMin;
			strMin = is__.readString();
			int iOffset;
			iOffset = is__.readInt();
			int iLimit;
			iLimit = is__.readInt();
			string strCreateTimeBegin;
			strCreateTimeBegin = is__.readString();
			string strCreateTimeEnd;
			strCreateTimeEnd = is__.readString();
			int iResult;
			string strResInfo;
			int iTotalCount;
			AMSManager.STRUDrawDetail[] vecDrawDetails;
			bool ret__ = obj__.QueryDrawDetail(strSPID, strMin, iOffset, iLimit, strCreateTimeBegin, strCreateTimeEnd, out iResult, out strResInfo, out iTotalCount, out vecDrawDetails, current__);
			os__.writeInt(iResult);
			os__.writeString(strResInfo);
			os__.writeInt(iTotalCount);
			if(vecDrawDetails == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(vecDrawDetails.Length);
				for(int ix__ = 0; ix__ < vecDrawDetails.Length; ++ix__)
				{
					vecDrawDetails[ix__].write__(os__);
				}
			}
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QueryOkSettlementDetailAgent___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strSPID;
			strSPID = is__.readString();
			string strMin;
			strMin = is__.readString();
			int iOffset;
			iOffset = is__.readInt();
			int iLimit;
			iLimit = is__.readInt();
			int iNo;
			iNo = is__.readInt();
			int iResult;
			string strResInfo;
			int iTotalCount;
			AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent;
			bool ret__ = obj__.QueryOkSettlementDetailAgent(strSPID, strMin, iOffset, iLimit, iNo, out iResult, out strResInfo, out iTotalCount, out vecSettlementDetailAgent, current__);
			os__.writeInt(iResult);
			os__.writeString(strResInfo);
			os__.writeInt(iTotalCount);
			if(vecSettlementDetailAgent == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(vecSettlementDetailAgent.Length);
				for(int ix__ = 0; ix__ < vecSettlementDetailAgent.Length; ++ix__)
				{
					vecSettlementDetailAgent[ix__].write__(os__);
				}
			}
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QueryNoSettlementDetailAgent___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strSPID;
			strSPID = is__.readString();
			string strMin;
			strMin = is__.readString();
			int iOffset;
			iOffset = is__.readInt();
			int iLimit;
			iLimit = is__.readInt();
			int iResult;
			string strResInfo;
			int iNoAllCount;
			string iNoAllAmt;
			int iNoTodayCount;
			string iNoTodayAmt;
			int iRefundCOunt;
			string iRefundAmt;
			int iNoHisCount;
			string iNoHisAmt;
			int iTotalCount;
			AMSManager.STRUSettlementDetailAgent[] vecSettlementDetailAgent;
			bool ret__ = obj__.QueryNoSettlementDetailAgent(strSPID, strMin, iOffset, iLimit, out iResult, out strResInfo, out iNoAllCount, out iNoAllAmt, out iNoTodayCount, out iNoTodayAmt, out iRefundCOunt, out iRefundAmt, out iNoHisCount, out iNoHisAmt, out iTotalCount, out vecSettlementDetailAgent, current__);
			os__.writeInt(iResult);
			os__.writeString(strResInfo);
			os__.writeInt(iNoAllCount);
			os__.writeString(iNoAllAmt);
			os__.writeInt(iNoTodayCount);
			os__.writeString(iNoTodayAmt);
			os__.writeInt(iRefundCOunt);
			os__.writeString(iRefundAmt);
			os__.writeInt(iNoHisCount);
			os__.writeString(iNoHisAmt);
			os__.writeInt(iTotalCount);
			if(vecSettlementDetailAgent == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(vecSettlementDetailAgent.Length);
				for(int ix__ = 0; ix__ < vecSettlementDetailAgent.Length; ++ix__)
				{
					vecSettlementDetailAgent[ix__].write__(os__);
				}
			}
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QuerySubAccountRollCount___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			int uid;
			uid = is__.readInt();
			int curType;
			curType = is__.readInt();
			int usrType;
			usrType = is__.readInt();
			int rollType;
			rollType = is__.readInt();
			string begTime;
			begTime = is__.readString();
			string endTime;
			endTime = is__.readString();
			int errCode;
			string errInfo;
			int ret__ = obj__.QuerySubAccountRollCount(uid, curType, usrType, rollType, begTime, endTime, out errCode, out errInfo, current__);
			os__.writeInt(errCode);
			os__.writeString(errInfo);
			os__.writeInt(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QuerySubAccountRollList___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			int uid;
			uid = is__.readInt();
			int curType;
			curType = is__.readInt();
			int usrType;
			usrType = is__.readInt();
			int rollType;
			rollType = is__.readInt();
			string begTime;
			begTime = is__.readString();
			string endTime;
			endTime = is__.readString();
			int offset;
			offset = is__.readInt();
			int limit;
			limit = is__.readInt();
			AMSManager.AccRollRecordEx[] rolls;
			int errCode;
			string errInfo;
			obj__.QuerySubAccountRollList(uid, curType, usrType, rollType, begTime, endTime, offset, limit, out rolls, out errCode, out errInfo, current__);
			if(rolls == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(rolls.Length);
				for(int ix__ = 0; ix__ < rolls.Length; ++ix__)
				{
					rolls[ix__].write__(os__);
				}
			}
			os__.writeInt(errCode);
			os__.writeString(errInfo);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		public static IceInternal.DispatchStatus QueryAirOrderBat___(IAMSManager obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string strSpid;
			strSpid = is__.readString();
			string strStime;
			strStime = is__.readString();
			string strEtime;
			strEtime = is__.readString();
			int ioffset;
			ioffset = is__.readInt();
			int iLimit;
			iLimit = is__.readInt();
			int iTotal;
			int iReturnNum;
			int iErrCode;
			string strErrInfo;
			AMSManager.STRAirOrderinfo[] SEQrolls;
			bool ret__ = obj__.QueryAirOrderBat(strSpid, strStime, strEtime, ioffset, iLimit, out iTotal, out iReturnNum, out iErrCode, out strErrInfo, out SEQrolls, current__);
			os__.writeInt(iTotal);
			os__.writeInt(iReturnNum);
			os__.writeInt(iErrCode);
			os__.writeString(strErrInfo);
			if(SEQrolls == null)
			{
				os__.writeSize(0);
			}
			else
			{
				os__.writeSize(SEQrolls.Length);
				for(int ix__ = 0; ix__ < SEQrolls.Length; ++ix__)
				{
					SEQrolls[ix__].write__(os__);
				}
			}
			os__.writeBool(ret__);
			return IceInternal.DispatchStatus.DispatchOK;
		}

		private static string[] all__ =
	{
		"DownloadAccountDetails",
		"DownloadAirTicket",
		"DownloadTransInfo",
		"DownloadUserOrder",
		"GetString",
		"ice_id",
		"ice_ids",
		"ice_isA",
		"ice_ping",
		"QueryAccountRollCount",
		"QueryAccountRollCountByCond",
		"QueryAccountRollList",
		"QueryAccountRollList2",
		"QueryAccountRollListByCond",
		"QueryAirOrderBat",
		"QueryAirTicketCount",
		"QueryBulletinInfo",
		"QueryDrawDetail",
		"QueryHistoryTranInfo2",
		"QueryNoSettlementDetailAgent",
		"QueryOkSettlementDetailAgent",
		"QuerySettDetailExByAgentId",
		"QuerySettDetailExBySpid",
		"QuerySettlementDetail",
		"QuerySpAccountRollCount",
		"QuerySpAccountRollList",
		"QuerySubAccountRollCount",
		"QuerySubAccountRollList",
		"QueryTranInfo",
		"QueryTranInfo2",
		"QueryTranInfoEx",
		"QueryTranReport",
		"QueryTransBankRollList",
		"QueryTransInfoCount",
		"QueryUserOrderCount",
		"StatOrderInfo"
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
					return DownloadAccountDetails___(this, inS__, current__);
				}
				case 1:
				{
					return DownloadAirTicket___(this, inS__, current__);
				}
				case 2:
				{
					return DownloadTransInfo___(this, inS__, current__);
				}
				case 3:
				{
					return DownloadUserOrder___(this, inS__, current__);
				}
				case 4:
				{
					return GetString___(this, inS__, current__);
				}
				case 5:
				{
					return ice_id___(this, inS__, current__);
				}
				case 6:
				{
					return ice_ids___(this, inS__, current__);
				}
				case 7:
				{
					return ice_isA___(this, inS__, current__);
				}
				case 8:
				{
					return ice_ping___(this, inS__, current__);
				}
				case 9:
				{
					return QueryAccountRollCount___(this, inS__, current__);
				}
				case 10:
				{
					return QueryAccountRollCountByCond___(this, inS__, current__);
				}
				case 11:
				{
					return QueryAccountRollList___(this, inS__, current__);
				}
				case 12:
				{
					return QueryAccountRollList2___(this, inS__, current__);
				}
				case 13:
				{
					return QueryAccountRollListByCond___(this, inS__, current__);
				}
				case 14:
				{
					return QueryAirOrderBat___(this, inS__, current__);
				}
				case 15:
				{
					return QueryAirTicketCount___(this, inS__, current__);
				}
				case 16:
				{
					return QueryBulletinInfo___(this, inS__, current__);
				}
				case 17:
				{
					return QueryDrawDetail___(this, inS__, current__);
				}
				case 18:
				{
					return QueryHistoryTranInfo2___(this, inS__, current__);
				}
				case 19:
				{
					return QueryNoSettlementDetailAgent___(this, inS__, current__);
				}
				case 20:
				{
					return QueryOkSettlementDetailAgent___(this, inS__, current__);
				}
				case 21:
				{
					return QuerySettDetailExByAgentId___(this, inS__, current__);
				}
				case 22:
				{
					return QuerySettDetailExBySpid___(this, inS__, current__);
				}
				case 23:
				{
					return QuerySettlementDetail___(this, inS__, current__);
				}
				case 24:
				{
					return QuerySpAccountRollCount___(this, inS__, current__);
				}
				case 25:
				{
					return QuerySpAccountRollList___(this, inS__, current__);
				}
				case 26:
				{
					return QuerySubAccountRollCount___(this, inS__, current__);
				}
				case 27:
				{
					return QuerySubAccountRollList___(this, inS__, current__);
				}
				case 28:
				{
					return QueryTranInfo___(this, inS__, current__);
				}
				case 29:
				{
					return QueryTranInfo2___(this, inS__, current__);
				}
				case 30:
				{
					return QueryTranInfoEx___(this, inS__, current__);
				}
				case 31:
				{
					return QueryTranReport___(this, inS__, current__);
				}
				case 32:
				{
					return QueryTransBankRollList___(this, inS__, current__);
				}
				case 33:
				{
					return QueryTransInfoCount___(this, inS__, current__);
				}
				case 34:
				{
					return QueryUserOrderCount___(this, inS__, current__);
				}
				case 35:
				{
					return StatOrderInfo___(this, inS__, current__);
				}
			}

			_System.Diagnostics.Debug.Assert(false);
			return IceInternal.DispatchStatus.DispatchOperationNotExist;
		}

		#endregion
	}
}

namespace MediCS
{
	public abstract class IMediCustServiceDisp_ : Ice.ObjectImpl, IMediCustService
	{
		#region Slice operations

		public long addOpLog(MediCS.OperationLog opLog)
		{
			return addOpLog(opLog, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract long addOpLog(MediCS.OperationLog opLog, Ice.Current current__);

		public void queryOpLog(MediCS.CompareCond[] condition, int offset, int limit, out MediCS.OperationLog[] results)
		{
			queryOpLog(condition, offset, limit, out results, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract void queryOpLog(MediCS.CompareCond[] condition, int offset, int limit, out MediCS.OperationLog[] results, Ice.Current current__);

		public int queryOpCount(MediCS.CompareCond[] condition)
		{
			return queryOpCount(condition, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract int queryOpCount(MediCS.CompareCond[] condition, Ice.Current current__);

		public void updateAppealRecord(string transId, string appealId, MediCS.KVDict kvs)
		{
			updateAppealRecord(transId, appealId, kvs, Ice.ObjectImpl.defaultCurrent);
		}

		public abstract void updateAppealRecord(string transId, string appealId, MediCS.KVDict kvs, Ice.Current current__);

		#endregion

		#region Slice type-related members

		public static new string[] ids__ = 
	{
		"::Ice::Object",
		"::MediCS::IMediCustService"
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
			return ids__[1];
		}

		public override string ice_id(Ice.Current current__)
		{
			return ids__[1];
		}

		public static new string ice_staticId()
		{
			return ids__[1];
		}

		#endregion

		#region Operation dispatch

		public static IceInternal.DispatchStatus addOpLog___(IMediCustService obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			MediCS.OperationLog opLog;
			opLog = new MediCS.OperationLog();
			opLog.read__(is__);
			try
			{
				long ret__ = obj__.addOpLog(opLog, current__);
				os__.writeLong(ret__);
				return IceInternal.DispatchStatus.DispatchOK;
			}
			catch(MediCS.ErrorBase ex)
			{
				os__.writeUserException(ex);
				return IceInternal.DispatchStatus.DispatchUserException;
			}
		}

		public static IceInternal.DispatchStatus queryOpLog___(IMediCustService obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			MediCS.CompareCond[] condition;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 3);
			condition = new MediCS.CompareCond[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				condition[ix__] = new MediCS.CompareCond();
				condition[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			int offset;
			offset = is__.readInt();
			int limit;
			limit = is__.readInt();
			MediCS.OperationLog[] results;
			try
			{
				obj__.queryOpLog(condition, offset, limit, out results, current__);
				if(results == null)
				{
					os__.writeSize(0);
				}
				else
				{
					os__.writeSize(results.Length);
					for(int ix__ = 0; ix__ < results.Length; ++ix__)
					{
						results[ix__].write__(os__);
					}
				}
				return IceInternal.DispatchStatus.DispatchOK;
			}
			catch(MediCS.ErrorBase ex)
			{
				os__.writeUserException(ex);
				return IceInternal.DispatchStatus.DispatchUserException;
			}
		}

		public static IceInternal.DispatchStatus queryOpCount___(IMediCustService obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			MediCS.CompareCond[] condition;
		{
			int szx__ = is__.readSize();
			is__.startSeq(szx__, 3);
			condition = new MediCS.CompareCond[szx__];
			for(int ix__ = 0; ix__ < szx__; ++ix__)
			{
				condition[ix__] = new MediCS.CompareCond();
				condition[ix__].read__(is__);
				is__.checkSeq();
				is__.endElement();
			}
			is__.endSeq(szx__);
		}
			try
			{
				int ret__ = obj__.queryOpCount(condition, current__);
				os__.writeInt(ret__);
				return IceInternal.DispatchStatus.DispatchOK;
			}
			catch(MediCS.ErrorBase ex)
			{
				os__.writeUserException(ex);
				return IceInternal.DispatchStatus.DispatchUserException;
			}
		}

		public static IceInternal.DispatchStatus updateAppealRecord___(IMediCustService obj__, IceInternal.Incoming inS__, Ice.Current current__)
		{
			checkMode__(Ice.OperationMode.Normal, current__.mode);
			IceInternal.BasicStream is__ = inS__.istr();
			IceInternal.BasicStream os__ = inS__.ostr();
			string transId;
			transId = is__.readString();
			string appealId;
			appealId = is__.readString();
			MediCS.KVDict kvs;
			kvs = MediCS.KVDictHelper.read(is__);
			try
			{
				obj__.updateAppealRecord(transId, appealId, kvs, current__);
				return IceInternal.DispatchStatus.DispatchOK;
			}
			catch(MediCS.ErrorBase ex)
			{
				os__.writeUserException(ex);
				return IceInternal.DispatchStatus.DispatchUserException;
			}
		}

		private static string[] all__ =
	{
		"addOpLog",
		"ice_id",
		"ice_ids",
		"ice_isA",
		"ice_ping",
		"queryOpCount",
		"queryOpLog",
		"updateAppealRecord"
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
					return addOpLog___(this, inS__, current__);
				}
				case 1:
				{
					return ice_id___(this, inS__, current__);
				}
				case 2:
				{
					return ice_ids___(this, inS__, current__);
				}
				case 3:
				{
					return ice_isA___(this, inS__, current__);
				}
				case 4:
				{
					return ice_ping___(this, inS__, current__);
				}
				case 5:
				{
					return queryOpCount___(this, inS__, current__);
				}
				case 6:
				{
					return queryOpLog___(this, inS__, current__);
				}
				case 7:
				{
					return updateAppealRecord___(this, inS__, current__);
				}
			}

			_System.Diagnostics.Debug.Assert(false);
			return IceInternal.DispatchStatus.DispatchOperationNotExist;
		}

		#endregion
	}
}
