using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

namespace ZelloVoice.Web.Models
{
    public class IDistance
    {
        double Centimeters { get; }
        DistanceUnit DefaultUnit { get; }
        double Feet { get; }
        double Inches { get; }
        double Kilometers { get; }
        double Meters { get; }
        double Miles { get; }
        double Millimeters { get; }
        double Yards { get; }

       // string ToString();
    }

    public enum DistanceUnit
    {
        Millimeter = 0,
        Centimeter = 1,
        Meter = 2,
        Kilometer = 3,
        Inch = 4,
        Foot = 5,
        Yard = 6,
        Mile = 7
    }

    public class AssetHistoryCollection : ObservableCollection<IAssetHistory>
    {
        public AssetHistoryCollection() { }
        public AssetHistoryCollection(IEnumerable<IAssetHistory> items)
            : base(items)
        {
        }

        public void AddRange(IEnumerable<IAssetHistory> items)
        {
            foreach (IAssetHistory assetHistory in items.OfType<IAssetHistory>())
            {
                base.Add(assetHistory);
            }
        }
    }
}