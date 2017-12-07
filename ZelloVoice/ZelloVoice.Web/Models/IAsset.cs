using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

namespace ZelloVoice.Web.Models
{
    public class IAsset
    {
      //  IAssetType AssetType { get; set; }
        int? AttributesProfileId { get; set; }
        long DeviceId { get; set; }
      //  IDeviceCollection Devices { get; }
        long DimVersion { get; set; }
      //  IDisplayType DisplayType { get; }
        float? Height { get; set; }
        long IconSize { get; set; }
     //   DeviceLogicType LogicType { get; set; }
        long MapColor { get; set; }
        long MapIcon { get; set; }
        string MessagingVersion { get; set; }
        double? Mileage { get; set; }
        string MobileId { get; set; }
        string Name { get; set; }
        IOrganization Organization { get; set; }
        long? Runtime { get; set; }
      //  IAssetConfiguration SpeedingConfiguration { get; set; }
      //  IAssetConfiguration StoppedConfiguration { get; set; }
      //  [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "VIN")]
        string VIN { get; set; }
      //  ProfileVoiceStatus VoiceStatus { get; set; }
        float? Weight { get; set; }

      //  void Add(IDevice device);
    }

    public interface IOrganization
    {
        IAssetTypeCollection AssetTypeCollection { get; }
        ITalkGroup DefaultTalkGroup { get; set; }
        long Id { get; }
        string Name { get; }
        OrganizationMetadata OrganizationMetadata { get; }
        IOrganizationCollection SubOrganizationCollection { get; }
    }

    public interface IAssetTypeCollection
    {
    }

    public interface ITalkGroup
    {
        DynamicTalkGroupState DynamicTalkGroupState { get; set; }
        long Id { get; set; }
        ListenState InitialListenState { get; set; }
        bool IsDynamicTalkGroup { get; }
        string Name { get; set; }
        //[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
       // [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        List<long> Organizations { get; }
        IOrganization ParentSubOrganization { get; set; }
        ReadOnlyCollection<IParticipant> Participants { get; }
        IList<IProfile> Profiles { get; }
        long? SiteId { get; set; }
        TalkGroupType TalkGroupType { get; set; }
    }

    public enum TalkGroupType
    {
        Unknown = 0,
        Static = 1,
        HomeSiteDTG = 2,
        JobsiteDTG = 3,
        OrderDTG = 4,
        Emergency = 5,
        ContactList = 6
    }

    public enum DynamicTalkGroupState
    {
        Unknown = 0,
        Unsubscribed = 1,
        Subscribing = 2,
        Subscribed = 3
    }
    public enum ListenState
    {
        Unknown = 0,
        Closed = 1,
        Open = 2
    }

    public interface IParticipant
    {
        IPresence Presence { get; set; }
        IProfile Profile { get; set; }
        bool Talking { get; }
    }

    public interface IPresence
    {
        bool Muted { get; set; }
        bool RX { get; set; }
        bool TX { get; set; }
    }

    public interface IProfile
    {
        ITalkGroup DefaultTalkGroup { get; set; }
        long Id { get; set; }
        TimeSpan InactiveDefault { get; set; }
        //[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        Dictionary<ITalkGroup, MuteSetting> MuteSettings { get; set; }
        string Name { get; set; }
        ProfileType ProfileType { get; }
        string ReferenceId { get; }
        long? SystemReferenceId { get; }
        IEnumerable<ITalkGroup> TalkGroups { get; set; }
        ProfileVoiceStatus VoiceStatus { get; set; }

        ITalkGroup GetTalkGroup(long talkGroupId);
        void Save();
    }

    public enum MuteSetting
    {
        Unknown = 0,
        On = 1,
        Off = 2
    }

    public enum ProfileType
    {
        Unknown = 0,
        Android = 1,
        Device = 2,
        Other = 3,
        TVUser = 4
    }

    public enum ProfileVoiceStatus
    {
        Unknown = 0,
        Joined = 1,
        Talking = 2,
        NotJoined = 3,
        Away = 4,
        Connecting = 5,
        Busy = 6,
        Standby = 7,
        Headphones = 8
    }

    [Flags]
   // [SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames")]
    public enum OrganizationMetadata
    {
        None = 0,
        Shared = 1,
        Local = 2,
        Foreign = 4,
        Parent = 8
    }

    public interface IOrganizationCollection : ICloneable,  IList<IOrganization>, ICollection<IOrganization>, IEnumerable<IOrganization>
    {
    }
}