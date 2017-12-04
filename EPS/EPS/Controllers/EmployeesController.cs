using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EPS.DAL;
using EPS.Models;
using EPS.DTO;

namespace EPS.Controllers
{
    public class EmployeesController : Controller
    {
        private EPSContext db = new EPSContext();

        // GET: Employees
        public ActionResult Index()
        {          
            return View(db.Employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(/*string id*/)
        {
            var id = "v-haowli";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MSAlias,WSAlias,ChineseName,EnglishName,DateofBirth,Major,OnBoardDate,OM,Group,Lob,Product")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MSAlias,WSAlias,ChineseName,EnglishName,DateofBirth,Major,OnBoardDate,OM,Group,Lob,Product")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult AddSkill(/*string? id*/)
        {
            var id = "v-haowli";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            SkillDTO skill = new SkillDTO();
            skill.Owner = id;
            skill.Assessedby = User.Identity.Name.Split('\\')[1];
            skill.Date = DateTime.Today.ToShortDateString();

            ViewBag.Type = new SelectList(db.Skills.GroupBy(x => x.Type).Select(s => s.Key));
            return View(skill);
        }

        [HttpPost]
        public ActionResult AddSkill(SkillDTO SkillDTO)
        {
            var Owner = db.Employees.Find(SkillDTO.Owner);
            if (SkillDTO.SkillID == 0)
            {
                ModelState.AddModelError("", "Please selete a skill.");
                SkillDTO.Type = "Please Choose...";
                ViewBag.Type = new SelectList(db.Skills.GroupBy(x => x.Type).Select(s => s.Key));
                return View(SkillDTO);
            }
               
            var Skill = db.Skills.Find(SkillDTO.SkillID);
            if (Owner.Skills.Contains(Skill))
            {
                ModelState.AddModelError("", SkillDTO.Owner+" already has this skill.");
                ViewBag.Type = new SelectList(db.Skills.GroupBy(x => x.Type).Select(s => s.Key));
                SkillDTO.Type = "Please Choose...";
                return View(SkillDTO);
            }
            Owner.Skills.Add(Skill);
            EmployeeSkillDetail detail = new EmployeeSkillDetail();
            detail.Comment = SkillDTO.Comment;
            detail.Date = Convert.ToDateTime(SkillDTO.Date);
            detail.Assessedby = SkillDTO.Assessedby;
            detail.Employee = SkillDTO.Owner;
            detail.level = SkillDTO.level;
            detail.Skill = Skill.Name;
            db.SkillDetails.Add(detail);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }


        public class skilloption
        {
            public string Name { get; set; }
            public List<object> list { get; set; }
        }

        public ActionResult GetSKill(string Type)
        {
            var skills = db.Skills.Where(x => x.Type == Type && x.ParentID!=0).GroupBy(x=>x.ParentID).ToList();
            List<skilloption> list = new List<skilloption>();
            foreach(var s in skills)
            {
                skilloption option = new skilloption();
                List<object> list2 = new List<object>();
                option.Name = db.Skills.Where(x => x.ID==s.Key).Select(y=>y.Name).FirstOrDefault();
                foreach(var ss in s)
                {
                    list2.Add(new {Level = ss.Level,Name = ss.Name , ID = ss.ID});
                }
                option.list = list2;
                list.Add(option);
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

      
    }
    
}
