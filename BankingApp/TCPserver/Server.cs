using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BankingLibrary;

class Server
{
    static async Task Main()
    {
        const int port = 12345;
        TcpListener listener = new TcpListener(IPAddress.Any, port);

        Database.Initialize();

        listener.Start();
        Console.WriteLine($"Serveur démarré sur le port {port}...");

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            _ = HandleClient(client);
        }
    }

    static async Task HandleClient(TcpClient client)
    {
        try
        {
            using NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            while (true)
            {
                int bytes = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytes == 0) break;
                string request = Encoding.UTF8.GetString(buffer, 0, bytes);

                string response = ProcessRequest(request);
                byte[] responseData = Encoding.UTF8.GetBytes(response);
                await stream.WriteAsync(responseData, 0, responseData.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur client : {ex.Message}");
        }
        finally
        {
            client.Close();
        }
    }

    static string ProcessRequest(string request)
    {
        string[] parts = request.Split(':');
        string command = parts[0].ToLower();

        return command switch
        {
            "balance" => Database.GetBalance(parts[1]) >= 0 ?
                         $"{Database.GetBalance(parts[1]):C}" : "Utilisateur introuvable.",
            "transfer" when parts.Length == 4 =>
                         Database.TransferFunds(parts[1], parts[2], decimal.Parse(parts[3])),
            _ => "Commande inconnue."
        };
    }
}
