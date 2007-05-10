using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using xmmssharp;

 public class runme {
     static void OnStatusUpdated(XmmsResult res, xmms_playback_status_t t){
     Console.WriteLine("Staus updated: " + t);
     }
     static void OnPLStatusUpdated(XmmsResult res, xmms_playlist_changed_actions_t a){
     Console.WriteLine("PL Staus updated:"+a);
     }
     static void Main() {
     XmmsConnection c = new XmmsConnection(null);
     if (c.Connect()){
        XmmsPlayback pb = new XmmsPlayback(c);
        XmmsConfig conf = new XmmsConfig(c);
        Console.WriteLine(conf.List());
        pb.StatusUpdated += new XmmsPlayback.StatusUpdatedHandler(OnStatusUpdated);
        XmmsMedialib ml = new XmmsMedialib(c);
        ml.AddEntry("file:///windows/C/Downloads/Linkin Park/01-Wake.mp3");
        XmmsPlaylist pl = new XmmsPlaylist(c);
        pb.Stop();
        pl.PlayListChanged += new XmmsPlaylist.PlayListChangedHandler (OnPLStatusUpdated);
        pl.Clear();
        pl.Add("file:///windows/C/Downloads/Linkin Park/01-Wake.mp3");
        pl.Add("file:///windows/C/Downloads/Linkin Park/01-Wake.mp3");
        pb.Start();

        /*foreach(XmmsPlaylistItem ilst in pl.List()){
            Console.WriteLine("item:"+ilst);
        }*/
        //pb.Stop();
     }else{
     Console.WriteLine("cannot connect");
     }

     }
   
 }
