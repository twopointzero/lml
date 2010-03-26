using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace twopointzero.Lml.Validation
{
    public class ValidatorInstance
    {
        private readonly List<Exception> _exceptions = new List<Exception>(1);

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal ValidatorInstance()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal void AddException(Exception exception)
        {
            _exceptions.Add(exception);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal void Throw()
        {
            if (_exceptions.Count == 1)
            {
                throw _exceptions[0];
            }
            throw new AggregateException(_exceptions);
        }
    }
}