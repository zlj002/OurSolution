using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace OurHelper
{
    public class DayTools
    {
        /// <summary>
        /// 获取一年有多少周
        /// </summary>
        /// <param name="strYear"></param>
        /// <returns></returns>
        public static int GetYearWeekCount(int strYear)
        {
            System.DateTime fDt = DateTime.Parse(strYear.ToString() + "-01-01");
            int k = Convert.ToInt32(fDt.DayOfWeek);//得到该年的第一天是周几 
            if (k == 1)
            {
                int countDay = fDt.AddYears(1).AddDays(-1).DayOfYear;
                int countWeek = countDay / 7 + 1;
                return countWeek;

            }
            else
            {
                int countDay = fDt.AddYears(1).AddDays(-1).DayOfYear;
                int countWeek = countDay / 7 + 2;
                return countWeek;
            }
        }


        /// <summary>
        /// 求当前日期是一年的中第几周
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int WeekOfYear(DateTime curDay)
        {
            int firstdayofweek = Convert.ToInt32(Convert.ToDateTime(curDay.Year.ToString() + "- " + "1-1 ").DayOfWeek);

            int days = curDay.DayOfYear;
            int daysOutOneWeek = days - (7 - firstdayofweek);

            if (daysOutOneWeek <= 0)
            {
                return 1;
            }
            else
            {
                int weeks = daysOutOneWeek / 7;
                if (daysOutOneWeek % 7 != 0)
                    weeks++;

                return weeks + 1;
            }
        }
        //根据开学日期、第几周、第几天计算具体日期
        public static string GetDateByDayAndWeekIndexAndDayIndex(DateTime date, int weekIndex, int dayIndex)
        {
            int dayDiff = (int)date.DayOfWeek - dayIndex;
            return date.AddDays((weekIndex - 1) * 7 - dayDiff).ToString("yyyy-MM-dd");
        }

        //根据开始时间、结束时间计算一共有多少周
        public static int GetWeeksByStartDayAndEndDay(DateTime startWeek, DateTime endWeek)
        {
            TimeSpan ts = endWeek - startWeek;
            double result = ts.Days / 7.0;
            return (int)Math.Ceiling(result);
        }

        public static int GetDiffDaysBetweenSDateTimeAndEDateTime(DateTime startDateTime, DateTime endDateTime)
        {
            TimeSpan ts = DateTime.Parse(endDateTime.ToString("yyyy-MM-dd")) - DateTime.Parse(startDateTime.ToString("yyyy-MM-dd"));
            return ts.Days;
        }
        /// <summary>
        /// 根据时间获取星期几
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string GetChineseDayOfWeekByDatetime(DateTime datetime)
        {
            string[] day = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            return day[(int)datetime.DayOfWeek];
        }

        public static int FormatDayOfWeekByDateTime(DateTime datetime)
        {
            int[] day = { 7, 1, 2, 3, 4, 5, 6 };
            return day[(int)datetime.DayOfWeek];
        }

        /// <summary>
        /// 以一个日期为标准，计算起星期X的日期
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="weekday"></param>
        /// <param name="Number"></param>
        /// <returns></returns>
        public static DateTime GetWeekUpOfDate(DateTime dt, DayOfWeek weekday, int Number)
        {
            int wd1 = (int)weekday;
            int wd2 = (int)dt.DayOfWeek;
            return wd2 == wd1 ? dt.AddDays(7 * Number) : dt.AddDays(7 * Number - wd2 + wd1);
        }

        /// <summary>
        /// 根据开始日期和当前日期计算当前为第几周
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="mydate"></param>
        /// <returns></returns>
        public static int GetWeekOrder(DateTime startDate, DateTime mydate)
        {

            TimeSpan vTimeSpan = new TimeSpan(mydate.Ticks - startDate.Ticks);
            int totalDays = vTimeSpan.Days;
            //int i = 1;
            int result = 0;
            for (int i = 1; i <= totalDays; i++)
            {
                if (startDate.AddDays(i).DayOfWeek == DayOfWeek.Saturday)
                {
                    result = result + 1;
                }
            }
            if (mydate.DayOfWeek != DayOfWeek.Saturday)
            {
                result = result + 1;
            }
            return result;
        }
    }
}
