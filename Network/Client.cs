using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthCare.Scene;

namespace RemoteHealthCare.Network
{
    internal class Client
    {
        private TcpClient _client;
        private NetworkStream _stream;

        private byte[] _totalBuffer = new byte[0];
        private byte[] _buffer = new byte[1024];

        //A dictionary that saves the nodes' uuid's with the name as key
        private Dictionary<string, string> nodes = new Dictionary<string, string>();
        private List<string> routes = new List<string>();

        public string Path { get; }
        public string Id { get; private set; }
        public Client()
        {
            Path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString() + "/Json";
            Id = string.Empty;
        }

        public async Task Connect(string ip, int port)
        {
            //checking if it is a valid IP
            if (ip == null || port < 1000)
                throw new MissingFieldException("IP is null or port is already in use");

            //trying to make a connection with server
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(ip, port);
                Console.WriteLine($"Connection made with {ip}:{port}");
                _stream = _client.GetStream();
                Send(@"{""id"": ""session/list""}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //creating tunnel to send data 
            _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
            CreateTunnel();
        }

        public void SetSkyBox(double time)
        {
            JObject ob = JObject.Parse(File.ReadAllText(Path + "/skybox.json"));
            ob["data"]["dest"] = Id;
            ob["data"]["data"]["data"]["time"] = time;

            Console.WriteLine($"message: {ob}");
            Send(ob.ToString());
        }
        public void CreateTerrain(string name)
        {
            if (!nodes.ContainsKey(name))
            {
                JObject terrain = JObject.Parse(File.ReadAllText(Path + "/terrain.json"));
                terrain["data"]["dest"] = Id;

                var heights = terrain["data"]["data"]["data"]["heights"] as JArray;


                Terrain t = new Terrain();
                for (var i = 0; i < 256; i++)
                   for (var j = 0; j < 256; j++)
                       heights.Add(t.TerrainHeights[j, i]);

                Send(terrain.ToString());;

                Thread.Sleep(5000);

                //add a node to show the terrain
                JObject node = JObject.Parse(File.ReadAllText(Path + "/terrain_node.json"));
                node["data"]["dest"] = Id;
                node["data"]["data"]["data"]["name"] = name;
                this.nodes.Add(name, "fakeId");

                Console.WriteLine($"message: {node}");
                Send(node.ToString());

                Thread.Sleep(10000);

                //add a texture to the terrain
                JObject texture = JObject.Parse(File.ReadAllText(Path + "/add_texture.json"));
                texture["data"]["dest"] = Id;
                texture["data"]["data"]["data"]["id"] = nodes[name];

                Console.WriteLine($"message: {texture}");
                Send(texture.ToString());
                
                
            }
            else
            { 
             
                Console.WriteLine("Terrain name: " + name + " is already used.");
            }
        }

        public void OnRead(IAsyncResult ar)
        {
            //make the message darkgray so it isnt as obnoxious
            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.WriteLine("Message sent");

            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                int rc = _stream.EndRead(ar);
                _totalBuffer = Concat(_totalBuffer, _buffer, rc);
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                return;
            }
            while (_totalBuffer.Length >= 4)
            {
                int packetSize = BitConverter.ToInt32(_totalBuffer, 0);
                if (_totalBuffer.Length >= packetSize + 4)
                {
                    string data = Encoding.UTF8.GetString(_totalBuffer, 4, packetSize);
                    JObject jData = JObject.Parse(data);

                    switch (jData["id"].ToObject<string>())
                    {
                        case "session/list":

                            //The last location of the username in the list, as the username might be in the server multible times and only the most recent one works.
                            int lastLocation = 0;

                            //Go through the list to find your username with the id for tunneling
                            for (int i = 0; jData["data"].ToArray().Length > i; i++)
                            {
                                Console.WriteLine($"session id user: {jData["data"].ElementAt(i)["clientinfo"]["user"]}");
                                if ($"{jData["data"].ElementAt(i)["clientinfo"]["user"]}" == Environment.UserName)
                                {
                                    lastLocation = i;
                                    Console.WriteLine($"New last location = {lastLocation}");
                                }
                            }

                            //Get your id for tunneling
                            var session = jData["data"].ElementAt(lastLocation)["id"];

                            //JSon message to request a tunnel
                            string message = @"{""id"" : ""tunnel/create"", ""data"" : {""session"" : """ + session + "\", \"key\" : \"\"}}";
                            Console.WriteLine($"Sending: {message}");

                            //Send that message
                            Send(message);
                            break;

                        case "tunnel/create":

                            //check if you recieve an error message and print that message
                            if (jData["data"]["status"].ToObject<string>() == "error")
                            {
                                Console.WriteLine("Error while making a tunnel with server, are you running NetwerkEngine?");
                                Console.WriteLine("Server error message:\n" + jData["data"]);
                                break;
                            }

                            //Get the tunnel id and save it
                            Console.WriteLine($"\nServer response Data: {jData["data"]}");
                            Id = jData["data"]["id"].ToObject<string>();

                            //throw an error if the id is empty somehow
                            if (Id.Equals(string.Empty))
                                throw new Exception("Error, couldn't fetch id from tunnel/create");
                            break;

                        case "tunnel/send":

                            string tunnelId = jData["data"]["data"]["id"].ToObject<string>();

                            //dont show the callbacks so its easier to debug
                            if (tunnelId == "callback")
                            {
                                Console.WriteLine("callback");
                                break;
                            }

                            //reacht to a add node response to get the id
                            if (tunnelId == "scene/node/add")
                            {
                                Console.WriteLine("node add:" + jData);
                                lock (this.nodes) { 
                                    Console.WriteLine("removing: " + jData["data"]["data"]["data"]["name"]);
                                    this.nodes.Remove(jData["data"]["data"]["data"]["name"].ToObject<string>());
                                    Console.WriteLine("adding: " + jData["data"]["data"]["data"]["name"]);
                                    this.nodes.Add(jData["data"]["data"]["data"]["name"].ToObject<string>(), jData["data"]["data"]["data"]["uuid"].ToObject<string>());
                                    

                                }
                                Console.WriteLine("node id: " + jData["data"]["data"]["data"]["uuid"]);
                                break;
                            }

                            //react to a add route response to get the id
                            if (tunnelId == "route/add")
                            {
                                this.routes.Add(jData["data"]["data"]["data"]["uuid"].ToObject<string>());
                                Console.WriteLine("route id: " + jData["data"]["data"]["data"]["uuid"]);
                                break;
                            }

                            //react to the get scene response so that all the nodes get updated
                            if (tunnelId == "scene/get") 
                            {
                                Console.WriteLine("updating all nodes");
                                this.UpdateNodes(jData["data"]["data"]["data"]["children"]);
                                break;
                            }


                            //No handling implemented so write the full response
                            Console.WriteLine("No handling implemented for the id: " + jData["data"]["data"]["id"]);
                            Console.WriteLine($"Server response: {jData}");
                            break;

                        default:
                            // Server response for other functions
                            Console.WriteLine("No handling implemented for the id: " + jData["id"]);
                            Console.WriteLine($"Server response: {jData}");
                            break;
                    }
                    var newBuffer = new byte[_totalBuffer.Length - packetSize - 4];
                    Array.Copy(_totalBuffer, packetSize + 4, newBuffer, 0, newBuffer.Length);
                    _totalBuffer = newBuffer;
                }
                else
                    break;
            }
            _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
        }

        //update the nodes library when getting all nodes from the server
        private void UpdateNodes(JToken jChildren)
        {
            //reset the dictionary
            this.nodes = new Dictionary<string, string>();
            Console.WriteLine("nodes:");

            //add each child in the response
            for (int i = 0; i < jChildren.ToArray<JToken>().Length; i++) 
            {
                Console.WriteLine(jChildren[i]["name"].ToObject<string>());

                try
                {
                    this.nodes.Add(jChildren[i]["name"].ToObject<string>(), jChildren[i]["uuid"].ToObject<string>());
                }
                catch
                {

                }
            }
        }

        private static byte[] Concat(byte[] b1, byte[] b2, int count)
        {
            byte[] r = new byte[b1.Length + count];
            System.Buffer.BlockCopy(b1, 0, r, 0, b1.Length);
            System.Buffer.BlockCopy(b2, 0, r, b1.Length, count);
            return r;
        }
        public void Send(string message)
        {
            byte[] prefix = BitConverter.GetBytes(message.Length);
            byte[] data = Encoding.ASCII.GetBytes(message);
            _stream.Write(prefix, 0, prefix.Length);
            _stream.Write(data, 0, data.Length);
        }
        public void GetScene()
        {
            JObject ob = JObject.Parse(File.ReadAllText(Path + "/get_scene.json"));
            ob["data"]["dest"] = Id;

            Console.WriteLine($"message: {ob}");
            Send(ob.ToString());
        }

        //TODO put overwrite funcion in
        public void SaveScene(String sceneName, bool overwriteFile)
        {
            JObject ob = JObject.Parse(File.ReadAllText(Path + "/save_scene.json"));
            ob["data"]["dest"] = Id;
            ob["data"]["data"]["data"]["filename"] = sceneName;
            var overwrite = ob["data"]["data"]["data"]["heights"] as JArray;

            Console.WriteLine($"message: {ob}");
            Send(ob.ToString());
        }

        public void LoadScene(String sceneName)
        {
            JObject ob = JObject.Parse(File.ReadAllText(Path + "/load_scene.json"));
            ob["data"]["dest"] = Id;
            ob["data"]["data"]["data"]["filename"] = sceneName;

            Console.WriteLine($"message: {ob}");
            Send(ob.ToString());
        }

        private void CreateTunnel()
        {
            JObject ob = JObject.Parse(File.ReadAllText(Path + "/reset.json"));
            ob["data"]["dest"] = Id;

            Console.WriteLine($"message: {ob}");
            Send(ob.ToString());
        }

        public void ResetScene()
        {
            JObject ob = JObject.Parse(File.ReadAllText(Path + "/reset.json"));
            ob["data"]["dest"] = Id;

            Console.WriteLine($"message: {ob}");
            Send(ob.ToString());
        }

        //find the nodes with the gives name
        public void FindNode(string nodeName) 
        {
            JObject ob = JObject.Parse(File.ReadAllText(Path + "/find_node.json"));
            ob["data"]["dest"] = Id;
            ob["data"]["data"]["data"]["name"] = nodeName;

            Console.WriteLine($"message: {ob}");
            Send(ob.ToString());
        }

        //delete teh node with the given name
        public void DeleteNode(string nodeName)
        {
            if (this.nodes.ContainsKey(nodeName)) 
            { 
                JObject ob = JObject.Parse(File.ReadAllText(Path + "/delete_node.json"));
                ob["data"]["dest"] = Id;
                ob["data"]["data"]["data"]["id"] = this.nodes[nodeName];

                Console.WriteLine($"message: {ob}");
                Send(ob.ToString());

                //also remove the node in the dictionary
                this.nodes.Remove(nodeName);
            }
        }

        public void CreateBike(string bikeName)
        {
            JObject bike = JObject.Parse(File.ReadAllText(Path + "/bike.json"));
            bike["data"]["dest"] = Id;
            bike["data"]["data"]["data"]["name"] = bikeName;

            //make sure the name isnt already used
            if (!nodes.ContainsKey(bikeName))
            {
                nodes.Add(bikeName, "fakeId");
                Console.WriteLine($"message: {bike}");
                Send(bike.ToString());
            }
            else
            {
                Console.WriteLine("Node name " + bikeName + " already used");
            }
        }

        //check if the node id has alreade been received in OnRead
        public bool IdReceived(string nodeName)
        {
            //OnRead removes and then adds the key and id so this fucking sucks
            if (this.nodes.ContainsKey(nodeName))
            { 
                return this.nodes[nodeName] != "fakeId";
            }
            else 
            {
                return false;
            }
        }

        public void AddRoute()
        {
            JObject ob = JObject.Parse(File.ReadAllText(Path + "/add_route.json"));
            ob["data"]["dest"] = Id;

            Console.WriteLine($"message: {ob}");
            Send(ob.ToString());
        }

        //check id the route id has already been added in OnRead
        public bool RouteExists(int route) 
        {
            return this.routes.Count-1 >= route;
        }

        

        public void FollowRoute(int route, string nodeName)
        {
            if (this.nodes.ContainsKey(nodeName) && RouteExists(route)) 
            {
                JObject ob = JObject.Parse(File.ReadAllText(Path + "/follow_route.json"));
                ob["data"]["dest"] = Id;

                ob["data"]["data"]["data"]["route"] = this.routes[route];
                ob["data"]["data"]["data"]["node"] = this.nodes[nodeName];

                Console.WriteLine($"message: {ob}");
                Send(ob.ToString());
            } else 
            { 
                Console.WriteLine("route " + route + " and/or " + nodeName + " does not exist");
            }
        }
    }
}