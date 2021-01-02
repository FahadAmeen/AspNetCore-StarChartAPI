using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id:int}",Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObjects =  _context.CelestialObjects.SingleOrDefault(o => o.Id.Equals(id));
            if (celestialObjects == null) return NotFound();
            celestialObjects.Satellites = new List<CelestialObject>(_context.CelestialObjects.Where(o => o.OrbitedObjectId.Equals(o.Id)));
            return Ok(celestialObjects);
        }
        [HttpGet("{name}", Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects =  _context.CelestialObjects.Where(o=>o.Name.Equals(name)).ToList();
            
            if (celestialObjects .Count==0) return NotFound();
            foreach (var o in celestialObjects)
            {
                o.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId.Equals(o.Id)).ToList();
            }

            return Ok(celestialObjects);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects =  _context.CelestialObjects.ToList();
            if (celestialObjects.Count == 0) return NotFound();

            foreach (var o in celestialObjects)
            {
                o.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId.Equals(o.Id)).ToList();
            }
            return Ok(celestialObjects);
        }

    }
}
