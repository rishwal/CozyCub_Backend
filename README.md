# CozyCub Ecommerce Backend

Welcome to the CozyCub Ecommerce Backend repository! This repository contains the backend codebase for the CozyCub Ecommerce website, built using ASP.NET Core.

## Features

- **Dependency Injection**: Utilizes ASP.NET Core's built-in dependency injection for managing application services.
- **JWT Authentication and Authorization**: Implements JSON Web Token (JWT) based authentication and authorization for secure user access.
- **DTOs (Data Transfer Objects)**: Uses DTOs to transfer data between different layers of the application, enhancing decoupling and security.
- **Mapping**: Utilizes mapping libraries like AutoMapper for mapping between entity models and DTOs.
- **Entity Framework**: Leverages Entity Framework Core for ORM (Object-Relational Mapping) to interact with the database.
- **RESTful APIs**: Provides RESTful API endpoints for performing CRUD (Create, Read, Update, Delete) operations on resources.
- **Secured Endpoints**: Implements authentication and authorization middleware to secure API endpoints.
- **Proper Documentation**: Includes detailed comments and documentation to aid understanding and maintainability.
- **Proper Status Codes**: Returns appropriate HTTP status codes for different API responses to ensure consistency and clarity.
- **Exception Handling**: Implements exception handling middleware to handle exceptions globally and provide consistent error responses.

## Getting Started

To get started with the CozyCub Ecommerce Backend, follow these steps:

1. **Clone the Repository**: Clone this repository to your local machine using Git:
   ```bash
   git clone https://github.com/rishwal/CozyCub_Backend.git
   ```

2. **Set Up the Database**: Ensure you have a compatible database (e.g., SQL Server) configured and update the connection string in `appsettings.json`.

3. **Install Dependencies**: Install the required dependencies using NuGet Package Manager:
   ```bash
   dotnet restore
   ```

4. **Run the Application**: Run the application using the following command:
   ```bash
   dotnet run
   ```

5. **Explore the APIs**: Once the application is running, explore the various API endpoints documented in the source code and interact with them using tools like Postman.

## Contributing

Contributions are welcome! If you'd like to contribute to the CozyCub Ecommerce Backend, please follow these steps:

1. Fork the repository and create your branch (`git checkout -b feature/your-feature`).
2. Commit your changes (`git commit -am 'Add some feature'`).
3. Push to your branch (`git push origin feature/your-feature`).
4. Create a new Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

