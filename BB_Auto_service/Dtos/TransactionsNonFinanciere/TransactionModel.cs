using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Dtos.TransactionsNonFinanciere
{
    public class TransactionModel
    {
        public string gw_cb_merchant { get; set; }
        public string gw_cb_magasin { get; set; }
        public string gw_cb_terminal { get; set; }
        public int? gw_cb_op_type { get; set; }
        public int? gw_cb_payment_type { get; set; }
        public string gw_cb_imei { get; set; }
        public string gw_cb_op_date { get; set; }
        public string gw_cb_card_mask { get; set; }
        public int? gw_cb_payment_status { get; set; }
        public int? gw_cb_cancel_status { get; set; }
        public string gw_cb_auth_data { get; set; }
        public string gw_cb_account_number { get; set; }
        public string gw_cb_amount { get; set; }
        public string gw_cb_voucher_number { get; set; }

        public int? gw_cb_code_status { get; set; }
        public string gw_cb_error_description { get; set; }



    }
}
