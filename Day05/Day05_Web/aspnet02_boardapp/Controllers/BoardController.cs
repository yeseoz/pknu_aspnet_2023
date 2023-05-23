using aspnet02_boardapp.Data;
using aspnet02_boardapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace aspnet02_boardapp.Controllers
{
    // https://localhost:7800/Board/Index
    public class BoardController : Controller
    {

        private readonly ApplicationDbContext _db;

        public BoardController(ApplicationDbContext db)
        {
            _db = db;   // 알아서 DB가 연결
        }

        // startcount = 1, 11, 21, 31, 41, ...
        // endcount = 10, 20, 30, 40, 50
        public IActionResult Index(int page = 1)    // 게시판 최초 화면 리스트
        {
            // IEnumerable<Board> objBoardList = _db.Boards.ToList();  //SELECT쿼리
            //var objBoardList = _db.Boards.FromSql(@$"SELECT *  FROM boards").ToList();
            var totalCount = _db.Boards.Count(); // 전체 글 갯수
            var pageSize = 10; // 게시판 한 페이지에 10개씩 리스트 업\
            var totalPage = totalCount / pageSize;

            // 제[일 첫번재 페이지, 제일 마지막 페이지
            if (totalCount % pageSize > 0)
            {
                totalPage++; // 나머지 글이 있으면 전체 페이지를 1 증가
            }

            var countPage = 10;
            var startPage = ((page - 1) / countPage * countPage + 1);
            var endPage = startPage + countPage - 1;
            if(totalPage< endPage)
            {
                endPage = totalPage;
            }

            int startCount = ((page - 1) * countPage)+ 1;
            int endCount = startCount + (pageSize - 1);

            // HTML화면에서 사용하기 위해서 선언 == ViewData, TempData 동일한 역할
            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;
            ViewBag.Page = page;
            ViewBag.TotalPage = totalPage;

            var StartCount = new MySqlParameter("startCount", startCount);
            var EndCount = new MySqlParameter("endCount", endCount);

            var objBoardList = _db.Boards.FromSql($"CALL New_PagingBoard({StartCount}, {EndCount})").ToList();


            return View(objBoardList);
        }


        // https://localhost:7139/Board/Create
        //GetMethod로 화면을 URL로 부를때 액션
        [HttpGet]
        public IActionResult Create()  // 게시판 글쓰기
        { 
            return View();
        }

        // Submit이 발생해서 내부로 데이터를 전달하는 액션
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Board board)
        {
            try
            {
                board.PostDate = DateTime.Now;  // 현재 저장하는 일시를 할당
                _db.Boards.Add(board);    // INSERT
                _db.SaveChanges();    //COMMIT

                TempData["succeed"] = "새 게시글이 저장되었습니다."; // 성공메세지
            }
            catch (Exception)
            {
                TempData["error"] = "게시글 작성에 오류가 발생하였습니다.";
            }
            
            return RedirectToAction("Index", "Board");
        }

        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id ==0) 
            {
                return NotFound();  // Error.cshtml이 표시
            }

            var board = _db.Boards.Find(Id);    // SELECT * FROM board WHERE Id = @id

            if (board == null)
            {
                return NotFound();
            }

            return View(board);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Board board)
        {
            board.PostDate = DateTime.Now;  // 현재 저장하는 일시를 할당
            _db.Boards.Update(board);   // UPDATE query실행
            _db.SaveChanges();          // COMMIT

            TempData["succeed"] = "게시글이 수정되었습니다."; // 성공메세지

            return RedirectToAction("Index", "Board");
        }

        public IActionResult Delete(int? Id) 
        {
            if (Id == null || Id == 0)
            {
                return NotFound();  // Error.cshtml이 표시
            }

            var board = _db.Boards.Find(Id);    // SELECT * FROM board WHERE Id = @id

            if (board == null)
            {
                return NotFound();
            }

            return View(board);
        }

        [HttpPost]
        public IActionResult DeletePost(int? Id)
        {
            var board = _db.Boards.Find(Id);

            if (board == null)
            {
                return NotFound();
            }

            _db.Boards.Remove(board);   // Delete query실행
            _db.SaveChanges();          // COMMIT

            TempData["succeed"] = "게시글이 삭제되었습니다."; // 성공메세지

            return RedirectToAction("Index", "Board");
        }

        [HttpGet]
        public IActionResult Details(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();  // Error.cshtml이 표시
            }

            var board = _db.Boards.Find(Id);    // SELECT * FROM board WHERE Id = @id

            if (board == null)
            {
                return NotFound();
            }

            board.ReadCount++; //조회수를 1증가 
            _db.Boards.Update(board); // 업데이트 쿼리 실행
            _db.SaveChanges(); // 커밋

            return View(board);
        }
    }
}
