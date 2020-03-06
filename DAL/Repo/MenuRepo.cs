using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DAL.Repo
{
    public class MenuRepo : BaseRepo
    {
        public List<Menu> GetAllList()
        {
            List<Menu> menu = new List<Menu>();
            menu = (from c in Context.Menus select c).ToList();
            return menu;
        }

        public List<MenuDetails> GetMenuByUser(Guid userID)
        {
            List<MenuDetails> objMenu = new List<MenuDetails>();

            objMenu = (from ma in Context.MenuAccesses
                       join m in Context.Menus on ma.MenuID equals m.MenuID
                       where ma.IsActive == true
                       where ma.UserID == userID
                       orderby m.Name
                       select new
                       {
                           m.MenuID,
                           m.Name,
                           m.Controller,
                           m.View,
                           m.Icon,
                           m.IsActive
                       }).ToList().Select(Rec => new
                          MenuDetails(Rec.MenuID, Rec.Name, Rec.Controller, Rec.View, Rec.Icon, Rec.IsActive)).ToList();

            return objMenu;
        }

        public List<SelectListItem> GetList()
        {
            List<SelectListItem> ObjForList = new List<SelectListItem>();

            List<Menu> objMenu = (from m in Context.Menus orderby m.Name select m).ToList();

            foreach (Menu item in objMenu)
            {
                SelectListItem objItem = new SelectListItem();
                objItem.Text = item.Name;
                objItem.Value = item.MenuID.ToString();

                ObjForList.Add(objItem);
            }

            return ObjForList;
        }

        public Menu GetById(Guid id)
        {
            Menu menu = new Menu();
            List<Menu> objMenu = new List<Menu>();
            objMenu = GetAllList();
            menu = objMenu.Where(m => m.MenuID == id).FirstOrDefault();

            return menu;
        }

        public void Save(Menu menu)
        {
            menu.MenuID = Guid.NewGuid();
            menu.IsActive = true;
            Context.Menus.Add(menu);
            Context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            Menu delete = (from c in Context.Menus where c.MenuID == id select c).FirstOrDefault();
            delete.IsActive = false;
            Context.SaveChanges();
        }

        public void Update(Guid id, Menu update)
        {
            Menu menu = (from c in Context.Menus where c.MenuID == id select c).FirstOrDefault();

            menu.Name = update.Name;

            menu.Controller = update.Controller;

            menu.View = update.View;

            menu.Icon = update.Icon;

            Context.SaveChanges();
        }

        public List<MenuDetails> GetMenu()
        {
            List<MenuDetails> objMenu = new List<MenuDetails>();


            objMenu = (from m in Context.Menus
                       where m.IsActive == true
                       orderby m.Name
                       select new
                       {
                           m.MenuID,
                           m.Name,
                           m.Controller,
                           m.View,
                           m.Icon,
                           m.IsActive

                       }).ToList().Select(Rec => new
                          MenuDetails(Rec.MenuID, Rec.Name, Rec.Controller, Rec.View, Rec.Icon, Rec.IsActive)).ToList();

            return objMenu;
        }
    }

    public class MenuDetails
    {
        public MenuDetails(Guid _id, string _name, string _controller, string _view, string _icon, bool? _isActive)
        {
            MenuID = _id;
            Name = _name;
            Controller = _controller;
            View = _view;
            Icon = _icon;
            IsActive = (_isActive ?? false);
        }

        public Guid MenuID { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
        public string View { get; set; }
        public string Icon { get; set; }
        public bool? IsActive { get; set; }
    }
}
