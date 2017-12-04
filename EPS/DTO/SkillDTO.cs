using EPS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPS.DTO
{
    public class SkillDTO
    {
        public string Owner { get; set; }
        public int SkillID { get; set; }        
        public string Date { get; set; }
        public string  Assessedby { get; set; }
        public string Type { get; set; }
        public string Comment { get; set; }
    }
}