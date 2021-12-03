using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Repository
{
    public class BaseRepository
    {
        public ResponseModel<T> Create<T>(string procedureName, Dictionary<string, object> inputParameters, ref Dictionary<string, object> outputParameters)
        {
            ResponseModel<T> res = new ResponseModel<T>();
            List<T> items = new List<T>();
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

                    List<string> genericTypePropertyNames = GetGenericTypePropertyNames<T>();
                    List<string> readerColumnNames = GetReaderColumnNames(reader);
                    while (reader.Read())
                    {
                        T item = MapTFromReader<T>(genericTypePropertyNames, readerColumnNames, reader);

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
                    res.Results = new List<T>();
                    return res;
                }

            }
        }


        private T MapTFromReader<T>(List<string> genericTypePropertyNames, List<string> readerColumnNames, SqlDataReader reader)
        {
            // find if a property from T is the same as
            // reader column name
            object objectToMap;
            foreach (string item in genericTypePropertyNames)
            {
                if(readerColumnNames.FindIndex(x => x == item) != -1)
                {
                    objectToMap["item"] = reader["item"].ToString();
                }
            }
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
