
using System;
using System.Runtime.InteropServices;
namespace xmmssharp
{
	
	/*
  public static SWIGTYPE_p_xmmsc_result_St xmmsc_playback_tickle(SWIGTYPE_p_xmmsc_connection_St c) {
  public static SWIGTYPE_p_xmmsc_result_St xmmsc_playback_current_id(SWIGTYPE_p_xmmsc_connection_St c) {
  public static SWIGTYPE_p_xmmsc_result_St xmmsc_playback_seek_ms(SWIGTYPE_p_xmmsc_connection_St c, SWIGTYPE_p_uint32_t milliseconds) {
  public static SWIGTYPE_p_xmmsc_result_St xmmsc_playback_seek_ms_rel(SWIGTYPE_p_xmmsc_connection_St c, int milliseconds) {
  public static SWIGTYPE_p_xmmsc_result_St xmmsc_playback_seek_samples(SWIGTYPE_p_xmmsc_connection_St c, SWIGTYPE_p_uint32_t samples) {
  public static SWIGTYPE_p_xmmsc_result_St xmmsc_playback_seek_samples_rel(SWIGTYPE_p_xmmsc_connection_St c, int samples) {
  public static SWIGTYPE_p_xmmsc_result_St xmmsc_playback_playtime(SWIGTYPE_p_xmmsc_connection_St c) {
  public static SWIGTYPE_p_xmmsc_result_St xmmsc_playback_status(SWIGTYPE_p_xmmsc_connection_St c) {
  public static SWIGTYPE_p_xmmsc_result_St xmmsc_playback_volume_set(SWIGTYPE_p_xmmsc_connection_St c, string channel, SWIGTYPE_p_uint32_t volume) {
  public static SWIGTYPE_p_xmmsc_result_St xmmsc_playback_volume_get(SWIGTYPE_p_xmmsc_connection_St c) {
  public static SWIGTYPE_p_xmmsc_result_St xmmsc_signal_playback_playtime(SWIGTYPE_p_xmmsc_connection_St c) {
	
	*/
	public class XmmsPlayback
	{
        private XmmsConnection connection;
        public XmmsPlayback(XmmsConnection conn)
        {
            connection = conn;
            XmmsResult r = new XmmsResult ( XmmsClientInterface.xmmsc_broadcast_playback_status (connection.Handle));
            r.NotifierSet(new XmmsResult.notifier_func(status_updated));
            r = new XmmsResult ( XmmsClientInterface.xmmsc_broadcast_playback_volume_changed (connection.Handle));
            r.NotifierSet(new XmmsResult.notifier_func(volume_changed));
            r = new XmmsResult ( XmmsClientInterface.xmmsc_broadcast_playback_current_id (connection.Handle));
            r.NotifierSet(new XmmsResult.notifier_func(current_id));
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
        
        public event VolumeChangedHandler VolumeChanged;
        public delegate void VolumeChangedHandler(XmmsResult res, int data);
        public void OnVolumeChanged(XmmsResult res, int data){
            if(VolumeChanged != null)
                VolumeChanged(res, data );
        }

        public event StatusUpdatedHandler StatusUpdated;
        public delegate void StatusUpdatedHandler(XmmsResult res, xmms_playback_status_t status);
        public void OnStatusUpdated(XmmsResult res, xmms_playback_status_t status){
            if(StatusUpdated != null)
                StatusUpdated(res, status);
        }

        public event CurrentIdHandler CurrentId;
        public delegate void CurrentIdHandler(XmmsResult res, int id);
        public void OnCurrentId(XmmsResult res, int id){
            if(CurrentId != null)
                CurrentId(res, id);
        }

        
        void status_updated (IntPtr res)
        {
            XmmsResult r = new XmmsResult ( res );                    
            OnStatusUpdated( r, (xmms_playback_status_t)r.GetUint());
        }

        void current_id (IntPtr res)
        {
            XmmsResult r = new XmmsResult ( res );                    
            OnCurrentId( r, (int) r.GetUint());
        }

        void volume_changed (IntPtr res)
        {
            XmmsResult r = new XmmsResult ( res );                    
            OnVolumeChanged( r, (int)r.GetUint());
        }

        

	}
}
