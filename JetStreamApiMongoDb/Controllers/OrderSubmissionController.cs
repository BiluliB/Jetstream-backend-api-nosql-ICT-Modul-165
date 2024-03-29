﻿using JetStreamApiMongoDb.DTOs.Requests;
using JetStreamApiMongoDb.DTOs.Responses;
using JetStreamApiMongoDb.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace JetStreamApiMongoDb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderSubmissionController : ControllerBase
    {
        private readonly IOrderSubmissionService _orderSubmissionService;
        private readonly IUserService _userService;

        public OrderSubmissionController(IOrderSubmissionService orderSubmissionService, IUserService userService)
        {
            _orderSubmissionService = orderSubmissionService;
            _userService = userService;
        }

        /// <summary>
        /// Creates a new order submission
        /// </summary>
        /// <param name="createDTO"></param>
        /// <returns>CreateAtAction, BadRequest</returns>
        [HttpPost]
        [ProducesResponseType(typeof(List<OrderSubmissionDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderSubmissionDTO>> Create([FromBody] OrderSubmissionCreateDTO createDTO)
        {
            try
            {
                var orderSubmissionDTO = await _orderSubmissionService.Create(createDTO);
                return CreatedAtAction(nameof(GetById), new { id = orderSubmissionDTO.Id }, orderSubmissionDTO);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets all order submissions
        /// </summary>
        /// <returns>Not Found, Ok, BadRequest</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<OrderSubmissionDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<OrderSubmissionDTO>>> GetAll()
        {
            try
            {
                var orderSubmissionDTOs = await _orderSubmissionService.GetAll();
                if (orderSubmissionDTOs == null)
                {
                    return NotFound();
                }
                return Ok(orderSubmissionDTOs);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid Id format.");
            }
        }

        /// <summary>
        /// Gets an order submission by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Not Found, Ok, BadRequest</returns>
        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(typeof(OrderSubmissionDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderSubmissionDTO>> GetById(string id)
        {
            try
            {
                var orderSubmissionDTO = await _orderSubmissionService.GetById(new ObjectId(id));
                if (orderSubmissionDTO == null)
                {
                    return NotFound();
                }
                return Ok(orderSubmissionDTO);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid Id format.");
            }
        }

        /// <summary>
        /// Updates an order submission
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateDTO"></param>
        /// <returns>Not Found, Ok, BadRequest, StatusCode</returns>
        [HttpPut("{id:length(24)}")]
        [ProducesResponseType(typeof(OrderSubmissionDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<IActionResult> Update(string id, OrderSubmissionUpdateDTO updateDTO)
        {
            try
            {
                var objectId = new ObjectId(id);
                var updatedOrderSubmission = await _orderSubmissionService.Update(objectId, updateDTO);

                if (updatedOrderSubmission == null)
                {
                    return NotFound();
                }

                return Ok(updatedOrderSubmission);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Cancels an order submission
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Not Found, Ok</returns>
        [HttpPut("{id:length(24)}/cancel")]
        [ProducesResponseType(typeof(OrderSubmissionDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<ActionResult<OrderSubmissionDTO>> CancelOrder(string id)
        {
            try
            {
                var objectId = new ObjectId(id);
                var updatedOrderSubmission = await _orderSubmissionService.Cancel(objectId);

                if (updatedOrderSubmission == null)
                {
                    return NotFound();
                }

                return Ok(updatedOrderSubmission);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes an order submission
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Ok, NotFound, StatusCode</returns>
        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(OrderSubmissionDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var objectId = new ObjectId(id);
                await _orderSubmissionService.Delete(objectId);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Assigns an order submission to a user
        /// </summary>
        /// <param name="orderSubmissionId"></param>
        /// <param name="userId"></param>
        /// <returns>Ok, NotFound</returns>
        [HttpPost("{orderSubmissionId}/assign/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignOrderSubmissionToUser(string orderSubmissionId, string userId)
        {
            try
            {
                await _orderSubmissionService.AssignOrderSubmissionToUser(orderSubmissionId, userId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Updates an order submission
        /// </summary>
        /// <param name="orderSubmissionId"></param>
        /// <param name="userId"></param>
        /// <returns>Ok, NotFound</returns>
        [HttpPut("{orderSubmissionId}/user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrderSubmission(string orderSubmissionId, string userId)
        {
            try
            {
                await _orderSubmissionService.UpdateOrderSubmission(orderSubmissionId, userId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
