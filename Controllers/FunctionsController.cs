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

    public class FunctionsController : ApiController
    {
        private saprfcdbEntities db = new saprfcdbEntities();

        // GET: api/functions
        public IHttpActionResult Getfunctions()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<function> function = db.functions.ToList();

                var finctionRecord = from f in function
                                     select new function
                                     {
                                         func_id = f.func_id,
                                         func_name = f.func_name,
                                         cost_center = f.cost_center,
                                         is_active = f.is_active
                                     };

                return Json(finctionRecord);


            }
        }

        [Route("api/functions/GetActivefunctions")]
        [HttpGet]
        public IHttpActionResult GetActivefunctions()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<function> function = db.functions.ToList();

                var finctionRecord = from f in function
                                     where f.is_active == 1
                                     select new function
                                     {
                                         func_id = f.func_id,
                                         func_name = f.func_name,
                                         cost_center = f.cost_center,
                                         is_active = f.is_active
                                     };

                return Json(finctionRecord);


            }
        }

        // GET: api/functions/5
        [ResponseType(typeof(function))]
        public IHttpActionResult Getfunction(int id)
        {
            function function = db.functions.Find(id);
            if (function == null)
            {
                return NotFound();
            }

            return Ok(function);
        }


        // PUT: api/functions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putfunction(int id, function function)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != function.func_id)
            {
                return BadRequest();
            }

            db.Entry(function).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Json(new { message = "Successfull!" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!functionExists(id))
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


        // POST: api/Function
        //[HttpPost]
        [ResponseType(typeof(function))]
        public IHttpActionResult CreateFunction(function function)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.functions.Add(function);

            if (functionNameExists(function.func_name))
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

        // DELETE: api/functions/5
        [ResponseType(typeof(function))]
        public IHttpActionResult Deletefunction(int id)
        {
            function function = db.functions.Find(id);
            if (function == null)
            {
                return NotFound();
            }

            db.functions.Remove(function);
            db.SaveChanges();

            return Ok(function);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool functionExists(int id)
        {
            return db.functions.Count(e => e.func_id == id) > 0;
        }
        private bool functionNameExists(string id)
        {
            return db.functions.Count(e => e.func_name == id) > 0;
        }
    }
}