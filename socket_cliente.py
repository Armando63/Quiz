using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("1 = Buscar servidor (broadcast)");
        Console.WriteLine("2 = Conectar por IP manual");
        string opcion = Console.ReadLine();

        UdpClient udp = new UdpClient(65534); // IMPORTANTE: escuchar en mismo puerto
        udp.EnableBroadcast = true;

        byte[] msg = Encoding.UTF8.GetBytes("BUSCAR_SERVIDOR");

        IPEndPoint destino;

        if (opcion == "2")
        {
            Console.Write("Ingresa la IP del servidor: ");
            string ip = Console.ReadLine();
            destino = new IPEndPoint(IPAddress.Parse(ip), 65534);
        }
        else
        {
            destino = new IPEndPoint(IPAddress.Broadcast, 65534);
        }

        udp.Send(msg, msg.Length, destino);

        IPEndPoint remoto = new IPEndPoint(IPAddress.Any, 0);
        udp.Client.ReceiveTimeout = 5000;

        try
        {
            byte[] resp = udp.Receive(ref remoto);
            string respuesta = Encoding.UTF8.GetString(resp);

            Console.WriteLine("Servidor encontrado!");
            Console.WriteLine("IP: " + remoto.Address);
            Console.WriteLine("Respuesta: " + respuesta);
        }
        catch (SocketException)
        {
            Console.WriteLine("No hay servidor respondiendo");
        }

        udp.Close();
    }
}