
using System;
using System.Collections.Generic;

namespace xmmssharp
{
	
	
	public class XmmsConfig
	{

        private XmmsConnection connection;
        
        
        public XmmsConfig(XmmsConnection conn)
        {
            connection = conn;
            XmmsResult r = new XmmsResult ( XmmsClientInterface.xmmsc_broadcast_configval_changed (connection.Handle));
            r.NotifierSet(new XmmsResult.notifier_func(configval_changed));
            
        }
        
        public Dictionary<string, object> List(){
            XmmsResult r = new XmmsResult ( XmmsClientInterface.xmmsc_configval_list(connection.Handle) );
            r.Wait();
            return r.GetDict();
        }
        
        public void SetValue(string key, string val){
            XmmsResult r = new XmmsResult ( XmmsClientInterface.xmmsc_configval_set(connection.Handle, key, val) );
            r.Wait();
            if(r.IsError()) r.RaiseError();
        }
        
        public string GetValue(string key){
            XmmsResult r = new XmmsResult ( XmmsClientInterface.xmmsc_configval_get(connection.Handle, key) );
            r.Wait();
            return r.GetString();
        }
        
        public XmmsResult Register(string key, string default_value){
            XmmsResult r = new XmmsResult ( XmmsClientInterface.xmmsc_configval_register(connection.Handle, key, default_value) );
            r.Wait();
            return r;
        }
        
        public event ConfigValChangedHandler ConfigValChanged;
        public delegate void ConfigValChangedHandler(XmmsResult res, int id);
        public void OnConfigValChanged(XmmsResult res, int id){
            if(ConfigValChanged != null)
                ConfigValChanged(res, id);
        }

        
        void configval_changed (IntPtr res)
        {
        // TODO: FIX THIS
            XmmsResult r = new XmmsResult ( res );                    
            OnConfigValChanged( r, (int)r.GetUint());
        }
	}
}
