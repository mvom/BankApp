---
# BankingApp

TCP banking application in C# with client, server, and manager consoles.

## Description

**BankingApp** is a TCP banking application in C# composed of multiple modular projects: server, client, and manager console.
All projects share a **SQLite database** to store accounts and balances.

* **Server**: centralizes all client requests and manages the database.
* **Client**: allows users to view their balance and perform transfers.
* **Manager**: console to add, remove, and list all clients.
* **BankingLibrary**: shared library containing all database operations.

---

## Features

### Current Features

* Add, remove, and list clients (Manager)
* View balance and perform transfers (Client)
* Shared SQLite database for all projects
* Modular architecture using the `BankingLibrary`

### Future Features / Advanced Ideas

* **Client authentication** (passwords) 🔒
* **Transaction log** to track operation history 📝
* **Graphical interface** with WinForms or WPF 🎨
* **Network security** using SSL/TLS to encrypt communications between client and server 🌐

---

## File Organisation 

BankingApp/
├─ BankingLibrary/ # Bibliothèque partagée
│ └─ Database.cs
├─ TCPserver/ # Serveur TCP
│ └─ Server.cs
├─ TCPclient/ # Client utilisateur
│ └─ Client.cs
├─ TCPmanager/ # Console manager
│ └─ Manager.cs
├─ bank.db # Base SQLite partagée
└─ BankingApp.sln # Solution .NET

---

## Installation and execution

1. Install [.NET 7+ SDK](https://dotnet.microsoft.com/download).  
2. Open PowerShell in the `BankingApp` folder and run:

### Server

powershell
cd TCPserver
dotnet run

cd TCPclient
dotnet run

cd TCPmanager
dotnet run



