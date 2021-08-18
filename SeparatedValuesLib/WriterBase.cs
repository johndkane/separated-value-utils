using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.PlanktonSoup.SeparatedValuesLib {

    /// <summary>
    /// The base class framework for writing separated values to a string format
    /// and representing them as lines in a file.
    /// Inherit this class to provide an implementation that escapes
    /// and separates values for a specific format.
    /// </summary>
    /// <typeparam name="TLineObject">The type of objects that are written as lines</typeparam>
    /// <remarks>
    /// Implementors should inherit this class. For an example see the 
    /// <see cref="CharSVWriter{TLineObject}"/> subclass.
    /// </remarks>
    public abstract class WriterBase<TLineObject> {

        /// <summary>
        /// Settings provided to this writer.
        /// </summary>
        public IWriterSettings<TLineObject> Settings { get; protected internal set; }

        /// <summary>
        /// Statistics about the writing process maintained by this writer.
        /// </summary>
        public WriterStats Stats { get; protected internal set; }

        /// <summary>
        /// Create a writer with the given options.
        /// </summary>
        /// <param name="options"></param>
        public WriterBase(IWriterSettings<TLineObject> options) {
            Settings = options ?? throw new ArgumentNullException(nameof(options));
            Stats = new WriterStats();
        }

        /// <summary>
        /// If a column spec was provided to this instance.
        /// </summary>
        public bool HasColumnSpec
            => !(Settings.ColumnsSpec is null);

        /// <summary>
        /// A header line can only be written if a column spec was provided
        /// to this writer. See <see cref="HasColumnSpec"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws hen there is no column spec in <see cref="Settings"/> </exception>
        public void WriteHeaderLine() {
            if (!HasColumnSpec)
                throw new InvalidOperationException("column spec not defined, cannot write unknown header line - use WriteLine to output a custom header instead");
            WriteLine(Settings.ColumnsSpec);
        }

        /// <summary>
        /// Convenient params wrapper around <see cref="FormatLine(IEnumerable{object})"/>.
        /// </summary>
        public string FormatLine(params object[] lineValues)
            => FormatLine((IEnumerable<object>)lineValues);

        public bool NeedsQualification(CellInfo cell, AnalyzeCellDelegate customCheck = null)
            => Settings.AlwaysQualifyAllColumns
                || (cell.Column.Spec?.AlwaysQualify ?? false)
                || (Settings?.FuncQualificationCheck?.Invoke(cell) ?? false)
                || (customCheck?.Invoke(cell) ?? false);

        /// <summary>
        /// Formats the given object into a line of text for writing.
        /// </summary>
        /// <param name="lineObj">The object to format.</param>
        /// <returns>A line of text for writing.</returns>
        public string FormatLine(TLineObject lineObj) {
            Dictionary<string, object> lineDef = Settings.FuncDefineLineObject(lineObj, Stats) ?? throw new NullReferenceException($"method {nameof(Settings.FuncDefineLineObject)} returned a null value");
            return FormatLine(lineDef);
        }

        /// <summary>
        /// Generates a line of text for writing from the given dictionary
        /// of column names and their values.
        /// </summary>
        /// <param name="lineDefinition">The dictionary defining the line.</param>
        /// <returns>A line of text for writing.</returns>
        /// <remarks>
        /// <para>
        /// The headers of the given definition are mapped to 
        /// <see cref="WriterOptions{TLineObject}.ColumnsSpec"/>
        /// and the line is output in spec order.
        /// </para>
        /// </remarks>
        /// <seealso cref="Settings"/>
        public string FormatLine(Dictionary<string, object> lineDefinition) {

            if (lineDefinition is null)
                throw new ArgumentNullException(nameof(lineDefinition));

            // join only the columns the writer will use

            var sortedMappings = Settings.ColumnsSpec.Join(
                inner: lineDefinition,
                outerKeySelector: colSpec => colSpec.Name,
                innerKeySelector: valueMapping => valueMapping.Key,
                resultSelector: (colSpec, valueMapping)
                    => new ValueMapping(valueMapping.Value, colSpec));

            return BuildLine(sortedMappings).ToString();
        }

        /// <summary>
        /// Generate a line of text from the given values.
        /// The order of the given values is retained.
        /// </summary>
        /// <param name="lineValues">The items to put in the line. Their order is retained.</param>
        /// <returns>A line of text for writing.</returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="lineValues"/> is null.</exception>
        /// <remarks>
        /// Use <see cref="FormatLine(Dictionary{string, object})"/> to match
        /// values to the writer columns specification.
        /// </remarks>
        public string FormatLine(IEnumerable<object> lineValues) {

            if (lineValues is null)
                throw new ArgumentNullException(nameof(lineValues));

            var lineValueMappings = lineValues.Select(value => new ValueMapping(value, null));

            return BuildLine(lineValueMappings).ToString();
        }

        /// <summary>
        /// The core logic to build a line of text using callbacks specified
        /// in settings.
        /// </summary>
        /// <param name="lineMappings">The line values mapped to column specs.</param>
        /// <returns>A <see cref="StringBuilder"/> containing the line to write.</returns>
        internal StringBuilder BuildLine(IEnumerable<ValueMapping> lineMappings) {

            StringBuilder lineBuilder = new();

            int colIndex = 0;
            ColumnInfo colInfo = new ColumnInfo(colIndex);

            foreach (var mapping in lineMappings) {

                string text = Settings.FuncStringifyValue(mapping.Value, Stats, colInfo);

                string escapedText = Settings.FuncEscapeText?.Invoke(text, Stats, colInfo) ?? text;

                CellInfo cell = new CellInfo(mapping.Value, escapedText, colInfo);

                string finalText;

                if (NeedsQualification(cell))
                    finalText = Settings.FuncQualifyText(cell.Text, Stats, colInfo);
                else
                    finalText = escapedText;

                if (colInfo.Index == 0)
                    lineBuilder.Append(finalText);
                else {
                    string expandedLine = Settings.PrependTextSeparator(finalText, Stats, colInfo);
                    lineBuilder.Append(expandedLine);
                }

                colIndex += 1;
                colInfo = new ColumnInfo(colIndex);
            }

            return lineBuilder;
        }

        public void WriteLines(params IEnumerable<object>[] lines)
            => WriteLines((IEnumerable<IEnumerable<object>>)lines);

        /// <summary>
        /// Wraps <see cref="WriteLine(IEnumerable{object})"/>
        /// </summary>
        /// <param name="lines"></param>
        public void WriteLines(IEnumerable<IEnumerable<object>> lines) {
            if (!(lines is null))
                foreach (var lineValues in lines)
                    WriteLine(lineValues);
        }

        /// <summary>
        /// Convenience params wrapper around <see cref="WriteLine(IEnumerable{object})"/>.
        /// </summary>
        public void WriteLine(params object[] lineValues)
            => WriteLine((IEnumerable<object>)lineValues);

        /// <summary>
        /// Writes the given values after properly formatting them into a line.
        /// </summary>
        /// <param name="lineValues"></param>
        /// <seealso cref="FormatLine(IEnumerable{object})"/>
        public void WriteLine(IEnumerable<object> lineValues) {
            string lineStr = FormatLine(lineValues);
            Settings.Writer.WriteLine(lineStr);
            Stats.IncrementLinesWritten();
        }

        /// <summary>
        /// Writes the given object after properly formatting it as a line.
        /// </summary>
        /// <param name="lineObj"></param>
        /// <seealso cref="FormatLine(TLineObject)"/>
        public void WriteObject(TLineObject lineObj) {
            string lineStr = FormatLine(lineObj);
            Settings.Writer.WriteLine(lineStr);
            Stats.IncrementLinesWritten();
        }

        /// <summary>
        /// A convenience params wrapper around <see cref="WriteObjects(IEnumerable{TLineObject})"/>.
        /// </summary>
        /// <param name="lineObjects"></param>
        public void WriteObjects(params TLineObject[] lineObjects)
            => WriteObjects((IEnumerable<TLineObject>)lineObjects);

        /// <summary>
        /// Writes multiple objects as lines. This is a convenience wrapper
        /// around <see cref="WriteObject(TLineObject)"/>.
        /// </summary>
        /// <param name="lineObject">The objects to write.</param>
        public void WriteObjects(IEnumerable<TLineObject> lineObject) {
            if (!(lineObject is null))
                foreach (var obj in lineObject)
                    WriteObject(obj);
        }

        /// <summary>
        /// Automatically closes the write when this object is disposed
        /// if <see cref="WriterOptions{TLineObject}.AutoClose"/> is true
        /// in <see cref="Settings"/>.
        /// </summary>
        public void Dispose() {
            if (Settings.AutoClose)
                Settings.Writer?.Close();
        }
    }
}
