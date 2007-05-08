
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace xmmssharp
{
/*


 public static SWIGTYPE_p_xmmsc_result_St xmmsc_playlist_sort(SWIGTYPE_p_xmmsc_connection_St c, string arg1) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_playlist_set_next(SWIGTYPE_p_xmmsc_connection_St c, SWIGTYPE_p_uint32_t arg1) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_playlist_set_next_rel(SWIGTYPE_p_xmmsc_connection_St c, SWIGTYPE_p_int32_t arg1) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_playlist_move(SWIGTYPE_p_xmmsc_connection_St c, SWIGTYPE_p_uint32_t arg1, SWIGTYPE_p_uint32_t arg2) {
 
*/

public class XmmsPlaylist
{
    private XmmsConnection connection;
    public XmmsPlaylist(XmmsConnection conn)
    {
        connection = conn;
        XmmsResult r = new XmmsResult ( XmmsClientInterface.xmmsc_broadcast_playlist_changed (connection.Handle));
        r.NotifierSet(new XmmsResult.notifier_func(playlist_changed), IntPtr.Zero);
    }

    public XmmsResult Add(string uri){
        XmmsResult res = new XmmsResult(XmmsClientInterface.xmmsc_playlist_add(connection.Handle, uri));
        res.Wait();
        if (res.IsError()) res.RaiseError();
        return res;
    }

    public XmmsResult Remove(uint idx){
        HandleRef p = SWIGTYPE_p_xmmsc_connection_St.getCPtr(connection.Handle);
        IntPtr p2 = xmmsc_playlist_remove(p, idx);
        SWIGTYPE_p_xmmsc_result_St res = new SWIGTYPE_p_xmmsc_result_St(p2, true);
        XmmsResult ress = new XmmsResult(res);
        ress.Wait();
        if (ress.IsError()) ress.RaiseError();
        return ress;        
    }

    public uint CurrentPos(){
        XmmsResult res = new XmmsResult(XmmsClientInterface.xmmsc_playlist_current_pos(connection.Handle));
        res.Wait();
        if (res.IsError()) res.RaiseError();
        return res.GetUint();
    }
    
    public XmmsResult Shuffle(){
        XmmsResult res = new XmmsResult(XmmsClientInterface.xmmsc_playlist_shuffle(connection.Handle));
        res.Wait();
        if (res.IsError()) res.RaiseError();
        return res;
    }


    public XmmsResult RAdd(string uri){
        XmmsResult res = new XmmsResult(XmmsClientInterface.xmmsc_playlist_radd(connection.Handle, uri));
        res.Wait();
        if (res.IsError()) res.RaiseError();
        return res;
    }

    public XmmsResult Add(uint id){
        HandleRef p = SWIGTYPE_p_xmmsc_connection_St.getCPtr(connection.Handle);
        IntPtr p2 = xmmsc_playlist_add_id(p, id);
        SWIGTYPE_p_xmmsc_result_St res = new SWIGTYPE_p_xmmsc_result_St(p2, true);
        XmmsResult ress = new XmmsResult(res);
        ress.Wait();
        if (ress.IsError()) ress.RaiseError();
        return ress;
    }

    public XmmsResult Insert(string uri, uint pos){
        HandleRef p = SWIGTYPE_p_xmmsc_connection_St.getCPtr(connection.Handle);
        IntPtr p2 = xmmsc_playlist_insert(p, pos, uri);
        SWIGTYPE_p_xmmsc_result_St res = new SWIGTYPE_p_xmmsc_result_St(p2, true);
        XmmsResult ress = new XmmsResult(res);
        ress.Wait();
        if (ress.IsError()) ress.RaiseError();
        return ress;
    }

    public XmmsResult Insert(uint id, uint pos){
        HandleRef p = SWIGTYPE_p_xmmsc_connection_St.getCPtr(connection.Handle);
        IntPtr p2 = xmmsc_playlist_insert_id(p, pos, id);
        SWIGTYPE_p_xmmsc_result_St res = new SWIGTYPE_p_xmmsc_result_St(p2, true);
        XmmsResult ress = new XmmsResult(res);
        ress.Wait();
        if (ress.IsError()) ress.RaiseError();
        return ress;
    }


    public List<XmmsMedialibItem> List(){
        List<XmmsMedialibItem> lst = new List<XmmsMedialibItem>();
        XmmsResult res =  new XmmsResult (XmmsClientInterface.xmmsc_playlist_list(connection.Handle));
        res.Wait();
        if (res.IsError()) {
            throw new XmmsException(res.GetError());
        }

        XmmsMedialib lib = new XmmsMedialib(connection);
        while (res.IsValidList()) {
            uint ui = res.GetUint();

            XmmsResult info_res = lib.GetInfo(ui);

            if (info_res.DictHasKey ("title")) {
                lst.Add(new XmmsMedialibItem(info_res));
            }
            res.ListNext();
        }


        return lst;

    }

    public void Clear(){
        XmmsResult res = new XmmsResult(XmmsClientInterface.xmmsc_playlist_clear(connection.Handle));
        res.Wait();
    }
    
        public event PlayListChangedHandler PlayListChanged;
        public delegate void PlayListChangedHandler(XmmsResult res, xmms_playlist_changed_actions_t action);
        public void OnPlayListChanged(XmmsResult res, xmms_playlist_changed_actions_t action){
            if(PlayListChanged != null)
                PlayListChanged(res, action );
        }
        
        void playlist_changed (IntPtr res, IntPtr udata)
        {
        
            SWIGTYPE_p_xmmsc_result_St r = new SWIGTYPE_p_xmmsc_result_St(res, true);
            XmmsResult info_res =  new XmmsResult (r);           
            OnPlayListChanged( info_res, (xmms_playlist_changed_actions_t)info_res.GetDictInt32("type") );
            
        }


    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_playlist_remove")]
    extern static IntPtr xmmsc_playlist_remove(HandleRef res, uint id);
    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_playlist_add_id")]
    extern static IntPtr xmmsc_playlist_add_id(HandleRef res, uint id);
    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_playlist_insert")]
    extern static IntPtr xmmsc_playlist_insert(HandleRef res, uint pos, string s);
    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_playlist_insert_id")]
    extern static IntPtr xmmsc_playlist_insert_id(HandleRef res, uint pos, uint id);
}
}
