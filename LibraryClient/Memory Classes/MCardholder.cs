using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClient
{
    /// <summary>
    /// A class meant to represent the Entity-Framework code-generated class Cardholder locally in memory.
    /// Inherits from MPerson class.
    /// </summary>
    public class MCardholder : MPerson
    {
        public Cardholder Cardholder { get; private set; }  //A read-only property of this object's associated Cardholder
        public string Phone { get; set; }  //Phone number of MCardholder/Cardholder
        public string LibraryCardId { get; set; }  //Library Card ID of MCardholder/Cardholder
        public ICollection<CheckOutLog> CheckOutLogs {get; set;}  //A collection of CheckOutLogs that contain this objects's associated Cardholder

        //A boolean Property that returns true if this MCardholder has any books that are overdue in CheckOutLogs
        public bool HasOverdueBooks
        {
            get
            {
                foreach(CheckOutLog log in CheckOutLogs)
                {
                    if (log.IsOverdue)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Constructor for MCardholder
        /// Copeis the fields and Properties of Cardholder in argument
        /// </summary>
        /// <param name="ch">Cardholder used to initialize fields and Properties</param>
        public MCardholder (Cardholder ch)
        {
            Cardholder = ch;
            this.ID = ch.PersonId;
            this.FirstName = ch.Person.FirstName;
            this.LastName = ch.Person.LastName;
            this.Phone = ch.Phone;
            this.LibraryCardId = ch.LibraryCardId;
            this.CheckOutLogs = ch.CheckOutLogs;
        }

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns>string that describes this object</returns>
        public override string ToString()
        {
            string result = base.ToString();
            result += string.Format("\t{0, -10}\tPhone: {1, -11}",LibraryCardId, Phone);
            return result;
        }
    }
}
