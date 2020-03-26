﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Services.Helpers
{
    public class DateTimeHelper : IDateTimeHelper
    {
        public int timezone_shift; // Сдвиг GMT
        IConfiguration Configuration;

        //=======================================================================================================================================
        public DateTimeHelper(IConfiguration configuration)
        {
            this.Configuration = configuration;
            try { timezone_shift = Convert.ToInt32(this.Configuration.GetSection("MainSettings")["TimezoneShift"]); } catch (Exception) { }
        }

        //=======================================================================================================================================
        public int GetTimeZoneShift()
        {
            return timezone_shift;
        }

        //=======================================================================================================================================
        public long ConvertToUnixTimeMilliseconds(DateTime date_time) // Конвертация timestamp в unixtime
        {
            var date = date_time;
            DateTime origin = new DateTime(1970, 1, 1, timezone_shift, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date - origin;
            try
            {
                return Convert.ToInt64(Math.Round(Math.Floor(diff.TotalMilliseconds), 0));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //=======================================================================================================================================
        public int ConvertToUnixTimeSeconds(DateTime date_time) // Конвертация timestamp в unixtime
        {
            var date = date_time;
            DateTime origin = new DateTime(1970, 1, 1, timezone_shift, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date - origin;
            try
            {
                return Convert.ToInt32(Math.Round(Math.Floor(diff.TotalSeconds), 0));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //=======================================================================================================================================
        public int ConvertToUnixTimeSeconds(string date_time) // Конвертация строкового timestamp (01.11.2018 12:18:00) в unixtime
        {
            try
            {
                var date = DateTime.ParseExact(date_time, "dd.MM.yyyy HH:mm:ss", null); // Случай 09.11.2018 12:18:00
                DateTime origin = new DateTime(1970, 1, 1, timezone_shift, 0, 0, 0);
                TimeSpan diff = date - origin;
                return Convert.ToInt32(Math.Round(Math.Floor(diff.TotalSeconds), 0));
            }
            catch (Exception)
            {
                try
                {
                    var date = DateTime.ParseExact(date_time, "d.MM.yyyy HH:mm:ss", null); // Случай 9.11.2018 12:18:00
                    DateTime origin = new DateTime(1970, 1, 1, timezone_shift, 0, 0, 0);
                    TimeSpan diff = date - origin;
                    return Convert.ToInt32(Math.Round(Math.Floor(diff.TotalSeconds), 0));
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        //=======================================================================================================================================
        public long GetCurrentDayToUnixTimeSeconds() // Возвращает текущий день в unixtime
        {
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, timezone_shift, 0, 0, DateTimeKind.Utc);
            DateTime origin = new DateTime(1970, 1, 1, timezone_shift, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date - origin;
            try
            {
                return Convert.ToInt64(Math.Round(Math.Floor(diff.TotalSeconds), 0));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //=======================================================================================================================================
        public string DateToFolderName(DateTime dateTime) // Возвращает дату в пригодном для наименования папки формате
        {
            //var date_time = DateTime.UtcNow.AddHours(timezone_shift);
            var date_time = dateTime;
            string Fix(string s)
            {
                if (s.Length == 1) return "0" + s;
                else return s;
            }
            string dd = Fix(date_time.Day.ToString());
            string mm = Fix(date_time.Month.ToString());
            string yyyy = date_time.Year.ToString();
            return dd + "_" + mm + "_" + yyyy;
        }

        //=======================================================================================================================================
    }
}
