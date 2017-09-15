using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore
{
    class book_sell
    {
        public string book_id;
        public float money;
        public int left_amount;
        public int sell_amount;
        public int res_amount;


        public ControlAccess dbutil;
        public book_sell(string mbook_id)
        {
            book_id = mbook_id;
            sell_amount = 0;
            left_amount = 0;
            res_amount = 0;
            money = 0;


            dbutil = new ControlAccess();
        }
        public void sell(int amount)
        {
            string sql1 = "select b_amount from Book where b_isbn='" + book_id + "'";
            string sql2 = "select b_sell_price from Book where b_isbn='" + book_id + "'";
            left_amount = Convert.ToInt32(dbutil.ExecuteScalar(sql1));
            money = (float)(Convert.ToDouble(dbutil.ExecuteScalar(sql2)));
            sell_amount=amount;
        }
        public void reserve(int amount)
        {
            res_amount = amount;
        }
    }
}
