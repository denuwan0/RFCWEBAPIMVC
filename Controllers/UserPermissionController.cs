using Newtonsoft.Json.Linq;
using RFCWEBAPIMVC.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace RFCWEBAPIMVC.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class UserPermissionController : ApiController
    {
        private saprfcdbEntities db = new saprfcdbEntities();

        // GET: api/user_permissions
        public IHttpActionResult Getuser_permissions()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<user_permission> user_permission = db.user_permission.ToList();

                var user_permissionRecord = from up in user_permission
                                            select new user_permission
                                            {
                                                perm_id = up.perm_id,
                                                user_sub_grp_id = up.user_sub_grp_id,
                                                url = up.url,
                                                url_type = up.url_type,
                                                is_view = up.is_view,
                                                is_edit = up.is_edit,
                                                is_active = up.is_active
                                            };

                return Json(user_permissionRecord);


            }
        }



        [Route("api/user_permissions/byGrpId")]
        [HttpGet]
        // GET: api/user_permissions
        public IHttpActionResult byGrpId(int byGrpId)
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<user_permission> user_permission = db.user_permission.ToList();

                var user_permissionRecord = from up in user_permission
                                            where up.user_sub_grp_id == byGrpId
                                            select new user_permission
                                            {
                                                perm_id = up.perm_id,
                                                user_sub_grp_id = up.user_sub_grp_id,
                                                url = up.url,
                                                url_type = up.url_type,
                                                is_view = up.is_view,
                                                is_edit = up.is_edit,
                                                is_active = up.is_active
                                            };

                return Json(user_permissionRecord);


            }
        }

        [Route("api/user_permissions/activeListByGrpId1")]
        [HttpGet]
        // GET: api/user_permissions
        public IHttpActionResult activeListByGrpId1(int sub_id)
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<user_permission> user_permission = db.user_permission.ToList();
                List<user_sub_group> user_sub_group = db.user_sub_group.ToList();
                List<url> url = db.urls.ToList();
                List<url_type> url_type = db.url_type.ToList();

                var user_permissionRecord = from up in user_permission
                                            join ut in url_type
                                            on up.url_type equals ut.url_type_id
                                            where ut.is_active == 1 && up.user_sub_grp_id == sub_id
                                            && up.is_active == 1
                                            group ut by new
                                            {
                                                ut.url_type_id,
                                                ut.url_type_name
                                            } into type
                                            select new UrlTypeDetails
                                            {
                                                url_type_id = type.Key.url_type_id,
                                                url_type_name = type.Key.url_type_name,
                                            };



                return Json(user_permissionRecord);

            }
        }

        [Route("api/user_permissions/activeListByGrpId")]
        [HttpGet]
        // GET: api/user_permissions
        public IHttpActionResult activeListByGrpId(int sub_id, int url_type_id)
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<user_permission> user_permission = db.user_permission.ToList();
                List<user_sub_group> user_sub_group = db.user_sub_group.ToList();
                List<url> url = db.urls.ToList();
                List<url_type> url_type = db.url_type.ToList();

                var user_permissionRecord = from up in user_permission
                                            join usg in user_sub_group
                                            on up.user_sub_grp_id equals usg.sub_group_id
                                            join u in url
                                            on up.url equals u.url_id
                                            join ut in url_type
                                            on up.url_type equals ut.url_type_id
                                            where usg.is_active == 1 && u.is_active == 1
                                            && ut.is_active == 1 && up.user_sub_grp_id == sub_id
                                            && up.url_type == url_type_id && up.is_active == 1
                                            select new ActiveUserPerm
                                            {
                                                perm_id = up.perm_id,
                                                sub_grp_id = up.user_sub_grp_id,
                                                sub_grp_name = usg.sub_group_name,
                                                url_id = up.url,
                                                menu_name = u.menu_name,
                                                url_value = u.url_value,
                                                url_type = up.url_type,
                                                url_type_name = ut.url_type_name,
                                                is_view = up.is_view,
                                                is_edit = up.is_edit,
                                                is_active = up.is_active
                                            };

                return Json(user_permissionRecord);

            }
        }

        [Route("api/user_permissions/activeListByGrpIdPermUpdate")]
        [HttpGet]
        // GET: api/user_permissions
        public IHttpActionResult activeListByGrpIdPermUpdate(int sub_id)
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<user_permission> user_permission = db.user_permission.ToList();
                List<user_sub_group> user_sub_group = db.user_sub_group.ToList();
                List<url> url = db.urls.ToList();
                List<url_type> url_type = db.url_type.ToList();

                var user_permissionRecord = from up in user_permission
                                            join u in url
                                            on up.url equals u.url_id
                                            join ut in url_type
                                            on up.url_type equals ut.url_type_id
                                            join sg in user_sub_group
                                            on up.user_sub_grp_id equals sg.sub_group_id
                                            where u.is_active == 1
                                            && ut.is_active == 1 && up.user_sub_grp_id == sub_id
                                            select new ActiveUserPerm
                                            {
                                                perm_id = up.perm_id,
                                                sub_grp_id = up.user_sub_grp_id,
                                                sub_grp_name = sg.sub_group_name,
                                                url_id = up.url,
                                                menu_name = u.menu_name,
                                                url_value = u.url_value,
                                                url_type = up.url_type,
                                                url_type_name = ut.url_type_name,
                                                is_view = up.is_view,
                                                is_edit = up.is_edit,
                                                is_active = up.is_active
                                            };

                return Json(user_permissionRecord);

            }
        }

        [Route("api/user_permissions/deletePermByGrpId")]
        [HttpGet]
        // GET: api/user_permissions
        public IHttpActionResult deletePermByGrpId(int byGrpId)
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<user_permission> user_permission = db.user_permission.ToList();


                var user_permissionRecord = from up in user_permission
                                            where up.user_sub_grp_id == byGrpId
                                            select new user_permission
                                            {
                                                perm_id = up.perm_id
                                            };



                if (user_permission == null)
                {
                    return NotFound();
                }

                for (int i = 0; i < user_permission.Count; i++)
                {
                    db.user_permission.Remove(user_permission[i]);
                    db.SaveChanges();

                }

                return Json(new { message = "Successfull!" });


            }
        }


        // GET: api/user_permissions/5
        [ResponseType(typeof(user_permission))]
        public IHttpActionResult Getuser_permission(int id)
        {
            user_permission user_permission = db.user_permission.Find(id);
            if (user_permission == null)
            {
                return NotFound();
            }

            return Ok(user_permission);
        }


        // PUT: api/user_permissions/5
        [Route("api/user_permissions/updatePermission")]
        [HttpPut]
        public IHttpActionResult updatePermission(JArray data)
        {
            using (var ctx = new saprfcdbEntities())
            {

                foreach (JObject item in data)
                {
                    List<user_permission> user_permission = db.user_permission.ToList();

                    foreach (var val in user_permission.Where(w => w.perm_id == (int)item.GetValue("perm_id")))
                    {
                        val.is_active = (byte)item.GetValue("is_active");
                        val.is_view = (byte)item.GetValue("is_view");
                        val.is_edit = (byte)item.GetValue("is_edit");
                    }

                }

                db.SaveChanges();
                return Json(new { message = "Successfull!" });
            }
        }


        // POST: api/User_permission
        [HttpPost]
        [ResponseType(typeof(user_permission))]
        public IHttpActionResult CreateUser_permission(user_permission data)
        {
            using (var ctx = new saprfcdbEntities())
            {
                List<user_sub_group> user_sub_group = db.user_sub_group.ToList();
                var str = from usg in user_sub_group select usg.sub_group_id;

                foreach (var user_sub_group_id in str)
                {

                    user_permission userPerm = new user_permission();

                    userPerm.user_sub_grp_id = user_sub_group_id;
                    userPerm.url = data.url;
                    userPerm.url_type = data.url_type;
                    userPerm.is_view = data.is_view;
                    userPerm.is_edit = data.is_edit;
                    userPerm.is_active = data.is_active;
                    db.user_permission.Add(userPerm);
                    db.SaveChanges();

                }

                return Json(new { message = "Successfull!" });
            }
            //return CreatedAtRoute("DefaultApi", new { id = location.loc_id }, location);
        }



        // DELETE: api/user_permissions/5
        [ResponseType(typeof(user_permission))]
        public IHttpActionResult Deleteuser_permission(int id)
        {
            user_permission user_permission = db.user_permission.Find(id);
            if (user_permission == null)
            {
                return NotFound();
            }

            db.user_permission.Remove(user_permission);
            db.SaveChanges();

            return Ok(user_permission);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool user_permissionExists(int id)
        {
            return db.user_permission.Count(e => e.perm_id == id) > 0;
        }
    }
}