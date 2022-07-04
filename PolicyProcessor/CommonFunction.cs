using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace PolicyProcessor
{
    public static class CommonFunction
    {
        /// <summary>
        /// Convert given date time to timestamp
        /// </summary>
        /// <param name="value">Date Time</param>
        /// <returns>Date Time Spamp</returns>
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        /// <summary>
        /// Convert List to Datatable
        /// </summary>
        /// <typeparam name="T">T is a generic class</typeparam>
        /// <param name="models">List object</param>
        /// <returns>Converted Datatable</returns>
        public static DataTable ConvertToDataTable<T>(List<T> models)
        {
            // creating a data table instance and typed it as our incoming model   
            // as I make it generic, if you want, you can make it the model typed you want.  
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties of that model  
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Loop through all the properties              
            // Adding Column name to our datatable  
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names    
                dataTable.Columns.Add(prop.Name);
            }
            // Adding Row and its value to our dataTable  
            foreach (T item in models)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows    
                    values[i] = Props[i].GetValue(item, null);
                }
                // Finally add value to datatable    
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}
