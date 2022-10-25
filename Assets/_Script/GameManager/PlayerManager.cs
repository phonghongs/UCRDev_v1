using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
public class PlayerShape
{
    public int playerID;
    public PrometeoCarController controller;
    public GameObject obj;
    public int playerPort;
    public Vector3 getPosition()
    {
        return obj.transform.position;
    }
    public Quaternion getRotation()
    {
        return obj.transform.rotation;
    }
}

public class PlayerManager : MonoBehaviour
{
    public RenderTexture player1;
    public RenderTexture player2;
    public CanvasController playerCanvas;
    private PlayerShape[] players = null;
    private VehiclesPool pool;
    private int numPlayer = 0;
    private Transform[] playerPosition;
    public GameObject[] vehiclePrefabs;
    public int targetPlayer = 0;
    public static PlayerManager Instance { get; private set; }

    public int getNumPlayer()
    {
        return numPlayer;
    }

    public PlayerShape[] getPlayers()
    {
        return players;
    }

    public PlayerShape getPlayer(int index = 0)
    {
        return players[index];
    }

    public Vector3 getPositionPlayers(int index = 0)
    {
        return players[index].obj.transform.position;
    }

    public Quaternion getRotationPlayers(int index = 0)
    {
        return players[index].obj.transform.rotation;
    }

    public void SetControllerAvtivate(int index)
    {
        if (index < 0 || index >= numPlayer)
        {
            return;
        }
        for (int i = 0; i < numPlayer; i++)
        {
            players[i].controller.controllerActivate = false;
        }
        players[index].controller.controllerActivate = true;
        targetPlayer = index;
    }

    public void SetAVCOntroller()
    {
        for (int ind = 0; ind < numPlayer; ind += 1)
        {
            players[ind].controller.isAvController = !players[ind].controller.isAvController;
            Debug.LogError(players[ind].controller.isAvController);
        }
    }

    public void ResetPlayer(GameObject other)
    {
        for (int ind = 0; ind < numPlayer; ind += 1)
        {
            if (players[ind].controller.vehicleID == other.gameObject.GetComponent<PrometeoCarController>().vehicleID){
                StartCoroutine(ResetHelper(other, ind));
            }
        }
    }

    IEnumerator ResetHelper(GameObject other, int ind){
        players[ind].controller.isAvController = false;
        //other.transform.position = playerPosition[ind].transform.position;
        //other.transform.rotation = playerPosition[ind].transform.rotation;
        other.transform.position = MapManager.Instance.resetPosition.transform.position;
        other.transform.rotation = MapManager.Instance.resetPosition.transform.rotation;
        other.GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(0.1f);
        other.GetComponent<Rigidbody>().isKinematic = false;
        yield return new WaitForSeconds(2f);
        other.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void SetSpawnPosition(Transform[] _playerPosition)
    {
        numPlayer = _playerPosition.Length;
        playerPosition = _playerPosition;

        pool = VehiclesPool.Create(vehiclePrefabs);
        pool.ReclaimAll();
        players = new PlayerShape[numPlayer];

        for (int ind = 0; ind < numPlayer; ind += 1)
        {
            int perfabIndx = ind;
            if (ind >= vehiclePrefabs.Length)
            {
                perfabIndx = UnityEngine.Random.Range(0, vehiclePrefabs.Length);
            }
            var shape = pool.Get((ShapeLabel)perfabIndx);
            var newObj = shape.obj;
            newObj.transform.position = playerPosition[ind].transform.position;
            newObj.transform.rotation = playerPosition[ind].transform.rotation;

            newObj.GetComponent<PrometeoCarController>().vehicleID = ind;

            players[ind] = new PlayerShape()
            {
                playerID = ind,
                obj = newObj,
                controller = newObj.GetComponent<PrometeoCarController>(),
                playerPort = 0000
            };
        }
    }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(transform.root.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool isInnit = false;
    // Update is called once per frame
    void Update()
    {
        if (Instance.players.Length == 2)
        {
            if (!isInnit)
            {
                isInnit = true;
                players[0].controller.Cam4Round2.targetTexture = player1;
                players[1].controller.Cam4Round2.targetTexture = player2;
            }
            else
            {
                playerCanvas.FomatPlayer1(
                    SocketManager.Instance.remotePort[0].ToString(),
                    checkpoint.Instance.currentCheckpoint[0].ToString(),
                    checkpoint.Instance.CalTotalScore(0).ToString()
                    );
                playerCanvas.FomatPlayer2(
                    SocketManager.Instance.remotePort[1].ToString(),
                    checkpoint.Instance.currentCheckpoint[1].ToString(),
                    checkpoint.Instance.CalTotalScore(1).ToString()
                    );
            }
        }
    }
}
