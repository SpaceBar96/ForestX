using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo
{
    public class UserRepo : BaseRepo
    {
        public List<User> GetAllList()
        {
            return (from c in Context.Users select c).ToList();
        }

        public List<UserDetails> GetList()
        {
            List<UserDetails> objPortals = new List<UserDetails>();

            objPortals = (from u in Context.Users
                          orderby u.Name
                          select new
                          {
                              u.UserID,
                              u.Name,
                              u.Email,
                              u.Password,
                              u.IsActive

                          }).ToList().Select(Rec => new
                          UserDetails(Rec.UserID, Rec.Name, Rec.Email, Rec.Password, Rec.IsActive)).ToList();

            return objPortals;
        }

        public UserDetails GetUserById(Guid id)
        {
            UserDetails user = new UserDetails();
            List<UserDetails> objUser = new List<UserDetails>();
            objUser = GetList();
            user = objUser.Where(s => s.id == id).FirstOrDefault();

            return user;
        }

        public bool CheckEmail(string _email)
        {
            bool verify = true;
            if (Context.Users.Any(u => u.Email == _email.Trim()))
            {
                verify = false;
            }
            else
            {
                verify = true;
            }
            return verify;
        }

        public string GetPassword(string email)
        {
            User user = (from u in Context.Users where u.Email == email select u).FirstOrDefault();
            return user.Password;
        }

        public User GetUser(Guid id)
        {
            User user = new User();
            List<User> userList = new List<User>();
            userList = GetAllList();
            user = userList.Where(s => s.UserID == id).FirstOrDefault();
            return user;
        }

        public void ActivateAccount(Guid id)
        {
            var getRecord = (from u in Context.Users where u.UserID == id select u).FirstOrDefault();
            getRecord.IsActive = true;
            Context.SaveChanges();
        }

        //ADD HERE THE EMAIL CHANGE PASSWORD
        public void ChangePassword(Guid id, string password)
        {
            var getRecord = (from u in Context.Users where u.UserID == id select u).FirstOrDefault();
            getRecord.Password = password;
            Context.SaveChanges();
        }

        public void SaveAtLogin(RegisterModel save)
        {
            User user = new User();
            save.UserID = Guid.NewGuid();
            save.IsActive = false;

            user.UserID = save.UserID;
            user.Name = save.Name;
            user.Email = save.Email;
            user.Password = save.Password;
            user.IsActive = save.IsActive;
            Context.Users.Add(user);
            Context.SaveChanges();
        }

        public void Save(User user)
        {
            user.UserID = Guid.NewGuid();
            user.IsActive = true;
            Context.Users.Add(user);
            Context.SaveChanges();
        }

        public void Update(Guid id, User update)
        {
            User user = (from c in Context.Users where c.UserID == id select c).FirstOrDefault();

            user.Name = update.Name;
            user.Email = update.Email;
            user.Password = update.Password;

            Context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            User delete = (from c in Context.Users where c.UserID == id select c).FirstOrDefault();
            delete.IsActive = false;
            Context.SaveChanges();
        }

        public void Recover(Guid id)
        {
            User delete = (from c in Context.Users where c.UserID == id select c).FirstOrDefault();
            delete.IsActive = true;
            Context.SaveChanges();
        }

        public User GetCredentials(string email)
        {
            User objUser = new User();
            if (Context.Users.Any(u => u.Email == email.Trim() && u.IsActive == true))
            {
                objUser = Context.Users.SingleOrDefault(Rec => Rec.Email == email.Trim());
            }
            else
            {
                objUser = null;
            }

            return objUser;
        }
    }

    public class UserDetails
    {
        public UserDetails(Guid _id, string _name, string _email, string _password, bool? _isActive)
        {
            id = _id;
            Name = _name;
            Email = _email;
            Password = _password;
            IsActive = (_isActive ?? false);
        }

        public UserDetails()
        {

        }

        public Guid id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
