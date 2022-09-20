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
using TMPro;

public class SocketManager : MonoBehaviour{
    // Start is called before the first frame update

    private bool[] mRunning;
    private Thread[] mThread;
    private TcpListener[] tcp_Listener;
    private int[] remotePort;

    public TMP_Text portShow;

    public void StartSocketServer(){
        //int numPlayer = PlayerManager.Instance.getNumPlayer();
        int numPlayer = MapManager.Instance.spawnPosition.Length;
        //Socket repair
        tcp_Listener = new TcpListener[numPlayer];
        mRunning = new bool[numPlayer];
        mThread = new Thread[numPlayer];

        if (numPlayer == 0)
        {
            Debug.LogError("Player don't exits!");
            return;
        }
        Debug.Log($"ABC: {numPlayer}");
        IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();
        // Create Player Port
        remotePort = new int[numPlayer];
        int baseIP = 11000;
        int i = 0;
        for (int j = 0; j < numPlayer; j ++){
            // remotePort[j] = baseIP + j;
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
            portShow.text += remotePort[j].ToString() + " ";
            Debug.Log(remotePort[j]);
            RestartServer(j);
        }

      
        // Create Player instance
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
                            case 18520:
                                PrometeoCarController.VehicleStageCl vhS = PlayerManager.Instance.getPlayer(serverIndex).controller.GetState();
                                var VehicleStage_ = new VehicleStage{
                                    Cmd = 18520,
                                    Speed = vhS.crSpeed,
                                    Angle = vhS.crSteering,
                                    Heading = vhS.rotation.y
                                };
                                returnMessage = JsonConvert.SerializeObject(VehicleStage_);
                                msg = Encoding.UTF8.GetBytes(returnMessage);
                                PlayerManager.Instance.getPlayer(serverIndex).controller.SetAVCOntroller(myDetails.Speed, myDetails.Angle);
                                break;
                            case 331:
                                msg = PlayerManager.Instance.getPlayer(serverIndex).controller.imageResult.segmentIMG;
                                break;
                            default:
                            break;
                        }
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
    }
    void Start(){
        StartSocketServer();
    }
    void Update(){
        
    }
}
