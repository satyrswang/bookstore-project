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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookStore
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            string staid = UserName.Text;
            string staPw = PassWord.Password;
            functions func = new functions();
            bool boo= func.checkLogin(staid, staPw);
            if (boo)
            {
                if(func.position=="Clerk")
                {
                    StorageAdmin storage = new StorageAdmin();
                    storage.mem_id = staid;
                    storage.UserId.Content = staid;
                    storage.UserName.Content = func.name;                    
                    storage.Show();
                    storage.button.IsEnabled = true;
                    this.Close();
                }
                else if(func.position=="Cashier")
                {
                    SaleMen sell = new SaleMen();
                    sell.staff_now = staid;
                    sell.UserId.Content = staid;
                    sell.UserName.Content = func.name;                    
                    sell.Show();
                    this.Close();
                }
                else
                {
                    ShopKeeper keeper = new ShopKeeper();
                    keeper.UserId.Content = staid;
                    keeper.UserName.Content = func.name;
                    keeper.button.IsEnabled = true;
                    keeper.button_Copy.IsEnabled = true;
                    keeper.Show();
                    this.Close();
                }
            }
        }
    }
}
