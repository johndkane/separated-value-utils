using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Com.PlanktonSoup.SeparatedValuesLib {

    /// <summary>
    /// Options for the <see cref="SeparatedValuesWriterBase{TLineObject}"/>
    /// </summary>
    public class WriterOptions<TLineObject> {

        /// <summary>
        /// Utility method to define an object using its public properties that
        /// are readable.
        /// </summary>
        /// <param name="obj">The object to define.</param>
        /// <returns>A dictionary containing the public properties of the given object.</returns>
        /// <remarks>
        /// This method is the default for <see cref="FuncDefineLineObject"/>.
        /// </remarks>
        static public Dictionary<string, object> DefinePublicProps(TLineObject obj) {

            var props = typeof(TLineObject).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            Dictionary<string, object> def = new();

            if (!(props is null)) {
                foreach (var p in props)
                    if (p.CanRead)
                        def[p.Name] = p.GetValue(obj);
            }

            return def;
        }

        /// <summary>
        /// The default method to stringify any value by making an internal
        /// call to <see cref="Convert.ToString(object)"/>.
        /// </summary>
        /// <param name="value">The value to convert to a string.</param>
        /// <param name="columnName">The column name of the writer this value belongs to.</param>
        /// <returns></returns>
        static public string DefaultStringifyValue(object value, string columnName)
            => Convert.ToString(value);

        /// <summary>
        /// Creates a minimal writer options instance. The implementor is expected
        /// to fill in missing delegates and properties otherwise these options
        /// will fail the writer.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="autoCloseWriter"></param>
        /// <param name="columnSpecOrNull"></param>
        protected internal WriterOptions(TextWriter writer, bool autoCloseWriter,
            IEnumerable<string> columnSpecOrNull) {
            this.Writer = writer;
            this.AutoClose = autoCloseWriter;
            this.ColumnSpec = columnSpecOrNull;
        }

        /// <summary>
        /// Initialize all members of this options instance.
        /// </summary>
        /// <param name="writer">The writer to use.</param>
        /// <param name="autoCloseWriter">Whether to close the writer automatically when done.</param>
        /// <param name="columnSpecOrNull">The optional column spec. Value can still be written without this.</param>
        /// <param name="escapeValue">The function to escape a given value.</param>
        /// <param name="separateValue">The function to separate a given value from the previous values.</param>
        /// <param name="defineLineOrDefault">The function to define an object.</param>
        /// <param name="stringifyValueOrDefault">The function to stringify a value.</param>
        public WriterOptions(TextWriter writer, bool autoCloseWriter,
            IEnumerable<string> columnSpecOrNull,
            TransformStringDelegate escapeValue,
            TransformStringDelegate separateValue,
            DefineObjectDelegate<TLineObject> defineLineOrDefault = null,
            StringifyValueDelegate stringifyValueOrDefault = null) {

            this.Writer = writer ?? throw new ArgumentNullException(nameof(writer));
            this.AutoClose = autoCloseWriter;

            this.ColumnSpec = columnSpecOrNull;

            AssignFuncEscapeValue(escapeValue);
            AssignFuncSeparateValue(separateValue);

            AssignFuncDefineLineOrDefault(defineLineOrDefault);
            AssignFuncStringifyValueOrDefault(stringifyValueOrDefault);
        }

        /// <summary>
        /// Assign the function to separate values.
        /// </summary>
        /// <param name="funcSeparateValue"></param>
        protected internal void AssignFuncSeparateValue(TransformStringDelegate funcSeparateValue) {
            this.FuncSeparateValue = funcSeparateValue ?? throw new ArgumentNullException(nameof(funcSeparateValue));
        }

        /// <summary>
        /// Assign the function to escape values.
        /// </summary>
        /// <param name="funcEscapeValue"></param>
        protected internal void AssignFuncEscapeValue(TransformStringDelegate funcEscapeValue) {
            this.FuncEscapeValue = funcEscapeValue ?? throw new ArgumentNullException(nameof(funcEscapeValue));
        }

        /// <summary>
        /// Assign the function to define an object to become a line for writing.
        /// </summary>
        /// <param name="funcDefineLine">Defaults to <see cref="DefinePublicProps(TLineObject)"/> if null.</param>
        protected internal void AssignFuncDefineLineOrDefault(DefineObjectDelegate<TLineObject> funcDefineLine) {
            this.FuncDefineLineObject = funcDefineLine ?? DefinePublicProps;
        }

        /// <summary>
        /// Assign the function to stringify values for line writing.
        /// </summary>
        /// <param name="funcStringifyValue">Defaults to <see cref="DefaultStringifyValue(object, string)"/> if null.</param>
        protected internal void AssignFuncStringifyValueOrDefault(StringifyValueDelegate funcStringifyValue) {
            this.FuncStringifyValue = funcStringifyValue ?? DefaultStringifyValue;
        }

        /// <summary>
        /// The text writer to use to write lines with.
        /// </summary>
        public TextWriter Writer { get; internal protected set; }

        /// <summary>
        /// Whether to automatically close the <see cref="Writer"/> when done writing.
        /// </summary>
        public bool AutoClose { get; internal protected set; }

        /// <summary>
        /// Function to define a line to write using its column names.
        /// </summary>
        public DefineObjectDelegate<TLineObject> FuncDefineLineObject { get; private set; }

        /// <summary>
        /// Function to convert a <typeparamref name="TLineObject"/> instance into a string.
        /// </summary>
        public StringifyValueDelegate FuncStringifyValue { get; private set; }

        /// <summary>
        /// Function to escape a value for writing.
        /// </summary>
        public TransformStringDelegate FuncEscapeValue { get; private set; }

        /// <summary>
        /// Function to separate the next value from the previous ones, for writing.
        /// </summary>
        public TransformStringDelegate FuncSeparateValue { get; private set; }

        /// <summary>
        /// The columns to use for writing. The order of these columns
        /// is retained by the <see cref="SeparatedValuesWriterBase{TLineObject}"/>.
        /// </summary>
        public IEnumerable<string> ColumnSpec { get; internal protected set; }
    }
}
