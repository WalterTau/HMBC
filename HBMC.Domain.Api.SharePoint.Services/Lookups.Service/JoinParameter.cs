using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointHelper.Lookups
{
    /// <summary>
    /// This class wil store Join Parameter
    /// </summary>
   public  class JoinParameter
    {
        /// <summary>
        /// Name of the list you want to join with
        /// </summary>
        public string ListName { get; set; }
        /// <summary>
        /// This will be the foreign key between two list
        /// </summary>
        public string JoinField { get; set; }
        /// <summary>
        /// This will be the join Type from table to next table
        /// </summary>
        public JoinType JoinType { get; set; }
    }
}
