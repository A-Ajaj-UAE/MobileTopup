# MobileTopup

Project README
This README serves as a guide to understanding the structure and functionality of the project. It provides an overview of the solution, its components, assumptions made during development, and future improvements.

# Project Structure
API Project: This project contains the services to be consumed by the application. It includes endpoints for managing balances, transactions, and other related functionalities.

# Business Layer
API Project: This project contains the services to be consumed by the application. It includes services for managing balances, transactions, and other related functionalities.

# Contracts: 
A shared project containing contracts/interfaces that define the interactions between different services. These contracts facilitate dependency injection and promote loose coupling between components.

# Infrastructure: 
This project handles repository and database operations. It includes implementations for data access and storage. Note: There is a simulation of database operations as no actual database repository exists.

# Test Project: 
Contains test cases to validate different acceptance criteria and ensure the correctness of the implemented functionalities.

# HTTP Balance and Debit Service:
A service created within the API project and consumed via HttpClient. This service handles balance adjustments and debit transactions.

# Tools and techniqes
Repository and MS SQL Server Local Database with Auto Migration: This suggests that the project uses a repository pattern for data access and interacts with a Microsoft SQL Server database. The term "auto migration" likely refers to some automated process for managing database schema changes.

AutoMapper: AutoMapper is a library used in .NET development for object-to-object mapping. It simplifies the process of mapping properties from one object to another.

Middleware for Exception Handler: Middleware refers to software components that are assembled into an application pipeline to handle requests and responses. In this context, the middleware is likely used to intercept and handle exceptions that occur during the execution of the application.

FluentValidation: FluentValidation is a .NET library for building strongly-typed validation rules. It provides a fluent interface for defining validation logic and integrates well with ASP.NET and ASP.NET Core applications.

Unit Tests Covering the Acceptance Criteria: This indicates that the project includes unit tests that verify whether the software meets its acceptance criteria. Unit tests are typically small, focused tests that validate individual components or units of code.

Robust Layered Architecture: A robust layered architecture implies that the project's codebase is organized into distinct layers (e.g., presentation layer, business logic layer, data access layer) to promote modularity, scalability, and maintainability.

Contracts and Database Entities Isolation to Support Microservice Architecture: This suggests that the project is designed with a microservices architecture in mind. Contracts likely refer to interfaces or contracts that define the communication between microservices, and database entities isolation implies that each microservice has its own isolated data storage to maintain separation of concerns and autonomy.

These tools and practices are commonly used in modern software development to improve code quality, maintainability, and scalability.

# Assumptions and Notes
1- Mistake in Verified and Unverified Max Limit: The task contained errors in the maximum limits for verified and unverified accounts. In the implementation, it was assumed that the maximum limit for verified accounts is 1000, and for unverified accounts, it is 500.
You may need to run on port 7270 or update the host in appsetting to meet your url.

# Conclusion
This README provides a comprehensive overview of the project structure, assumptions, and potential areas for improvement. It serves as a guide for developers and stakeholders to understand the solution and its future direction.
