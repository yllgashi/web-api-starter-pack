using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ResponseModel<T>
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public bool Error { get; set; }
        public List<T> Results { get; set; }
    }
}
