using System;
using BankingLibrary;

class Manager
{
    static void Main()
    {
        Database.Initialize();

        while (true)
        {
            Console.WriteLine("\n--- Manager ---");
            Console.WriteLine("1. Ajouter un client");
            Console.WriteLine("2. Supprimer un client");
            Console.WriteLine("3. Voir tous les comptes");
            Console.WriteLine("4. Quitter");
            Console.Write("Choix : ");
            string choice = Console.ReadLine()!;

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
        var accounts = Database.ListAccounts();
        Console.WriteLine("\n--- Comptes bancaires ---");
        foreach (var account in accounts)
        {
            Console.WriteLine($"{account.UserName} : {account.Balance:C}");
        }
    }
}
