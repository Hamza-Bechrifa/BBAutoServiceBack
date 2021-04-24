using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Dtos.Version
{
    public class SuiviVersionDto
    {
        public int nbTerminalsConcerned { get; set; }
        public int nbRequestEffected { get; set; }
        public int nbUpdateEffected { get; set; }
        public List<TermianlConcernd> terminalsConcerned { get; set; }
    }
    public class TermianlConcernd
    {
        public string idTermianlConcernd { get; set; }
        public string idMagasinTermianlConcernd { get; set; }
        public string idMerchantTermianlConcernd { get; set; }
        public string dateRequestTMS { get; set; }
        public string ResultRequestTMS { get; set; }
        public string actualBinaryVersion { get; set; }
        public string actualConfVersion { get; set; }
        public int? binaryVersion { get; set; }
        public int? confVersion { get; set; }
        public string startDateBinary { get; set; }
        public string endDateBinary { get; set; }
        public string startDateConfig { get; set; }
        public string endDateConfig { get; set; }
        public string versionId { get; set; }
        public string GwVersionZipFile { get; set; }
    }
}
