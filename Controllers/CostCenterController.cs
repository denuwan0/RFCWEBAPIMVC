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

    public class Cost_CentersController : ApiController
    {
        private saprfcdbEntities db = new saprfcdbEntities();

        // GET: api/cost_centers
        public IHttpActionResult Getcost_centers()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<cost_center> cost_center = db.cost_center.ToList();

                var cost_centerRecord = from cc in cost_center
                                        select new cost_center
                                        {
                                            cost_id = cc.cost_id,
                                            cost_name = cc.cost_name,
                                            cost_code = cc.cost_code,
                                            is_active = cc.is_active
                                        };

                return Json(cost_centerRecord);


            }
        }

        [Route("api/cost_centers/get_active_list")]
        [HttpGet]
        public IHttpActionResult get_active_list()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<cost_center> cost_center = db.cost_center.ToList();

                var cost_centerRecord = from cc in cost_center
                                        where cc.is_active == 1
                                        select new cost_center
                                        {
                                            cost_id = cc.cost_id,
                                            cost_name = cc.cost_name,
                                            cost_code = cc.cost_code,
                                            is_active = cc.is_active
                                        };

                return Json(cost_centerRecord);


            }
        }

        // GET: api/cost_centers/5
        [ResponseType(typeof(cost_center))]
        public IHttpActionResult Getcost_center(int id)
        {
            cost_center cost_center = db.cost_center.Find(id);
            if (cost_center == null)
            {
                return NotFound();
            }

            return Ok(cost_center);
        }


        // PUT: api/cost_centers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putcost_center(int id, cost_center cost_center)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cost_center.cost_id)
            {
                return BadRequest();
            }

            db.Entry(cost_center).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Json(new { message = "Successfull!" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!cost_centerExists(id))
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


        // POST: api/Cost_center
        [HttpPost]
        [ResponseType(typeof(cost_center))]
        public IHttpActionResult CreateCost_center(cost_center cost_center)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.cost_center.Add(cost_center);
            if (cost_center_codeExists(cost_center.cost_code))
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

        // DELETE: api/cost_centers/5
        [ResponseType(typeof(cost_center))]
        public IHttpActionResult Deletecost_center(int id)
        {
            cost_center cost_center = db.cost_center.Find(id);
            if (cost_center == null)
            {
                return NotFound();
            }

            db.cost_center.Remove(cost_center);
            db.SaveChanges();

            return Ok(cost_center);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool cost_centerExists(int id)
        {
            return db.cost_center.Count(e => e.cost_id == id) > 0;
        }

        private bool cost_center_codeExists(string id)
        {
            return db.cost_center.Count(e => e.cost_code == id.ToString()) > 0;
        }
    }
}