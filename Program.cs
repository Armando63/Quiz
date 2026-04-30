using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

class Program
{
    static void Main()
    {
        string ip = ""; // LA IP QUE DICE EL SERVER
        int puerto = 5000;

        try
        {
            TcpClient cliente = new TcpClient(ip, puerto);
            NetworkStream stream = cliente.GetStream();

            Console.WriteLine("Conectado al servidor!");

            // 📥 Recibir pregunta
            byte[] buffer = new byte[2048];
            int bytes = stream.Read(buffer, 0, buffer.Length);

            string mensaje = Encoding.UTF8.GetString(buffer, 0, bytes);

            var doc = JsonDocument.Parse(mensaje);
            var root = doc.RootElement;

            if (root.GetProperty("tipo").GetString() == "pregunta")
            {
                Console.WriteLine("\n🎮 PREGUNTA:");
                Console.WriteLine(root.GetProperty("texto").GetString());

                var opciones = root.GetProperty("opciones");

                for (int i = 0; i < opciones.GetArrayLength(); i++)
                {
                    Console.WriteLine($"{i}: {opciones[i].GetString()}");
                }

                // 🔥 VALIDACIÓN DE INPUT
                int opcion;
                while (true)
                {
                    Console.Write("\nElige una opción: ");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out opcion))
                        break;

                    Console.WriteLine("Ingresa un número válido");
                }

                // 📤 Enviar respuesta
                string respuestaJson = JsonSerializer.Serialize(new
                {
                    tipo = "respuesta",
                    respuesta = opcion
                });

                byte[] datos = Encoding.UTF8.GetBytes(respuestaJson);
                stream.Write(datos, 0, datos.Length);

                // 📥 Recibir resultado
                byte[] buffer2 = new byte[1024];
                int bytes2 = stream.Read(buffer2, 0, buffer2.Length);

                string resultado = Encoding.UTF8.GetString(buffer2, 0, bytes2);

                Console.WriteLine("\n🎯 Resultado:");
                Console.WriteLine(resultado);
            }

            cliente.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error al conectar:");
            Console.WriteLine(e.Message);
        }
    }
}