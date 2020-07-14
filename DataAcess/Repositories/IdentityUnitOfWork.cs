using DataAcess.Entities;
using DataAcess.Identity;
using DataAcess.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private EF.AppContext _db;

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private IUserProfileManager _userProfileManager;

        public IdentityUnitOfWork(string connectionString)
        {
            _db = new EF.AppContext(connectionString);
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_db));
            _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(_db));
            _userProfileManager = new UserProfileManager(_db);
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager; }
        }

        public IUserProfileManager UserProfileManager
        {
            get { return _userProfileManager; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager; }
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _userManager.Dispose();
                    _roleManager.Dispose();
                    _userProfileManager.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}

