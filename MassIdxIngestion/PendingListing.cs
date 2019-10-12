namespace MassIdxIngestion
{
    /// <summary>
    /// Data Object. Contains information about a MassIdx Listing that is pending
    /// </summary>
    public class PendingListing
    {
        /// <summary>
        /// Captures the identifier received from the source of the data
        /// prepended with the identifier of the source.
        /// Ex: ZLW::7655288 - the pair of colons delimit the source from the id
        /// </summary>
        public string Id { get; set; }
        public string PROP_TYPE { get; set; }
        public string LIST_NO { get; set; }
        public string STATUS { get; set; }
        public string LIST_PRICE { get; set; }
        public string STREET_NO { get; set; }
        public string STREET_NAME { get; set; }
        public string UNIT_NO { get; set; }
        public string AREA { get; set; }
        public string ZIP_CODE { get; set; }
        public string SQUARE_FEET { get; set; }
        public string NO_ROOMS { get; set; }
        public string NO_BEDROOMS { get; set; }
        public string NO_FULL_BATHS { get; set; }
        public string NO_HALF_BATHS { get; set; }
        public string COUNTY { get; set; }
        public string STATE { get; set; }
        public string NO_BATHS { get; set; }

    }
}
