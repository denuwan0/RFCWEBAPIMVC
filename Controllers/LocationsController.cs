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

    public class LocationsController : ApiController
    {
        private saprfcdbEntities db = new saprfcdbEntities();

        // GET: api/locations
        public IHttpActionResult Getlocations()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<location> location = db.locations.ToList();

                var locationRecord = from l in location
                                     select new location
                                     {
                                         loc_id = l.loc_id,
                                         loc_name = l.loc_name,
                                         is_active = l.is_active
                                     };

                return Json(locationRecord);


            }
        }


        [Route("api/locations/get_active_list")]
        [HttpGet]
        public IHttpActionResult get_active_list()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<location> location = db.locations.ToList();

                var locationRecord = from l in location
                                     where l.is_active == 1
                                     select new location
                                     {
                                         loc_id = l.loc_id,
                                         loc_name = l.loc_name,
                                         is_active = l.is_active
                                     };

                return Json(locationRecord);


            }
        }

        // GET: api/locations/5

        [ResponseType(typeof(location))]
        public IHttpActionResult Getlocation(int id)
        {
            location location = db.locations.Find(id);
            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }


        // PUT: api/locations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putlocation(int id, location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != location.loc_id)
            {
                return BadRequest();
            }

            db.Entry(location).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Json(new { message = "Successfull!" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!locationExists(id))
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


        // POST: api/Location
        //[HttpPost]
        [ResponseType(typeof(location))]
        public IHttpActionResult CreateLocation(location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.locations.Add(location);
            if (locationNameExists(location.loc_name))
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

            //return CreatedAtRoute("DefaultApi", new { id = location.loc_id }, location);
        }

        // DELETE: api/locations/5
        [ResponseType(typeof(location))]
        public IHttpActionResult Deletelocation(int id)
        {
            location location = db.locations.Find(id);
            if (location == null)
            {
                return NotFound();
            }

            db.locations.Remove(location);
            db.SaveChanges();

            return Ok(location);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool locationExists(int id)
        {
            return db.locations.Count(e => e.loc_id == id) > 0;
        }

        private bool locationNameExists(string id)
        {
            return db.locations.Count(e => e.loc_name == id) > 0;
        }
    }
}