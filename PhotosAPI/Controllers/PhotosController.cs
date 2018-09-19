using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PhotosAPI;

namespace PhotosAPI.Controllers
{
    public class PhotosController : ApiController
    {
        private bommuraj_photosEntities db = new bommuraj_photosEntities();

        // GET: api/Photos
        public IQueryable<Photo> GetPhotos()
        {
            return db.Photos;
        }

        // GET: api/Photos/5
        [ResponseType(typeof(Photo))]
        public IHttpActionResult GetPhoto(long id)
        {
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return NotFound();
            }

            return Ok(photo);
        }

        // PUT: api/Photos/5
        [Route("api/Photos/UpdatePhoto")]
        [HttpPost]
        public IHttpActionResult UpdatePhoto(Photo photo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (photo.PhotoID != photo.PhotoID)
            {
                return BadRequest();
            }

            db.Entry(photo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoExists(photo.PhotoID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Photos
        [Route("api/Photos/InsertPhoto")]
        [HttpPost]
        public IHttpActionResult InsertPhoto(Photo photo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Photos.Add(photo);
            db.SaveChanges();
            return Ok(new { response = photo });
            //return Json(photo, Configuration.Formatters.JsonFormatter);
        }

        // DELETE: api/Photos/5
        [Route("api/Photos/DeletePhoto")]
        [HttpPost]
        public IHttpActionResult DeletePhoto(Photo photo)
        {
            Photo Photo = db.Photos.Find(photo.PhotoID);
            if (photo == null)
            {
                return NotFound();
            }

            db.Photos.Remove(Photo);
            db.SaveChanges();

            return Ok(Photo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhotoExists(long id)
        {
            return db.Photos.Count(e => e.PhotoID == id) > 0;
        }
    }
}