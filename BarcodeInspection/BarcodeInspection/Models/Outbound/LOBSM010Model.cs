using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace BarcodeInspection.Models.Outbound
{
    public class LOBSM010Model 
    {

        public LOBSM010Model()
        {

        }

        //public LOBSM010Model(XElement element)
        //{
        //    Compky = element.Element(XName.Get("COMPKY")).Value;
        //    Wareky = element.Element(XName.Get("WAREKY")).Value;
        //    Rqshpd = element.Element(XName.Get("RQSHPD")).Value;
        //    Dlwrky = element.Element(XName.Get("DLWRKY")).Value;
        //    Ruteky = element.Element(XName.Get("RUTEKY")).Value;
        //    Dlvycd = element.Element(XName.Get("DLVYCD")).Value;
        //    Dlvynm = element.Element(XName.Get("DLVYNM")).Value;
        //    Lb1Count = element.Element(XName.Get("LB1COUNT")).Value;
        //}

        [JsonProperty("compky")]
        public string Compky { get; set; }


        [JsonProperty("wareky")]
        public string Wareky { get; set; }

        [JsonProperty("rqshpd")]
        public string Rqshpd { get; set; }

        [JsonProperty("dlwrky")]
        public string Dlwrky { get; set; }

        [JsonProperty("ruteky")]
        public string Ruteky { get; set; }

        [JsonProperty("dlvycd")]
        public string Dlvycd { get; set; }


        [JsonProperty("dlvynm")]
        public string Dlvynm { get; set; }


        [JsonProperty("lbl_count")]
        public string Lb1Count { get; set; }
    }


}
