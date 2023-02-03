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
      TcpClient clientSocket = null;
      clientSocket = Initialize();
      CommandLoop(clientSocket);
    }

    static TcpClient Initialize() {
      TcpClient clientSocket = new TcpClient();
      clientSocket.Connect("localhost", 9999);
      
      string connectMsg;
      if (clientSocket.Connected) {
        connectMsg = "This client successfully connected to a server.";
      } else {
        connectMsg = "This client did not connect to the server. Game Over.";
      }
      Console.WriteLine(connectMsg);

      return clientSocket;
    }

    static void CommandLoop( TcpClient clientSocket) {
      NetworkStream stream = clientSocket.GetStream();
      clientSocket.SendTimeout = 0;
      clientSocket.ReceiveTimeout = 0;

      /* don't send text through this socket, we sent a byte array. Should probably use a real Encoding like UTF-8 or something instead of ASCII for a real project. Use the same Encoding throughout the entire program and associated programs.*/
      while (true) {
        byte[] bt = new byte[1024];
        int bytesread = clientSocket.Client.Receive(bt);
        string command = Encoding.ASCII.GetString(bt, 0, bytesread);
        Console.WriteLine(command);

        byte[] txt = Encoding.ASCII.GetBytes(command);
        stream.Write(txt, 0, txt.Length);
      }
    }
  }
}