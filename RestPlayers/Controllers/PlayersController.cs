using Microsoft.AspNetCore.Mvc;
using PlayerLib;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestPlayers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private PlayersRepository _playersRepository;

        public PlayersController(PlayersRepository playersRepository)
        {
            _playersRepository = playersRepository;
        }


        // GET: api/<PlayersController>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public ActionResult<IEnumerable<Player>> Get()
        {
            var players = _playersRepository.GetAllPlayers();
            if (players == null)
            {
                return NotFound();
            }
            return Ok(players);
        }

        // GET api/<PlayersController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Player> Get(int id)
        {
            var player = _playersRepository.GetPlayerById(id);
            if (player == null)
            {
                return NotFound();
            }
            return Ok(player);
        }

        // POST api/<PlayersController>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public ActionResult<Player> Post([FromBody] Player player)
        {
            if (player == null)
            {
                return BadRequest();
            }

            try
            {
                player.ValidateFirstNameLength();
                player.ValidateLastNameLength();
                player.ValidateNumber();
                player.ValidatePosition();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            _playersRepository.AddPlayer(player);
            return CreatedAtAction(nameof(Get), new { id = player.Id }, player);
        }

        // PUT api/<PlayersController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id}")]
        public ActionResult<Player> Put(int id, [FromBody] Player player)
        {
            if (player == null || player.Id != id)
            {
                return BadRequest();
            }

            var existingPlayer = _playersRepository.GetPlayerById(id);
            if (existingPlayer == null)
            {
                return NotFound();
            }

            try
            {
                player.ValidateFirstNameLength();
                player.ValidateLastNameLength();
                player.ValidateNumber();
                player.ValidatePosition();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            _playersRepository.UpdatePlayer(player);
            return Ok(player);
        }

        // DELETE api/<PlayersController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        public ActionResult<Player> Delete(int id)
        {
            var player = _playersRepository.GetPlayerById(id);
            if (player == null)
            {
                return NotFound();
            }
            _playersRepository.DeletePlayer(id);
            return Ok(player);
        }
    }
}
