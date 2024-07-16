using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ILogger<ActivitiesController> _logger;

        public ActivitiesController(DataContext context, ILogger<ActivitiesController> logger)
        {
            _logger = logger;
            _context = context;            
        }

        [HttpGet] // api/activities
        public async Task<ActionResult<List<Activity>>> GetActivities() 
        {
            return await _context.Activities.ToListAsync();
        }

        [HttpGet("{id}")] // api/activities/ajbjbel
        public async Task<ActionResult<Activity>> GetActivityById(Guid id) {
            return await _context.Activities.FindAsync(id);
        }
    }
}