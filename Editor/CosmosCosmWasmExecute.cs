using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEditor;

#if UNITY_EDITOR 
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Examples;
#endif

namespace CosmosDev.UniCosm
{

  [AddComponentMenu("UniCosm/CosmosCosmWasmExecute")]
  [HelpURL("https://cosmdev.github.io/unicosm-doc/scripts/CosmosCosmWasmExecute.cs.html")]
  public class CosmosCosmWasmExecute : MonoBehaviour
  {
    [DllImport("__Internal")]
    private static extern bool CosmosCosmWasmExecuteCall(
        string chainId,
        string rpcURL,
        string gasPrice,
        string chainDenom,
        string contractAddr,
        string execNameJson,
        string execVariablesJson,
        string signerType,
        string experimentalSuggestChain
    );
 
#if UNITY_EDITOR
        [Title("Signer configuration")] 
        [EnumToggleButtons]        
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
#endif
    public SelectSigner selectSigner;
#if UNITY_EDITOR
        [Tooltip("You must link the channel configuration to use this script")]  
#endif
    public CosmosDev.UniCosm.CosmosChainConfig chainConfig;
#if UNITY_EDITOR
        [Tooltip("Button linked to call this script.")]    
#endif
    public Button buttonOnclick;

#if UNITY_EDITOR
        [Title("Cosmwasm configuration")] 
        [Tooltip("Address of smartcontract linked to the chain configuration.")]
#endif
    public string contractAddress = "bcna17m0rkavhnsgwtaz79ugguw42caea3wzhqzkjtcstcvsjdh0jxxdspehgwy";
    [SerializeField]
    public ExectuteNameClass exectuteName;
    [SerializeField]
#if UNITY_EDITOR
        [TableList]
#endif
    public SubItemClass[] exectuteVariables;

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
        public void DebugCosmwasmExecute() { 
            if (myServer != null)
            {         
                if (myServer.ServerIsOnline == true)
                {
                    var sendSuggestChain = ""; 
                    if (this.chainConfig._ChainConfiguration.experimentalSuggestChain != "") {
                        sendSuggestChain = "&experimentalSuggestChain=" + this.chainConfig._ChainConfiguration.experimentalSuggestChain; 
                    }   

                    string exectuteNameJson = JsonUtility.ToJson(exectuteName);
                    string exectuteVariablesJson = JsonHelper.ToJson(exectuteVariables, true);
                    Application.OpenURL(myServer.host + "/?type=CosmosCosmWasmExecute&chainId=" + this.chainConfig._ChainConfiguration.chainId + 
                        "&rpc=" + this.chainConfig._ChainConfiguration.rpcURL + 
                        "&contractAddress=" + contractAddress + 
                        "&gasPrice=" + chainConfig._ChainConfiguration.gasPrice + 
                        "&chainDenom=" + chainConfig._ChainConfiguration.chainDenom + 
                        "&exectuteNameJson=" + exectuteNameJson + 
                        "&exectuteVariablesJson=" + exectuteVariablesJson +
                        "&signer=" + selectSigner.ToString() + 
                        sendSuggestChain
                    );                 
                } 
            }  
        } 

#endif

    // Start is called before the first frame update
    void Start()
    {
      Button btn = buttonOnclick.GetComponent<Button>();
      btn.onClick.AddListener(TaskOnClickConnect);
    }

    void TaskOnClickConnect()
    {
      Debug.Log("Try to connect with keplr");
#if UNITY_EDITOR
                Debug.Log("Debug from editor");

                //Convert array to JSON
                string execNameJson = JsonUtility.ToJson(exectuteName);
                Debug.Log(execNameJson);
                string execVariablesJson = JsonHelperCosm.ToJson(exectuteVariables, true);
                Debug.Log(execVariablesJson);
                
                Debug.Log(chainConfig._ChainConfiguration.experimentalSuggestChain);

#elif UNITY_WEBGL
                //Convert array to JSON
                string execNameJson = JsonUtility.ToJson(exectuteName);
                Debug.Log(execNameJson);
                string execVariablesJson = JsonHelperCosm.ToJson(exectuteVariables, true);
                Debug.Log(execVariablesJson);

                CosmosCosmWasmExecuteCall(
                    chainConfig._ChainConfiguration.chainId.ToString(),             
                    chainConfig._ChainConfiguration.rpcURL.ToString(), 
                    chainConfig._ChainConfiguration.gasPrice.ToString(), 
                    chainConfig._ChainConfiguration.chainDenom.ToString(), 
                    contractAddress, 
                    execNameJson, 
                    execVariablesJson,
                    selectSigner.ToString(),
                    this.chainConfig._ChainConfiguration.experimentalSuggestChain
                ); 
#else
      Debug.Log("Debug from any other platform");
#endif
    }

    public enum SelectSigner
    {
      Keplr,
      Cosmostation,
      Leap
    };
    [System.Serializable]
    public class ExectuteNameClass
    {
      public string exectuteName;
    }
    [System.Serializable]
    public class SubItemClass
    {
      public string key;
      public string Value;
    }
  }
  public static class JsonHelper
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
