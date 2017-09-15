using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookStore
{
    /// <summary>
    /// SaleMen.xaml 的交互逻辑
    /// </summary>
    public partial class SaleMen : Window
    {
        public string staff_now = "1001";//需要在弹出窗口前置为登录员工的员工号
        trade_record traderecord;//交易记录
        reserve_record reserverecord;//预订记录

        public ControlAccess dbutil;
        private int sell;


        public SaleMen()
        {
            InitializeComponent();           
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            dbutil = new ControlAccess();
            newrecord();//创建交易窗口
            tra_id1.Text = traderecord.trade_number;
            newreserve();//创建预订窗口
            rev_id1.Text = reserverecord.reserve_number;
        }
        private void Mem1_Click(object sender, RoutedEventArgs e)
        {
            this.MemManagement.Visibility = Visibility.Visible;
            this.TradeManagement.Visibility = Visibility.Hidden;
            this.ReserveManagement.Visibility = Visibility.Hidden;

        }


        private void Trad_Click(object sender, RoutedEventArgs e)
        {
            this.MemManagement.Visibility = Visibility.Hidden;
            this.TradeManagement.Visibility = Visibility.Visible;
            this.ReserveManagement.Visibility = Visibility.Hidden;
        }

        private void Trad_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.MemManagement.Visibility = Visibility.Hidden;
            this.TradeManagement.Visibility = Visibility.Hidden;
            this.ReserveManagement.Visibility = Visibility.Visible;
        }





        //
        //销售
        //
        //添加书籍
        void showbooks()
        {
            bookbox.Items.Clear();
            for (int i = 0; i < traderecord.books.Count; i++)
            {
                WrapPanel panel = new WrapPanel();
                TextBlock tb = new TextBlock();
                tb.Text = traderecord.books[i].book_id;
                tb.Width = 250;
                TextBlock tb1 = new TextBlock();
                tb1.Text = Convert.ToString(traderecord.books[i].sell_amount);
                tb1.Width = 150;
                TextBlock tb2 = new TextBlock();
                tb2.Text = Convert.ToString(traderecord.books[i].left_amount);
                tb2.Width = 150;
                panel.Children.Add(tb);
                panel.Children.Add(tb1);
                panel.Children.Add(tb2);
                bookbox.Items.Add(panel);
            }
        }
        void addbook(string isbn, int number)
        {
            int imark = 0;
            bool bmark = true;
            //查找是否已有该书，若有，则直接加上购买数
            for (int i = 0; i < traderecord.books.Count; i++)
            {
                if (traderecord.books[i].book_id == isbn)
                {
                    traderecord.books[i].sell_amount += number;
                    if (traderecord.books[i].sell_amount > traderecord.books[i].left_amount) { bmark = false; traderecord.books[i].sell_amount -= number; }
                    imark = 1;
                    break;
                }
            }
            //若没有，新建书籍记录并添加
            if (imark == 0)
            {
                book_sell book = new book_sell(isbn);
                book.sell(number);
                if (book.left_amount < book.sell_amount) bmark = false;
                else traderecord.books.Add(book);
            }
            if (bmark) showbooks();
            else MessageBox.Show("库存不足！");//购买量大于库存时，显示库存不足
        }

        //查找当前会员已创建的未交易订单
        void selectrev()
        {
            string sql = "select rev_id from ReserveRecord where mem_id='" + mem_id1.Text + "' and is_success=0";
            DataSet set = dbutil.GetDataSet(sql);
            for (int i = 0; i < set.Tables[0].Rows.Count; i++)
            {
                res_id.Items.Add(Convert.ToString(set.Tables[0].Rows[i]["rev_id"]));
            }
            res_id.UpdateLayout();
        }
        //在交易中添加已有的预订订单
        void addrev_id()
        {
            string sql = "select b_isbn,incl_amount from Include where rev_id='" + res_id.Text + "'";
            DataSet set = dbutil.GetDataSet(sql);
            for (int i = 0; i < set.Tables[0].Rows.Count; i++)
            {
                addbook(Convert.ToString(set.Tables[0].Rows[i]["b_isbn"]), Convert.ToInt32(set.Tables[0].Rows[i]["incl_amount"]));
            }
        }
        //确定金额
        void getmoney()
        {
            traderecord.mem_id = mem_id1.Text;
            traderecord.res_id = res_id.Text;
            if (trade_way.Text == "现金") traderecord.trade_way = "money";
            else traderecord.trade_way = trade_way.Text;
            traderecord.getmoney();
            trade_money.Text = Convert.ToString(traderecord.money);
        }
        //确认交易
        void addtrade()
        {
            traderecord.writerecord();
            if (traderecord.res_id != "") traderecord.change_reserve();
        }
        //交易订单初始化
        void newrecord()
        {
            traderecord = new trade_record();
            traderecord.getsta_id(staff_now);
            traderecord.gettrade_number();
        }
        //查询销售
        void tradeinfo()
        {
            //根据输入的员工号或会员号或交易号查询交易信息

            string sql;
            if (tra_id.Text == "" && mem_id.Text == "" && isbn.Text == "") sql = "select * from TradeRecord natural join Sell";
            else if (tra_id.Text == "" && mem_id.Text != "" && isbn.Text == "") sql = "select * from TradeRecord natural join Sell natural join Pay where mem_id='" + mem_id.Text + "'";
            else if (tra_id.Text != "" && mem_id.Text == "" && isbn.Text == "") sql = "select * from TradeRecord natural join Sell where tra_id='" + tra_id.Text + "'";
            else if (tra_id.Text == "" && mem_id.Text == "" && isbn.Text != "") sql = "select * from TradeRecord natural join Sell where b_isbn='" + isbn.Text + "'";
            else if (tra_id.Text == "" && mem_id.Text != "" && isbn.Text != "") sql = "select * from TradeRecord natural join Sell natural join Pay where mem_id='" + mem_id.Text + "' and b_isbn='" + isbn.Text + "'";
            else if (tra_id.Text != "" && mem_id.Text != "" && isbn.Text == "") sql = "select * from TradeRecord natural join Sell natural join Pay where mem_id='" + mem_id.Text + "' and tra_id='" + tra_id.Text + "'";
            else if (tra_id.Text != "" && mem_id.Text == "" && isbn.Text != "") sql = "select * from TradeRecord natural join Sell where tra_id='" + tra_id.Text + "' and b_isbn='" + isbn.Text + "'";
            else sql = "select * from TradeRecord natural join Sell natural join Pay where mem_id='" + mem_id.Text + "'tra_id='" + tra_id.Text + "' and b_isbn='" + isbn.Text + "'";
            DataSet set = dbutil.GetDataSet(sql);
            databox.ItemsSource = set.Tables[0].DefaultView;
        }


        //
        //预定
        //
        //查询预订信息
        void reserveinfo()
        {
            //根据输入的会员号或预定号查询预订信息
            string sql;
            if (rev_id.Text == "" && mem_id2.Text == "" && isbn2.Text == "") sql = "select * from ReserveRecord natural join Include";
            else if (rev_id.Text == "" && mem_id2.Text != "" && isbn2.Text == "") sql = "select * from ReserveRecord natural join Include where mem_id='" + mem_id2.Text + "'";
            else if (rev_id.Text != "" && mem_id2.Text == "" && isbn2.Text == "") sql = "select * from ReserveRecord natural join Include where rev_id='" + rev_id.Text + "'";
            else if (rev_id.Text == "" && mem_id2.Text == "" && isbn2.Text != "") sql = "select * from ReserveRecord natural join Include where b_isbn='" + isbn2.Text + "'";
            else if (rev_id.Text == "" && mem_id2.Text != "" && isbn2.Text != "") sql = "select * from ReserveRecord natural join Include where mem_id='" + mem_id2.Text + "' and b_isbn='" + isbn2.Text + "'";
            else if (rev_id.Text != "" && mem_id2.Text != "" && isbn2.Text == "") sql = "select * from ReserveRecord natural join Include where mem_id='" + mem_id2.Text + "' and rev_id='" + rev_id.Text + "'";
            else if (rev_id.Text != "" && mem_id2.Text == "" && isbn2.Text != "") sql = "select * from ReserveRecord natural join Include where rev_id='" + rev_id.Text + "' and b_isbn='" + isbn2.Text + "'";
            else sql = "select * from ReserveRecord natural join Include where mem_id='" + mem_id2.Text + "'rev_id='" + rev_id.Text + "' and b_isbn='" + isbn2.Text + "'";
            DataSet set = dbutil.GetDataSet(sql);
            rev_box.ItemsSource = set.Tables[0].DefaultView;
        }
        //添加书籍并显示
        void showrevbooks()
        {
            listView_Copy.Items.Clear();
            for (int i = 0; i < reserverecord.books.Count; i++)
            {
                WrapPanel panel = new WrapPanel();
                TextBlock tb = new TextBlock();
                tb.Text = reserverecord.books[i].book_id;
                tb.Width = 250;
                TextBlock tb1 = new TextBlock();
                tb1.Text = Convert.ToString(Convert.ToString(reserverecord.books[i].res_amount));
                tb1.Width = 170;
                panel.Children.Add(tb);
                panel.Children.Add(tb1);
                listView_Copy.Items.Add(panel);
            }
        }
        void addreservebook()
        {
            for (int i = 0; i < reserverecord.books.Count; i++)
            {
                if (reserverecord.books[i].book_id == isbn3.Text)
                {
                    reserverecord.books[i].res_amount += Convert.ToInt32(amount.Text);
                    showrevbooks();
                    return;
                }
            }
            book_sell book = new book_sell(isbn3.Text);
            book.reserve(Convert.ToInt32(amount.Text));
            reserverecord.books.Add(book);
            showrevbooks();
        }
        //添加预定记录
        void addreserve()
        {
            reserverecord.mem_id = mem_id3.Text;
            reserverecord.writerecord();
        }
        //预订订单初始化
        void newreserve()
        {
            reserverecord = new reserve_record();
            reserverecord.getreserve_number();
        }


        //预订
        private void button1_Copy10_Click(object sender, RoutedEventArgs e)
        {
            addreservebook();
        }

        private void button_Copy6_Click(object sender, RoutedEventArgs e)
        {
            addreserve();
            MessageBox.Show("预订成功！");
            reserverecord.clear();
            reserverecord.getreserve_number();
            rev_id1.Text = reserverecord.reserve_number;
            showrevbooks();
            mem_id3.Text = reserverecord.mem_id;
            isbn3.Text = null;
            amount.Text = null;
        }

        private void button1_Copy9_Click(object sender, RoutedEventArgs e)
        {
            reserverecord.clear();
            showrevbooks();
            mem_id3.Text = reserverecord.mem_id;
            isbn3.Text = null;
            amount.Text = null;
        }

        private void button_Copy7_Click(object sender, RoutedEventArgs e)
        {
            reserveinfo();
        }


        //销售
        private void button_Click(object sender, RoutedEventArgs e)
        {
            tradeinfo();
        }

        private void button_Copy4_Click(object sender, RoutedEventArgs e)
        {
            selectrev();
        }

        private void button1_Copy5_Click(object sender, RoutedEventArgs e)
        {
            addrev_id();
        }

        private void button1_Copy6_Click(object sender, RoutedEventArgs e)
        {
            addbook(isbn1.Text, Convert.ToInt32(amount1.Text));
        }

        private void button1_Copy7_Click(object sender, RoutedEventArgs e)
        {
            getmoney();
        }

        private void button_Copy5_Click(object sender, RoutedEventArgs e)
        {
            addtrade();
            MessageBox.Show("交易成功！");
            traderecord.clear();
            traderecord.gettrade_number();
            tra_id1.Text = traderecord.trade_number;
            showbooks();
            mem_id1.Text = null;
            isbn1.Text = null;
            amount1.Text = null;
            trade_money.Text = null;
            res_id.Text = null;
        }

        private void button1_Copy8_Click(object sender, RoutedEventArgs e)
        {
            traderecord.clear();
            showbooks();
            mem_id1.Text = null;
            isbn1.Text = null;
            amount1.Text = null;
            trade_money.Text = null;
            res_id.Text = null;
        }


        //会员模块

        //会员查询按钮
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("ok");
            Member mem = new Member();
            string id = mid.Text;
            q_name.Text = mem.getmName(id);
            q_birth.Text = mem.getmBirth(id).ToString();
            q_contact.Text = mem.getmTel(id);
            q_level.Text = mem.getmLevel(id);
            q_gentle.Text = mem.getmGender(id);
            q_id.Text = mem.getmAddr(id);
            q_discount.Text = mem.getmDiscount(id);
            q_open.Text = mem.getmOpenTime(id).ToString();
            q_end.Text = mem.getmExpireTime(id).ToString();
            q_address.Text = mem.getmAddr(id);

        }
        //会员注册按钮
        Member c_mem = new Member(); string le;
        private void c_level_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //   MessageBox.Show("ok");
            string one = "senior";
            string two = "middle";
            string three = "junior";
            if (c_level.SelectedIndex == 0) le = one;
            else if (c_level.SelectedIndex == 1) le = two;
            else if (c_level.SelectedIndex == 2) le = three;

            c_fare.Text = c_mem.findmFare(le).ToString();

            //  MessageBox.Show("a");
        }
        private void button_Copy2_Click(object sender, RoutedEventArgs e)
        {
            string monnum = c_open.Text;
            System.DateTime opentime = System.DateTime.Now;

            System.DateTime endtime, birthtime;
            c_id.Text = c_mem.getNextID();
            c_mem.setmOpenTime();

            string birth = c_birth.Text;
            birthtime = System.DateTime.Parse(birth.Substring(0, 4) + "-" + birth.Substring(4, 2) + "-" + birth.Substring(6, 2));
            endtime = c_mem.setmExpireTime(opentime, monnum);

            int intmoney; int shouldmoney; int returnmoney;
            intmoney = int.Parse(c_actual.Text);
            shouldmoney = int.Parse(c_should.Text);
            returnmoney = intmoney - shouldmoney;
            c_return.Text = returnmoney.ToString();
            c_mem.addMember(c_id.Text, c_name.Text, c_contact.Text, c_gender_cb.Text,
                birthtime, c_address.Text, opentime, endtime, le);
            MessageBox.Show("注册成功！");
        }

        private void c_open_TextChanged(object sender, TextChangedEventArgs e)
        {
            int i_fare;
            i_fare = int.Parse(c_fare.Text);
            c_should.Text = c_mem.getFareSum(i_fare, c_open.Text).ToString();
        }

        private void c_actual_TextChanged(object sender, TextChangedEventArgs e)
        {
            int c_i_fare, c_s_fare;
            c_i_fare = int.Parse(c_actual.Text);
            c_s_fare = int.Parse(c_should.Text);
            c_return.Text = (c_i_fare - c_s_fare).ToString();
        }

        //*******************会员续费
       // Member x_mem = new Member();
       // private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        //{
            //Member x_mem = new Member();

       // }

       /* private void x_id_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Member x_mem = new Member();
            string xid = x_id.Text;
            x_level.Text = x_mem.getmLevel(xid);
            x_opentime.Text = x_mem.getmOpenTime(xid).ToString();
            x_end_time.Text = x_mem.getmExpireTime(xid).ToString();

        }*/

        //private void x_s_time_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}

        //private void button_Copy1_Click(object sender, RoutedEventArgs e)
        //{
            //x_mem.updatemExpireTime(x_id,)
            // x_mem.updatemExpireTime()
        //}


      
        /*private void x_month_TextChanged(object sender, TextChangedEventArgs e)
        {
            x_mem.setmExpireTime(x_mem.setmOpenTime(), x_month.Text);
            string m_fare = x_mem.getmFare(x_id.Text);
            int fare = int.Parse(m_fare);
            x_should.Text = x_mem.getFareSum(fare, x_month.Text).ToString();
        }*/

        //更新会员信息 
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string uid = u_id.Text;
            Member u_mem = new Member();
            u_name.Text = u_mem.getmName(uid);
            u_contact.Text = u_mem.getmTel(uid);
            u_address.Text = u_mem.getmAddr(uid);
            u_birth.Text = u_mem.getmBirth(uid).ToString();
            u_open_time.Text = u_mem.getmOpenTime(uid).ToString();
            u_gender_cb.Text = u_mem.getmGender(uid);
            u_level.Text = u_mem.getmLevel(uid);
        }
        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            string uid = u_id.Text;
            Member u_mem = new Member();
            u_mem.updatemAddr(uid, u_address.Text);
            u_mem.updatemBirth(uid, u_birth.Text);
            u_mem.updatemGender(uid, u_gender_cb.Text);
            u_mem.updatemLevel(uid, u_level.Text);
            u_mem.updatemName(uid, u_name.Text);
            u_mem.updatemTel(uid, u_contact.Text);
            MessageBox.Show("更新会员信息成功！");
        }

       // private void s_name_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}

        private void s_id_TextChanged(object sender, TextChangedEventArgs e)
        {
            Member smem = new Member();
            string sid = s_id.Text;
            s_open_time.Text = smem.getmOpenTime(sid).ToString();
            s_y_fare.Text = smem.getmFare(sid);
            s_y_level.Text = smem.getmLevel(sid);
            s_name.Text = smem.getmName(sid);
            s_contact.Text = smem.getmTel(sid);
        }

        private void s_level_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Member smem = new Member();
            string sid = s_id.Text;
            string g_level = null;
            int tag = 0;
            string ori = smem.getmLevel(sid);
            if (ori == "junior") tag = 1;
            else if (ori == "middle") tag = 2;
            else if (ori == "senior") tag = 3;

            string one = "senior";
            string two = "middle";
            string three = "junior";
            if (s_level_cb.SelectedIndex == 0) g_level = one;
            else if (s_level_cb.SelectedIndex == 1) g_level = two;
            else if (s_level_cb.SelectedIndex == 2) g_level = three;


            smem.updatemLevel(sid, g_level);
            s_x_fare.Text = smem.getmFare(sid);
            s_shoule.Text = s_x_fare.Text;
        }

        private void s_actual_TextChanged(object sender, TextChangedEventArgs e)
        {
            int re_money, i_actual, i_should;
            i_actual = int.Parse(s_actual.Text);
            i_should = int.Parse(s_shoule.Text);
            re_money = i_actual - i_should;
            s_return.Text = re_money.ToString();

        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
         //   c_id.Text = c_mem.getNextID();
        }

        private void c_id_Loaded(object sender, RoutedEventArgs e)
        {
            c_id.Text = c_mem.getNextID();
        }
        Member x_mem = new Member();
        private void x_id_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            string xid = x_id.Text;
            x_name.Text = x_mem.getmName(xid);
            x_opentime.Text = x_mem.getmOpenTime(xid).ToString();
            x_end_time.Text = x_mem.getmExpireTime(xid).ToString();
            x_level.Text = x_mem.getmLevel(xid);
            x_fare.Text = x_mem.getmFare(xid);
        }

        private void x_month_TextChanged(object sender, TextChangedEventArgs e)
        {
            x_newend.Text = x_mem.setmExpireTime(System.DateTime.Now, x_month.Text).ToString();
      //      x_should.Text = (int.Parse(x_month.Text) * int.Parse(x_fare.Text)).ToString();
            x_should.Text = x_mem.getFareSum(int.Parse(x_month.Text), x_fare.Text).ToString();
        }

        private void x_should_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            x_mem.updatemExpireTime(x_id.Text, x_newend.Text);
            MessageBox.Show("会员续期成功 ！");
        }

        private void x_actual_TextChanged(object sender, TextChangedEventArgs e)
        {
            x_return.Text = (int.Parse(x_actual.Text) - int.Parse(x_should.Text)).ToString();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("会员升级成功！");
        }

       

    }
}
