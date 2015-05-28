using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace arinars.common
{
    public class RegexUtil
    {
		public const string OnlyNumber = @"^(\d)+$";
		public const string Money = @"^(\d|-)?(\d|,)*\.?\d*$";
		public const string Email = @"^[\w-\._\+%]+@(?:[\w-]+\.)+[\w]{2,6}$";
        public const string PersonalNumber = @"^\(?[\d\s]{6}-[\d\s]{7}$";
		public const string CompNumber = @"^\d{3}\-\d{2}\-\d{5}$";
        public const string ZipCode = @"^\(?[\d\s]{3}-[\d\s]{3}$";
        public const string Decimal = @"(?!^0*$)(?!^0*\.0*$)^\d{1,18}(\.\d{1,2})?$";
		public const string Pwd = @"^.*(?=.{12,40})(?=.*[\d])(?=.*[a-z]).*$";	//12~40Byte 사이 길이, 하나 이상의 숫자와 하나 이상의 소문자 조합

		public const string Mobile = @"^((01[16789])\-[1-9]\d{2,3}\-\d{4}|010\-[1-9]\d{3}\-\d{4})$";	// 국내 무선 전화
		public const string Tel = @"^0\d{1,2}-\d{3,4}-\d{4}$";	// 국내 유선 전화
		public const string NationTel = @"^\d{1,3}-\d{1,3}-\d{3,4}-\d{4}$";	// 해외 전화
		public const string Phone = @"^([1-9]\d{0,4}-\d{1,3}-\d{3,4}-\d{4}|0\d{1,2}-\d{3,4}-\d{4})$";	// 국내 유선 + 해외 전화(유선,무선)

        public const string Month = @"^19\d{2}-([0][1-9]|[1][0-2])|20\d{2}-([0][1-9]|[1][0-2])$";
    }
}
