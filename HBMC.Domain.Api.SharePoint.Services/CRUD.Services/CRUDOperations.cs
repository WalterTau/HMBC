using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint;
using SharePointHelper.Lookups;
using System.Net;
using HBMC.Domain.Api.SharePoint.Services;
using HBMC.Domain.Api;
using System.Security;

namespace SharePointHelper.CRUD
{
    /// <summary>
    /// This class has static methods for CRUD(Create,Read,Update,Delete)
    /// </summary>
   public class CRUDOperations
    {

        static HBMC.Domain.Api.ISharePointConnectionSetting _connectionSetting;

        public CRUDOperations(ISharePointConnectionSetting connectionSetting)
        {
            _connectionSetting = connectionSetting;
        }

        /// <summary>
        /// Saves new item in the list that matches the provided listName and parameters
        /// </summary>
        /// <param name="clientContext">Client Context</param>
        /// <param name="listName">Name of the list</param>
        /// <param name="parameterList">List of fields that will be updates(ParameterName=FieldName,ParameterValue=FieldValue)</param>
        /// <returns>True if saved successfully, false if not saved successfully</returns>
        public static bool Save(string listName, List<Parameter> parameterList)
        {

            ClientContext clientContext = new ClientContext(_connectionSetting.ConnectionSharePointUrl);
            SecureString securePassword = new SecureString();
            _connectionSetting.Pasword.ToList().ForEach(securePassword.AppendChar);
            clientContext.Credentials = new SharePointOnlineCredentials(_connectionSetting.Username, securePassword);
            if (parameterList != null && parameterList.Count > 0)
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
                ClientContext clientContext = new ClientContext(_connectionSetting.ConnectionSharePointUrl);
                SecureString securePassword = new SecureString();
                _connectionSetting.Pasword.ToList().ForEach(securePassword.AppendChar);
                clientContext.Credentials = new SharePointOnlineCredentials(_connectionSetting.Username, securePassword);
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
        /// Returns items on the list[listName = name of the list where query is performed] that matches the provided parameterList[filter]
        /// </summary>
        /// <param name="clientContext">Client Context</param>
        /// <param name="listName">name of the list</param>
        /// <param name="parameterList">filter list</param>
        /// <param name="orderByField">field to order if paramlist is null</param>
        /// <returns>ListItems</returns>
        public static IEnumerable<ListItem> GetAnyListData(string listName, List<Parameter> parameterList = null, string orderByField = null)
        {
            try
            {
                ///<summary>
                ///Connect to Online SharePoint
                ///</summary>
                ClientContext clientContext = new ClientContext(_connectionSetting.ConnectionSharePointUrl);
                SecureString securePassword = new SecureString();
                _connectionSetting.Pasword.ToList().ForEach(securePassword.AppendChar);
                clientContext.Credentials = new SharePointOnlineCredentials(_connectionSetting.Username, securePassword);
                var listItem = new List<ListItem>();
                var camlQuery = new CamlQuery();

                if (clientContext != null)
                {
                    using (clientContext)
                    {
                        camlQuery.ViewXml = string.Format(@"<Query><OrderBy><FieldRef Name = '{0}'/></OrderBy></Query>", orderByField);
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
