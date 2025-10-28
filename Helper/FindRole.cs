using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_rab_4._2_KhasanovaNG_BPI_23_01.Model;

namespace Lab_rab_4._2_KhasanovaNG_BPI_23_01.Helper
{
    public class FindRole
    {
        int id;
        public FindRole(int id)
        {
            this.id = id;
        }

        public bool RolePredicate(Role role)
        {
            return role.Id == id;
        }
    }
}
