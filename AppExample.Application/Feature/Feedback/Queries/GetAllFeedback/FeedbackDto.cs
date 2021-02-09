using System;

namespace AppExample.Application.Feature.Feedback.Queries.GetAllFeedback
{
    public class FeedbackDto
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string Email { get; set; }
        public string Message { get; set; }
        
        public DateTime Date { get; set; }
    }
}