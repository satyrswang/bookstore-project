using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;


namespace BookStore
{
    class reserve_record
    {
        public string reserve_number;
        public List<book_sell> books;
        public string mem_id;
        public System.DateTime time;


        public ControlAccess dbutil;
        public reserve_record()
        {
            reserve_number = null;
            books = new List<book_sell>();
            mem_id = null;
            time = new System.DateTime();

            dbutil = new ControlAccess();
        }
        //得到预订订单号
        public void getreserve_number()
        {
            string sql = "select max(rev_id) from ReserveRecord";
            string sql1 = "select count(*) from ReserveRecord";
            long reserve_id;
            if (Convert.ToInt32(dbutil.ExecuteScalar(sql1)) == 0) reserve_id = 4001;
            else reserve_id = Convert.ToInt64(dbutil.ExecuteScalar(sql)) + 1;
            reserve_number = Convert.ToString(reserve_id);
        }

        public void writerecord()
        {
            //添加预订订单
            time = System.DateTime.Now;
            string sql1 = "insert into ReserveRecord(rev_id,rev_time,is_success,mem_id) values('" + reserve_number + "'," + "to_date('" + Convert.ToString(time) + "','yyyy-mm-dd,hh24:mi:ss')"+",'0',"+"'" + mem_id + "')";
            dbutil.ExecNonQuery(sql1);
            //添加预订订单号-书籍-数量
            for (int i = 0; i < books.Count; i++)
            {
                string sql = "insert into Include(rev_id,b_isbn,incl_amount) values('" + reserve_number + "','" + books[i].book_id + "'," + books[i].res_amount + ")";
                dbutil.ExecNonQuery(sql);
            }
        }
        //重置书籍信息
        public void clear()
        {
            books.Clear();
            mem_id = null;
        }
    }
}
