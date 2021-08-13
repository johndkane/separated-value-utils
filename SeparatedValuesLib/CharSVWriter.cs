using System;

namespace Com.PlanktonSoup.SeparatedValuesLib {

    /// <summary>
    /// Creates a writer that can write objects and values 
    /// to a Character-Separated-Values format.
    /// </summary>
    /// <typeparam name="TLineObject"></typeparam>
    public class CharSVWriter<TLineObject> : WriterBase<TLineObject>, IDisposable {

        public CharSVWriter(CharSVOptions<TLineObject> options)
            : base(options) {
            ;
        }

    }
}
