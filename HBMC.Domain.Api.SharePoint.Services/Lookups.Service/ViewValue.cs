using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointHelper.Lookups
{
    /// <summary>
    /// This class will store the fields that will be displayed
    /// </summary>
   public class ViewValue
    {
        /// <summary>
        /// The actual column name
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// The display name like AS of SQL, so this value must be unique
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// List name where the field is coming from
        /// </summary>
        public string ListName { get; set; }
    }
}
