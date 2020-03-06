using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo
{
    public class BaseRepo
    {
        private ForestEntities context;

        public ForestEntities Context
        {
            get
            {
                if (context == null)
                {
                    context = new ForestEntities();
                }

                return context;
            }
        }
    }
}
            
