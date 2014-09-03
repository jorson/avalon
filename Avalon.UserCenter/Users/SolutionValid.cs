using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Avalon.UserCenter.Models;

namespace Avalon.UserCenter.Users
{
    public class SolutionValid
    {
        public static readonly Regex CodeRegex = new Regex(SolutionRegexString.Code);

        public static UserCode ValidCodeFormat(string solutionCode)
        {
            if (!solutionCode.IsNullOrWhiteSpace())
            {
                if (CodeRegex.IsMatch(solutionCode))
                    return UserCode.Success;
                return UserCode.InvalidSolutionCode;
            }

            return UserCode.EmptySolutionCode;
        }

        public static UserCode ValidSystemAbbreviationFormat(string systemAbbreviation)
        {
            if (!systemAbbreviation.IsNullOrWhiteSpace())
            {
                if (systemAbbreviation.Length > 10)
                    return UserCode.InvalidSystemAbbreviation;
            }
            return UserCode.Success;
        }
    }
}
