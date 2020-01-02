namespace Creative.System.Core
{
    public class DistributionRangeItem<TRange, TValue>
    {
        public TRange RangeMinimumInclusive { get; set; }
        public TRange RangeMaximumExclusive { get; set; }
        public int Count { get; set; }
        public double Mean { get; set; }
        public TValue Median { get; set; }
        public TValue Mode { get; set; }
        public TValue MinimumValue { get; set; }
        public TValue MaximumValue { get; set; }
    }
}