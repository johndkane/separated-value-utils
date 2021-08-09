using System;

namespace Com.PlanktonSoup.SeparatedValuesLib {

    /// <summary>
    /// Creates a writer that can write objects and values 
    /// to a Character-Separated-Values format.
    /// </summary>
    /// <typeparam name="TLineObject"></typeparam>
    public class CharSeparatedValuesWriter<TLineObject> : SeparatedValuesWriterBase<TLineObject>, IDisposable {

        public CharSeparatedValuesStrategy Info { get; internal protected set; }

        public CharSeparatedValuesWriter(CharSeparatedValuesOptions<TLineObject> options)             
            : base(options) {
            ;
        }

    }
}
