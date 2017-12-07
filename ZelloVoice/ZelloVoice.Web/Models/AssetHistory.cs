using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZelloVoice.Web.Models
{
    public class AssetHistory : IAssetHistory, IDisposable
    {
        #region data members
        /// <summary>
        /// Unique identifier of this AssetHistory object
        /// </summary>
        private Guid _guid = Guid.Empty;

        /// <summary>
        /// formatted date time to prevent constant date conversion operations from occurring each
        /// time the date is retrieved (which occurs each time an infragistics data grid is scrolled)
        /// </summary>
        private string _formattedDateTime = string.Empty;

        /// <summary>
        /// Timezone in which this event or position or other piece of asset history occurred
        /// </summary>
        private string _timeZoneName = string.Empty;

        /// <summary>
        /// Address at which this piece of asset history occurred
        /// </summary>
        private string _address = null;

        /// <summary>
        /// Direction that the asset was heading at the time that this piece of history was recorded
        /// </summary>
        private string _directionString = String.Empty;

        /// <summary>
        /// Address street name, line 01
        /// </summary>
        private string _addressStreetNameLine01 = null;

        /// <summary>
        /// Address street name, line 02
        /// </summary>
        private string _addressStreetNameLine02 = null;

        /// <summary>
        /// Used to retrieve actual date/time asynchronously
        /// </summary>
      //  private BackgroundWorker _bgwActualTimeWorker = null;

        /// <summary>
        /// Used to retrieve time zone asynchronously
        /// </summary>
     //   private BackgroundWorker _bgwTimeZoneWorker = null;

        /// <summary>
        /// Reverse geocoded data for this entity (given the lat/lon of where this piece of history was recorded)
        /// </summary>
        protected dynamic _reverseGeocodedData = null;

        /// <summary>
        /// Instance level lock used for accessing dictionary
        /// </summary>
        protected readonly object _asyncLoadReverseGeocodedDataLock = new object();

        /// <summary>
        /// In-memory cache of latlon keyed reverse geocoded objects
        /// </summary>
        private static Dictionary<string, dynamic> s_dictionaryOfLatLonsToGeocodedData = null;

        /// <summary>
        /// Class level lock used for accessing dictionary
        /// </summary>
        //private static readonly object s_dictionaryOfLatLonsToGeocodedDataLock = new object();
        #endregion


        #region services
        /// <summary>
        /// Geocode Service
        /// </summary>
       // private static MapServices.IGeocode s_geoCodeService = null;

        /// <summary>
        /// TimeZone Service 
        /// </summary>
      //  private static ITimeZoneService s_timeZoneService = null;

      //  private static IMapService s_mapService = null;
        /// <summary>
        /// TimeZoneOffsetHelper Service
        /// </summary>
    //    private static MapServices.ITimeZoneOffsetHelper s_timeZoneOffsetHelperService = null;

        /// <summary>
        /// UserSettings Service
        /// </summary>
      //  private static UserSettingsService s_userSettingsService = null;

      //  private static CapabilityService s_capabilityService = null;

        public static IEnumerable<KeyValuePair<DateTime, string>> LatLonReverseGeocodeResult = null;

        public static bool IsALKMap = false;
        #endregion


        //private static IConnectionService s_connectionService
        //{
        //    get
        //    {
        //        return Trimble.MobileSolutions.Data.Mappers.Factory.ConfiguredConnectionService;
        //    }
        //}

        #region delegates
        /// <summary>
        /// Describes method type capable of converting/formatting a date time instance
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public delegate DateTime DateTimeConversion(DateTime value);
        #endregion


        #region events
        /// <summary>
        /// INotifyPropertyChanged PropertyChanged event
        /// </summary>
      //  public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        #region properties
        /// <summary>
        /// The entity for which this AssetHistory object describes
        /// </summary>
        protected IHistoricalLocation Entity
        {
            get;
            private set;
        }

        /// <summary>
        /// Inner Asset instance for which this AssetHistory object describes
        /// </summary>
        public IAsset Asset
        {
            get;
            private set;
        }

        /// <summary>
        /// Globally unique identifier of this AssetHistory object
        /// </summary>
        public string Id
        {
            get { return this.GetDefaultId(); }
        }

        /// <summary>
        /// Defined by the subclass
        /// </summary>
        public long TypeId
        {
            get { return this.GetDefaultTypeId(); }
        }

        /// <summary>
        /// DateTimeConverter
        /// </summary>
        public DateTimeConversion DateTimeConverter { get; private set; }

        /// <summary>
        /// Timestamp associated with this record
        /// Note: The Location and Events History (LEH) window data grid, by default, sorts on this property and
        /// so therefore we should try to avoid formatting here (we return the UTC timestamp rather than the formatted timestamp)
        /// </summary>
        public virtual DateTime Date
        {
            get
            {
                if (null != this.Entity)
                {
                    return this.Entity.Timestamp;
                }
                else
                    return DateTime.MinValue;
            }

        }

        /// <summary>
        /// Stringized timestamp associated with this record (formatted and, if required, in the actual time zone)
        /// </summary>
        public string FormattedDateTime
        {
            get
            {
                if (null == this._formattedDateTime || this._formattedDateTime.Equals(Properties.Resources.Loading, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (null != this._bgwActualTimeWorker && !this._bgwActualTimeWorker.IsBusy)
                        this._bgwActualTimeWorker.RunWorkerAsync();
                    else
                    {
                        ComputeFormattedDate(string.Empty);
                    }
                }

                // return to caller
                return this._formattedDateTime;
            }
        }

        /// <summary>
        /// Timezone in which this event or position or other piece of asset history occurred
        /// </summary>
        public string TimeZoneName
        {
            get
            {
                if (null == this._timeZoneName || this._timeZoneName.Equals(Properties.Resources.Loading, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (null != this._bgwTimeZoneWorker && !this._bgwTimeZoneWorker.IsBusy)
                        this._bgwTimeZoneWorker.RunWorkerAsync();
                    else
                    {
                        this._timeZoneName = string.Empty;
                    }
                }

                // return to caller
                return this._timeZoneName;
            }
        }

        /// <summary>
        /// Address at which this piece of asset history occurred
        /// </summary>
        public string Address
        {
            get
            {
                if (null == this._address)
                    this._address = this.GetAddress();

                return this._address;
            }
        }

        /// <summary>
        /// Defined by the subclass
        /// </summary>
        public string Name
        {
            get { return this.GetDefaultName(); }
        }

        /// <summary>
        /// Description for this piece of asset history
        /// </summary>
        public string Description
        {
            get { return this.GetDefaultDescription(); }
        }

        /// <summary>
        /// Site at which this piece of asset history occurred
        /// </summary>
        public string SiteName
        {
            get { return this.GetDefaultSiteName(); }
        }

        /// <summary>
        /// Mileage of the asset at the time that this piece of history occurred
        /// </summary>
        public double? Mileage
        {
            get
            {
                return this.MileageEntity == null ? (double?)null :
                    Business.Entities.Properties.Settings.Default.Data.UnitTypeUnitSystem[typeof(IDistance)] == UnitSystem.English ?
                            this.MileageEntity.Miles : this.MileageEntity.Kilometers;
            }
        }

        /// <summary>
        /// Speed of the asset at the time that this piece of history occurred
        /// </summary>
        public double? Speed
        {
            get
            {
                return this.SpeedEntity == null ? (double?)null :
                    Business.Entities.Properties.Settings.Default.Data.UnitTypeUnitSystem[typeof(IDistance)] == UnitSystem.English ?
                    this.SpeedEntity.MilesPerHour : this.SpeedEntity.KilometersPerHour;
            }
        }

        /// <summary>
        /// Mileage Entity
        /// </summary>
        public IDistance MileageEntity
        {
            get
            {
                return this.GetMileageEntity();
            }
        }

        /// <summary>
        /// Speed Entity
        /// </summary>
        public ISpeed SpeedEntity
        {
            get
            {
                return this.GetSpeedEntity();
            }
        }

        /// <summary>
        /// Heading at the time of this piece of history
        /// </summary>
        public double? Heading
        {
            get { return this.GetHeading(); }
        }

        /// <summary>
        /// Direction that the asset was heading at the time that this piece of history was recorded
        /// </summary>
        public string Direction
        {
            get
            {
                if (String.IsNullOrEmpty(this._directionString))
                    this._directionString = this.GetDirectionString();

                // return to caller
                return this._directionString;
            }
        }

        /// <summary>
        /// Recorded latitude for this piece of asset history
        /// </summary>
        public double? Latitude
        {
            get
            {
                return this.GetLatitude();
            }
        }

        /// <summary>
        /// Recorded longitude for this piece of asset history
        /// </summary>
        public double? Longitude
        {
            get
            {
                return this.GetLongitude();
            }
        }

        /// <summary>
        /// Hydraulic pressure (entity) at the time that this piece of history was recorded
        /// </summary>
        public IPressure HydraulicPressureEntity
        {
            get
            {
                return this.GetHydraulicPressureEntity();
            }
        }

        /// <summary>
        /// Hydraulic pressure (primitive value) at the time that this piece of history was recorded
        /// </summary>
        public double? HydraulicPressure
        {
            get
            {
                return null == this.HydraulicPressureEntity ? (double?)null :
                    Business.Entities.Properties.Settings.Default.Data.UnitTypeUnitSystem[typeof(IPressure)] == UnitSystem.English ?
                            this.HydraulicPressureEntity.PoundsPerSquareInch : this.HydraulicPressureEntity.Bars;
            }
        }

        /// <summary>
        /// Average drum speed (entity) at the time that this piece of asset history occurred
        /// </summary>
        public IFrequency AverageDrumSpeedEntity
        {
            get
            {
                return this.GetAverageDrumSpeedEntity();
            }
        }

        /// <summary>
        /// Average drum speed (primitive value) at the time that this piece of asset history occurred
        /// </summary>
        public double? AverageDrumSpeed
        {
            get
            {
                return null == this.AverageDrumSpeedEntity ? (double?)null :
                    Business.Entities.Properties.Settings.Default.Data.UnitTypeUnitSystem[typeof(IFrequency)] == UnitSystem.English ?
                            this.AverageDrumSpeedEntity.RevolutionsPerMinute : this.AverageDrumSpeedEntity.RevolutionsPerMinute;
            }
        }

        /// <summary>
        /// Returns whether or not this event was automatically generated
        /// </summary>
        public bool AutoGenerated
        {
            get
            {
                if (this.IsEvent)
                {
                    return (this.Entity as IExportEvent).AutoGenerated;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Returns the address street name (line 01)
        /// </summary>
        private string AddressStreetNameLine1
        {
            get
            {
                if (null == this._addressStreetNameLine01)
                {
                    lock (this._asyncLoadReverseGeocodedDataLock)
                    {
                        if (null != this._reverseGeocodedData && null != this._reverseGeocodedData.Address &&
                            null != this._reverseGeocodedData.Address.Names && this._reverseGeocodedData.Address.Names.Length > 0)
                        {
                            this._addressStreetNameLine01 = this._reverseGeocodedData.Address.Names[0];
                        }
                        else
                            this._addressStreetNameLine01 = String.Empty;
                    }
                }

                return this._addressStreetNameLine01;
            }
        }

        /// <summary>
        /// Returns the address street name (line 02)
        /// </summary>
        private string AddressStreetNameLine2
        {
            get
            {
                if (null == this._addressStreetNameLine02)
                {
                    lock (this._asyncLoadReverseGeocodedDataLock)
                    {
                        if (null != this._reverseGeocodedData && null != this._reverseGeocodedData.Address &&
                            null != this._reverseGeocodedData.Address.Names && this._reverseGeocodedData.Address.Names.Length > 1)
                        {
                            // initialize address line 02 to an empty string
                            StringBuilder sbAddressStreetNameLine02 = new StringBuilder(String.Empty);

                            // loop through the rest of the strings and concatinate thems as a singular street name
                            // note: left this as-is (other than correcting the missing "!" before String.IsNullOrWhiteSpace call), but...
                            // ...what is the point of this? it seems it will create a giant blob of info for address line 2?
                            for (int index = 1; index < this._reverseGeocodedData.Address.Names.Length; index++)
                            {
                                if (!String.IsNullOrWhiteSpace(this._reverseGeocodedData.Address.Names[index].Trim()))
                                    sbAddressStreetNameLine02.Append(this._reverseGeocodedData.Address.Names[index] + " ");
                            }

                            // trim off trailing whitespace
                            this._addressStreetNameLine02 = sbAddressStreetNameLine02.ToString().TrimEnd();
                        }
                        else
                            this._addressStreetNameLine02 = String.Empty;
                    }
                }

                return this._addressStreetNameLine02;
            }
        }
        #endregion


        #region service accessors
        /// <summary>
        /// Geocode Service
        /// </summary>
        private static MapServices.IGeocode GeoCodeService
        {
            get
            {
                if (null == s_geoCodeService)
                    s_geoCodeService = MapServices.Factory.Geocode();

                return s_geoCodeService;
            }
        }

        /// <summary>
        /// TimeZone Service
        /// </summary>
        private static ITimeZoneService TimeZoneService
        {
            get
            {
                if (null == s_timeZoneService)
                    s_timeZoneService = new TimeZoneService(UserSettingsService);

                return s_timeZoneService;
            }
        }

        public static IMapService MapService
        {
            get
            {
                if (null == s_mapService)
                    s_mapService = new ConnectedFleet.BusinessBase.Services.MapService(s_connectionService, UserSettingsService, CapabilityService);

                return s_mapService;
            }
            set
            {
                s_mapService = value;
            }
        }

        /// <summary>
        /// TimeZoneOffsetHelper Service
        /// </summary>
        private static MapServices.ITimeZoneOffsetHelper TimeZoneOffsetHelperService
        {
            get
            {
                if (null == s_timeZoneOffsetHelperService)
                    s_timeZoneOffsetHelperService = MapServices.Factory.TimeZoneOffset();

                return s_timeZoneOffsetHelperService;
            }
        }

        /// <summary>
        /// UserSettings Service
        /// </summary>
        private static UserSettingsService UserSettingsService
        {
            get
            {
                if (null == s_userSettingsService)
                    s_userSettingsService = new UserSettingsService();

                return s_userSettingsService;
            }
        }

        private static ICapabilityService CapabilityService
        {
            get
            {
                if (null == s_capabilityService)
                    s_capabilityService = new CapabilityService();

                return s_capabilityService;
            }
        }
        #endregion


        #region constructors
        /// <summary>
        /// Static constructor for initializing our dictionary of latlons->geocoded data
        /// </summary>
        static AssetHistory()
        {
            if (null == s_dictionaryOfLatLonsToGeocodedData)
                s_dictionaryOfLatLonsToGeocodedData = new Dictionary<string, dynamic>();
        }

        /// <summary>
        /// Constructor taking an Asset object
        /// </summary>
        /// <param name="asset"></param>
        protected AssetHistory(IAsset asset)
        {
            // check arguments
            if (null == asset)
                throw new ArgumentNullException("asset");

            // initialize member vars
            this.Asset = asset;
            this.DateTimeConverter = Module.SessionService.ConvertUtcToSession;
        }

        /// <summary>
        /// Constructor taking a HistoricalLocation object and an Asset object
        /// </summary>
        /// <param name="historicalLocation"></param>
        /// <param name="asset"></param>
        protected AssetHistory(IHistoricalLocation historicalLocation, IAsset asset)
            : this(asset)
        {
            // check arguments
            if (null == historicalLocation)
                throw new ArgumentNullException("historicalLocation");

            if (null == asset)
                throw new ArgumentNullException("asset");

            // initialize member vars
            this.Entity = historicalLocation;

            // some pieces of data depend on long-running operations; attempt to...
            // ...cut some of that time down by running background threads to load objects that this data depends upon
            InitializeBackgroundWorkers();
        }
        #endregion


        #region private / internal methods
        /// <summary>
        /// Retrieves the address for the specified entity
        /// </summary>
        /// <param name="locationEntity"></param>
        /// <returns></returns>
        internal static string GetAddress(IHistoricalLocation locationEntity)
        {
            // return value
            string returnValue = String.Empty;

            // check argument for null
            if (null == locationEntity)
                throw new ArgumentNullException("locationEntity");

            if (null == locationEntity.MapPoint)
                throw new ArgumentNullException("IHistoricalLocation.MapPoint");


            if ((!Double.IsNaN(locationEntity.MapPoint.Lat)) &&
                (!Double.IsNaN(locationEntity.MapPoint.Lon)) &&
                (null == locationEntity.Address))
            {
                try
                {
                    // reverse geocode the lat/lon to obtain the address of the event
                    returnValue = GeoCodeService.Reverse(locationEntity.MapPoint.Lat, locationEntity.MapPoint.Lon).ToFullAddress();
                }
                catch (MapperExceptionBase ex)
                {
                    // log exception
                    IDictionary<string, object> properties = (new KeyValuePair<string, object>[]{
                                    new KeyValuePair<string, object>("Service", "Geocode.Reverse"),
                                    new KeyValuePair<string, object>("Entity", "LocationEventHistory.Address"),
                                    new KeyValuePair<string, object>("Latitude", locationEntity.MapPoint.Lat),
                                    new KeyValuePair<string, object>("Longitude", locationEntity.MapPoint.Lon)}).ToList().ToDictionary(k => k.Key, v => v.Value);

                    Utility.Logging.LogHelper.WriteEntry(ex, System.Diagnostics.TraceEventType.Warning, properties);
                }
            }
            else
            {
                if (null != locationEntity.Address)
                    returnValue = locationEntity.Address.ToFullAddress();
            }

            // return to caller
            return returnValue;
        }

        /// <summary>
        /// Retrieves a stringized version of the vehicle heading value at the time that this
        /// piece of asset history was recorded
        /// </summary>
        /// <returns></returns>
        private string GetDirectionString()
        {
            if (!this.Heading.HasValue)
                return String.Empty;

            Orientation direction = DirectionConverter.ConvertDegrees(this.Heading.Value);
            switch (direction)
            {
                case Orientation.North:
                    return Properties.Resources.North;
                case Orientation.Northeast:
                    return Properties.Resources.NorthEast;
                case Orientation.East:
                    return Properties.Resources.East;
                case Orientation.Southeast:
                    return Properties.Resources.SouthEast;
                case Orientation.South:
                    return Properties.Resources.South;
                case Orientation.Southwest:
                    return Properties.Resources.SouthWest;
                case Orientation.West:
                    return Properties.Resources.West;
                case Orientation.Northwest:
                    return Properties.Resources.NorthWest;
                default:
                    return Properties.Resources.North;
            }
        }

        /// <summary>
        /// Readies our background worker object
        /// </summary>
        private void InitializeBackgroundWorkers()
        {
            // actual time background worker thread
            if (null == this._bgwActualTimeWorker)
            {
                // instantiate obj, initialize
                this._bgwActualTimeWorker = new BackgroundWorker();
                this._bgwActualTimeWorker.WorkerReportsProgress = false;
                this._bgwActualTimeWorker.WorkerSupportsCancellation = false;

                // set dowork handler
                this._bgwActualTimeWorker.DoWork += delegate
                {
                    ComputeFormattedDate(Properties.Resources.NotApplicable);
                };
            }

            // time zone background worker thread
            if (null == this._bgwTimeZoneWorker)
            {
                // instantiate obj, initialize
                this._bgwTimeZoneWorker = new BackgroundWorker();
                this._bgwTimeZoneWorker.WorkerReportsProgress = false;
                this._bgwTimeZoneWorker.WorkerSupportsCancellation = false;

                // set dowork handler
                this._bgwTimeZoneWorker.DoWork += delegate
                {
                    // store current value as previous for later comparison
                    string previousTimeZoneName = this._timeZoneName ?? String.Empty;

                    // retrieve actual time zone (note: potentially long-running op due to reverse geocoding req.)
                    this._timeZoneName = GetTimezoneName();

                    // if our time zone value has changed then notify any listeners (e.g. our presentation grid)
                    if (null != this._timeZoneName && !this._timeZoneName.Equals(previousTimeZoneName))
                        this.OnPropertyChanged("TimeZoneName");
                };
            }
        }

        private void ComputeFormattedDate(string dateDisplayValue)
        {
            // store current value as previous for later comparison
            string previousFormattedDateTime = this._formattedDateTime ?? String.Empty;

            // retrieve actual time (note: potentially long-running op due to reverse geocoding req.)
            DateTime dt = DateTime.MinValue;
            this._formattedDateTime = (DateTime.MinValue != (dt = this.GetDate()) ? dt.ToString() : dateDisplayValue);

            // if our time zone value has changed then notify any listeners (e.g. our presentation grid)
            if (null != this._formattedDateTime && !this._formattedDateTime.Equals(previousFormattedDateTime))
                this.OnPropertyChanged("FormattedDateTime");
        }

        /// <summary>
        /// Retrieves the reverse geocoded data from the telogis sdk
        /// 
        /// Note: We may want to consider using the business entity caching mechanism here in the future
        /// rather than this temporary in-memory caching approach
        /// 
        /// Note: we don't necessarily need to be thread-safe here with our usage of the static dictionary - the worst case scenario is 
        /// we unnecessarily add a new entry into our dictionary for a latlon we've already recorded - adding a lock actually seems to slow 
        /// things down more than the extra reverse geocoding ops that sometimes occur due to the race condition
        /// </summary>
        protected void RetrieveReverseGeocodedData()
        {
            if (null == this._reverseGeocodedData && null != this.Entity && null != this.Entity.MapPoint && !Double.IsNaN(this.Entity.MapPoint.Lat) && !Double.IsNaN(this.Entity.MapPoint.Lon))
            {
                // create stringized version of the decimal lat + decimal lon using the entity map point
                // note: this will be used as a key into a dictionary that maps these keys to geocoded data
                string stringizedLatLon = String.Format("{0},{1}", (decimal)this.Entity.MapPoint.Lat, (decimal)this.Entity.MapPoint.Lon);

                // check if latlon key exists in our dictionary
                // lock (s_dictionaryOfLatLonsToGeocodedDataLock) (left commented out lock in so that the note above is read)
                {
                    if (!s_dictionaryOfLatLonsToGeocodedData.TryGetValue(stringizedLatLon, out this._reverseGeocodedData))
                    {
                        // if we were unable to retrieve cached reverse geocoded data from our dictionary then
                        // move forward with getting the data from the telogis sdk
                        this._reverseGeocodedData = MapService.ReverseToGeocodeFull(this.Entity.MapPoint.Lat, this.Entity.MapPoint.Lon);

                        if (null != this._reverseGeocodedData)
                        {
                            // update our dictionary first with the exact latlon 
                            // note: we first add the exact latlon in the event that it is not within the streetlink returned
                            s_dictionaryOfLatLonsToGeocodedData[stringizedLatLon] = this._reverseGeocodedData;

                            if (!IsALKMap) //Since we don't have ALK SDK, I don't think this is possible.
                            {
                                // update our dictionary with all latlons for the streetlink inside of this geocoded data obj
                                MapService.UpdateStreetLink(this._reverseGeocodedData, s_dictionaryOfLatLonsToGeocodedData);
                            }
                        }
                    }
                }
            }
        }
        #endregion


        #region protected methods
        /// <summary>
        /// Notifies listeners that the specified property has changed
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (null != handler)
                handler(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Retrieves a new unique identifier for this object
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDefaultId()
        {
            if (this._guid == Guid.Empty)
                this._guid = Guid.NewGuid();

            return _guid.ToString();
        }

        /// <summary>
        /// Retrieves the default type id for this object (simply returns 0 for now (do not know the history))
        /// </summary>
        /// <returns></returns>
        protected virtual long GetDefaultTypeId()
        {
            return 0;
        }

        /// <summary>
        /// Returns whether or not actual time (given the time zone where the event or position was captured) is enabled
        /// Note: Application performance is impacted when actual time is used
        /// </summary>
        protected bool IsActualTimeEnabled
        {
            get
            {
                return UserSettingsService.ActualTimezoneEnabled;
            }
        }

        private MapServices.ITimeZoneOffsetHelper TimeZoneOffsetAlkService
        {
            get
            {
                return MapServices.Factory.TimeZoneOffset();
            }
        }

        private KeyValuePair<DateTime, string> GetTimeOffsetAndAbbreviation(DateTime utcTime, double lat, double lon)
        {
            TimeSpan timeZone = TimeZoneOffsetAlkService.GetTimeOffset(utcTime, lat, lon);
            KeyValuePair<DateTime, string> actualTimeWithAbbreviation = MapService.GetAssetTimeAndAbbreviation(utcTime, timeZone);
            return actualTimeWithAbbreviation;
        }


        /// <summary>
        /// Retrieves the Timestamp for this entity; may be formatted, may also be in the actual time zone if the settings specify to do so
        /// </summary>
        /// <returns></returns>
        protected virtual DateTime GetDate()
        {
            // return value
            Nullable<DateTime> ret = null;

            if (null != this.Entity)
            {
                if (this.IsActualTimeEnabled)
                {
                    //Yet to verify with Telogis
                    lock (this._asyncLoadReverseGeocodedDataLock)
                    {
                        try
                        {
                            if (IsALKMap)
                            {
                                KeyValuePair<DateTime, string> reverseGeocode = GetActualTimeWithAbbreviation(this.Entity.Timestamp);
                                ret = reverseGeocode.Key;
                            }
                            else
                            {
                                if (null == this._reverseGeocodedData)
                                    this.RetrieveReverseGeocodedData();

                                if (null != this._reverseGeocodedData)
                                {
                                    ret = this._reverseGeocodedData.TimeZone.ConvertTime(this.Entity.Timestamp);
                                }
                            }
                        }
                        catch { ret = DateTime.MinValue; }
                    }
                }
                else
                {
                    // otherwise simply apply the date/time converter/formatter
                    ret = DateTimeConverter(Entity.Timestamp);
                }
            }

            // return to caller
            return ret.HasValue ? ret.Value : DateTime.MinValue;
        }

        private KeyValuePair<DateTime, string> GetActualTimeWithAbbreviation(DateTime entityTimeStamp)
        {
            KeyValuePair<DateTime, string> actualTimeWithAbbreviation = new KeyValuePair<DateTime, string>(entityTimeStamp, "NA");
            if (LatLonReverseGeocodeResult != null)
            {
                List<KeyValuePair<DateTime, string>> timeZoneList = LatLonReverseGeocodeResult.Where(a => a.Key.ToShortDateString().Equals(entityTimeStamp.ToShortDateString())).ToList();

                if (timeZoneList.Any() && timeZoneList.Count() == 2) //For a given date we would be having 2 entries, if it is not then we have a problem in ALKGeocode Service.
                {
                    string timeZone = null;

                    if (entityTimeStamp.TimeOfDay <= timeZoneList[0].Key.TimeOfDay)
                    {
                        timeZone = timeZoneList[0].Value;
                    }
                    else
                    {
                        timeZone = timeZoneList[1].Value;
                    }

                    if (!string.IsNullOrEmpty(timeZone))
                    {
                        actualTimeWithAbbreviation = MapService.GetAssetTimeAndAbbreviation(this.Entity.Timestamp, timeZone);
                    }
                }
            }

            return actualTimeWithAbbreviation;
        }

        /// <summary>
        /// Retrieves the timezone in which this event or position or other piece of asset history occurred
        /// </summary>
        protected virtual string GetTimezoneName()
        {
            // cache current time zone name
            string timeZoneName = String.Empty;

            if (this.IsActualTimeEnabled && null != this.Entity)
            {
                // time zone offset
                TimeSpan timeZoneOffset = TimeSpan.Zero;

                // attempt to load timezone data from reverse geocoded latlon
                lock (this._asyncLoadReverseGeocodedDataLock)
                {
                    try
                    {
                        if (IsALKMap)
                        {
                            KeyValuePair<DateTime, string> reverseGeocode = GetActualTimeWithAbbreviation(this.Entity.Timestamp);
                            timeZoneName = reverseGeocode.Value;
                        }
                        else
                        {
                            if (null == this._reverseGeocodedData)
                                this.RetrieveReverseGeocodedData();

                            if (null != this._reverseGeocodedData)
                            {
                                timeZoneOffset = this._reverseGeocodedData.TimeZone.ConvertTime(this.Entity.Timestamp) - this.Entity.Timestamp;
                            }
                        }
                    }
                    catch
                    {
                        timeZoneOffset = TimeSpan.Zero;
                        timeZoneName = Properties.Resources.NotApplicable;
                    }
                }

                if (!IsALKMap)
                {
                    // map the offset to an abbreviation
                    timeZoneName = timeZoneOffset != TimeSpan.Zero ? TimeZoneService.TimeZoneOffsetAbbreviation(timeZoneOffset) : Properties.Resources.NotApplicable;
                }
            }
            else
            {
                // otherwise just return the abbreviation for the local time zone
                timeZoneName = TimeZoneService.TimeZoneOffsetAbbreviation(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now));
            }


            // return to caller
            return timeZoneName;
        }

        /// <summary>
        /// Retrieves the address at which this piece of asset history occurred
        /// </summary>
        /// <returns></returns>
        protected virtual string GetAddress()
        {
            // return value
            string returnValue = String.Empty;

            if (null != this.Entity)
            {
                if (null == this.Entity.Address)
                {
                    if (null != this.Entity.MapPoint && (!Double.IsNaN(this.Entity.MapPoint.Lat)) && (!Double.IsNaN(this.Entity.MapPoint.Lon)))
                    {
                        try
                        {
                            //Yet to verify with Telogis
                            lock (this._asyncLoadReverseGeocodedDataLock)
                            {
                                if (null == this._reverseGeocodedData)
                                    this.RetrieveReverseGeocodedData();

                                // if we were able to preload reverse geocoded data then use that
                                if (null != this._reverseGeocodedData && null != this._reverseGeocodedData.Address)
                                {
                                    // create address business entity
                                    Trimble.MobileSolutions.Business.Entities.Map.IAddress address =
                                        Trimble.MobileSolutions.Business.Entities.Map.Factory.Address(
                                                                this._reverseGeocodedData.Address.Number.ToString(),
                                                                this.AddressStreetNameLine1,
                                                                String.Empty/*this.AddressStreetNameLine2*/,
                                                                this._reverseGeocodedData.Address.City,
                                                                MapService.ShortenState(this._reverseGeocodedData.Address.Region, this._reverseGeocodedData.Address.Country),
                                                                this._reverseGeocodedData.Address.PostalCode,
                                                                MapService.ConvertCountryToMapCountry(this._reverseGeocodedData.Address.Country),
                                                                this.Entity.MapPoint);

                                    // set return value
                                    returnValue = address.ToFullAddress();
                                }
                            }

                            // reverse geocode the lat/lon to obtain the address of the event
                            if (String.IsNullOrWhiteSpace(returnValue))
                                returnValue = GeoCodeService.Reverse(this.Entity.MapPoint.Lat, this.Entity.MapPoint.Lon).ToFullAddress();
                        }
                        catch (MapperExceptionBase ex)
                        {
                            // create collection of properties to add to log statement
                            IDictionary<string, object> properties = (new KeyValuePair<string, object>[]{
                                    new KeyValuePair<string, object>("Service", "Geocode.Reverse"),
                                    new KeyValuePair<string, object>("Entity", "LocationEventHistory.Address"),
                                    new KeyValuePair<string, object>("Latitude", this.Entity.MapPoint.Lat),
                                    new KeyValuePair<string, object>("Longitude", this.Entity.MapPoint.Lon)}).ToList().ToDictionary(k => k.Key, v => v.Value);

                            // log exception
                            Utility.Logging.LogHelper.WriteEntry(ex, System.Diagnostics.TraceEventType.Warning, properties);
                        }
                    }
                }
                else
                {
                    returnValue = this.Entity.Address.ToFullAddress();
                }
            }

            // return to caller
            return returnValue;
        }

        /// <summary>
        /// Determined by subclass impl. - otherwise String.Empty
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDefaultName()
        {
            return String.Empty;
        }

        /// <summary>
        /// Retrieves a description for this piece of asset history
        /// </summary>
        protected virtual string GetDefaultDescription()
        {
            return string.Empty;
        }

        /// <summary>
        /// Retrieves the default site name (empty string)
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDefaultSiteName()
        {
            return String.Empty;
        }

        /// <summary>
        /// Retrieves the mileage entity from this instances internal entity obj
        /// </summary>
        /// <returns></returns>
        protected virtual IDistance GetMileageEntity()
        {
            if (null != this.Entity)
                return this.Entity.Mileage;
            else
                return null;
        }

        /// <summary>
        /// Retrieves the speed entity from this instances internal entity obj
        /// </summary>
        /// <returns></returns>
        protected virtual ISpeed GetSpeedEntity()
        {
            if (null != this.Entity)
                return this.Entity.Speed;
            else
                return null;
        }

        /// <summary>
        /// Retrieves the heading at the time that this piece of asset history was recorded
        /// </summary>
        /// <returns></returns>
        protected virtual double? GetHeading()
        {
            if (null != this.Entity)
                return this.Entity.Heading;
            else
                return null;
        }

        /// <summary>
        /// Returns the latitude within the entity object for this piece of asset history
        /// </summary>
        /// <returns></returns>
        protected virtual double? GetLatitude()
        {
           // return null == this.Entity ? (double?)null : null == this.Entity.MapPoint ? (double?)null : this.Entity.MapPoint.Lat;
        }

        /// <summary>
        /// Returns the longitude within the entity object for this piece of asset history
        /// </summary>
        /// <returns></returns>
        protected virtual double? GetLongitude()
        {
           // return null == this.Entity ? (double?)null : null == this.Entity.MapPoint ? (double?)null : this.Entity.MapPoint.Lon;
        }

        /// <summary>
        /// Retrieves the inner hydraulic pressure entity
        /// </summary>
        /// <returns></returns>
        protected virtual IPressure GetHydraulicPressureEntity()
        {
            return null;
        }

        /// <summary>
        /// Retrieves the inner average drump speed entity
        /// </summary>
        /// <returns></returns>
        protected virtual IFrequency GetAverageDrumSpeedEntity()
        {
            return null;
        }
        #endregion


        #region public methods
        /// <summary>
        /// Returns whether or not the inner Entity object is an instance of IExportEvent
        /// </summary>
        public bool IsEvent
        {
            get { return this.Entity is IExportEvent; }
        }
        #endregion

        public void Dispose()
        {

            //if (this._bgwActualTimeWorker != null)
            //{
            //    this._bgwActualTimeWorker.Dispose();
            //}
            //if (this._bgwTimeZoneWorker != null)
            //{
            //    this._bgwTimeZoneWorker.Dispose();
            //}
            //this.PropertyChanged -= PropertyChanged;
        }
    }
}