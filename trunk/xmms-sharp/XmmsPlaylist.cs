
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace xmmssharp
{

public class XmmsPlaylist
{
    private XmmsConnection connection;
    public XmmsPlaylist(XmmsConnection conn)
    {
        connection = conn;
        XmmsResult r = new XmmsResult ( XmmsClientInterface.xmmsc_broadcast_playlist_changed (connection.Handle));
        r.NotifierSet(new XmmsResult.notifier_func(playlist_changed));
        r = new XmmsResult ( XmmsClientInterface.xmmsc_broadcast_playlist_current_pos (connection.Handle));
        r.NotifierSet(new XmmsResult.notifier_func(playlist_current_pos));
    }


    public XmmsResult Sort(string properties){
        XmmsResult res = new XmmsResult(XmmsClientInterface.xmmsc_playlist_sort(connection.Handle, properties));
        res.Wait();
        if (res.IsError()) res.RaiseError();
        return res;
    }

    public XmmsResult Add(string uri){
        XmmsResult res = new XmmsResult(XmmsClientInterface.xmmsc_playlist_add(connection.Handle, uri));
        res.Wait();
        if (res.IsError()) res.RaiseError();
        return res;
    }

    public XmmsResult Remove(uint idx){
        IntPtr p2 = xmmsc_playlist_remove(connection.HandleRef, idx);
        XmmsResult ress = new XmmsResult(p2);
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
        IntPtr p2 = xmmsc_playlist_add_id(connection.HandleRef, id);
        XmmsResult ress = new XmmsResult(p2);
        ress.Wait();
        if (ress.IsError()) ress.RaiseError();
        return ress;
    }

    public XmmsResult Insert(string uri, uint pos){
        IntPtr p2 = xmmsc_playlist_insert(connection.HandleRef, pos, uri);
        XmmsResult ress = new XmmsResult(p2);
        ress.Wait();
        if (ress.IsError()) ress.RaiseError();
        return ress;
    }

    public XmmsResult Insert(uint id, uint pos){
        IntPtr p2 = xmmsc_playlist_insert_id(connection.HandleRef, pos, id);
        XmmsResult ress = new XmmsResult(p2);
        ress.Wait();
        if (ress.IsError()) ress.RaiseError();
        return ress;
    }


    public XmmsResult Move(int pos1, int pos2){
        IntPtr p2 = xmmsc_playlist_move(connection.HandleRef, pos1, pos2);
        XmmsResult ress = new XmmsResult(p2);
        ress.Wait();
        if (ress.IsError()) ress.RaiseError();
        return ress;
    }

    public XmmsResult SetNextRel(int pos){
        IntPtr p2 = xmmsc_playlist_set_next_rel(connection.HandleRef, pos);
        XmmsResult ress = new XmmsResult(p2);
        ress.Wait();
        if (ress.IsError()) ress.RaiseError();
        return ress;
    }

    public XmmsResult SetNext(int pos){
        IntPtr p2 = xmmsc_playlist_set_next(connection.HandleRef, pos);
        XmmsResult ress = new XmmsResult(p2);
        ress.Wait();
        if (ress.IsError()) ress.RaiseError();
        return ress;
    }


    public List<XmmsMedialibItem> List(){
        List<XmmsMedialibItem> lst = new List<XmmsMedialibItem>();
        XmmsResult res =  new XmmsResult (XmmsClientInterface.xmmsc_playlist_list(connection.Handle));
        res.Wait();
        if (res.IsError()) res.RaiseError();

        XmmsMedialib lib = new XmmsMedialib(connection);
        while (res.IsValidList()) {
            uint ui = res.GetUint();

            XmmsResult info_res = lib.GetInfo(ui);

            if (info_res.DictHasKey ("title")) {
                lst.Add(new XmmsMedialibItem(connection, info_res));
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

    void playlist_changed (IntPtr res)
    {
        XmmsResult info_res =  new XmmsResult (res);
        OnPlayListChanged( info_res, (xmms_playlist_changed_actions_t)info_res.GetDictInt32("type") );
    }

    public event PlayListCurrentPosHandler PlayListCurrentPos;
    public delegate void PlayListCurrentPosHandler(XmmsResult res, uint pos);
    public void OnPlayListCurrentPos(XmmsResult res, uint pos){
        if(PlayListCurrentPos != null)
            PlayListCurrentPos(res, pos );
    }

    void playlist_current_pos (IntPtr res)
    {
        XmmsResult info_res =  new XmmsResult (res);
        OnPlayListCurrentPos( info_res, info_res.GetUint() );
    }


    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_playlist_set_next")]
    extern static IntPtr xmmsc_playlist_set_next(HandleRef res, int pos);
    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_playlist_set_next_rel")]
    extern static IntPtr xmmsc_playlist_set_next_rel(HandleRef res, int pos);
    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_playlist_move")]
    extern static IntPtr xmmsc_playlist_move(HandleRef res, int pos1, int pos2);

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
