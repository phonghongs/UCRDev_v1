using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShape
{
    public PrometeoCarController controller;
    public GameObject obj;
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
    private PlayerShape[] players = null;
    private VehiclesPool pool;
    private int numPlayer = 0;
    private Transform[] playerPosition;
    public GameObject[] vehiclePrefabs;
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
            int perfabIndx = UnityEngine.Random.Range(0, vehiclePrefabs.Length);
            var shape = pool.Get((ShapeLabel)perfabIndx);
            var newObj = shape.obj;
            newObj.transform.position = playerPosition[ind].transform.position;
            newObj.transform.rotation = playerPosition[ind].transform.rotation;

            players[ind] = new PlayerShape()
            {
                obj = newObj,
                controller = newObj.GetComponent<PrometeoCarController>()
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
