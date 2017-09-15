using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.OracleClient;

namespace BookStore
{
    /// <summary>
    /// ShopKeeper.xaml 的交互逻辑
    /// </summary>
    public partial class ShopKeeper : Window
    {
        ControlAccess conn = new ControlAccess();
        public ShopKeeper()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.StaffManagement.Visibility = Visibility.Visible;
            this.ReportManagement.Visibility = Visibility.Hidden;
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.StaffManagement.Visibility = Visibility.Hidden;
            this.ReportManagement.Visibility = Visibility.Visible;
        }

        private void button_Copy3_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            Staff staInstance = new BookStore.Staff();
            StaffManagement staM = new BookStore.StaffManagement();
            //查询结果：true--查询到，false--未查询到
            bool iRes=staM.InquireStaffInfo(textBox_Copy11.Text, staInstance, conn);
            if (iRes == true)
            {
                DateTime dt = Convert.ToDateTime(staInstance.Sta_birth);
                DateTime dt2 = Convert.ToDateTime(staInstance.Sta_sign_date);

                textBox2.Text = staInstance.Sta_name;
                textBox3.Text = staInstance.Sta_gender;
                textBox6.Text = staInstance.Sta_address;
                textBox4.Text = staInstance.Sta_tele;
                textBox5.Text = Convert.ToString(dt.Year) + "/" + Convert.ToString(dt.Month) + "/" + Convert.ToString(dt.Day);
                textBox7.Text = Convert.ToString(dt2.Year) + "/" + Convert.ToString(dt2.Month) + "/" + Convert.ToString(dt2.Day);
                

                string st = "";
                if (staInstance.Sta_on_job == "Yes") st = "在职";
                if (staInstance.Sta_on_job == "No") st = "离职";
                textBox7_Copy3.Text =st;
                string str="";
                if (staInstance.Po_title == "Cashier") str = "销售管理";
                if (staInstance.Po_title == "Clerk") str = "库存管理";
                if (staInstance.Po_title == "Manager") str = "管理员";

                textBox7_Copy2.Text=str;
            }
            else
            {
                //*****************************未查询到，弹框！
            }
        }

        private void textBox3_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboBox_Copy4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void textBox7_Copy5_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void button1_Copy2_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            Staff staInstance = new BookStore.Staff();
            StaffManagement staM = new BookStore.StaffManagement();
            //查询结果：true--查询到，false--未查询到
            bool iRes = staM.InquireStaffInfo(textBox_Copy.Text, staInstance, conn);
            if (iRes == true)
            {
                DateTime dt = Convert.ToDateTime(staInstance.Sta_birth);
                DateTime dt2 = Convert.ToDateTime(staInstance.Sta_sign_date);

                textBox2_Copy.Text = staInstance.Sta_name;
                textBox3_Copy.Text = staInstance.Sta_gender;
                textBox6_Copy.Text = staInstance.Sta_address;
                textBox4_Copy.Text = staInstance.Sta_tele;
                textBox5_Copy.Text = Convert.ToString(dt.Year) + "/" + Convert.ToString(dt.Month) + "/" + Convert.ToString(dt.Day);
                textBox7_Copy.Text = Convert.ToString(dt2.Year) + "/" + Convert.ToString(dt2.Month) + "/" + Convert.ToString(dt2.Day);
                string st = "";
                if (staInstance.Sta_on_job == "Yes") st = "在职";
                if (staInstance.Sta_on_job == "No") st = "离职";
                textBox7_Copy5.Text = st;
                string str = "";
                if (staInstance.Po_title == "Cashier") str = "销售管理";
                if (staInstance.Po_title == "Clerk") str = "库存管理";
                if (staInstance.Po_title == "Manager") str = "管理员";

                textBox7_Copy4.Text = str;
            }
            else
            {
                MessageBox.Show("未查询到！");
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //**********************************************************此处应有数据格式检测！
            conn.Open();
            StaffManagement staM = new BookStore.StaffManagement();

            string str1 = "";
            string str2 = "";
            if (textBox7_Copy4.Text == "销售管理") str1 = "Clerk";
            else if (textBox7_Copy4.Text == "库存管理") str1 = "Cashier";
            else if (textBox7_Copy4.Text == "管理员") str1 = "Manager";
            else { MessageBox.Show("岗位信息错误！"); return; }
            if (textBox7_Copy5.Text == "在职") str2 = "Yes";
            else if (textBox7_Copy5.Text == "离职") str2 = "No";
            staM.UpdateStaffInfo(textBox_Copy.Text, textBox2_Copy.Text, textBox3_Copy.Text, textBox6_Copy.Text, 
                textBox5_Copy.Text, textBox4_Copy.Text, textBox7_Copy.Text,str1,str2, conn);
            MessageBox.Show("插入成功！");
        }

        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {
            textBox_Copy.Text = "";
            textBox2_Copy.Text = "";
            textBox3_Copy.Text = "";
            textBox6_Copy.Text = "";
            textBox5_Copy.Text = "";
            textBox4_Copy.Text = "";
            textBox7_Copy.Text = "";
            textBox7_Copy4.Text = "";
            textBox7_Copy5.Text = "";
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //日期格式检查：yyyy-mm-dd
            conn.Open();
            StaffManagement staM = new BookStore.StaffManagement();
            if(checkBox1.IsChecked==true&&checkBox2.IsChecked==true&&checkBox3.IsChecked==false)
            {
                staM.CreateAttendInfo(textBox10.Text, textBox.Text, '1', '1', '0',conn);
            }
            else if(checkBox1.IsChecked==true&&checkBox2.IsChecked==false&&checkBox3.IsChecked==false)
            {
                staM.CreateAttendInfo(textBox10.Text, textBox.Text, '1', '0', '0', conn);
            }
            else if(checkBox1.IsChecked==false&&checkBox2.IsChecked==true&&checkBox3.IsChecked==false)
            {
                staM.CreateAttendInfo(textBox10.Text, textBox.Text, '0', '1', '0', conn);
            }
            else
            {
                MessageBox.Show("请仔细检查出勤信息--缺勤时无迟到、早退");
                Console.WriteLine("check again!");
            }
        }

        private void button_Copy2_Click(object sender, RoutedEventArgs e)
        {
            //***************************************输入数据格式检查
            conn.Open();
            StaffManagement staM = new BookStore.StaffManagement();
            bool iRes = staM.CreateLeaveInfo(textBox11.Text, textBox12.Text, textBox13.Text, Convert.ToInt16(textBox14.Text), conn);
            if (iRes == true)
            {
                MessageBox.Show("请假记录创建成功！");
            }
            else
            {
                MessageBox.Show("已有此条请假记录，请假记录创建失败");
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            textBox11.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
            textBox14.Text = "";
        }

        

        private void button2_Copy_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            StaffManagement staM = new BookStore.StaffManagement();
            DataSet ds=staM.InquireLeave2(textBox9.Text, conn);
            if (ds != null)
                dataGrid2.ItemsSource = ds.Tables[0].DefaultView;
            else
            {
                MessageBox.Show("无请假记录");
            }
        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void button1_Copy1_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            StaffManagement staM = new BookStore.StaffManagement();
            DataSet ds = staM.InquireAttendance2(textBox16.Text, textBox15.Text, conn);
            if (ds != null)
                dataGrid.ItemsSource = ds.Tables[0].DefaultView;
            else
            {
                MessageBox.Show("无出勤记录！");
            }
            DataSet ds2 = staM.InquireMonthStatistics2(textBox16.Text, conn);
            if (ds2 != null)
                dataGrid1.ItemsSource = ds2.Tables[0].DefaultView;
            else
            {
                MessageBox.Show("无月统计记录！");
            }
        }

        private void textBox14_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(textBox12.Text!=null&&textBox12.Text!=""&&textBox14.Text!="")
            {
                DateTime dt = Convert.ToDateTime(textBox12.Text);
                DateTime dt2 = dt.AddDays(Convert.ToInt32(textBox14.Text));
                string str = Convert.ToString(dt2.Year)+"/" + Convert.ToString(dt2.Month)+"/" + Convert.ToString(dt2.Day);
                textBox13.Text = str;
            }
        }

        private void textBox5_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void button2_Copy1_Click(object sender, RoutedEventArgs e)
        {
            functions func = new functions();
            string str;
            if (ComboBox1.Text == "库存管理") str = "Clerk";
            else str = "Cashier";
            func.createUser(textBox2_Copy1.Text,
            textBox3_Copy1.Text,
            textBox6_Copy1.Text,
            textBox5_Copy1.Text,
            textBox4_Copy1.Text,
            str,
            textBox5_Copy2.Text);
            textBox_Copy1.Text = func.msta_id; 
        }

        private void button2_Copy2_Click(object sender, RoutedEventArgs e)
        {
            textBox2_Copy1.Clear();
            textBox3_Copy1.Clear();
            textBox6_Copy1.Clear();
            textBox5_Copy1.Clear();
            textBox4_Copy1.Clear();
            ComboBox1.SelectedIndex=-1;
            textBox5_Copy2.Clear();
            textBox_Copy1.Clear();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            functions func = new functions();
            DataSet set = func.monthsell();
            dataGrid3.ItemsSource = set.Tables[0].DefaultView;
        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            functions func = new functions();
            DataSet set = func.tpyeSell();
            dataGrid4.ItemsSource = set.Tables[0].DefaultView;
        }
    }
}
