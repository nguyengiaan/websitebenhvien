﻿using System;
using System.IO;
using System.Threading.Tasks;
using elFinder.NetCore;
using elFinder.NetCore.Drivers.FileSystem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;


namespace websitebenhvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FilemanagerController : Controller
    {
        // Chỉ cho phép người dùng có quyền admin mới được vào
        [Authorize(Roles = "admin,Quanlyfile")]
        [Route("/File")]
        public IActionResult Index()
        {
            return View();
        }
             IWebHostEnvironment _env;
        public FilemanagerController(IWebHostEnvironment env) => _env = env;

        // Url để client-side kết nối đến backend
        // /el-finder-file-system/connector
          [Authorize(Roles = "admin,Quanlyfile")]
        [Route("/file-manager-connector")]
        public async Task<IActionResult> Connector()
        {
            var connector = GetConnector();
            return await connector.ProcessAsync(Request);
        }

        // Địa chỉ để truy vấn thumbnail
        // /el-finder-file-system/thumb
         [Authorize(Roles = "admin,Quanlyfile")]
        [Route("/file-manager-thumb/{hash}")]
        public async Task<IActionResult> Thumbs(string hash)
        {
            var connector = GetConnector();
            return await connector.GetThumbnailAsync(HttpContext.Request, HttpContext.Response, hash);
        }

        private Connector GetConnector()
        {
            // Thư mục gốc lưu trữ là wwwwroot/files (đảm bảo có tạo thư mục này)
            string pathroot = "Uploads";
            string Requesturl = "Resources";
            var driver = new FileSystemDriver();

            string absoluteUrl = UriHelper.BuildAbsolute(Request.Scheme, Request.Host);
            var uri = new Uri(absoluteUrl);

            // .. ... wwww/files
            string rootDirectory = Path.Combine(_env.ContentRootPath, pathroot);

            // https://localhost:5001/files/
            string url = $"{uri.Scheme}://{uri.Authority}/{Requesturl}/";
            string urlthumb = $"{uri.Scheme}://{uri.Authority}/file-manager-thumb/";


            var root = new RootVolume( rootDirectory, url, urlthumb)
            {
                //IsReadOnly = !User.IsInRole("Administrators")
                IsReadOnly = false, // Can be readonly according to user's membership permission
                IsLocked = false, // If locked, files and directories cannot be deleted, renamed or moved
                Alias = "Files", // Beautiful name given to the root/home folder
                //MaxUploadSizeInKb = 2048, // Limit imposed to user uploaded file <= 2048 KB
                //LockedFolders = new List<string>(new string[] { "Folder1" }
                ThumbnailSize = 100,
            };


            driver.AddRoot(root);

            return new Connector(driver)
            {
                // This allows support for the "onlyMimes" option on the client.
                MimeDetect = MimeDetectOption.Internal
            };
        }
    }
}
