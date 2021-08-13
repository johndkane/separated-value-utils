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
    /// <see cref="CharSeparatedValuesWriter{TLineObject}"/> subclass.
    /// </remarks>
    public abstract class WriterBase<TLineObject> {

        /// <summary>
        /// Settings provided to this writer.
        /// </summary>
        public WriterOptions<TLineObject> Settings { get; protected internal set; }

        /// <summary>
        /// Statistics about the writing process maintained by this writer.
        /// </summary>
        public WriterStats Stats { get; protected internal set; }

        /// <summary>
        /// Create a writer with the given options.
        /// </summary>
        /// <param name="options"></param>
        public WriterBase(WriterOptions<TLineObject> options) {
            Settings = options ?? throw new ArgumentNullException(nameof(options));
            Stats = new WriterStats();
        }

        /// <summary>
        /// If a column spec was provided to this instance.
        /// </summary>
        public bool HasColumnSpec
            => !(Settings.ColumnSpec is null);

        /// <summary>
        /// A header line can only be written if a column spec was provided
        /// to this writer. See <see cref="HasColumnSpec"/>
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws when there is no column spec in <see cref="Settings"/> </exception>
        public void WriteHeaderLine() {
            if (!HasColumnSpec)
                throw new InvalidOperationException("column spec not defined, cannot write");
            WriteLine(Settings.ColumnSpec);
        }

        /// <summary>
        /// Convenient params wrapper around <see cref="FormatLine(IEnumerable{object})"/>.
        /// </summary>
        public string FormatLine(params object[] items)
            => FormatLine((IEnumerable<object>)items);

        /// <summary>
        /// Generates a text line for writing from the given values; values 
        /// are ordered to match the column spec in <see cref="Settings"/>.
        /// </summary>
        /// <param name="items">The items to put in the line. Their order is retained.</param>
        /// <returns>A line of text for writing.</returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="items"/> is null.</exception>
        public string FormatLine(IEnumerable<object> items) {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            StringBuilder lineBuilder = new();

            int colIndex = 0;
            foreach (var item in items) {
                string escapedItem = Settings.FuncEscapeValue(Settings.FuncStringifyValue(item, null), null);
                if (colIndex == 0)
                    lineBuilder.Append(escapedItem);
                else
                    lineBuilder.Append(Settings.FuncSeparateValue(escapedItem, null));
                colIndex += 1;
            }

            return lineBuilder.ToString();
        }

        /// <summary>
        /// Formats the given object into a line of text for writing.
        /// </summary>
        /// <param name="obj">The object to format.</param>
        /// <returns>A line of text for writing.</returns>
        public string FormatLine(TLineObject obj) {
            Dictionary<string, object> lineDef = Settings.FuncDefineLineObject(obj) ?? throw new NullReferenceException($"method {nameof(Settings.FuncDefineLineObject)} returned a null value");
            return FormatLine(lineDef);
        }

        /// <summary>
        /// Generates a line of text for writing from the given dictionary
        /// of column name keys and their values.
        /// </summary>
        /// <param name="lineDef"></param>
        /// <returns>A line of text for writing.</returns>
        /// <remarks>
        /// <para>
        /// This method ensures the columns are output in the same order
        /// as defined in <see cref="WriterOptions{TLineObject}.ColumnSpec"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="Settings"/>
        public string FormatLine(Dictionary<string, object> lineDef) {
            if (lineDef is null)
                throw new ArgumentNullException(nameof(lineDef));

            // join only the columns the writer will use

            var orderedCells = Settings.ColumnSpec.Join(
                inner: lineDef,
                outerKeySelector: writerCol => writerCol,
                innerKeySelector: lineCell => lineCell.Key,
                resultSelector: (writerCol, lineCell) => lineCell);

            var escapedCells = orderedCells
                .Select(cell => new {
                    EscapedText = Settings.FuncEscapeValue(Settings.FuncStringifyValue(cell.Value, cell.Key), cell.Key),
                    Column = cell.Key,
                });

            StringBuilder lineBuilder = new();

            int colIndex = 0;
            foreach (var cell in escapedCells) {
                if (colIndex != 0)
                    lineBuilder.Append(Settings.FuncSeparateValue(cell.EscapedText, cell.Column));
                else
                    lineBuilder.Append(cell.EscapedText);
                colIndex += 1;
            }

            return lineBuilder.ToString();
        }

        /// <summary>
        /// Convenience params wrapper around <see cref="WriteLine(IEnumerable{object})"/>.
        /// </summary>
        public void WriteLine(params object[] values)
            => WriteLine((IEnumerable<object>)values);

        /// <summary>
        /// Writes the given values after properly formatting them into a line.
        /// </summary>
        /// <param name="values"></param>
        /// <seealso cref="FormatLine(IEnumerable{object})"/>
        public void WriteLine(IEnumerable<object> values) {
            string lineStr = FormatLine(values);
            Settings.Writer.WriteLine(lineStr);
            Stats.IncrementLinesWritten();
        }

        /// <summary>
        /// Writes the given object after properly formatting it as a line.
        /// </summary>
        /// <param name="obj"></param>
        /// <seealso cref="FormatLine(TLineObject)"/>
        public void WriteObject(TLineObject obj) {
            string lineStr = FormatLine(obj);
            Settings.Writer.WriteLine(lineStr);
            Stats.IncrementLinesWritten();
        }

        /// <summary>
        /// A convenience params wrapper around <see cref="WriteObjects(IEnumerable{TLineObject})"/>.
        /// </summary>
        /// <param name="objects"></param>
        public void WriteObjects(params TLineObject[] objects)
            => WriteObjects((IEnumerable<TLineObject>)objects);

        /// <summary>
        /// Writes multiple objects as lines. This is a convenience wrapper
        /// around <see cref="WriteObject(TLineObject)"/>.
        /// </summary>
        /// <param name="objects">The objects to write.</param>
        public void WriteObjects(IEnumerable<TLineObject> objects) {
            if (!(objects is null))
                foreach (var obj in objects)
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
