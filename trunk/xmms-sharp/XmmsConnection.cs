
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace xmmssharp
{


public class XmmsConnection
{
//TODO: add discon event
//  public static void xmmsc_disconnect_callback_set(SWIGTYPE_p_xmmsc_connection_St c, SWIGTYPE_p_f_p_void__void callback, SWIGTYPE_p_void userdata) {
//   public static SWIGTYPE_p_xmmsc_result_St xmmsc_broadcast_quit(SWIGTYPE_p_xmmsc_connection_St c) {

    public XmmsConnection(string client_name)
    {
        if (client_name == null || client_name == "")
            clientName = "xmms_sharp";
        else
            clientName = client_name;
    }

    public SWIGTYPE_p_xmmsc_connection_St Handle;

    public HandleRef HandleRef {
    get { return SWIGTYPE_p_xmmsc_connection_St.getCPtr(Handle); }
    }

    private string clientName;

    public Dictionary<string, object> GetMainStats(){
        XmmsResult res = new XmmsResult( XmmsClientInterface.xmmsc_main_stats(Handle) );        
        res.Wait();
        return res.GetDict();
    }


    public List<Dictionary<string, object>> GetPluginlist(xmms_plugin_type_t type){
        List<Dictionary<string, object>> l = new List<Dictionary<string, object>>();       
        XmmsResult res = new XmmsResult( XmmsClientInterface.xmmsc_plugin_list(Handle, type) );
        res.Wait();
        while (res.IsValidList()){
            l.Add(res.GetDict());
            res.ListNext();
        }
        return l;        
    }

    public string GetLastError(){
        return XmmsClientInterface.xmmsc_get_last_error(Handle);
    }

    public bool Connect(){
        Handle = XmmsClientInterface.xmmsc_init(clientName);
        return XmmsClientInterface.xmmsc_connect(Handle, null) == 1;
    }

    public void Disconnect(){
        XmmsClientInterface.xmmsc_unref(Handle);
    }
    
    ~XmmsConnection(){
        Disconnect();
    }
}
}
