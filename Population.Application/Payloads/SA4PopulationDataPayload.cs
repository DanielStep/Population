public class SA4PopulationDataPayload
    {
        public string GenericRegionCode { get; set; }
        public string Sex { get; set; }

        public RegionCodeType RegionCodeType
        {
            get
            {
                int result;
                int.TryParse(GenericRegionCode, out result);

                if (result <= 9)
                {
                    return RegionCodeType.StateCode;
                }
                else
                {
                    return RegionCodeType.ASGS_2016;
                }
            }
        }
    }