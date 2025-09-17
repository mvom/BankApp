# BankingApp
TCP banking application in C# with client, server, and manager consoles.

## Description
BankingApp is a simple TCP banking application in C#. It includes:

- **Server**: Handles client requests and manages the database.
- **Client**: Allows users to view balance and perform transfers.
- **Manager**: Admin console to add or remove clients and see all accounts.
- **BankingLibrary**: Shared library that contains database operations.

## Features
- Add, remove, and list clients (Manager)
- View balance and transfer funds (Client)
- Shared SQLite database for all projects
- Modular architecture for easy maintenance

## Setup
1. Make sure [.NET 7+ SDK](https://dotnet.microsoft.com/download) is installed.
2. Open PowerShell in project folder:
   ```powershell
   cd C:\Users\Eto\TCPserver
   dotnet run
