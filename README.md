# Autorovers is a vehicle catalog platform designed to provide detailed specifications, reviews, and comparisons of bikes and cars. Inspired by platforms like BikeWale and CarDekho, Autorovers focuses on delivering accurate data and a smooth user experience — without the buying/selling complexity.

---------------------------------------------------------------------------------------------------------------------------------------

✨ Features

📖 Comprehensive Catalog – Browse detailed specs, images, and key features of vehicles.

🔍 Advanced Search & Filter – Quickly find models by brand, price, or engine size.

⭐ User Reviews & Ratings – Share and read authentic feedback from riders/drivers.

🖼️ Image Uploads – Visual galleries for every vehicle.

🚗 Future Expansion – Extensible to both two-wheelers and four-wheelers.

---------------------------------------------------------------------------------------------------------------------------------------

🏗️ Tech Stack

Backend: ASP.NET Core (.NET 8) with Clean Architecture

Database: PostgreSQL with Entity Framework Core

Design Pattern: Modular Monolith, CQRS, Unit of Work

Frontend: (Planned) React / TypeScript

DevOps: Azure DevOps Pipelines for CI/CD

---------------------------------------------------------------------------------------------------------------------------------------

📂 Project Structure
Autorovers/
├── AutoroversApi/          # API layer (endpoints, controllers)
├── Autorovers.Application/ # Business logic, CQRS handlers
├── Autorovers.Domain/      # Entities, aggregates, domain events
├── Autorovers.Infrastructure/ # Database, repositories, EF Core migrations
└── README.md

---------------------------------------------------------------------------------------------------------------------------------------

🚀 Getting Started
Prerequisites

.NET 8 SDK

PostgreSQL

Setup
# Clone the repo
git clone https://github.com/<your-username>/Autorovers.git
cd Autorovers

# Apply migrations
dotnet ef database update -p Autorovers.Infrastructure -s AutoroversApi

# Run the API
dotnet run --project AutoroversApi


The API will be available at https://localhost:5001 (or as configured).

---------------------------------------------------------------------------------------------------------------------------------------

🛣️ Roadmap

 Add car catalog alongside bikes

 Build React + TypeScript frontend

 Integrate authentication & user profiles

 Add admin dashboard for vehicle management

 Deploy to Azure with CI/CD pipelines

---------------------------------------------------------------------------------------------------------------------------------------

🤝 Contributing

Contributions are welcome! Please fork the repository and open a pull request.

---------------------------------------------------------------------------------------------------------------------------------------

📜 License

This project is licensed under the MIT License.
