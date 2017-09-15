using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OracleClient;
using System.Windows;
using System.Windows.Forms;
using BookStore;

namespace BookStore
{
    class functions
    {
        public string msta_id;
        public string position;
        public string name;
             public bool checkLogin(string sta_id,string password)
               {
                   sta_id = sta_id.Trim();
                   password = password.Trim();
                   ControlAccess ctrl = new ControlAccess();
                   string sqlStr1 = "select count(sta_id) from STAFF where sta_id='" + sta_id+"'";
                   if (Convert.ToInt32(ctrl.ExecuteScalar(sqlStr1)) <= 0)
                   {
                System.Windows.Forms.MessageBox.Show("没有这个账号，请重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
                   }
                   string sqlStr2 = string.Format("select count(*) from STAFF where sta_id='{0}'and sta_password='{1}' ", sta_id, password);
                   if(Convert.ToInt32(ctrl.ExecuteScalar(sqlStr2))==1)
                   {
                       string sql = string.Format("select po_title from STAFF where sta_id='{0}'", sta_id);
                       position = Convert.ToString(ctrl.ExecuteScalar(sql));
                       string sql1 = string.Format("select sta_name from STAFF where sta_id='{0}'", sta_id);
                       name = Convert.ToString(ctrl.ExecuteScalar(sql1));
                       return true;
                   }
                   else
                   {
                System.Windows.Forms.MessageBox.Show("密码不对，请重新输入密码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                       return false;
                   }
               }
       
        //创建账户
        //员工ID需要在数据库中设置主键为自动生成
        //需要用到的参数是员工的姓名，等级，地址，出生日期，电话号码，职位，密码，
     
        public bool  createUser(string sta_name, 
                               string sta_gender,
                               string sta_address,
                               string sta_birth,
                               string sta_tele,
                               string po_title,
                               string sta_password)
        {
            ControlAccess ctrl = new ControlAccess();

            System.DateTime currentTime = new System.DateTime();
            string strYMD = currentTime.ToString("d");        //生成入职时间


            string strmax = "select max(sta_id) from STAFF";
            int a = 1;
            int number = Convert.ToInt32(ctrl.ExecuteScalar(strmax));
            msta_id = Convert.ToString(number + 1);       //生成员工编号

            Staff sta = new Staff();
            sta.Sta_id = msta_id;
            sta.Sta_name = sta_name;
            sta.Sta_gender = sta_gender;
            sta.Sta_address = sta_address;
            sta.Sta_birth = sta_birth;
            sta.Sta_tele = sta_tele;
            sta.Po_title = po_title;
            sta.Sta_password = sta_password;
            sta.Sta_sign_date = strYMD;
            


           
            

           
            try
            {

            string sqlstr = "insert into STAFF values('" + sta.Sta_id + "'" + "," + "'" + sta.Sta_name + "'" + "," + "'" + sta.Sta_gender + "'" + "," + "'" + sta.Sta_address + "'" + "," + "to_date('" + sta.Sta_birth + "'" + "," + "'" + "yyyy-mm-dd" + "'" + ")" +","+"'"+ sta.Sta_tele + "'" + "," + "to_date('" + sta.Sta_birth + "'" + "," + "'" + "yyyy-mm-dd" + "'" + ")" + "," + "'" + sta.Po_title + "'" + "," + "'" + sta.Sta_password + "'"+","+"'"+"yes"+"'" + ")";
            Console.WriteLine(sqlstr);
            ctrl.ExecuteScalar(sqlstr);
           
               
            }
            catch (Exception ex)
            {
               ctrl.Close();
                return false;
            }
            finally
            {
               ctrl.Close();
           }
            return true;
        }
        //店长修改员工的职位
        //需要传入员工的ID和被修改为 的职位
        public  bool changePower(string sta_id,string po_title)
        {
            ControlAccess ctrl = new ControlAccess();
            try
            {
                string sqlstr = string.Format("update STAFF set po_title='{0}' where sta_id='{1}'", po_title, sta_id);
              //  ctrl.Open();
                ctrl.ExecuteScalar(sqlstr);
                
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                ctrl.Close();
            }
            return true;
        }
        //员工每月的月薪的计算
        //迟到早退扣20，缺勤扣100,请假扣80
        //需要输入员工的ID，年，月
        //结果返回int型的salary
       public void salaryPerMonth( string ms_year, string ms_month)
        {
            int ms_year1 = Convert.ToInt32(ms_year);
            int ms_month1 = Convert.ToInt32(ms_month);
            
            int salary = 0;
            ControlAccess ctrl = new ControlAccess();
            try
            {
                //System.DateTime currentTime = new System.DateTime();
                // string strYMD = currentTime.ToString("d");
                string sqlid = "select sta_id from staff where sta_on_job='yes'";
                OracleDataReader odr= ctrl.ExecuteReader(sqlid);
                if (odr.HasRows)
                {
                    while (odr.Read())
                    {
                        string sta_id = Convert.ToString( odr[0]);
                        string sqlsal = string.Format("select po_salary from POSITION natural join STAFF  where  STAFF.sta_id='{0}'", sta_id);
                        string sqlday = string.Format("select ms_absent,ms_lea_early,ms_late,ms_leave from MONTHSTATISTICS where sta_id='{0}' and ms_year='{1}' and ms_month='{2}'", sta_id, ms_year1, ms_month1);


                        DataSet set = ctrl.GetDataSet(sqlday);
                        salary = Convert.ToInt32(ctrl.ExecuteScalar(sqlsal)) - Convert.ToInt32(set.Tables[0].Rows[0][0]) * 100 - (Convert.ToInt32(set.Tables[0].Rows[0][1]) + Convert.ToInt32(set.Tables[0].Rows[0][2])) * 20 - Convert.ToInt32(set.Tables[0].Rows[0][3]) * 80;
                        string sqlupdate = string.Format("update MONTHSTATISTICS set ms_salary='{0}'", salary);
                        ctrl.ExecuteScalar(sqlupdate);
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                ctrl.Close();
            }
            
        }


        //每月员工上班状态
        //需要输入年月
        public DataSet monthstatics(string ms_year, string ms_month)
       {
           salaryPerMonth(ms_year, ms_month);
           ControlAccess ctrl = new ControlAccess();
           string str = string.Format("select sta_id,ms_attend,ms_absent,ms_lea_early,ms_late,ms_leave,ms_salary from MONTHSTATISTICS where ms_year='{0}' and ms_month='{1}'order by sta_id", ms_year, ms_month);
           return ctrl.GetDataSet(str);
        }

        //某个员工在某年某月的销售额
        public float permonthsell(string sta_id,string year,string month)

        {
            float monthSell=0;
            int year1 = Convert.ToInt32(year);
            int month1 = Convert.ToInt32(month);
            string sql = string.Format("select tra_money,tra_time from TRADERECORD where sta_id='{0}' ",sta_id);
            ControlAccess ctrl = new ControlAccess();
            
                OracleDataReader odr = ctrl.ExecuteReader(sql);
                if (odr.HasRows)
                {
                    while (odr.Read())
                    {
                        float amount = Convert.ToInt32(odr[0]);
                        DateTime time=Convert.ToDateTime(odr[1]);
                        if (time.Year == year1 && time.Month == month1)
                        {
                            monthSell = monthSell + amount;
                        }
                    }
                }

                return monthSell;
            
                ctrl.Close();

               
        }
        public String findName(string sta_id)
        {
            ControlAccess ctrl = new ControlAccess();
            string str = String.Format("select sta_name from STAFF where sta_id='{0]'", sta_id);
            string sta_name = Convert.ToString(ctrl.ExecuteScalar(str));
            return sta_name;
        }
        public DataSet monthsell()
        {
                string sql= "select sta_id as ID,sum(tra_money)   Amount,to_char(tra_time,'yyyy-mm') as Month   from TRADERECORD group by sta_id,to_char(tra_time,'yyyy-mm')";
                ControlAccess ctrl = new ControlAccess();
                DataSet set=  ctrl.GetDataSet(sql);
            
                
            //    set.Tables[0].Columns.Add("sta_name", typeof(System.String));
            //    foreach (DataRow row in set.Tables[0].Rows)
            //    {
            //        row[3] = findName(Convert.ToString(row[0]));
            //    }
                return set;
                ctrl.Close();

        }


        //按月分组统计每一种类型的书籍的销售量
        public DataSet tpyeSell()
        {
           // string time=ms_year+"/"+ms_month;
          //  DateTime timeend =Convert.ToDateTime(time).AddMonths(1).AddDays(-1);
          //  DateTime  timebegin= Convert.ToDateTime(time);

           string sql = "select tp_name  ID,sum(sel_amount) Amount,to_char(tra_time,'yyyy-mm') as Month from TRADERECORD natural join SELL natural join BOOK natural join TYPE group by tp_name,to_char(tra_time,'yyyy-mm') order by sum(sel_amount)";
           ControlAccess ctrl = new ControlAccess();
           DataSet set=  ctrl.GetDataSet(sql);
           return set;
           ctrl.Close();

        }



       

       
    }
}
