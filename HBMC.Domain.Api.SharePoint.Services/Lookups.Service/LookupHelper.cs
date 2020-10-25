using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharePointHelper.CRUD;
using System.Configuration;
using System.Net;
using Nancy.Json;
using System.Web.Mvc;

namespace SharePointHelper.Lookups
{
    /// <summary>
    /// This class has generic methods for lookups
    /// </summary>
   public class LookupHelper
    {
        
        /// <summary>
        /// Check if the user exists in SharePoint group that matches the provided email address and group name
        /// </summary>
        /// <param name="groupName">NAne of the group</param>
        /// <param name="emailAddress">User email address</param>
        /// <param name="clientContext">Client Context</param>
        /// <returns>True or False</returns>
        public static bool UserExistsInSharePointGroup(string groupName, string emailAddress, ClientContext clientContext = null)
        {
            try
            {
                clientContext = GetClientContextFromAdminOrWindows(clientContext);
                using (clientContext)
                {
                    var user = clientContext.Web.EnsureUser(emailAddress);
                    clientContext.Load(user);
                    clientContext.ExecuteQuery();

                    var groups = user.Groups.GetByName(groupName);
                    clientContext.Load(groups);
                    clientContext.ExecuteQuery();
                    return true;
                }

            }
            catch
            {
                return false;
            }
        }
  
        private static int GetAggregateFromAnyContext(ClientContext clientContext, string listName, string FieldName, string aggregateFunction, string filterViewXml)
        {
            var AggregateResult = 0;
            var oList = clientContext
                                    .Web
                                    .Lists
                                    .GetByTitle(listName);
            ClientResult<string> Counter = oList.RenderListData(filterViewXml);
            clientContext.ExecuteQuery();
            JavaScriptSerializer JSSerializer = new JavaScriptSerializer();
            dynamic OP = JSSerializer.DeserializeObject(Counter.Value);
            dynamic[] objResult = OP["Row"];
            if(objResult != null && objResult.Count() > 0)
            {
                var aggregateField = string.Format("{0}.{1}", FieldName, aggregateFunction);
                var itemResult = objResult[0][aggregateField];
                AggregateResult = itemResult != null ? int.Parse(itemResult) : AggregateResult;
            }
            return AggregateResult;
        }
      
      
        /// <summary>
        /// Returns Client Context from Admin Credentials if the clientContext paramater is null else returns ClientContext
        /// </summary>
        /// <param name="clientContext">Client Context</param>
        /// <returns></returns>
        public static ClientContext GetClientContextFromAdminOrWindows(ClientContext clientContext = null)
            => clientContext == null ? GetClientContextFromAdminOrWindows()
                                     : clientContext;
    
        /// <summary>
        /// This returns the list of ListItems that matches the provided viewValues[the values that will be returned after join],
        /// </summary>
        /// <param name="clientContext">Client Context</param>
        /// <param name="viewValues">Fields that will be returned after join</param>
        /// <param name="projectedFields">The The looup table colums</param>
        /// <param name="joinParameters">Join Parameter list</param>
        /// <param name="parameterList">Filter list</param>
        /// <returns></returns>
        public static IEnumerable<ListItem> GetLeftOrInnerJoinedFilteredList(ClientContext clientContext, List<ViewValue> viewValues, List<ViewValue> projectedFields, List<JoinParameter> joinParameters, List<Parameter> parameterList = null)
        {
            if (joinParameters.Count < 2) throw new Exception("No join can be performed on 1 list, rather use GetAnyListData() from CRUD Operations");
            if (viewValues.Count == 0) throw new Exception("The retutn fields could not be null");
            if (joinParameters.Count(x => x.JoinType == JoinType.NONE) > 1) throw new Exception("NONE join type can only be one in a join");
            var mainTable = joinParameters.FirstOrDefault();
            if (mainTable == null) throw new Exception("Main List cannot be null");
            var oList = new List<ListItem>();
            var conditions = new List<string>();
            var viewFieldsQuery = string.Empty;
            var projectedFieldsQuery = string.Empty;
            var filterQuery = string.Empty;
            #region View Fields
            viewValues.ForEach(x => conditions.Add(string.Format("<FieldRef Name='{0}'/>", x.DisplayName)));
            viewFieldsQuery = string.Join(Environment.NewLine, conditions);
            conditions.Clear();
            #endregion
            #region Projected Fields
            projectedFields.ForEach(x =>
             conditions.Add(string.Format("<Field Name ='{0}' Type='Lookup' List='{1}' ShowField='{2}'/>",
                            GetDisplayName(viewValues, x),
                            x.ListName,
                           x.FieldName)));
            projectedFieldsQuery = string.Join(Environment.NewLine, conditions);
            conditions.Clear();
            #endregion
            var QueryStr = string.Empty;
            var JoinQuery = string.Empty;               
            joinParameters.Remove(mainTable);
            foreach(var item in joinParameters)
            {
                var currentIndex = joinParameters.FindIndex(x => x == item);
                var previousJoin = currentIndex == 0 ? mainTable : joinParameters[currentIndex - 1];
                JoinQuery += string.Format(@"<Join Type='{0}' ListAlias='{1}'>
                                         <Eq>
                                              <FieldRef Name='{2}' RefType='ID'/>
                                              <FieldRef List = '{1}' Name='ID'/>
                                         </Eq>
                                        </Join>",
                                      GetJoinType(previousJoin.JoinType),
                                      item.ListName,
                                      item.JoinField);               
                
            }
            var calmQuery = new CamlQuery();
            if (parameterList != null && parameterList.Count > 0)
                filterQuery = FilterByAnyField(parameterList).ViewXml.Replace("<View>", string.Empty).Replace("</View>", string.Empty);
            calmQuery.ViewXml = string.IsNullOrEmpty(filterQuery) 
                                ?   string.Format(@"<View>
                                                        <ViewFields>{0}</ViewFields>
                                                        <Joins>{1}</Joins>
                                                        <ProjectedFields>{2}</ProjectedFields>
                                                    </View>", 
                                                viewFieldsQuery,
                                                JoinQuery,
                                                projectedFieldsQuery)
                                                
                                  : string.Format(@"<View>
                                                        <ViewFields>{0}</ViewFields>
                                                        <Joins>{1}</Joins>
                                                        <ProjectedFields>{2}</ProjectedFields>
                                                        {3}
                                                    </View>",
                                                viewFieldsQuery,
                                                JoinQuery,
                                                projectedFieldsQuery,
                                                filterQuery);
            if(clientContext != null)
            {
                using (clientContext)
                {
                    var listCollection = clientContext
                                         .Web
                                         .Lists
                                         .GetByTitle(mainTable.ListName)
                                         .GetItems(calmQuery);
                    clientContext.Load(listCollection);
                    clientContext.ExecuteQuery();
                    if(listCollection != null && listCollection.Count > 0)
                    {
                        var notMainTableFields = viewValues.Where(x => x.ListName != mainTable.ListName)
                                                            .Select(p => p.DisplayName)
                                                            .ToList();
                        foreach (var item in listCollection)
                        {                           
                            notMainTableFields.ForEach(x => item[x] = GetLookupValue((FieldLookupValue)item[x]));
                            oList.Add(item);
                        }
                    }                
                }
            }
            return oList;
        }
        private static string GetDisplayName(List<ViewValue> viewValues, ViewValue projectedField)
            => viewValues.FirstOrDefault(x => x.FieldName == projectedField.FieldName && x.ListName == projectedField.ListName)?.DisplayName ?? string.Empty;



        /// <summary>
        /// Returns the user domain in UPPERCASE that matches the provided loginName eg domain\username
        /// </summary>
        /// <param name="loginName">Log in name eg domain\username</param>
        /// <returns></returns>
        public static string GetUserDomain(string loginName, int index = 0)
            => !string.IsNullOrEmpty(loginName) ?
                (!loginName.IndexOf("\\").Equals(-1) ? (loginName.Split('\\')[index]).ToUpper().ToString()
                                                        : string.Empty)
              : string.Empty;
        /// <summary>
        /// This method created a List of strings from Choice column in a list
        /// </summary>
        /// <param name="spContext">Client Context</param>
        /// <param name="listName">Name of the List</param>
        /// <param name="columnName">Choice column</param>
        /// <returns>List of strings</returns>
        public static List<string> GetChoiceColumnValues(ClientContext spContext, string listName, string columnName)
        {
            CRUDOperations.ValidateListName(listName);
            var genericList = new List<string>();
            var choiceValues = string.Empty;            
            if(spContext != null)
            {
                using (spContext)
                {                     
                    var list = spContext.Web.Lists.GetByTitle(listName);
                    Field choice = list.Fields.GetByInternalNameOrTitle(columnName);
                    FieldChoice fieldChoice = spContext.CastTo<FieldChoice>(choice);
                    spContext.Load(fieldChoice, f => f.Choices);
                    spContext.ExecuteQuery();
                    fieldChoice.Choices.GroupBy(field => field)
                                       .Select(field => field.FirstOrDefault())
                                       .ToList()
                                       .ForEach(field => genericList.Add(field));                                        
                }
            }            
            return genericList;
        }

        /// <summary>
        /// Returns a Generic SelectListItem list based on the provided listName, return fields, selected Value, selectedTest
        /// </summary>
        /// <param name="spContext">ASharePoint Client context</param>
        /// <param name="listName">Name of the list</param>
        /// <param name="returnFields">List of fields that need to be returned in a list</param>
        /// <param name="selectedValue">The value that will be selected on the lookup when you post to server</param>
        /// <param name="selectedText">The value you will see on the lookup when you select</param>
        /// <param name="filterList">Nullable filter list</param>
        /// <param name="_operator">Logical Operator</param>
        /// <returns></returns>
        public static List<SelectListItem> GetSharePointLookupList(ClientContext spContext, string listName, List<string> returnFields, string selectedValue, string selectedText, List<Parameter> filterList = null, Operator _operator = Operator.NONE)
        {
            CRUDOperations.ValidateListName(listName);
            var genericSelectList = new List<SelectListItem>();
            if (spContext != null)
            {
                using (spContext)
                {                   
                    var listItems = CRUDOperations.GetAnyListData(spContext, listName, filterList, selectedValue);
                    if (listItems != null)
                    {
                        foreach (var listItem in listItems)
                        {
                            if(!string.IsNullOrEmpty(listItem[selectedValue]?.ToString() ?? string.Empty) && !string.IsNullOrEmpty(listItem[selectedText]?.ToString() ?? string.Empty))
                            {
                                if (genericSelectList.Count(g => g.Value.Trim() == (listItem[selectedValue].ToString()).Trim() && g.Text.Trim() == (listItem[selectedText].ToString())) == 0)
                                {
                                    genericSelectList.Add(new SelectListItem
                                    {
                                        Text = listItem[selectedText].ToString(),
                                        Value = listItem[selectedValue].ToString()
                                    });
                                }
                            }                           
                        }
                    }
                }
            }
            return genericSelectList;
        }
        
     
        /// <summary>
        /// Returns the list for fields in a caml query format that matches the provided lookup fields
        /// </summary>
        /// <param name="fieldNames">List of Lookup fields</param>
        /// <returns>CamlQuery lookup fields</returns>
        public static CamlQuery GetAllItems(List<string> fieldNames)
        {
            var camlQuery = new CamlQuery();
            var fields = new List<string>();
            if (fieldNames != null && fieldNames.Count > 0)
            {
                fieldNames.ForEach(field => fields.Add(string.Format("<FieldRef Name='{0}' Ascending='TRUE'/>", field)));
                camlQuery.ViewXml = string.Format(@"<Query>                                                    
                                                       {0}                                                              
                                                    </Query>", string.Join(Environment.NewLine, fields));
            }
            return camlQuery;
        }
        private static void SetWorkflowStatusConditionToIn(List<Parameter> parameterList)
        {
            foreach(var parameter in parameterList.Where(x => x.ParameterType == SharePointDataType.WORKFLOWSTATUS))
            {
                if(parameter.WorkflowStatus == WorkflowStatus.None && !new[] { Condition.IsNotNull, Condition.IsNull }.Contains(parameter.Condition))
                    throw new Exception("Workflow Status cannot be none when you have IsNotNull or IsNull as the condition");
                if(parameter.WorkflowStatus == WorkflowStatus.Cancelled)
                {
                    if(parameter.Condition == Condition.EqualTo)
                    {
                        parameter.Condition = Condition.In;
                        parameter.InValues = GetWorkflowStatusValue(parameter.WorkflowStatus);
                    }
                    else
                    {
                        parameter.ParameterValue = GetWorkflowStatusValue(parameter.WorkflowStatus).FirstOrDefault();
                    }                   
                }
                else
                {
                    parameter.ParameterValue = GetWorkflowStatusValue(parameter.WorkflowStatus).FirstOrDefault();
                }
                
            }        
        }
        
        /// <summary>
        /// Returns a caml query that matches the provided filter by parameterList
        /// </summary>
        /// <param name="parameterList">Filter parameter</param>
        /// <returns></returns>
        public static CamlQuery FilterByAnyField(List<Parameter> parameterList)
        {
            var camlQuery = new CamlQuery();
            var conditions = new List<string>();
            if (parameterList == null || parameterList.Count == 0) throw new ArgumentException("The filter cannot empty");
            if (parameterList.Count(x => x.Operator == Operator.NONE) > 1) throw new ArgumentException("NONE Operator can not be more that one in a query, only expecting NONE operator to be one(which will be the last item in the parameter list because there is no more filter)");
            var numberOfItems = parameterList.Count;
            //Set Workflow 
            SetWorkflowStatusConditionToIn(parameterList);       
            var defaultQuery = "<{0}><FieldRef Name='{1}'/><Value Type='{2}'>{3}</Value></{4}>";
            var NotOrNullQuery = "<{0}><FieldRef Name='{1}'/></{2}>";
            if (numberOfItems <= 2)
            {
                #region Items less than or equal to two
                if (numberOfItems == 2)//Two items on the parameterList
                {
                    parameterList.ForEach(parameter => conditions.Add(new[] { Condition.IsNull, Condition.IsNotNull }.Contains(parameter.Condition)
                                                                            ? string.Format(NotOrNullQuery,
                                                                                            GetCondition(parameter.Condition),
                                                                                            parameter.ParameterName,
                                                                                            GetCondition(parameter.Condition))
                                                                            : parameter.Condition == Condition.In
                                                                            ? GetInOperatorCamlQuery(parameter)                                                                            
                                                                            : string.Format(defaultQuery,
                                                                                            GetCondition(parameter.Condition),
                                                                                            parameter.ParameterName,
                                                                                            GetSharePointDataType(parameter.ParameterType),
                                                                                            parameter.ParameterValue,
                                                                                            GetCondition(parameter.Condition))));      
                    var firstItemOperator = parameterList.FirstOrDefault()?.Operator ?? Operator.NONE;
                    var query = firstItemOperator != Operator.NONE
                                ? string.Format("<{0}>{1}</{2}>", GetLogicalOperator(firstItemOperator),
                                                                  string.Join(Environment.NewLine, conditions),
                                                                  GetLogicalOperator(firstItemOperator))
                                : string.Empty;
                    if (string.IsNullOrEmpty(query)) throw new ArgumentException("Your first logical operator cannot be NONE becuase you using more than one condition");
                    conditions.Clear();
                    conditions.Add(query);
                }
                else
                {
                    parameterList.ForEach(parameter => conditions.Add(new[] { Condition.IsNull, Condition.IsNotNull }.Contains(parameter.Condition)
                                                                           ? string.Format(NotOrNullQuery,
                                                                                         GetCondition(parameter.Condition),
                                                                                         parameter.ParameterName,
                                                                                         GetCondition(parameter.Condition))
                                                                           : parameter.Condition == Condition.In
                                                                           ? GetInOperatorCamlQuery(parameter)
                                                                           : string.Format(defaultQuery,
                                                                                            GetCondition(parameter.Condition),
                                                                                            parameter.ParameterName,
                                                                                            GetSharePointDataType(parameter.ParameterType),
                                                                                            parameter.ParameterValue,
                                                                                            GetCondition(parameter.Condition))));
                }
                #endregion
            }
            else// More than two items in a list
            {
                #region Items greater that 2
                var firstList = parameterList.Take(2).ToList();// Take first 2 for making the first conditon
                if (firstList == null || firstList.Count == 0) throw new ArgumentException("Your first 2 items cannot be empty on the list");
                var firstItemOperator = firstList.FirstOrDefault()?.Operator ?? Operator.NONE;
                var secondItemOperator = firstList.LastOrDefault()?.Operator ?? Operator.NONE;
                var lastItemOperator = parameterList.LastOrDefault()?.Operator ?? Operator.NONE;
                var previousOperator = Operator.NONE;
                if (firstItemOperator == Operator.NONE) throw new ArgumentException("If you have more than 2 items in the parameter list your logical operator cannot be NONE");
                if (secondItemOperator == Operator.NONE) throw new ArgumentException("If you have more than 2 items in the paramater list your logical operator cannot be NONE");
                if (lastItemOperator != Operator.NONE) throw new ArgumentException("Last item on the parameter list can only have Operator NONE");
                firstList.ForEach(first => conditions.Add(new[] { Condition.IsNotNull, Condition.IsNull }.Contains(first.Condition)
                                                                ? string.Format(NotOrNullQuery,
                                                                                GetCondition(first.Condition),
                                                                                first.ParameterName,
                                                                                GetCondition(first.Condition))
                                                                : first.Condition == Condition.In
                                                                ? GetInOperatorCamlQuery(first)
                                                                : string.Format(defaultQuery,
                                                                                GetCondition(first.Condition),
                                                                                first.ParameterName,
                                                                                GetSharePointDataType(first.ParameterType),
                                                                                first.ParameterValue,
                                                                                GetCondition(first.Condition))));                           
                firstList.ForEach(first => parameterList.Remove(first));//Remove the first two items on the default list
                var firstCondition = conditions.Count > 0 ? string.Format("<{0}>{1}</{2}>", GetLogicalOperator(firstItemOperator), string.Join(Environment.NewLine, conditions), GetLogicalOperator(firstItemOperator))
                                                           : string.Empty;
                if (string.IsNullOrEmpty(firstCondition)) throw new ArgumentException("You first condition cannot be null");
                conditions.Clear();
                conditions.Add(firstCondition);                
                foreach (var parameter in parameterList)//Starting from the third
                {
                    previousOperator = secondItemOperator != Operator.NONE ? secondItemOperator : previousOperator;
                    var finalQuery = string.Empty;

                    var innerQuery = new[] { Condition.IsNull, Condition.IsNotNull }.Contains(parameter.Condition)
                                     ? string.Format(NotOrNullQuery,
                                                     GetCondition(parameter.Condition),
                                                     parameter.ParameterName,
                                                     GetCondition(parameter.Condition))
                                    : parameter.Condition == Condition.In
                                    ? GetInOperatorCamlQuery(parameter)
                                    : string.Format(defaultQuery,
                                                    GetCondition(parameter.Condition),
                                                    parameter.ParameterName,
                                                    GetSharePointDataType(parameter.ParameterType),
                                                    parameter.ParameterValue,
                                                    GetCondition(parameter.Condition));                    
                    if (secondItemOperator != Operator.NONE)
                    {
                        finalQuery = string.Format("<{0}>{1}{2}</{3}>",
                                                   GetLogicalOperator(secondItemOperator),
                                                   string.Join(Environment.NewLine, conditions),
                                                   innerQuery,
                                                   GetLogicalOperator(secondItemOperator));
                        //clear  the list
                        conditions.Clear();
                        //Add final query to the list
                        conditions.Add(finalQuery);                       
                        secondItemOperator = Operator.NONE;
                    }
                    else
                    {
                        if (parameter.Operator == lastItemOperator)
                        {
                            finalQuery = string.Format("<{0}>{1}{2}</{3}>",
                                                      GetLogicalOperator(previousOperator),
                                                      string.Join(Environment.NewLine, conditions),
                                                      innerQuery,
                                                      GetLogicalOperator(previousOperator));
                            //clear the list
                            conditions.Clear();
                            //Add updated query to clist
                            conditions.Add(finalQuery);
                        }
                        else
                        {
                            finalQuery = string.Format("<{0}>{1}{2}</{3}>",
                                                      GetLogicalOperator(previousOperator),
                                                      string.Join(Environment.NewLine, conditions),
                                                      innerQuery,
                                                      GetLogicalOperator(previousOperator));
                            previousOperator = parameter.Operator;
                            //clear the list
                            conditions.Clear();
                            //Add updated query to clist
                            conditions.Add(finalQuery);
                        }
                    }

                }
                #endregion

            }
            camlQuery.ViewXml = string.Format(@"<View>
                                                      <Query>
                                                             <Where>
                                                                {0}
                                                             </Where>
                                                      </Query>
                                                </View>", string.Join(Environment.NewLine, conditions));
            return camlQuery;
        }
               
        private static string GetInOperatorCamlQuery(Parameter parameter)
        {
            if (parameter.Condition == Condition.In && parameter.InValues.Count < 1)
                throw new Exception("The IN Operator must have list of values");
            var fieldQuery = string.Empty;
            var values = new List<string>();           
            fieldQuery = string.Format("<FieldRef Name='{0}'/>", parameter.ParameterName);
            parameter.InValues.ForEach(item => values.Add(string.Format(@"<Value Type='{0}'>{1}</Value>",
                                                                             GetSharePointDataType(parameter.ParameterType),
                                                                             item)));            
            return string.Format(@"<In>{0}<Values>{1}</Values></In>", 
                                   fieldQuery,
                                   string.Join(Environment.NewLine, values));
        }
        /// <summary>
        /// This method filter by any field that matches the provided Parameter object and Operator enum, If you passing one column in Parameter List then your Operator enum is NONE
        /// </summary>
        /// <param name="parameters">Parameter Values</param>
        /// <param name="_operator">Operator Enum</param>
        /// <returns>Calm Query Filter</returns>
        public static CamlQuery FilterByAnyField(List<Parameter> parameters, Operator _operator)
        {
            var camlQuery = new CamlQuery();
            var conditions = new List<string>();
            if(parameters != null && parameters.Count > 0)
            {
                if(parameters.Count() <= 2)// 2 filters or less
                {
                    #region Items less than or equal to 2
                    if (parameters.Count() == 2)// 2 conditions only
                    {
                        parameters.ForEach(parameter => conditions.Add(string.Format("<{0}><FieldRef Name='{1}'/><Value Type='{2}'>{3}</Value></{4}>",
                                                                           GetCondition(parameter.Condition),
                                                                           parameter.ParameterName,
                                                                           GetSharePointDataType(parameter.ParameterType),
                                                                           parameter.ParameterValue,
                                                                           GetCondition(parameter.Condition))));
                        var query = string.Format("<{0}>{1}</{2}>", GetLogicalOperator(_operator), string.Join(Environment.NewLine, conditions), GetLogicalOperator(_operator));
                        conditions.Clear();
                        conditions.Add(query);
                    }
                    else
                    {
                        parameters.ForEach(parameter => conditions.Add(string.Format("<{0}><FieldRef Name='{1}'/><Value Type='{2}'>{3}</Value></{4}>",
                                                                           GetCondition(parameter.Condition),
                                                                           parameter.ParameterName,
                                                                           GetSharePointDataType(parameter.ParameterType),
                                                                           parameter.ParameterValue,
                                                                           GetCondition(parameter.Condition))));
                    }
                    #endregion
                }
                else // more than tow items in the Parameter
                {
                    var firstList = parameters.Take(2).ToList();// Take the first 2
                    firstList.ForEach(first => conditions.Add(string.Format("<{0}><FieldRef Name='{1}'/><Value Type='{2}'>{3}</Value></{4}>",
                                                                            GetCondition(first.Condition),
                                                                            first.ParameterName,
                                                                            GetSharePointDataType(first.ParameterType),
                                                                            first.ParameterValue,
                                                                            GetCondition(first.Condition))));
                    //Remove items that has been added to conditions
                    firstList.ForEach(first => parameters.Remove(first));
                    var firstCondition = conditions.Count > 0 ? string.Format("<{0}>{1}</{2}>", GetLogicalOperator(_operator), string.Join(Environment.NewLine, conditions), GetLogicalOperator(_operator)) 
                                                              : string.Empty;
                    conditions.Clear();
                    if (!string.IsNullOrEmpty(firstCondition))
                        conditions.Add(firstCondition);
                    foreach(var param  in parameters)
                    {
                       var finalQuery = string.Empty;
                        var innerQuery = string.Format("<{0}><FieldRef Name='{1}'/><Value Type='{2}'>{3}</Value></{4}>",
                                                                            GetCondition(param.Condition),
                                                                            param.ParameterName,
                                                                            GetSharePointDataType(param.ParameterType),
                                                                            param.ParameterValue,
                                                                            GetCondition(param.Condition));
                        finalQuery = string.Format("<{0}>{1}{2}</{3}>",
                                                   GetLogicalOperator(_operator),
                                                   string.Join(Environment.NewLine, conditions),
                                                   innerQuery,
                                                   GetLogicalOperator(_operator));
                        //Clear the list
                        conditions.Clear();
                        conditions.Add(finalQuery);
                    }
                }
                
                //Check if the parameter list length is 1 and operator is not NONE
                //If parameter list length is 1 and operator is not null then set logical Operator to NONE because if parameter list has one item we don't need to have logical operator
                //var logicalOperator = parameters.Count == 1 && _operator != Operator.NONE ? string.Empty : GetLogicalOperator(_operator);

                camlQuery.ViewXml = string.Format(@"<View>
                                                          <Query>
                                                                  <Where>
                                                                         {0}
                                                                  </Where>
                                                          </Query>
                                                    </View>", string.Join(Environment.NewLine, conditions));
            }
            return camlQuery;
        }
        /// <summary>
        /// Returns any list data that matches the provided listName,ReturnAllFields(if true then return everything on the list else return specified fields on the list), returnFields(the fields that will be returned), filterList(filter on the list), _operator (logical operator(AND,OR,NONE)), orderByField(The field for ordering the list)
        /// </summary>
        /// <param name="spContext">Client Context</param>
        /// <param name="listName">Name of the list</param>
        /// <param name="returnAllFields">Are all fields returned</param>
        /// <param name="returnFileds">List of fields to be returned</param>
        /// <param name="filterList">Filter paramater for the list</param>
        /// <param name="_operator">Logical Operator</param>
        /// <param name="orderByField"> Order by field</param>
        /// <returns></returns>
        public static IEnumerable<ListItem> GetAnyListData(ClientContext spContext, string listName, bool returnAllFields, List<string> returnFileds = null, List<Parameter> filterList = null, Operator _operator = Operator.NONE, string orderByField = null)
        {
            var listItems = new List<ListItem>();
            //If returnFields is true and orderBy is null throw exception because when you want to return the whole list you need to specify the order of the list
            if (returnAllFields && string.IsNullOrEmpty(orderByField)) throw new ArgumentException("If you want to return all fields specify the column that will be orderd with parameter [orderByField]");
            //If returnFields is false and returnFields is null then throw exception becuase you can't say you not returning all fields while the return field is null
            if (!returnAllFields && returnFileds == null) throw new ArgumentException("If returnAllFields is false and returnFields is null specify which fields should be returned");
            
             if(spContext != null)
            {
                using (spContext)
                {
                    var list = spContext.Web.Lists.GetByTitle(listName);
                    var camlQuery = new CamlQuery();
                    ListItemCollection listItemCollection = null;
                    if (returnAllFields)// Return all fields
                    {
                        camlQuery.ViewXml = filterList == null   ?  string.Format(@"<Query><OrderBy><FieldRef Name = '{0}'/></OrderBy></Query>", orderByField)
                                                                 : FilterByAnyField(filterList, _operator).ViewXml;


                        listItemCollection = list.GetItems(camlQuery);
                        spContext.Load(listItemCollection);
                        spContext.ExecuteQuery();
                        if(listItemCollection != null)                        
                            listItemCollection.ToList().ForEach(item => listItems.Add(item));                        
                    }
                    else // Return specific fields
                    {
                        camlQuery.ViewXml = filterList == null ? GetAllItems(returnFileds).ViewXml
                                                               : FilterByAnyField(filterList, _operator).ViewXml;
                        listItemCollection = list.GetItems(camlQuery);
                        spContext.Load(listItemCollection);
                        spContext.ExecuteQuery();
                    }
                }
            }
            return listItems;
        }

        /// <summary>
        /// Returns a lookup value from lookup field
        /// </summary>
        /// <param name="value">lookup field</param>
        /// <returns>Lookup Value</returns>
        public static string GetLookupValue(FieldLookupValue value) => (value?.LookupValue ?? string.Empty).Trim();
        /// <summary>
        /// Returns the LookupValue from FieldLookupValue
        /// </summary>
        /// <param name="value">FieldUserLookup value</param>
        /// <returns>Lookup value</returns>
        public static string GetUserFieldValue(FieldUserValue value) => (value?.LookupValue ?? string.Empty).Trim();
        /// <summary>
        /// Returns a Sharepoint Date format
        /// </summary>
        /// <param name="value">string object</param>
        /// <returns></returns>
        public static string GetSharePointDateFormat(object value)
        {
            if (value == null) return null;
            return DateTime.Parse(value.ToString()).ToString("yyyy-MM-ddThh:mm:ssZ");
        }

        private static List<string> GetWorkflowStatusValue(WorkflowStatus _workflowStatus)
        {
            var values = new List<string>();
            var cancelled = new List<string> { "4", "15" };
            switch (_workflowStatus)
            {
                case WorkflowStatus.NotStarted:
                    values.Add("0");
                    break;
                case WorkflowStatus.FailedOnStart:
                    values.Add("1");
                    break;
                case WorkflowStatus.InProgress:
                    values.Add("2");
                    break;
                case WorkflowStatus.ErrorOccured:
                    values.Add("3");
                    break;
                case WorkflowStatus.Cancelled:
                    cancelled.ForEach(cancel => values.Add(cancel));
                    break;
                case WorkflowStatus.Completed:
                    values.Add("5");
                    break;
                case WorkflowStatus.FailedOnStartRetrying:
                    values.Add("6");
                    break;
                case WorkflowStatus.ErrorOccuredRetrying:
                    values.Add("7");
                    break;
                case WorkflowStatus.Approved:
                    values.Add("16");
                    break;
                case WorkflowStatus.Rejected:
                    values.Add("17");
                    break;
                default:
                    values = new List<string>();
                    break;                    
            }
            return values;
        }
        #region GetWorkflow Status Text  
        /// <summary>
        /// Returns the workflow status text that matches the provided status value(encoded number)
        /// </summary>
        /// <param name="statusValue">Encoded Sharepoint Workflow Status value</param>
        /// <returns>Status Text</returns>
                  
        public static string GetWorkflowStatusText(int statusValue)
        {
            if (statusValue == -1) return string.Empty;
            return statusValue.Equals(0) ? SharePointStringHelper.NotStarted
                   : statusValue.Equals(1) ? SharePointStringHelper.FailedOnStart
                   : statusValue.Equals(2) ? SharePointStringHelper.InProgress
                   : new[] { 4, 15 }.Contains(statusValue) ? SharePointStringHelper.Canceled
                   : statusValue.Equals(3) ? SharePointStringHelper.ErrorOccured
                   : statusValue.Equals(5) ? SharePointStringHelper.Completed
                   : statusValue.Equals(6) ? SharePointStringHelper.FailedOnStartRetrying
                   : statusValue.Equals(7) ? SharePointStringHelper.ErrorOccuredRetrying
                   : statusValue.Equals(16) ? SharePointStringHelper.Approved
                   : statusValue.Equals(17) ? SharePointStringHelper.Rejected
                   : string.Empty;
        }
        #endregion
        #region Get Workflow Event Text and Number by enum
        /// <summary>
        /// Returns a dictionary contaning Event Number and Event display text that matches the provied Event Enum
        /// </summary>
        /// <param name="WEvent">Event Enum</param>
        /// <returns>Event Dictionary</returns>
        public static Dictionary<int, string> GetWorkflowEvents(WorkflowEvent WEvent)
        {
            var workflowEvents = new Dictionary<int, string>();
            switch (WEvent)
            {                
                case WorkflowEvent.TASKCOMPLETED:
                    workflowEvents.Add(6, SharePointStringHelper.TaskCompleted);
                    break;
                case WorkflowEvent.TASKCREATED:
                    workflowEvents.Add(5, SharePointStringHelper.TaskCreated);
                    break;
                case WorkflowEvent.TASKDELETED:
                    workflowEvents.Add(9, SharePointStringHelper.TaskDeleted);
                    break;
                case WorkflowEvent.TASKMODIFIED:
                    workflowEvents.Add(7, SharePointStringHelper.TaskModified);
                    break;
                case WorkflowEvent.TASKROLLEDBACK:
                    workflowEvents.Add(8, SharePointStringHelper.TaskRolledBack);
                    break;
                case WorkflowEvent.WORKFLOWCANCELLED:
                    workflowEvents.Add(3, SharePointStringHelper.WorkflowCancelled);
                    break;
                case WorkflowEvent.WORKFLOWCOMMENT:
                    workflowEvents.Add(11, SharePointStringHelper.WorkflowComment);
                    break;
                case WorkflowEvent.WORKFLOWCOMPLETED:
                    workflowEvents.Add(2, SharePointStringHelper.WorkflowCompleted);
                    break;
                case WorkflowEvent.WORKFLOWDELETED:
                    workflowEvents.Add(4, SharePointStringHelper.WorkflowDeleted);
                    break;
                case WorkflowEvent.WORKFLOWERROR:
                    workflowEvents.Add(10, SharePointStringHelper.WorkflowError);
                    break;
                case WorkflowEvent.WORKFLOWSTARTED:
                    workflowEvents.Add(1, SharePointStringHelper.WorkflowStarted);
                    break;
                default:
                    workflowEvents.Add(0, SharePointStringHelper.None);
                    break;
            }
            return workflowEvents;
        }
        #endregion
        #region Get Event Enum by Event Number
        /// <summary>
        /// Returns the even enum from event number
        /// </summary>
        /// <param name="eventNumber">Event Number</param>
        /// <returns>Event Enum</returns>
        public static string GetEventText(int eventNumber)
        {
            return eventNumber.Equals(6) ? SharePointStringHelper.TaskCompleted
                   : eventNumber.Equals(5) ? SharePointStringHelper.TaskCreated
                   : eventNumber.Equals(9) ? SharePointStringHelper.TaskDeleted
                   : eventNumber.Equals(7) ? SharePointStringHelper.TaskModified
                   : eventNumber.Equals(8) ? SharePointStringHelper.TaskRolledBack
                   : eventNumber.Equals(3) ? SharePointStringHelper.WorkflowCancelled
                   : eventNumber.Equals(11) ? SharePointStringHelper.WorkflowComment
                   : eventNumber.Equals(2) ? SharePointStringHelper.WorkflowCompleted
                   : eventNumber.Equals(4) ? SharePointStringHelper.WorkflowDeleted
                   : eventNumber.Equals(10) ? SharePointStringHelper.WorkflowError
                   : eventNumber.Equals(1) ? SharePointStringHelper.WorkflowStarted
                   : string.Empty;
        }
         
        #endregion
        private static string GetSharePointDataType(SharePointDataType _sharePointDataType)
        {
            var returnedDataType = string.Empty;
            switch (_sharePointDataType)
            {
                case SharePointDataType.TEXT:
                    returnedDataType = SharePointStringHelper.Text;
                    break;
                case SharePointDataType.NOTE:
                    returnedDataType = SharePointStringHelper.Note;
                    break;
                case SharePointDataType.CHOICE:
                    returnedDataType = SharePointStringHelper.Choice;
                    break;
                case SharePointDataType.NUMBER:
                    returnedDataType = SharePointStringHelper.Number;
                    break;
                case SharePointDataType.CURRENCY:
                    returnedDataType = SharePointStringHelper.Currency;
                    break;
                case SharePointDataType.DATETIME:
                    returnedDataType = SharePointStringHelper.DateTime;
                    break;
                case SharePointDataType.LOOKUP:
                    returnedDataType = SharePointStringHelper.Lookup;
                    break;
                case SharePointDataType.COUNTER:
                    returnedDataType = SharePointStringHelper.Counter;
                    break;
                case SharePointDataType.WORKFLOWSTATUS:
                    returnedDataType = SharePointStringHelper.WorkflowStatus;
                    break;           
                default:
                    returnedDataType = string.Empty;
                    break;
            }
            return returnedDataType; 
        }
        private static string GetJoinType(JoinType type)
        {
            var joinType = string.Empty;
            switch (type)
            {
                case JoinType.INNER:
                    joinType = SharePointStringHelper.InnerJoin;
                    break;
                case JoinType.LEFT:
                    joinType = SharePointStringHelper.LeftJoin;
                    break;
                default:
                    joinType = string.Empty;
                    break;
            }
            return joinType;
        }

        private static string GetLogicalOperator(Operator _operator)
        {
            var returnedOperator = string.Empty;
            switch (_operator)
            {
                case Operator.AND:
                    returnedOperator = SharePointStringHelper.And;
                    break;
                case Operator.OR:
                    returnedOperator = SharePointStringHelper.Or;
                    break;
                default:
                    returnedOperator = string.Empty;
                    break;
            }
            return returnedOperator;
        }
        private static string GetCondition(Condition condition)
        {
            var returnedCondition = string.Empty;
            switch (condition)
            {
                case Condition.EqualTo:
                    returnedCondition = SharePointStringHelper.EqualTo;
                    break;
                case Condition.NotEqaulTo:
                    returnedCondition = SharePointStringHelper.NoEqualTo;
                    break;
                case Condition.GreaterThan:
                    returnedCondition = SharePointStringHelper.GreaterThan;
                    break;
                case Condition.LessThan:
                    returnedCondition = SharePointStringHelper.LessThan;
                    break;
                case Condition.GreaterThanOrEqualTo:
                    returnedCondition = SharePointStringHelper.GreaterThanOrEqualTo;
                    break;
                case Condition.LessThanOrEqualTo:
                    returnedCondition = SharePointStringHelper.LessThanOrEqualTo;
                    break;
                case Condition.BeginsWith:
                    returnedCondition = SharePointStringHelper.BeginsWith;
                    break;
                case Condition.Contains:
                    returnedCondition = SharePointStringHelper.Contains;
                    break;
                case Condition.IsNotNull:
                    returnedCondition = SharePointStringHelper.IsNotNull;
                    break;
                case Condition.IsNull:
                    returnedCondition = SharePointStringHelper.IsNull;
                    break;
                case Condition.In:
                    returnedCondition = SharePointStringHelper.In;
                    break;
                default:
                    returnedCondition = string.Empty;
                    break;
            }
            return returnedCondition;
        }
        /// <summary>
        /// Returns aggregate function in string that matches the provided aggregate enum
        /// </summary>
        /// <param name="aggregate">Aggregate function</param>
        /// <returns>Aggregate function in String</returns>
        private static string GetAggregateFunction(Aggregate aggregate)
        {
            var returnedAggregate = string.Empty;
            switch (aggregate)
            {
                case Aggregate.COUNT:
                    returnedAggregate = SharePointStringHelper.Count;
                    break;
                case Aggregate.AVERAGE:
                    returnedAggregate = SharePointStringHelper.Average;
                    break;
                case Aggregate.MAXIMUM:
                    returnedAggregate = SharePointStringHelper.Maximum;
                    break;
                case Aggregate.MINIMUM:
                    returnedAggregate = SharePointStringHelper.Minimum;
                    break;
                case Aggregate.SUM:
                    returnedAggregate = SharePointStringHelper.Sum;
                    break;
                case Aggregate.STDEV:
                    returnedAggregate = SharePointStringHelper.StandardDeviation;
                    break;
                case Aggregate.VAR:
                    returnedAggregate = SharePointStringHelper.Variance;
                    break;
                default:
                    returnedAggregate = "NONE";
                    break;
            }
            return returnedAggregate;
        }


    }
}
