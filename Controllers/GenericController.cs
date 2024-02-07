using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using myOrderApi.Data; 

namespace myOrderApi
{
    [ApiController]
    [Route("[controller]")]
    public class GenericController<T> : ControllerBase where T : class
    {
        private readonly IRepository<T> _repository;

        public GenericController(IRepository<T> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> GetAll()
        {
            var items = await _repository.GetAllAsync();
            return Ok(items);
        }

        // Hämtning
        [HttpGet("{id}")]
        public async Task<ActionResult<T>> Get(string id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // Skapar nytt objekt
        [HttpPost]
        public async Task<ActionResult<T>> Post(T item)
        {
            await _repository.AddAsync(item);
            return CreatedAtAction(nameof(Get), new { id = item.GetType().GetProperty("Id")?.GetValue(item, null) }, item);
        }

        // Uppdaterar befintligt objekt
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, T item)
        {
            await _repository.UpdateAsync(id, item);
            return NoContent();
        }

        // Tar bort objekt
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
