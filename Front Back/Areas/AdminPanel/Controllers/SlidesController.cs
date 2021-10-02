using Front_Back.DAL;
using Front_Back.Extension;
using Front_Back.Models;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Front_Back.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SlidesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public object Helper { get; private set; }

        public SlidesController(AppDbContext context, IWebHostEnvironment env) {

            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.Slides);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Slide slide)
        {

            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid)
                if(!slide.Photo.CheckType("image/"))
            {
                    ModelState.AddModelError("Photo","Please select image format");
                return View();
            }
            if (slide.Photo.Length / 1024 > 200)
            {
                ModelState.AddModelError("Photo", "Please select image must be less than 200kbb");
                return View();
            }
            string fileName = await slide.Photo.SaveFile("img",_env.WebRootPath);

            if (fileName != null)
            {
                Slide newSlide = new Slide
                {
                    Image = fileName
                };

                _context.Add(newSlide);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Some problem exist");
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> DeleteAsync (int? id)
        {
            if (id == null) return NotFound();
            Slide slide = await _context.Slides.FindAsync(id);
            if (slide == null) return NotFound();
            
            string resultPath = Path.Combine(_env.WebRootPath, "img", slide.Image);
            if (System.IO.File.Exists(resultPath)){
                System.IO.File.Delete(resultPath);
            }
         
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }

    
}
