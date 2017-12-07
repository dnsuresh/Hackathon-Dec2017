using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZelloVoiceWeb.Controllers
{
    public class StateWatcher
    {        
            // Prevent external instantiation
        private StateWatcher()
        {

        }

        public static StateWatcher Instance
        {
            get
            {
                return InstanceContainer.Instance;
            }
        }

        public AxPttLib.AxPtt AxMesh
        {
            get;
            set;
        }

        //public WorkItem WorkItem
        //{
        //    get;
        //    set;
        //}

        //public Timer PollingTimer;

        //IVoiceProfileService _voiceProfileService;
        /// <summary>
        /// Gets the voice service.
        /// </summary>
        /// 
        /*
        private IVoiceProfileService VoiceProfileService
        {
            get
            {
                if (_voiceProfileService == null)
                {
                    _voiceProfileService = this.WorkItem.Services.Get<IVoiceProfileService>();
                }

                return _voiceProfileService;
            }
        }

        private IContacts Contacts
        {
            get
            {
                IContacts result = null;

                try
                {
                    result = AxMesh.Contacts;
                }
                catch (Exception e)
                {
                    LogHelper.WriteEntry(string.Format("Exception getting Zello contacts.  Exception: {0}", e.ToString()), TraceEventType.Error);
                }

                return result;
            }
        }

        public void Initialize(WorkItem workItem)
        {
            WorkItem = workItem;
            Users = new List<IContact>(GetUsers());

            ((IPtt3)AxMesh.GetOcx()).AutoConnect = false;
            UpdateUserStatus(Users);
            if (!SettingsService.DisableCoherency)
            {
                if (SettingsService.UsePolling)
                {
                    // Using "due time" instead of "period time" to avoid too many calls.
                    PollingTimer = new Timer(PollingTimerCallback, null, (long)SettingsService.PollingInterval.TotalMilliseconds, Timeout.Infinite);
                }
                else
                {
                    AxMesh.ContactListChanged += AxMesh_ContactListChanged;
                }
            }

            if (!SettingsService.DisableVal)
            {
                AxMesh.AudioMessageInStart += AxMesh_AudioMessageInBeginReceived;
                AxMesh.MessageInBegin += AxMesh_MessageInBeginReceived;
                AxMesh.MessageInEnd += AxMesh_MessageInEndReceived;
            }

            OnChannelsInitialized();
        }

        public void PopulateContacts()
        {
            Users = new List<IContact>(GetUsers());
        }

        public void ReInitialize(WorkItem workItem)
        {
            WorkItem = workItem;

            ((IPtt3)AxMesh.GetOcx()).AutoConnect = false;

            Users = new List<IContact>(GetUsers());
            UpdateUserStatus(Users);
            OnChannelsInitialized();
        }

        public List<IContact> Users = new List<IContact>();

        private List<IContact> GetUsers()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var contacts = Contacts;
            List<IContact> result = null;

            if (contacts != null)
            {
                int max = contacts.Count;
                result = new List<IContact>(max);

                for (int i = 0; i < max; i++)
                {
                    IContact contact = contacts.Item[i];
                    if (contact.Type == CONTACT_TYPE.CTUSER)
                    {
                        result.Add(contact);
                    }
                }
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            LogHelper.WriteEntry(string.Format("GetUsers : Time taken (in ms) to get the contacts from ActiveX control is {0}", elapsedMs), TraceEventType.Information);

            return result != null ? result : new List<IContact>();
        }

        // Consider depricating
        public IContact GetChannel(string channelName)
        {
            IContacts contacts = Contacts;
            if (contacts != null)
            {
                int max = contacts.Count;
                for (int i = 0; i < max; ++i)
                {
                    IContact contact = contacts.Item[i];
                    if (contact.Type == CONTACT_TYPE.CTCHANNEL || contact.Type == CONTACT_TYPE.CTGROUP)
                    {
                        if (contact.Name.Equals(channelName))
                            return contact;
                    }
                }
            }
            return null;
        }

        public List<IContact> GetAllChannels()
        {
            var contacts = Contacts;
            List<IContact> result = null;

            if (contacts != null)
            {
                int max = contacts.Count;
                result = new List<IContact>(max);

                for (int i = 0; i < max; ++i)
                {
                    IContact contact = contacts.Item[i];
                    if (contact.Type == CONTACT_TYPE.CTCHANNEL || contact.Type == CONTACT_TYPE.CTGROUP)
                    {
                        result.Add(contact);
                    }
                }
            }

            return result != null ? result : new List<IContact>();
        }

        private int processingList1 = 0;

        void AxMesh_MessageInBeginReceived(object sender, AxPttLib.IPttEvents_MessageInBeginEvent e)
        {
            try
            {
                if (Interlocked.Exchange(ref processingList1, 1) == 0)
                {
                    try
                    {
                        PttLib.IAudioInMessage pMessage = (PttLib.IAudioInMessage)e.pMessage;

                        if (pMessage != null)
                        {
                            PttLib.IContact pContact = pMessage.Sender;

                            if (pContact != null)
                            {
                                OnBeginInMessage(pContact, e);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }
            }
            finally
            {
                Interlocked.Exchange(ref processingList1, 0);
            }
        }

        private void AxMesh_MessageInEndReceived(object sender, AxPttLib.IPttEvents_MessageInEndEvent e)
        {
            try
            {
                // For now I'm omitting the interlocked stuff... I'd like to understand why it was added before I add it.
                PttLib.IAudioInMessage pMessage = (PttLib.IAudioInMessage)e.pMessage;
                if (pMessage != null)
                {
                    PttLib.IContact pContact = pMessage.Sender;

                    if (pContact != null)
                    {
                        OnEndInMessage(pContact, e);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unexpected exception while processing AxMesh_MessageInEndReceived.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.Source);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        void AxMesh_AudioMessageInBeginReceived(object sender, AxPttLib.IPttEvents_AudioMessageInStartEvent e)
        {
            try
            {
                if (Interlocked.Exchange(ref processingList1, 1) == 0)
                {
                    try
                    {
                        PttLib.IAudioInMessage pMessage = (PttLib.IAudioInMessage)e.pMessage;

                        if (pMessage != null)
                        {
                            PttLib.IContact pContact = pMessage.Sender;

                            if (pContact != null)
                            {
                                if (pContact.Name == "unmuted")
                                {
                                    e.pbActivate = true;
                                }
                                if (pContact.Name == "muted")
                                {
                                    e.pbActivate = false;

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }
            }
            finally
            {
                Interlocked.Exchange(ref processingList1, 0);
            }
        }

        private Stopwatch stopwatch = new Stopwatch();
        private int processingList = 0;

        // SCF: I'm not thrilled with this. The problem is that people started using the StateWatcher.Instance to access the Zello SDK
        //		instead of making a proper SCSF service which abstracts it. As a result we don't have easy access to dependency injection here, 
        //		so I'm just hacking in a flag that the voice module can use to let us know when the system is ready to process coherency.

        //Benny: Noticed PerformCoherencyCheck call was happening even when zello is disconnected, creating the problem of channels geting wiped out
        //Made readyForCoherency false on zello disconnect
        private bool readyForCoherency = false;
        public void IndicateReadyForCoherency(bool isReady)
        {
            readyForCoherency = isReady;
        }

        DateTime lastCheck = DateTime.Now.Subtract(new TimeSpan(0, 1, 0));
        void AxMesh_ContactListChanged(object sender, EventArgs e)
        {
            TimeSpan ts = new TimeSpan(lastCheck.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);

            //zello fires the coherency all the time but we don't want to go back to the stack all the time.
            if (ts2.TotalSeconds - ts.TotalSeconds >= SettingsService.CoherencyWaitInterval.TotalSeconds)
            {
                PerformCoherencyCheck();
            }
        }

        private void PollingTimerCallback(object state)
        {
            try
            {
                PerformCoherencyCheck();
            }
            finally
            {
                // Set the next due time
                PollingTimer.Change((long)SettingsService.PollingInterval.TotalMilliseconds, Timeout.Infinite);
            }
        }

        private void PerformCoherencyCheck()
        {
            if (!readyForCoherency)
            {
                return;
            }

            // Don't do a coherency check while one is already going on.
            if (Interlocked.Exchange(ref processingList, 1) == 0)
            {
                try
                {
                    lastCheck = DateTime.Now;
                    ProcessUserCoherency();
                }
                finally
                {
                    Interlocked.Exchange(ref processingList, 0);
                }
            }
        }

        private ISessionService _sessionService = null;
        private ISessionService SessionService
        {
            get
            {
                if (_sessionService == null)
                {
                    _sessionService = this.WorkItem.Services.Get<ISessionService>();
                }
                return _sessionService;
            }
        }

        private Dictionary<string, ONLINE_STATUS> CurrentUserStatus = new Dictionary<string, ONLINE_STATUS>();
        private void ProcessUserCoherency()
        {
            Users = GetUsers();

            IEnumerable<IContact> connectionStatusChanged = Users.Where(user => HasStatusChanged(user)).ToList();

            if (connectionStatusChanged.Any())
            {
                UpdateUserStatus(connectionStatusChanged);
                OnUsersStatusChanged(connectionStatusChanged);

                //Raising this event once per the set, So that the treeviews can subscribe to this instead of the UserChanged event, which is fired for each contact.
                OnContactStatusChanged();
            }
        }

        private bool HasStatusChanged(IContact newUserStatus)
        {
            return (CurrentUserStatus.ContainsKey(newUserStatus.Name) && CurrentUserStatus[newUserStatus.Name] != newUserStatus.Status);
        }

        private void UpdateUserStatus(IEnumerable<IContact> contacts)
        {
            foreach (IContact contact in contacts)
            {
                LogHelper.WriteEntry(string.Format("{0} User status changed to {1}", contact.Name, contact.Status), TraceEventType.Information);
                UpdateUserStatus(contact);
            }
        }

        public ONLINE_STATUS GetUserStatus(string name)
        {
            return (CurrentUserStatus.ContainsKey(name)) ? CurrentUserStatus[name] : ONLINE_STATUS.OSUNKNOWN;
        }

        private void UpdateUserStatus(IContact contact)
        {
            if (CurrentUserStatus.ContainsKey(contact.Name))
            {
                CurrentUserStatus[contact.Name] = contact.Status;
            }
            else
            {
                CurrentUserStatus.Add(contact.Name, contact.Status);
            }
        }

        public IEnumerable<IContact> GetUsers(IContact contact)
        {
            List<IContact> result = new List<IContact>();

            IGroup group = contact as IGroup;

            if (group != null)
            {

            }
            else
            {
                PttLib.IChannel channel = contact as PttLib.IChannel;
            }

            List<IProfile> profileList = VoiceProfileService.GetVoiceProfiles(false).ToList<IProfile>();

            IEnumerable<IProfile> profiles = from p in profileList
                                                where p.TalkGroups.Select(t => t.Name).Contains(contact.Name)
                                                select p;

            foreach (IProfile profile in profiles)
            {
                IContact user = Users.Where<IContact>(u => u.Name == profile.ReferenceId).FirstOrDefault();

                if (user != null)
                {
                    result.Add(user);
                }
            }

            return result;
        }

        public event EventHandler<EventArgs> ChannelsInitialized;
        private void OnChannelsInitialized()
        {
            if (ChannelsInitialized != null)
            {
                ChannelsInitialized(AxMesh, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs<IEnumerable<IContact>>> UsersStatusChanged;

        private void OnUsersStatusChanged(IEnumerable<IContact> users)
        {
            if (UsersStatusChanged != null)
            {
                LogHelper.WriteEntry(string.Format("OnUsersStatusChanged: Users status changed is {0} triggering UsersStatusChanged ", users.Count()), System.Diagnostics.TraceEventType.Information);

                IndicateReadyForCoherency(false); //This will stop the timer to process till the previous process is completed.

                Task.Factory.StartNew(() => UsersStatusChanged(AxMesh, new EventArgs<IEnumerable<IContact>>(users)))
                    .ContinueWith(a => IndicateReadyForCoherency(true));
            }
        }

        public event EventHandler ContactStatusChanged;
        private void OnContactStatusChanged()
        {
            //View/Trees just need only refresh will be subscribing to it.
            if (ContactStatusChanged != null)
            {
                ContactStatusChanged(this, null);
            }
        }

        // need to check how to register this event here and bind it to Presenter
        public event EventHandler<EventArgs<AxPttLib.IPttEvents_MessageInBeginEvent>> BeginInMessage;
        private void OnBeginInMessage(IContact channel, AxPttLib.IPttEvents_MessageInBeginEvent e)
        {
            if (BeginInMessage != null)
            {
                BeginInMessage(channel, new EventArgs<AxPttLib.IPttEvents_MessageInBeginEvent>(e));
            }
        }

        // need to check how to register this event here and bind it to Presenter
        public event EventHandler<EventArgs<AxPttLib.IPttEvents_MessageInEndEvent>> EndInMessage;
        private void OnEndInMessage(IContact channel, AxPttLib.IPttEvents_MessageInEndEvent e)
        {
            if (EndInMessage != null)
            {
                EndInMessage(channel, new EventArgs<AxPttLib.IPttEvents_MessageInEndEvent>(e));
            }
        }
        */

        private class InstanceContainer
        {
            // Prevent early instantiation due to beforefieldinit flag
            static InstanceContainer() { }

            internal static readonly StateWatcher Instance = new StateWatcher();
        }
    }
    }