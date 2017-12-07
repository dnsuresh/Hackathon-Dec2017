using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZelloVoice.Web.Models;
using ZelloVoice.Web.ExportDataService;
using Trimble.MobileSolutions.Business.Entities.Events;

namespace ZelloVoice.Web.Controllers
{
    public class LocationAndEventHistoryController : Controller
    {
        // GET: LocationAndEventHistory
        public ActionResult Index()
        {
            GetData();
            return View();
        }

        ExportDataMapperClient client = new ExportDataMapperClient();



        public void GetData()
        {
            List<long> assets = new List<long>();
            assets.Add(128114);
            // assets.Add(61873);
            IEnumerable<long> assetList = assets.AsEnumerable<long>();
            long[] assetsArray = new long[1] { 128114 };
            string sessionid = Session["Sessionid"].ToString();
            Trimble.MobileSolutions.Business.Entities.Events.Data.ExportPosition[]
                    eventCollection = null;
            Trimble.MobileSolutions.Business.Entities.Events.Data.ExportEvent[] events = null;
            // eventCollection = target.ExportEvents("", startTime, endTime, assetList);
            DateTime startTime = DateTime.Parse("12/07/2017 12:00:00 AM");
            DateTime endTime = DateTime.Parse("12/08/2017 11:59:59 PM");
            try
            {
                eventCollection = (Trimble.MobileSolutions.Business.Entities.Events.Data.ExportPosition[])client.ExportPositions(sessionid, startTime, endTime, assetsArray);
                dynamic d = client.ExportEvents(sessionid, startTime, endTime, assetsArray);
            }
            catch (Exception ex)
            { }
        }
    }

        //    long[] _messageEventIds = {
        //                               //Predefined Message (DIM) Ids 
        //                               1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20
        //                               ,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41
        //                               //Material Added
        //                               //,149
        //                               //Fuel Added (Special DIM)
        //                               ,131
        //                               //Oil Added (Special DIM)
        //                               ,132
        //                               //Freeform Messsage Ids
        //                               ,151 };

        //    long[] _pushButtonMessageIds = {
        //                               //Predefined Message (DIM) Ids 
        //                               1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20
        //                               ,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41
        //                               };

        //    [Flags]
        //    public enum HistoricalFilter
        //    {
        //        None = 0,
        //        Events = 0x01,
        //        Positions = 0x02,
        //        Messages = 0x04,
        //        All = Events | Positions | Messages,
        //    }

        //    public IEnumerable<IAssetHistory> RetrieveHistory(IAsset asset, DateTime fromDateTime, DateTime toDateTime, HistoricalFilter filter)
        //    {
        //        try
        //        {
        //            if (filter == HistoricalFilter.None) return new AssetHistoryCollection();

        //            //DateTime fromUTCDateTime = Module.SessionService.ConvertSessionToUtc(fromDateTime);
        //            // DateTime toUTCDateTime = Module.SessionService.ConvertSessionToUtc(toDateTime);
        //        DateTime fromUTCDateTime = fromDateTime;
        //        DateTime toUTCDateTime = toDateTime;
        //            if (asset == null)
        //            {
        //                throw new ArgumentNullException("Asset");
        //            }

        //            AssetHistoryCollection assetHistoryCollection = new AssetHistoryCollection();

        //            //Retrieve Events
        //            List<IAssetHistory> eventHistoryCollection = new List<IAssetHistory>();
        //            if ((filter & HistoricalFilter.Events) != HistoricalFilter.None)
        //            {
        //                IEnumerable<IAssetHistory> events = RetrieveEvents(asset, fromUTCDateTime, toUTCDateTime);

        //                //If Messaging is enabled 
        //                //and messages are not selected then remove
        //                if ((filter & HistoricalFilter.Messages) == HistoricalFilter.None)
        //                {
        //                    events = from e in events where (!_messageEventIds.Contains(Convert.ToInt64(e.TypeId))) select e;
        //                }

        //                if (events.Any())
        //                {
        //                    eventHistoryCollection.AddRange(events);
        //                }
        //            }

        //            //Retrieve TextMessages
        //            List<IAssetHistory> textMessageHistoryCollection = new List<IAssetHistory>();
        //            if ((filter & HistoricalFilter.Messages) != HistoricalFilter.None)
        //            {
        //                //If events not chosen
        //                //Retrieve only MessageEvents    
        //                //and add it to EventsHistory
        //                if ((filter & HistoricalFilter.Events) == HistoricalFilter.None)
        //                {
        //                    IEnumerable<IAssetHistory> messageEvents = RetrieveMessageEvents(asset, fromUTCDateTime, toUTCDateTime);

        //                    if (messageEvents.Any())
        //                    {
        //                        //If asset is shared local show only non push button messages
        //                        if (((asset.Organization.OrganizationMetadata & OrganizationMetadata.Shared) == OrganizationMetadata.Shared) &&
        //                           ((asset.Organization.OrganizationMetadata & OrganizationMetadata.Local) == OrganizationMetadata.Local))
        //                        {
        //                            //Add Non Push Button Messages
        //                            eventHistoryCollection.AddRange(messageEvents.Where(o => !_pushButtonMessageIds.Contains(Convert.ToInt64(o.TypeId))));
        //                        }
        //                        else
        //                        {
        //                            eventHistoryCollection.AddRange(messageEvents);
        //                        }
        //                    }
        //                }
        //            }

        //            //Merge text messages
        //            SetMessageForEvents(eventHistoryCollection, ref textMessageHistoryCollection);

        //            //Add Events and Messages after Merge
        //            assetHistoryCollection.AddRange(eventHistoryCollection);
        //            assetHistoryCollection.AddRange(textMessageHistoryCollection);

        //            //Retrieve Positions
        //            if ((filter & HistoricalFilter.Positions) != HistoricalFilter.None)
        //            {
        //                IEnumerable<IAssetHistory> positionsEnumerable = RetrievePositions(asset, fromUTCDateTime, toUTCDateTime);

        //                if (positionsEnumerable.Any())
        //                {
        //                    assetHistoryCollection.AddRange(positionsEnumerable);
        //                }
        //            }

        //            return assetHistoryCollection;
        //        }
        //        catch (Exception ex)
        //        {
        //           // ExceptionPolicy.HandleException(ex, ExceptionPolicyNames.MappersPolicy);
        //            if (ex.InnerException != null)
        //            {
        //                throw;
        //            }
        //        }

        //        //ConnectionService.BypassLocalDataSore();
        //        //if (this.LDSDisabled != null)
        //        //{
        //        //    LDSDisabled(this, EventArgs.Empty);
        //        //}

        //        try
        //        {
        //            return RetrieveHistory(asset, fromDateTime, toDateTime, filter);
        //        }
        //        catch (Exception ex)
        //        {
        //           // ExceptionPolicy.HandleException(ex, ExceptionPolicyNames.MappersPolicy);
        //            throw;
        //        }
        //    }

        //    private IEnumerable<IAssetHistory> RetrievePositions(IAsset asset, DateTime fromUTCDateTime, DateTime toUTCDateTime)
        //    {
        //        //IExportDataMapperClient cl;
        //        //cl.ExportData<IExportPosition>(asset, fromUTCDateTime, toUTCDateTime).History
        //        //                            .Select(e => new LocationHistory(e, asset)).OfType<IAssetHistory>();

        //        return ExportDataMapper.ExportData<IExportPosition>(asset, fromUTCDateTime, toUTCDateTime).History
        //                                    .Select(e => new LocationHistory(e, asset)).OfType<IAssetHistory>();
        //    }

        //    private IExportDataMapperClient ExportDataMapper
        //    {
        //        get
        //        {
        //            return GetMapper<IExportData, IExportDataMapperClient>();

        //}
        //    }

        //    public interface IExportDataMapperClient : IExportDataMapper, IMapper<IExportData>, IMapper, IDisposable, IMapper<IExportPosition>, IMapper<IExportEvent>, IMapper<IExportMessageEvent>, IMapper<IExportTextMessage>
        //    {
        //      //  [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        //        IAssetHistory<T> ExportData<T>(IAsset asset, DateTime fromDate, DateTime toDate) where T : IExportData;
        //        //IEnumerable<IAssetHistory<T>> ExportData<T>(IEnumerable<IAsset> assets, DateTime fromDate, DateTime toDate) where T : IExportData;
        //        //[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        //        //IAssetHistory<T> ExportData<T>(string sessionId, IAsset asset, DateTime fromDate, DateTime toDate) where T : IExportData;
        //        //[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        //        //[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        //        //IEnumerable<IAssetHistory<T>> ExportData<T>(string sessionId, IEnumerable<IAsset> assets, DateTime fromDate, DateTime toDate) where T : IExportData;
        //    }

        //    internal void SetMessageForEvents(List<IAssetHistory> eventHistoryCollection, ref List<IAssetHistory> textMessageHistoryCollection)
        //    {
        //        foreach (ITextMessageHistory eventMessageHistory in eventHistoryCollection.OfType<ITextMessageHistory>())
        //        {
        //            IEnumerable<ITextMessageHistory> eventEnumerable = from ITextMessageHistory messageHistory in textMessageHistoryCollection
        //                                                               where (messageHistory.AssetId == eventMessageHistory.AssetId
        //                                                              && (messageHistory.EventTimestamp == eventMessageHistory.EventTimestamp))
        //                                                               select messageHistory;
        //            if (eventEnumerable.Any())
        //            {
        //                ITextMessageHistory messageHistory = eventEnumerable.First();
        //                eventMessageHistory.TextMessage = messageHistory.TextMessage;

        //                //Remove this text message from TextMessage collection
        //                //because it is merged with events
        //                textMessageHistoryCollection.Remove(messageHistory);
        //            }
        //        }
        //    }
        //    List<long> _eventTypesToBeDiscarded;
        //    long _userDataEventTypeId = -1;
        //    long _trackerInitializationEventTypeId = 0;
        //    private List<long> EventTypesToBeDiscarded
        //    {
        //        get
        //        {
        //            if (_eventTypesToBeDiscarded == null)
        //            {
        //                _eventTypesToBeDiscarded = new List<long>();
        //                //Add events that are to be discarded

        //                //Discard user data events
        //                _eventTypesToBeDiscarded.Add(_userDataEventTypeId);
        //                //Discard tracker initialization events
        //                _eventTypesToBeDiscarded.Add(_trackerInitializationEventTypeId);
        //            }

        //            return _eventTypesToBeDiscarded;
        //        }
        //    }

        //    public class TextMessageEventHistory : AssetHistory, ITextMessageHistory, IEventHistory
        //    {
        //        public TextMessageEventHistory(IExportEvent historicalMessageEvent, IAsset asset)
        //            : base(historicalMessageEvent, asset)
        //        {
        //            this.Event = historicalMessageEvent;
        //        }

        //    }
        //        private IEnumerable<IAssetHistory> RetrieveEvents(IAsset asset, DateTime fromUTCDateTime, DateTime toUTCDateTime)
        //    {
        //        IList<IAssetHistory> returnEvents = new List<IAssetHistory>();

        //        IEnumerable<IExportEvent> exportEvents = ExportDataMapper.ExportData<IExportEvent>(asset, fromUTCDateTime, toUTCDateTime).History
        //                .Where(e => !EventTypesToBeDiscarded.Contains(e.EventId));

        //        foreach (IExportEvent exportEvent in exportEvents)
        //        {
        //            //If event id is a message event id
        //            //then add it as a TextMessageEventHistory
        //            if (_messageEventIds.Contains(exportEvent.EventId))
        //            {
        //                //Add as TextMessageEventHistory
        //                returnEvents.Add(new TextMessageEventHistory(exportEvent, asset) { CalculateDIMTextMethod = this.CalculateDIMText });
        //            }
        //            else
        //            {
        //                //Add as regular EventHistory
        //                EventHistory eventHistory = new EventHistory(exportEvent, asset);

        //                if ((this.LDSService == null) || (!this.LDSService.IsLDSAvailable))
        //                {
        //                    eventHistory.CalculateDIMTextMethod = CalculateDIMText;
        //                }

        //                returnEvents.Add(eventHistory);
        //            }
        //        }

        //        return returnEvents.OfType<IAssetHistory>();
        //    }

        //    private IEnumerable<IAssetHistory> RetrieveMessageEvents(IAsset asset, DateTime fromUTCDateTime, DateTime toUTCDateTime)
        //    {
        //        //if ((this.LDSService != null) && (this.LDSService.IsLDSAvailable))
        //        //{
        //        //    return ExportDataMapper.ExportData<IExportMessageEvent>(asset, fromUTCDateTime, toUTCDateTime).History
        //        //                                .Select(e => new TextMessageEventHistory(e, asset) { CalculateDIMTextMethod = this.CalculateDIMText }).OfType<IAssetHistory>();
        //        //}
        //        //else
        //        //{
        //            return ExportDataMapper.ExportData<IExportEvent>(asset, fromUTCDateTime, toUTCDateTime).History
        //                                        .Select(e => new TextMessageEventHistory(e, asset) { CalculateDIMTextMethod = this.CalculateDIMText }).OfType<IAssetHistory>();
        //       // }
        //    }
    }
}