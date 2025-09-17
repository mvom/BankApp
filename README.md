## BankingApp 

description: |
  TCP banking application in C# with client, server, and manager consoles.

  BankingApp is a TCP banking application in C# composed of multiple modular projects: server, client, and manager console.
  All projects share a SQLite database to store accounts and balances.

  - Server: centralizes all client requests and manages the database.
    
  - Client: allows users to view their balance and perform transfers.
    
  - Manager: console to add, remove, and list all clients.
    
  - BankingLibrary: shared library containing all database operations.

features:
  current:
    - Add, remove, and list clients (Manager)
    - View balance and perform transfers (Client)
    - Shared SQLite database for all projects
    - Modular architecture using the BankingLibrary
  future:
    - Client authentication (passwords) 🔒
    - Transaction log to track operation history 📝
    - Graphical interface with WinForms or WPF 🎨
    - Network security using SSL/TLS 🌐


## File Organisation 

```
BankingApp/
├─ BankingLibrary/ # Shared library
│ └─ Database.cs
├─ TCPserver/ # TCP server
│ └─ Server.cs
├─ TCPclient/ # User client
│ └─ Client.cs
├─ TCPmanager/ # Manager consol 
│ └─ Manager.cs
├─ bank.db # Base SQLite shared
└─ BankingApp.sln # .NET Solution
```
---

## Installation and execution

1. Install [.NET 7+ SDK](https://dotnet.microsoft.com/download).  
2. Open PowerShell in the `BankingApp` folder and run:

### Server

```powershell
cd TCPserver
dotnet run

cd TCPClient
dotnet run

cd TCPManager
dotnet run



