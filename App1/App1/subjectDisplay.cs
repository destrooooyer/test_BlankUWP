using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    class subjectDisplay
    {
        List<string> name;
        List<string> teacher;
        List<string> location;
        List<int> weekBegin;
        List<int> weekEnd;
        List<int> isDanShuangZhou;

        public List<string> Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public List<string> Teacher
        {
            get
            {
                return teacher;
            }

            set
            {
                teacher = value;
            }
        }

        public List<string> Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
            }
        }

        public List<int> WeekBegin
        {
            get
            {
                return weekBegin;
            }

            set
            {
                weekBegin = value;
            }
        }

        public List<int> WeekEnd
        {
            get
            {
                return weekEnd;
            }

            set
            {
                weekEnd = value;
            }
        }

        public List<int> IsDanShuangZhou
        {
            get
            {
                return isDanShuangZhou;
            }

            set
            {
                isDanShuangZhou = value;
            }
        }

        public subjectDisplay()
        {
            name = new List<string>();
            teacher = new List<string>();
            location = new List<string>();
            weekBegin = new List<int>();
            weekEnd = new List<int>();
            isDanShuangZhou = new List<int>();
        }

        public void pushBack(string name,string teacher,string location,int weekBegin,int weekEnd,int isDanShuangZhou)
        {
            this.name.Add(name);
            this.teacher.Add(teacher);
            this.location.Add(location);
            this.weekBegin.Add(weekBegin);
            this.WeekEnd.Add(weekEnd);
            this.isDanShuangZhou.Add(isDanShuangZhou);
        }

    }
}
