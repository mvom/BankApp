using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace BankingLibrary
{
    public static class Database
    {
        // Chemin unique pour tous les projets
        private const string ConnectionString = "Data Source=C:\\Users\\Eto\\bank.db";

        public static void Initialize()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Accounts (
                    UserName TEXT PRIMARY KEY,
                    Balance REAL NOT NULL
                );
            ";
            command.ExecuteNonQuery();

            // Ajouter comptes par défaut si vide
            command.CommandText = "SELECT COUNT(*) FROM Accounts;";
            long count = (long)command.ExecuteScalar()!;
            if (count == 0)
            {
                command.CommandText = @"
                    INSERT INTO Accounts (UserName, Balance) VALUES
                    ('user1', 1000.00),
                    ('user2', 1500.50),
                    ('eto', 100000.00),
                    ('esso', 5000.00),
                    ('fayi', 200.00);
                ";
                command.ExecuteNonQuery();
            }
        }

        // ======================== Client operations ========================
        public static decimal GetBalance(string username)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Balance FROM Accounts WHERE UserName = $user";
            command.Parameters.AddWithValue("$user", username);
            var result = command.ExecuteScalar();
            return result != null ? Convert.ToDecimal(result) : -1;
        }

        public static string TransferFunds(string from, string to, decimal amount)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            var getFrom = connection.CreateCommand();
            getFrom.Transaction = transaction;
            getFrom.CommandText = "SELECT Balance FROM Accounts WHERE UserName = $user";
            getFrom.Parameters.AddWithValue("$user", from);
            var fromBalanceObj = getFrom.ExecuteScalar();
            if (fromBalanceObj == null) return "Compte source introuvable.";

            decimal fromBalance = Convert.ToDecimal(fromBalanceObj);
            if (fromBalance < amount) return $"Fonds insuffisants sur le compte {from}.";

            var updateFrom = connection.CreateCommand();
            updateFrom.Transaction = transaction;
            updateFrom.CommandText = "UPDATE Accounts SET Balance = Balance - $amount WHERE UserName = $user";
            updateFrom.Parameters.AddWithValue("$amount", amount);
            updateFrom.Parameters.AddWithValue("$user", from);
            updateFrom.ExecuteNonQuery();

            var updateTo = connection.CreateCommand();
            updateTo.Transaction = transaction;
            updateTo.CommandText = "UPDATE Accounts SET Balance = Balance + $amount WHERE UserName = $user";
            updateTo.Parameters.AddWithValue("$amount", amount);
            updateTo.Parameters.AddWithValue("$user", to);
            int updated = updateTo.ExecuteNonQuery();

            if (updated == 0)
            {
                transaction.Rollback();
                return "Compte destination introuvable.";
            }

            transaction.Commit();
            return $"Virement réussi : {amount:C} transférés de {from} à {to}.";
        }

        // ======================== Manager operations ========================
        public static string AddClient(string username, decimal initialBalance)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Accounts (UserName, Balance) VALUES ($user, $balance)";
            command.Parameters.AddWithValue("$user", username);
            command.Parameters.AddWithValue("$balance", initialBalance);

            try
            {
                command.ExecuteNonQuery();
                return $"Client {username} ajouté avec un solde initial de {initialBalance:C}.";
            }
            catch (SqliteException)
            {
                return $"Erreur : le client {username} existe déjà.";
            }
        }

        public static string RemoveClient(string username)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Accounts WHERE UserName = $user";
            command.Parameters.AddWithValue("$user", username);
            int rows = command.ExecuteNonQuery();
            return rows > 0 ? $"Client {username} supprimé." : $"Client {username} introuvable.";
        }

        public static List<(string UserName, decimal Balance)> ListAccounts()
        {
            var list = new List<(string, decimal)>();
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT UserName, Balance FROM Accounts";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                string user = reader.GetString(0);
                decimal balance = reader.GetDecimal(1);
                list.Add((user, balance));
            }
            return list;
        }
    }
}
