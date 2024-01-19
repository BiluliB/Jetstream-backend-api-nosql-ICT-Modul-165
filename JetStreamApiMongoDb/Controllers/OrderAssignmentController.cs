using JetStreamApiMongoDb.Interfaces;
using JetStreamApiMongoDb.Models;
using JetStreamApiMongoDb.Services;
using Microsoft.AspNetCore.Mvc;


namespace JetStreamApiMongoDb.Controllers
{
    public class OrderAssignmentController : Controller
    {
        private readonly IOrderAssignmentService _orderAssignmentService;

        public OrderAssignmentController(IOrderAssignmentService orderAssignmentService)
        {
            _orderAssignmentService = orderAssignmentService;
        }

        // POST api/orderassignment
        [HttpPost("{orderSubmissionId}/assign/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignOrderSubmissionToUser(string orderSubmissionId, string userId)
        {
            try
            {
                await _orderAssignmentService.AssignOrderSubmissionToUser(orderSubmissionId, userId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT api/orderassignment/{orderSubmissionId}/user/{userId}
        [HttpPut("{orderSubmissionId}/user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrderSubmission(string orderSubmissionId, string userId)
        {
            try
            {
                await _orderAssignmentService.UpdateOrderSubmission(orderSubmissionId, userId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
