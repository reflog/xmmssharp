
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace xmmssharp
{


public class XmmsResult
{
    private SWIGTYPE_p_xmmsc_result_St res;

    public XmmsResult(SWIGTYPE_p_xmmsc_result_St res_)
    {
        res = res_;        
    }
    
    override public string ToString(){
        int r = XmmsClientInterface.xmmsc_result_get_type(res);
        return ((xmms_object_cmd_arg_type_t) r).ToString();
    }
    
    public bool IsValidList(){
        return XmmsClientInterface.xmmsc_result_list_valid (res) == 1;
    }
    public void Wait(){
        XmmsClientInterface.xmmsc_result_wait (res);
    }

    public string GetError(){
        return XmmsClientInterface.xmmsc_result_get_error(res);
    }

    public bool IsError(){
        return XmmsClientInterface.xmmsc_result_iserror (res) == 1;
    }

    public uint GetUint(){
        uint ui = 0;
        if (xmmsc_result_get_uint(res.swigCPtr, out ui) == 0){
            throw new XmmsException("Broken Result");
        }
        return ui;
    }

    public int GetDictInt32(string key){
        int ui = 0;
        if (xmmsc_result_get_dict_entry_int32(res.swigCPtr, key, out ui) == 0){
            throw new XmmsException("Broken Result");
        }
        return ui;
    }

    ~XmmsResult(){
        XmmsClientInterface.xmmsc_result_unref (res);
    }

    public string GetDictString(string key){

        StringBuilder s= new StringBuilder(1024);
        if(xmmsc_result_get_dict_entry_str(res.swigCPtr, key, s) == 0){
            throw new XmmsException("Broken Result");
        }
        return s.ToString();
    }


    public void ListNext(){
        XmmsClientInterface.xmmsc_result_list_next (res);
    }

    public bool DictHasKey(string key){
        return XmmsClientInterface.xmmsc_result_get_dict_entry_type (res, key) != xmmsc_result_value_type_t.XMMSC_RESULT_VALUE_TYPE_NONE;
    }

    public string EntryFormat(string format){
        StringBuilder line = new StringBuilder(1024);
        xmmsc_entry_format(line, line.Capacity, format, res.swigCPtr);
        return line.ToString();
    }

    public int GetResultType(){
        return XmmsClientInterface.xmmsc_result_get_type(res);
    }

    public bool IsDict(){
        return (xmms_object_cmd_arg_type_t) GetResultType() == xmms_object_cmd_arg_type_t.XMMS_OBJECT_CMD_ARG_DICT;
    }

    public bool IsPropDict(){
        return (xmms_object_cmd_arg_type_t) GetResultType() == xmms_object_cmd_arg_type_t.XMMS_OBJECT_CMD_ARG_PROPDICT;
    }

    public Dictionary<string, object> GetDict(){
        Dictionary<string, object> dict = new Dictionary<string, object>();
        if (!IsDict() && !IsPropDict())
            throw new XmmsException("Tried GetDict on non dict result");
        GCHandle idict = GCHandle.Alloc( dict );        
        if ( IsDict()) 
            xmmsc_result_dict_foreach (res.swigCPtr, new foreach_dict_func(foreach_dict), (IntPtr)idict );
        else 
            xmmsc_result_propdict_foreach (res.swigCPtr, new foreach_propdict_func(foreach_propdict), (IntPtr)idict );
            
        idict.Free();
        return dict;
    }


    delegate void foreach_dict_func (string key, int type,  IntPtr val, IntPtr udata);
    static void foreach_dict (string key, int type,  IntPtr val, IntPtr udata)
    {
        GCHandle gch = (GCHandle)udata;
        Dictionary<string, object> dict = (Dictionary<string, object>)(gch.Target);
        if(type == (int)xmmsc_result_value_type_t.XMMSC_RESULT_VALUE_TYPE_STRING) {
            dict[key] = Marshal.PtrToStringAnsi (val);
        }else{
            dict[key] = (int) val;
        }
    }
    delegate void foreach_propdict_func (string key, int type,  IntPtr val, string source, IntPtr udata);
    static void foreach_propdict (string key, int type,  IntPtr val, string source, IntPtr udata){
        GCHandle gch = (GCHandle)udata;
        Dictionary<string, object> dict = (Dictionary<string, object>)(gch.Target);
        if(type == (int)xmmsc_result_value_type_t.XMMSC_RESULT_VALUE_TYPE_STRING) {
            dict[key] = Marshal.PtrToStringAnsi (val);
        }else{
            dict[key] = (int) val;
        }
    }

    public void RaiseError(){
        throw new XmmsException(GetError());
    }

    public void NotifierSet(notifier_func func, IntPtr data){    
      xmmsc_result_notifier_set (res.swigCPtr , func, data);
    }
    
    public delegate void notifier_func (IntPtr res, IntPtr udata);
    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_result_notifier_set")]
    extern static void xmmsc_result_notifier_set (HandleRef res, notifier_func f, IntPtr user_data);    
    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_result_dict_foreach")]
    extern static void xmmsc_result_dict_foreach (HandleRef res, foreach_dict_func f, IntPtr user_data);
    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_result_propdict_foreach")]
    extern static void xmmsc_result_propdict_foreach (HandleRef res,  foreach_propdict_func f, IntPtr user_data);
    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_result_get_dict_entry_str")]
    extern static int xmmsc_result_get_dict_entry_str (HandleRef info_res, string  key, StringBuilder outi);
    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_result_get_dict_entry_int32")]
    extern static int xmmsc_result_get_dict_entry_int32 (HandleRef info_res, string  key, out int outi);
    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_result_get_uint")]
    extern static int xmmsc_result_get_uint (HandleRef res, out uint user_data);
    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_entry_format")]
    extern static void xmmsc_entry_format(StringBuilder s , int i , string s2, HandleRef res);


}
}
