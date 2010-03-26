using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace twopointzero.Lml.Validation
{
    [Serializable]
    public class AggregateException : Exception
    {
        private const string DefaultMessage =
            "One or more exceptions have been encountered and captured in this aggregate exception.";

        private readonly ReadOnlyCollection<Exception> _innerExceptions;

        public AggregateException(IEnumerable<Exception> exceptions) : this(DefaultMessage, exceptions)
        {
        }

        public AggregateException(string message, IEnumerable<Exception> exceptions) : base(message, exceptions.Last())
        {
            _innerExceptions = exceptions.ToList().AsReadOnly();
        }

        protected AggregateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _innerExceptions =
                (ReadOnlyCollection<Exception>)info.GetValue("InnerExceptions", typeof (ReadOnlyCollection<Exception>));
        }

        public ReadOnlyCollection<Exception> InnerExceptions
        {
            get { return _innerExceptions; }
        }

        public override string Message
        {
            get
            {
                var sb = new StringBuilder();
                for (int i = 0; i < _innerExceptions.Count; i++)
                {
                    if (i != 0)
                    {
                        sb.Append(" ");
                    }
                    sb.Append(_innerExceptions[i].Message);
                }

                return base.Message + sb;
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("InnerExceptions", _innerExceptions);
        }
    }
}