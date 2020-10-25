using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointHelper.Lookups
{
    /// <summary>
    /// This class wil set the constant strings
    /// </summary>
   public static class SharePointStringHelper
    {
        #region Conditions
        /// <summary>
        /// <Eq></Eq>
        /// </summary>
        public const string EqualTo = "Eq";
        /// <summary>
        /// <Neq></Neq>
        /// </summary>
        public const string NoEqualTo = "Neq";
        /// <summary>
        /// <Gt></Gt>
        /// </summary>
        public const string GreaterThan = "Gt";
        /// <summary>
        /// <Lt></Lt>
        /// </summary>
        public const string LessThan = "Lt";
        /// <summary>
        /// <Geq></Geq>
        /// </summary>
        public const string GreaterThanOrEqualTo = "Geq";
        /// <summary>
        /// <Leq></Leq>
        /// </summary>
        public const string LessThanOrEqualTo = "Leq";
        /// <summary>
        /// <BeginsWith></BeginsWith>
        /// </summary>
        public const string BeginsWith = "BeginsWith";
        /// <summary>
        /// <Contains></Contains>
        /// </summary>
        public const string Contains = "Contains";
        /// <summary>
        /// <IsNotNull></IsNotNull>
        /// </summary>
        public const string IsNotNull = "IsNotNull";
        /// <summary>
        /// <IsNull></IsNull>
        /// </summary>
        public const string IsNull = "IsNull";
        /// <summary>
        /// <In></In>
        /// </summary>
        public const string In = "In";
        #endregion
        #region SharePoint Data Types
        /// <summary>
        /// Text Data Type
        /// </summary>
        public const string Text = "Text";
        /// <summary>
        /// Multiline Text
        /// </summary>
        public const string Note = "Note";
        /// <summary>
        /// Choice(menu to choose from)
        /// </summary>
        public const string Choice = "Choice";
        /// <summary>
        /// Number(1,1.0,100)
        /// </summary>
        public const string Number = "Number";
        /// <summary>
        /// Currency($)
        /// </summary>
        public const string Currency = "Currency";
        /// <summary>
        /// Date and Time
        /// </summary>
        public const string DateTime = "DateTime";
        /// <summary>
        /// Lookup field from other list
        /// </summary>
        public const string Lookup = "Lookup";
        /// <summary>
        /// Counter
        /// </summary>
        public const string Counter = "COUNTER";
        /// <summary>
        /// WorkflowStatus 
        /// </summary>
        public const string WorkflowStatus = "WorkflowStatus";
        #endregion
        #region Logical Operators
        /// <summary>
        /// AND Operator
        /// </summary>
        public const string And = "And";
        /// <summary>
        /// OR Operator
        /// </summary>
        public const string Or = "Or";
        #endregion
        #region Joins
        /// <summary>
        /// Inner Join INNER
        /// </summary>
        public const string InnerJoin = "INNER";
        /// <summary>
        /// Left Join LEFT
        /// </summary>
        public const string LeftJoin = "LEFT";
        #endregion
        #region Aggregate Functions
        /// <summary>
        /// COUNT Aggregate function
        /// </summary>
        public const string Count = "COUNT";
        /// <summary>
        /// AVG Aggregate Function
        /// </summary>
        public const string Average = "AVG";
        /// <summary>
        /// MAX Aggregate Function
        /// </summary>
        public const string Maximum = "MAX";
        /// <summary>
        /// MIN Aggregate Function
        /// </summary>
        public const string Minimum = "MIN";
        /// <summary>
        /// SUM Aggregate Function
        /// </summary>
        public const string Sum = "SUM";
        /// <summary>
        /// Standard Deviation Aggregate Function
        /// </summary>
        public const string StandardDeviation = "STDEV";
        /// <summary>
        /// VARIENCE Aggregate Fuction
        /// </summary>
        public const string Variance = "VAR";
        #endregion
        #region Workflow Event Names
        /// <summary>
        /// None event
        /// </summary>
        public const string None = "None";
        /// <summary>
        /// Task Completed
        /// </summary>
        public const string TaskCompleted = "Task Completed";
        /// <summary>
        /// Task Created
        /// </summary>
        public const string TaskCreated = "Task Created";
        /// <summary>
        /// Task Deleted
        /// </summary>
        public const string TaskDeleted = "Task Deleted";
        /// <summary>
        /// Task Modified
        /// </summary>
        public const string TaskModified = "Task Modified";
        /// <summary>
        /// Task Rolled Back
        /// </summary>
        public const string TaskRolledBack = "Task Rolled Back";
        /// <summary>
        /// Task Cancelled
        /// </summary>
        public const string WorkflowCancelled = "Workflow Cancelled";
        /// <summary>
        /// Workflow Comment
        /// </summary>
        public const string WorkflowComment = "Workflow Comment";
        /// <summary>
        /// Workflow Compoleted
        /// </summary>
        public const string WorkflowCompleted = "Workflow Completed";
        /// <summary>
        /// Workflow Error
        /// </summary>
        public const string WorkflowError = "Workflow Error";
        /// <summary>
        /// Workflow Started
        /// </summary>
        public const string WorkflowStarted = "Workflow Started";
        /// <summary>
        /// Workflow Deleted
        /// </summary>
        public const string WorkflowDeleted = "Workflow Deleted";
        #endregion
        #region Workflow Status Text
        /// <summary>
        /// Not Started
        /// </summary>
        public const string NotStarted = "Not Started";
        /// <summary>
        /// Failed To Start
        /// </summary>
        public const string FailedOnStart = "Failed on Start";
        /// <summary>
        /// In Progress
        /// </summary>
        public const string InProgress = "In Progress";
        /// <summary>
        /// Error Occured
        /// </summary>
        public const string ErrorOccured = "Error Occured";
        /// <summary>
        /// Canceled
        /// </summary>
        public const string Canceled = "Canceled";
        /// <summary>
        /// Completed
        /// </summary>
        public const string Completed = "Completed";
        /// <summary>
        /// Failed on Start(retrying)
        /// </summary>
        public const string FailedOnStartRetrying = "Failed on Start(retrying)";
        /// <summary>
        /// Error Occured(retrying)
        /// </summary>
        public const string ErrorOccuredRetrying = "Error Occured(retrying)";
        /// <summary>
        /// Approved
        /// </summary>
        public const string Approved = "Approved";
        /// <summary>
        /// Rejected
        /// </summary>
        public const string Rejected = "Rejected";
        #endregion

    }
    #region Aggregate Funcions
    /// <summary>
    /// This will hold Aggegate funtions
    /// </summary>
    public enum Aggregate
    {
        /// <summary>
        /// No aggregate function
        /// </summary>
        NONE,
        /// <summary>
        /// Count aggregate function
        /// </summary>
        COUNT,
        /// <summary>
        /// Average
        /// </summary>
        AVERAGE,
        /// <summary>
        /// Maximum
        /// </summary>
        MAXIMUM,
        /// <summary>
        /// Minimum
        /// </summary>
        MINIMUM,
        /// <summary>
        /// Sum
        /// </summary>
        SUM,
        /// <summary>
        /// Standard Deviation
        /// </summary>
        STDEV,
        /// <summary>
        /// Variance
        /// </summary>
        VAR
    }
    #endregion
    #region Aggregate Functions
    /// <summary>
    /// This enum will store logical operators {And, Or, None = where NONE is no logical operator}
    /// </summary>
    public enum Operator
    {
        /// <summary>
        /// No logical Operator means you filtering with one column
        /// </summary>
        NONE,
        /// <summary>
        /// And
        /// </summary>
        AND,
        /// <summary>
        /// Or
        /// </summary>
        OR

    }
    #endregion
    #region SharePoint Data Types
    /// <summary>
    /// These are the Data Types that will be passed by the user
    /// </summary>
    public enum SharePointDataType
    {
        /// <summary>
        /// None Datatype
        /// </summary>
        NONE,
        /// <summary>
        /// Single line of text
        /// </summary>
        TEXT,
        /// <summary>
        /// Multiple lines of text
        /// </summary>
        NOTE,
        /// <summary>
        /// Choice(menu to choose from)
        /// </summary>
        CHOICE,
        /// <summary>
        /// Interger
        /// </summary>
        NUMBER,
        /// <summary>
        /// Money
        /// </summary>
        CURRENCY,
        /// <summary>
        /// Date and Time
        /// </summary>
        DATETIME,
        /// <summary>
        /// Lookup Datat Type
        /// </summary>
        LOOKUP,
        /// <summary>
        /// Counter Data Type(usually primary key [ID]).
        /// </summary>
        COUNTER,
        /// <summary>
        /// Sharepoint Workflow Status
        /// </summary>
        WORKFLOWSTATUS

    }
    #endregion
    #region Logical Operators 
    /// <summary>
    /// This enum will store Conditions for the query
    /// </summary>
    public enum Condition
    {
        /// <summary>
        /// None Condition
        /// </summary>
        None,
        /// <summary>
        /// Eq(=)
        /// </summary>
        EqualTo,
        /// <summary>
        /// Neq(!=)
        /// </summary>
        NotEqaulTo,
        /// <summary>
        /// 
        /// </summary>
        GreaterThan,
        /// <summary>
        /// Lq(Less Than)
        /// </summary>
        LessThan,
        /// <summary>
        /// Geq (>=)
        /// </summary>
        GreaterThanOrEqualTo,
        /// <summary>
        /// Leq (Less Than Or Equal To)
        /// </summary>
        LessThanOrEqualTo,
        /// <summary>
        /// Begins With a value
        /// </summary>
        BeginsWith,
        /// <summary>
        /// Contains a value
        /// </summary>
        Contains,
        /// <summary>
        /// Value is not null
        /// </summary>
        IsNotNull,
        /// <summary>
        /// Value is Null
        /// </summary>
        IsNull,
        /// <summary>
        /// In operator IN(value1, value2,value3)
        /// </summary>
        In
    }
    /// <summary>
    /// This enum will specify a join Types where it LEFT,INNER
    /// </summary>
    public enum JoinType
    {
        /// <summary>
        /// Left JOIN
        /// </summary>
        LEFT,
        /// <summary>
        /// Inner Join
        /// </summary>
        INNER,
        /// <summary>
        /// None Join
        /// </summary>
        NONE
    }
    #endregion
    #region Workflow Statuses
    /// <summary>
    /// Sharepoint Workflow Statuses
    /// </summary>
    public enum WorkflowStatus
    {
        /// <summary>
        /// No status set
        /// </summary>
        None,
        /// <summary>
        /// Not Started
        /// </summary>
        NotStarted,
        /// <summary>
        /// Failed on Start
        /// </summary>
        FailedOnStart,
        /// <summary>
        /// In Progress
        /// </summary>
        InProgress,
        /// <summary>
        /// Error Occured
        /// </summary>
        ErrorOccured,
        /// <summary>
        /// Cancelled
        /// </summary>
        Cancelled,
        /// <summary>
        /// Completed
        /// </summary>
        Completed,
        /// <summary>
        /// Failed on Start(retrying)
        /// </summary>
        FailedOnStartRetrying,
        /// <summary>
        /// Error Occured(retrying)
        /// </summary>
        ErrorOccuredRetrying,
        /// <summary>
        /// Approved
        /// </summary>
        Approved,
        /// <summary>
        /// Rejected
        /// </summary>
        Rejected

    }
    #endregion
    #region Workflow Events
    /// <summary>
    /// This enum will hold sharepoint event types
    /// </summary>
    public enum WorkflowEvent
    {
        /// <summary>
        /// None 
        /// </summary>
        NONE,
        /// <summary>
        /// Task Completed
        /// </summary>
        TASKCOMPLETED,
        /// <summary>
        /// Task Created
        /// </summary>
        TASKCREATED,
        /// <summary>
        /// Task Deleted
        /// </summary>
        TASKDELETED,
        /// <summary>
        /// Task Modified
        /// </summary>
        TASKMODIFIED,
        /// <summary>
        /// Task Rolled Back
        /// </summary>
        TASKROLLEDBACK,
        /// <summary>
        /// Workflow Cancelled
        /// </summary>
        WORKFLOWCANCELLED,
        /// <summary>
        /// Workflow Comment
        /// </summary>
        WORKFLOWCOMMENT,
        /// <summary>
        /// Workflow Deleted
        /// </summary>
        WORKFLOWDELETED,
        /// <summary>
        /// Workflow Error
        /// </summary>
        WORKFLOWERROR,
        /// <summary>
        /// Workflow started
        /// </summary>
        WORKFLOWSTARTED,
        /// <summary>
        /// Workflow Completed
        /// </summary>
        WORKFLOWCOMPLETED

    }
    #endregion
}
