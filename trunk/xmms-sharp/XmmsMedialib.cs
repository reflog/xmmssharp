
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace xmmssharp
{

/*
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_playlist_save_current(SWIGTYPE_p_xmmsc_connection_St conn, string name) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_playlist_load(SWIGTYPE_p_xmmsc_connection_St conn, string name) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_get_info(SWIGTYPE_p_xmmsc_connection_St arg0, SWIGTYPE_p_uint32_t arg1) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_add_to_playlist(SWIGTYPE_p_xmmsc_connection_St c, string query) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_playlists_list(SWIGTYPE_p_xmmsc_connection_St arg0) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_playlist_list(SWIGTYPE_p_xmmsc_connection_St arg0, string playlist) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_playlist_import(SWIGTYPE_p_xmmsc_connection_St conn, string playlist, string url) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_playlist_export(SWIGTYPE_p_xmmsc_connection_St conn, string playlist, string mime) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_playlist_remove(SWIGTYPE_p_xmmsc_connection_St conn, string playlist) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_rehash(SWIGTYPE_p_xmmsc_connection_St conn, SWIGTYPE_p_uint32_t id) {
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
        HandleRef p = SWIGTYPE_p_xmmsc_connection_St.getCPtr(connection.Handle);
        IntPtr p2 = xmmsc_medialib_remove_entry ( p, id);
        SWIGTYPE_p_xmmsc_result_St res = new SWIGTYPE_p_xmmsc_result_St(p2, true);
        XmmsResult info_res =  new XmmsResult (res);
        info_res.Wait();
        if (info_res.IsError()) {
            throw new XmmsException(info_res.GetError());
        }
        return info_res;
    }
    
    public XmmsResult GetInfo(uint ui)
    {
        HandleRef p = SWIGTYPE_p_xmmsc_connection_St.getCPtr(connection.Handle);
        IntPtr p2 = xmmsc_medialib_get_info ( p, ui);
        SWIGTYPE_p_xmmsc_result_St res = new SWIGTYPE_p_xmmsc_result_St(p2, true);
        XmmsResult info_res =  new XmmsResult (res);
        info_res.Wait();
        if (info_res.IsError()) {
            throw new XmmsException(info_res.GetError());
        }
        return info_res;
    }

    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_medialib_get_info")]
    extern static IntPtr xmmsc_medialib_get_info (HandleRef res, uint user_data);
    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_medialib_remove_entry")]
    extern static IntPtr xmmsc_medialib_remove_entry (HandleRef res, int id);

}
}
