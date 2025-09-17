using System;
using Microsoft.Data.Sqlite;

class Manager
{
    static void Main()
    {
        Console.WriteLine("--- Manager Console ---");

        Database.Initialize(); // ensure DB exists

        while (true)
        {
            Console.WriteLine("\n1. Ajouter un client");
            Console.WriteLine("2. Supprimer un client");
            Console.WriteLine("3. Voir tous les comptes");
            Console.WriteLine("4. Quitter");
            Console.Write("Choix : ");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddClient();
                    break;
                case "2":
                    RemoveClient();
                    break;
                case "3":
                    ListAccounts();
                    break;
                case "4":
                    Console.WriteLine("Fermeture de la console manager...");
                    return;
                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }
    }

    static void AddClient()
    {
        Console.Write("Nom du nouveau client : ");
        string? user = Console.ReadLine();
        Console.Write("Solde initial : ");
        string? balanceStr = Console.ReadLine();

        if (decimal.TryParse(balanceStr, out decimal balance))
        {
            string result = Database.AddClient(user!, balance);
            Console.WriteLine(result);
        }
        else
        {
            Console.WriteLine("Solde invalide.");
        }
    }

    static void RemoveClient()
    {
        Console.Write("Nom du client à supprimer : ");
        string? user = Console.ReadLine();
        string result = Database.RemoveClient(user!);
        Console.WriteLine(result);
    }

    static void ListAccounts()
    {
        using var connection = new SqliteConnection("Data Source=bank.db");
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT UserName, Balance FROM Accounts";

        using var reader = command.ExecuteReader();
        Console.WriteLine("\n--- Comptes bancaires ---");
        while (reader.Read())
        {
            string name = reader.GetString(0);
            decimal balance = reader.GetDecimal(1);
            Console.WriteLine($"{name} : {balance:C}");
        }
    }
}
