using System.Collections;

namespace RFCWEBAPIMVC.Models
{
    public class FSRep
    {
        public string company1 { get; set; }
        public string period1 { get; set; }
        public IList pArea1 { get; set; }
        public IList pSubArea1 { get; set; }
        public IList epfList1 { get; set; }
        public string fromDate1 { get; set; }
        public string toDate1 { get; set; }
    }
}