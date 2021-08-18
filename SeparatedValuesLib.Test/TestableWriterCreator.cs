using Com.PlanktonSoup.SeparatedValuesLib;
using System.IO;
using System.Text;

namespace SeparatedValuesLib.Test {

    public class TestableWriterCreator {

        public const string Col_Fullname = "FullName";
        public const string Col_Age = "Age";
        public const string Col_Birth = "Birthday";
        public const string Col_Note = "Note";

        static public readonly string[] CsvColumnNames = new string[] {
            Col_Fullname,
            Col_Age,
            Col_Note,
            Col_Birth,
            };

        static public CharSVWriter<Person> CreateCsvPersonWriter(StringBuilder sb) {

            TextWriter sbWriter = new StringWriter(sb);

            var csvWriter = CharSVWriter<Person>.Create(separator: ',',
                writer: sbWriter, autoCloseWriter: true,
                columnsSpec: CsvColumnNames?.Spec(),
                defineLine: (person, stats) => new() {
                    { Col_Fullname, string.Concat(person.FirstName, " ", person.LastName).Trim() },
                    { Col_Birth, person.Birth },
                    { Col_Age, Helper.CalcAgeInYears(person.Birth) },
                    { Col_Note, person.Note },
                });

            return csvWriter;
        }

        static public CharSVWriter<Person> CreateCsvPersonWriterPinningAgeQualification(StringBuilder sb) {

            TextWriter sbWriter = new StringWriter(sb);

            var csvWriter = CharSVWriter<Person>.Create(separator: ',',
                writer: sbWriter, autoCloseWriter: true,
                columnsSpec: new ColumnSpec[] {
                    new ColumnSpec(Col_Fullname, false),
                    new ColumnSpec(Col_Age, true)
                },
                defineLine: (person, stats) => new() {
                    { Col_Fullname, string.Concat(person.FirstName, " ", person.LastName).Trim() },
                    { Col_Birth, person.Birth },
                    { Col_Age, Helper.CalcAgeInYears(person.Birth) },
                    { Col_Note, person.Note },
                });

            return csvWriter;
        }

    }
}
