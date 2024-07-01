using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Pages.Admin.Products
{
    public class EditModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbContext context;

        [BindProperty]
        public ProductDto ProductDto { get; set; } = new ProductDto();

        public Product Product { get; set; } = new Product();

        public string errorMessage = "";
        public string successMessage = "";

        public EditModel(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            this.environment = environment;
            this.context = context;
        }
        public void OnGet(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }

            var product = context.Products.Find(id);
            if (product == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }

            ProductDto.Name = product.Name;
            ProductDto.Price = product.Price;
            ProductDto.Description = product.Description;
            ProductDto.Brand = product.Brand;
            ProductDto.Category = product.Category;

            Product = product;
        }

        public void OnPost(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }


            var product = context.Products.Find(id);
            if (product == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }

            if (!ModelState.IsValid)
            {
                errorMessage = "Please provide all required fields";
                return;
            }

            // update the image file if we have a new image file uploaded
            string newImageFileName = product.ImageFileName;
            if (ProductDto.ImageFile != null)
            {
                newImageFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newImageFileName += Path.GetExtension(ProductDto.ImageFile.FileName);

                string imageFullPath = environment.WebRootPath + "/products/" + newImageFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    ProductDto.ImageFile.CopyTo(stream);
                }
                // delete old image file
                string oldImageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
            }

            // update product in the database

            product.Name = ProductDto.Name;
            product.Price = ProductDto.Price;
            product.Description = ProductDto.Description ?? "";
            product.Brand = ProductDto.Brand;
            product.Category = ProductDto.Category;
            product.ImageFileName = newImageFileName;

            context.SaveChanges();

            Product = product;

            successMessage = "Product updated successfully";
            Response.Redirect("/Admin/Products/Index");


        }
    }
}
