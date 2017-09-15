using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace BookStore
{
    class StaffManagement
    {
        ///功能：查询员工信息
        ///输入：1.员工id,用于存放员工信息的Staff对象；2.通过Staff sta=new Staff()创建；3.数据库连接ControlAccess对象
        ///输出：bool,查询到返回true,未查询到返回false
        public bool InquireStaffInfo(string staId, Staff sta, ControlAccess con)
        {
            if (InquireStaffInfo(staId, con) == false)
                return false;
            else
            {
                string sqlcommand = "select STA_ID,STA_NAME,STA_GENDER,STA_ADDRESS," +
                    "STA_BIRTH,STA_TELE,STA_SIGN_DATE,PO_TITLE,STA_PASSWORD,STA_ON_JOB from STAFF where STA_ID="
                    + "'" + staId + "'";
                DataSet dataset = con.GetDataSet(sqlcommand);
                sta.Sta_id = Convert.ToString(dataset.Tables[0].Rows[0][0]);
                sta.Sta_name = Convert.ToString(dataset.Tables[0].Rows[0][1]);
                sta.Sta_gender = Convert.ToString(dataset.Tables[0].Rows[0][2]);
                sta.Sta_address = Convert.ToString(dataset.Tables[0].Rows[0][3]);
                sta.Sta_birth = Convert.ToString(dataset.Tables[0].Rows[0][4]);
                sta.Sta_tele = Convert.ToString(dataset.Tables[0].Rows[0][5]);
                sta.Sta_sign_date = Convert.ToString(dataset.Tables[0].Rows[0][6]);
                sta.Po_title = Convert.ToString(dataset.Tables[0].Rows[0][7]);
                sta.Sta_password = Convert.ToString(dataset.Tables[0].Rows[0][8]);
                sta.Sta_on_job = Convert.ToString(dataset.Tables[0].Rows[0][9]);

                return true;
            }
        }


        ///内部调用函数
        public bool InquireStaffInfo(string staId, ControlAccess con)
        {
            string sqlcommand = "select count(*) from STAFF where STA_ID=" + "'" + staId + "'";
            DataSet dateset = con.GetDataSet(sqlcommand);
            int iCount = Convert.ToInt32(dateset.Tables[0].Rows[0][0]);
            if (iCount > 0) return true;
            else return false;
        }

        ///功能：删除员工
        ///输入：员工id,数据库连接ControlAccess对象
        ///输出：bool,删除成功返回true,否则返回false
        public bool DeleteStaff(string staId, ControlAccess conn)
        {
            if (InquireStaffInfo(staId, conn))//数据库中能查询到此员工
            {
                string sqlcommand = "update STAFF set STA_ON_JOB='NO' where STA_ID='" + staId + "'";
                int ires = conn.ExecNonQuery(sqlcommand);
                //Console.WriteLine("delete successfully!");
                return true;
            }
            return false;
        }


        /// <summary>
        /// 更新员工信息
        /// </summary>
        /// <param name="staId"></param>员工id
        /// <param name="staName"></param>员工姓名
        /// <param name="staGender"></param>员工性别
        /// <param name="staAddress"></param>员工住址
        /// <param name="staBirth"></param>员工生日
        /// <param name="staTele"></param>员工电话
        /// <param name="staSignDate"></param>员工入职日期
        /// <param name="poTitle"></param>员工职位
        /// <param name="staPassword"></param>员工密码
        /// <param name="conn"></param>数据库连接ControlAccess对象
        /// <returns></returns>更新是否成功,若成功返回true,否则返回false
        public bool UpdateStaffInfo(string staId, string staName, string staGender, string staAddress,
            string staBirth, string staTele, string staSignDate, string poTitle, string state,
            ControlAccess conn)
        {
            if (InquireStaffInfo(staId, conn))
            {
                string sqlCommand = "update STAFF set STA_NAME=" + "'" + staName + "'" + "where STA_ID=" +
                    "'" + staId + "'";
                int ires = conn.ExecNonQuery(sqlCommand);
                sqlCommand = "update STAFF set STA_GENDER=" + "'" + staGender + "'" + "where STA_ID=" +
                   "'" + staId + "'";
                ires = conn.ExecNonQuery(sqlCommand);
                sqlCommand = "update STAFF set STA_ADDRESS=" + "'" + staAddress + "'" + "where STA_ID=" +
                   "'" + staId + "'";
                ires = conn.ExecNonQuery(sqlCommand);
                sqlCommand = "update STAFF set STA_BIRTH=" + "to_date('" + staBirth + "','yyyy-mm-dd') "
                + "where STA_ID=" +
                   "'" + staId + "'";
                ires = conn.ExecNonQuery(sqlCommand);
                sqlCommand = "update STAFF set STA_TELE=" + "'" + staTele + "'" + "where STA_ID=" +
                   "'" + staId + "'";
                ires = conn.ExecNonQuery(sqlCommand);
                sqlCommand = "update STAFF set STA_SIGN_DATE=" + "to_date('" + staSignDate + "','yyyy-mm-dd') " + "where STA_ID=" +
                   "'" + staId + "'";
                ires = conn.ExecNonQuery(sqlCommand);
                sqlCommand = "update STAFF set PO_TITLE=" + "'" + poTitle + "'" + "where STA_ID=" +
                   "'" + staId + "'";
                ires = conn.ExecNonQuery(sqlCommand);
                sqlCommand = "update STAFF set STA_ON_JOB=" + "'" +state + "'" + "where STA_ID=" +
                   "'" + staId + "'";
                ires = conn.ExecNonQuery(sqlCommand);
                Console.WriteLine("update successfully!");

                return true;
            }
            return false;
        }

        /// <summary>
        /// 创建出勤记录
        /// </summary>
        /// <param name="staId"></param>员工id
        /// <param name="arDate"></param>出勤日期
        /// <param name="isLate"></param>是否迟到
        /// <param name="isLevEarly"></param>是否早退
        /// <param name="isAbsent"></param>是否缺勤
        /// <param name="conn"></param>数据库连接ControlAccess对象
        /// <returns></returns>若操作成功返回true,否则返回false
        public bool CreateAttendInfo(string staId, string arDate, char isLate, char isLevEarly, char isAbsent, ControlAccess conn)
        {
            if (InquireAttendance(staId, arDate, conn) == false)
            {
                string sqlcommand = "insert into ATTENDANCERECORD values('" +
                    staId + "'," + "to_date('" + arDate + "','yyyy-mm-dd'), " + "'" + isLate + "','" + isLevEarly + "','" + isAbsent + "')";
                int ires = conn.ExecNonQuery(sqlcommand);
                return true;
            }
            return false;
        }


        /// <summary>
        /// 查询出勤信息
        /// </summary>
        /// <param name="staId"></param>员工id
        /// <param name="arDate"></param>员工出勤日期
        /// <param name="isLate"></param>是否迟到
        /// <param name="isLevEarly"></param>是否早退
        /// <param name="isAbsent"></param>是否缺勤
        /// <param name="iNumOfData"></param>数据条数
        /// <param name="con"></param>数据库连接ControlAccess对象
        /// <returns></returns>若查询到返回true,否则返回false
        public DataSet InquireAttendance2(string staId, string arDate,  ControlAccess con)
        {
            if (InquireAttendance(staId, arDate, con) == true)
            {
                string sqlcommand = "select STA_ID,AR_DATE,IS_LATE,IS_LEV_EARLY,IS_ABSENT" +
                   " from ATTENDANCERECORD where STA_ID="
                   + "'" + staId + "' and AR_DATE=" + "to_date('" + arDate + "','yyyy-mm-dd') ";
                DataSet dataset = con.GetDataSet(sqlcommand);
                //ar.StaId1 = Convert.ToString(dataset.Tables[0].Rows[0][0]);
                //ar.ArDate1 = Convert.ToString(dataset.Tables[0].Rows[0][1]);
                //ar.IsLate1 = Convert.ToChar(dataset.Tables[0].Rows[0][2]);
                //ar.IsLevEarly1 = Convert.ToChar(dataset.Tables[0].Rows[0][3]);
                //ar.IsAbsent1 = Convert.ToChar(dataset.Tables[0].Rows[0][4]);
                return dataset;
            }
            return null;

        }


        /// 内部接口
        public bool InquireAttendance(string staId, string arDate, ControlAccess con)
        {
            string sqlcommand = "select count(*) from ATTENDANCERECORD where STA_ID=" + "'" + staId + "' and AR_DATE=" +
                "to_date('" + arDate + "','yyyy-mm-dd') ";
            DataSet dateset = con.GetDataSet(sqlcommand);
            int iCount = Convert.ToInt32(dateset.Tables[0].Rows[0][0]);
            if (iCount > 0) return true;
            else return false;
        }

        public bool CreateLeaveInfo(string staId, string lrStartTime, string lrEndTime,
            int lrDays,ControlAccess conn)
        {
            if (InquireLeave(staId, lrStartTime, conn) == false)
            {
                string sqlcommand = "insert into LEAVERECORD  values(" +
                  "to_date('" + lrStartTime + "','yyyy-mm-dd'), " + "'" + staId + "'," + "to_date('" + lrEndTime + "','yyyy-mm-dd'), " + "'" + lrDays + "')";
                int ires = conn.ExecNonQuery(sqlcommand);
                //sqlcommand = "select extract(year from" + "to_date('" + lrStartTime + "','yyyy-mm-dd')"+")";
                //DataSet dataset = conn.GetDataSet(sqlcommand);

                return true;
            }
            return false;
        }

        /// <summary>
        /// 查询请假信息
        /// </summary>
        /// <param name="staId"></param>
        /// <param name="lrStartTime"></param>
        /// <param name="lrEndTime"></param>
        /// <param name="lrDays"></param>
        /// <param name="iNumOfData"></param>
        /// <returns></returns>
        public DataSet InquireLeave2(string staId, ControlAccess con )
        {
            if (InquireLeave(staId,con) == true)
            {
                string sqlcommand = "select LR_START_TIME,STA_ID,LR_END_TIME,LR_DAYS" +
                   " from LEAVERECORD where STA_ID="+ "'" + staId + "'";
                DataSet dataset = con.GetDataSet(sqlcommand);
               
                return dataset;
            }
            return null;
        }

        ///内部接口
        public bool InquireLeave(string staId, string lrStartTime,ControlAccess con)
        {
            string sqlcommand = "select count(*) from LEAVERECORD where STA_ID=" + "'" + staId + "' and LR_START_TIME=" +
                "to_date('" + lrStartTime + "','yyyy-mm-dd') ";
            DataSet dateset = con.GetDataSet(sqlcommand);
            int iCount = Convert.ToInt32(dateset.Tables[0].Rows[0][0]);
            if (iCount > 0) return true;
            else return false;
        }

        public bool InquireLeave(string staId,ControlAccess con)
        {
            string sqlcommand = "select count(*) from LEAVERECORD where STA_ID=" + "'" + staId  + "'";
            DataSet dateset = con.GetDataSet(sqlcommand);
            int iCount = Convert.ToInt32(dateset.Tables[0].Rows[0][0]);
            Console.WriteLine(iCount);
            if (iCount > 0) return true;
            else return false;
        }
        public bool InquireMonthStatistics(string staId, ControlAccess con)
        {
            string sqlcommand = "select count(*) from MONTHSTATISTICS where STA_ID=" + "'" + staId + "'";
            DataSet dateset = con.GetDataSet(sqlcommand);
            int iCount = Convert.ToInt32(dateset.Tables[0].Rows[0][0]);
            Console.WriteLine(iCount);
            if (iCount > 0) return true;
            else return false;
        }
        
        public DataSet InquireMonthStatistics2(string staId, ControlAccess con)
        {
            if (InquireMonthStatistics(staId, con) == true)
            {
                string sqlcommand = "select STA_ID,MS_YEAR,MS_MONTH,MS_ATTEND,MS_ABSENT,MS_ABSENT,MS_LEA_EARLY,MS_LATE,MS_SALARY" +
                   " from MONTHSTATISTICS where STA_ID=" + "'" + staId + "'";
                DataSet dataset = con.GetDataSet(sqlcommand);
                //拉到dataGrid1中
                return dataset;
            }
            return null;
        }
    }
}
