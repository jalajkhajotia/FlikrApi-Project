using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyExample.Controllers;
using MyExample.Models;
using Newtonsoft.Json;

namespace MyExample.Controllers
{
    public class FlickrImage
    {
      
      
        public Uri Image320 { get; set; }
        public Uri Image1024 { get; set; }
        public Uri ImageOriginal { get; set; }
        public Uri ImagePage { get; set; }
        public Photo photo { get; set; }
        public string keyword { get; set; }

        public async static Task<List<FlickrImage>> GetFlickrImages(string keyword)
        {  HttpClient client = new HttpClient();
        string flickrApiKey = "656e36e5e04aed1878962f02cb2a661d";
       // var baseUrl = string.Format("https://api.flickr.com/services/rest/?method=flickr.photos.search&license=4%2C5%2C6%2C7&api_key={0}&format=json&nojsoncallback=1", flickrApiKey);

       // if (!string.IsNullOrWhiteSpace(keyword))
          //  baseUrl += string.Format("&text=%22{0}%22", keyword);
        string baseUrl = "https://api.flickr.com/services/rest/?method=flickr.photos.search&api_key=656e36e5e04aed1878962f02cb2a661d&tags=" + keyword + "&format=json&nojsoncallback=1";
            string msg =  await client.GetStringAsync(baseUrl);
            FlickrData apiData = JsonConvert.DeserializeObject <FlickrData>(msg);
            List<FlickrImage> images = new List<FlickrImage>();
            
            if (apiData.stat == "ok")
			{
				foreach (Photo data in apiData.photos.photo)
				{
					//http://farm{farm-id}.staticflickr.com/{server-id}/{id}_{secret}{size}.jpg
					FlickrImage img = new FlickrImage();
                    img.photo = data;
                    img.photo.keyword = keyword;
                   
					string baseFlickrUrl = string.Format(
						"http://farm{0}.staticflickr.com/{1}/{2}_{3}", 
						data.farm, data.server, data.id, data.secret);

					img.Image320= new Uri(baseFlickrUrl + "_n.jpg");
					img.Image1024= new Uri(baseFlickrUrl + "_b.jpg");
					img.ImageOriginal = new Uri(baseFlickrUrl + ".jpg");

                    HttpClient cli = new HttpClient();
                  byte[] bytes= await  cli.GetByteArrayAsync(img.Image320);
                 
                 // FileStream fs = new FileStream(string.Format("{2}_{3}_n", data.id, data.secret), FileMode.Open, FileAccess.Read);
                  //int iBytesRead = fs.Read(m_barrImg, 0,
                  //               Convert.ToInt32(this.m_lImageFileLength));
                  //fs.Close();

                  img.photo.image320 = bytes;

					//http://www.flickr.com/photos/{user-id}/{photo-id}
					string imagePage = string.Format("http://www.flickr.com/photos/{0}/{1}", data.owner, data.id);
					img.ImagePage = new Uri(imagePage);

					images.Add(img);
				}
			}

			return images;
		}
        }
    
    }


    public class PhotoController : Controller
    {
        private MovieDBContext db = new MovieDBContext();

        // GET: /Photo/

        public async Task<ActionResult> Index(string keyword)
        {
           
            return await Index(keyword,1);
         
        }

        [HttpPost]
        public  async Task<ActionResult> Index(string keyword,int z=2)
        {
            var allPhotos = from a in db.Photos
                            select a;

            var abc = allPhotos.Any(x => x.keyword == keyword);
          
          
            if (abc)
            {
                var selectedPhotos = from x in allPhotos
                                     where x.keyword == keyword
                                     select x;

                //<a href= @(string.Format(
                //     "http://farm{0}.staticflickr.com/{1}/{2}_{3}.jpg",	 item.farm,item.server, item.id, item.secret).ToString()) > <img src= @(string.Format(
                //         "http://farm{0}.staticflickr.com/{1}/{2}_{3}_n.jpg",	 item.farm,item.server, item.id, item.secret).ToString()) /> </a>@*
                List<FlickrImage> images = new List<FlickrImage>();
                foreach (var p in selectedPhotos)
                {
                    FlickrImage obj = new FlickrImage();
                    obj.Image1024 = (new Uri(string.Format(
                    "http://farm{0}.staticflickr.com/{1}/{2}_{3}.jpg", p.farm, p.server, p.id, p.secret)));
                    obj.Image320 = new Uri(string.Format(
                    "http://farm{0}.staticflickr.com/{1}/{2}_{3}_n.jpg", p.farm, p.server, p.id, p.secret));
                    obj.ImagePage = new Uri(string.Format("http://www.flickr.com/photos/{0}/{1}", p.owner, p.id));
                    images.Add(obj);

                }
                return View(images);
            }
            List<Photo> photos = new List<Photo>();
            List<FlickrImage> imgs = await FlickrImage.GetFlickrImages(keyword);
            foreach (var p in imgs)
            {
                photos.Add(p.photo);
            }

            Create(photos);
            return View(imgs);
        }

        //
        // GET: /Photo/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult All()
        {
           IEnumerable<Photo> photos = from m in db.Photos
                         select m;

           if (photos.Count()!=0)
            {
                return View(photos);
            }


           return View(new Photo());
        }


        //
        // GET: /Photo/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Photo/Create

      //  [HttpPost]
        public void Create(List<Photo> photos)
        {
            var allPhotos = from a in db.Photos
                            select a;

            if (ModelState.IsValid)
            {
                foreach (var photo in photos)
                {
                    if (allPhotos.SingleOrDefault(x=>x.id==photo.id)==null)
                    db.Photos.Add(photo);
                }

                db.SaveChanges();
              //  return RedirectToAction("Index");
            }
           // return View(photos);
        }

    

        //
        // GET: /Photo/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Photo/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Photo/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Photo/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
