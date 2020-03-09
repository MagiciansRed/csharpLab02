using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading;


namespace lab02
{
    class Program
    {
        
        private static void ProcessClientRequestst(object argument){

            TcpClient client = (TcpClient)argument;
            StreamReader sr = new StreamReader(client.GetStream());
                StreamWriter sw = new StreamWriter(client.GetStream());
                try
                {
                    // client request
                    string request = sr.ReadLine();
                    Console.WriteLine(request);
                    string[] tokens = request.Split(' ');
                    string page = tokens[1];
                    if(page == "/"){
                        page = "/index.html";
                    }

                    // Find the file
                    StreamReader file = new StreamReader("./" + page);
                    sw.WriteLine("HTTP/1.0 200 OK\n");

                    // Send the file
                    string data = file.ReadLine();
                    while (data != null){
                        sw.WriteLine(data);
                        sw.Flush();
                        data = file.ReadLine();
                    }
                }
                catch (Exception e)
                {
                    sw.WriteLine("HTTP/1.0 404 OK\n");
                    sw.WriteLine("<h1>404 - File or directory not found.</h1>");
                    sw.Flush();
                }
                client.Close();

        }
        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(8000);
            listener.Start();
            Console.WriteLine("server started...");

            while (true)
            {
                Console.WriteLine("Waiting for a connetion...");
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Accepted new client connection...");
                Thread t = new Thread(ProcessClientRequestst);
                t.Start(client);
            }
        }
    }
}
