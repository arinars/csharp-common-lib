using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace arinars.common
{
    public class DateUtil
    {
        /// <summary>
        ///  정적으로 각 변수들을 선언하여 값을 강제 초기화 시킨다. 
        ///  이를 통해 정적 함수 사용시, 인스턴스 생성 없이 즉각 값 사용 가능 
        /// </summary>
        private System.Globalization.CultureInfo mCulture;              // 언어셋팅 -> 컨피그 or 글로벌 영역으로 빼야할듯.   
        private string mYear; 
        private string mMonth ;
        private string mDay;
        private string mHour;
        private string mMinute;
        private string mSecond;
        private string mHHmmss; // 현재 시간(HHss)
        private string mNameOfDay;    // 요일 이름 가져오기      


        public System.Globalization.CultureInfo getCulture()
        {
            return this.mCulture;
        }

        public string getYear()
        {
            return this.mYear = DateTime.Now.ToString("yyyy"); 
        }

        public string getMonth()
        {
            return this.mMonth = DateTime.Now.ToString("MM");
        }

        public string getDay()
        {
            return this.mDay = DateTime.Now.ToString("dd");
        }

        public string getHour()
        {
            return this.mHour = DateTime.Now.ToString("HH");
        }

        public string getMinute()
        {
            return this.mMinute = DateTime.Now.ToString("mm");
        }

        public string getSecond()
        {
            return this.mSecond = DateTime.Now.ToString("ss");
        }

        public string getHHmmss()
        {
            return this.mHHmmss = DateTime.Now.ToString("HH:mm:ss");
        }

        public string getNameOfDay()
        {
            return this.mNameOfDay = mCulture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);
        }


       


        /// <summary>
        /// 생성자 
        /// </summary>
        public DateUtil() { 
             this.mCulture = new System.Globalization.CultureInfo("ko-KR");              // 언어셋팅 -> 컨피그 or 글로벌 영역으로 빼야할듯.   
             this.mYear = DateTime.Now.ToString("yyyy");
             this.mMonth = DateTime.Now.ToString("MM");
             this.mDay = DateTime.Now.ToString("dd");
             this.mHour = DateTime.Now.ToString("HH");
             this.mMinute = DateTime.Now.ToString("mm");
             this.mSecond = DateTime.Now.ToString("ss");
             this.mHHmmss = DateTime.Now.ToString("HH:mm:ss"); // 현재 시간(HHss)
             this.mNameOfDay = mCulture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);    // 요일 이름 가져오기  
        }

        /// <summary>
        /// 유형에 따른 데이터 포맷 추출 
        /// </summary>
        /// <param name="aType"></param>
        /// <returns></returns>
 
        public string getDateTime(int? aType){
            switch (aType){
                case 1 :
                    return mYear + "년 " + mMonth + "월 " + mDay + "일 " + mNameOfDay + " " + mHHmmss;
                    break;

                default :
                    return mYear + "년 " + mMonth + "월 " + mDay + "일 " + mNameOfDay + " " + mHHmmss;
                    break;
            }
            return "적절한 날짜 표시 유형을 확인하지 못하였습니다.";
        }
       
    }




    public static class DatetimeExtention{


        /// <summary>
        /// 특정월의 첫번째일 가져오기
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public static DateTime First(this DateTime current)
        {
            DateTime first = current.AddDays(1 - current.Day);
            return first;
        }

        
        /// <summary>
        /// 마지막 일 가져오기
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public static DateTime LastDay(this DateTime current)
        {
            int daysInMonth = DateTime.DaysInMonth(current.Year, current.Month);

            DateTime last = current.First().AddDays(daysInMonth - 1);
            return last;
        }

    }
    

   
}

