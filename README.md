# MobileTopup

Project README
This README serves as a guide to understanding the structure and functionality of the project. It provides an overview of the solution, its components, assumptions made during development, and future improvements.

# Project Structure
API Project: This project contains the services to be consumed by the application. It includes endpoints for managing balances, transactions, and other related functionalities.

# Contracts: 
A shared project containing contracts/interfaces that define the interactions between different services. These contracts facilitate dependency injection and promote loose coupling between components.

# Infrastructure: 
This project handles repository and database operations. It includes implementations for data access and storage. Note: There is a simulation of database operations as no actual database repository exists.

# Test Project: 
Contains test cases to validate different acceptance criteria and ensure the correctness of the implemented functionalities.

# HTTP Balance and Debit Service:
A service created within the API project and consumed via HttpClient. This service handles balance adjustments and debit transactions.

Assumptions and Notes
1- Mistake in Verified and Unverified Max Limit: The task contained errors in the maximum limits for verified and unverified accounts. In the implementation, it was assumed that the maximum limit for verified accounts is 1000, and for unverified accounts, it is 500.

2-No DB Repository: Due to time constraints, a simulation of the database repository and its structure was implemented. Actual read/write operations are not performed.

3-Additional Endpoint Created: An extra endpoint was created to provide an example of usage or to fulfill specific requirements.

Multiple Tools Used: The project utilizes various tools such as IConfiguration, FluentValidation, UnitTest, and logging for efficient development, validation, and testing.

# Future Improvements
Refactoring and Separation of Balance Project: The balance-related functionalities can be further separated and refactored into their own dedicated project for better organization and maintainability.

Actual DB Implementation: Implementing an actual database repository and performing real read/write operations would enhance the reliability and scalability of the solution.

More Specific Custom Exceptions and Responses: Enhancing error handling by implementing more specific and custom exceptions, along with informative responses, would improve the user experience and debugging process.

# Test Users
use the endpoint of admin to get list of users.
# you need to set the BalanceEndpoint in the appsetting.

Conclusion
This README provides a comprehensive overview of the project structure, assumptions, and potential areas for improvement. It serves as a guide for developers and stakeholders to understand the solution and its future direction.
