using AutoRent_Logic.Services.Interfaces;
using System.Collections.Generic;

namespace AutoRent_Logic.Services
{
    public class ReviewsEnumerator: Interfaces.IEnumerable<Reviews>
    {
        List<Reviews> review;
        Reviews currentReviews;
        int currentIndex;
        public ReviewsEnumerator(List<Reviews> reviewsList)
        {
            review = reviewsList;
            currentIndex = -1;
        }

        public Reviews Current => currentReviews;

        public IEnumerator<Reviews> GetEnumerator()
        {
            foreach (Reviews reviews in review)
            {
                yield return reviews;
            }
        }

        IEnumerator Interfaces.IEnumerable<Reviews>.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public bool MoveNext()
        {
            if (currentIndex < review.Count - 1)
            {
                currentIndex++;
                currentReviews = review[currentIndex];
                return true;
            }
            return false;
        }
        public void Reset()
        {
            currentIndex = -1;
        }
    }
}