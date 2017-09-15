using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookStore
{
    public class Member
    /**会员信息类**/
    {
        private string mID;     //会员号
        private string mName;   //会员姓名
        private string mTel;    //会员手机号
        private string mGender; //会员性别
        private DateTime mBirth;//会员生日,可为空
        private string mAddr;   //会员住址,可为空
        private DateTime mOpenTime;  //会员开通时间
        private DateTime mExpireTime;//会员截止日期
        private string mLevel;  //会员等级
        private int mFare;      //会员月费
      /*  public void qqqq()
        {
            mID = mName = mTel = mGender = mAddr = mLevel  = null;
            mFare = 0;
           // mBirth = mOpenTime = mExpireTime = 0;
        }
*/
        /**实例化数据库通用方法类DbUtil
         * 问题：员工登陆时已根据账号和密码实例化一个DbUtil（比如db1）
         * 解决：最后删除下面这句实例化，直接调用db1
        **/
        ControlAccess dbutil = new ControlAccess();

        /***************************
         * 以下是会员注册相关函数
         ***************************/
        public string getNextID()  //根据会员表里maxID生成新的会员号
        {
            int maxID;
            string sql = "select max(mem_id) from Member";
            maxID = Convert.ToInt32(dbutil.ExecuteScalar(sql));
            return Convert.ToString(maxID+1);
        }

        public DateTime setmOpenTime()  //以系统当前时间作为会员开通时间
        {
            System.DateTime tupdatetime = new System.DateTime();
            tupdatetime = System.DateTime.Now;
            return tupdatetime;             
        }

        public DateTime setmExpireTime(DateTime mOpenTime, string monthNum)  //根据opentime和monthnum算出会员截止日期
        {
            return mOpenTime.AddMonths(Convert.ToInt32(monthNum));
        }

        public int findmFare(string mLevel)  //根据会员等级查找相应会费
        {
            string sql = "select lev_fare from MemberLevel where lev_title = '" + mLevel + "'";
            mFare = Convert.ToInt32(dbutil.ExecuteScalar(sql));
            return mFare;
        }

        public int getFareSum(int mFare, string monthNum) //根据月费和月份数算出应收取的总费用
        {
            return mFare * Convert.ToInt32(monthNum);
        }

        public void addMember(string mID, string mName, string mTel, string mGender,DateTime mBirth, 
                              string mAddr, DateTime mOpenTime, DateTime mExpireTime, string mLevel)  //向Member表里添加一行数据
        {
            string sql = "insert into Member values('" + mID + "','" + mName + "','" + mTel + "','" + mGender + "',"+ 
                          "to_date('" + mBirth + "','yyyy-mm-dd,hh24:mi:ss')" + ",'"+ mAddr + "',"+"to_date('" + mOpenTime + 
                          "','yyyy-mm-dd,hh24:mi:ss')"+"," + "to_date('"+mExpireTime + "','yyyy-mm-dd,hh24:mi:ss')" + ",'" + mLevel + "')";
            int result = dbutil.ExecNonQuery(sql);
        }


        /*********************************
         * 以下是各类查找函数（根据会员号）
         *********************************/
        public string getmName(string mID)  //查找并返回会员姓名
        {
            string sql = "select mem_name from Member where mem_id = '" + mID + "'";
            mName = Convert.ToString(dbutil.ExecuteScalar(sql));
            return mName;
        }

        public string getmTel(string mID)  //查找并返回会员电话
        {
            string sql = "select mem_tele from Member where mem_id = '" + mID + "'";
            mTel = Convert.ToString(dbutil.ExecuteScalar(sql));
            return mTel;
        }

        public string getmGender(string mID)  //查找并返回会员性别
        {
            string sql = "select mem_gender from Member where mem_id = '" + mID + "'";
            mGender = Convert.ToString(dbutil.ExecuteScalar(sql));
            return mGender;
        }

        public DateTime getmBirth(string mID)  //查找并返回会员生日
        {
            string sql = "select mem_birth from Member where mem_id = '" + mID + "'";
            mBirth = Convert.ToDateTime(dbutil.ExecuteScalar(sql));
            return mBirth;
        }

        public string getmAddr(string mID)  //查找并返回会员地址
        {
            string sql = "select mem_address from Member where mem_id = '" + mID + "'";
            mAddr = Convert.ToString(dbutil.ExecuteScalar(sql));
            return mAddr;
        }

        public DateTime getmOpenTime(string mID)  //查找并返回会员开通时间
        {
            string sql = "select mem_open_time from Member where mem_id = '" + mID + "'";
            mOpenTime = Convert.ToDateTime(dbutil.ExecuteScalar(sql));
            return mOpenTime;
        }

        public DateTime getmExpireTime(string mID)  //查找并返回会员截止日期
        {
            string sql = "select mem_expire_time from Member where mem_id = '" + mID + "'";
            mExpireTime = Convert.ToDateTime(dbutil.ExecuteScalar(sql));
            return mExpireTime;
        }

        public string getmLevel(string mID)  //查找并返回会员等级
        {
            string sql = "select lev_title from Member where mem_id = '" + mID + "'";
            mLevel = Convert.ToString(dbutil.ExecuteScalar(sql));
            return mLevel;
        }

        public string getmDiscount(string mID)  //查找并返回会员折扣
        {
            string sql = "select lev_discount from Member natural join MemberLevel where mem_id = '" + mID + "'";
            return Convert.ToString(dbutil.ExecuteScalar(sql));
        }

        public string getmFare(string mID)  //查找并返回会员月费
        {
            string sql = "select lev_fare from Member natural join MemberLevel where mem_id = '" + mID + "'";
            return Convert.ToString(dbutil.ExecuteScalar(sql));
        }


        /*********************************
         * 以下是会员更新相关函数
         *********************************/
        public void updatemName(string mID, string mName)  //更新会员姓名
        {
            string sql = "update Member set mem_name = '" + mName + "' where mem_id = '" + mID + "'";
            int result = dbutil.ExecNonQuery(sql);
        }

        public void updatemTel(string mID, string mTel)  //更新会员电话
        {
            string sql = "update Member set mem_tele = '" + mTel + "' where mem_id = '" + mID + "'";
            int result = dbutil.ExecNonQuery(sql);
        }

        public void updatemGender(string mID, string mGender)  //更新会员性别
        {
            string sql = "update Member set mem_gender = '" + mGender + "' where mem_id = '" + mID + "'";
            int result = dbutil.ExecNonQuery(sql);
        }

        public void updatemBirth(string mID, string mBirth)  //更新会员生日
        {
            string sql = "update Member set mem_birth = " + "to_date('" + mBirth + "','yyyy-mm-dd,hh24:mi:ss')" + " where mem_id = '" + mID + "'";
            int result = dbutil.ExecNonQuery(sql);
        }

        public void updatemAddr(string mID, string mAddr)  //更新会员地址
        {
            string sql = "update Member set mem_address = '" + mAddr + "' where mem_id = '" + mID + "'";
            int result = dbutil.ExecNonQuery(sql);
        }

        public void updatemLevel(string mID, string mLevel)  //更新会员等级
        {
            string sql = "update Member set lev_title = '" + mLevel + "' where mem_id = '" + mID + "'";
            int result = dbutil.ExecNonQuery(sql);
        }

        public void updatemExpireTime(string mID, string mExpireTime)  //更新会员截止日期
        {
            string sql = "update Member set mem_expire_time = " + "to_date('" + mExpireTime + "','yyyy-mm-dd,hh24:mi:ss')" + " where mem_id = '" + mID + "'";
            int result = dbutil.ExecNonQuery(sql);
        }
        /********************
         * 会员到期相关函数
         ********************/
        public bool judgeDue()  //更新会员截止日期
        {
            System.DateTime nowtime = new System.DateTime();
            nowtime = System.DateTime.Now;
            string sql = "select mem_expire_time from Member where mem_expire_time <= " + "to_date('" + nowtime + "','yyyy-mm-dd,hh24:mi:ss')";
            int result = dbutil.ExecNonQuery(sql);
            if (result > 0)
                return true;
            else return false;
        }
    }
}
