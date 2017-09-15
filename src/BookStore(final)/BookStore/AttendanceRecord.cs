using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore
{
    public class AttendanceRecord
    {
        private string StaId;
        private string ArDate;
        private char IsLate;
        private char IsLevEarly;
        private char IsAbsent;

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

        public string ArDate1
        {
            get
            {
                return ArDate;
            }

            set
            {
                ArDate = value;
            }
        }

        public char IsLate1
        {
            get
            {
                return IsLate;
            }

            set
            {
                IsLate = value;
            }
        }

        public char IsLevEarly1
        {
            get
            {
                return IsLevEarly;
            }

            set
            {
                IsLevEarly = value;
            }
        }

        public char IsAbsent1
        {
            get
            {
                return IsAbsent;
            }

            set
            {
                IsAbsent = value;
            }
        }
    }
}
