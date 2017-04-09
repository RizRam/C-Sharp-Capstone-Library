using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClient
{
    /// <summary>
    /// EventArg to be used in conjunction with DatabaseErrorHandler delegates and events
    /// </summary>
    public class DatabaseEventArgs : EventArgs
    {
        public string Error { get; private set; } //Error message

        /// <summary>
        /// Constructor for DatabaseEventArgs
        /// </summary>
        /// <param name="message">Error message to be passed to event subscribers</param>
        public DatabaseEventArgs(string message)
        {
            Error = message;
        }
    }
}
