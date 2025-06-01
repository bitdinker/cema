using Microsoft.Data.Sqlite;

public static class DatabaseHelper
{
    private static string DatabasePath => Path.Combine(Directory.GetCurrentDirectory(), "cema.db3");
    private static string ConnectionString => $"Data Source={DatabasePath}";

    public static void EnsureDatabaseCreated()
    {
        if (!File.Exists(DatabasePath))
        {
            CreateDatabase();
        }
    }

    private static void CreateDatabase()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var createCarsTableCommand = connection.CreateCommand();
            createCarsTableCommand.CommandText =
            @"
                CREATE TABLE Cars (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    LicensePlate TEXT
                )
            ";
            createCarsTableCommand.ExecuteNonQuery();

            var createExpenseGroupsTableCommand = connection.CreateCommand();
            createExpenseGroupsTableCommand.CommandText =
            @"
                CREATE TABLE ExpenseGroups (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT
                )
            ";
            createExpenseGroupsTableCommand.ExecuteNonQuery();

            var createExpensesTableCommand = connection.CreateCommand();
            createExpensesTableCommand.CommandText =
            @"
                CREATE TABLE Expenses (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Supplier TEXT,
                    Amount REAL,
                    GroupId INTEGER,
                    CarId INTEGER,
                    FOREIGN KEY (GroupId) REFERENCES ExpenseGroups(Id),
                    FOREIGN KEY (CarId) REFERENCES Cars(Id)
                )
            ";
            createExpensesTableCommand.ExecuteNonQuery();
        }
    }

    public static void InsertCar(Car car)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var insertCommand = connection.CreateCommand();
            insertCommand.CommandText =
            @"
                INSERT INTO Cars (Name, LicensePlate)
                VALUES (@Name, @LicensePlate)
            ";
            insertCommand.Parameters.AddWithValue("@Name", car.Name);
            insertCommand.Parameters.AddWithValue("@LicensePlate", car.LicensePlate);
            insertCommand.ExecuteNonQuery();
        }
    }

    public static Car GetCarById(int id)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var selectCommand = connection.CreateCommand();
            selectCommand.CommandText =
            @"
                SELECT Id, Name, LicensePlate
                FROM Cars
                WHERE Id = @Id
            ";
            selectCommand.Parameters.AddWithValue("@Id", id);

            using (var reader = selectCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Car { Id = reader.GetInt32(0), Name = reader.GetString(1), LicensePlate = reader.GetString(2) };
                }
            }
        }
        return null; // Or throw an exception if car not found
    }

    public static List<Car> GetAllCars()
    {
        var cars = new List<Car>();
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            // Modify the SQL query to include the total accumulated expense
            var selectCommand = connection.CreateCommand();
            selectCommand.CommandText =
            @"
                SELECT Id, Name, LicensePlate
                FROM Cars
            ";

            // Use a LEFT JOIN to include cars with no expenses and sum the amount
            selectCommand.CommandText =
             @"
             SELECT C.Id, C.Name, C.LicensePlate, SUM(E.Amount) AS TotalExpenses
             FROM Cars AS C
             LEFT JOIN Expenses AS E ON C.Id = E.CarId
             GROUP BY C.Id, C.Name, C.LicensePlate
             ";

            using (var reader = selectCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Handle potential DBNull for TotalExpenses if a car has no expenses
                    decimal totalExpenses = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3);
                    cars.Add(new Car { Id = reader.GetInt32(0), Name = reader.GetString(1), LicensePlate = reader.GetString(2), TotalExpenses = totalExpenses });
                }
            }
        }
        return cars;
    }

    public static void UpdateCar(Car car)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var updateCommand = connection.CreateCommand();
            updateCommand.CommandText =
            @"
                UPDATE Cars
                SET Name = @Name, LicensePlate = @LicensePlate
                WHERE Id = @Id
            ";
            updateCommand.Parameters.AddWithValue("@Name", car.Name);
            updateCommand.Parameters.AddWithValue("@LicensePlate", car.LicensePlate);
            updateCommand.Parameters.AddWithValue("@Id", car.Id);
            updateCommand.ExecuteNonQuery();
        }
    }

    public static void DeleteCar(int id)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var deleteCommand = connection.CreateCommand();
            deleteCommand.CommandText =
            @"
                DELETE FROM Cars
                WHERE Id = @Id
            ";
            deleteCommand.Parameters.AddWithValue("@Id", id);
            deleteCommand.ExecuteNonQuery();
        }
    }

    public static void InsertExpense(Expense expense)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var insertCommand = connection.CreateCommand();
            insertCommand.CommandText =
            @"
 INSERT INTO Expenses (Date, Supplier, Amount, GroupId, CarId)
 VALUES (@Date, @Supplier, @Amount, @GroupId, @CarId)
 ";
            insertCommand.Parameters.AddWithValue("@Date", expense.Date.ToString("yyyy-MM-dd"));
            insertCommand.Parameters.AddWithValue("@Supplier", expense.Supplier);
            insertCommand.Parameters.AddWithValue("@Amount", expense.Amount);
            insertCommand.Parameters.AddWithValue("@GroupId", expense.GroupId);
            insertCommand.Parameters.AddWithValue("@CarId", expense.CarId);
            insertCommand.ExecuteNonQuery();
        }
    }

    public static List<Expense> GetAllExpenses()
    {
        var expenses = new List<Expense>();
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var selectCommand = connection.CreateCommand();
            selectCommand.CommandText =
            @"
 SELECT Id, Date, Supplier, Amount, GroupId, CarId
 FROM Expenses
 ";

            using (var reader = selectCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    expenses.Add(new Expense
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.Parse(reader.GetString(1)),
                        Supplier = reader.GetString(2),
                        Amount = reader.GetDecimal(3),
                        GroupId = reader.GetInt32(4),
                        CarId = reader.GetInt32(5)
                    });
                }
            }
        }
        return expenses;
    }

    public static List<Expense> GetExpensesByCarId(int carId)
    {
        var expenses = new List<Expense>();
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var selectCommand = connection.CreateCommand();
            selectCommand.CommandText =
            @"
 SELECT Id, Date, Supplier, Amount, GroupId, CarId
 FROM Expenses
 WHERE CarId = @CarId
 ";
            selectCommand.Parameters.AddWithValue("@CarId", carId);

            using (var reader = selectCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    expenses.Add(new Expense { Id = reader.GetInt32(0), Date = DateTime.Parse(reader.GetString(1)), Supplier = reader.GetString(2), Amount = reader.GetDecimal(3), GroupId = reader.GetInt32(4), CarId = reader.GetInt32(5) });
                }
            }
        }
        return expenses;
    }

    public static void UpdateExpense(Expense expense)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var updateCommand = connection.CreateCommand();
            updateCommand.CommandText =
            @"
 UPDATE Expenses
 SET Date = @Date, Supplier = @Supplier, Amount = @Amount, GroupId = @GroupId, CarId = @CarId
 WHERE Id = @Id
 ";
            updateCommand.Parameters.AddWithValue("@Date", expense.Date.ToString("yyyy-MM-dd"));
            updateCommand.Parameters.AddWithValue("@Supplier", expense.Supplier);
            updateCommand.Parameters.AddWithValue("@Amount", expense.Amount);
            updateCommand.Parameters.AddWithValue("@GroupId", expense.GroupId);
            updateCommand.Parameters.AddWithValue("@CarId", expense.CarId);
            updateCommand.Parameters.AddWithValue("@Id", expense.Id);
            updateCommand.ExecuteNonQuery();
        }
    }

    public static void DeleteExpense(int id)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var deleteCommand = connection.CreateCommand();
            deleteCommand.CommandText =
            @"
 DELETE FROM Expenses
 WHERE Id = @Id
 ";
            deleteCommand.Parameters.AddWithValue("@Id", id);
            deleteCommand.ExecuteNonQuery();
        }
    }

    public static void InsertExpenseGroup(ExpenseGroup group)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var insertCommand = connection.CreateCommand();
            insertCommand.CommandText =
            @"
                INSERT INTO ExpenseGroups (Name)
                VALUES (@Name)
            ";
            insertCommand.Parameters.AddWithValue("@Name", group.Name);
            insertCommand.ExecuteNonQuery();
        }
    }

    public static List<ExpenseGroup> GetAllExpenseGroups()
    {
        var groups = new List<ExpenseGroup>();
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var selectCommand = connection.CreateCommand();
            selectCommand.CommandText =
            @"
                SELECT Id, Name
                FROM ExpenseGroups
            ";

            using (var reader = selectCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    groups.Add(new ExpenseGroup { Id = reader.GetInt32(0), Name = reader.GetString(1) });
                }
            }
        }
        return groups;
    }

    public static void UpdateExpenseGroup(ExpenseGroup group)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var updateCommand = connection.CreateCommand();
            updateCommand.CommandText =
            @"
                UPDATE ExpenseGroups
                SET Name = @Name
                WHERE Id = @Id
            ";
            updateCommand.Parameters.AddWithValue("@Name", group.Name);
            updateCommand.Parameters.AddWithValue("@Id", group.Id);
            updateCommand.ExecuteNonQuery();
        }
    }

    public static void DeleteExpenseGroup(int id)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var deleteCommand = connection.CreateCommand();
            deleteCommand.CommandText = "DELETE FROM ExpenseGroups WHERE Id = @Id";
            deleteCommand.Parameters.AddWithValue("@Id", id);
            deleteCommand.ExecuteNonQuery();
        }
    }
}

