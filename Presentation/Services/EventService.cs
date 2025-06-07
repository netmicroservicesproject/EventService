using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using System.Collections.Generic;

// Services for getting the events from the database, copilot assisted
namespace Presentation.Services {
    public class EventService {
        private readonly DataContext _context;

        public EventService(DataContext context) {
            _context = context;
        }
        // Get all
        public async Task<IEnumerable<EventEntity>> GetAllAsync() { 
            var entities = await _context.Events.ToListAsync();
            return entities;
        }
        // Get one specific
        public async Task<EventEntity?> GetAsync(string eventId) { 
            var entity = await _context.Events.FirstOrDefaultAsync(x => x.Id == eventId);
            return entity;
        }
    }
}
