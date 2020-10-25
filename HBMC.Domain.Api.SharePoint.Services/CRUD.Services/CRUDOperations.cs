using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint;
using SharePointHelper.Lookups;
using System.Net;
using Newtonsoft.Json;


namespace SharePointHelper.CRUD
{
    /// <summary>
    /// This class has static methods for CRUD(Create,Read,Update,Delete)
    /// </summary>
   public class CRUDOperations
    {
       
        /// <summary>
        /// Returns a list of items that matches the provided filter and pageSize 
        /// </summary>
        /// <param name="listName">Name of the list</param>
        /// <param name="parameterList">Filter parameters</param>
        /// <param name="pageSize">Page Size [eg. 10 items]</param>
        /// <param name="OrderByField">Field for Order</param>
        /// <param name="Ascending">Sorting of the list[Ascending=True or False]</param>
        /// <param name="clientContext">Client Context</param>
        /// <returns></returns>
        public static IEnumerable<ListItem> GetPagedData(string listName, int pageSize,string OrderByField, bool Ascending = true, List<Parameter> parameterList = null, ClientContext clientContext = null)
        {
            try
            {
                ValidateListName(listName);
                ValidateParamelistAndOrdeByField(parameterList, OrderByField);
                var RowLimit = string.Format("<RowLimit>{0}</RowLimit><OrderBy><FieldRef Name='{1}' Ascending='{2}'/></OrderBy>", pageSize, OrderByField, Ascending);
                var filterQuery = parameterList != null ? LookupHelper.FilterByAnyField(parameterList) : null;
                var removedView = !string.IsNullOrEmpty(filterQuery?.ViewXml) ? filterQuery.ViewXml.Replace("<View>", string.Empty).Replace("</View>", string.Empty) : null;
                var withRowLimit = !string.IsNullOrEmpty(removedView) ? string.Format("{0}{1}", removedView, RowLimit) : string.Format("{0}", RowLimit);
                filterQuery = new CamlQuery();
                filterQuery.ViewXml = string.Format("<View>{0}</View>", withRowLimit);
                var oListItems = new List<ListItem>();
                clientContext = clientContext ?? (clientContext = LookupHelper.GetClientContextFromAdminOrWindows(clientContext));
                if (clientContext != null)
                {
                    using (clientContext)
                    {
                        var oList = clientContext
                                    .Web
                                    .Lists
                                    .GetByTitle(listName)
                                    .GetItems(filterQuery);
                        clientContext.Load(oList);
                        clientContext.ExecuteQuery();
                        if (oList != null && oList.Count() > 0)
                            oList.ToList().ForEach(item => oListItems.Add(item));
                    }
                }
                return oListItems;
            }
            catch(Exception exception)
            {
                return new List<ListItem>();
            }    
                
        }
    
        /// <summary>
        /// Saves new item in the list that matches the provided listName and parameters
        /// </summary>
        /// <param name="clientContext">Client Context</param>
        /// <param name="listName">Name of the list</param>
        /// <param name="parameterList">List of fields that will be updates(ParameterName=FieldName,ParameterValue=FieldValue)</param>
        /// <returns>True if saved successfully, false if not saved successfully</returns>
        public static bool Save(ClientContext clientContext, string listName, List<Parameter> parameterList)
        {
            ValidateListName(listName);
            if(parameterList != null && parameterList.Count > 0)
            {
                if (clientContext != null)
                {
                    using (clientContext)
                    {
                        var parameters = new ListItemCreationInformation();
                        var listToSave = clientContext.Web.Lists.GetByTitle(listName);
                        var listItem = listToSave.AddItem(parameters);
                        parameterList.ForEach(parameter => SetListItemFields(listItem, parameter));                          
                        listItem.Update();
                        clientContext.ExecuteQuery();
                    }
                }
            }
            return parameterList != null || parameterList.Count > 0;
           
        }
        /// <summary>
        /// Set ListItem fields
        /// </summary>
        /// <param name="oListItem">List Item object</param>
        /// <param name="parameter">Paramater Fields that will set List Item Object</param>
        public static void SetListItemFields(ListItem oListItem, Parameter parameter)
        {
            if (parameter.IsLookupField)
                oListItem[parameter.ParameterName] = SetLookupValue(parameter.ParameterValue);
            else if (parameter.IsPeoplePicker)
            {
                validatePoplePicker(parameter);
                oListItem[parameter.ParameterName] = parameter.User;
            }
            else
                oListItem[parameter.ParameterName] = parameter.ParameterValue;
        }
        private static FieldLookupValue SetLookupValue(string parameterValue)
            => new FieldLookupValue
            {
                LookupId = int.Parse(parameterValue)
            };
      
        /// <summary>
        /// Updates the current list item that matches the provided listItemId(list primary key),listName(name of the list) and parameterList(list of fields to be updated)
        /// </summary>
        /// <param name="clientContext">Client Context</param>
        /// <param name="listItemId">List Item Id [primary key]</param>
        /// <param name="listName">name of the list</param>
        /// <param name="parameterList">Fields to be updated, [Field] and [Value]</param>
        /// <returns>True if updated successfully, false if update failed</returns>
        public static bool Update(ClientContext clientContext, int listItemId, string listName, List<Parameter> parameterList)
        {
            ValidateListName(listName);
            if(parameterList != null && parameterList.Count > 0)
            {
                if(clientContext != null)
                {
                    using (clientContext)
                    {
                        var requestList = clientContext.Web.Lists.GetByTitle(listName);
                        var listItem = requestList.GetItemById(listItemId);
                        clientContext.Load(listItem);
                        clientContext.ExecuteQuery();

                        if (listItem != null)
                            parameterList.ForEach(parameter => SetListItemFields(listItem, parameter));
                        listItem.Update();
                        clientContext.ExecuteQuery();
                        return true;
                    }
                }
            }
            return false;
        }
    
        /// <summary>
        /// Removes an item on the list(listName = name of the list where item will be deleted) that matches the provided listItemId(list primary key) 
        /// </summary>
        /// <param name="clientContext">Client Context</param>
        /// <param name="listName">Name of the list where deletion will happen</param>
        /// <param name="listItemId">list primary key[Id]</param>
        /// <returns>Returns true if item was deleted successfully else false</returns>
        public static bool Delete(ClientContext clientContext, string listName, int listItemId)
        {
            ValidateListName(listName);
            if(clientContext != null)
            {
                using (clientContext)
                {
                    var list = clientContext.Web.Lists.GetByTitle(listName);
                    var listItem = list.GetItemById(listItemId);
                    if(listItem != null)
                    {
                        clientContext.Load(listItem);
                        clientContext.ExecuteQuery();

                        listItem.Recycle();
                        clientContext.ExecuteQuery();
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Returns ListItem that matches the provided listName(name of the list where search is performed),listItemId(list item id[primary key]
        /// </summary>
        /// <param name="clientContext">Client Context</param>
        /// <param name="listName">Name of the list</param>
        /// <param name="listItemId">List item Id [primary key]</param>
        /// <returns>List Item details</returns>
        public static ListItem GetListItemById(ClientContext clientContext, string listName, int listItemId)
        {
            try
            {
                ValidateListName(listName);
                ListItem listItem = null;
                if (clientContext != null)
                {
                    using (clientContext)
                    {
                        listItem = clientContext
                                       .Web
                                       .Lists
                                       .GetByTitle(listName)
                                       .GetItemById(listItemId);
                        if (listItem != null)
                        {
                            clientContext.Load(listItem);
                            clientContext.ExecuteQuery();
                        }
                    }
                }
                return listItem;
            }
            catch(Exception excption)
            {
                return null;
            }
           
        }
        /// <summary>
        /// Returns the list item that matches the provided listItemId[primary key]
        /// </summary>
        /// <param name="listName">Name of the list</param>
        /// <param name="listItemId">List Item Id [primary key]</param>
        /// <returns>List Item Details</returns>
        public static ListItem GetListItemById(string listName, int listItemId)
        {
            try
            {
        
            
                ValidateListName(listName);
                var clientContext = LookupHelper.GetClientContextFromAdminOrWindows();
                ListItem listItem = null;
                if (clientContext != null)
                {
                    using (clientContext)
                    {
                        listItem = clientContext
                                       .Web
                                       .Lists
                                       .GetByTitle(listName)
                                       .GetItemById(listItemId);
                        if (listItem != null)
                        {
                            clientContext.Load(listItem);
                            clientContext.ExecuteQuery();
                        }
                    }
                }
                return listItem;
            }
            catch(Exception exception)
            {
                return null;
            }
           
        }
        /// <summary>
        /// Validate the provided listname where it's null or empty
        /// </summary>
        /// <param name="listName">name of the list</param>
        public static void ValidateListName(string listName)
        {
            if (string.IsNullOrEmpty(listName)) throw new ArgumentException("The list name parameter cannot be Empty or null");
        }
        /// <summary>
        /// This will validate People Picker
        /// </summary>
        /// <param name="parameter"></param>
       private static void validatePoplePicker(Parameter parameter)
        {
            if (parameter.IsPeoplePicker && parameter.User == null)
                throw new Exception("If IsPeoplePicker is true then User cannot be null");
        }
       
        /// <summary>
        /// Validates whether the Parameter list or orderByField are null values.If both parameters are null throw exception
        /// </summary>
        /// <param name="parameterList">Parameter list values</param>
        /// <param name="orderByField">Order by field</param>
        public static void ValidateParamelistAndOrdeByField(List<Parameter> parameterList, string orderByField)
        {
            if (string.IsNullOrEmpty(orderByField) && parameterList == null) throw new ArgumentException("Either parametrList or orderByField cannot be null(one of them must be null)");
        }
        /// <summary>
        /// Validate the listitem Id if it's not less than or equal to 0
        /// </summary>
        /// <param name="id">List Item Id</param>
        public static void ValidateListItemKey(int id)
        {
            if (id <= 0) throw new Exception("No list item can have 0 as the key");
        }
       

        /// <summary>
        /// Returns items on the list[listName = name of the list where query is performed] that matches the provided parameterList[filter]
        /// </summary>
        /// <param name="clientContext">Client Context</param>
        /// <param name="listName">name of the list</param>
        /// <param name="parameterList">filter list</param>
        /// <param name="orderByField">field to order if paramlist is null</param>
        /// <returns>ListItems</returns>
        public static IEnumerable<ListItem> GetAnyListData(ClientContext clientContext, string listName, List<Parameter> parameterList = null, string orderByField = null)
        {
            try
            {
                ValidateListName(listName);
                ValidateParamelistAndOrdeByField(parameterList, orderByField);
                var listItem = new List<ListItem>();
                var camlQuery = new CamlQuery();

                if (clientContext != null)
                {
                    using (clientContext)
                    {
                        camlQuery.ViewXml = parameterList != null ? LookupHelper.FilterByAnyField(parameterList).ViewXml
                                                                  : string.Format(@"<Query><OrderBy><FieldRef Name = '{0}'/></OrderBy></Query>", orderByField);
                        var listItemCollection = clientContext
                                                .Web
                                                .Lists
                                                .GetByTitle(listName)
                                                .GetItems(camlQuery);
                        clientContext.Load(listItemCollection);
                        clientContext.ExecuteQuery();
                        if (listItemCollection != null)
                            listItemCollection.ToList().ForEach(item => listItem.Add(item));

                    }
                }
                return listItem;
            }
            catch (Exception excption)
            {
                return new List<ListItem>();
            }

        }
    }
}
