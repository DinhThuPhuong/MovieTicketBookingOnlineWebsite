﻿//using DoAnCoSoTL.Models;
//using DoAnCoSoTL.Repositories;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace MovieTickets.Controllers
//{

//    public class CategoryController : Controller
//    {
//        ICategoryRepository categoryRepo;
//        MovieContext db;
//        #region Constructor Injection
//        public CategoryController(ICategoryRepository _categoryRepo, MovieContext _db)
//        {
//            this.categoryRepo = _categoryRepo;
//            this.db = _db;


//        }
//        #endregion


//        #region User
//        #region Index
//        public ActionResult Index()
//        {

//            List<Category> categories = categoryRepo.GetAll();

//            return View(categories);
//        }
//        #endregion
//        #region Details
//        public ActionResult Details(int id)
//        {

//            Category category = categoryRepo.GetById(id);
//            return View("DetailsUser", category);
//        }
//        #endregion
//        #endregion


//        #region Admin
//        #region Index
//        //[Authorize(Roles = "Admin")]
//        public IActionResult AdminCategories()
//        {
//            List<Category> categories = categoryRepo.GetAll();
//            return View("AdminCategories", categories);
//        }
//        #endregion
//        #region Details
//        //[Authorize(Roles = "Admin")]
//        public ActionResult CategoriesDetailsAdmin(int id)
//        {

//            Category Categories = categoryRepo.GetById(id);
//            return View("CategoriesDetailsAdmin", Categories);
//        }
//        #endregion
//        #region Insert
//        #region Get
//        public IActionResult InsertCategoryForm()
//        {
//            return View("InsertCategoryForm", new Category());
//        }
//        #endregion
//        #region Post
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        //[Authorize(Roles = "Admin")]
//        public IActionResult Create(Category newCategory, List<IFormFile> images)
//        {
//            if (ModelState.IsValid)
//            {
//                // Gọi phương thức Insert từ repository để thêm danh mục mới
//                var result = categoryRepo.Insert(newCategory, images);

//                // Kiểm tra kết quả và xử lý tùy thuộc vào nhu cầu của ứng dụng của bạn
//                if (result != null)
//                {
//                    // Đã thêm danh mục mới thành công
//                    return RedirectToAction("AdminCategories");
//                }
//                else
//                {
//                    // Xử lý khi có lỗi xảy ra trong quá trình thêm danh mục mới
//                    ModelState.AddModelError(string.Empty, "Failed to insert category.");
//                    return View(newCategory);
//                }
//            }

//            // Nếu mô hình không hợp lệ, hiển thị lại form với các thông báo lỗi
//            return View(newCategory);
//        }


//            // Nếu mô hình không hợp lệ, hiển thị lại form với các thông báo lỗi
         
    


//        #endregion

//        #endregion
//        #region Update
//        #region Get
//        public IActionResult UpdateCategoryForm(int id)
//        {
//            var category = categoryRepo.GetById(id);
//            return View("UpdateCategoryForm", category);
//        }
//        #endregion
//        #region Post
//        public IActionResult UpdateCategory(Category EditCategory, List<IFormFile> Image)
//        {
//            if (ModelState.IsValid)
//            {
//                categoryRepo.Update(EditCategory, Image);
//                return RedirectToAction("AdminCategories");
//            }
//            return RedirectToAction("UpdateCategoryForm", EditCategory);
//        }
//        #endregion

//        #endregion
//        #region Delete
//        public ActionResult Delete(int id)
//        {
//            int numOfRowsDeleted = categoryRepo.Delete(id);
//            return RedirectToAction("AdminCategories");

//        }
//        #endregion
//        #endregion
//        #region Search
//        [HttpGet]
//        //[Authorize(Roles = "Admin")]
//        public async Task<IActionResult> AdminCategories(string Keyword)
//        {
//            ViewData["searching"] = Keyword;
//            var categories = db.Categories.Select(x => x);
//            if (!string.IsNullOrEmpty(Keyword))
//            {
//                categories = categories.Where(c => c.Name.Contains(Keyword));

//            }
//            return View(await categories.AsNoTracking().ToListAsync());
//        }
//        #endregion
//        #region GetById
//        public ActionResult Category(int id)
//        {

//            Category category = categoryRepo.GetById(id);
//            return View(category);
//        }
//        #endregion

//        #region ForView
//        public ActionResult Grid()
//        {

//            return PartialView("_Grid", categoryRepo.GetAll());
//        }

//        public ActionResult List()
//        {

//            return PartialView("_List", categoryRepo.GetAll());
//        }
//        #endregion
//        // To Get Category by ID




//        //get all Categories for admin

//        //searching ---------------------------



//        //To get Category by name
//        //public ActionResult Details(string name)
//        //{

//        //    Category category = categoryRepo.GetByName(name);
//        //    return View("DetailsUser");
//        //}
//        //The details of Categories for admin

//        //insert form ------------------------------------------

//        // To add new movie


//        //insert Category

//        //-------------------------------------------------------------------------



//        // To Edit any Movie


//        // ------------------------------------------------------------


//        // POST: MovieController/Edit/5



//        // To delete movies




//    }
//}
