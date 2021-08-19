using System;
using System.Text;
using Xunit;

namespace SeparatedValuesLib.Test {

    public class CsvWriterTester {

        [Fact]
        public void Expect_correct_Csv_lines() {

            // create a csv writer

            StringBuilder sb = new StringBuilder();
<<<<<<< HEAD
=======
            TextWriter sbWriter = new StringWriter(sb);

            // define csv options

            var csvOptions = CharSVOptions<Person>.Csv(writer: sbWriter,
                // 
                columnSpecOrNull: columns,
                defineObjectOrDefault: person => new Dictionary<string, object> {
                    { Col_Fullname, string.Concat(person.FirstName, " ", person.LastName).Trim() },
                    { Col_Age, person.Age },
                    { Col_Birth, person.Birth },
                    { Col_Note, person.Note },
                });

            // create the csv writer

            using (var csvWriter = new CharSVWriter<Person>(csvOptions)) {
>>>>>>> 44fa28409c4555545145d07eb5a703650c87353d

            var csvWriter = TestableWriterCreator.CreateCsvPersonWriter(sb);

            using (csvWriter) {
                csvWriter.WriteHeaderLine();

                csvWriter.WriteObjects(new() {
                    FirstName = "John,s",
                    LastName = "Doe",
                    Birth = new DateTime(1970, 1, 1),
                    Note = "test \"record\" 1",
                },
                new() {
                    FirstName = "Sarah",
                    LastName = "Smith",
                    Birth = new DateTime(1973, 7, 3),
                    Note = "test record 2",
                });

                /* Or write plain values as a line item without mapping them to the column names.
                 * It is the responsiblity of the caller to order the values properly
                 */
                csvWriter.WriteLine("John Jacob Jingleheimer Schmidt", new DateTime(1950, 1, 1), "infamous actor");

                Assert.Equal(4, csvWriter.Stats.TotalLinesWritten);
            }

            Assert.NotEqual(0, sb.Length);

            int actualLineCount = sb.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Length;
            Assert.Equal(4, actualLineCount); // includes header row
        }

    }
}
