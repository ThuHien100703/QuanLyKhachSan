using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuanLyKhachSan.Models;
using QuanLyKhachSan.Models.Authentication;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace QuanLyKhachSan.Controllers
{
    
    [Route("admin")]
    [Authentication]
    public class AdminController : Controller
    {
        QuanLyKhachSanContext db = new QuanLyKhachSanContext();
        [Route("")]
        [Route("admin")]
        public IActionResult Index()
        {
            return View();
        }
        //LOGIN
        [HttpPost]
        public IActionResult Login(string email, string pass)
        {
            // Kiểm tra thông tin đăng nhập
            if (IsValidUser(email, pass)) // Thay thế bằng phương thức xác thực thực tế
            {
                // Lưu thông tin người dùng vào session hoặc cookie
                HttpContext.Session.SetString("UserEmail", email);
                return RedirectToAction("Index", "Admin"); // Chuyển hướng đến trang admin
            }

            ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
            return View();
        }

        private bool IsValidUser(string email, string password)
        {
            // Thực hiện kiểm tra thông tin người dùng. Đây chỉ là ví dụ.
            return email == "admin@example.com" && password == "password"; // Thay thế bằng logic thực tế
        }



        //QUẢN LÝ NHÂN VIÊN

        [Route("QLNV")]
        public IActionResult QLNV(int page = 1, int pageSize = 10)
        {
            var listNV = db.Nhanviens.ToList();
            var totalRecords = listNV.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var pagedList = listNV.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedList);
        }
        //ThemNhanVien
        [Route("ThemNhanVien")]
        public IActionResult ThemNhanVien()
        {
            return View();
        }
        [Route("ThemNhanVien")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemNhanVien(Nhanvien nv)
        {
            if (ModelState.IsValid)
            {
                db.Nhanviens.Add(nv);
                db.SaveChanges();
                return RedirectToAction("QLNV");
            }
            else
            {
                // Thêm thông báo lỗi cho trường hợp không hợp lệ
                ModelState.AddModelError("", "Dữ liệu nhập vào không hợp lệ. Vui lòng kiểm tra lại.");
            }
            return View(nv);
        }

        //Xóa nhân viên
        [HttpGet]
        [Route("XoaNhanVien")]
        public IActionResult XoaNhanVien(string IdNv)
        {
            // Tìm nhân viên theo IdNv
            var nhanVien = db.Nhanviens.Find(IdNv);

            // Kiểm tra xem nhân viên có tồn tại không
            if (nhanVien == null)
            {
                TempData["Message"] = "Nhân viên không tồn tại!";
                return RedirectToAction("QLNV");
            }

            // Xóa nhân viên
            db.Nhanviens.Remove(nhanVien);
            db.SaveChanges();

            TempData["Message"] = "Xóa thành công!!!";
            return RedirectToAction("QLNV");
        }

        //Sửa nhân viên
        [Route("SuaNhanVien")]
        [HttpGet]
        public ActionResult SuaNhanVien(string IdNv)
        {
            Nhanvien nv = db.Nhanviens.FirstOrDefault(x => x.IdNv == IdNv);
            if (nv == null)
            {
                return NotFound();
            }

            return View(nv);
        }
        [Route("SuaNhanVien")]
        [HttpPost]

        public ActionResult SuaNhanVien(Nhanvien n)
        {
            Nhanvien nv = db.Nhanviens.FirstOrDefault(x => x.IdNv == n.IdNv);
            if (nv != null)
            {
                nv.HoTenNv = n.HoTenNv;
                nv.NgaySinhNv = n.NgaySinhNv;
                nv.Cccd = n.Cccd;
                nv.SdtNv = n.SdtNv;
                nv.DiaChi = n.DiaChi;
                nv.ChucVu = n.ChucVu;
                nv.Email = n.Email;
                nv.Pass = n.Pass;
                db.SaveChanges();
            }

            return RedirectToAction("QLNV");
        }



        //QUẢN LÝ LOẠI PHÒNG
        [Route("QLlP")]
        public IActionResult QLLP(string? tenLP)
        {
            // Lấy danh sách loại phòng từ cơ sở dữ liệu
            var timKiem = db.Loaiphongs.AsQueryable();

            // Kiểm tra nếu có từ khóa tìm kiếm theo tên
            if (!string.IsNullOrEmpty(tenLP))
            {
                timKiem = timKiem.Where(r => r.TenLp.Contains(tenLP)); // Tìm kiếm theo tên
            }

            // Lưu từ khóa tìm kiếm để hiển thị lại trên giao diện
            ViewBag.TenLp = tenLP;

            // Lấy danh sách đã tìm kiếm
            var listLP = timKiem.ToList();

            return View(listLP);
        }


        //Thêm loại phòng
        [Route("ThemLoaiPhong")]
        public IActionResult ThemLoaiPhong()
        {
            
            return View();
        }

        [Route("ThemLoaiPhong")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemLoaiPhong(Loaiphong lp)
        {
            // Kiểm tra ModelState
            if (ModelState.IsValid)
            {
                try
                {
                    db.Loaiphongs.Add(lp);
                    db.SaveChanges();
                    return RedirectToAction("QLLP");
                }
                catch (Exception ex)
                {
                    // Ghi lại lỗi (nếu cần)
                    ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi thêm loại phòng: " + ex.Message);
                }
            }
            return View(lp);
        }
        //Xóa loại phòng
        [HttpGet]
        [Route("XoaLoaiPhong")]
        public IActionResult XoaLoaiPhong(string IdLp)
        {
            // Tìm loại phòng theo IdNv
            var loaiPhong = db.Loaiphongs.Find(IdLp);

            // Kiểm tra xem loại phòng có tồn tại không
            if (loaiPhong == null)
            {
                TempData["Message"] = "Nhân viên không tồn tại!";
                return RedirectToAction("QLLP");
            }

            // Xóa loại phòng   
            db.Loaiphongs.Remove(loaiPhong);
            db.SaveChanges();

            TempData["Message"] = "Xóa thành công!!!";
            return RedirectToAction("QLLP");
        }


        //Sửa loại phòng
        [Route("SuaLoaiPhong")]
        [HttpGet]
        public IActionResult SuaLoaiPhong(string IdLp)
        {
            
            var loaiphong = db.Loaiphongs.Find(IdLp);
            return View(loaiphong);
        }
        [Route("SuaLoaiPhong")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaLoaiPhong(Loaiphong lp)
        {
            if (ModelState.IsValid)
            {
                db.Update(lp);
                db.SaveChanges();
                return RedirectToAction("QLLP");
            }
            return View(lp);
        }

        //PHIẾU THUÊ PHÒNG
        [Route("PhieuThuePhong")]
        public IActionResult PhieuThuePhong(int? idP, string? idNv, string? idKh, int page = 1, int pageSize = 15)
        {
            var timKiem = db.Phieuthuephongs.AsQueryable();

            // Kiểm tra nếu có ID_P để tìm kiếm
            if (idP.HasValue)
            {
                timKiem = timKiem.Where(r => r.IdP == idP.Value); // Tìm kiếm theo ID_P
            }
            // Kiểm tra nếu có từ khóa tìm kiếm theo IdNV
            if (!string.IsNullOrEmpty(idNv))
            {
                timKiem = timKiem.Where(r => r.IdNv.Contains(idNv)); 
            }
            // Kiểm tra nếu có từ khóa tìm kiếm theo IdKH
            if (!string.IsNullOrEmpty(idKh))
            {
                timKiem = timKiem.Where(r => r.IdKh.Contains(idKh));
            }

            

           
            // Lưu từ khóa tìm kiếm để hiển thị lại trên giao diện
            ViewBag.IdP = idP;
            ViewBag.IdNv = idNv;
            ViewBag.IdKh = idKh;


            // Lấy danh sách đã tìm kiếm
            var listPTP = timKiem.ToList();
            var totalRecords = listPTP.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var pagedList = listPTP.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;



            return View(pagedList);
        }
        //Tạo phiếu

        [Route("ThemPhieuThuePhong")]
        public IActionResult ThemPhieuThuePhong()
        {

            // Lấy danh sách khách hàng để hiển thị trong dropdown
            ViewBag.IdKh = new SelectList(db.Khachhangs.ToList(), "IdKh", "IdKh");

            // Lấy danh sách nhân viên có chức vụ là Quản Lý và Tiếp tân
            var filteredEmployees = db.Nhanviens
                .Where(nv => nv.ChucVu == "Quản Lý" || nv.ChucVu == "Tiếp tân")
                .ToList();
            ViewBag.IdNv = new SelectList(filteredEmployees, "IdNv", "HoTenNv");

            // Lấy danh sách phòng để hiển thị trong dropdown
            ViewBag.IdP = new SelectList(db.Phongs.ToList(), "IdP", "TenPhong");

            // Trả về view để hiển thị form nhập liệu
            return View();

        }

        [Route("ThemPhieuThuePhong")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemPhieuThuePhong(Phieuthuephong ptp)
        {
            // Kiểm tra tính hợp lệ của ModelState
            if (ModelState.IsValid)
            {


                // Kiểm tra trùng IDPTP
                if (db.Phieuthuephongs.Any(existingPtp => existingPtp.IdPtp == ptp.IdPtp))
                {
                    ModelState.AddModelError("IdPtp", "ID Phiếu Thuê Phòng đã tồn tại.");
                }

                // Kiểm tra trùng IDPTP
                if (db.Phieuthuephongs.Any(existingPtp => existingPtp.IdPtp == ptp.IdPtp))
                {
                    ModelState.AddModelError("IdPtp", "ID Phiếu Thuê Phòng đã tồn tại.");
                }

                // Kiểm tra ngày trả phòng không nhỏ hơn ngày thuê phòng
                if (ptp.NgayTraPhong < ptp.NgayThuePhong)
                {
                    ModelState.AddModelError("NgayTraPhong", "Ngày trả phòng phải lớn hơn hoặc bằng ngày thuê phòng.");
                }

                // Nếu không có lỗi trong ModelState, tiến hành tính toán
                if (ModelState.IsValid)
                {
                    // Lấy giá phòng
                    var phong = db.Phongs.Find(ptp.IdP);
                    if (phong != null)
                    {
                        // Kiểm tra giá phòng phải có giá trị
                        if (phong.GiaP.HasValue && phong.GiaP.Value > 0) // Kiểm tra GiaP không null và lớn hơn 0
                        {
                            // Tính số ngày thuê phòng
                            int numberOfDays = (ptp.NgayTraPhong.DayNumber - ptp.NgayThuePhong.DayNumber);
                            ptp.TongTienPhong = numberOfDays * phong.GiaP.Value; // Tính tổng tiền phòng
                        }
                        else
                        {
                            ModelState.AddModelError("GiaP", "Giá phòng không hợp lệ.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("IdP", "Không tìm thấy phòng.");
                    }

                    // Kiểm tra ModelState một lần nữa sau khi tính toán
                    if (ModelState.IsValid)
                    {
                        db.Phieuthuephongs.Add(ptp);
                        db.SaveChanges();
                        return RedirectToAction("PhieuThuePhong");
                    }
                }
            }
            // Nếu ModelState không hợp lệ, trả lại view
            ViewBag.IdKh = new SelectList(db.Khachhangs.ToList(), "IdKh", "HoTenKh");
            ViewBag.IdNv = new SelectList(db.Nhanviens.ToList(), "IdNv", "HoTenNv");
            ViewBag.IdP = new SelectList(db.Phongs.ToList(), "IdP", "TenPhong");
            return View(ptp);
        }

        [Route("GetGiaPhong")]
        public JsonResult GetGiaPhong(int idP)
        {
            var phong = db.Phongs.Find(idP);
            if (phong != null)
            {
                return Json(phong.GiaP); // Giả sử GiaPhong là thuộc tính chứa giá phòng
            }
            return Json(0); // Trả về 0 nếu không tìm thấy
        }

        //Xóa PTP
        [Route("XoaPTP")]
        public IActionResult XoaPTP(string IdPtp)
        {
            db.Remove(db.Phieuthuephongs.Find(IdPtp));
            db.SaveChanges();
            TempData["Message"] = "Xóa thành công!!!";
            return RedirectToAction("PhieuThuePhong");

        }

        //Sua PTP

        [Route("SuaPhieuThue")]
        [HttpGet]
        public IActionResult SuaPhieuThue(string IdPtp)
        {
           

            // Load dropdown data
            ViewBag.IdKh = new SelectList(db.Khachhangs.ToList(), "IdKh", "IdKh");
            //
            var filteredEmployees = db.Nhanviens
                .Where(nv => nv.ChucVu == "Quản Lý" || nv.ChucVu == "Tiếp tân")
                .ToList();
            //
            ViewBag.IdNv = new SelectList(filteredEmployees, "IdNv", "HoTenNv");
            //
            ViewBag.IdP = new SelectList(db.Phongs.ToList(), "IdP", "TenPhong");

            // Get the existing rental record
            var PTP = db.Phieuthuephongs.Find(IdPtp);

            return View(PTP);
        }
        [Route("SuaPhieuThue")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaPhieuThue(Phieuthuephong ptp)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra ngày trả phòng không được trước ngày thuê phòng
                // Tính toán số ngày thuê phòng
                int numberOfDays = (ptp.NgayTraPhong.DayNumber - ptp.NgayThuePhong.DayNumber);

                // Kiểm tra số ngày thuê phải lớn hơn hoặc bằng 0
                if (numberOfDays < 0)
                {
                    ModelState.AddModelError("NgayTraPhong", "Ngày trả phòng phải lớn hơn hoặc bằng ngày thuê phòng.");
                }

                // Kiểm tra ModelState sau khi thêm lỗi nếu có
                if (!ModelState.IsValid)
                {
                    // Nếu ModelState không hợp lệ, trả lại view với thông tin hiện tại
                    ViewBag.IdKh = new SelectList(db.Khachhangs.ToList(), "IdKh", "IdKh");
                    ViewBag.IdNv = new SelectList(db.Nhanviens
                        .Where(nv => nv.ChucVu == "Quản Lý" || nv.ChucVu == "Tiếp tân")
                        .ToList(), "IdNv", "HoTenNv");
                    ViewBag.IdP = new SelectList(db.Phongs.ToList(), "IdP", "TenPhong");
                    return View(ptp);
                }

                // Cập nhật thông tin
                db.Update(ptp);
                db.SaveChanges();
                return RedirectToAction("PhieuThuePhong");
            }

            // Nếu ModelState không hợp lệ, trả lại view
            ViewBag.IdKh = new SelectList(db.Khachhangs.ToList(), "IdKh", "IdKh");
            ViewBag.IdNv = new SelectList(db.Nhanviens
                .Where(nv => nv.ChucVu == "Quản Lý" || nv.ChucVu == "Tiếp tân")
                .ToList(), "IdNv", "HoTenNv");
            ViewBag.IdP = new SelectList(db.Phongs.ToList(), "IdP", "TenPhong");
            return View(ptp);
        }




        //QUẢN LÝ KHÁCH HÀNG
        [Route("QLKH")]
        public IActionResult QLKH(string? hoTen, string? soCCCD, int page = 1, int pageSize = 15)
        {
            // Lấy danh sách khách hàng từ cơ sở dữ liệu
            var timKiem = db.Khachhangs.AsQueryable();

            // Kiểm tra nếu có từ khóa tìm kiếm theo tên
            if (!string.IsNullOrEmpty(hoTen))
            {
                timKiem = timKiem.Where(r => r.HoTenKh.Contains(hoTen)); // Tìm kiếm theo tên
            }

            // Kiểm tra nếu có số CCCD để tìm kiếm
            if (!string.IsNullOrEmpty(soCCCD))
            {
                timKiem = timKiem.Where(r => r.SoCccd.Contains(soCCCD)); // Tìm kiếm theo số CCCD
            }

            // Tính tổng số bản ghi sau khi tìm kiếm
            var totalRecords = timKiem.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Lấy danh sách đã phân trang
            var pagedList = timKiem.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Thiết lập các thông tin phân trang
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.HoTen = hoTen; // Lưu từ khóa tìm kiếm để hiển thị lại trên giao diện
            ViewBag.SoCccd = soCCCD;

            return View(pagedList);
        }

        //Thêm khách hàng
        [Route("ThemKhachHang")]
        public IActionResult ThemKhachHang()
        {
            return View();
        }

        [HttpPost]
        [Route("ThemKhachHang")]
        [ValidateAntiForgeryToken]
        public IActionResult ThemKhachHang(Khachhang kh)
        {
            if (ModelState.IsValid)
            {
                // Check if IdKh already exists
                var existingCustomerById = db.Khachhangs.Any(k => k.IdKh == kh.IdKh);
                if (existingCustomerById)
                {
                    ModelState.AddModelError("IdKh", "ID khách hàng đã tồn tại.");
                }

                // Check if SoCccd already exists
                var existingCustomerByCCCD = db.Khachhangs.Any(k => k.SoCccd == kh.SoCccd);
                if (existingCustomerByCCCD)
                {
                    ModelState.AddModelError("SoCccd", "Số CCCD đã tồn tại.");
                }

                // If ModelState is still invalid after checks, return the view with the current model
                if (!ModelState.IsValid)
                {
                    return View(kh);
                }

                // No errors, proceed to add the customer
                db.Khachhangs.Add(kh);
                db.SaveChanges();
                return RedirectToAction("QLKhachHang");
            }

            // If ModelState is invalid from the start, add a general error
            ModelState.AddModelError("", "Dữ liệu nhập vào không hợp lệ. Vui lòng kiểm tra lại.");
            return View(kh);
        }

        //Xóa khách hàng
        [HttpGet]
        [Route("XoaKhachHang")]
        public IActionResult XoaKhachHang(string IdKh)
        {
            // Tìm khách hàng theo IdKh
            var khachHang = db.Khachhangs.Find(IdKh);

            // Kiểm tra xem khách hàng có tồn tại không
            if (khachHang == null)
            {
                TempData["Message"] = "Nhân viên không tồn tại!";
                return RedirectToAction("QLKH");
            }

            // Xóa khách hàng
            db.Khachhangs.Remove(khachHang);
            db.SaveChanges();

            TempData["Message"] = "Xóa thành công!!!";
            return RedirectToAction("QLKH");
        }


        ////sửa KH
        [Route("SuaKhachHang")]
        [HttpGet]
        public IActionResult SuaKhachHang(string IdKh)
        {

            var KH = db.Khachhangs.Find(IdKh);
            return View(KH);
        }
        [Route("SuaKhachHang")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaKhachHang(Khachhang kh)
        {
            if (ModelState.IsValid)
            {
                // Check if IdKh exists and is different from the one being edited
                var existingCustomerById = db.Khachhangs
                    .Any(k => k.IdKh == kh.IdKh && k.IdKh != kh.IdKh);
                if (existingCustomerById)
                {
                    ModelState.AddModelError("IdKh", "ID khách hàng đã tồn tại.");
                }

                // Check if SoCccd exists and is different from the one being edited
                var existingCustomerByCCCD = db.Khachhangs
                    .Any(k => k.SoCccd == kh.SoCccd && k.IdKh != kh.IdKh);
                if (existingCustomerByCCCD)
                {
                    ModelState.AddModelError("SoCccd", "Số CCCD đã tồn tại.");
                }

                // If there are any validation errors, return the view with current model
                if (!ModelState.IsValid)
                {
                    return View(kh);
                }

                // Update the customer if no errors
                db.Update(kh);
                db.SaveChanges();
                return RedirectToAction("QLKH");
            }

            // If ModelState is invalid from the start, add a general error
            ModelState.AddModelError("", "Dữ liệu nhập vào không hợp lệ. Vui lòng kiểm tra lại.");
            return View(kh);
        }


        //QUẢN LÝ PHÒNG
        [HttpGet]
        [Route("QLPhong")]
        public IActionResult QLPhong(bool? tinhTrangPhong, int? to, int? from, string? IdLp, int pageNumber = 1, int pageSize = 10)
        {
            var timKiem = db.Phongs.AsQueryable();

            // Thực hiện các điều kiện lọc
            if (tinhTrangPhong.HasValue)
            {
                timKiem = timKiem.Where(r => r.TinhTrang == tinhTrangPhong.Value);
            }

            if (to.HasValue)
            {
                timKiem = timKiem.Where(r => r.GiaP >= to.Value);
            }

            if (from.HasValue)
            {
                timKiem = timKiem.Where(r => r.GiaP <= from.Value);
            }

            if (!string.IsNullOrEmpty(IdLp))
            {
                timKiem = timKiem.Where(r => r.IdLp == IdLp);
            }

            // Lấy toàn bộ dữ liệu phù hợp
            var allItems = timKiem.ToList();

            // Số lượng dữ liệu
            var totalItems = allItems.Count();

            // Tính số trang
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Kiểm tra xem pageNumber có hợp lệ không
            if (pageNumber < 1) pageNumber = 1;  // Đảm bảo không nhỏ hơn 1
            if (pageNumber > totalPages) pageNumber = totalPages; // Đảm bảo không lớn hơn tổng số trang

            // Phân trang
            var items = allItems
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Giữ lại các tham số lọc trong ViewBag
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.TinhTrang = tinhTrangPhong;
            ViewBag.To = to;
            ViewBag.From = from;
            ViewBag.IdLp = IdLp;

            return View(items);
        }


        //Them phong
        [Route("ThemPhong")]
        public IActionResult ThemPhong()
        {
            ViewBag.IdLp = new SelectList(db.Loaiphongs.ToList(), "IdLp", "TenLp");
            return View();
        }
        [Route("ThemPhong")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemPhong(Phong p)
        {
            if (ModelState.IsValid)
            {
                db.Phongs.Add(p);
                db.SaveChanges();
                return RedirectToAction("QLPhong");
            }
            return View(p);
        }

        //checkout về 0
        [Route("CheckOut")]
        [HttpGet]
        public IActionResult CheckOut(int IdP)
        {
            var room = db.Phongs.Find(IdP);
            if (room == null)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy phòng
            }

            if (room.TinhTrang) // Giả định TinhTrang cho biết phòng có đang được thuê hay không
            {
                room.TinhTrang = false; // Đánh dấu là đã trả phòng
                db.Update(room);
                db.SaveChanges();
                TempData["SuccessMessage"] = $"Phòng {room.IdP} đã được trả.";
            }
            else
            {
                TempData["ErrorMessage"] = $"Phòng {room.IdP} hiện đang trống.";
            }

            return RedirectToAction(nameof(QLPhong)); // Chuyển hướng về hành động QLPhong
        }


        //Check In 
        [Route("CheckIn")]
        [HttpGet]
        public IActionResult CheckIn(int IdP)
        {
            var room = db.Phongs.Find(IdP);
            if (room == null)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy phòng
            }

            if (!room.TinhTrang) // Giả định TinhTrang cho biết phòng có đang trống hay không
            {
                room.TinhTrang = true; // Đánh dấu là đã thuê phòng
                db.Update(room);
                db.SaveChanges();
                TempData["SuccessMessage"] = $"Phòng {room.IdP} đã được thuê.";
            }
            else
            {
                TempData["ErrorMessage"] = $"Phòng {room.IdP} hiện đang được thuê.";
            }

            return RedirectToAction(nameof(QLPhong)); // Chuyển hướng về hành động QLPhong
        }

        //Sua phong
        [Route("SuaPhong")]
        public IActionResult SuaPhong(int IdP)
        {
            ViewBag.IdLp = new SelectList(db.Loaiphongs.ToList(), "IdLp", "TenLp");
            var phong = db.Phongs.Find(IdP);
            return View(phong);
        }
        [Route("SuaPhong")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaPhong(Phong phong)
        {
            if (ModelState.IsValid)
            {
                db.Update(phong);
                db.SaveChanges();
                return RedirectToAction("QLPhong");
            }
            return View(phong);
        }


        //Xoa phòng
        [Route("XoaPhong")]
        [HttpGet]
        public IActionResult XoaPhong(int IdP)
        {
            db.Remove(db.Phongs.Find(IdP));
            db.SaveChanges();
            TempData["Message"] = "phòng đã được xóa!!!";
            return RedirectToAction("QLPhong");

        }


        //HÓA ĐƠN
        [Route("HoaDon")]
        public IActionResult HoaDon(int page = 1, int pageSize = 20)
        {
            var listHD = db.Hoadons.ToList();
            var totalRecords = listHD.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var pagedList = listHD.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedList);
        }

        //Them hóa đơn
        [Route("ThemHoaDon")]
        public IActionResult ThemHoaDon()
        {
            ViewBag.IdPtp = new SelectList(db.Phieuthuephongs.ToList(), "IdPtp", "IdPtp");
            return View();
        }
        [Route("ThemHoaDon")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemHoaDon(Hoadon hd)
        {
            // Kiểm tra xem mã hóa đơn đã tồn tại chưa
            if (db.Hoadons.Any(h => h.IdHd == hd.IdHd))
            {
                ModelState.AddModelError("IdHd", "Mã hóa đơn đã tồn tại."); // Thêm thông báo lỗi
            }

            if (ModelState.IsValid)
            {
                db.Hoadons.Add(hd);
                db.SaveChanges();
                return RedirectToAction("HoaDon");
            }

            // Nếu có lỗi, trả về view với thông tin đã nhập
            ViewBag.IdPtp = new SelectList(db.Phieuthuephongs.ToList(), "IdPtp", "IdPtp", hd.IdPtp);
            return View(hd);
        }




        //GetTongTienPhong
        [Route("GetTongTienPhong")]
        public JsonResult GetTongTienPhong(string IdPtp)
        {
            var phieu = db.Phieuthuephongs.Find(IdPtp);
            if (phieu != null)
            {
                return Json(phieu.TongTienPhong);
            }
            return Json(0);
        }


        //GetNgayTraPhong
        [Route("GetNgayTraPhong")]
        public JsonResult GetNgayTraPhong(string IdPtp)
        {
            var phieu = db.Phieuthuephongs.Find(IdPtp);
            if (phieu != null)
            {
                return Json(phieu.NgayTraPhong);
            }
            return Json(0);
        }



        //Xóa hóa đơn
        [Route("XoaHoaDon")]
        [HttpGet]
        public IActionResult XoaHoaDon(string IdHd)
        {
            db.Remove(db.Hoadons.Find(IdHd));
            db.SaveChanges();
            TempData["Message"] = "hóa đơn đã được xóa!!!";
            return RedirectToAction("HoaDon");

        }
        //Sua phong
        [Route("SuaHD")]
        public IActionResult SuaHD(string IdHd)
        {
            ViewBag.IdPtp = new SelectList(db.Phieuthuephongs.ToList(), "IdPtp", "IdPtp");
            var hd = db.Hoadons.Find(IdHd);
            return View(hd);
        }
        [Route("SuaHD")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaHD(Hoadon hd)
        {
            if (ModelState.IsValid)
            {
                db.Update(hd);
                db.SaveChanges();
                return RedirectToAction("HoaDon");
            }
            return View(hd);
        }

        //
        //[Route("ThanhToan")]
        //[HttpGet]
        //public IActionResult ThanhToan(int IdP)
        //{
        //    var room = db.Phongs.Find(IdP);
        //    if (room == null)
        //    {
        //        return NotFound(); // Trả về 404 nếu không tìm thấy phòng
        //    }

        //    if (room.TinhTrang) // Giả định TinhTrang cho biết phòng có đang được thuê hay không
        //    {
        //        room.TinhTrang = false; // Đánh dấu là đã trả phòng
        //        db.Update(room);
        //        db.SaveChanges();
        //        TempData["SuccessMessage"] = $"Phòng {room.IdP} đã được trả.";
        //    }
        //    else
        //    {
        //        TempData["ErrorMessage"] = $"Phòng {room.IdP} hiện đang trống.";
        //    }

        //    return RedirectToAction(nameof(QLPhong)); // Chuyển hướng về hành động QLPhong
        //}

        [Route("ThongKeDoanhThu")]
        public async Task<IActionResult> ThongKeDoanhThu()
        {
            var doanhThuTheoThang = await db.Hoadons
                .Where(hd => hd.NgayThanhToan != null) // Đảm bảo không có giá trị null
                .GroupBy(hd => new { hd.NgayThanhToan.Year, hd.NgayThanhToan.Month })
                .Select(g => new
                {
                    Nam = g.Key.Year,
                    Thang = g.Key.Month,
                    TongDoanhThu = g.Sum(hd => hd.ThanhTien)
                })
                .OrderBy(g => g.Nam) // Sắp xếp theo năm
                .ThenBy(g => g.Thang) // Sắp xếp theo tháng
                .ToListAsync();

            return View(doanhThuTheoThang);
        }


        [Route("ThongKeDoanhThuTheoNam")]
        public async Task<IActionResult> ThongKeDoanhThuTheoNam()
        {
            var doanhThuTheoNam = await db.Hoadons
                .Where(hd => hd.NgayThanhToan != null) // Đảm bảo không có giá trị null
                .GroupBy(hd => hd.NgayThanhToan.Year) // Nhóm theo năm
                .Select(g => new
                {
                    Nam = g.Key,
                    TongDoanhThu = g.Sum(hd => hd.ThanhTien) // Tính tổng doanh thu
                })
                .OrderBy(g => g.Nam) // Sắp xếp theo năm
                .ToListAsync();

            return View(doanhThuTheoNam);
        }

        [Route("SDThongKeDoanhThu")]
        public async Task<IActionResult> SdThongKeDoanhThu()
        {
            var doanhThuTheoThang = await db.Hoadons
        .Where(hd => hd.NgayThanhToan != null)
        .GroupBy(hd => new { hd.NgayThanhToan.Year, hd.NgayThanhToan.Month })
        .Select(g => new
        {
            Nam = g.Key.Year,
            Thang = g.Key.Month, // Thêm thuộc tính Thang
            TongDoanhThu = g.Sum(hd => hd.ThanhTien)
        })
        .OrderBy(g => g.Nam)
        .ThenBy(g => g.Thang)
        .ToListAsync();

            return View(doanhThuTheoThang);
        }
    }
}
