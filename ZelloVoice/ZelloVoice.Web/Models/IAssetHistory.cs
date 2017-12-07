using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZelloVoice.Web.Models
{

    public interface IAssetHistory<T>
    {
        IAsset Asset { get; }
        Exception Exception { get; }
        DateTime FromDate { get; }
        IEnumerable<T> History { get; }
        DateTime ToDate { get; }
    }
    public interface IAssetHistory
    {
        string Id { get; }
        long TypeId { get; }
        IAsset Asset { get; }
        bool IsEvent { get; }
        string FormattedDateTime { get; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Date")]
        DateTime Date { get; }
        string TimeZoneName { get; }
        string Name { get; }
        string Description { get; }
        string SiteName { get; }
        string Address { get; }
        IDistance MileageEntity { get; }
        ISpeed SpeedEntity { get; }
        double? Heading { get; }
        string Direction { get; }
        double? Latitude { get; }
        double? Longitude { get; }
        double? Mileage { get; }
        double? Speed { get; }
        IPressure HydraulicPressureEntity { get; }
        IFrequency AverageDrumSpeedEntity { get; }
        double? HydraulicPressure { get; }
        double? AverageDrumSpeed { get; }
    }

    public interface ILocationHistory : IAssetHistory
    {
        IHistoricalLocation Location { get; }
    }

    public interface IHistoricalLocation
    {
        IAddress Address { get; }
    }

    public interface IEventHistory : IAssetHistory
    {
        IExportEvent Event { get; }

    }

    public delegate string CalculateDIMText(IExportEvent exportEvent, IAsset asset);

    public interface ITextMessageHistory : IEventHistory, ITextMessage
    {
        ITextMessage TextMessage { get; set; }

        CalculateDIMText CalculateDIMTextMethod { get; set; }
    }

    public interface ITextMessage
    {
        long AssetId { get; }
        DateTime? DeliveredDateTime { get; }
        //[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DIM")]
       // [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
        long DIMListID { get; }
        DateTime EventTimestamp { get; }
        string Id { get; }
        string MessageText { get; }
        PredefinedPriority Priority { get; }
        DateTime? ReadDateTime { get; }
        string Receiver { get; }
        DateTime? RespondedDateTime { get; }
        string Response { get; }
        IEnumerable<string> Responses { get; }
        string Sender { get; }
        long? SequenceId { get; }
    }

    public enum PredefinedPriority
    {
        Custom = 0,
        Informational = 1,
        Warning = 2,
        Alert = 3,
        WorkOrderManagement = 4,
        None = 5
    }

    public interface IAddress
    {
        string AddressLine1 { get; }
        string AddressLine2 { get; }
        string City { get; }
        Country Country { get; }
        string CountryValue { get; }
        string County { get; }
        IMapPoint Location { get; }
        string Number { get; }
        string PostalCode { get; }
        string Region { get; }
        string Street { get; }

        string ToFullAddress();
        string ToFullAddress(bool multiline);
        string ToFullAddress(bool multiline, bool showCountry);
        string ToShortAddress();
        string ToString();
    }

    public interface IMapPoint
    {
        double Lat { get; set; }
        double Lon { get; set; }
        MapPointType MapPointType { get; }

        bool IsEmpty();
        bool IsInvalid();
    }

    public enum MapPointType
    {
        Valid = 0,
        Empty = 1,
        Invalid = 2
    }

    public enum Country
    {
        None = 0,
        Afghanistan = 1,
        Albania = 2,
        Algeria = 3,
        AmericanSamoa = 4,
        Andorra = 5,
        Angola = 6,
        Anguilla = 7,
        Armenia = 8,
        AntiguaAndBarbuda = 9,
        Argentina = 10,
        Aruba = 11,
        Australia = 12,
        Austria = 13,
        Bahamas = 14,
        Bahrain = 15,
        Bangladesh = 16,
        Barbados = 17,
        Belize = 19,
        Benin = 20,
        Bermuda = 21,
        Bhutan = 22,
        Bolivia = 23,
        Botswana = 24,
        Brazil = 26,
        BritishVirginIslands = 27,
        Brunei = 28,
        BruneiDarussalam = 28,
        Bulgaria = 29,
        BurkinaFaso = 30,
        Myanmar = 31,
        Burundi = 32,
        Belarus = 33,
        Cameroon = 34,
        Canada = 35,
        CapeVerde = 36,
        CaymanIslands = 37,
        CentralAfricanRepublic = 38,
        Chad = 39,
        Chile = 40,
        China = 41,
        Croatia = 42,
        Kyrgyzstan = 43,
        Colombia = 44,
        Comoros = 45,
        RepublicOfCongo = 46,
        CookIslands = 47,
        CostaRica = 48,
        SmallerTerritoriesOfTheUK = 49,
        Cuba = 50,
        Cyprus = 51,
        CzechRepublic = 52,
        Denmark = 53,
        Djibouti = 54,
        Dominica = 55,
        DominicanRepublic = 56,
        EastTimor = 57,
        Ecuador = 58,
        Egypt = 59,
        ElSalvador = 60,
        EquatorialGuinea = 61,
        Ethiopia = 62,
        FaroeIslands = 63,
        FalklandIslands = 64,
        FalklandIslandsMalvinas = 64,
        Fiji = 65,
        Finland = 66,
        FrenchGuiana = 68,
        FrenchPolynesia = 69,
        Georgia = 70,
        Gabon = 71,
        Gambia = 72,
        Ghana = 75,
        Gibraltar = 76,
        Greece = 77,
        Greenland = 78,
        Grenada = 79,
        Guadeloupe = 80,
        Guam = 81,
        Guatemala = 82,
        Guinea = 83,
        GuineaBissau = 84,
        Guyana = 85,
        Haiti = 86,
        Honduras = 88,
        SmallerTerritoriesOfNorway = 89,
        Hungary = 90,
        Iceland = 91,
        India = 92,
        Indonesia = 93,
        Iran = 94,
        Iraq = 95,
        Ireland = 96,
        Israel = 97,
        Jamaica = 99,
        Japan = 100,
        Jordan = 101,
        Kazakhstan = 102,
        Kenya = 103,
        Kiribati = 104,
        NorthKorea = 105,
        SouthKorea = 106,
        Kuwait = 107,
        Laos = 108,
        Lebanon = 109,
        Lesotho = 110,
        Liberia = 111,
        Libya = 112,
        Liechtenstein = 113,
        Luxembourg = 114,
        SmallerTerritoriesOfChile = 115,
        Madagascar = 116,
        Malawi = 117,
        Malaysia = 118,
        Maldives = 119,
        Mali = 120,
        Malta = 121,
        MarshallIslands = 122,
        Martinique = 123,
        Mauritania = 124,
        Mauritius = 125,
        Mexico = 126,
        Micronesia = 127,
        Monaco = 128,
        Mongolia = 129,
        Montserrat = 130,
        Morocco = 131,
        Mozambique = 132,
        Namibia = 133,
        Nauru = 134,
        Nepal = 135,
        NetherlandsAntilles = 137,
        Slovenia = 138,
        NewCaledonia = 139,
        NewZealand = 140,
        Nicaragua = 141,
        Niger = 142,
        Nigeria = 143,
        Niue = 144,
        NorfolkIsland = 145,
        NorthernMarianaIslands = 146,
        Norway = 147,
        Oman = 148,
        Pakistan = 149,
        Palau = 150,
        Panama = 151,
        PapuaNewGuinea = 152,
        Paraguay = 153,
        Peru = 154,
        Philippines = 155,
        Poland = 157,
        Portugal = 158,
        PuertoRico = 159,
        Qatar = 160,
        Reunion = 161,
        Romania = 162,
        Rwanda = 163,
        SaintHelena = 164,
        SaintKittsAndNevis = 165,
        SaintLucia = 166,
        SaintPierreAndMiquelon = 167,
        SaintVincentAndTheGrenadines = 168,
        Samoa = 169,
        SanMarino = 170,
        SaoTomeAndPrincipe = 171,
        SaudiArabia = 172,
        Senegal = 173,
        Seychelles = 174,
        SierraLeone = 175,
        Singapore = 176,
        SolomonIslands = 177,
        Somalia = 178,
        SouthAfrica = 179,
        SriLanka = 181,
        Sudan = 182,
        Suriname = 183,
        SvalbardAndJanMayen = 184,
        Swaziland = 185,
        Sweden = 186,
        Syria = 188,
        Taiwan = 189,
        Macedonia = 190,
        Thailand = 191,
        Togo = 192,
        Turkmenistan = 193,
        Tonga = 194,
        TrinidadTobago = 195,
        Tunisia = 196,
        Turkey = 197,
        TurksAndCaicosIslands = 198,
        Tuvalu = 199,
        Uganda = 200,
        Ukraine = 201,
        UnitedArabEmirates = 202,
        Uzbekistan = 205,
        Uruguay = 206,
        Vanuatu = 208,
        VaticanCity = 209,
        Venezuela = 210,
        Vietnam = 211,
        USVirginIslands = 213,
        WallisAndFutuna = 214,
        WesternSahara = 215,
        Yemen = 216,
        Tajikistan = 217,
        DemocraticRepublicOfCongo = 219,
        Zambia = 220,
        Zimbabwe = 221,
        BosniaAndHerzegovina = 222,
        Eritrea = 223,
        Estonia = 224,
        Latvia = 225,
        Lithuania = 226,
        Mayotte = 227,
        Palestine = 228,
        Moldova = 229,
        Russia = 230,
        Serbia = 231,
        Slovakia = 232,
        Antarctica = 233,
        Azerbaijan = 234,
        Guernsey = 238,
        Jersey = 241,
        Cambodia = 242,
        FrenchSouthernTerritories = 243,
        Tokelau = 244,
        Tanzania = 245,
        USMinorOutlyingIslands = 246,
        Montenegro = 249,
        ExternalTerritoriesOfAustralia = 250,
        IvoryCoast = 251,
        Unknown = 999,
        Germany = 1031,
        USA = 1033,
        US = 1033,
        UnitedStates = 1033,
        UnitedStatesOfAmerica = 1033,
        Spain = 1034,
        France = 1036,
        Italy = 1040,
        Netherlands = 1043,
        Switzerland = 2055,
        UK = 2057,
        UnitedKingdom = 2057,
        GreatBritain = 2057,
        Belgium = 2067,
        HongKong = 3076
    }
}