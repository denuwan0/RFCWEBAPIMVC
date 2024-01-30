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

    public class UserGroupsController : ApiController
    {
        private saprfcdbEntities db = new saprfcdbEntities();

        // GET: api/user_groups
        public IHttpActionResult Getuser_groups()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<user_group> user_group = db.user_group.ToList();

                var user_groupRecord = from ug in user_group
                                       select new user_group
                                       {
                                           grp_id = ug.grp_id,
                                           grp_name = ug.grp_name,
                                           grp_desc = ug.grp_desc,
                                           is_active = ug.is_active
                                       };

                return Json(user_groupRecord);


            }
        }

        [Route("api/user_groups/get_active_list")]
        [HttpGet]
        // GET: api/user_groups
        public IHttpActionResult get_active_list()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<user_group> user_group = db.user_group.ToList();

                var user_groupRecord = from ug in user_group
                                       where ug.is_active == 1
                                       select new user_group
                                       {
                                           grp_id = ug.grp_id,
                                           grp_name = ug.grp_name,
                                           grp_desc = ug.grp_desc,
                                           is_active = ug.is_active
                                       };

                return Json(user_groupRecord);


            }
        }

        // GET: api/user_groups/5
        [ResponseType(typeof(user_group))]
        public IHttpActionResult Getuser_group(int id)
        {
            user_group user_group = db.user_group.Find(id);
            if (user_group == null)
            {
                return NotFound();
            }

            return Ok(user_group);
        }


        // PUT: api/user_groups/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putuser_group(int id, user_group user_group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user_group.grp_id)
            {
                return BadRequest();
            }

            db.Entry(user_group).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Json(new { message = "Successfull!" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!user_groupExists(id))
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


        // POST: api/User_group
        //[HttpPost]
        [ResponseType(typeof(user_group))]
        public IHttpActionResult CreateUser_group(user_group user_group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.user_group.Add(user_group);
            if (user_groupNameExists(user_group.grp_name))
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

        // DELETE: api/user_groups/5
        [ResponseType(typeof(user_group))]
        public IHttpActionResult Deleteuser_group(int id)
        {
            user_group user_group = db.user_group.Find(id);
            if (user_group == null)
            {
                return NotFound();
            }

            db.user_group.Remove(user_group);
            db.SaveChanges();

            return Ok(user_group);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool user_groupExists(int id)
        {
            return db.user_group.Count(e => e.grp_id == id) > 0;
        }

        private bool user_groupNameExists(string id)
        {
            return db.user_group.Count(e => e.grp_name == id) > 0;
        }
    }
}