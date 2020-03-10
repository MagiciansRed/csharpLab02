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
        static void Main(string[] args)
        {
            int port = 8000;
            try{
            if(args.Length != 0){
                port = Int32.Parse(args[0]);
            }
            }catch(FormatException){
                Console.WriteLine(String.Format("Unable to parse {0}",args[0]));
            }
            Console.WriteLine(String.Format("Running on port {0}",port));
            
            TcpListener listener = new TcpListener(port);
            listener.Start();

            while (true)
            {
                Console.WriteLine("Waiting for a connetion");
                TcpClient client = listener.AcceptTcpClient();
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
        }
    }
}
