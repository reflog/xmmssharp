
using System;
using System.Runtime.InteropServices;
namespace xmmssharp
{
	
	
	public class XmmsPlayback
	{
        private XmmsConnection connection;
        public XmmsPlayback(XmmsConnection conn)
        {
            connection = conn;
            XmmsResult r = new XmmsResult ( XmmsClientInterface.xmmsc_broadcast_playback_status (connection.Handle));
            r.NotifierSet(new XmmsResult.notifier_func(status_updated), IntPtr.Zero);
        }
        
        public XmmsResult Start(){
            XmmsResult res = new XmmsResult(XmmsClientInterface.xmmsc_playback_start(connection.Handle));
            res.Wait();
            if (res.IsError()) res.RaiseError();
            return res;
        }

        public XmmsResult Pause(){
            XmmsResult res = new XmmsResult(XmmsClientInterface.xmmsc_playback_pause(connection.Handle));
            res.Wait();
            if (res.IsError()) res.RaiseError();
            return res;
        }

        public XmmsResult Stop(){
            XmmsResult res = new XmmsResult(XmmsClientInterface.xmmsc_playback_stop(connection.Handle));
            res.Wait();
            if (res.IsError()) res.RaiseError();
            return res;
        }
        public event StatusUpdatedHandler StatusUpdated;
        public delegate void StatusUpdatedHandler(XmmsResult res, object user_data);
        public void OnStatusUpdated(XmmsResult res, object user_data){
            if(StatusUpdated != null)
                StatusUpdated(res, user_data );
        }
        
        void status_updated (IntPtr res, IntPtr udata)
        {
            GCHandle gch = (GCHandle)udata;
            SWIGTYPE_p_xmmsc_result_St r = new SWIGTYPE_p_xmmsc_result_St(res, true);
            XmmsResult info_res =  new XmmsResult (r);            
            OnStatusUpdated( info_res, gch.Target);
            gch.Free();
        }
        

	}
}
