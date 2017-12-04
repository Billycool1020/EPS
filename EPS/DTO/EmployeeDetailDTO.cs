using EPS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPS.DTO
{
    public class EmployeeDetailDTO
    {
        public Employee employee { get; set; }
        public List<EmployeeSkillDetail> TechnicalSkillList { get; set; }
        public List<EmployeeSkillDetail> LanguageSkillList { get; set; }
        public List<EmployeeSkillDetail> OtherSkillList { get; set; }

    }
}