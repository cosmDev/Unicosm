
using UnityEngine;
using Debug = UnityEngine.Debug;
using UnityEditor;
#if UNITY_EDITOR 
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Reflection;


using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
#endif

namespace CosmosDev.UniCosm
{ 
[AddComponentMenu("UniCosm/CosmosDebugServer")]
public class CosmosDebugServer : MonoBehaviour
{
 
    public string host;
    public int port = 3031;
    public string SaveFolder = "Assets/Unicosm/StreamingAssets";
    public int bufferSize = 16;
    public static CosmosDebugServer Instance { get; private set; }

#if UNITY_EDITOR  
    [HideInEditorMode]
#endif
    public bool ServerIsOnline;
    

    private MonoBehaviour controller;
    SimpleHTTPServer myServer;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
 
    } 

#if UNITY_EDITOR 
    [DisableIf("ServerIsOnline")]
    [HorizontalGroup("Split", 0.5f)]
    [Button(ButtonSizes.Large)]
    private void StartUnicosmServer()
    {
        Debug.Log("Start http server");
        StartServer();
        ServerIsOnline = true;
    }

    [DisableInEditorMode]
    [EnableIf("ServerIsOnline")]
    [VerticalGroup("Split/right")]
    [Button(ButtonSizes.Large)]
    private void StopUnicosmServer()
    {
        Debug.Log("Stop http server");
        myServer.Stop();
        ServerIsOnline = false;
    }
 
    public void StartServer()
    {
        myServer = new SimpleHTTPServer(GetSaveFolderPath, port, controller, bufferSize);
        Debug.Log("Server started on " + GetHttpUrl(port));
        // this.host = GetHttpUrl(port);
        this.host = "http://localhost:" + port;
        myServer.OnJsonSerialized += (result) =>
        {
            return JsonUtility.ToJson(result);
        };
    }
    string GetSaveFolderPath
    {
        get
        {
            return SaveFolder;            
        }
    }
    public static string GetHttpUrl(int port)
    {
        return $"http://{GetLocalIPAddress()}:" + port;
    }

    /// <summary>
    /// Get the Host IPv4 adress
    /// </summary>
    /// <returns>IPv4 address</returns>
    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
    public void StopServer()
    {
        Application.Quit();
    }
 
#endif
}
}
 
