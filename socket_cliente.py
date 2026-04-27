using System.Net;
using System.Net.Sockets;
using System.Text;

// Crear cliente UDP
UdpClient clienteUdp = new UdpClient();

// Habilitar envío por broadcast (a toda la red)
clienteUdp.EnableBroadcast = true;

// Mensaje que se enviará para buscar el servidor
string mensajeBusqueda = "BUSCAR_SERVIDOR";
byte[] datosMensaje = Encoding.UTF8.GetBytes(mensajeBusqueda);

// Enviar mensaje a toda la red en el puerto 65534
clienteUdp.Send(
    datosMensaje,
    datosMensaje.Length,
    new IPEndPoint(IPAddress.Broadcast, 65534)
);

// Endpoint para recibir respuesta (cualquier IP y puerto)
IPEndPoint endpointRemoto = new IPEndPoint(IPAddress.Any, 0);

// Tiempo máximo de espera para recibir respuesta (5 segundos)
clienteUdp.Client.ReceiveTimeout = 5000;

try
{
    // Esperar respuesta del servidor
    byte[] respuesta = clienteUdp.Receive(ref endpointRemoto);

    // Si llega respuesta, significa que hay servidor disponible
    Console.WriteLine("Se conectó con el servidor");

    // mostrar quién respondió
    Console.WriteLine($"IP del servidor: {endpointRemoto.Address}");
}
catch (SocketException)
{
    // Si no hay respuesta en el tiempo establecido
    Console.WriteLine("No hay servidor disponible");
}