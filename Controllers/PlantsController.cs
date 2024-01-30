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

    public class PlantsController : ApiController
    {
        private saprfcdbEntities db = new saprfcdbEntities();

        // GET: api/plants
        public IHttpActionResult Getplants()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<plant> plant = db.plants.ToList();

                var plantRecord = from p in plant
                                  select new plant
                                  {
                                      plant_id = p.plant_id,
                                      plant_name = p.plant_name,
                                      plant_code = p.plant_code,
                                      loc_id = p.loc_id,
                                      cost_center = p.cost_center,
                                      is_active = p.is_active
                                  };

                return Json(plantRecord);


            }
        }

        [Route("api/plants/GetpActivelants")]
        [HttpGet]
        public IHttpActionResult GetpActivelants()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<plant> plant = db.plants.ToList();

                var plantRecord = from p in plant
                                  where p.is_active == 1
                                  select new plant
                                  {
                                      plant_id = p.plant_id,
                                      plant_name = p.plant_name,
                                      plant_code = p.plant_code,
                                      loc_id = p.loc_id,
                                      cost_center = p.cost_center,
                                      is_active = p.is_active
                                  };

                return Json(plantRecord);


            }
        }

        // GET: api/plants/5
        [ResponseType(typeof(plant))]
        public IHttpActionResult Getplant(int id)
        {
            plant plant = db.plants.Find(id);
            if (plant == null)
            {
                return NotFound();
            }

            return Ok(plant);
        }


        // PUT: api/plants/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putplant(int id, plant plant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != plant.plant_id)
            {
                return BadRequest();
            }

            db.Entry(plant).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Json(new { message = "Successfull!" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!plantExists(id))
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


        // POST: api/plant
        //[HttpPost]
        [ResponseType(typeof(plant))]
        public IHttpActionResult CreatePlant(plant plant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.plants.Add(plant);

            if (plantNameExists(plant.plant_name))
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

        // DELETE: api/plants/5
        [ResponseType(typeof(plant))]
        public IHttpActionResult Deleteplant(int id)
        {
            plant plant = db.plants.Find(id);
            if (plant == null)
            {
                return NotFound();
            }

            db.plants.Remove(plant);
            db.SaveChanges();

            return Ok(plant);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool plantExists(int id)
        {
            return db.plants.Count(e => e.plant_id == id) > 0;
        }
        private bool plantNameExists(string id)
        {
            return db.plants.Count(e => e.plant_name == id) > 0;
        }
    }
}