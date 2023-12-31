﻿using System;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using userAPI.Models;
using userAPI.Services;

namespace userAPI.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly MongoDBService _mongoDBService;

        public UserController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await _mongoDBService.GetAsync();
        }


        [HttpGet("{id}/employees")]
        public async Task<ActionResult<List<int>>> GetUserEmployees(string id)
        {
            var employees = await _mongoDBService.GetEmployeeListByUserIdAsync(id);
            if (employees == null)
                return NotFound();

            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            await _mongoDBService.CreateAsync(user);
            return CreatedAtAction(nameof(Get), new { id = user.id }, user);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> AddEmployee(string id, [FromBody] string employeeID)
        {
            await _mongoDBService.AddToEmployeesAsync(id, employeeID);
            return NoContent();
        }
    }
}
