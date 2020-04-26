using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using GDNET.Extensions;

namespace GDNET.Exceptions
{
    /// <summary>
    /// An exception from the GD servers. Will provide a message explaining the error code.
    /// </summary>
    public class GdWebException : Exception
    {
        /// <summary>
        /// The type of error from the gd servers.
        /// </summary>
        public GdErrorType ErrorType;

        public GdWebException(string message)
            : base(message)
        {

        }
    }

    /// <summary>
    /// The type of error from the gd servers.
    /// </summary>
    public enum GdErrorType
    {
        [Description("The request data provided is most likely invalid.")]
        Invalid = -1,
    }
}
