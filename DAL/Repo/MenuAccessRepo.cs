using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo
{
    public class MenuAccessRepo : BaseRepo
    {
        public List<MenuAccess> GetAllList()
        {
            return (from c in Context.MenuAccesses select c).ToList();
        }

        public List<MenuAccessDtls> GetList()
        {
            List<MenuAccessDtls> objPortals = new List<MenuAccessDtls>();

            objPortals = (from ma in Context.MenuAccesses
                          join m in Context.Menus on ma.MenuID equals m.MenuID
                          join u in Context.Users on ma.UserID equals u.UserID
                          where ma.IsActive == true
                          orderby m.Name
                          select new MenuAccessDtls
                          {
                              AccessID = ma.AccessID,
                              Name = m.Name,
                              UserName = u.Name,
                              IsActive = ma.IsActive,

                          }).ToList();

            return objPortals;
        }

        public MenuAccessDtls GetById(Guid id)
        {
            MenuAccessDtls menu = new MenuAccessDtls();
            List<MenuAccessDtls> objMenu = new List<MenuAccessDtls>();
            objMenu = GetList();
            menu = objMenu.Where(m => m.AccessID == id).FirstOrDefault();

            return menu;
        }

        public MenuAccess GetMenuAccess(Guid id)
        {
            MenuAccess menuAccess = new MenuAccess();
            List<MenuAccess> maList = new List<MenuAccess>();
            maList = GetAllList();
            menuAccess = maList.Where(s => s.AccessID == id).FirstOrDefault();
            return menuAccess;

        }

        public void Save(MenuAccess menuAccess)
        {
            menuAccess.AccessID = Guid.NewGuid();
            menuAccess.IsActive = true;
            Context.MenuAccesses.Add(menuAccess);
            Context.SaveChanges();
        }

        public void Update(Guid id, MenuAccess update)
        {
            MenuAccess menuAccess = (from c in Context.MenuAccesses where c.AccessID == id select c).FirstOrDefault();

            menuAccess.MenuID = update.MenuID;

            menuAccess.UserID = update.UserID;

            Context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            MenuAccess delete = (from c in Context.MenuAccesses where c.AccessID == id select c).FirstOrDefault();
            delete.IsActive = false;
            Context.SaveChanges();
        }

    }

    public class MenuAccessDtls
    {
        public MenuAccessDtls()
        {

        }

        public MenuAccessDtls(Guid Id, string name, string userName, bool? isActive)
        {
            AccessID = Id;
            Name = name;
            UserName = userName;
            IsActive = isActive;
        }


        public Guid AccessID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public bool? IsActive { get; set; }
    }
}
