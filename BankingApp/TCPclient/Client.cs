using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Client
{
    static async Task Main()
    {
        const string serverAddress = "127.0.0.1";
        const int port = 12345;

        try
        {
            using TcpClient client = new TcpClient();
            await client.ConnectAsync(serverAddress, port);
            using NetworkStream stream = client.GetStream();

            Console.Write("Nom d'utilisateur : ");
            string username = Console.ReadLine()!;

            // Affiche immédiatement le solde après login
            byte[] requestData = Encoding.UTF8.GetBytes($"balance:{username}");
            await stream.WriteAsync(requestData, 0, requestData.Length);

            byte[] buffer = new byte[1024];
            int bytes = await stream.ReadAsync(buffer, 0, buffer.Length);
            Console.WriteLine($"Solde actuel : {Encoding.UTF8.GetString(buffer, 0, bytes)}");

            while (true)
            {
                Console.WriteLine("\n--- Menu Banque ---");
                Console.WriteLine("1. Consulter solde");
                Console.WriteLine("2. Effectuer un virement");
                Console.WriteLine("3. Quitter");
                Console.Write("Choix : ");
                string choice = Console.ReadLine()!;

                string request = choice switch
                {
                    "1" => $"balance:{username}",
                    "2" => GetTransferRequest(),
                    "3" => "exit",
                    _ => string.Empty
                };

                if (request == "exit") break;
                if (string.IsNullOrEmpty(request)) { Console.WriteLine("Choix invalide"); continue; }

                await stream.WriteAsync(Encoding.UTF8.GetBytes(request), 0, Encoding.UTF8.GetByteCount(request));
                bytes = await stream.ReadAsync(buffer, 0, buffer.Length);
                Console.WriteLine($"Réponse du serveur : {Encoding.UTF8.GetString(buffer, 0, bytes)}");
            }
        }
        catch (Exception ex) { Console.WriteLine($"Erreur : {ex.Message}"); }
    }

    static string GetTransferRequest()
    {
        Console.Write("Compte source : "); string from = Console.ReadLine()!;
        Console.Write("Compte destination : "); string to = Console.ReadLine()!;
        Console.Write("Montant : "); string amount = Console.ReadLine()!;
        return $"transfer:{from}:{to}:{amount}";
    }
}
