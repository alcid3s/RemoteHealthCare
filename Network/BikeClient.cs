﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RemoteHealthCare.Scene;

namespace RemoteHealthCare.Network
{
    internal class BikeClient
    {
        private TcpClient _client;
        private NetworkStream _stream;

        private byte[] _totalBuffer = new byte[0];
        private byte[] _buffer = new byte[1024];

        // saves the nodes' uuid's with the name as key
        private Dictionary<string, string> nodes = new Dictionary<string, string>();
        private List<string> routes = new List<string>();
        
        public string Path { get; }
        public string Id { get; private set; }
        
        public BikeClient()
        {
            Path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString() + "/Json";
            Id = string.Empty;
        }

        /// <summary>
        /// Connects to the main server
        /// </summary>
        /// <param name="ip">address of the server</param>
        /// <param name="port">port-number the server is running on</param>
        public async Task Connect(string ip, int port)
        {
            // checks if the given ip is valid
            if (ip == null || port < 1000)
            {
                throw new MissingFieldException("IP is null or port is already in use");
            }

            //Makes a connection with the server with the given ip and port
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(ip, port);
                Console.WriteLine($"Connection made with {ip}:{port}");
                _stream = _client.GetStream();
                Send(@"{""id"": ""session/list""}");
            }
            //Writes an exception when connection fails
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //Creates tunnel to send data 
            _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
            CreateTunnel();
        }

        /// <summary>
        /// Sets the skybox to represent the given time as the time of day
        /// </summary>
        /// <param name="time">representation of the time as day, represented as: HHMM</param>
        public void SetSkyBox(double time)
        {
            JObject ob = JObject.Parse(File.ReadAllText(Path + "/skybox.json"));
            ob["data"]["dest"] = Id;
            ob["data"]["data"]["data"]["time"] = time;

            Console.WriteLine($"message: {ob}");
            Send(ob.ToString());
        }
        
        /// <summary>
        /// Generates terrain using the given string as an id
        /// </summary>
        /// <param name="name">identifier of the terrain taht will be created</param>
        public void CreateTerrain(string name)
        {
            // checks if the terrain doesn't already exist
            if (!nodes.ContainsKey(name))
            {
                JObject terrain = JObject.Parse(File.ReadAllText(Path + "/terrain.json"));
                terrain["data"]["dest"] = Id;
                
                var heights = terrain["data"]["data"]["data"]["heights"] as JArray;


                Terrain t = new Terrain();
                for (var i = 0; i < 256; i++)
                {
                    for (var j = 0; j < 256; j++)
                    {
                        heights.Add(t.TerrainHeights[j, i]);
                    }
                }

                Send(terrain.ToString());;

                Thread.Sleep(5000);

                // adds a node to show the terrain
                JObject node = JObject.Parse(File.ReadAllText(Path + "/terrain_node.json"));
                node["data"]["dest"] = Id;
                node["data"]["data"]["data"]["name"] = name;
                this.nodes.Add(name, "fakeId");

                Console.WriteLine($"message: {node}");
                Send(node.ToString());

                Thread.Sleep(10000);

                // adds a texture to the terrain
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
        
        /// <summary>
        /// Handles incoming messages
        /// </summary>
        private void OnRead(IAsyncResult ar)
        {
            //Makes the message darkgray to improve readability
            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.WriteLine("Message sent");

            Console.ForegroundColor = ConsoleColor.White;

            //Checks if the object does not already exist
            try
            {
                var rc = _stream.EndRead(ar);
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

                    //TODO comments boven cases zetten met uitleg?
                    switch (jData["id"].ToObject<string>())
                    {
                        case "session/list":

                            //The last location of the username in the list, as the username might be in the server multiple times and only the most recent one works.
                            int lastLocation = 0;

                            //Goes through the list to find the username with the id used for tunneling
                            for (int i = 0; jData["data"].ToArray().Length > i; i++)
                            {
                                Console.WriteLine($"session id user: {jData["data"].ElementAt(i)["clientinfo"]["user"]}");
                                if ($"{jData["data"].ElementAt(i)["clientinfo"]["user"]}" == Environment.UserName)
                                {
                                    lastLocation = i;
                                    Console.WriteLine($"New last location = {lastLocation}");
                                }
                            }

                            //Gets the id for tunneling
                            var session = jData["data"].ElementAt(lastLocation)["id"];

                            //Messages the request in the form of JSON to the tunnel
                            string message = @"{""id"" : ""tunnel/create"", ""data"" : {""session"" : """ + session + "\", \"key\" : \"\"}}";
                            Console.WriteLine($"Sending: {message}");

                            //Sends the message
                            Send(message);
                            break;
                        
                        case "tunnel/create":
                            // checks if the received message is an error and prints that message
                            if (jData["data"]["status"].ToObject<string>() == "error")
                            {
                                Console.WriteLine("Error while making a tunnel with server, are you running NetworkEngine?");
                                Console.WriteLine("Server error message:\n" + jData["data"]);
                                break;
                            }

                            // gets the tunnel id and saves it
                            Console.WriteLine($"\nServer response Data: {jData["data"]}");
                            Id = jData["data"]["id"].ToObject<string>();

                            // throws an error if the id is empty
                            if (Id != null && Id.Equals(string.Empty))
                                throw new Exception("Error, couldn't fetch id from tunnel/create");
                            break;

                        case "tunnel/send":

                            string tunnelId = jData["data"]["data"]["id"].ToObject<string>();

                            //dont show the callbacks so its easier to debug
                            //TODO wat?
                            if (tunnelId == "callback")
                            {
                                Console.WriteLine("callback");
                                break;
                            }
                            
                            if (tunnelId == "scene/node/add")
                            {
                                // adds the node to the scene. if the node already exist, deletes it first
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

                            if (tunnelId == "route/add")
                            {
                                // adds the route to the list
                                this.routes.Add(jData["data"]["data"]["data"]["uuid"].ToObject<string>());
                                Console.WriteLine("route id: " + jData["data"]["data"]["data"]["uuid"]);
                                break;
                            }

                            if (tunnelId == "scene/get") 
                            {
                                // updates all the nodes in reaction to get scene response
                                Console.WriteLine("updating all nodes");
                                this.UpdateNodes(jData["data"]["data"]["data"]["children"]);
                                break;
                            }
                            // writes full response when no handling is implemented
                            Console.WriteLine("No handling implemented for the id: " + jData["data"]["data"]["id"]);
                            Console.WriteLine($"Server response: {jData}");
                            break;

                        default:
                            // writes default response when function is not found
                            Console.WriteLine("No handling implemented for the id: " + jData["id"]);
                            Console.WriteLine($"Server response: {jData}");
                            break;
                    }
                    var newBuffer = new byte[_totalBuffer.Length - packetSize - 4];
                    Array.Copy(_totalBuffer, packetSize + 4, newBuffer, 0, newBuffer.Length);
                    _totalBuffer = newBuffer;
                }
                else
                {
                    break;
                }
            }
            _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
        }

        // Updates the node library when receiving all nodes from the server
        private void UpdateNodes(JToken jChildren)
        {
            // resets the dictionary
            this.nodes = new Dictionary<string, string>();
            Console.WriteLine("nodes:");

            // adds each child within the response
            for (int i = 0; i < jChildren.ToArray<JToken>().Length; i++) 
            {
                Console.WriteLine(jChildren[i]["name"].ToObject<string>());

                try
                {
                    this.nodes.Add(jChildren[i]["name"].ToObject<string>(), jChildren[i]["uuid"].ToObject<string>());
                }
                catch
                {
                    // ignored
                }
            }
        }

        private static byte[] Concat(byte[] b1, byte[] b2, int count)
        {
            var r = new byte[b1.Length + count];
            Buffer.BlockCopy(b1, 0, r, 0, b1.Length);
            Buffer.BlockCopy(b2, 0, r, b1.Length, count);
            return r;
        }

        private void Send(string message)
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

        //TODO put overwrite function in
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

        public void AddPanel(string name) 
        {
            JObject panel = JObject.Parse(File.ReadAllText(Path + "/add_panel.json"));
            panel["data"]["dest"] = Id;
            panel["data"]["data"]["data"]["name"] = name;

            //make sure the name isnt already used
            if (!nodes.ContainsKey(name))
            {
                nodes.Add(name, "fakeId");
                Console.WriteLine($"message: {panel}");
                Send(panel.ToString());
            }
            else
            {
                Console.WriteLine("Node name " + name + " already used");
            }
        }

        public void AddTextToPanel(string panelName) 
        {
            JObject text = JObject.Parse(File.ReadAllText(Path + "/drawtext.json"));
            text["data"]["dest"] = Id;
            

            //make sure the name is present already used
            if (nodes.ContainsKey(panelName))
            {
                if (nodes[panelName] != "fakeID") { 
                    text["data"]["data"]["data"]["id"] = this.nodes[panelName];
                    Console.WriteLine($"message: {text}");
                    Send(text.ToString());
                }
            }
            else
            {
                Console.WriteLine("Node name " + panelName + " already used");
            }
        }

        public void AddLineToPanel(string panelName) 
        {
            JObject line = JObject.Parse(File.ReadAllText(Path + "/drawline.json"));
            line["data"]["dest"] = Id;
            

            //make sure the name is present already used
            if (nodes.ContainsKey(panelName))
            {
                if (nodes[panelName] != "fakeID") { 
                    line["data"]["data"]["data"]["id"] = this.nodes[panelName];
                    Console.WriteLine($"message: {line}");
                    Send(line.ToString());
                }
            }
            else
            {
                Console.WriteLine("Node name " + panelName + " already used");
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