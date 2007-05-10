
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace xmmssharp
{


public class XmmsMedialibItem
{
    public string mime = "";
    public int id = 0;
    public string url = "";
    public string artist = "";
    public string album = "";
    public string title = "";
    public string date = "";
    public int tracknr = 0;
    public string genre = "";
    public int bitrate = 0;
    public string comment = "";
    public string commentlang = "";
    public int duration = 0;
    public string channel = "";
    public string samplerate = "";
    public int lmod = 0;
    public int resolved = 0;
    public string gain_track = "";
    public string gain_album = "";
    public string peak_track = "";
    public string peak_album = "";
    public string compilation = "";
    public string album_id = "";
    public string artist_id = "";
    public string track_id = "";
    public int added = 0;
    public string bpm = "";
    public string laststarted = "";
    public int size = 0;
    public string isvbr = "";
    public string subtunes = "";
    public string chain = "";
    public int timesplayed = 0;
    public int type = 0;
    public string partofset = "";
    public string picture_front = "";
    public string picture_front_mime = "";
    public string startsample = "";
    public string stopsample = "";
    public string available = "";

    private XmmsConnection connection;

    public XmmsMedialibItem(XmmsConnection con, XmmsResult res){
        connection = con;
        Dictionary<string, object> dict = res.GetDict();
        foreach(string n in XmmsMediaLibEntryProperty.GetNames(typeof(XmmsMediaLibEntryProperty)))
        if (dict.ContainsKey(n)){
            FieldInfo fi = GetType().GetField(n);
            fi.SetValue(this, dict[n]);
        }
    }

    override public string ToString(){
        string s = "";
        foreach(string n in XmmsMediaLibEntryProperty.GetNames(typeof(XmmsMediaLibEntryProperty)))
        {
            FieldInfo fi = GetType().GetField(n);
            object o = fi.GetValue(this);
            s += "prop: " + n + " val: " + o + "\n";
        }

        return s;
    }

    public XmmsMedialibItem()
    {
    }

    public void SetProperty(string key, int val){
        IntPtr p2 = xmmsc_medialib_entry_property_set_int ( connection.HandleRef, id, key, val);
        XmmsResult info_res =  new XmmsResult (p2);
        info_res.Wait();
        if (info_res.IsError()) {
            throw new XmmsException(info_res.GetError());
        }
    }

    public void SetProperty(string key, string val){
        IntPtr p2 = xmmsc_medialib_entry_property_set_str (connection.HandleRef, id, key, val);
        XmmsResult info_res =  new XmmsResult (p2);
        info_res.Wait();
        if (info_res.IsError()) {
            throw new XmmsException(info_res.GetError());
        }
    }

    public void RemoveProperty(string key){
        IntPtr p2 = xmmsc_medialib_entry_property_remove (connection.HandleRef, id, key);
        XmmsResult info_res =  new XmmsResult (p2);
        info_res.Wait();
        if (info_res.IsError()) {
            throw new XmmsException(info_res.GetError());
        }
    }

    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_medialib_entry_property_set_int")]
    extern static IntPtr xmmsc_medialib_entry_property_set_int (HandleRef res, int id, string key, int val);
    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_medialib_entry_property_set_str")]
    extern static IntPtr xmmsc_medialib_entry_property_set_str (HandleRef res, int id, string key, string val);
    [DllImport("XmmsClientInterface", EntryPoint="xmmsc_medialib_entry_property_remove")]
    extern static IntPtr xmmsc_medialib_entry_property_remove (HandleRef res, int id, string key);


}
}
