using _2TDSPK.Database.Models;
using _2TDSPK.ML;
using _2TDSPK.Repository.Interface;
using MongoDB.Bson;

namespace _2TDSPK.API.Service
{
    public class ProductService
    {
        private IRepository<Product> _productRepository { get; }
        private IRepository<UserLike> _userLikeRepository { get; }
        public RecommendationEngine _recommendationEngine { get; }

        public ProductService(IRepository<Product> productRepository, RecommendationEngine recommendationEngine, IRepository<UserLike> userLikeRepository)
        {
            _productRepository = productRepository;
            _recommendationEngine = recommendationEngine;
            _userLikeRepository = userLikeRepository;
        }


        public List<Product> RecommendedProducts(ObjectId userId)
        {

            _recommendationEngine.TrainModel(_userLikeRepository.GetAll());

            var products = _productRepository.GetAll();
            var recommendedProducts = new List<Product>();

            foreach (var product in products)
            {
                if (!ObjectId.TryParse(product.Id.ToString(), out ObjectId parsedProductId))
                {
                    continue;
                }

                float score = _recommendationEngine.Predict(userId, parsedProductId);

                if (score > 0.5)
                {
                    recommendedProducts.Add(product);
                }
            }

            return recommendedProducts;
        }
    }
}
