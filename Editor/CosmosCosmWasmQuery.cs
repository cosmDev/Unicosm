using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

// TableColumnWidthExampleComponent.cs

using System;


#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Examples;
#endif

namespace CosmosDev.UniCosm
{
  [AddComponentMenu("UniCosm/CosmosCosmWasmQuery")]
  [HelpURL("https://cosmdev.github.io/unicosm-doc/scripts/CosmosCosmWasmQuery.cs.html")]
  public class CosmosCosmWasmQuery : MonoBehaviour
  {

    [DllImport("__Internal")]
    private static extern bool CosmosCosmWasmQueryCall(string chainId, string contractAddr, string queryNameJson, string queryVariablesJson);

#if UNITY_EDITOR
        [Header("Configuration")]
        [Tooltip("You must link the channel configuration to use this script")]
#endif
    public CosmosDev.UniCosm.CosmosChainConfig chainConfig;

#if UNITY_EDITOR
        [Tooltip("Button linked to call this script.")]    
#endif
    public Button buttonOnclick;
    public bool callOnLoad;

#if UNITY_EDITOR
        [Header("Cosmwasm configuration")]
        [Tooltip("Address of smartcontract linked to the chain configuration.")]
#endif
    public string contractAddress = "bcna1wkwy0xh89ksdgj9hr347dyd2dw7zesmtrue6kfzyml4vdtz6e5wsmlf7je";

#if UNITY_EDITOR
        [Tooltip("The data to exectute on smartcontract. Data must be in json!")]
#endif
    [SerializeField]
    public QueryNameClass queryName;
    [SerializeField]
#if UNITY_EDITOR
        [TableList]
#endif
    public QuerySubItemClass[] queryVariables;

#if UNITY_EDITOR
        [Title("Debug configuration")] 
#endif
    public bool DebugMode = false;

#if UNITY_EDITOR

        [ShowIf("@this.DebugMode == true")]
        public CosmosDev.UniCosm.CosmosDebugServer myServer;

        [ShowIf("@this.DebugMode == true")] 
        [OnInspectorGUI]
        [GUIColor("RGB(0, 1, 0)")]
        private void ServerOnline()
        {   
            if (myServer != null)
            {         
                if (myServer.ServerIsOnline == true)
                {
                    UnityEditor.EditorGUILayout.HelpBox("Server is ONLINE", UnityEditor.MessageType.Info);            
                } 
            }         
        }
        
        [ShowIf("@this.DebugMode == true")] 
        [OnInspectorGUI]
        [GUIColor("red")]
        private void ServerOffline()
        {
            if (myServer != null)
            {         
                if (myServer.ServerIsOnline == false)
                {
                    UnityEditor.EditorGUILayout.HelpBox("Server is OFFLINE", UnityEditor.MessageType.Error);            
                } 
            }                        
        } 

        [ShowIf("@this.DebugMode == true")]
        [DisableIf("@this.myServer == null && this.DebugMode == true")] 
        [Button(ButtonSizes.Large)]
        public void DebugCosmWasmQuery() { 
            if (myServer != null)
            {         
                if (myServer.ServerIsOnline == true)
                {
                    var sendSuggestChain = ""; 
                    if (this.chainConfig._ChainConfiguration.experimentalSuggestChain != "") {
                        sendSuggestChain = "&experimentalSuggestChain=" + this.chainConfig._ChainConfiguration.experimentalSuggestChain; 
                    }   

                    string queryNameJson = JsonUtility.ToJson(queryName);
                    string queryVariablesJson = JsonHelperCosm.ToJson(queryVariables, true);
                    Application.OpenURL(myServer.host + "/?type=CosmosCosmWasmQuery&chainId=" + this.chainConfig._ChainConfiguration.chainId + 
                        "&rpc=" + this.chainConfig._ChainConfiguration.rpcURL + 
                        "&contractAddress=" + contractAddress + 
                        "&queryNameJson=" + queryNameJson + 
                        "&queryVariablesJson=" + queryVariablesJson +
                        sendSuggestChain
                    );                 
                } 
            }  
        } 
#endif

    // Start is called before the first frame update
    void Start()
    {

      if (callOnLoad)
        TaskOnClickConnect();
      else
      {
        Button btn = buttonOnclick.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClickConnect);
      }
    }

    void TaskOnClickConnect()
    {
      Debug.Log("Try to call smartcontract");
#if UNITY_EDITOR
                Debug.Log("Debug from editor");
                string queryNameJson = JsonUtility.ToJson(queryName);
                Debug.Log(queryNameJson);
                string queryVariablesJson = JsonHelperCosm.ToJson(queryVariables, true);
                Debug.Log(queryVariablesJson);
 
#elif UNITY_WEBGL
                string queryNameJson = JsonUtility.ToJson(queryName);
                Debug.Log(queryNameJson);
                string queryVariablesJson = JsonHelperCosm.ToJson(queryVariables, true);
                Debug.Log(queryVariablesJson);

                CosmosCosmWasmQueryCall(this.chainConfig._ChainConfiguration.rpcURL, contractAddress, queryNameJson, queryVariablesJson); 
#else
      Debug.Log("Debug from any other platform");
#endif
    }
  }
  [System.Serializable]
  public class QueryNameClass
  {
    public string queryName;
  }
  [System.Serializable]
  public class QuerySubItemClass
  {
#if UNITY_EDITOR
        [TableColumnWidth(60)]
#endif
    public string key;
    public string Value;
  }
  class JsonHelperCosm
  {
    public static T[] FromJson<T>(string json)
    {
      Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
      return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
      Wrapper<T> wrapper = new Wrapper<T>();
      wrapper.Items = array;
      return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
      Wrapper<T> wrapper = new Wrapper<T>();
      wrapper.Items = array;
      return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
      public T[] Items;
    }
  }
}


