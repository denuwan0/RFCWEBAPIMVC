using RFCWEBAPIMVC.Models;
using System;
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

    public class UrlTypesController : ApiController
    {
        private saprfcdbEntities db = new saprfcdbEntities();

        // GET: api/t_codes
        public IHttpActionResult GetUrlType()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<url_type> url_type = db.url_type.ToList();

                var urlTypeRecord = from ut in url_type
                                    select new url_type
                                    {
                                        url_type_id = ut.url_type_id,
                                        url_type_name = ut.url_type_name,
                                        is_active = ut.is_active
                                    };

                return Json(urlTypeRecord);

            }
        }

        [Route("api/url_types/get_active_list")]
        [HttpGet]
        // GET: api/t_codes
        public IHttpActionResult get_active_list()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<url_type> url_type = db.url_type.ToList();

                var urlTypeRecord = from ut in url_type
                                    where ut.is_active == 1
                                    select new url_type
                                    {
                                        url_type_id = ut.url_type_id,
                                        url_type_name = ut.url_type_name,
                                        is_active = ut.is_active
                                    };

                return Json(urlTypeRecord);


            }
        }

        // GET: api/t_codes/5
        [ResponseType(typeof(url_type))]
        public IHttpActionResult GetUrlType(int id)
        {
            url_type url_type = db.url_type.Find(id);
            if (url_type == null)
            {
                return NotFound();
            }

            return Ok(url_type);
        }


        // PUT: api/t_codes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUrlType(int id, url_type url_type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != url_type.url_type_id)
            {
                return BadRequest();
            }

            db.Entry(url_type).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Json(new { message = "Successfull!" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!urlTypeExists(id))
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


        // POST: api/T_code
        //[HttpPost]
        [ResponseType(typeof(url_type))]
        public IHttpActionResult CreateUrlType(url_type url_type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.url_type.Add(url_type);

            try
            {

                db.SaveChanges();
                return Json(new { message = "Successfull!" });
            }
            catch (DbUpdateException Dbex)
            {
                if (urlTypeExists(url_type.url_type_id))
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

        private bool urlTypeExists(object url_type)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/t_codes/5
        [ResponseType(typeof(url_type))]
        public IHttpActionResult DeleteUrlType(int id)
        {
            url_type url_type = db.url_type.Find(id);
            if (url_type == null)
            {
                return NotFound();
            }

            db.url_type.Remove(url_type);
            db.SaveChanges();

            return Ok(url_type);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool urlTypeExists(int id)
        {
            return db.url_type.Count(e => e.url_type_id == id) > 0;
        }
    }
}