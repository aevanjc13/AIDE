using System;
using System.Collections.Generic;
using System.Text;
namespace GDC.PH.AIDE.BusinessLayer
{
	public class clsStatus: BusinessObjectBase
	{

		#region InnerClass
		public enum clsStatusFields
		{
			GRP_ID,
			GRP_NAME,
			DESCR,
			STATUS
		}
		#endregion

		#region Data Members

			byte _gRP_ID;
			string _gRP_NAME;
			string _dESCR;
			byte _sTATUS;

		#endregion

		#region Properties

		public byte  GRP_ID
		{
			 get { return _gRP_ID; }
			 set
			 {
				 if (_gRP_ID != value)
				 {
					_gRP_ID = value;
					 PropertyHasChanged("GRP_ID");
				 }
			 }
		}

		public string  GRP_NAME
		{
			 get { return _gRP_NAME; }
			 set
			 {
				 if (_gRP_NAME != value)
				 {
					_gRP_NAME = value;
					 PropertyHasChanged("GRP_NAME");
				 }
			 }
		}

		public string  DESCR
		{
			 get { return _dESCR; }
			 set
			 {
				 if (_dESCR != value)
				 {
					_dESCR = value;
					 PropertyHasChanged("DESCR");
				 }
			 }
		}

		public byte  STATUS
		{
			 get { return _sTATUS; }
			 set
			 {
				 if (_sTATUS != value)
				 {
					_sTATUS = value;
					 PropertyHasChanged("STATUS");
				 }
			 }
		}


		#endregion

		#region Validation

		internal override void AddValidationRules()
		{
			ValidationRules.AddRules(new Validation.ValidateRuleNotNull("GRP_ID", "GRP_ID"));
			ValidationRules.AddRules(new Validation.ValidateRuleNotNull("GRP_NAME", "GRP_NAME"));
			ValidationRules.AddRules(new Validation.ValidateRuleStringMaxLength("GRP_NAME", "GRP_NAME",20));
			ValidationRules.AddRules(new Validation.ValidateRuleNotNull("DESCR", "DESCR"));
			ValidationRules.AddRules(new Validation.ValidateRuleStringMaxLength("DESCR", "DESCR",20));
			ValidationRules.AddRules(new Validation.ValidateRuleNotNull("STATUS", "STATUS"));
		}

		#endregion

	}
}
