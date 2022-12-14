using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthCare.GUI;
using RemoteHealthCare.Scene;

namespace RemoteHealthCare.Network {
    internal class BikeClient {
        private static TcpClient _client;
        private static NetworkStream _stream;

        private byte[] _totalBuffer = new byte[0];
        private byte[] _buffer = new byte[1024];

        // saves the nodes' uuid's with the name as key
        private Dictionary<string, string> _nodes = new Dictionary<string, string>();
        private List<string> _routes = new List<string>();

        private Terrain _terrain = new Terrain();

        public static bool Connected { get; set; }
        public static bool hasTunnel { get; set; } = false;
        public string Path { get; }
        public string Id { get; private set; }

        private string _ip;
        private int _port;

        public BikeClient(string ip, int port) {

            // checks if the given ip is valid
            if (ip == null || port < 1000)
                throw new MissingFieldException("IP is null or port is already in use");

            _ip = ip;
            _port = port;

            Path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString() +
                   "/Json";
            Id = string.Empty;
        }

        public void Disconnect()
        {
            if (_stream != null && _client != null)
            {
                _stream.Close();
                _client.Close();
                Connected = false;
            }
            else
                throw new Exception("Connection with VR server was never even there.");

        }

        /// <summary>
        /// Creates a connection to the main server
        /// </summary>
        /// <param name="ip">Address of the server</param>
        /// <param name="port">Port-number the server is running on</param>
        public void Connect() {
            //Console.WriteLine("attempting to create tunnel with server");

            // makes a connection with the server with the given ip and port
            try {
                _client = new TcpClient();
                _client.Connect(_ip, _port);
                //Console.WriteLine($"Connection made with {_ip}:{_port}");
                _stream = _client.GetStream();
                Send(@"{""id"": ""session/list""}");
                Connected = true;
            }
            // writes an exception when connection fails
            catch (Exception e) {
                //Console.WriteLine("Error while connecting to server: " + e.Message);
            }

            // creates tunnel to send data 
            _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
            ResetScene();
        }

        /// <summary>
        /// Generates terrain using the given string as an id
        /// </summary>
        /// <param name="name">Identifier of the terrain that will be created</param>
        public void CreateTerrain(string name) {
            // checks if the terrain doesn't already exist
            if (!_nodes.ContainsKey(name)) {
                JObject terrain = JObject.Parse(File.ReadAllText(Path + "/terrain.json"));
                terrain["data"]["dest"] = Id;

                var heights = terrain["data"]["data"]["data"]["heights"] as JArray;
                
                for (var i = 0; i < 256; i++)
                {
                    for (var j = 0; j < 256; j++)
                    {
                        heights.Add(_terrain.TerrainHeights[j, i]);
                    }
                }

                Send(terrain.ToString());

                // adds a node to show the terrain
                JObject node = JObject.Parse(File.ReadAllText(Path + "/terrain_node.json"));
                node["data"]["dest"] = Id;
                node["data"]["data"]["data"]["name"] = name;
                this._nodes.Add(name, "fakeId");

                //Console.WriteLine($"message: {node}");
                Send(node.ToString());

                //wait for the id
                while (!IdReceived(name))
                    Thread.Sleep(1);

                // adds a texture to the terrain
                JObject texture = JObject.Parse(File.ReadAllText(Path + "/add_texture.json"));
                texture["data"]["dest"] = Id;
                texture["data"]["data"]["data"]["id"] = _nodes[name];

                //Console.WriteLine($"message: {texture}");
                Send(texture.ToString());
            }
            else {
                //Console.WriteLine("Terrain name: " + name + " is already used.");
            }
        }

        /// <summary>
        /// Handles incoming messages
        /// </summary>
        /// <param name="ar"></param>
        private void OnRead(IAsyncResult ar) {
            // makes the message darkgray to improve readability
            Console.ForegroundColor = ConsoleColor.DarkGray;

            //Console.WriteLine("Message sent");

            Console.ForegroundColor = ConsoleColor.White;

            // checks if the object does not already exist
            try {
                var rc = _stream.EndRead(ar);
                _totalBuffer = Concat(_totalBuffer, _buffer, rc);
            }
            catch (Exception) {
                //Console.WriteLine("Error");
                return;
            }


            while (_totalBuffer.Length >= 4) {
                int packetSize = BitConverter.ToInt32(_totalBuffer, 0);
                if (_totalBuffer.Length >= packetSize + 4) {
                    string data = Encoding.UTF8.GetString(_totalBuffer, 4, packetSize);
                    JObject jData = JObject.Parse(data);

                    switch (jData["id"].ToObject<string>()) {
                        case "session/list":

                            // the last location of the username in the list, as the username might be in the server multiple times and only the most recent one works.
                            int lastLocation = 0;

                            // iterates through the list of usernames to find the correct id used for tunneling
                            for (int i = 0; jData["data"].ToArray().Length > i; i++) {
                                //Console.WriteLine($"session id user: {jData["data"].ElementAt(i)["clientinfo"]["user"]}");
                                if ($"{jData["data"].ElementAt(i)["clientinfo"]["user"]}" == Environment.UserName)
                                {
                                    lastLocation = i;
                                    //Console.WriteLine($"New last location = {lastLocation}");
                                }
                            }

                            // gets the id for tunneling
                            var session = jData["data"].ElementAt(lastLocation)["id"];

                            // messages the request in the form of JSON to the tunnel
                            string message = @"{""id"" : ""tunnel/create"", ""data"" : {""session"" : """ + session +
                                             "\", \"key\" : \"\"}}";
                            //Console.WriteLine($"Sending: {message}");

                            // sends the message
                            Send(message);
                            break;


                        case "tunnel/create":
                            // checks if the received message is an error and prints that message
                            if (jData["data"]["status"].ToObject<string>() == "error") {
                                //Console.WriteLine("Error while making a tunnel with server, are you running NetworkEngine?");
                                if (AccountLogin.clientScreen != null)
                                    AccountLogin.clientScreen.SetErrorMessage("Error while making a tunnel with server, are you running NetworkEngine?");
                                //Console.WriteLine("Server error message:\n" + jData["data"]);
                                Send(@"{""id"": ""session/list""}");
                                break;
                            }

                            // gets the tunnel id and saves it
                            //Console.WriteLine($"\nServer response Data: {jData["data"]}");
                            Id = jData["data"]["id"].ToObject<string>();
                            hasTunnel = true;
                            Console.WriteLine("got tunnel");

                            // throws an error if the id is empty
                            if (Id != null && Id.Equals(string.Empty)) {
                                throw new Exception("Error, couldn't fetch id from tunnel/create");
                            }

                            break;


                        case "tunnel/send":

                            string tunnelId = jData["data"]["data"]["id"].ToObject<string>();

                            switch (jData["data"]["data"]["id"].ToObject<string>()) {
                                // shows only "callback" so it doesn't clutter the log
                                case "callback":
                                    //Console.WriteLine("callback");
                                    break;


                                case "scene/node/add":
                                    // adds the node to the scene. if the node already exist, deletes it first
                                    //Console.WriteLine("node add:" + jData);
                                    try {
                                        lock (this._nodes) {
                                            //Console.WriteLine("removing: " + jData["data"]["data"]["data"]["name"]);
                                            this._nodes.Remove(jData["data"]["data"]["data"]["name"]
                                                .ToObject<string>());
                                            //Console.WriteLine("adding: " + jData["data"]["data"]["data"]["name"]);
                                            this._nodes.Add(jData["data"]["data"]["data"]["name"].ToObject<string>(),
                                                jData["data"]["data"]["data"]["uuid"].ToObject<string>());
                                        }
                                    }
                                    catch {
                                        //Console.WriteLine("error message");
                                        break;
                                    }

                                    //Console.WriteLine("node id: " + jData["data"]["data"]["data"]["uuid"]);
                                    break;

                                case "route/add":
                                    // adds the route to the list
                                    this._routes.Add(jData["data"]["data"]["data"]["uuid"].ToObject<string>());
                                    //Console.WriteLine("route id: " + jData["data"]["data"]["data"]["uuid"]);
                                    break;

                                case "scene/get":
                                    // updates all the nodes in reaction to get scene response
                                    //Console.WriteLine("updating all nodes");
                                    this.UpdateNodes(jData["data"]["data"]["data"]["children"]);
                                    break;

                                default:
                                    // writes full response when no handling is implemented
                                    //Console.WriteLine("No handling implemented for the id: " +
                                                      //jData["data"]["data"]["id"]);
                                    //Console.WriteLine($"Server response: {jData}");
                                    break;
                            }

                            break;

                        default:
                            // writes default response when function is not found
                            //Console.WriteLine("No handling implemented for the id: " + jData["id"]);
                            //Console.WriteLine($"Server response: {jData}");
                            break;
                    }

                    var newBuffer = new byte[_totalBuffer.Length - packetSize - 4];
                    Array.Copy(_totalBuffer, packetSize + 4, newBuffer, 0, newBuffer.Length);
                    _totalBuffer = newBuffer;
                }
                else {
                    break;
                }
            }

            _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
        }

        /// <summary>
        /// Updates the node library when receiving all nodes from the server
        /// </summary>
        /// <param name="jChildren">All nodes that the server sends</param>
        private void UpdateNodes(JToken jChildren) {
            // resets the dictionary
            this._nodes = new Dictionary<string, string>();
            //Console.WriteLine("nodes:");

            // adds each child within the response
            for (int i = 0; i < jChildren.ToArray<JToken>().Length; i++) {
                //Console.WriteLine(jChildren[i]["name"].ToObject<string>());
                //Console.WriteLine(jChildren[i]);

                try {
                    this._nodes.Add(jChildren[i]["name"].ToObject<string>(), jChildren[i]["uuid"].ToObject<string>());
                }
                catch {
                    // ignored
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static byte[] Concat(byte[] b1, byte[] b2, int count) {
            var r = new byte[b1.Length + count];
            Buffer.BlockCopy(b1, 0, r, 0, b1.Length);
            Buffer.BlockCopy(b2, 0, r, b1.Length, count);
            return r;
        }

        /// <summary>
        /// Sends the given message to the main server
        /// </summary>
        /// <param name="message">Message that will be sent to the server</param>
        private void Send(string message) {
            byte[] prefix = BitConverter.GetBytes(message.Length);
            byte[] data = Encoding.ASCII.GetBytes(message);
            _stream.Write(prefix, 0, prefix.Length);
            _stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Gets the scene
        /// </summary>
        public void GetScene() {
            JObject ob = JObject.Parse(File.ReadAllText(Path + "/get_scene.json"));
            ob["data"]["dest"] = Id;

            //Console.WriteLine($"message: {ob}");
            Send(ob.ToString());
        }

        //TODO put overwrite function in
        /// <summary>
        /// Saves the simulation scene
        /// </summary>
        /// <param name="sceneName">Name of the scene that will be saved</param>
        /// <param name="overwriteFile">File that will be written to</param>
        public void SaveScene(String sceneName, bool overwriteFile) {
            JObject ob = JObject.Parse(File.ReadAllText(Path + "/save_scene.json"));
            ob["data"]["dest"] = Id;
            ob["data"]["data"]["data"]["filename"] = sceneName;
            var overwrite = ob["data"]["data"]["data"]["heights"] as JArray;

            //Console.WriteLine($"message: {ob}");
            Send(ob.ToString());
        }

        /// <summary>
        /// Loads the simulation scene
        /// </summary>
        /// <param name="sceneName">Name of the scene that will be loaded</param>
        public void LoadScene(String sceneName) {
            JObject ob = JObject.Parse(File.ReadAllText(Path + "/load_scene.json"));
            ob["data"]["dest"] = Id;
            ob["data"]["data"]["data"]["filename"] = sceneName;

            //Console.WriteLine($"message: {ob}");
            Send(ob.ToString());
        }

        /// <summary>
        /// Resets the simulation scene
        /// </summary>
        public void ResetScene() {
            Program.NetworkEngineRunning = false;

            JObject ob = JObject.Parse(File.ReadAllText(Path + "/reset.json"));
            ob["data"]["dest"] = Id;

            //Console.WriteLine($"message: {ob}");
            Send(ob.ToString());
        }

        /// <summary>
        /// Finds the node with the given name
        /// </summary>
        /// <param name="nodeName">Node that needs to be found</param>
        public void FindNode(string nodeName) {
            JObject ob = JObject.Parse(File.ReadAllText(Path + "/find_node.json"));
            ob["data"]["dest"] = Id;
            ob["data"]["data"]["data"]["name"] = nodeName;

            //Console.WriteLine($"message: {ob}");
            Send(ob.ToString());
        }

        /// <summary>
        /// Deletes the node from the simulation with the given name
        /// </summary>
        /// <param name="nodeName">Node that needs to be deleted</param>
        public void DeleteNode(string nodeName) {
            if (!this._nodes.ContainsKey(nodeName)) return;
            JObject ob = JObject.Parse(File.ReadAllText(Path + "/delete_node.json"));
            ob["data"]["dest"] = Id;
            ob["data"]["data"]["data"]["id"] = this._nodes[nodeName];

            //Console.WriteLine($"message: {ob}");
            Send(ob.ToString());

            // removes the node in the dictionary
            this._nodes.Remove(nodeName);
        }

        /// <summary>
        /// Creates a model with a parent
        /// </summary>
        /// <param name="modelName">Name of the model you want to create</param>
        /// <param name="modelParent">Name of the parent of the model</param>
        /// <param name="position">3 byte array of the position in X, Y, Z order</param>
        /// <param name="scale">scale of the model</param>
        /// <param name="rotation">3 bybte array of the rotation over X Y Z</param>
        /// <param name="fileName">Name and location of the 3d model file</param>
        /// <param name="animation">Name and location of the animation file, can be empty</param>
        /// <param name="animation">true to not draw the backsides of models, false to draw the backsides of models</param>
        public void CreateModel(string modelName, string modelParent, decimal[] position, decimal scale,
            decimal[] rotation, string fileName, string animation, bool cullbackfaces) {
            // makes sure the name isn't already in use
            if (!_nodes.ContainsKey(modelName)) {
                JObject model = JObject.Parse(File.ReadAllText(Path + "/bike.json"));
                model["data"]["dest"] = Id;
                model["data"]["data"]["data"]["name"] = modelName;

                if (modelParent != "") {
                    if (IdReceived(modelParent)) {
                        model["data"]["data"]["data"]["parent"] = this._nodes[modelParent];
                    }
                    else {
                        return;
                    }
                }
              
                var modelPosition = model["data"]["data"]["data"]["components"]["transform"]["position"] as JArray;
                double minimum = _terrain.TerrainHeights[(int)position[0], (int)position[2]];
                for (int i = 0; i < 3; i++)
                {
                    int currentX = (int)position[0] - 1 + i;
                    if (currentX < 0 || currentX >= 256)
                    {
                        continue;
                    }
                    for (int j = 0; j < 3; j++)
                    {
                        int currentY = (int)position[2] - 1 + j;
                        if (currentY < 0 || currentY >= 256)
                        {
                            continue;
                        }
                        double current = _terrain.TerrainHeights[currentX, currentY];
                        if (current < minimum)
                        {
                            minimum = current;
                        }
                    }
                }
                modelPosition[0] = position[0];
                modelPosition[1] = position[1] + (decimal)minimum;
                modelPosition[2] = position[2];

                model["data"]["data"]["data"]["components"]["transform"]["scale"] = scale;

                var modelRotation = model["data"]["data"]["data"]["components"]["transform"]["rotation"] as JArray;
                modelRotation[0] = rotation[0];
                modelRotation[1] = rotation[1];
                modelRotation[2] = rotation[2];

                model["data"]["data"]["data"]["components"]["model"]["file"] = fileName;

                if (animation != "")
                    model["data"]["data"]["data"]["components"]["model"]["animation"] = animation;
                else
                    model["data"]["data"]["data"]["components"]["model"]["animated"] = false;

                model["data"]["data"]["data"]["components"]["model"]["cullbackfaces"] = cullbackfaces;


                _nodes.Add(modelName, "fakeId");
                //Console.WriteLine($"message: {model}");
                Send(model.ToString());
            }
            else {
                //Console.WriteLine("Node name " + modelName + " already used or parent id not received");
            }
        }

        /// <summary>
        /// Creates bike to the simulation using the given name
        /// </summary>
        /// <param name="bikeName">Name of the bike that will be created</param>
        public void CreateBike(string bikeName) {
            // makes sure the name isn't already in use
            if (!_nodes.ContainsKey(bikeName) && IdReceived("Camera")) {
                JObject bike = JObject.Parse(File.ReadAllText(Path + "/bike.json"));
                bike["data"]["dest"] = Id;
                bike["data"]["data"]["data"]["name"] = bikeName;
                bike["data"]["data"]["data"]["parent"] = this._nodes["Camera"];

                _nodes.Add(bikeName, "fakeId");
                //Console.WriteLine($"message: {bike}");
                Send(bike.ToString());
            }
            else {
                //Console.WriteLine("Node name " + bikeName + " already used");
            }
        }

        /// <summary>
        /// Adds a panel to the simulation using the given name
        /// </summary>
        /// <param name="name">Name of the panel that will be created</param>
        public void AddPanel(string name) {
            // makes sure the name isn't already in use and that the camera id has been received
            if (!_nodes.ContainsKey(name) && IdReceived("Camera")) {
                JObject panel = JObject.Parse(File.ReadAllText(Path + "/add_panel.json"));
                panel["data"]["dest"] = Id;
                panel["data"]["data"]["data"]["name"] = name;
                panel["data"]["data"]["data"]["parent"] = this._nodes["Camera"];

                _nodes.Add(name, "fakeId");
                //Console.WriteLine($"message: {panel}");
                Send(panel.ToString());
            }
            else {
                //Console.WriteLine("Node name " + name + " already used or camera id isnt received");
            }
        }

        /// <summary>
        /// Applies clear_panel.json to the given panel
        /// </summary>
        /// <param name="panelName">Name of the panel that will be cleared</param>
        public void ClearPanel(string panelName) {
            //makes sure id of the panel has been received
            if (IdReceived(panelName)) {
                JObject clear = JObject.Parse(File.ReadAllText(Path + "/clear_panel.json"));
                clear["data"]["dest"] = Id;
                clear["data"]["data"]["data"]["id"] = this._nodes[panelName];

                //Console.WriteLine($"message: {clear}");
                Send(clear.ToString());
            }
            else {
                //Console.WriteLine("Node name " + panelName + " has no id received");
            }
        }

        /// <summary>
        /// Applies add_panel_image.json to the given panel
        /// </summary>
        /// <param name="panelName">Name of the panel the image will be applied to</param>
        public void AddPanelImage(string panelName) {
            //makes sure id of the panel has been received
            if (IdReceived(panelName)) {
                JObject image = JObject.Parse(File.ReadAllText(Path + "/add_panel_image.json"));
                image["data"]["dest"] = Id;
                image["data"]["data"]["data"]["id"] = this._nodes[panelName];

                //Console.WriteLine($"message: {image}");
                Send(image.ToString());
            }
            else {
                //Console.WriteLine("Node name " + panelName + " has no id received");
            }
        }

        /// <summary>
        /// Applies swap_panel.json to the given panel
        /// Making the latest changes visible
        /// </summary>
        /// <param name="panelName">Name of the panel that will be swapped</param>
        public void SwapPanelBuffer(string panelName) {
            //makes sure id of the panel has been received
            if (IdReceived(panelName)) {
                JObject swap = JObject.Parse(File.ReadAllText(Path + "/swap_panel.json"));
                swap["data"]["dest"] = Id;
                swap["data"]["data"]["data"]["id"] = this._nodes[panelName];

                //Console.WriteLine($"message: {swap}");
                Send(swap.ToString());
            }
            else {
                //Console.WriteLine("Node name " + panelName + " has no id received");
            }
        }

        /// <summary>
        /// Applies drawtext.json to the given panel
        /// </summary>
        /// <param name="panelName">Name of the panel the text will be applied to</param>
        /// <param name="text">string that will be written on the panel</param>
        /// <param name="line">The line on which the text will be placed</param>
        public void AddTextToPanel(string panelName, string text, int line) {
            JObject textJson = JObject.Parse(File.ReadAllText(Path + "/drawtext.json"));
            textJson["data"]["dest"] = Id;

            if (IdReceived(panelName)) {
                textJson["data"]["data"]["data"]["id"] = this._nodes[panelName];
                textJson["data"]["data"]["data"]["text"] = text;

                var lineArray = textJson["data"]["data"]["data"]["position"] as JArray;
                lineArray[1] = line * 54;
                //Console.WriteLine($"message: {textJson}");
                Send(textJson.ToString());
            }
            else {
                //Console.WriteLine("Node name " + panelName + " already used");
            }
        }
        
        /// <summary>
        /// Applies drawline.json to the given panel
        /// </summary>
        /// <param name="panelName">Name of the panel the lines will be applied to</param>
        public void AddLineToPanel(string panelName) {
            JObject line = JObject.Parse(File.ReadAllText(Path + "/drawline.json"));
            line["data"]["dest"] = Id;

            if (_nodes.ContainsKey(panelName)) {
                if (_nodes[panelName] != "fakeID") {
                    line["data"]["data"]["data"]["id"] = this._nodes[panelName];
                    //Console.WriteLine($"message: {line}");
                    Send(line.ToString());
                }
            }
            else {
                //Console.WriteLine("Node name " + panelName + " already used");
            }
        }
        
        /// <summary>
        /// Checks if the node ID has already been received in <see cref="OnRead"/>
        /// </summary>
        /// <param name="nodeName">Node ID that will be checked</param>
        /// <returns>Whether the ID has already been received in <see cref="OnRead"/></returns>
        public bool IdReceived(string nodeName) {
            return this._nodes.ContainsKey(nodeName) && this._nodes[nodeName] != "fakeId";
        }

        /// <summary>
        /// Adds route to the simulation using the add_route.json
        /// </summary>
        public void AddRoute() {
            JObject ob = JObject.Parse(File.ReadAllText(Path + "/add_route.json"));
            ob["data"]["dest"] = Id;

            //Console.WriteLine($"message: {ob}");
            Send(ob.ToString());
        }

        /// <summary>
        /// Checks if there is an existing route
        /// </summary>
        /// <param name="route">TODO change this</param>
        /// <returns>Whether there is a route present</returns>
        //TODO change this method
        public bool RouteExists(int route) {
            return this._routes.Count - 1 >= route;
        }

        /// <summary>
        /// Makes it so that the node follows the given route
        /// </summary>
        /// <param name="route">Route that the node will follow</param>
        /// <param name="nodeName">Name of the node that will follow the route. Most likely a bike in sim and camera in vive</param>
        public void FollowRoute(int route, string nodeName) {
            if (this._nodes.ContainsKey(nodeName) && RouteExists(route)) {
                JObject ob = JObject.Parse(File.ReadAllText(Path + "/follow_route.json"));
                ob["data"]["dest"] = Id;

                ob["data"]["data"]["data"]["route"] = this._routes[route];
                ob["data"]["data"]["data"]["node"] = this._nodes[nodeName];

                //Console.WriteLine($"message: {ob}");
                Send(ob.ToString());
            }
            else {
                //Console.WriteLine("route " + route + " and/or " + nodeName + " does not exist");
            }
        }

        /// <summary>
        /// Updates the speed of the bike to the given speed
        /// </summary>
        /// <param name="speed">Speed that the bike should be at</param>
        public void UpdateSpeed(decimal speed) {
            JObject ob = JObject.Parse(File.ReadAllText(Path + "/update_bike_speed.json"));
            ob["data"]["dest"] = Id;
            ob["data"]["data"]["data"]["speed"] = speed / 2;

            try {
                ob["data"]["data"]["data"]["node"] = _nodes["Camera"];
            }
            catch (Exception e) {
                //Console.WriteLine($"Nodes doesnt contain bike: \n{e}");
            }

            Send(ob.ToString());
        }
        
        /// <summary>
        /// Sets the skybox
        /// </summary>
        public void SetSkyBox() {
            JObject ob = JObject.Parse(File.ReadAllText(Path + "/skybox.json"));
            ob["data"]["dest"] = Id;

            //Console.WriteLine($"message: {ob}");
            Send(ob.ToString());
        }
        
        /// <summary>
        /// Add a visible road to a route
        /// </summary>
        /// <param name="route">the number of the route to give a road</param>
        public void AddRoad(int route) {
            if (!RouteExists(route)) return;
            JObject add_road = JObject.Parse(File.ReadAllText(Path + "/add_road.json"));
            add_road["data"]["dest"] = Id;
            add_road["data"]["data"]["data"]["route"] = _routes[route];

            //Console.WriteLine($"message: {add_road}");
            Send(add_road.ToString());
        }

        /// <summary>
        /// Add trees around the road, avoiding said road
        /// </summary>
        public void AddVegetation() {
            //Console.WriteLine("Attempting to place vegetation...");
            Random random = new Random();
            decimal[] treeSpecies = { 4, 4, 4, 7, 7}; //Ratio between tree types
            List<decimal[]> staticBadLocations = SimulateRoute();
            List<decimal[]> dynamicBadLocations = new List<decimal[]>();
            int treeCounter = 0;
            int shrubCounter = 0;

            //Adding trees
            for (int i = 0; i < 5000; i++) {
                decimal randomScale = (decimal)(random.NextDouble() / 2.5 + 0.8);
                decimal[] position = new decimal[3];
                position[0] = random.Next(256);
                position[2] = random.Next(256);
                bool isValidTree = true;

                foreach (decimal[] badLocation in staticBadLocations) {
                    if (Math.Sqrt(Math.Pow(Decimal.ToDouble(position[0] - badLocation[0]), 2) +
                                  Math.Pow(Decimal.ToDouble(position[2] - badLocation[2]), 2)) < (double)(5 * randomScale)) {
                        isValidTree = false;
                    }
                }

                foreach (decimal[] badLocation in dynamicBadLocations) {
                    if (Math.Sqrt(Math.Pow(Decimal.ToDouble(position[0] - badLocation[0]), 2) +
                                  Math.Pow(Decimal.ToDouble(position[2] - badLocation[2]), 2)) < (double)((decimal)4.5 * randomScale) - 1.25) {
                        isValidTree = false;
                    }
                }
                
                if (isValidTree) {
                    CreateModel("tree" + i, "terrain", position, randomScale * 2, new decimal[3],
                        "data/NetworkEngine/models/trees/fantasy/tree" + treeSpecies[random.Next(treeSpecies.Length)] +".obj", "", false);
                    dynamicBadLocations.Add(position);
                    treeCounter++;
                    //Console.WriteLine("Placed tree!");
                }
                else {
                    //Console.WriteLine("Attempted to place tree " + i + " at invalid location, skipping tree...");
                }
            }

            //Adding shrubs
            for (int i = 0; i < 6000; i++) {
                decimal randomScale = (decimal)(random.NextDouble() / 2.5 + 0.4);
                decimal[] position = new decimal[3];
                position[0] = random.Next(256);
                position[1] = (decimal) - 1.5 * randomScale;
                position[2] = random.Next(256);
                bool isValidShrub = true;

                foreach (decimal[] badLocation in staticBadLocations) {
                    if (Math.Sqrt(Math.Pow(Decimal.ToDouble(position[0] - badLocation[0]), 2) +
                                  Math.Pow(Decimal.ToDouble(position[2] - badLocation[2]), 2)) < 4.5) {
                        isValidShrub = false;
                    }
                }

                foreach (decimal[] badLocation in dynamicBadLocations) {
                    if (Math.Sqrt(Math.Pow(Decimal.ToDouble(position[0] - badLocation[0]), 2) +
                                  Math.Pow(Decimal.ToDouble(position[2] - badLocation[2]), 2)) < (double)((decimal)2.5 * randomScale)) {
                        isValidShrub = false;
                    }
                }
                
                if (isValidShrub) {
                    CreateModel("shrub" + i, "terrain", position, randomScale, new decimal[3],
                        "data/NetworkEngine/models/trees/fantasy/tree4.obj", "", false);
                    staticBadLocations.Add(position);
                    shrubCounter++;
                    //Console.WriteLine("Placed shrub!");
                }
                else {
                    //Console.WriteLine("Attempted to place shrub " + i + " at invalid location, skipping shrub...");
                }
            }

            //Console.WriteLine("Finished placing vegetation!\nTrees: " + treeCounter + "\nShrubs: " + shrubCounter);
            
        }
        
        /// <summary>
        /// Calcuates all points in between turn points on the route
        /// </summary>
        /// <returns>Returns a list containing all points along the route including turn points using decimal arrays where index 0 is X coordinate, index 2 is Y coordinates</returns>
        private List<decimal[]> SimulateRoute() {
            //Console.WriteLine("Creating list of bad locations...");
            int xOffset = 33;
            int yOffset = 63;
            List<decimal[]> badLocations = new List<decimal[]>();
            List<decimal[]> initialBadLocations = new List<decimal[]>
            {
                new decimal[] {xOffset + 40, 0, 0 + yOffset},
                new decimal[] {xOffset + 120, 0, 50 + yOffset},
                new decimal[] {xOffset + 155, 0 ,50 + yOffset},
                new decimal[] {xOffset + 130, 0, 20 + yOffset},
                new decimal[] {xOffset + 140, 0, 10 + yOffset},
                new decimal[] {xOffset + 195, 0, 45 + yOffset},
                new decimal[] {xOffset + 200, 0, 90 + yOffset},
                new decimal[] {xOffset + 170, 0, 90 + yOffset},
                new decimal[] {xOffset + 135, 0, 100 + yOffset},
                new decimal[] {xOffset + 140, 0, 125 + yOffset},
                new decimal[] {xOffset + 105, 0, 140 + yOffset},
                new decimal[] {xOffset + 95, 0, 115 + yOffset},
                new decimal[] {xOffset + 95, 0, 95 + yOffset},
                new decimal[] {xOffset + 55, 0, 60 + yOffset},
                new decimal[] {xOffset + 20, 0, 60 + yOffset},
                new decimal[] {xOffset + 5, 0, 75 + yOffset},
                new decimal[] {xOffset + 15, 0, 105 + yOffset},
                new decimal[] {xOffset + 10, 0, 130 + yOffset},
                new decimal[] {xOffset + 55, 0, 140 + yOffset},
                new decimal[] {xOffset + 100, 0, 110 + yOffset},
                new decimal[] {xOffset + 105, 0, 60 + yOffset},
                new decimal[] {xOffset + 45, 0, 40 + yOffset},
                new decimal[] {xOffset + 20, 0, 50 + yOffset},
                new decimal[] {xOffset + 00, 0, 30 + yOffset},
                new decimal[] { xOffset + 40, 0, 0 + yOffset}
            };
        
            for (int i = 0; i < initialBadLocations.Count - 1; i++) {
                badLocations = badLocations.Concat(CalculateIntervalPoints(initialBadLocations[i], initialBadLocations[i + 1])).ToList();
            }
        
            //Console.WriteLine("Finished creating list of bad locations:\n");
            return badLocations;
        }
        
        /// <summary>
        /// Calculates all points in between two specified turn points on the route
        /// </summary>
        /// <param name="p1">Turn point 1</param>
        /// <param name="p2">Turn point 2</param>
        /// <returns>Returns a list containing all in between points including turn points using decimal arrays where index 0 is X coordinate, index 2 is Y coordinate</returns>
        private List<decimal[]> CalculateIntervalPoints(decimal[] p1, decimal[] p2) {
            List<decimal[]> points = new List<decimal[]>();
            int resolution = 20;
        
            double x1 = Decimal.ToDouble(p1[0]);
            double y1 = Decimal.ToDouble(p1[2]);
            double x2 = Decimal.ToDouble(p2[0]);
            double y2 = Decimal.ToDouble(p2[2]);
            
            double xDistance = Math.Abs((x1 - x2) / resolution);
            if (x1 - x2 > 0) { xDistance *= -1; }
            double yDistance = Math.Abs((y1 - y2) / resolution);
            if (y1 - y2 > 0) { yDistance *= -1; }
        
            for (int i = 0; i < resolution + 1; i++) {
                decimal[] intervalPoint = new decimal[3];
                
                intervalPoint[0] = (decimal)(x1 + xDistance * i);
                intervalPoint[1] = 0;
                intervalPoint[2] = (decimal)(y1 + yDistance * i);
        
                points.Add(intervalPoint);
            }
        
            return points;
        }


    }
}