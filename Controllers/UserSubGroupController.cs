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

    public class UserSubGroupsController : ApiController
    {
        private saprfcdbEntities db = new saprfcdbEntities();

        // GET: api/user_sub_groups
        public IHttpActionResult Getuser_sub_groups()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<user_sub_group> user_sub_group = db.user_sub_group.ToList();

                var user_sub_groupRecord = from usg in user_sub_group
                                           select new user_sub_group
                                           {
                                               sub_group_id = usg.sub_group_id,
                                               group_id = usg.group_id,
                                               sub_group_name = usg.sub_group_name,
                                               sub_group_desc = usg.sub_group_desc,
                                               is_active = usg.is_active
                                           };

                return Json(user_sub_groupRecord);


            }
        }

        [Route("api/user_sub_groups/get_active_list")]
        [HttpGet]
        // GET: api/user_sub_groups
        public IHttpActionResult get_active_list()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<user_sub_group> user_sub_group = db.user_sub_group.ToList();

                var user_sub_groupRecord = from usg in user_sub_group
                                           where usg.is_active == 1
                                           select new user_sub_group
                                           {
                                               sub_group_id = usg.sub_group_id,
                                               group_id = usg.group_id,
                                               sub_group_name = usg.sub_group_name,
                                               sub_group_desc = usg.sub_group_desc,
                                               is_active = usg.is_active
                                           };

                return Json(user_sub_groupRecord);


            }
        }





        // GET: api/user_sub_groups/5
        [ResponseType(typeof(user_sub_group))]
        public IHttpActionResult Getuser_sub_group(int id)
        {
            user_sub_group user_sub_group = db.user_sub_group.Find(id);
            if (user_sub_group == null)
            {
                return NotFound();
            }

            return Ok(user_sub_group);
        }


        // PUT: api/user_sub_groups/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putuser_sub_group(int id, user_sub_group user_sub_group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user_sub_group.sub_group_id)
            {
                return BadRequest();
            }

            db.Entry(user_sub_group).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Json(new { message = "Successfull!" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!user_sub_groupExists(id))
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


        // POST: api/User_sub_group
        //[HttpPost]
        [ResponseType(typeof(user_sub_group))]
        public IHttpActionResult Createuser_sub_group(user_sub_group user_sub_group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.user_sub_group.Add(user_sub_group);
            if (user_sub_groupNameExists(user_sub_group.sub_group_name))
            {
                return Json(new { message = "Already Exists!" });
            }
            else
            {
                try
                {

                    db.SaveChanges();
                    int sub_grp_id = user_sub_group.sub_group_id;

                    using (var ctx = new saprfcdbEntities())
                    {
                        List<url> url = db.urls.ToList();

                        var urlRecord = from u in url
                                            //where u.is_active == 1
                                        select new url
                                        {
                                            url_id = u.url_id,
                                            url_type = u.url_type,
                                            url_value = u.url_value,
                                            is_active = u.is_active
                                        };

                        foreach (var url_detail in urlRecord)
                        {

                            user_permission userPerm = new user_permission();

                            userPerm.user_sub_grp_id = sub_grp_id;
                            userPerm.url = url_detail.url_id;
                            userPerm.url_type = url_detail.url_type;
                            userPerm.is_view = 0;
                            userPerm.is_edit = 0;
                            userPerm.is_active = 0;
                            db.user_permission.Add(userPerm);
                            db.SaveChanges();

                        }

                        return Json(new { message = "Successfull!" });
                    }


                }
                catch (DbUpdateException Dbex)
                {
                    return Json(new { message = "Failed!" });
                }
            }




            //return CreatedAtRoute("DefaultApi", new { id = location.loc_id }, location);
        }

        // DELETE: api/user_sub_groups/5
        [ResponseType(typeof(user_sub_group))]
        public IHttpActionResult Deleteuser_sub_group(int id)
        {
            user_sub_group user_sub_group = db.user_sub_group.Find(id);
            if (user_sub_group == null)
            {
                return NotFound();
            }

            db.user_sub_group.Remove(user_sub_group);
            db.SaveChanges();

            return Ok(user_sub_group);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool user_sub_groupExists(int id)
        {
            return db.user_sub_group.Count(e => e.group_id == id) > 0;
        }

        private bool user_sub_groupNameExists(string id)
        {
            return db.user_sub_group.Count(e => e.sub_group_name == id) > 0;
        }
    }
}