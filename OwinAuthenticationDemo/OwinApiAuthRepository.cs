using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OwinAuthenticationDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OwinAuthenticationDemo
{
    //Repository class to query the DB using the Identity DB context
    public class OwinApiAuthRepository : IDisposable
    {
        public OwinAuthDbContext _context;

        private UserManager<IdentityUser> _userMgr;

        public OwinApiAuthRepository()
        {
            _context = new OwinAuthDbContext();
            _userMgr = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_context));
        }

        //Create use within the DB
        public async Task<IdentityResult>CreateUserAsync(User model)
        {
            IdentityUser idUser = new IdentityUser();
            idUser.UserName = model.UserName;
            idUser.Email = model.Email;
            idUser.EmailConfirmed = model.EmailConfirmed;

            var result = await _userMgr.CreateAsync(idUser, model.Password);

            return result;
        }

        //Find the user based on credentials provided
        public async Task<IdentityUser>GetUserAsync(string username, string password)
        {
            IdentityUser user = await _userMgr.FindAsync(username, password);

            return user;
        }

        public void Dispose()
        {
            _context.Dispose();
            _userMgr.Dispose();
        }
    }
}