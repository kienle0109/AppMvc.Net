using AppMvc.Net.Models;

namespace AppMvc.Net.Services
{
    public class ProductService : List<ProductModel>
    {
        public ProductService() {
            this.AddRange(new ProductModel[] {
                new ProductModel() { Id = 1, Name = "IphoneX", Price =1000},
                new ProductModel() { Id = 2, Name = "SamSung", Price = 900},
                new ProductModel() { Id = 3, Name = "Nokia", Price = 800},
                new ProductModel() { Id = 4, Name = "Xiaomi", Price = 700},
            });
        }
    }
}