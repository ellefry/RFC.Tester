namespace BHSW2_2.Pinion.DataService.AppServices.Dtos
{
    public class OutboundTransferInput
    {
        /// <summary>
        /// Material No.
        /// </summary>
        public string MATNR { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        public int MENGE { get; set; }

        /// <summary>
        /// Deliever Plant
        /// </summary>
        public string WERKS { get; set; } = "WH00";

        /// <summary>
        /// Deliever stock location
        /// </summary>
        public string LGORT { get; set; }

        /// <summary>
        /// Special deliever flag
        /// </summary>
        public string SOBKZ { get; set; } = "Z";

        /// <summary>
        /// Special deliever stock code
        /// </summary>
        public string LIFNR_KDAUF { get; set; }

        /// <summary>
        /// Deliever stock status
        /// </summary>
        public string INSMK { get; set; }

        /// <summary>
        /// Receive plant
        /// </summary>
        public string UMWRK { get; set; } = "WH00";

        /// <summary>
        /// Receive storage location
        /// </summary>
        public string UMLGO { get; set; } = "3202";

        /// <summary>
        /// Special receive flag
        /// </summary>
        public string UMSOK { get; set; } = "Z";

        /// <summary>
        /// Rceive stock code
        /// </summary>
        public string EMLIF { get; set; }

        /// <summary>
        /// Settlement flag
        /// </summary>
        public string Zsettlement { get; set; }


        /// <summary>
        /// WMS voucher No. Unique code
        /// </summary>
        public string ZWMSNUMBER { get; set; }

        /// <summary>
        /// WMS No. current is null
        /// </summary>
        public string ZWMSNO { get; set; }

        public string ZWMSDATE { get; set; }

        public string ZWMSOPERATOR { get; set; }

        public string ZWMSTIME { get; set; }

        /// <summary>
        /// HU
        /// </summary>
        public string VEKP_EXIDV { get; set; }

        /// <summary>
        /// HU quantity
        /// </summary>
        public int VEPO_VEMN { get; set; }

    }
}
