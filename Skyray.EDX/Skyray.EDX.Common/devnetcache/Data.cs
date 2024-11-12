using System;
using System.EnterpriseServices; 
using System.Runtime.InteropServices;  


[assembly: ApplicationActivation(ActivationOption.Server)]
namespace stressDLL
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	[ObjectPooling(Enabled=true, MinPoolSize=2, MaxPoolSize=5)]
	public class Data : ServicedComponent  
	{
		public Data()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public string GetTime(string SessionID)
		{
			string RV="";
            Skyray.EDX.Common.INFRA.DevCache oCache = new Skyray.EDX.Common.INFRA.DevCache();  
			System.Collections.Hashtable oData =   (System.Collections.Hashtable)oCache.GetObject(SessionID);
			if ((string)oData["UID"] == "Natty")
				RV = System.DateTime.Now.ToString();
			return RV;
		}
		public string GetTime()
		{
			string RV="";
			// it doesn't matter if we use the web context or the COM+ context. it wont work one way or another
			    
			if ((string)((System.Collections.Specialized.NameValueCollection)  System.Web.HttpContext.Current.Items["Form"])["UID"] == "Natty")
				RV = System.DateTime.Now.ToString();
			return RV;
		}
	}

}
