using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Globalization;
using System.Text;
using System.Text.Unicode;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
            string inputXml = File.ReadAllText(@"../../../Datasets/categories-products.xml");
            string result = GetCategoriesByProductsCount(context);
            Console.WriteLine(result);
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            ExportCategoriesByProductCountDto[] byProductCountDtos = context.Categories
                .Include(c=> c.CategoryProducts)
                .Select(c => new ExportCategoriesByProductCountDto()
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count(),
                    AveragePrice = c.CategoryProducts.Average(p => p.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(p => p.Product.Price),
                })
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Categories");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCategoriesByProductCountDto[]), xmlRoot);

            StringWriter stringWriter = new StringWriter(sb);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            xmlSerializer.Serialize(stringWriter, byProductCountDtos, namespaces);

            return sb.ToString().TrimEnd();
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            ExportSoldProductsDto[] soldProductsDtos = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new ExportSoldProductsDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold.Select(p => new ExportProductDto()
                    {
                        Name = p.Name,
                        Price = p.Price,
                    })
                    .ToList()
                })
                .Take(5)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Users");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportSoldProductsDto[]), xmlRoot);
    
            StringWriter stringWriter= new StringWriter(sb);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            xmlSerializer.Serialize(stringWriter, soldProductsDtos, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            ExportProductsInRangeDto[] filteredProducts = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000 )
                .OrderBy(p => p.Price)
                .Select(p => new ExportProductsInRangeDto()
                {
                    Name = p.Name,
                    Price= p.Price,
                    BuyerFullName = $"{p.Buyer.FirstName} {p.Buyer.LastName}"
                })
                .Take(10)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Products");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportProductsInRangeDto[]), xmlRoot);
            
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter stringWriter= new StringWriter(sb);

            xmlSerializer.Serialize(stringWriter, filteredProducts, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<ProductShopProfile>()));

            XmlRootAttribute xmlRoot = new XmlRootAttribute("CategoryProducts");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCategoryProductDto[]), xmlRoot);

            using StringReader reader = new StringReader(inputXml);

            ImportCategoryProductDto[] categoryProductDtos = (ImportCategoryProductDto[])xmlSerializer.Deserialize(reader);

            ICollection<CategoryProduct> categoryProducts = new List<CategoryProduct>();

            foreach (var dto in categoryProductDtos)
            {
                if (!context.Products.Any(p => p.Id == dto.ProductId))
                {
                    continue;
                }

                if (!context.Categories.Any(c => c.Id == dto.CategoryId))
                {
                    continue;
                }

                CategoryProduct categoryProduct = mapper.Map<CategoryProduct>(dto);
                categoryProducts.Add(categoryProduct);
            }
            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<ProductShopProfile>()));

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Categories");
            XmlSerializer xmlSerializer= new XmlSerializer(typeof(ImportCategoryDto[]), xmlRoot);

            using StringReader reader = new StringReader(inputXml);

            ImportCategoryDto[] categoryDtos = (ImportCategoryDto[])xmlSerializer.Deserialize(reader);

            ICollection<Category> categories = new List<Category>();

            foreach (var dto in categoryDtos)
            {
                if (string.IsNullOrEmpty(dto.Name))
                {
                    continue;
                }

                Category category = mapper.Map<Category>(dto);
                categories.Add(category);
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>()));

            XmlRootAttribute root = new XmlRootAttribute("Products");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportProductDto[]), root);

            using StringReader stringReader = new StringReader(inputXml);

            ImportProductDto[] productDtos = (ImportProductDto[])xmlSerializer.Deserialize(stringReader);

            Product[] products = mapper.Map<Product[]>(productDtos);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";

        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>()));

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Users");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportUserDto[]), xmlRoot);

            using StringReader streamReader= new StringReader(inputXml);

            ImportUserDto[] importUserDtos = (ImportUserDto[])xmlSerializer.Deserialize(streamReader);

            User[] users = mapper.Map<User[]>(importUserDtos);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }


    }
}