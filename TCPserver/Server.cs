using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Server
{
    static async Task Main(string[] args)
    {
        const int port = 12345; // Listening port
        TcpListener server = new TcpListener(IPAddress.Any, port);

        try
        {
            // Initialize database
            Database.Initialize();

            // Start server
            server.Start();
            Console.WriteLine($"Serveur bancaire démarré. En attente de connexions sur le port {port}...");

            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                Console.WriteLine("Nouveau client connecté !");
                _ = HandleClientAsync(client); // Handle client in background
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
        }
        finally
        {
            server.Stop();
            Console.WriteLine("Serveur arrêté.");
        }
    }

    private static async Task HandleClientAsync(TcpClient client)
    {
        try
        {
            using NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            while (true)
            {
                // Wait for request
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) break; // Client disconnected

                string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Requête reçue : {request}");

                // Process request
                string response = ProcessRequest(request);

                // Send reply
                byte[] responseData = Encoding.UTF8.GetBytes(response);
                await stream.WriteAsync(responseData, 0, responseData.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la communication avec un client : {ex.Message}");
        }
        finally
        {
            client.Close();
        }
    }

    private static string ProcessRequest(string request)
    {
        string[] parts = request.Split(':');
        string command = parts[0].ToLower();

        return command switch
        {
            "balance" => parts.Length == 2
                ? Database.GetBalance(parts[1]) >= 0
                    ? $"Le solde de {parts[1]} est de {Database.GetBalance(parts[1]):C}."
                    : "Utilisateur introuvable."
                : "Erreur : commande balance invalide. Format: balance:username",

            "transfer" => parts.Length == 4 && decimal.TryParse(parts[3], out decimal amount)
                ? Database.TransferFunds(parts[1], parts[2], amount)
                : "Erreur : requête de virement invalide. Format: transfer:from:to:amount",

            "add" => parts.Length == 3 && decimal.TryParse(parts[2], out decimal startBalance)
                ? Database.AddClient(parts[1], startBalance)
                : "Erreur : commande add invalide. Format: add:username:balance",

            "remove" => parts.Length == 2
                ? Database.RemoveClient(parts[1])
                : "Erreur : commande remove invalide. Format: remove:username",

            "exit" => "Déconnexion demandée.",

            _ => "Commande inconnue."
        };
    }
}
