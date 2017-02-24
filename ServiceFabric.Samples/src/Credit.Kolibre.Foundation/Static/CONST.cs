// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : CONST.cs
// Created          : 2016-12-09  7:18 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************


#pragma warning disable 1591

namespace Credit.Kolibre.Foundation.Static
{
    public static class CONST
    {
        public const string BASE36_CHARACTERS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const long EPOCH_MILLISECONDS = 62135596800000;
        public const long EPOCH_SECONDS = 62135596800;
        public const long EPOCH_TICKS = 621355968000000000;
        public const long FILE_TIME_OFFSET = 504911232000000000;
        public const int LOWER_CASE_OFFSET = 'a' - 'A';
        public const long MILLISECOND_TICKS = 10000;

        // regex string
        public const string BANK_CARD_GEGEX_STRING = @"^\d{14,19}$";
        public const string CELLPHONE_REGEX_STRING = @"^(13|14|15|17|18)\d{9}$";
        public const string COMPLEX_PASSWORD_REGEX_STRING = @"^(?![^A-Z~!@#$%^&*_]+$)(?![^a-z]+$)(?!\D+$).{8,18}$";
        public const string DATETIME_STRING_REGEX_STRING = @"^\d{4}-[01]\d-[0123]\d\s{1,2}[012]\d:[0-6]\d:[0-6]\d$";
        public const string DATETIME_YEAR_MONTH_DAY_STRING_REGEX_STRING = @"^[0-9]{4}-(((0[13578]|(10|12))-(0[1-9]|[1-2][0-9]|3[0-1]))|(02-(0[1-9]|[1-2][0-9]))|((0[469]|11)-(0[1-9]|[1-2][0-9]|30)))$";
        public const string EMAIL_REGEX_STRING = "^((([A-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+(\\.([A-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+)*)|((\\x22)((((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(([\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x7f]|\\x21|[\\x23-\\x5b]|[\\x5d-\\x7e]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(\\\\([\\x01-\\x09\\x0b\\x0c\\x0d-\\x7f]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF]))))*(((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(\\x22)))@((([A-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([A-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([A-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([A-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.)+(([A-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([A-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([A-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([A-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.?$";
        public const string ID_CARD_REGEX_STRING = @"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}([0-9]|X|x)$";
        public const string IP_ADDRESS_REGEX_STRING = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
        public const string NUMERIC_PASSWORD_REGEX_STRING = @"^\d{6}$";
        public const string SIMPLE_PASSWORD_REGEX_STRING = @"^[a-zA-Z\d~!@#$%^&*_]{6,18}$";
        public const string URL_REGEX_STRING = @"^(https?|ftp)://[^\s/$.?#].[^\s]*$";
        public const string CMB_CAPTCHA_REGEX_STRING = @"^\w{4,10}$";
        public const string CMB_PASSWORD_REGEX_STRING = @"^[a-zA-Z\d~!@#$%^&*_]{6,50}$";
        public const string NUMERIC_CAPTCHA_REGEX_STRING = @"^\d{6}$";
        public const string POSITIVE_NUMBER_REGEX_STRING = @"^[1-9]\d{0,}$";
        public const string POSITIVE_NUMBER_WITH_ZERO_REGEX_STRING = @"^([1-9]\d{0,}|0)$";
    }
}