using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClient
{

    /// <summary>
    /// An Abstract data type to store CheckOutLog objects.
    /// Uses a generic List of CheckOutLogs as the internal data structured
    /// </summary>
    public class CheckOutLogList : IEnumerable<CheckOutLog>
    {
        private const int MINIMUM_CAPACITY = 1000;  //minimum capcity of the internal list
        private List<CheckOutLog> logs;  //the internal data structure of the class.

        //A list of all overdue books/logs in the collection
        public List<CheckOutLog> OverdueLogs
        {
            get
            {
                var overdues = (from log in logs
                                where log.IsOverdue
                                select log).ToList();
                overdues.Sort();
                return overdues;
            }
        }

        public int Count { get { return logs.Count; } }  //The current count of elements stored in logs

        /// <summary>
        /// Constructor for CheckOutLogList
        /// Initializes internal list with minimum capacity
        /// </summary>
        public CheckOutLogList()
        {
            logs = new List<CheckOutLog>(MINIMUM_CAPACITY);
        }

        /// <summary>
        /// Constructor for CheckOutLogList
        /// Initializes internal list with minimum capacity and Adds all elements of the collection in the argument into internal list
        /// </summary>
        /// <param name="list">Generic ICollection of CheckOutLog objects to be added into collection</param>
        public CheckOutLogList(ICollection<CheckOutLog> list) : this()
        {
            this.Add(list);
        }

        #region Add

        /// <summary>
        /// Adds a CheckOutLog into the collection, keeps list sorted after addition.
        /// </summary>
        /// <param name="log"></param>
        public void Add(CheckOutLog log)
        {
            logs.Add(log);
            logs.Sort();
        }

        /// <summary>
        /// Adds all elements in collection passed in argument into this collection.  Keeps list sorted
        /// </summary>
        /// <param name="checkOutLogs">A generic ICollection of CheckOutLog objects to be added into this collection</param>
        public void Add(ICollection<CheckOutLog> checkOutLogs)
        {
            logs.AddRange(checkOutLogs);
            logs.Sort();
        }

        #endregion Add

        #region Remove

        /// <summary>
        /// Removes CheckOutLog from collection
        /// </summary>
        /// <param name="log">CheckOutLog to be removed from collection</param>
        /// <returns>Returns true if operation is successful, false otherwise</returns>
        public bool Remove(CheckOutLog log)
        {
            return logs.Remove(log);
        }

        #endregion Remove

        /// <summary>
        /// Finds all CheckOutLog objects that contain the searchToken passed in the argument.
        /// </summary>
        /// <param name="searchToken">string used to find CheckOutLog objects</param>
        /// <returns>Returns a Generic List of CheckOutLog objects that contain the search toke</returns>
        public List<CheckOutLog> FindAll(string searchToken)
        {
            List<CheckOutLog> results = new List<CheckOutLog>();
            foreach(CheckOutLog log in logs)
            {
                if (log.ContainsToken(searchToken)) results.Add(log);
            }

            return results;
        }

        #region IEnumerable
        //IEnumerable implementation

        public IEnumerator<CheckOutLog> GetEnumerator()
        {
            return logs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion IEnumerable
    }
}
