﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZelloVoice.Web.Models
{
    public interface IExportEvent: IOemEvent
    {
        IFrequency AverageDrumSpeed { get; }
       // [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
       // [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DIM")]
        long DIMListID { get; }
        IKeyedValue<long, string> EventSite { get; }
        IPressure HydraulicPressure { get; }
        IEventParameterCollection ParameterCollection { get; }
        long Port { get; }
        string PortData { get; }
        IPortData PortDataEntity { get; }
    }

    public interface IOemEvent
    {
        bool AutoGenerated { get; }
        int EventId { get; }
        long OemId { get; }
    }

    public interface IKeyedValue<TKey, TValue> where TKey : IComparable
    {
        TKey Key { get; }
        TValue Value { get; }
    }

    public interface IEventParameterCollection : IList<IEventParameter>, ICollection<IEventParameter>, IEnumerable<IEventParameter>
    {
        int MaxIndex { get; }

       // void Add(IEventDescriptionParameter eventDescriptionParameterCollection, string rawValue);
    }

    public interface IPortData
    {
        string Description { get; }
        PortDataType PortDataType { get; }
    }

    public interface IEventParameter 
    {
        string MetricValue { get; }
        //IEventDescriptionParameter Parameter { get; }
    }

    public enum PortDataType
    {
        Unknown = 0,
        Empty = 1,
        QosMetrics = 1002,
        VehicleAlert = 1011,
        DigitalTicketingMessage = 16359,
        ConfigurationMessage = 16359
    }

    public interface IExportData : IHistoricalLocation
    {
        IAddress Address { get; }
    }

    public interface IExportPosition : IExportData, IHistoricalLocation
    {
    }
}