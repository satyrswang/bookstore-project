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
using System.Collections;

namespace BookStore
{
    /// <summary>
    /// StorageAdmin.xaml 的交互逻辑
    /// </summary>
    public partial class StorageAdmin : Window
    {
        private StorageOperation op = new StorageOperation();
        private ArrayList book_list = new ArrayList();
        private string mag_id = "";

        public string mem_id = "";
        public StorageAdmin()
        {
            InitializeComponent();
        }

        //查询书籍界面-----------------------------------------------------------------------------------------------
        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            string type_id = op.GetType_idByDescription(tex_type.Text);
            Book info = new Book(isbn1: tex_isbn.Text, title1: tex_title.Text, author1: tex_author.Text, publisher1: tex_publisher.Text, type_id1: type_id);

            Book[] book_array = op.SearchBook(info);
            if (book_array == null) return;

            DataTable table = new DataTable();
            DataColumn isbn = new DataColumn("书籍ID", typeof(string));
            DataColumn title = new DataColumn("书名", typeof(string));
            DataColumn author = new DataColumn("作者", typeof(string));
            DataColumn publisher = new DataColumn("出版社", typeof(string));
            DataColumn amount = new DataColumn("库存量", typeof(int));
            DataColumn cost_price = new DataColumn("进价", typeof(float));
            DataColumn sell_price = new DataColumn("售价", typeof(float));
            DataColumn type = new DataColumn("类型", typeof(string));
            DataColumn shelf = new DataColumn("书架位置", typeof(string));

            table.Columns.Add(isbn);
            table.Columns.Add(title);
            table.Columns.Add(author);
            table.Columns.Add(publisher);
            table.Columns.Add(amount);
            table.Columns.Add(cost_price);
            table.Columns.Add(sell_price);
            table.Columns.Add(type);
            table.Columns.Add(shelf);

            for (int i = 0; i < book_array.Length; i++)
            {
                DataRow row = table.NewRow();
                row["书籍ID"] = book_array[i].isbn;
                row["书名"] = book_array[i].title;
                row["作者"] = book_array[i].author;
                row["出版社"] = book_array[i].publisher;
                row["库存量"] = book_array[i].amount;
                row["进价"] = book_array[i].cost_price;
                row["售价"] = book_array[i].sell_price;
                row["类型"] = op.GetTypeName(book_array[i].type_id);
                row["书架位置"] = op.GetLocation(book_array[i].shelf_id);
                table.Rows.Add(row);
            }

            books_info.ItemsSource = table.DefaultView;
        }

        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            tex_isbn.Clear();
            tex_author.Clear();
            tex_publisher.Clear();
            tex_title.Clear();
            tex_type.Clear();
        }

        //书籍入库部分-------------------------------------------------------------------------------------------------------------
        private void button_Copy2_Click(object sender, RoutedEventArgs e)
        {

            string type_id = op.GetType_idByDescription(textBox2_Copy8.Text);
            int add_num = -1;
            float sell = -1;
            float cost = -1;
            int.TryParse(textBox2_Copy1.Text, out add_num);
            float.TryParse(textBox2_Copy6.Text, out sell);
            float.TryParse(textBox2_Copy7.Text, out cost);


            Book info = new Book(isbn1: textBox2.Text, title1: textBox2_Copy.Text, author1: textBox2_Copy3.Text,
                                language1: textBox2_Copy2.Text, amount1: add_num,
                                publisher1: textBox2_Copy4.Text, sell_price1: sell,
                                cost_price1: cost, shelf_id1: textBox1_Copy3.Text, type_id1: type_id);

            book_list.Add(info);
            Book[] book_array = new Book[book_list.Count];
            book_list.CopyTo(book_array);
            listView.Items.Clear();
            for (int i = 0; i < book_array.Length; i++)
            {
                WrapPanel panel = new WrapPanel();

                TextBlock isbn = new TextBlock();
                isbn.Text = book_array[i].isbn;
                isbn.Width = 100;
                panel.Children.Add(isbn);

                TextBlock title = new TextBlock();
                title.Text = book_array[i].title;
                title.Width = 50;
                panel.Children.Add(title);

                TextBlock author = new TextBlock();
                author.Text = book_array[i].author;
                author.Width = 40;
                panel.Children.Add(author);

                TextBlock language = new TextBlock();
                language.Text = book_array[i].language;
                language.Width = 40;
                panel.Children.Add(language);

                TextBlock amount = new TextBlock();
                amount.Text = book_array[i].amount.ToString();
                amount.Width = 40;
                panel.Children.Add(amount);

                TextBlock publisher = new TextBlock();
                publisher.Text = book_array[i].publisher;
                publisher.Width = 50;
                panel.Children.Add(publisher);

                TextBlock sell_price = new TextBlock();
                sell_price.Text = book_array[i].sell_price.ToString();
                sell_price.Width = 40;
                panel.Children.Add(sell_price);

                TextBlock cost_price = new TextBlock();
                cost_price.Text = book_array[i].cost_price.ToString();
                cost_price.Width = 40;
                panel.Children.Add(cost_price);

                TextBlock tp_id = new TextBlock();
                tp_id.Text = book_array[i].type_id;
                tp_id.Width = 40;
                panel.Children.Add(tp_id);

                listView.Items.Add(panel);

            }

            textBox2.Clear();
            textBox2_Copy.Clear();
            textBox2_Copy3.Clear();
            textBox2_Copy2.Clear();
            textBox2_Copy8.Clear();
            textBox2_Copy4.Clear();
            textBox2_Copy6.Clear();
            textBox2_Copy7.Clear();
            textBox1_Copy3.Clear();
            textBox2_Copy1.Clear();
        }

        private void textBox1_Loaded(object sender, RoutedEventArgs e)
        {
            mag_id = op.NewManageId();
            textBox1.Text = mag_id;
        }

        private void textBox_Copy4_Loaded(object sender, RoutedEventArgs e)
        {
            string time = DateTime.Now.ToShortDateString();
            textBox_Copy4.Text = time;
        }

        private void button_Copy3_Click_1(object sender, RoutedEventArgs e)
        {
            if (book_list.Count <= 0) return;
            Book[] book_array = new Book[book_list.Count];
            book_list.CopyTo(book_array);
            for (int i = 0; i < book_array.Length; i++)
            {
                if (i == 0) op.CreateManageRecord(mem_id, 1, mag_id);
                op.InputStock(book_array[i], mag_id);
            }


            book_list.Clear();
            listView.Items.Clear();
        }

        private void textBox2_Copy1_TextChanged(object sender, TextChangedEventArgs e)
        {
            int amount;
            int.TryParse(textBox2_Copy1.Text.Trim(), out amount);
            string tp_id = op.GetType_idByDescription(textBox2_Copy8.Text);
            string shel_id = op.GetShelf_id(tp_id, amount);
            if (shel_id == null || (textBox2_Copy1.Text.Trim() == string.Empty)) { textBox1_Copy3.Clear(); }
            else textBox1_Copy3.Text = shel_id;
        }


        //书籍出库部分---------------------------------------------------------------------------------------

        private void textBox3_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            mag_id = op.NewManageId();
            Book[] result = null;
            result = op.SearchBook(new Book(isbn1: textBox3.Text));
            if (result != null)
            {
                textBox3_Copy1.Text = result[0].title;
                textBox3_Copy2.Text = result[0].amount.ToString();
                textBox3_Copy3.Text = result[0].amount.ToString();
            }
            else
            {
                textBox3_Copy1.Clear();
                textBox3_Copy2.Clear();
                textBox3_Copy3.Clear();

            }
        }

        private void textBox3_Copy_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            int origin_num = 0;
            int.TryParse(textBox3_Copy2.Text, out origin_num);
            int out_num = 0;
            int.TryParse(textBox3_Copy.Text.Trim(), out out_num);

            int result = origin_num - out_num;
            if (result >= 0) textBox3_Copy3.Text = (result).ToString();
            else textBox3_Copy3.Text = "0";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (textBox3.Text == null || textBox3.Text == "") { MessageBox.Show("请输入书籍！"); return; }
            op.OutputStock(mem_id, textBox3.Text, Convert.ToInt32(textBox3_Copy.Text), mag_id);
        }

        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {
            textBox3.Clear();
            textBox3_Copy.Clear();
            textBox3_Copy1.Clear();
            textBox3_Copy2.Clear();
            textBox3_Copy3.Clear();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.tabControl.Visibility = Visibility.Visible;
        }

    }

}
