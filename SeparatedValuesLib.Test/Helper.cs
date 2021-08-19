using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SeparatedValuesLib.Test {

    static public class Helper {

        static public string GetCellValue(this IEnumerable<IEnumerable<string>> parsedLines, int lineIndex, int columnIndex) {
            var line = parsedLines.Skip(lineIndex).Take(1).FirstOrDefault();
            if (!(line is null)) {
                var cellValue = line.Skip(columnIndex).Take(1).FirstOrDefault();
                return cellValue;
            }
            return null;
        }

        static public IEnumerable<string> QuickSplitIntoLines(StringBuilder sb) {
            if (!(sb is null)) {
                var lines = Regex.Split(sb.ToString(), "$^", RegexOptions.Multiline);
                foreach (var line in lines) {
                    yield return line;
                }
            }
        }

        static public int CalcAgeInYears(DateTime birthDate, DateTime? nowOrSystemDefault = null) {

            nowOrSystemDefault = nowOrSystemDefault ?? DateTime.Now;

            DateTime now() => nowOrSystemDefault.Value;

            int yearsDiff = now().Year - birthDate.Year;

            long birthYearTicksSinceStart = (birthDate - new DateTime(birthDate.Year, 1, 1, 0, 0, 0)).Ticks;

            long nowYearTicksSinceStart = (now() - new DateTime(now().Year, 1, 1, 0, 0, 0)).Ticks;

            return (yearsDiff > 0 && nowYearTicksSinceStart < birthYearTicksSinceStart)
                ? (yearsDiff - 1)
                : yearsDiff;
        }

    }
}
