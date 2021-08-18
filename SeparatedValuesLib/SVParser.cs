using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.PlanktonSoup.SeparatedValuesLib {

    public class SVParser {

        readonly char separator;
        readonly char quote;
        readonly TextReader reader;

        public SVParser(char separator, char qualifierQuote, TextReader reader) {
            this.reader = reader;
            this.quote = qualifierQuote;
            this.separator = separator;
        }

        public IEnumerable<IEnumerable<string>> ParseAll() {
            string line;
            while ((line = reader.ReadLine()) != null) {
                yield return ParseLine(line);
            }
        }

        public IEnumerable<string> ParseLine(string line) {

            StringBuilder accumValue = new StringBuilder();
            bool insideQualifiedValue = false;
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
                switch (state) {

                    case SVReaderState.S0_StartLine:

                        if (ch == quote) {
                            state = SVReaderState.S1_QualificationOpener;
                            insideQualifiedValue = true;
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

            if (state.In(SVReaderState.S4_BuildingValue)) {
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
