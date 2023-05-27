using AutoRent_Logic.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AutoRent_Test.Services_Test
{
    [TestClass]
    public class ReviewsEnumerator_Test
    {
        [TestMethod]
        public void TestCurrentProperty()
        {
            List<Reviews> reviews = new List<Reviews>
            {
                new Reviews("name1", "review1"),
                new Reviews("name2", "review2"),
                new Reviews("name3", "review3")
            };
            ReviewsEnumerator enumerator = new ReviewsEnumerator(reviews);
            Assert.IsNull(enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual(reviews[0], enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual(reviews[1], enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual(reviews[2], enumerator.Current);
        }

        [TestMethod]
        public void TestGetEnumerator()
        {
            List<Reviews> reviews = new List<Reviews>
            {
                new Reviews("name1", "review1"),
                new Reviews("name2", "review2"),
                new Reviews("name3", "review3")
            };
            ReviewsEnumerator enumerator = new ReviewsEnumerator(reviews);
            IEnumerator<Reviews> e = enumerator.GetEnumerator();
            int i = 0;
            while (e.MoveNext())
            {
                Assert.AreEqual(reviews[i], e.Current);
                i++;
            }
        }

        [TestMethod]
        public void TestExplicitGetEnumerator()
        {
            List<Reviews> reviews = new List<Reviews>
            {
                new Reviews("name1", "review1"),
                new Reviews("name2", "review2"),
                new Reviews("name3", "review3")
            };
            var reviewsEnumerator = new ReviewsEnumerator(reviews);
            // Act
            var result = new List<Reviews>();
            foreach (var review in reviewsEnumerator)
            {
                result.Add(review);
            }
            // Assert
            Assert.AreEqual(reviews.Count, result.Count);
            for (int i = 0; i < reviews.Count; i++)
            {
                Assert.AreEqual(reviews[i].Name, result[i].Name);
                Assert.AreEqual(reviews[i].Feedback, result[i].Feedback);
            }
        }

        [TestMethod]
        public void TestMoveNext()
        {
            List<Reviews> reviews = new List<Reviews>
            {
                new Reviews("name1", "review1"),
                new Reviews("name2", "review2"),
                new Reviews("name3", "review3")
            };
            ReviewsEnumerator enumerator = new ReviewsEnumerator(reviews);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(reviews[0], enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(reviews[1], enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(reviews[2], enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        public void TestReset()
        {
            List<Reviews> reviews = new List<Reviews>
            {
                new Reviews("name1", "review1"),
                new Reviews("name2", "review2"),
                new Reviews("name3", "review3")
            };
            ReviewsEnumerator enumerator = new ReviewsEnumerator(reviews);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(reviews[0], enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(reviews[1], enumerator.Current);
            enumerator.Reset();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(reviews[0], enumerator.Current);
        }
    }
}
