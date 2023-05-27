using System;

namespace AutoRent_Logic.Services
{
    public class Reviews
    {
        private string name;
        private string feedback;
        public DateTime TheDateFeedback { get; set; } = DateTime.Now;
        public Reviews(string name, string feedback)
        {
            Name = name;
            Feedback = feedback;
            TheDateFeedback = DateTime.Now;
        }

        public string Name 
        {
            get
            {
                return name;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Name cannot be null or empty");
                }
                name = value;
            }
        }
        public string Feedback 
        {
            get
            {
                return feedback;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Fee cannot be null or empty");
                }
                feedback = value;
            }
        }
    }
}