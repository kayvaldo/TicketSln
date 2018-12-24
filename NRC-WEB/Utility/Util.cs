using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NRC_WEB.Utility
{
    public class Util
    {
        public static List<SelectListItem> GetCollectionSelectList(List<string> collection)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            if (collection == null || !collection.Any())
            {
                return selectList;
            }

            for (int i = 0; i < collection.Count; i++)
            {
                selectList.Add(new SelectListItem { Text = collection[i], Value = i.ToString() });
            }

            return selectList;
        }

        
    }
}