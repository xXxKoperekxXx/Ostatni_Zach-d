using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using SimpleJSON;
using Random = UnityEngine.Random;

public class NetWorkManager : MonoBehaviour
{
    public static NetWorkManager instance;
    
        static private readonly char[] Delimeter = new char[] {':'};
    
    [HideInInspector]
    public bool onLogged = false;
    
    [HideInInspector]
    public GameObject localPlayer;
    
    [HideInInspector]
    public string local_player_id;
    
    public Dictionary<string, PlayerManager> networkPlayers = new Dictionary<string, PlayerManager>();
    
    public Transform[] spawnPoints;
    
    [Header("Local Player Prefab")]
    public List<GameObject> localPlayerPrefab;
    
    [Header("Network Player Prefab")]
    public List<GameObject> networkPlayerPrefab;

    private int _isTrabant;
    
    [HideInInspector]
    public bool isGameOver;
    
    void Awake()
    {
        Application.ExternalEval("socket.isReady = true;");
    }
    
    void Start()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
        
            instance = this;
    
            EmitPing();
            EmitJoin();
        }
        else{
            Destroy(this.gameObject);
        }
    
    
    }
    ///<param name="_msg">Message.</param>
    public void OnPrintPongMsg(string data)
    {
        
        var pack = data.Split(Delimeter);
        Debug.Log("Received message: "+pack[0] + "from server : pong");
    
    }
    public void EmitPing(){
        JSONObject jsonObject = new JSONObject();
        jsonObject["msg"] = "ping";
    
        Application.ExternalCall("socket.emit","PING", jsonObject);
    }
    
    ///<param name="_login">Login</param>
    public void EmitJoin()
    {
        JSONObject jsonObject = new JSONObject();
        jsonObject["name"] = "dupa";
        int index = Random.Range(0, spawnPoints.Length);
        string msg = string.Empty;
        int randomNumber = Random.Range(1, 101);
        _isTrabant = 0;
        if (randomNumber > 50)
        {
            _isTrabant = 1;
        }

        jsonObject["position"] = spawnPoints[index].position.x + ":" + spawnPoints[index].position.y + ":" +
                                 spawnPoints[index].position.z;
        jsonObject["isTrabant"] = _isTrabant;
        Application.ExternalCall("socket.emit","LOGIN", jsonObject);
        
    }
    ///<param name="_data">Data.</param>
    public void OnJoinGame(string data)
    {
        Debug.Log("success, joined");
        
        var pack = data.Split(Delimeter);
        

        onLogged = true;
        
        PlayerManager newPlayer;
        
        newPlayer = GameObject.Instantiate(localPlayerPrefab[_isTrabant], new Vector3(float.Parse(pack[2]),float.Parse(pack[3]),float.Parse(pack[4])),
        Quaternion.identity).GetComponent<PlayerManager>();
        
        Debug.Log("Playerspawned");
        
        newPlayer.id = pack[0];
        newPlayer.isLocalPlayer = true;
        newPlayer.isOnline = true;
        newPlayer.Set3DName(pack[1]);
    
        networkPlayers[pack[0]] = newPlayer;
    
        localPlayer = networkPlayers[pack[0]].gameObject;
        local_player_id = pack[0];
        
        Debug.Log("pl in game");
    
    
    
    }
    
    ///<param name="_msg">Message.</param>
    void OnSpawnPlayer(string data)
    {
        var pack = data.Split(Delimeter);
        bool alreadyExist = false;
    
        if(networkPlayers.ContainsKey(pack[0]))
        {
            alreadyExist = true;
        }
        if(!alreadyExist)
        {
            Debug.Log("received spawn network player");    
            PlayerManager newPlayer;
            
            
            var car = Int32.Parse(pack[5]);

            newPlayer = GameObject.Instantiate(networkPlayerPrefab[car], new Vector3(float.Parse(pack[2]), float.Parse(pack[3]), float.Parse(pack[4])), Quaternion.identity).GetComponent<PlayerManager>();
            Debug.Log("player spawned");
            newPlayer.id = pack[0];
            
            newPlayer.isLocalPlayer = false;
            
            newPlayer.isOnline = true;
            
            //newPlayer.Set3DName(pack[1]);
            
            newPlayer.gameObject.name = pack[0];
            
            networkPlayers[pack[0]] = newPlayer;
            
        }
            
    
    }
    
    ///<param name="data">position and roattion</param>
    
    public void EmitMoveAndRotate(JSONObject data)
    {
        Application.ExternalCall("socket.emit", "MOVE_AND_ROTATE",data);
    }
    
    void OnUpdateMoveAndRotate(string data)
    {
        Debug.Log("received pos and rot");
        var pack = data.Split(Delimeter);
        
        if(networkPlayers.ContainsKey(pack[0]))
        {
            PlayerManager netPlayer = networkPlayers[pack[0]];
            
            netPlayer.UpdatePosition(new Vector3(float.Parse(pack[1]), float.Parse(pack[2]), float.Parse(pack[3])));
            
            netPlayer.UpdateRotation(new Quaternion(netPlayer.transform.rotation.x, float.Parse(pack[4]), netPlayer.transform.rotation.z, netPlayer.transform.rotation.w));
            
        }
    }
    
    ///<param name="_msg">message.</param>
    void OnUserDisconnected(string data)
    {
        var pack = data.Split(Delimeter);
        if(networkPlayers.ContainsKey(pack[0]))
        {
               Destroy(networkPlayers[pack[0]].gameObject);
        }
    }
        
    
    
    

}
