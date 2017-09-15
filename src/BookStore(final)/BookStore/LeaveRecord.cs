using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore
{
    public class LeaveRecord
    {
        private string LrStartTime;
        private string StaId;
        private string LrEndTime;
        private int LrDays;

        public string LrStartTime1
        {
            get
            {
                return LrStartTime;
            }

            set
            {
                LrStartTime = value;
            }
        }

        public string StaId1
        {
            get
            {
                return StaId;
            }

            set
            {
                StaId = value;
            }
        }

        public string LrEndTime1
        {
            get
            {
                return LrEndTime;
            }

            set
            {
                LrEndTime = value;
            }
        }

        public int LrDays1
        {
            get
            {
                return LrDays;
            }

            set
            {
                LrDays = value;
            }
        }
    }
}
