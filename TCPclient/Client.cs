using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Client
{
    static async Task Main(string[] args)
    {
        const string serverAddress = "127.0.0.1"; // Server IP adress
        const int port = 12345; // Port used by the server

        try
        {
            // Connection to server
            using TcpClient client = new TcpClient();
            Console.WriteLine($"Connexion au serveur {serverAddress}:{port}...");
            await client.ConnectAsync(serverAddress, port);

            using NetworkStream stream = client.GetStream();

            while (true)
            {
                Console.WriteLine("\n--- Menu Banque ---");
                Console.WriteLine("1. Consulter solde");
                Console.WriteLine("2. Effectuer un virement");
                Console.WriteLine("3. Quitter");
                Console.Write("Choix : ");
                string? choice = Console.ReadLine();

                string request = choice switch
                {
                    "1" => GetBalanceRequest(),
                    "2" => GetTransferRequest(),
                    "3" => "exit",
                    _ => string.Empty
                };

                if (string.IsNullOrEmpty(request))
                {
                    Console.WriteLine("Choix invalide. Veuillez réessayer.");
                    continue;
                }

                if (request == "exit")
                {
                    Console.WriteLine("Déconnexion...");
                    break;
                }

                byte[] requestData = Encoding.UTF8.GetBytes(request);
                await stream.WriteAsync(requestData, 0, requestData.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Réponse du serveur : {response}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
        }
    }

    private static string GetBalanceRequest()
    {
        Console.Write("Entrez votre nom d'utilisateur : ");
        string? user = Console.ReadLine();
        return $"balance:{user}";
    }

    private static string GetTransferRequest()
    {
        Console.Write("Nom du compte source : ");
        string? from = Console.ReadLine();

        Console.Write("Nom du compte destination : ");
        string? to = Console.ReadLine();

        Console.Write("Montant à transférer : ");
        string? amount = Console.ReadLine();

        return $"transfer:{from}:{to}:{amount}";
    }
}
