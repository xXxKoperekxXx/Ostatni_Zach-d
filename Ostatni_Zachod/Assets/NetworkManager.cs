using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    
        static private readonly char[] Delimeter = new char[] {':'};
    
    [HideInInspector]
    public bool onLogged = false;
    
    [HideInInspector]
    public GameObject localPlayer;
    
    [HideInInspector]
    public string local_player_id;
    
    public Dictionary<string, PlayerManager> networkPlayers = new Dictionary<string, PlayerManager>();
    
    [Header("Local Player Prefab")]
    public GameObject localPlayerPrefab;
    
    [Header("Network Player Prefab")]
    public GameObject networkPlayerPrefab;
    
    [Header("Camera Rig Prefab")]
    public GameObject camRigPref;
    
    [HideInInspector]
    public GameObject camRig;
    
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
    
            Debug.Log("start LOL");
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
        Dictionary<string,string>data = new Dictionary<string,string>();
        
        data["msg"] = "ping";
    
        //JSONObject jo = new JSONObject(data);
    
        //Application.ExternalCall("socket.emit","PING",new JSONObject(data));
    }
    
    ///<param name="_login">Login</param>
    public void EmitJoin()
    {
        Dictionary<string,string>data = new Dictionary<string,string>();
    
    
        //data["name"] = CanvasManager.instance.inputLogin.text;
    
        //int index = Random.Range(0, spawnPoints.Length);
        
        //string msg = string.Empty;
    
        //data["position"] = spawnPoints[index].position.x+":"+spawnPoints[index].position.y+":"+spawnPoints[index].position.z;
    
        //Application.ExternalCall("socket.emit", "LOGIN", new JSONObject(data));
        
    }
    ///<param name="_data">Data.</param>
    public void OnJoinGame(string data)
    {
        Debug.Log("success, joined");
        
        var pack = data.Split(Delimeter);
        
        onLogged = true;
        
        PlayerManager newPlayer;
        
        newPlayer = GameObject.Instantiate(localPlayerPrefab, new Vector3(float.Parse(pack[2]),float.Parse(pack[3]),float.Parse(pack[4])),
        Quaternion.identity).GetComponent<PlayerManager>();
        
        Debug.Log("Playerspawned");
        
        newPlayer.id = pack[0];
        newPlayer.isLocalPlayer = true;
        newPlayer.isOnline = true;
        newPlayer.Set3DName(pack[1]);
    
        networkPlayers[pack[0]] = newPlayer;
    
        localPlayer = networkPlayers[pack[0]].gameObject;
        local_player_id = pack[0];
    
        camRig = GameObject.Instantiate(camRigPref, new Vector3(0f,0f,0f), Quaternion.identity);
        camRig.GetComponent<camera_follow>().SetTarget(localPlayer.transform, newPlayer.cameraToTarget);
        //CanvasManager.instance.OpenScreen(1);
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
            
            newPlayer = GameObject.Instantiate(networkPlayerPrefab, new Vector3(float.Parse(pack[2]), float.Parse(pack[3]), float.Parse(pack[4])), Quaternion.identity).GetComponent<PlayerManager>();
            Debug.Log("player spawned");
            newPlayer.id = pack[0];
            
            newPlayer.isLocalPlayer = false;
            
            newPlayer.isOnline = true;
            
            newPlayer.Set3DName(pack[1]);
            
            newPlayer.gameObject.name = pack[0];
            
            networkPlayers[pack[0]] = newPlayer;
            
        }
            
    
    }
    
    ///<param name="data">position and roattion</param>
    
    public void EmitMoveAndRotate(Dictionary<string,string>data)
    {
        //JSONObject jo = new JSONObject(data);
        //Application.ExternalCall("socket.emit", "MOVE_AND_ROTATE",new JSONObject(data));
    }
    
    void onUpdateMoveAndRotate(string data)
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
