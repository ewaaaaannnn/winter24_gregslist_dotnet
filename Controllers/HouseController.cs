namespace gregslist_dotnet.Controllers;

[ApiController]
[Route("api/houses")]

public class HouseController : ControllerBase
{
  public HouseController(HouseService houseService, Auth0Provider auth0Provider)
  {
    _houseService = houseService;
    _auth0Provider = auth0Provider;
  }

  private readonly HouseService _houseService;
  private readonly Auth0Provider _auth0Provider;




  [HttpGet]

  public ActionResult<List<House>> GetAllHouses()
  {
    try
    {
      List<House> houses = _houseService.GetAllHouses();
      return Ok(houses);
    }
    catch (Exception error)
    {
      return BadRequest(error.Message);
    }
  }

  [HttpGet("{houseId}")]

  public ActionResult<House> GetHouseById(int houseId)
  {
    try
    {
      House house = _houseService.GetHouseById(houseId);
      return Ok(house);
    }
    catch (Exception error)
    {
      return BadRequest(error.Message);
    }
  }


  [Authorize]
  [HttpPost]

  public async Task<ActionResult<House>> CreateHouse([FromBody] House houseData)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>
      (HttpContext);
      houseData.CreatorId = userInfo.Id;
      House house = _houseService.CreateHouse(houseData);
      return Ok(house);
    }
    catch (Exception error)
    {
      return BadRequest(error.Message);
    }
  }

  [Authorize]
  [HttpDelete("{houseId}")]

  public async Task<ActionResult<string>> DeleteCar(int houseId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>
      (HttpContext);
      string message = _houseService.DeleteHouse(houseId, userInfo.Id);
      return Ok(message);
    }
    catch (Exception error)
    {
      return BadRequest(error.Message);
    }
  }


  [Authorize]
  [HttpPut("{houseId}")]

  public async Task<ActionResult<House>> UpdateHouse(int houseId, [FromBody] House houseUpdateData)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>
      (HttpContext);
      House updatedHouse = _houseService.UpdateHouse(houseId, userInfo.Id, houseUpdateData);
      return Ok(updatedHouse);
    }
    catch (Exception error)
    {
      return BadRequest(error.Message);
    }
  }
}


