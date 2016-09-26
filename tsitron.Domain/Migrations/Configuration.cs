using System.Collections.Generic;
using System.Globalization;
using System.Text;
using tsitron.Domain.Entitys.Customers;
using tsitron.Domain.Entitys.Goods;
using tsitron.Domain.Entitys.Secure;

namespace tsitron.Domain.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<tsitron.Domain.Context.TsitronContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(tsitron.Domain.Context.TsitronContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.UsrRoles.AddOrUpdate(r=>r.Title,
                new UsrRole {Title = "M", Descr = "Moderator"},
                new UsrRole { Title = "O", Descr = "Operator"},
                new UsrRole { Title = "S", Descr = "Seller"},
                new UsrRole { Title = "C", Descr = "Customer"}
                );
            context.Users.AddOrUpdate(u=>u.Login,
                new User { Login = "+79034441684", Name = "Admin", RegisterDate = DateTime.Now, ConfirmEmail = true, PasswordHash = "c84ae35c470c8c2fca384f8ab27abf0f", UserRole = new UsrRole {Title = "A", Descr = "Administrator"} }
                //FB%iS-_Xtr-nm=x
                );
 
        }
    }
}
