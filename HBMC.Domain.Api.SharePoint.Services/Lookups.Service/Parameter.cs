using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace SharePointHelper.Lookups
{
    /// <summary>
    /// This class will have fields that will be passed on the filter
    /// </summary>
  public  class Parameter
    {
        /// <summary>
        /// This will hold the field(column) that will be a filter
        /// </summary>
        public string ParameterName { get; set; }
        /// <summary>
        /// This will hold the Data  Type of the parameter
        /// </summary>
        public SharePointDataType ParameterType { get; set; }
        /// <summary>
        /// This will hold the field value
        /// </summary>
        public string ParameterValue { get; set; }
        /// <summary>
        /// This will field will hold a codition e.g EqualTo(<Eq></Eq>)
        /// </summary>
        public Condition Condition { get; set; }
        /// <summary>
        /// This field will hold logical operators
        /// </summary>
        public Operator Operator { get; set; }
        /// <summary>
        /// This will hold IN Operator values
        /// </summary>
        public List<string> InValues { get; set; }
        /// <summary>
        /// This will hold whether the value you passing is the lookup field or not
        /// </summary>
        public bool IsLookupField { get; set; }
        /// <summary>
        /// This will determine whether the field you want to update is a a people or not
        /// </summary>
        public bool IsPeoplePicker { get; set; }
        /// <summary>
        /// This is user object that will be passed to the people picker
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// Workflow Status
        /// </summary>
        public WorkflowStatus WorkflowStatus { get; set; }
    
    }
  
 
}
