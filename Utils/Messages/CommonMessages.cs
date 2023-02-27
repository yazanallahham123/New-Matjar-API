﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Utils.Messages
{
    public class CommonMessages
    {
        public const String UPDATED_SUCCESSFULLY = "Updated successfully.";
        public const String DELETED_SUCCESSFULLY = "Deleted successfully.";
        public const String ADDED_SUCCESSFULLY = "Added successfully.";
        public const String NOT_FOUND = "Not found.";        
        public const String ERROR_UPDATE = "An error occurred while updating the data, please try again.";
        public const String PUSH_EMPTY_VALUE = "You cannot push null values.";
        public const String NOT_LOGGEDIN = "You are not logged in.";
        public const String INVALID_PAGE_NUMBER = "Page must be bigger than 0.";
        public const String INVALID_PAGE_SIZE = "Page size must be bigger than 0.";

        public static List<String> ModelStateParser(ModelStateDictionary modelStateDictionary)
        {
            return modelStateDictionary.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)).ToList();
        }
    }
}
