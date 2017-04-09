using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClient
{
    /// <summary>
    /// An abstract data type that stores a collection of MPerson objects.
    /// Uses three generic Lists to store MLibrarians, MAuthors, and MCardholders to store the data internally
    /// </summary>
    public class PeopleList : IEnumerable<MPerson>
    {
        private const int MINIMUM_LIBRARIANS = 20;  //minimum capacity of MLibrarians
        private const int MINIMUM_AUTHORS = 1000;  //minimum capacity of MAuthors

        //internal generic list to store MLibrarian objects
        private List<MLibrarian> librarians;
        public List<MLibrarian> Librarians { get { return librarians; } }

        //internal generic list to store MCardholder objects
        private List<MCardholder> cardholders;
        public List<MCardholder> Cardholders { get { return cardholders; } }

        //internal generic list to store MAuthor objects
        private List<MAuthor> authors;
        public List<MAuthor> Authors { get { return authors; } }

        //The current number of elements in the collection
        public int Count { get { return librarians.Count + cardholders.Count + authors.Count; } }

        /// <summary>
        /// Default Constructor
        /// Initializes internal lists with specified minimum capacities.
        /// </summary>
        public PeopleList()
        {
            // = new List<MPerson>();
            librarians = new List<MLibrarian>(MINIMUM_LIBRARIANS);
            cardholders = new List<MCardholder>();
            authors = new List<MAuthor>(MINIMUM_AUTHORS);
        }

        //Creates a new PeopleList object and populates list with the contents of ICollecction<MPerson> passed in the argument.
        /// <summary>
        /// Constructor for PeopleList
        /// Initializes internal lists and populates the internal lists with the elements in the Icollection passed in the argument
        /// </summary>
        /// <param name="people">Generic ICollection of MPerson objects used to populate this collection</param>
        public PeopleList(ICollection<MPerson> people) : this()
        {
            this.Add(people);
        }

        #region Add

        /// <summary>
        /// Adds an MPerson to the collection.  Adds the person to the appropriate internal data structure according to its data type.
        /// Keeps all internal lists sorted according to PeopleComparer class.
        /// </summary>
        /// <param name="person">MPerson to be added into the collection</param>
        public void Add(MPerson person)
        { 
            if (person is MLibrarian)
            {
                librarians.Add((MLibrarian)person);
                librarians.Sort(new MPerson.PeopleComparer());
            }
            else if (person is MCardholder)
            {
                cardholders.Add((MCardholder)person);
                cardholders.Sort(new MPerson.PeopleComparer());
            }
            else if (person is MAuthor)
            {
                authors.Add((MAuthor)person);
                authors.Sort(new MPerson.PeopleComparer());
            }
        }

        /// <summary>
        /// Adds the contents of generic ICollection of MPerson objects into the collection.
        /// Keeps the list sorted according to the specifications of <c>PeopleComparer</c>
        /// </summary>
        /// <param name="people"></param>
        public void Add(ICollection<MPerson> people)
        {
            foreach(MPerson p in people)
            {
                Add(p);
            }
        }

        #endregion Add

        #region Remove

        /// <summary>
        /// Removes MPerson object from the collectoin
        /// </summary>
        /// <param name="person">MPerson to be removed</param>
        public void Remove(MPerson person)
        {
            if (person is MLibrarian)
            {
                librarians.Remove((MLibrarian)person);
            }
            else if (person is MAuthor)
            {
                authors.Remove((MAuthor)person);
            }
            else if (person is MCardholder)
            {
                cardholders.Remove((MCardholder)person);
            }
        }

        #endregion Remove

        #region IEnumerable
        //IEnumerable implementation

        public IEnumerator<MPerson> GetEnumerator()
        {
            //Add all elements in the collection into a single List and sort
            List<MPerson> list = new List<MPerson>();
            list.AddRange(librarians);
            list.AddRange(cardholders);
            list.AddRange(authors);

            list.Sort();

            //return the enumerator of the list
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion IEnumerable
    }
}
