using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace Repository
{
    public class BaseRepository
    {
        public ResponseModel Create(string procedureName, Dictionary<string, object> inputParameters, ref Dictionary<string, object> outputParameters)
        {
            ResponseModel res = new ResponseModel();
            List<object> items = new List<object>();
            using (var con = new SqlConnection(DbHelper.ConnectionString))
            {
                SqlCommand com = new SqlCommand(procedureName, con);
                com.CommandType = CommandType.StoredProcedure;
                foreach (var i in inputParameters)
                {
                    com.Parameters.AddWithValue(i.Key, i.Value);
                }

                try
                {
                    con.Open();
                    var reader = com.ExecuteReader();
                    if (!reader.HasRows) throw new Exception();

                    List<string> readerColumnNames = GetReaderColumnNames(reader);
                    while (reader.Read())
                    {
                        object item = MapObjectFromReader(readerColumnNames, reader);

                    }
                    res.Error = false;
                    res.Message = "";
                    res.StatusCode = 200;
                    res.Results = items;
                    return res;
                }
                catch (Exception ex)
                {
                    res.Error = true;
                    res.Message = "Something went wrong";
                    res.StatusCode = 500;
                    res.Results = new List<object>();
                    return res;
                }

            }
        }


        private object MapObjectFromReader(List<string> genericTypePropertyNames, SqlDataReader reader)
        {
            var dynamicObject = new ExpandoObject();
            foreach (string item in genericTypePropertyNames)
            {
                dynamicObject.TryAdd(item, reader["item"].ToString());
            }
            return dynamicObject;
        }

        #region FetchPropertyNamesLogic
        //private List<string> GetGenericTypePropertyNames<T>()
        //{
        //    List<string> props = new List<string>();
        //    foreach (var prop in typeof(T).GetProperties())
        //    {
        //        props.Add(prop.Name.ToLower());
        //    }

        //    return props;
        //}

        private List<string> GetReaderColumnNames(SqlDataReader reader)
        {
            List<string> columnNames = Enumerable.Range(0, reader.FieldCount)
            .Select(reader.GetName)
            .ToList();

            columnNames.ForEach(p => p = p.ToLower());

            return columnNames;
        }
        #endregion
    }
}
