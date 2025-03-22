# Employee Management Application

## Description

The Employee Management Application is a system designed to streamline the management of employee data, including personal information, roles, and authentication. This application offers administrators and managers a robust platform for handling employee records efficiently, with full CRUD (Create, Read, Update, Delete) functionalities, role-based access control, and secure authentication mechanisms.

## Features

- **User Authentication**: Secure login system with JWT-based token authentication.
- **Role Management**: Create, update, and manage roles (e.g., Employee, Leader, Admin) with different access privileges.
- **Employee Management**:
    - View a comprehensive list of employees.
    - Add new employee records.
    - Edit existing employee information.
    - Delete employee records.
- **Role-based Access Control**: Restrict access to certain functionalities based on user roles to enhance security.
- **Detailed Logging**: Enable data logging for detailed insights and debugging purposes.

## Database Configuration

To configure the database for the application, you'll need to specify the connection string in `appsettings.json`. Replace the placeholder values with your actual database configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=yourServerAddress;Database=yourDatabase;User Id=yourUsername;Password=yourPassword;"
  }
}
```

if you will run this application in docker, implement configurations bellow:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=mysql;database=employee;user=root;password=root"
  }
}
```

Ensure your database is set up accordingly before starting the application.

## Build and Deployment

### Prerequisites

- .NET 5.0 or higher
- MySQL Server (or compatible database server)

### Build Instructions

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/employee-management.git
   cd employee-management
   ```

2. Build the application:
   ```bash
   dotnet build
   ```

### Running the Application

1. Apply migrations to set up the database:
   ```bash
   dotnet ef database update --project .\EmployeeAPI.DataLibrary\EmployeeAPI.DataLibrary.csproj  --startup-project .\EmployeeAPI.Business\ --context EmployeeDbContext
   ```

2. Start the application:
   ```bash
   dotnet run
   ```

## Running with Docker

### Prerequisites
- Docker Desktop

This approach is only for testing porpouse, to production we advise you to implement secrets and other security rules. 

1. Building and executing containers;
    ```bash
     docker compose up --build -d
   ```

The application should now be running at `http://localhost:5000`.


## Testing

Run the following commands to execute the test suite and ensure the application is working correctly:

```bash
dotnet test
```

Ensure that all tests are passing before deploying to production environments.

## Test User Credentials

For testing purposes, you can use the following credentials to access the application:

- **Username:** admin
- **Password:** admin

These credentials should provide you with admin-level access to explore all the features of the application.

## Contributing

Contributions are welcome! Please create issues for any bugs or feature requests, and feel free to submit pull requests.

