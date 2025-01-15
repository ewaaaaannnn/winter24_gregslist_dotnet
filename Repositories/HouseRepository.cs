



namespace gregslist_dotnet.Repositories;

public class HouseRepository
{

  public HouseRepository(IDbConnection db)
  {
    _db = db;
  }

  private readonly IDbConnection _db;


  internal List<House> GetAllHouses()
  {
    string sql = @"
    SELECT
    houses.*,
    accounts.*
    FROM houses
    JOIN accounts ON houses.creator_id = accounts.id;";

    List<House> houses = _db.Query(sql, (House house, Account account) =>
    {
      house.Creator = account;
      return house;

    }).ToList();

    return houses;
  }

  internal House GetHouseById(int houseId)
  {
    string sql = @"
    SELECT
    houses.*,
    accounts.*
    FROM houses
    JOIN accounts ON houses.creator_id = accounts.id
    WHERE houses.id = @houseId;";

    House house = _db.Query(sql, (House house, Account account) =>
    {
      house.Creator = account;
      return house;
    }, new { houseId }).SingleOrDefault();
    return house;
  }

  internal House CreateHouse(House houseData)
  {
    string sql = @"
        INSERT INTO
        houses(sqft, bedrooms, bathrooms, imgUrl, description, price, creator_id)VALUES(@Sqft, @Bedrooms, @Bathrooms, @ImgUrl, @Description, @Price, @CreatorId);
        
        
        SELECT
        houses.*,
        accounts.*
        FROM houses
        JOIN accounts ON houses.creator_id = accounts.id
        WHERE houses.id = LAST_INSERT_ID();";

    House house = _db.Query(sql, (House house, Account account) =>
    {
      house.Creator = account;
      return house;
    }, houseData).SingleOrDefault();

    return house;
  }

  internal void DeleteHouse(int houseId)
  {
    string sql = "DELETE FROM houses WHERE id = @id LIMIT 1;";

    int rowsAffected = _db.Execute(sql, new { id = houseId });

    if (rowsAffected == 0) throw new Exception("Nothing was Deleted");
    if (rowsAffected > 1) throw new Exception("Too much was Deleted");
  }

  internal void UpdateHouse(House house)
  {
    string sql = @"
        UPDATE houses
        SET
        sqft = @Sqft,
        bedrooms = @Bedrooms,
        bathrooms = @Bathrooms,
        imgUrl = @ImgUrl,
        description = @Description,
        price = @Price
        WHERE id = @Id LIMIT 1;";
    int rowsAffected = _db.Execute(sql, house);

    if (rowsAffected == 0) throw new Exception("UPDATE WAS UNSUCCESSFUL");
    if (rowsAffected > 1) throw new Exception("UPDATE WAS TOO SUCCESSFUL");
  }
}