
using System;

namespace xmmssharp
{
	
	
	public class XmmsConnection
	{
		
		public XmmsConnection(string client_name)
		{
		    if (client_name == null || client_name == "")
		        clientName = "xmms_sharp";		        
		    else
		        clientName = client_name;
		}
	
	    public SWIGTYPE_p_xmmsc_connection_St Handle;
	    private string clientName;
	    
		public bool Connect(){
	     Handle = XmmsClientInterface.xmmsc_init(clientName);
    	 return XmmsClientInterface.xmmsc_connect(Handle, null) == 1;
		}
		
		public bool Disconnect(){
		 return true; 
		}
	}
}
