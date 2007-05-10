
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace xmmssharp
{

/*
 
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_add_to_playlist(SWIGTYPE_p_xmmsc_connection_St c, string query) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_playlists_list(SWIGTYPE_p_xmmsc_connection_St arg0) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_playlist_list(SWIGTYPE_p_xmmsc_connection_St arg0, string playlist) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_get_id(SWIGTYPE_p_xmmsc_connection_St conn, string url) {
 
*/
enum XmmsMediaLibEntryProperty{
    mime,
    id,
    url,
    artist,
    album,
    title,
    date,
    tracknr,
    genre,
    bitrate,
    comment,
    commentlang,
    duration,
    channel,
    samplerate,
    lmod,
    resolved,
    gain_track,
    gain_album,
    peak_track,
    peak_album,
    compilation,
    album_id,
    artist_id,
    track_id,
    added,
    bpm,
    laststarted,
    size,
    isvbr,
    subtunes,
    chain,
    timesplayed,
    partofset,
    picture_front,
    picture_front_mime,
    startsample,
    stopsample,
    available,
    type
}


public class XmmsMedialib
{
    private XmmsConnection connection;
    public XmmsMedialib(XmmsConnection conn)
    {
        connection = conn;
        XmmsResult r = new XmmsResult ( XmmsClientInterface.xmmsc_broadcast_medialib_playlist_loaded (connection.Handle));
        r.NotifierSet(new XmmsResult.notifier_func(playlist_loaded));
        r = new XmmsResult ( XmmsClientInterface.xmmsc_broadcast_medialib_entry_added (connection.Handle));
        r.NotifierSet(new XmmsResult.notifier_func(entry_added));
    }

    public List<XmmsMedialibItem> Query(string sql){
        List<XmmsMedialibItem> lst = new List<XmmsMedialibItem> ();
        XmmsResult res = new XmmsResult(XmmsClientInterface.xmmsc_medialib_select (connection.Handle, sql));
        res.Wait();
        if (res.IsError()) res.RaiseError();

        while (res.IsValidList()) {
            lst.Add(new XmmsMedialibItem(connection,res));
            res.ListNext();
        }

        return lst;
    }
    public XmmsResult PlaylistLoad(string name){
        XmmsResult res = new XmmsResult(XmmsClientInterface.xmmsc_medialib_playlist_load(connection.Handle, name));
        res.Wait();
        if(res.IsError()) res.RaiseError();
        return res;
    }


    public XmmsResult PlaylistSaveCurrent(string name){
        XmmsResult res = new XmmsResult(XmmsClientInterface.xmmsc_medialib_playlist_save_current(connection.Handle, name));
        res.Wait();
        if(res.IsError()) res.RaiseError();
        return res;
    }


    public XmmsResult PlaylistImport(string playlist, string url){
        XmmsResult res = new XmmsResult(XmmsClientInterface.xmmsc_medialib_playlist_import(connection.Handle, playlist, url));
        res.Wait();
        if(res.IsError()) res.RaiseError();
        return res;
    }

    public XmmsResult PlaylistExport(string playlist, string mime){
        XmmsResult res = new XmmsResult(XmmsClientInterface.xmmsc_medialib_playlist_export(connection.Handle, playlist, mime));
        res.Wait();
        if(res.IsError()) res.RaiseError();
        return res;
    }

    public XmmsResult PlaylistRemove(string playlist){
        XmmsResult res = new XmmsResult(XmmsClientInterface.xmmsc_medialib_playlist_remove(connection.Handle, playlist));
        res.Wait();
        if(res.IsError()) res.RaiseError();
        return res;
    }

    public XmmsResult AddEntry(string url)
    {
        XmmsResult res = new XmmsResult(XmmsClientInterface.xmmsc_medialib_add_entry(connection.Handle, url));
        res.Wait();
        if (res.IsError()) res.RaiseError();
        return res;
    }

    public XmmsResult PathImport(string path)
    {
        XmmsResult res = new XmmsResult(XmmsClientInterface.xmmsc_medialib_path_import(connection.Handle, path));
        res.Wait();
        if (res.IsError()) res.RaiseError();
        return res;
    }

    public XmmsResult Select(string query)
    {
        XmmsResult res = new XmmsResult(XmmsClientInterface.xmmsc_medialib_select(connection.Handle, query));
        res.Wait();
        if (res.IsError()) res.RaiseError();
        return res;
    }

    public XmmsResult RemoveEntry(int id){
        IntPtr p2 = xmmsc_medialib_remove_entry ( connection.HandleRef, id);
        XmmsResult info_res =  new XmmsResult (p2);
        info_res.Wait();
        if (info_res.IsError()) info_res.RaiseError();
        return info_res;
    }

    public XmmsResult Rehash(){
        return Rehash(0);
    }

    public XmmsResult Rehash(int id){
        IntPtr p2 = xmmsc_medialib_rehash ( connection.HandleRef, id);
        XmmsResult info_res =  new XmmsResult (p2);
        info_res.Wait();
        if (info_res.IsError()) info_res.RaiseError();
        return info_res;
    }


    public XmmsResult GetInfo(uint ui)
    {
        IntPtr p2 = xmmsc_medialib_get_info ( connection.HandleRef, ui);
        XmmsResult info_res =  new XmmsResult (p2);
        info_res.Wait();
        if (info_res.IsError()) {
            throw new XmmsException(info_res.GetError());
        }
        return info_res;
    }

    public event EntryAddedHandler EntryAdded;
    public delegate void EntryAddedHandler(XmmsResult res, object data);
    public void OnEntryAdded(XmmsResult res, object data){
        if(EntryAdded != null)
            EntryAdded(res, data );
    }

    void entry_added (IntPtr res)
    {
        XmmsResult info_res =  new XmmsResult (res);
        //TODO: fix me up
        OnEntryAdded( info_res, info_res);
    }

    public event PlayListLoadedHandler PlayListLoaded;
    public delegate void PlayListLoadedHandler(XmmsResult res, object data);
    public void OnPlayListLoaded(XmmsResult res, object data){
        if(PlayListLoaded != null)
            PlayListLoaded(res, data );
    }

    void playlist_loaded (IntPtr res)
    {
    // TODO: fix this
        XmmsResult info_res =  new XmmsResult (res);
        OnPlayListLoaded( info_res, info_res);
    }


    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_medialib_get_info")]
    extern static IntPtr xmmsc_medialib_get_info (HandleRef res, uint user_data);
    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_medialib_remove_entry")]
    extern static IntPtr xmmsc_medialib_remove_entry (HandleRef res, int id);
    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_medialib_rehash")]
    extern static IntPtr xmmsc_medialib_rehash (HandleRef res, int id);

}
}
