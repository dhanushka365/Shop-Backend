using Shop_Backend.Models;

namespace Shop_Backend.Repository
{
    public interface IProductRepository
    {
        List<Product> GetProducts();
        Product GetProductById(int id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
    }
}
