using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PttLib;

using Trimble.MobileSolutions.Business.Entities;
using Trimble.MobileSolutions.Business.Entities.Assets;
using Trimble.MobileSolutions.Business.Entities.Security;
using Trimble.MobileSolutions.Business.Entities.Voice;
using Trimble.MobileSolutions.Data.Mappers.Voice;
using Trimble.MobileSolutions.Infrastructure.Interface.Services;
/*
using Trimble.MobileSolutions.Infrastructure.Interface.Services;
using Trimble.MobileSolutions.Modules.Asset.Interface.Model;
using Trimble.MobileSolutions.Modules.Asset.Interface.Services;
using Trimble.MobileSolutions.Modules.Voice.Interface;
using Trimble.MobileSolutions.Modules.Voice.Interface.Constants;
using Trimble.MobileSolutions.Modules.Voice.Interface.Services;
using Trimble.MobileSolutions.Modules.ZelloVoice.Properties;
using Trimble.MobileSolutions.Utility.Events;
using Trimble.MobileSolutions.Utility.Logging;
*/

namespace ZelloVoiceWeb.Controllers
{
    public class ZelloVoiceService
    {
        private AxPttLib.AxPtt _axMesh { get; set; }

        ISessionService _sessionService;
        ISessionService SessionService
        {
            get
            {
                if (_sessionService == null)
                {
                    throw new InvalidOperationException("Zello VoiceService: The session service has not been provided. This should be a rare exception. If the problem persists, please contact support.");
                }
                return _sessionService;
            }
        }

        IVoiceProfileMapperPlatformWebService _profileMapper = null;
        public IVoiceProfileMapperPlatformWebService ProfileMapper
        {
            get
            {
                if (_profileMapper == null)
                {
                    _profileMapper = Trimble.MobileSolutions.Data.Mappers.Factory.GetMapper<Trimble.MobileSolutions.Business.Entities.Voice.IProfile>() as Trimble.MobileSolutions.Data.Mappers.Voice.IVoiceProfileMapperPlatformWebService;
                }
                return _profileMapper;
            }
        }


        void _axMesh_SignInSucceeded(object sender, EventArgs e)
        {
            //TelemetryService.TrackEvent("Zello voice signed on");
            //LogHelper.WriteEntry("axMesh_SignInSucceeded: Start", System.Diagnostics.TraceEventType.Information);
            //if (!initilized)
            //{
            //    initilized = true;
            //    StateWatcher.Instance.Initialize(WorkItem);
            //}
            //else
            //{
            //    LogHelper.WriteEntry("axMesh_SignInSucceeded: Reconnecting", System.Diagnostics.TraceEventType.Information);
            //    //Reconnect needs to refesh the Available Channels for the channels to work correctly              
            //    Trimble.MobileSolutions.Modules.ZelloVoice.Model.Channels.ClearChannels();
            //    StateWatcher.Instance.ReInitialize(WorkItem);
            //    VoiceTalkGroupsService.LoadMuteSettings();
            //    //Setting it to true, it was set false on disconnect
            //    StateWatcher.Instance.IndicateReadyForCoherency(true);
            //}
            //VoiceSignedIn = true;
            //InitializePresence();
            //OnZelloLogonSucceeded(this, null);
            //this.State = ConnectionState.Connected;
            //if (NeedsToResetReadyForCoherency)
            //{
            //    //Setting it to true again as SignInFailed can happen after it was set to true from the module controller
            //    StateWatcher.Instance.IndicateReadyForCoherency(true);
            //    NeedsToResetReadyForCoherency = false;
            //}
            //LogHelper.WriteEntry("axMesh_SignInSucceeded: End", System.Diagnostics.TraceEventType.Information);
        }

        //[EventPublication(EventTopicNames.ZelloLogonSucceeded, PublicationScope.Global)]
        //public event EventHandler OnZelloLogonSucceeded;

        void _axMesh_SignInStarted(object sender, EventArgs e)
        {
            //LogHelper.WriteEntry("_axMesh_SignInStarted", System.Diagnostics.TraceEventType.Information);
            //this.State = ConnectionState.Connecting;
        }

        void _axMesh_SignOutComplete(object sender, EventArgs e)
        {
            //LogHelper.WriteEntry("_axMesh_SignOutComplete", System.Diagnostics.TraceEventType.Information);
            //this.State = ConnectionState.Disconnected;
            //VoiceSignedIn = false;
            //StateWatcher.Instance.IndicateReadyForCoherency(false);
        }

        void _axMesh_SignOutStarted(object sender, EventArgs e)
        {
            //LogHelper.WriteEntry("_axMesh_SignOutStarted", System.Diagnostics.TraceEventType.Information);
            //this.State = ConnectionState.Disconnecting;
            //VoiceSignedIn = false;
            //StateWatcher.Instance.IndicateReadyForCoherency(false);
        }

        public void Connect()
        {
            // Logon
            //switch (State)
            //{
            //    case ConnectionState.Disconnected:
            //        InternalConnect();
            //        break;
            //    case ConnectionState.Disconnecting:
            //        // TODO 
            //        break;
            //    case ConnectionState.Connected:
            //    case ConnectionState.Connecting:
            //        // Ignore
            //        break;
            //}
        }

        public void WireEvents()
        {
            _axMesh.SignOutStarted += _axMesh_SignOutStarted;
            _axMesh.SignOutComplete += _axMesh_SignOutComplete;
            _axMesh.SignInStarted += _axMesh_SignInStarted;
            _axMesh.SignInSucceeded += _axMesh_SignInSucceeded;
           // _axMesh.SignInFailed += _axMesh_SignInFailed;
           // StateWatcher.Instance.UsersStatusChanged += Instance_UsersStatusChanged;
        }
    }
}