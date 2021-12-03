using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : BaseRepository
    {
        public ResponseModel Create(User user)
        {
            Dictionary<string, object> inputs = new Dictionary<string, object>();
            Dictionary<string, object> outputs = new Dictionary<string, object>();
            inputs.Add("@firstName", user.FirstName);
            inputs.Add("@lastName", user.LastName);

            return base.Create("dbo.usp_User_Create", inputs, ref outputs);
        }
    }
}
