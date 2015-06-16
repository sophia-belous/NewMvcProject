using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewBlog.Domain.Abstract;
using NewBlog.Domain.Entities;

namespace NewBlog.WebUI.Models
{
    public class AdminListModel
    {
        public AdminListModel(IAdminRepository adminRepository)
        {
            Posts = adminRepository.Posts();
        }

        public IList<Post> Posts { get; private set; }
    }
}