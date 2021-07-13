using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProductClient.Controllers
{
    public class ProductController : Controller
    {
        string Baseurl = "https://localhost:44380/";
        public async Task<IActionResult> GetAllProducts()
        {
            List<Product> ProductInfo = new List<Product>();

            using (var client = new HttpClient())
            {
                
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                 HttpResponseMessage Res = await client.GetAsync("api/Products");
                      
                if (Res.IsSuccessStatusCode)
                {
                    var ProductResponse = Res.Content.ReadAsStringAsync().Result;
                    ProductInfo = JsonConvert.DeserializeObject<List<Product>>(ProductResponse);

                }
              return View(ProductInfo);
            }

        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product p)
        {
            Product Pobj = new Product();
          //  HttpClient obj = new HttpClient();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("api/Products", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Pobj = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            }
            return RedirectToAction("GetAllProducts");
        }

        public async Task<IActionResult> Edit(int id)
        {
            Product p = new Product();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44380/api/Products/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    p = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            }
            return View(p);
            
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product p)
        {
            Product p1 = new Product();
            using (var httpClient = new HttpClient())
            {
                int id = p.pid;
                StringContent content1 = new StringContent(JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("https://localhost:44380/api/Products/" + id, content1))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    p1 = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            }
            return RedirectToAction("GetAllProducts");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            TempData["Prid"] = id;
            Product e = new Product();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44380/api/Products/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    e = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            }
            return View(e);

        }

        [HttpPost]
        public async Task<ActionResult> Delete(Product p)
        {
            int prid = Convert.ToInt32(TempData["Prid"]);
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:44380/api/Products/" + prid))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("GetAllProducts");
        }
    }
}
