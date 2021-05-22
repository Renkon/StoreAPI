using System;
using System.Collections.Generic;
using System.Text;

namespace StoreAPI.Core.Dto
{
    public class UserDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int NationalId { get; set; }

        public double MoneySpent { get; set; }
    }
}
