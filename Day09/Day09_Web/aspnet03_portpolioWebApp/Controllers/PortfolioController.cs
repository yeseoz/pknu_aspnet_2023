using aspnet02_boardapp.Data;
using aspnet03_portpolioWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;

namespace aspnet03_portpolioWebApp.Controllers
{
    public class PortfolioController : Controller
    {
        public readonly ApplicationDbContext _db;

        // 파일 업로드 웹환경 객체(필수!)
        private readonly IWebHostEnvironment _environment;

        public PortfolioController(ApplicationDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var list = _db.Portfolios.ToList(); // SELECT *
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // PortfolioModel(X) -> TempPortfolioMode선택(o)
            return View();
        }

        [HttpPost]
        public IActionResult Create(TempPortfolioModel temp)
        {
            // 파일 업로드, temp -> Model db 저장
            if(ModelState.IsValid)
            {
                // 파일 업로드되면 새로운 이미지 파일 명을 받음
                string upFileName = UploadImageFile(temp);

                // TempPortfolioModel -> PortfolioModel 변경
                var portfolio = new PortfolioModel()
                {
                    Division = temp.Division,
                    Title = temp.Title,
                    Description = temp.Description,
                    Url = temp.Url,
                    FileName = upFileName // 핵심입니다.
                };

                _db.Portfolios.Add(portfolio);
                _db.SaveChanges();

                TempData["succeed"] = "포트폴리오 저장완료!";
                return RedirectToAction("Index", "Portfolio");
            }
            return View(temp);

        }

        // Routing이나 GET/POST랑 관계없는 내부 메서드
        private string UploadImageFile(TempPortfolioModel temp)
        {
            var resultFileName = "";
            try
            {
                if (temp.PortfolioImage != null)
                {
                    // 파일 업로드 로직
                    string uploadFolder = Path.Combine(_environment.WebRootPath, "uploads"); // wwwroot 밑에 uploads라는 폴더 존재
                    resultFileName = Guid.NewGuid() + "_" + temp.PortfolioImage.FileName; // 중복된 이미지 파일명 제거
                    string filePath = Path.Combine(uploadFolder, resultFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        temp.PortfolioImage.CopyTo(fileStream);
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.Write(ex.Message);
            }


            

            return resultFileName;
        }
    }
}
