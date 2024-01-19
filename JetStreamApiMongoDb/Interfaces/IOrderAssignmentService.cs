namespace JetStreamApiMongoDb.Interfaces
{
    public interface IOrderAssignmentService
    {
        Task AssignOrderSubmissionToUser(string orderSubmissionId, string userId);
        Task UpdateOrderSubmission(string orderSubmissionId, string newUserId);
    }
}