using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.PlanktonSoup.SeparatedValuesLib {

    /// <summary>
    /// CSV reader based on Microsoft Excel type parsing logic.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Values containing the quote character (qualifier) are expected to be quoted, 
    /// otherwise it's an error. 
    /// Values containing the separator character as part of the value must be 
    /// quoted otherwise the value will be split on the separator and considered
    /// two values.
    /// A line ending with the separator is interpreted as having an empty value
    /// after the separator.
    /// Multi-line rows are not recognized by the parser: 
    /// each line is interpreted as a complete row.
    /// </para>
    /// <para>
    /// Currently an empty line is treated as a single value of an empty string. There
    /// are some good ideas here about how to implement it for more flexiblity
    /// https://stackoverflow.com/a/12755183/179972
    /// </para>
    /// </remarks>
    public class SVParser {

        readonly char separator;
        readonly char quote;
        readonly TextReader reader;

        public SVParser(char separator, char qualifierQuote, TextReader reader) {
            this.reader = reader;
            this.quote = qualifierQuote;
            this.separator = separator;
        }

        /// <summary>
        /// Parses all lines in the reader.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IEnumerable<string>> ParseAll() {
            string line;
            while ((line = reader.ReadLine()) != null) {
                yield return ParseLine(line);
            }
        }

        /// <summary>
        /// Parses a line of text.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public IEnumerable<string> ParseLine(string line) {

            StringBuilder accumValue = new StringBuilder();
            bool valueCompleteSeparatorOnly = false;
            SVReaderState state = SVReaderState.S0_StartLine;

            string valueOnce() {
                string value = accumValue.ToString();
                accumValue.Clear();
                return value;
            }

            void error() {
                throw new Exception();
            }

            foreach (char ch in line) {

                valueCompleteSeparatorOnly = false;

                switch (state) {

                    case SVReaderState.S0_StartLine:

                        if (ch == quote) {
                            state = SVReaderState.S1_QualificationOpener;
                        }
                        else if (ch == separator) {
                            state = SVReaderState.S5_ValueComplete;
                            yield return valueOnce();
                        }
                        else {
                            state = SVReaderState.S4_BuildingValue;
                            accumValue.Append(ch);
                        }
                        break;

                    case SVReaderState.S1_QualificationOpener:

                        if (ch == quote) {
                            state = SVReaderState.S2_QuoteChordStart;
                        }
                        else {
                            state = SVReaderState.S3_BuildingQualifiedValue;
                            accumValue.Append(ch);
                        }
                        break;

                    case SVReaderState.S2_QuoteChordStart:

                        if (ch == quote) {
                            state = SVReaderState.S3_BuildingQualifiedValue;
                            accumValue.Append(quote);
                        }
                        else if (ch == separator) {
                            state = SVReaderState.S5_ValueComplete;
                            yield return valueOnce();
                        }
                        else {
                            state = SVReaderState.S6_ERROR;
                        }
                        break;

                    case SVReaderState.S3_BuildingQualifiedValue:

                        if (ch == quote) {
                            state = SVReaderState.S2_QuoteChordStart;
                        }
                        else {
                            accumValue.Append(ch);
                        }
                        break;

                    case SVReaderState.S4_BuildingValue:

                        if (ch == quote) {
                            state = SVReaderState.S2_QuoteChordStart;
                        }
                        else if (ch == separator) {
                            state = SVReaderState.S5_ValueComplete;
                            yield return valueOnce();
                        }
                        else {
                            accumValue.Append(ch);
                        }
                        break;

                    case SVReaderState.S5_ValueComplete:

                        if (ch == quote) {
                            state = SVReaderState.S1_QualificationOpener;
                        }
                        if (ch == separator) {
                            valueCompleteSeparatorOnly = true;
                            yield return valueOnce();
                        }
                        else {
                            state = SVReaderState.S4_BuildingValue;
                            accumValue.Append(ch);
                        }
                        break;
                }

                // when an error state occurs effect it immediately 
                if (state == SVReaderState.S6_ERROR)
                    error();

            } //char loop until end of string reached


            // implicitly hits the endline here which is not handled by the above cases

            if (state.In(SVReaderState.S0_StartLine)) {
                yield return null;
            }
            else if (state.In(SVReaderState.S4_BuildingValue) || valueCompleteSeparatorOnly) {
                yield return valueOnce();
            }
            else if (state.In(SVReaderState.S1_QualificationOpener,
                SVReaderState.S2_QuoteChordStart,
                SVReaderState.S3_BuildingQualifiedValue)) {

                error();
            }

        }

    }
}
