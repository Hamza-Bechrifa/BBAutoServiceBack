using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Dtos.TransactionsNonFinanciere
{
    public class SearchModel
    {
        public string gw_cb_merchant { get; set; }
        public string gw_cb_magasin { get; set; }
        public string gw_cb_terminal { get; set; }
        public int gw_cb_op_type { get; set; }
        public int gw_cb_payment_type { get; set; }
        public string dateDebut { get; set; }
        public string dateFin { get; set; }
        public string voucher { get; set; }

    }
}
