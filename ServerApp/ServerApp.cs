using System; //need for Console.WriteLine
using System.IO; //need for StreamWriter
using System.Net; //need for IPAddress
using System.Net.Sockets; //need for Tcp stuff, NetworkStream
using System.Text; //need for Encoding.ASCII.GetString()

namespace ServerApp
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
			---  creates a listener called "listener" that uses TCP protocol, it listens on any IPAddress and uses port 9999
			---  .Start() makes it start listening
			---  the "client" variable is a TcpClient type. TcpClient is a TCP connection from a client to this server. The connection can come from any IP address but must be on 
			*/
			TcpListener listener = new TcpListener(IPAddress.Any, 9999);
			listener.Start(); 
			
			
			/*
			---  from the client connect, we get a Stream
			---  we create a StreamWriter that can write the the client connection Stream
			---  AutoFlush=true makes it so that every "write" command to the stream (which normally goes to a buffer and not yet added to the stream) is automatically put from the buffer into the stream. Without autoflush it will cause an exception to be thrown (TODO why is an exception thrown? Also, apparently AutoFlush is bad, see link below. What is the alternative to AutoFlush)
			
			*/
			TcpClient client = listener.AcceptTcpClient();
			NetworkStream networkStream = client.GetStream();
			StreamWriter writer = new StreamWriter(networkStream);
			writer.AutoFlush = true; 
			/*
			
			*/
			while (true)
			{
				/*
				---  creates a buffer for data to be read into
				---  .Read() reads incoming data (if there is any) into the buffer that is it's first argument, and returns the number of bytes read (so that you can use this later to retrieve the exact amount of data from the buffer that comprised the last Read() command)
				---  takes the exact amount of data from the buffer that was read into the buffer in step 2, encodes it from bytes into a data type (String here), and saves that data into the "dataReceived" variable
				*/
				Console.WriteLine("Enter Command...");
				string command = Console.ReadLine();
				Console.WriteLine("Sending command: \n" + command + "\n\n");
				try {
					writer.WriteLine(command);
				} catch (Exception e) {
					Console.WriteLine("inside exception");
					writer.Close();
					networkStream.Close();
					client.Close();
					Console.WriteLine(e.ToString());
					client = listener.AcceptTcpClient();
					networkStream = client.GetStream();
					writer = new StreamWriter(networkStream);
					writer.AutoFlush = true; 
				}

				byte[] buffer = new byte[client.ReceiveBufferSize];
				int bytesRead = networkStream.Read(buffer, 0, client.ReceiveBufferSize);
				string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
				
				/*
				if on this iteration of the loop it detects that data has been received (thus bytesRead is not 0), we:
					--- print the data received
					--- print the response data we want to send
					--- put the response data into the StreamWriter which connects to the client that gave us the request data
					--- if bytesRead is a weird value, we break from the loop
				*/
				if (bytesRead != 0) {
					Console.WriteLine("Recieved response: \n" + dataReceived + "\n\n");
					
				}
				else {
          /* why are we breaking out of the while loop in situations where there's no bytes read? Shouldn't this be the case most of the time?
          */
					break;
				}

			}
				/*
				makes it so the Console will not shut down after launch the program?
				*/
			
		}

	}

}
