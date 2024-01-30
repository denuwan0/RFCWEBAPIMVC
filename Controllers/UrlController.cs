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

    public class UrlsController : ApiController
    {
        private saprfcdbEntities db = new saprfcdbEntities();

        // GET: api/urls
        public IHttpActionResult Get_url()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<url> url = db.urls.ToList();

                var t_codeRecord = from ur in url
                                   select new url
                                   {
                                       url_id = ur.url_id,
                                       url_type = ur.url_type,
                                       url_value = ur.url_value,
                                       menu_name = ur.menu_name,
                                       is_active = ur.is_active
                                   };

                return Json(t_codeRecord);


            }
        }

        [Route("api/urls/get_active_list")]
        [HttpGet]
        // GET: api/t_codes
        public IHttpActionResult get_active_list()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<url> url = db.urls.ToList();

                var t_codeRecord = from ur in url
                                   where ur.is_active == 1
                                   select new url
                                   {
                                       url_id = ur.url_id,
                                       url_type = ur.url_type,
                                       url_value = ur.url_value,
                                       menu_name = ur.menu_name,
                                       is_active = ur.is_active
                                   };

                return Json(t_codeRecord);


            }
        }

        [Route("api/urls/get_active_url_details")]
        [HttpGet]
        // GET: api/t_codes
        public IHttpActionResult get_active_list_details()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<url> url = db.urls.ToList();
                List<url_type> url_type = db.url_type.ToList();
                List<user_permission> user_Permission = db.user_permission.ToList();
                List<user_sub_group> user_sub_group = db.user_sub_group.ToList();


                var urlDetailsRecord = from u in url
                                       join up in user_Permission
                                       on u.url_id equals up.url
                                       join ut in url_type
                                       on u.url_type equals ut.url_type_id
                                       join usg in user_sub_group
                                       on up.user_sub_grp_id equals usg.sub_group_id
                                       where u.is_active == 1 && ut.is_active == 1
                                       select new ActiveUserPerm
                                       {

                                           url_id = u.url_id,
                                           url_value = u.url_value,
                                           menu_name = u.menu_name,
                                           perm_id = up.perm_id,
                                           sub_grp_id = up.user_sub_grp_id,
                                           sub_grp_name = usg.sub_group_name,
                                           url_type = u.url_type,
                                           url_type_name = ut.url_type_name,
                                           is_active = up.is_active,
                                           is_view = up.is_view,
                                           is_edit = up.is_edit,

                                       };

                return Json(urlDetailsRecord);

            }
        }


        // GET: api/t_codes/5
        [ResponseType(typeof(url))]
        public IHttpActionResult GetUrl(int id)
        {
            url url = db.urls.Find(id);
            if (url == null)
            {
                return NotFound();
            }

            return Ok(url);
        }


        // PUT: api/t_codes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUrl(int id, url url)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != url.url_id)
            {
                return BadRequest();
            }

            db.Entry(url).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Json(new { message = "Successfull!" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!urlExists(id))
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
        [ResponseType(typeof(url))]
        public IHttpActionResult CreateUrl(url url)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.urls.Add(url);

            try
            {

                db.SaveChanges();
                //return last inserted  values
                int url_id = url.url_id;
                int url_type = url.url_type;
                return Json(new { message = "Successfull!", url_id = url_id, url_type = url_type });
            }
            catch (DbUpdateException Dbex)
            {
                if (urlExists(url.url_id))
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



        // DELETE: api/t_codes/5
        [ResponseType(typeof(url))]
        public IHttpActionResult Deleteurl(int id)
        {
            url url = db.urls.Find(id);
            if (url == null)
            {
                return NotFound();
            }

            db.urls.Remove(url);
            db.SaveChanges();

            return Ok(url);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool urlExists(int id)
        {
            return db.urls.Count(e => e.url_id == id) > 0;
        }
    }
}