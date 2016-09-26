using System;
using System.Linq;
using System.Web.Security;

namespace tsitron.Domain.Context
{
    public class Roles:RoleProvider
    {
        private readonly TsitronContext _context = new TsitronContext();
        public override bool IsUserInRole(string username, string roleName)
        {
            var i = int.Parse(username);
            var usr = ContextFabric.Context.Users.FirstOrDefault(u => u.Id == i);
            if (usr!=null)
            {
                return usr.UserRole.Title == roleName;
            }
            return false;
        }

        public override string[] GetRolesForUser(string username)
        {
            var i = int.Parse(username);
            var user = _context.Users.FirstOrDefault(c => c.Id == i);
            var s = user != null ? user.UserRole.Title : string.Empty;
            string[] resalt = { s };
            return resalt;
        }

        public override void CreateRole(string roleName)
        {
            throw new System.NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new System.NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new System.NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new System.NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new System.NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new System.NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new System.NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}