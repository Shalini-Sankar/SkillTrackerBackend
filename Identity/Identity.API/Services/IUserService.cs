using System;
using System.Collections.Generic;
using System.Text;
using Identity.API.Models;

namespace Identity.API.Services
{
    public interface IUserService
    {
        User GetUser(string Username, string Password);
    }
}
