

namespace gregslist_dotnet.Services;

public class HouseService
{

  public HouseService(HouseRepository houseRepository)
  {
    _houseRepository = houseRepository;
  }

  private readonly HouseRepository _houseRepository;

  internal List<House> GetAllHouses()
  {
    List<House> houses = _houseRepository.GetAllHouses();
    return houses;
  }

  internal House GetHouseById(int houseId)
  {
    House house = _houseRepository.GetHouseById(houseId);

    if (house == null) throw new Exception("This dont exist buddy");

    return house;
  }

  internal House CreateHouse(House houseData)
  {
    House house = _houseRepository.CreateHouse(houseData);
    return house;
  }

  internal string DeleteHouse(int houseId, string userId)
  {
    House house = GetHouseById(houseId);
    if (house.CreatorId != userId) throw new Exception("NOT UR HOUSE SO NOT IN MY HOUSE");
    _houseRepository.DeleteHouse(houseId);

    return "Deleted da house";

  }

  internal House UpdateHouse(int houseId, string userId, House houseUpdateData)
  {
    House house = GetHouseById(houseId);

    if (house.CreatorId != userId) throw new Exception("NOT UR HOUSE SO NOT IN MY HOUSE");

    house.Sqft = houseUpdateData.Sqft ?? house.Sqft;
    house.Bedrooms = houseUpdateData.Bedrooms ?? house.Bedrooms;
    house.Bathrooms = houseUpdateData.Bathrooms ?? house.Bathrooms;
    house.ImgUrl = houseUpdateData.ImgUrl ?? house.ImgUrl;
    house.Description = houseUpdateData.Description ?? house.Description;
    house.Price = houseUpdateData.Price ?? house.Price;

    _houseRepository.UpdateHouse(house);

    return house;
  }
}


