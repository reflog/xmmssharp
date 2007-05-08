
using System;
using System.Runtime.InteropServices;


namespace xmmssharp
{

/*
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_select(SWIGTYPE_p_xmmsc_connection_St conn, string query) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_playlist_save_current(SWIGTYPE_p_xmmsc_connection_St conn, string name) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_playlist_load(SWIGTYPE_p_xmmsc_connection_St conn, string name) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_add_entry(SWIGTYPE_p_xmmsc_connection_St conn, string url) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_add_entry_args(SWIGTYPE_p_xmmsc_connection_St conn, string url, int numargs, SWIGTYPE_p_p_char args) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_add_entry_encoded(SWIGTYPE_p_xmmsc_connection_St conn, string url) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_get_info(SWIGTYPE_p_xmmsc_connection_St arg0, SWIGTYPE_p_uint32_t arg1) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_add_to_playlist(SWIGTYPE_p_xmmsc_connection_St c, string query) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_playlists_list(SWIGTYPE_p_xmmsc_connection_St arg0) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_playlist_list(SWIGTYPE_p_xmmsc_connection_St arg0, string playlist) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_playlist_import(SWIGTYPE_p_xmmsc_connection_St conn, string playlist, string url) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_playlist_export(SWIGTYPE_p_xmmsc_connection_St conn, string playlist, string mime) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_playlist_remove(SWIGTYPE_p_xmmsc_connection_St conn, string playlist) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_path_import(SWIGTYPE_p_xmmsc_connection_St conn, string path) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_path_import_encoded(SWIGTYPE_p_xmmsc_connection_St conn, string path) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_rehash(SWIGTYPE_p_xmmsc_connection_St conn, SWIGTYPE_p_uint32_t id) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_get_id(SWIGTYPE_p_xmmsc_connection_St conn, string url) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_remove_entry(SWIGTYPE_p_xmmsc_connection_St conn, SWIGTYPE_p_uint32_t entry) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_entry_property_set_int(SWIGTYPE_p_xmmsc_connection_St c, SWIGTYPE_p_uint32_t id, string key, SWIGTYPE_p_int32_t value) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_entry_property_set_int_with_source(SWIGTYPE_p_xmmsc_connection_St c, SWIGTYPE_p_uint32_t id, string source, string key, SWIGTYPE_p_int32_t value) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_entry_property_set_str(SWIGTYPE_p_xmmsc_connection_St c, SWIGTYPE_p_uint32_t id, string key, string value) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_entry_property_set_str_with_source(SWIGTYPE_p_xmmsc_connection_St c, SWIGTYPE_p_uint32_t id, string source, string key, string value) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_entry_property_remove(SWIGTYPE_p_xmmsc_connection_St c, SWIGTYPE_p_uint32_t id, string key) {
 public static SWIGTYPE_p_xmmsc_result_St xmmsc_medialib_entry_property_remove_with_source(SWIGTYPE_p_xmmsc_connection_St c, SWIGTYPE_p_uint32_t id, string source, string key) {

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

}
}
