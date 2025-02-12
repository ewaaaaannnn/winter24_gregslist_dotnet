namespace gregslist_dotnet.Controllers;

[ApiController]
[Route("api/cars")] // TODO show off fancy thing
public class CarsController : ControllerBase
{
  public CarsController(CarsService carsService, Auth0Provider auth0Provider)
  {
    _carsService = carsService;
    _auth0Provider = auth0Provider;
  }

  private readonly CarsService _carsService;
  private readonly Auth0Provider _auth0Provider;


  [HttpGet]
  public ActionResult<List<Car>> GetAllCars()
  {
    try
    {
      List<Car> cars = _carsService.GetAllCars();
      return Ok(cars);
    }
    catch (Exception error)
    {
      return BadRequest(error.Message);
    }
  }

  [HttpGet("{carId}")]
  public ActionResult<Car> GetCarById(int carId)
  {
    try
    {
      Car car = _carsService.GetCarById(carId);
      return Ok(car);
    }
    catch (Exception error)
    {
      return BadRequest(error.Message);
    }
  }

  // NOTE the following method requires authorization 
  [Authorize] // .use(Auth0Provider.getUserInfo)
  [HttpPost]
  public async Task<ActionResult<Car>> CreateCar([FromBody] Car carData)
  {
    try
    {
      // request.userInfo
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      // assign ownership NEVER TRUST THE CLIENT
      carData.CreatorId = userInfo.Id;
      Car car = _carsService.CreateCar(carData);
      return Ok(car);
    }
    catch (Exception error)
    {
      return BadRequest(error.Message);
    }
  }

  [Authorize]
  [HttpDelete("{carId}")]
  public async Task<ActionResult<string>> DeleteCar(int carId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      string message = _carsService.DeleteCar(carId, userInfo.Id);
      return Ok(message);
    }
    catch (Exception error)
    {
      return BadRequest(error.Message);
    }
  }

  [Authorize]
  [HttpPut("{carId}")]
  public async Task<ActionResult<Car>> UpdateCar(int carId, [FromBody] Car carUpdateData)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      Car updatedCar = _carsService.UpdateCar(carId, userInfo.Id, carUpdateData);
      return Ok(updatedCar);
    }
    catch (Exception error)
    {
      return BadRequest(error.Message);
    }
  }
}