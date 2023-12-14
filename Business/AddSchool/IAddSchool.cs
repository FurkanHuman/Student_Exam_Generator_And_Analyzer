using Entity.Entities.Mains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.AddSchool;

public interface IAddSchool
{
    School AddSchool(Exam exam);
}
