using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using Newtonsoft.Json;
using System.Net.NetworkInformation;

public class PlayerShape {
    public PrometeoCarController controller;
    public GameObject obj;
    public Vector3 getPosition(){
        return obj.transform.position;
    }
    public Quaternion getRotation(){
        return obj.transform.rotation;
    }
}

public class SocketManager : MonoBehaviour{
    // Start is called before the first frame update
    public GameObject[] prefabs;
    public PlayerShape[] players;
    private bool[] mRunning;
    private Thread[] mThread;
    private TcpListener[] tcp_Listener;
    private int[] remotePort;
    private VehiclesPool pool;
    public GameObject[] playerPosition;
    private int numPlayer;
    public int getNumPlayer(){
        return this.numPlayer;
    }
    public PlayerShape[] getPlayers(){
        return players;
    }    
    public PlayerShape getPlayer(int index = 0){
        return players[index];
    }
    public Vector3 getPositionPlayers(int index = 0){
        return players[index].obj.transform.position;
    }
    public Quaternion getRotationPlayers(int index = 0){
        return players[index].obj.transform.rotation;
    }
    // public Vector3 getPositionPlayer(){
    //     return 
    // }
    public void SetControllerAvtivate(int index){
        if (index < 0 || index >= numPlayer){
            return;
        }
        for (int i = 0; i < numPlayer; i ++){
            players[i].controller.controllerActivate = false;
        }
        players[index].controller.controllerActivate = true;
    }

    public void StartSocketServer(){
        numPlayer = playerPosition.Length;
        pool = VehiclesPool.Create(prefabs);
        pool.ReclaimAll();
        IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();
        // Create Player Port
        remotePort = new int[numPlayer];
        int i = 0;
        int baseIP = 11000;
        for (int j = 0; j < numPlayer; j ++){
            bool portOpen =  true;
            while (i < 100 && portOpen) {
                remotePort[j] = baseIP + i;
                foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
                {
                    if (remotePort[j] == tcpi.LocalEndPoint.Port){
                        portOpen = false;
                        break;
                    }
                }

                i += 1;
                if (!portOpen)
                    portOpen = true;
                else 
                    break;
            }
            // Debug.Log(remotePort[j]);//
        }

        //Socket repair
        tcp_Listener = new TcpListener[numPlayer];
        mRunning = new bool[numPlayer];
        mThread = new Thread[numPlayer];
        // Create Player instance
        CreatePlayers();
    }
    public void CreatePlayers(){
        // Create Player instance
        numPlayer = playerPosition.Length;
        players = new PlayerShape[numPlayer];
        for (int ind = 0; ind < numPlayer; ind += 1){
            int perfabIndx = UnityEngine.Random.Range(0, prefabs.Length);
            var shape = pool.Get((ShapeLabel)perfabIndx);
            var newObj = shape.obj;
            newObj.transform.position = playerPosition[ind].transform.position;
            newObj.transform.rotation = playerPosition[ind].transform.rotation;

            players[ind] = new PlayerShape(){
                obj = newObj, 
                controller = newObj.GetComponent<PrometeoCarController>()
            };
            
            RestartServer(ind);
        }
    }
    void StartListening(int serverIndex){
        try{
            tcp_Listener[serverIndex] = new TcpListener(IPAddress.Any, remotePort[serverIndex]); //System.Net.IPAddress
            tcp_Listener[serverIndex].Start();
            // Debug.Log("Server Started at host: localhost, port "+remotePort);//

            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String jsonData = null;

            while (mRunning[serverIndex]){
                // check if new connections are pending, if not, be nice and sleep 100ms
                if (!tcp_Listener[serverIndex].Pending()){
                    Thread.Sleep(100);
                }
                else{
                    TcpClient client = tcp_Listener[serverIndex].AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    int i = 0;
                    jsonData = null;
                    byte[] msg = null;
                    // Loop to receive all the data sent by the client.
                    while((i = stream.Read(bytes, 0, bytes.Length))!=0){
                        jsonData = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        var myDetails = JsonConvert.DeserializeObject < SetController > (jsonData);
                        String returnMessage = "";
                        switch (myDetails.Cmd){
                            case 1:
                                PrometeoCarController.VehicleStageCl vhS = players[serverIndex].controller.GetState();
                                var VehicleStage_ = new VehicleStage{
                                    Cmd = 1,
                                    Speed = vhS.crSpeed,
                                    Angle = vhS.crSteering,
                                    Lat = vhS.location.x,
                                    Lon = vhS.location.y,
                                    Heading = vhS.rotation.y
                                };
                                returnMessage = JsonConvert.SerializeObject(VehicleStage_);
                                msg = Encoding.UTF8.GetBytes(returnMessage);
                                break;
                            case 2:
                                msg = players[serverIndex].controller.imageResult.originalIMG;
                                break;
                            case 3:
                                msg = players[serverIndex].controller.imageResult.segmentIMG;
                                break;
                            default:
                            break;
                        }
                        Debug.Log(msg.Length);
                        stream.Write(msg, 0, msg.Length);
                    }
                    client.Close();
                    Debug.Log("Client closed" );
                }
            } // while
        }
        catch (ThreadAbortException){
            // Debug.Log("Error");//
        }
        finally{
            mRunning[serverIndex] = false;
            tcp_Listener[serverIndex].Stop();
        }
    }
    public void RestartServer(int serverIndex){
        stopListening(serverIndex);
        mRunning[serverIndex] = true;
        mThread[serverIndex] = new Thread (() => StartListening(serverIndex));
        mThread[serverIndex].Start();
    }

    public void stopListening(int serverIndex){
        mRunning[serverIndex] = false;
    }
    private void Awake() {
        StartSocketServer();
    }
    void Start(){
        
    }
    void Update(){
        
    }
}
