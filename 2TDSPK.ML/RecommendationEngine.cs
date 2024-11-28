using _2TDSPK.Database.Models;
using Microsoft.ML;
using MongoDB.Bson;
using static _2TDSPK.ML.RecommendationEngine;

namespace _2TDSPK.ML
{
    public class RecommendationEngine
    {
        private readonly MLContext _mlContext = new MLContext();
        private ITransformer _model;

        public void TrainModel(IEnumerable<UserLike> allUserLike)
        {
            var productRatings = new List<ProductRating>();

            foreach (var like in allUserLike)
            {
                productRatings.Add(new ProductRating
                {
                    UserId = like.UserId.ToString(),
                    ProductId = like.ProductId.ToString(),
                    Label = 1
                });
            }

            var trainingData = _mlContext.Data.LoadFromEnumerable(productRatings);

            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "userIdEncoded", inputColumnName: nameof(ProductRating.UserId))
                   .Append(_mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "productIdEncoded", inputColumnName: nameof(ProductRating.ProductId)))
                   .Append(_mlContext.Recommendation().Trainers.MatrixFactorization(
                       labelColumnName: nameof(ProductRating.Label),
                       matrixColumnIndexColumnName: "userIdEncoded",
                       matrixRowIndexColumnName: "productIdEncoded"));

            _model = pipeline.Fit(trainingData);

        }

        public float Predict(ObjectId userId, ObjectId productId)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<ProductRating, ProductPrediction>(_model);

            var prediction = predictionEngine.Predict(new ProductRating
            {
                UserId = userId.ToString(),
                ProductId = productId.ToString()
            });

            return prediction.Score;

        }

        public class ProductPrediction
        {
            public float Score { get; set; }
        }

        public class ProductRating
        {
            public string UserId { get; set; }
            public string ProductId { get; set; }
            public float Label { get; set; }
        }

    }
}
