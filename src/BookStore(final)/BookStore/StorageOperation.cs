using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;
using System.Collections;


namespace BookStore
{
    class StorageOperation
    {
        public ControlAccess ctrl = new ControlAccess();

        //生成一条操作ID
        public string NewManageId()
        {
            string sql1 = "select max(mag_num) from (select to_number(mag_id) mag_num from ManageRecord)";
            int max_num;
            int.TryParse(ctrl.ExecuteScalar(sql1).ToString(), out max_num);
            string manage_id = Convert.ToString(max_num + 1);
            return manage_id;
        }

        //新建一条操作记录,并返回操作id
        public string CreateManageRecord(string staff_id, int remark, string manage_id)
        {
            if (remark != 0 && remark != 1) { return null; }

            string manage_time = DateTime.Now.ToShortDateString();

            string sql2 = "insert into ManageRecord(mag_id, mag_time,mag_remark, sta_id) values('" +
                manage_id + "'," +"to_date('" + manage_time + "','yyyy-mm-dd')" + ",'" + remark + "','" + staff_id + "')";
            ctrl.ExecNonQuery(sql2);
            return manage_id;
        }

        //获取符合书籍描述的类型id, 若无对应类型，则新增一条元组
        public string GetType_idByDescription(string description)
        {
            if (description == null || description == "") return null;
            string sql = "select tp_id from Type where tp_name like '%" + description + "%'";
            OracleDataReader reader = ctrl.ExecuteReader(sql);
            if (reader.Read()) { return reader[0].ToString(); }
            else
            {
                return InsertType(description);
            }

        }
        
        //向类型表中插入一条类型元组，返回该类型id
        public string InsertType(string type_name)
        {
            string sql1 = "select max(tp_num) from (select to_number(tp_id) tp_num from Type)";
            int max_num = Convert.ToInt32(ctrl.ExecuteScalar(sql1));
            string ntp_id = Convert.ToString(max_num + 1);

            string sql2 = "select reg_id from Type where tp_id = '" + max_num.ToString() + "'";
            string nreg_id = Convert.ToString(ctrl.ExecuteScalar(sql2));

            string sql3 = "insert into Type(tp_id, tp_name, reg_id) values ('" + ntp_id + "', '" + type_name + "', '"
                          + nreg_id + "')";
            ctrl.ExecNonQuery(sql3);
            return ntp_id;
        }

        //通过种类id获得对应区域id，在该区域中寻找符合容量要求的书架
        public string GetShelf_id(string type_id, int amount)
        {
            if (type_id == null || type_id == "") return null;
            string sql1 = "select * from Shelf natural join Type where tp_id = '" + type_id + "'";
            OracleDataReader reader = ctrl.ExecuteReader(sql1);
            while (reader.Read())
            {
                int capacity = Convert.ToInt32(reader[reader.GetOrdinal("shel_capacity")]);
                if (capacity > amount) { return reader[reader.GetOrdinal("shel_id")].ToString(); }
            }
            Console.WriteLine("已无对应可用书架!");
            return null;
        }

        //向书籍表中插入一条书籍元组
        private int InsertBook(Book book)
        {
            if (!book.HasAnyNull())
            {
                int result = 0;
                string sql1 = "insert into Book(b_isbn, b_title, b_author, b_language, b_amount, b_publisher,b_cost_price, b_sell_price, shel_id, tp_id) values ('" +
                            book.isbn + "','" + book.title + "','" + book.author + "','" + book.language + "'," + book.amount + ",'" + book.publisher + "'," + book.cost_price +
                            "," + book.sell_price + ",'" + book.shelf_id + "','" + book.type_id + "')";
                result =  ctrl.ExecNonQuery(sql1);

                string sql2 = "update Shelf set shel_capacity = shel_capacity - " + book.amount + "where shel_id = '" + book.shelf_id + "'";
                ctrl.ExecNonQuery(sql2);
                return result;
            }
            else
            {
                Console.WriteLine("书籍信息录入不全");
                Console.Read();
                return 0;
            }
        }


        //获取符合条件的书籍的OracleReader
        private OracleDataReader FetchBook(Book book)
        {
            if (!book.HasAnyData()) return null;
            string sql = "select * from Book where ";
            string format = "";
            if (book.IsbnHasData()) { sql += "b_isbn = '" + book.isbn + "'"; format = " and ";}
            if (book.TitleHasData()) { sql += format + "b_title = '" + book.title + "'"; format = " and "; }
            if (book.AuthorHasData()) { sql += format + "b_author = '" + book.author + "'"; format = " and "; }
            if (book.LanguageHasData()) { sql += format + "b_language = '" + book.language + "'"; format = " and "; }
            if (book.AmountHasData()) { sql += format + "b_amount = " + book.amount; format = " and "; }
            if (book.PublisherHasData()) { sql += format + "b_publisher = '" + book.publisher + "'"; format = " and "; }
            if (book.Cost_priceHasData()) { sql += format + "b_cost_price = " + book.cost_price; format = " and "; }
            if (book.Sell_priceHasData()) { sql += format + "b_sell_price = " + book.sell_price; format = " and "; }
            if (book.Shelf_idHasData()) { sql += format + "shel_id = '" + book.shelf_id + "'"; format = " and "; }
            if (book.Type_idHasData()) { sql += format + "tp_id = '" + book.type_id + "'"; format = " and "; }
            OracleDataReader reader = ctrl.ExecuteReader(sql);
            return reader;
        }

        //查询符合条件图书信息
        public Book[] SearchBook(Book cond_book)
        {
            ArrayList array = new ArrayList();
            OracleDataReader reader = FetchBook(cond_book);
            if (reader == null) return null;
            bool judge;
            while (reader.Read())
            {
                Book book = new Book();
                book.isbn = reader[reader.GetOrdinal("b_isbn")].ToString();
                book.title = reader[reader.GetOrdinal("b_title")].ToString();
                book.author = reader[reader.GetOrdinal("b_author")].ToString();
                book.language = reader[reader.GetOrdinal("b_language")].ToString();
                judge = int.TryParse(reader[reader.GetOrdinal("b_amount")].ToString(), out book.amount);
                if (!judge) { Console.WriteLine("Book amount data error!"); return null; }
                book.publisher = reader[reader.GetOrdinal("b_publisher")].ToString();
                judge = float.TryParse(reader[reader.GetOrdinal("b_cost_price")].ToString(), out book.cost_price);
                if (!judge) { Console.WriteLine("Book cost_price data error!"); return null; }
                judge = float.TryParse(reader[reader.GetOrdinal("b_sell_price")].ToString(), out book.sell_price);
                if (!judge) { Console.WriteLine("Book sell_price data error!"); return null; }
                book.shelf_id = reader[reader.GetOrdinal("shel_id")].ToString();
                book.type_id = reader[reader.GetOrdinal("tp_id")].ToString();
                array.Add(book);
            }
            reader.Close();
            if (array.Count == 0) return null;
            Book[] book_array = new Book[array.Count];
            array.CopyTo(book_array);
            return book_array;
        }


        //通过类型ID，获取类型名称
        public string GetTypeName(string type_id)
        {
            if (type_id == null || type_id == "") return null;
            string sql1 = "select tp_name from Type where tp_id  = '" + type_id + "'";
            object ob = ctrl.ExecuteScalar(sql1);
            if (ob == null)
            {
                return null;
            }
            else return ob.ToString();
        }

        //通过书架ID，获取书架摆放位置
        public string GetLocation(string shelf_id)
        {
            if (shelf_id == null || shelf_id == "") return null;
            string sql1 = "select reg_floor, reg_section from Region where reg_id = (select reg_id from Shelf where shel_id = '" + shelf_id + "')";
            OracleDataReader reader = ctrl.ExecuteReader(sql1);
            if (reader != null)
            {
                reader.Read();
                string location = reader[0].ToString() + "楼-" + reader[1].ToString() + "区";
                return location;
            }
            return null;
        }

        //按批更新图书信息
        public int ModifyBookInfo(Book original, Book lastest)
        {
            OracleDataReader reader = FetchBook(original);
            int sccess_count = 0;
            while (reader.Read())
            {
                string sql = "update Book set ";
                string format = "";
                if (lastest.IsbnHasData()) { sql += "b_isbn = '" + lastest.isbn + "'"; format = " , "; }
                if (lastest.TitleHasData()) { sql += format + "b_title = '" + lastest.title + "'"; format = " , "; }
                if (lastest.AmountHasData()) { sql += format + "b_author = '" + lastest.author + "'"; format = " , "; }
                if (lastest.LanguageHasData()) { sql += format + "b_language = '" + lastest.language + "'"; format = " , "; }
                if (lastest.AmountHasData()) { sql += format + "b_amount = " + lastest.amount; format = " , "; }
                if (lastest.PublisherHasData()) { sql += format + "b_publisher = '" + lastest.publisher + "'"; format = " , "; }
                if (lastest.Cost_priceHasData()) { sql += format + "b_cost_price = " + lastest.cost_price; format = " , "; }
                if (lastest.Sell_priceHasData()) { sql += format + "b_sell_price = " + lastest.sell_price; format = " , "; }
                if (lastest.Shelf_idHasData()) { sql += format + "shel_id = '" + lastest.shelf_id + "'"; format = " , "; }
                if (lastest.Type_idHasData()) { sql += format + "tp_id = '" + lastest.type_id + "'"; format = " , "; }
                sql += " where b_isbn = " + reader[reader.GetOrdinal("b_isbn")]; 
                ctrl.ExecNonQuery(sql);
                sccess_count += 1;
            }
            return sccess_count;
        }

        //书籍入库
        public bool InputStock(Book book, string manage_id)
        {
            string sql1 = "select count(*) from Book natural join Shelf where b_isbn = '" + book.isbn + "' and shel_capacity >= " + book.amount;
            if (Convert.ToInt32(ctrl.ExecuteScalar(sql1)) != 0)
           {
               string sql2 = "update Book set b_amount = b_amount + " + book.amount + "where b_isbn = '" + book.isbn + "'";
               string sql3 = "update Shelf set shel_capacity = shel_capacity - " + book.amount +
                             " where shel_id = (select shel_id from book where b_isbn = '" + book.isbn + "')";
               ctrl.ExecNonQuery(sql2);
               ctrl.ExecNonQuery(sql3);
               CreateUpdateInfo(book.isbn, manage_id, book.amount);
               return true;
           }

           string sql4 = "select * from Book where b_isbn = '" + book.isbn + "'";
           if (ctrl.ExecuteScalar(sql4) != null) { return false; }
           else
           {
               if (!book.Shelf_idHasData()) { return false; }
               else
               {
                   InsertBook(book);
                   CreateUpdateInfo(book.isbn, manage_id, book.amount);
                   return true;
               }
           }
           return false;
        }

        //更新Update表
        public void CreateUpdateInfo(string isbn, string manage_id, int amount)
        {
            string sql1 = "insert into Updated(mag_id, b_isbn, upd_amount) values('" + manage_id + "','" + isbn + "'," + amount + ")";
           
            ctrl.ExecNonQuery(sql1); 

        } 

        //书籍出库
        public bool OutputStock(string staff_id, string isbn, int amount, string manage_id)
        {
            string sql1 = "select * from Book where b_isbn = '" + isbn + "'";
            OracleDataReader reader = ctrl.ExecuteReader(sql1);
            if (reader.Read())
            {
                int add_to_shelf = 0;
                if ( Convert.ToInt32(reader[reader.GetOrdinal("b_amount")]) >= amount)
                {
                    string sql2 = "update Book set b_amount = b_amount - " + amount +
                                  " where shel_id = (select shel_id from book where b_isbn = '" + isbn + "')";
                    ctrl.ExecNonQuery(sql2);
                    add_to_shelf = amount;
                }
                else
                {
                    add_to_shelf = Convert.ToInt32(ctrl.ExecuteScalar("select b_amount from Book where b_isbn = '" + isbn + "'"));
                    string sql3 = "update Book set b_amount = 0 where b_isbn = '" + isbn + "'";
                    ctrl.ExecNonQuery(sql3);
                    amount = add_to_shelf;
                }
                string sql4 = "update Shelf set shel_capacity = shel_capacity + " + add_to_shelf +
               "where shel_id = (select shel_id from Book where b_isbn = '" + isbn + "')";
                ctrl.ExecNonQuery(sql4);
                string mag_id = CreateManageRecord(staff_id, 0, manage_id);
                CreateUpdateInfo(isbn, mag_id, amount);
                return true;

            }
            else return false;
        }

        //图书阈值提醒
        public Book[] ThresholdReminder()
        {
            const int threshold = 5;
            ArrayList al = new ArrayList();
            string sql1 = "select b_isbn, b_amount from Book where b_amount <= " + threshold.ToString();
            OracleDataReader reader = ctrl.ExecuteReader(sql1);
            while (reader.Read())
            {
                al.Add(new Book(isbn1:reader[0].ToString(), amount1:Convert.ToInt32(reader[1])));
            }
            if (al.Count == 0) return null;
            else
            {
                Book[] book_array = new Book[al.Count];
                al.CopyTo(book_array);
                return book_array;
            }
        }

    }
}
