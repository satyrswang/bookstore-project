using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore
{
    class trade_record
    {
        public string trade_number;
        public List<book_sell> books;
        public string mem_id;
        public string trade_way;
        public float money;
        public float discount;
        public string sta_id;
        public string res_id;
        public System.DateTime time;

        public ControlAccess dbutil;

        public trade_record()
        {
            trade_number = null;
            books = new List<book_sell>();
            mem_id = null;
            trade_way = null;
            res_id = null;
            money = 0;
            discount = 1;
            sta_id = null;
            time = new System.DateTime();


            dbutil = new ControlAccess();
        }

        public void getsta_id(string msta_id)
        {
            sta_id = msta_id;
        }
        public void gettrade_number()
        {
            string sql = "select max(tra_id) from TradeRecord";
            string sql1 = "select count(*） from TradeRecord";
            long trade_id;
            if (Convert.ToInt32(dbutil.ExecuteScalar(sql1)) == 0) trade_id = 5001;
            else trade_id = Convert.ToInt64(dbutil.ExecuteScalar(sql)) + 1;
            trade_number = Convert.ToString(trade_id);
        }
        //计算折扣
        public void getdiscount()
        {
            double pay=1;
            double mem = 1;
            string sql2 = "select pm_discount from PayMethod where pm_method='" + trade_way + "'";
            string str = Convert.ToString(dbutil.ExecuteScalar(sql2));
            pay = Convert.ToDouble(dbutil.ExecuteScalar(sql2));
            if (mem_id != null&&mem_id!="")
            {
                string sql1 = "select lev_discount from MemberLevel natural join Member where mem_id='" + mem_id + "'";
                mem = Convert.ToDouble(dbutil.ExecuteScalar(sql1));
            }
            discount = (float)(pay * mem);
        }
        //计算金额
        public void getmoney()
        {
            getdiscount();
            for (int i = 0; i < books.Count; i++)
                money += books[i].money * books[i].sell_amount;
            money = money * discount;
        }
        //更改预定状态
        public void change_reserve()
        {
            string sql = "update ReserveRecord set is_success=1 where rev_id='" + res_id + "'";
            dbutil.ExecNonQuery(sql);
        }
        //添加纪录
        public void writerecord()
        {
            //添加交易记录
            time = System.DateTime.Now;
            string sql1 = "insert into TradeRecord(tra_id,tra_time,tra_money,sta_id,pm_method) values('" + trade_number + "'," + "to_date('" + Convert.ToString(time) + "','yyyy-mm-dd,hh24:mi:ss')" +","+money+ ",'" + sta_id + "','" + trade_way + "')";
            dbutil.ExecNonQuery(sql1);
            //添加交易单号-书号-数量
            for (int i = 0; i < books.Count; i++)
            {
                string sql = "insert into Sell(b_isbn,tra_id,sel_amount) values('" + books[i].book_id + "','" + trade_number + "'," + books[i].sell_amount + ")";
                string sql_1 = "update Book set b_amount=b_amount-" + books[i].sell_amount + " where b_isbn='" + books[i].book_id + "'";
                dbutil.ExecNonQuery(sql);
                dbutil.ExecNonQuery(sql_1);
            }
            //若为会员，添加交易单号-会员号
            if (mem_id != "")
            {
                string sql2 = "insert into PAY(tra_id,mem_id) values('" + trade_number + "','" + mem_id + "')";
                dbutil.ExecNonQuery(sql2);
            }
        }
        //重置书籍信息
        public void clear()
        {
            books.Clear();
            mem_id = null;
            res_id = null;
            money = 0;
            discount = 1;
        }
    }
}
