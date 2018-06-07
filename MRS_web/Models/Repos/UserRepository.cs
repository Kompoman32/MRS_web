using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRS_web.Models.EDM;

namespace MRS_web.Models.Repos
{
    public class UserRepository
    {
        private ModelContainer1  cont;

        public UserRepository(ModelContainer1 _cont)
        {
            cont = _cont;
        }

        public IEnumerable<User> Users()
        {
            return (from u in cont.UserSet where !u.AdminPrivileges orderby u.FullName select u);
        }

        public IEnumerable<User> Admins()
        {
            return (from u in cont.UserSet where u.AdminPrivileges orderby u.FullName select u);
        }

        public User GetUser(string login)
        {
            var req = (from u in cont.UserSet where u.Login == login select u);
            return req.Any() ? req.First():null;
        }

        //TODO: добавление удаление edit
    }
}