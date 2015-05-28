using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace arinars.common
{
    public sealed class StringUtil
    {
        /// <summary>
		///  HTML을 제거하는 정규표현식
		///   - 정규식을 통해 해당 String 에서 HTML 을 제거한다.
		///   - aHtml : 전체 문자열
		///   - rpText : 바꿀 문자열
		/// </summary>
		/// <returns></returns>
		public static string ReplaceHtmlString(string aHtml, string rpText)
		{
			if(string.IsNullOrEmpty(aHtml))
			{
				return string.Empty;
			}
			else
			{
				//HTML을 제거하는 정규표현식
				Regex regHTML = new Regex("<(/)?([a-zA-Z]*)(\\s[a-zA-Z]*=[^>]*)?(\\s)*(/)?>");

				return regHTML.Replace(aHtml, rpText);
			}
		}

		/// <summary>
		///  줄을 바꾸는 정규표현식
		///   - &lt;br /&gt;태그를 줄바꿈 문자(\n)로 바꿀 때 사용
		///   - aHtml : 전체 문자열
		///   - rpText : 바꿀 문자열
		/// </summary>
		/// <returns></returns>
		public static string BrToText(string aHtml, string rpText)
		{
			if(string.IsNullOrEmpty(aHtml))
			{
				return string.Empty;
			}
			else
			{
				Regex regBR = new Regex("<[bB][rR](\\s)?(/)?>", RegexOptions.IgnoreCase);

				return regBR.Replace(aHtml, rpText);
			}
		}

		/// <summary>
		///  줄을 바꾸는 정규표현식
		///   - 줄바꿈 문자(\n)를 &lt;br /&gt;태그로 바꿀 때 사용
		///   - aHtml : 전체 문자열
		/// </summary>
		/// <returns></returns>
		public static string NlToBr(string aHtml)
		{
			if(string.IsNullOrEmpty(aHtml)) {
				return string.Empty;
			} else {
				return aHtml.Replace(Environment.NewLine, "<br />");
			}
		}

		public static string CutTitle(string aText, int aByteCount)
		{
			return CutTitle(aText, aByteCount, false);
		}

		public static string CutTitle(string aText, int aByteCount, bool aIsContainChar)
		{
			if (aByteCount < 1) return aText;
            if (string.IsNullOrEmpty(aText)) return aText;

			int lLength = 0;
			lLength = aIsContainChar == true ? aByteCount - 3 : aByteCount;
			System.Text.Encoding myEncoding = System.Text.Encoding.GetEncoding("ks_c_5601-1987");

			if (myEncoding.GetByteCount(aText) <= aByteCount)
			{
				return aText;
			}

			for (int i = aText.Length; i >= 0; i--)
			{
				aText = aText.Substring(0, i);
				if (myEncoding.GetByteCount(aText) <= lLength)
				{
					return aIsContainChar == true ? aText + "…" : aText;
				}
			}
			return "";
		}

		/// <summary>
		///  문자열의 HTML을 제거하고 Text 문자열만 길이를 측정하여 자르고 다시 HTML 처리한다.
		///   1. 정규식을 통해 해당 String 에서 HTML 을 제거한다.
		///   2. 남은 문자열을 aByteCount 만큼 잘라낸다
		///   3. 전체 문자열에서 1번에 나온 문자열을 2번에 나온 문자열로 Replace 처리한다.
		///   4. aIsContainChar 값에 따라 "..."를 뒤에 붙이거나 없앤다.
		/// </summary>
		/// <returns></returns>
		public static string CutHtml(string aHtml, int aByteCount, bool aIsContainChar)
		{
            // Html 제거 안되는 경우가 발생.
            string aText = ReplaceHtmlString(aHtml, "");
            
           

			if (aByteCount < 1) return aHtml;

			int lLength = 0;
			lLength = aIsContainChar == true ? aByteCount - 3 : aByteCount;
			System.Text.Encoding myEncoding = System.Text.Encoding.GetEncoding("ks_c_5601-1987");

			if (myEncoding.GetByteCount(aText) <= aByteCount)
			{
				return aHtml;
			}

			for (int i = aText.Length; i >= 0; i--)
			{
				string aCutText = aText.Substring(0, i);
				if (myEncoding.GetByteCount(aCutText) <= lLength)
				{
					return aIsContainChar == true ? aHtml.Replace(aText, aCutText) + "…" : aHtml.Replace(aText, aCutText);
				}
			}
			return "";
		}

        /// <summary>
        ///  문자열의 HTML을 제거하고 Text 문자열로 변환하여 길이를 측정하여 자른다.
        ///   1. 정규식을 통해 해당 String 에서 HTML 을 제거한다.
        ///   2. 남은 문자열을 aByteCount 만큼 잘라낸다
        ///   3. aIsContainChar 값에 따라 "..."를 뒤에 붙이거나 없앤다.
        /// </summary>
        /// <returns></returns>
        public static string StripHtml(string aHtml, int aByteCount, bool aIsContainChar)
        {
            string lHtml = StripHtml(aHtml);

            if (aByteCount < 1) return lHtml;
            if (string.IsNullOrEmpty(lHtml)) return lHtml;

            int lLength = 0;
            lLength = aIsContainChar == true ? aByteCount - 3 : aByteCount;
            System.Text.Encoding myEncoding = System.Text.Encoding.GetEncoding("ks_c_5601-1987");

            if (myEncoding.GetByteCount(lHtml) <= aByteCount)
            {
                return lHtml;
            }

            for (int i = lHtml.Length; i >= 0; i--)
            {
                lHtml = lHtml.Substring(0, i);
                if (myEncoding.GetByteCount(lHtml) <= lLength)
                {
                    return aIsContainChar == true ? lHtml + "…" : lHtml;
                }
            }
            return "";
        }

        //HTML 에서 Text만 추출한다.
        public static string StripHtml(string aHtml)
        {
            string output;
            //get rid of HTML tags
            output = System.Text.RegularExpressions.Regex.Replace(aHtml, "<[^>]*>", string.Empty);
            //get rid of multiple blank lines
            output = System.Text.RegularExpressions.Regex.Replace(output, @"^\s*$\n", string.Empty, System.Text.RegularExpressions.RegexOptions.Multiline);
            return output;
        }

        public static int? ToNullableInt32(string s, int? defVal)
        {
            int i;
            return int.TryParse(s, out i) ? (int?)i : defVal;
        }

        public static int ToInt32(string s, int defVal)
        {
            if (IsInt32(s))
            {
                return int.Parse(s);
            }
            else
            {
                return defVal;
            }
        }

        /// <summary>
        /// 날짜를 통한 이름 생성
        /// </summary>
        /// <returns></returns>
        public static string GetNewDateName()
        {
            string name = "";
            DateTime dt;
            dt = DateTime.Now;
            name = dt.Year.ToString() + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0') + dt.Hour.ToString().PadLeft(2, '0') + dt.Minute.ToString().PadLeft(2, '0') + dt.Second.ToString().PadLeft(2, '0') + dt.Millisecond.ToString() + RandomStringNumber(4);
            return name;
        }


        private const string _numbers = "0123456789";
        /// <summary>
        /// 일정길이의 숫자로 이루어진 랜덤 문자열을 생성한다.
        /// </summary>
        /// <param name="aSize">생성할 숫자로 이루어진 랜덤 문자열의 길이</param>
        /// <returns></returns>
        public static string RandomStringNumber(int aSize)
        {
            // Use a 4-byte array to fill it with random bytes
            byte[] lRandomBytes = new byte[4];

            // Generate 4 random bytes.
            RNGCryptoServiceProvider lRng = new RNGCryptoServiceProvider();
            lRng.GetBytes(lRandomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int lSeed = (lRandomBytes[0] & 0x7f) << 24 |
                         lRandomBytes[1]         << 16 |
                         lRandomBytes[2]         <<  8 |
                         lRandomBytes[3];

            Random lRandom = new Random(lSeed);

            char[] lPassword = new char[aSize];

            for (int i = 0; i < aSize; i++)
            {
                lPassword[i] = _numbers[lRandom.Next(_numbers.Length)];
            }
            return new string(lPassword);
        }
        

        /// <summary>
        /// 문자열이 Int32 형이 맞는지 확인
        /// </summary>
        /// <param name="s">확인할 문자열</param>
        /// <returns></returns>
        public static bool IsInt32(string s)
        {
            try
            {
                int i = Int32.Parse(s);
                return IsInt32InScope(i);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Int32형이 가질수 있는 값의 범위에 속하는지 확인
        /// </summary>
        /// <param name="num">확인할 숫자</param>
        /// <returns></returns>
        public static bool IsInt32InScope(int num)
        {
            return (num >= 0 && num <= Int32.MaxValue) ? true : false;
        }

		/// <summary>
		/// IP 정규식 검사
		/// </summary>
		public static bool CheckIPFormat(string pIp)
		{
			Regex regex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
			return regex.IsMatch(pIp);
		}

		/// <summary>
		/// *를 포함한 IP 정규식 검사
		/// </summary>
		/// <remarks>
		/// e.g) 10.1.10.*<br/>
		/// e.g) 10.*.10.*
		/// </remarks>
		public static bool CheckIPFormatWithStar(string pIp)
		{
			Regex regex = new Regex(@"^((\d{1,2}|1\d\d|2[0-4]\d|25[0-5])|\*)\.((\d{1,2}|1\d\d|2[0-4]\d|25[0-5])|\*)\.((\d{1,2}|1\d\d|2[0-4]\d|25[0-5])|\*)\.((\d{1,2}|1\d\d|2[0-4]\d|25[0-5])|\*)$");
			return regex.IsMatch(pIp);
		}

		/// <summary>
		/// 구분자를 기준으로 split 실행한다.
		/// </summary>
		/// <param name="aStr">문자열</param>
		/// <param name="aSep">구분자</param>
		/// <returns>aSep로 분리된 문자열 배열</returns>
		public static string[] Split(string aStr, string aSep, StringSplitOptions aOptions)
		{
            if (string.IsNullOrEmpty(aStr))
            {
                return Enumerable.Empty<string>().ToArray();
            }

            return aStr.Split(new string[] { aSep }, aOptions);
		}

        /// <summary>
        /// 구분자를 기준으로 join 후에 다시 split 실행한다.
        /// </summary>
		/// <param name="aStrArr">문자열배열</param>
		/// <param name="aSep">구분자</param>
        /// <returns>배열을 구분자로 합쳐 다시 구분자로 분리한 문자열 배열</returns>
        public static string[] Split(string[] aStrArr, string aSep, StringSplitOptions aOptions)
        {
            return Split(Join(aStrArr, aSep), aSep, aOptions);
        }

        public static string Join(string[] aStrArr, string aSep)
        {
            if (aStrArr == null)
            {
                return string.Empty;
            }

            return string.Join(aSep, aStrArr);
        }

		/// <summary>
		/// space를 구분으로 split 실행한다.
		/// </summary>
		/// <param name="orgStr">문자열</param>
		/// <returns>space가 제외된 문자열 배열</returns>
		public static string[] SplitWithSpace(string pResult)
		{
			return (new Regex(" +")).Split(pResult.Trim());
		}

		/// <summary>
		/// \n 기호를 구분으로 split 실행한다.
		/// </summary>
		/// <param name="orgStr">문자열</param>
		/// <returns>\n가 제외된 문자열 배열</returns>
		public static string[] SplitWithNewLine(string pResult)
		{
			return (new Regex("\n|\\n")).Split(pResult.Trim());
		}

        /// <summary>
        /// 전화번호의 지역번호을 가져온다.
        /// </summary>
        /// <param name="aStart"></param>
        /// <param name="aEnd"></param>
        /// <returns></returns>
        public static string TelLeft(string aSource, string aSeparator)
        {
            if (string.IsNullOrEmpty(aSource))
            {
                return string.Empty;
            }

            string[] aSources = aSource.Split(new string[] { aSeparator }, StringSplitOptions.None);
            if (aSources.Count() > 2)
            {
                return aSources[0];
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 전화번호의 국번을 가져온다.
        /// </summary>
        /// <param name="aStart"></param>
        /// <param name="aEnd"></param>
        /// <returns></returns>
        public static string TelMiddle(string aSource, string aSeparator)
        {
            if (string.IsNullOrEmpty(aSource))
            {
                return string.Empty;
            }

            string[] aSources = aSource.Split(new string[] { aSeparator }, StringSplitOptions.None);
            if (aSources.Count() > 2)
            {
                return aSources[1];
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 전화번호의 마지막 번호를 가져온다.
        /// </summary>
        /// <param name="aStart"></param>
        /// <param name="aEnd"></param>
        /// <returns></returns>
        public static string TelRight(string aSource, string aSeparator)
        {
            if (string.IsNullOrEmpty(aSource))
            {
                return string.Empty;
            }

            string[] aSources = aSource.Split(new string[] { aSeparator }, StringSplitOptions.None);
            if (aSources.Count() > 2)
            {
                return aSources[2];
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
