using Com.PlanktonSoup.SeparatedValuesLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace SeparatedValuesLib.Test {

    public class CsvWriterTester {

        class Person {
            public int Age;
            public string FirstName;
            public string LastName;
            public DateTime Birth;
            public string Note;
        }

        const string Col_Fullname = "FullName";
        const string Col_Age = "Age";
        const string Col_Birth = "Birthday";
        const string Col_Note = "Note";

        [Fact]
        public void Expect_correct_Csv_lines() {

            var columns = new string[] {
            Col_Fullname,
            Col_Birth ,
            Col_Note,
            };

            StringBuilder sb = new StringBuilder();
            TextWriter sbWriter = new StringWriter(sb);

            // define csv options

            var csvOptions = CharSeparatedValuesOptions<Person>.Csv(writer: sbWriter,
                // 
                columnSpecOrNull: columns,
                defineObjectOrDefault: person => new Dictionary<string, object> {
                    { Col_Fullname, string.Concat(person.FirstName, " ", person.LastName).Trim() },
                    { Col_Age, person.Age },
                    { Col_Birth, person.Birth },
                    { Col_Note, person.Note },
                });

            // create the csv writer

            using (var csvWriter = new CharSeparatedValuesWriter<Person>(csvOptions)) {

                // and write lines with it

                csvWriter.WriteHeaderLine();

                csvWriter.WriteObjects(new Person {
                    FirstName = "John,s",
                    LastName = "Doe",
                    Birth = new DateTime(1970, 1, 1),
                    Note = "test \"record\" 1",
                }, new Person {
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
            };

            Assert.NotEqual(0, sb.Length);

            int actualLineCount = sb.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Length;
            Assert.Equal(4, actualLineCount); // includes header row
        }

    }
}
