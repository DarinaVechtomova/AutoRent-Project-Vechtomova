using AutoRent_Logic.Contexts;
using AutoRent_Logic.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutoRent_Test.Contexts_Test
{
    [TestClass]
    public class ReviewsContext_Test
    {
        [TestMethod]
        public void SaveDataReviews_WritesToFile()
        {
            // Arrange           
            ReviewsContext reviewsContext = new ReviewsContext();
            Reviews reviews = new Reviews("Kate", "It's okay");
            string testPath = Path.Combine(Path.GetTempPath(), "test_reviews.json");
            DataBase dataBase = new DataBase();           
            dataBase.ReviewsList.Add(reviews);

            // Act         
            reviewsContext.SaveData(dataBase.ReviewsList, testPath);

            // Assert
            Assert.IsTrue(File.Exists(Path.GetFullPath(testPath)));
        }

        [TestMethod]
        public void SaveDataReviews_ThrowsException_WhenPathIsInvalid()
        {
            // Arrange
            ReviewsContext reviewsContext = new ReviewsContext();
            Reviews reviews = new Reviews("Kate", "It's okay");
            string testPath = Path.Combine(Path.GetTempPath(), "1/reviews.json");
            DataBase dataBase = new DataBase();
            dataBase.ReviewsList.Add(reviews);

            // Act + Assert
            Assert.ThrowsException<Exception>(() => reviewsContext.SaveData(dataBase.ReviewsList, testPath));
        }

        [TestMethod]
        public void ReadFromJsonReviews_ShouldReturnTrucks_WhenFileExists()
        {
            // Arrange
            ReviewsContext reviewsContext = new ReviewsContext();
            Reviews reviews = new Reviews("Kate", "It's okay");
            Reviews reviews2 = new Reviews("Vasya", "Cool");
            DataBase dataBase = new DataBase();
            dataBase.ReviewsList.Add(reviews);
            dataBase.ReviewsList.Add(reviews2);
            string testPath = Path.Combine(Path.GetTempPath(), "test.json"); ;
            reviewsContext.SaveData(dataBase.ReviewsList, testPath);

            // Act
            List<Reviews> result = reviewsContext.ReadFromJson(testPath);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(dataBase.ReviewsList.Count, result.Count);
            Assert.IsTrue(result.Any(t => t.Name == reviews.Name));
            Assert.IsTrue(result.Any(t => t.Feedback == reviews2.Feedback));
        }

        [TestMethod]
        public void ReadFromJsonReviews_ShouldReturnEmptyList_WhenFileDoesNotExist()
        {
            // Arrange
            ReviewsContext reviewsContext = new ReviewsContext();
            string testPath = "1.json";

            // Act + Assert
            Assert.ThrowsException<Exception>(() => reviewsContext.ReadFromJson(testPath));
        }
    }
}
