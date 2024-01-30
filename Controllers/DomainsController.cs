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

    public class DomainsController : ApiController
    {
        private saprfcdbEntities db = new saprfcdbEntities();

        // GET: api/domains
        public IHttpActionResult Getdomains()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<domain> domain = db.domains.ToList();

                var domainRecord = from d in domain
                                   select new domain
                                   {
                                       domain_id = d.domain_id,
                                       domain_name = d.domain_name,
                                       is_active = d.is_active
                                   };

                return Json(domainRecord);
            }
        }

        [Route("api/domains/domainActiveList")]
        [HttpGet]
        // GET: api/user_permissions
        public IHttpActionResult domainActiveList()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<domain> domain = db.domains.ToList();

                var domainRecord = from d in domain
                                   where d.is_active == 1
                                   select new domain
                                   {
                                       domain_id = d.domain_id,
                                       domain_name = d.domain_name,
                                       is_active = d.is_active
                                   };

                return Json(domainRecord);


            }
        }

        // GET: api/domains/1
        [ResponseType(typeof(domain))]
        public IHttpActionResult Getdomain(int id)
        {
            domain domain = db.domains.Find(id);
            if (domain == null)
            {
                return NotFound();
            }

            return Ok(domain);
        }


        // PUT: api/domains/1
        [ResponseType(typeof(void))]
        public IHttpActionResult Putdomain(int id, domain domain)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != domain.domain_id)
            {
                return BadRequest();
            }

            db.Entry(domain).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Json(new { message = "Successfull!" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!domainExists(id))
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


        // POST: api/domain
        //[HttpPost]
        [ResponseType(typeof(domain))]
        public IHttpActionResult CreateDomain(domain domain)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.domains.Add(domain);

            try
            {

                db.SaveChanges();
                return Json(new { message = "Successfull!" });
            }
            catch (DbUpdateException Dbex)
            {
                if (domainExists(domain.domain_id))
                {
                    return Conflict();
                }
                else
                {
                    throw Dbex;
                }
                return Json(new { message = "Failed!" });
            }


            //return CreatedAtRoute("DefaultApi", new { id = location.loc_id }, location);
        }

        // DELETE: api/domains/5
        [ResponseType(typeof(domain))]
        public IHttpActionResult Deletedomain(int id)
        {
            domain domain = db.domains.Find(id);
            if (domain == null)
            {
                return NotFound();
            }

            db.domains.Remove(domain);
            db.SaveChanges();

            return Ok(domain);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool domainExists(int id)
        {
            return db.domains.Count(e => e.domain_id == id) > 0;
        }
    }
}