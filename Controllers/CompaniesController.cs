using RFCWEBAPIMVC.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace RFCWEBAPIMVC.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class CompaniesController : ApiController
    {
        private saprfcdbEntities db = new saprfcdbEntities();

        // GET: api/companies
        public IHttpActionResult Getcompanies()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<company> company = db.companies.ToList();

                var companyRecord = from c in company
                                    select new company
                                    {
                                        company_id = c.company_id,
                                        company_code = c.company_code,
                                        company_name = c.company_name,
                                        is_active = c.is_active
                                    };

                return Json(companyRecord);


            }
        }

        [Route("api/companies/GetActiveCompanies")]
        [HttpGet]
        // GET: api/companies
        public IHttpActionResult GetActiveCompanies()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<company> company = db.companies.ToList();

                var companyRecord = from c in company
                                    where c.is_active == 1
                                    select new company
                                    {
                                        company_id = c.company_id,
                                        company_code = c.company_code,
                                        company_name = c.company_name,
                                        is_active = c.is_active
                                    };

                return Json(companyRecord);


            }
        }

        // GET: api/companies/5
        [ResponseType(typeof(company))]
        public IHttpActionResult Getcompany(int id)
        {
            company company = db.companies.Find(id);
            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }


        // PUT: api/companies/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putcompany(int id, company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != company.company_id)
            {
                return BadRequest();
            }

            db.Entry(company).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Json(new { message = "Successfull!" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!companyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
                return Json(new { message = "Failed!" });
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/Company
        [HttpPost]
        [ResponseType(typeof(company))]
        public IHttpActionResult CreateCompany(company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.companies.Add(company);

            if (companyCodeExists(company.company_code))
            {
                return Json(new { message = "Already Exists!" });
            }
            else
            {
                try
                {

                    db.SaveChanges();
                    return Json(new { message = "Successfull!" });
                }
                catch (DbUpdateException Dbex)
                {
                    return Json(new { message = "Failed!" });
                }
            }


            //return CreatedAtRoute("DefaultApi", new { id = company.company_id }, company);
        }

        // DELETE: api/companies/5
        [ResponseType(typeof(company))]
        public IHttpActionResult Deletecompany(int id)
        {
            company company = db.companies.Find(id);
            if (company == null)
            {
                return NotFound();
            }

            db.companies.Remove(company);
            db.SaveChanges();

            return Ok(company);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool companyExists(int id)
        {
            return db.companies.Count(e => e.company_id == id) > 0;
        }

        private bool companyCodeExists(int id)
        {
            return db.companies.Count(e => e.company_code == id) > 0;

        }
    }
}