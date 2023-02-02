using System; //need for Console.WriteLine
using System.IO; //need for StreamWriter
using System.Net; //need for IPAddress
using System.Net.Sockets; //need for Tcp stuff, NetworkStream
using System.Text; //need for Encoding.ASCII.GetString()
namespace ClientApp
{
  class Program
  {
    static void Main(string[] args)
    {
      TcpClient clientSocket = new TcpClient();
      clientSocket.Connect("localhost", 8765);
      //Console.WriteLine(clientSocket.Connected); // displays boolean, true if there is a connection

      var stream = clientSocket.GetStream();
    }
  }
}