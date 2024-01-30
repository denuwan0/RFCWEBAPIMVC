using RFCWEBAPIMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace RFCWEBAPIMVC.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class UsersController : ApiController
    {
        private saprfcdbEntities db = new saprfcdbEntities();

        // GET: api/users
        public IHttpActionResult Getusers()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<user> user = db.users.ToList();

                var userRecord = from u in user
                                 select new user
                                 {
                                     emp_id = u.emp_id,
                                     emp_epf = u.emp_epf,
                                     emp_name = u.emp_name,
                                     emp_password = u.emp_password,
                                     emp_function = u.emp_function,
                                     emp_location = u.emp_location,
                                     emp_plant = u.emp_plant,
                                     emp_company = u.emp_company,
                                     emp_user_grp = u.emp_user_grp,
                                     emp_user_sub_grp = u.emp_user_sub_grp,
                                     emp_email = u.emp_email,
                                     emp_mobile = u.emp_mobile,
                                     is_active = u.is_active
                                 };

                return Json(userRecord);


            }
        }


        [Route("api/users/get_active_list")]
        [HttpGet]
        // GET: api/users
        public IHttpActionResult get_active_list()
        {

            using (var ctx = new saprfcdbEntities())
            {

                List<user> user = db.users.ToList();

                var userRecord = from u in user
                                 where u.is_active == 1
                                 select new user
                                 {
                                     emp_id = u.emp_id,
                                     emp_epf = u.emp_epf,
                                     emp_name = u.emp_name,
                                     emp_password = u.emp_password,
                                     emp_function = u.emp_function,
                                     emp_location = u.emp_location,
                                     emp_plant = u.emp_plant,
                                     emp_company = u.emp_company,
                                     emp_user_grp = u.emp_user_grp,
                                     emp_user_sub_grp = u.emp_user_sub_grp,
                                     emp_email = u.emp_email,
                                     emp_mobile = u.emp_mobile,
                                     is_active = u.is_active
                                 };

                return Json(userRecord);


            }
        }

        [Route("api/users/sap_user_update")]
        [HttpGet]
        [ResponseType(typeof(user))]
        // GET: api/users
        public IHttpActionResult sap_user_update(int epf, int comp, int status, string token)
        {
            using (var ctx = new saprfcdbEntities())
            {
                List<user> user = db.users.ToList();
                List<company> company = db.companies.ToList();



                var joins = from u in user
                            join c in company on u.emp_company equals c.company_id
                            where c.company_code == comp && u.emp_epf == epf
                            select new UserValidate
                            {
                                emp_epf = u.emp_epf,
                                emp_id = u.emp_id,
                                emp_company = c.company_code,
                                emp_company_id = u.emp_company,
                                is_active = u.is_active
                            };

                string token_string = "token_string";

                if (userEpfExists(epf))
                {
                    if (token_string == token)
                    {

                        foreach (var val1 in joins)
                        {
                            foreach (var val2 in user.Where(u => u.emp_id == val1.emp_id))
                            {
                                val2.is_active = (byte)status;
                                db.SaveChanges();
                            }
                        }


                        return Json(new { message = "Successfull!" });
                    }
                    else
                    {
                        return Json(new { message = "Failed!" });
                    }

                    
                }
                else
                {

                    return Json(new { message = "User Not Found!" });

                }
            }

        }

        [Route("api/users/sap_user_create")]
        [HttpGet]
        [ResponseType(typeof(user))]
        // GET: api/users
        public IHttpActionResult sap_user_create(int epf, int comp, string name, int status, string token)
        {
            using (var ctx = new saprfcdbEntities())
            {
                List<company> companyDetails = db.companies.ToList();

                var joins = from c in companyDetails
                            where c.company_code == comp
                            select new UserValidate
                            {
                                emp_company = c.company_code,
                                emp_company_id = c.company_id
                            };

                string token_string = "token_string";

                if (userEpfExists(epf))
                {
                    return Json(new { message = "Already Exists!" });
                }
                else
                {

                    if (token_string == token)
                    {


                        foreach (var val in joins)
                        {
                            user user = new user();

                            string password = epf.ToString();
                            string email = 0.ToString();
                            int mobile = 0;
                            byte is_active = (Byte)status;

                            var passHashValue = string.Join("", MD5.Create().ComputeHash(
                            Encoding.ASCII.GetBytes(password)).Select(s => s.ToString("x2")));

                            user.emp_epf = epf;
                            user.emp_user_grp = 0;
                            user.emp_user_sub_grp = 0;
                            user.emp_name = name;
                            user.emp_email = email;
                            user.emp_password = passHashValue;
                            user.emp_company = val.emp_company_id;
                            user.emp_function = 0;
                            user.emp_location = 0;
                            user.emp_plant = 0;
                            user.emp_mobile = mobile;
                            user.is_active = is_active;
                            db.users.Add(user);
                            db.SaveChanges();


                        }
                        return Json(new { message = "Successfull!" });

                    }
                    else
                    {
                        return Json(new { message = "Failed!" });
                    }

                }

            }

        }

        // GET: api/users/5
        [ResponseType(typeof(user))]
        public IHttpActionResult Getuser(int id)
        {
            user user = db.users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        // PUT: api/users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putusern(int id, user user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.emp_id)
            {
                return BadRequest();
            }

            var passHashValue = string.Join("", MD5.Create().ComputeHash(
                 Encoding.ASCII.GetBytes(user.emp_password)).Select(s => s.ToString("x2")));

            user.emp_password = passHashValue;

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Json(new { message = "Successfull!" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!userExists(id))
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

        // POST: api/User
        [HttpPost]
        [ResponseType(typeof(user))]
        public IHttpActionResult CreateUser(user data)
        {
            using (var ctx = new saprfcdbEntities())
            {
                // List<user> user = db.users.ToList();
                //var str = from usg in user select usg.sub_group_id;

                if (userEpfExists(data.emp_epf))
                {
                    return Json(new { message = "Already Exists!" });
                }
                else
                {
                    try
                    {
                        user user = new user();

                        var passHashValue = string.Join("", MD5.Create().ComputeHash(
                        Encoding.ASCII.GetBytes(data.emp_password)).Select(s => s.ToString("x2")));

                        user.emp_epf = data.emp_epf;
                        user.emp_user_grp = data.emp_user_grp;
                        user.emp_user_sub_grp = data.emp_user_sub_grp;
                        user.emp_name = data.emp_name;
                        user.emp_email = data.emp_email;
                        user.emp_password = passHashValue;
                        user.emp_company = data.emp_company;
                        user.emp_function = data.emp_function;
                        user.emp_location = data.emp_location;
                        user.emp_plant = data.emp_plant;
                        user.emp_mobile = data.emp_mobile;
                        user.is_active = data.is_active;

                        db.users.Add(user);
                        db.SaveChanges();

                        return Json(new { message = "Successfull!" });
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return Json(new { message = "Failed!" });
                    }


                }

            }
            //return CreatedAtRoute("DefaultApi", new { id = location.loc_id }, location);
        }


        // POST: api/user
        //[HttpPost]
        //[ResponseType(typeof(user))]
        //public IHttpActionResult CreateUsern(user user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.users.Add(user);

        //    try
        //    {

        //        db.SaveChanges();
        //        return Json(new { message = "Successful" });
        //    }
        //    catch (DbUpdateException Dbex)
        //    {
        //        if (userExists(user.emp_id))
        //        {
        //            //return Conflict();
        //            db.Entry(user).State = EntityState.Modified;
        //        }
        //        else
        //        {
        //            throw Dbex;
        //        }
        //        return Json(new { message = "Failed" });
        //    }


        //    //return CreatedAtRoute("DefaultApi", new { id = location.loc_id }, location);
        //}

        // DELETE: api/users/5
        [ResponseType(typeof(user))]
        public IHttpActionResult Deleteuser(int id)
        {
            user user = db.users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool userExists(int id)
        {
            return db.users.Count(e => e.emp_id == id) > 0;
        }

        private bool userEpfExists(int id)
        {
            return db.users.Count(e => e.emp_epf == id) > 0;
        }
    }
}