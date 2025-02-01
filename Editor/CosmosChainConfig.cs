
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Networking;

#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
#endif

using System;
using System.Reflection;

namespace CosmosDev.UniCosm
{

  [Serializable]
  public class ChainConfiguration
  {
#if UNITY_EDITOR
        [BoxGroup("Chain Configuration")]
        [HorizontalGroup("Chain Configuration/Split", LabelWidth = 80)]
        [BoxGroup("Chain Configuration/Split/Name", showLabel: false)]
        //[BoxGroup("Chain Configuration/Split/Name/NameId", showLabel: false)]
#endif
    public string chainName, chainId;

#if UNITY_EDITOR
        [HideLabel, PropertyOrder(5)]
        [PreviewField(Height = 105), HorizontalGroup("Chain Configuration/Split", width: 105)]
#endif
    public Texture2D Icon;

#if UNITY_EDITOR
        [BoxGroup("Chain Configuration/Split/Name", showLabel: false)]
#endif
    public string apiURL, rpcURL;

#if UNITY_EDITOR
        [BoxGroup("Chain Configuration/test", showLabel: false)]
#endif
    public string viewDenom, chainDenom, exponent, addressPrefix, gasPrice, coingeckoId, experimentalSuggestChain;


  }

  [AddComponentMenu("UniCosm/CosmosChainConfig")]
  [HelpURL("https://cosmdev.github.io/unicosm-doc/scripts/CosmosChainConfig.cs.html")]
  /* [TypeInfoBox(
      "This examples demonstrate a similar use-case to that of the Custom Locator example.\n" +
      "But this time we showcase an AttributeProcessor that will only be applied to list items.")] */
  public class CosmosChainConfig : MonoBehaviour
  {
    /*         [Header("Chain Configuration")]
            public string chainId = ""; 
            public string chainName = "";
            public string apiURL = "";
            public string rpcURL = "";
            public string viewDenom = "";
            public string chainDenom = "";
            public string exponent = "";
            public string addressPrefix = "";
            public string gasPrice = "";
            public string coingeckoId = ""; */


#if UNITY_EDITOR
        [EnumPaging, OnValueChanged("SetCurrentTool")] 
        public SuggestChain SelectChain; 
        private string experimentalSuggestChain = "";
 
        private void ImportSuggestChain()
        {
            if (experimentalSuggestChain == "") {
                return;
            }
            StartCoroutine(GetSuggestChain(experimentalSuggestChain)); 
        }
#endif

#if UNITY_EDITOR
        private void SetCurrentTool()
        {
            ViewResult = false;
            if (SelectChain == SuggestChain.osmotest) {
                experimentalSuggestChain = "https://raw.githubusercontent.com/chainapsis/keplr-chain-registry/refs/heads/main/cosmos/osmo-test.json";
                StartCoroutine(GetSuggestChain(experimentalSuggestChain)); 
                 return;
            }
            experimentalSuggestChain = "https://raw.githubusercontent.com/chainapsis/keplr-chain-registry/refs/heads/main/cosmos/"+SelectChain.ToString()+".json";
            StartCoroutine(GetSuggestChain(experimentalSuggestChain)); 
        }
#endif

#if UNITY_EDITOR
        [HideInEditorMode]
#endif
    public bool ViewResult = false;
#if UNITY_EDITOR
        [HideInEditorMode]
#endif
    public bool lcdTestConnectTrue = false;
#if UNITY_EDITOR
        [HideInEditorMode]
#endif
    public bool rpcTestConnectTrue = false;
#if UNITY_EDITOR
        [HideInEditorMode]
#endif
    public bool ErrorConnectLcd = false;
#if UNITY_EDITOR
        [HideInEditorMode]
#endif
    public bool ErrorConnectRpc = false;

#if UNITY_EDITOR
        [HideInEditorMode]
#endif
    public string errorRpc = "";
#if UNITY_EDITOR
        [HideInEditorMode]
#endif
    public string error = "";

#if UNITY_EDITOR
        [HideLabel]
#endif
    public ChainConfiguration _ChainConfiguration;

#if UNITY_EDITOR
        [ShowIf("lcdTestConnectTrue", true)] 
        [OnInspectorGUI]
        [GUIColor("RGB(0, 1, 0)")]
        private void OnInspectorGUI()
        {
            UnityEditor.EditorGUILayout.HelpBox(_ChainConfiguration.chainName + " LCD connected!", UnityEditor.MessageType.Info);    
        }

        [ShowIf("rpcTestConnectTrue", true)] 
        [OnInspectorGUI]
        [GUIColor("RGB(0, 1, 0)")]
        private void OnInspectorGUI2()
        {
            UnityEditor.EditorGUILayout.HelpBox(_ChainConfiguration.chainName + " RPC connected!", UnityEditor.MessageType.Info);
            
        }
        
        [ShowIf("ErrorConnectLcd", true)] 
        [OnInspectorGUI]
        [GUIColor("#FF0000")]
        private void OnInspectorGUIErrorLcd()
        {
            UnityEditor.EditorGUILayout.HelpBox(_ChainConfiguration.chainName + " LCD not work: " + error, UnityEditor.MessageType.Info);    
        }
        [ShowIf("ErrorConnectRpc", true)] 
        [OnInspectorGUI]
        [GUIColor("#FF0000")]
        private void OnInspectorGUIErrorRpc()
        {
            UnityEditor.EditorGUILayout.HelpBox(_ChainConfiguration.chainName + " RPC not work: " + error, UnityEditor.MessageType.Info);    
        }
 
        // Group buttons
        [BoxGroup("Titles", ShowLabel = false)]      
        [Button(ButtonSizes.Large)]
        [TitleGroup("Titles/Test connection")]
        [ButtonGroup("Titles/Test connection/Buttons")]
        public void TestConnectLcd() { 
            StartCoroutine(GetRequest(_ChainConfiguration.apiURL, "lcd"));  
        }


        [Button(ButtonSizes.Large)]
        [ButtonGroup("Titles/Test connection/Buttons")]

        public void TestConnectRpc() {
            StartCoroutine(GetRequest(_ChainConfiguration.rpcURL, "rpc")); 
        }

        [ShowIf("ViewResult", true)] 
        [Button("Close", ButtonSizes.Large )] 
        [ButtonGroup("Titles/Test connection/Close")]
        private void ClosResult()
        {
            lcdTestConnectTrue = false;  
            rpcTestConnectTrue = false;   
            ErrorConnectLcd = false;
            ErrorConnectRpc = false;   
        } 
 
#endif
    IEnumerator GetSuggestChain(string uri)
    {
      using var www = UnityWebRequest.Get(uri);
      yield return www.SendWebRequest();

      if (www.result != UnityWebRequest.Result.Success)
      {
        Debug.LogWarning($"Error: {www.error}");
        yield break;
      }

      var text = www.downloadHandler.text;
      Debug.Log($"JSON: {text}");
      var loaded = JsonUtility.FromJson<jsonDataClass>(text);


      _ChainConfiguration.chainId = loaded.chainId;
      _ChainConfiguration.chainName = loaded.chainName;
      _ChainConfiguration.rpcURL = loaded.rpc;
      _ChainConfiguration.apiURL = loaded.rest;


      _ChainConfiguration.viewDenom = loaded.currencies[0].coinDenom;
      _ChainConfiguration.chainDenom = loaded.currencies[0].coinMinimalDenom;
      _ChainConfiguration.exponent = loaded.currencies[0].coinDecimals;
      _ChainConfiguration.addressPrefix = loaded.bech32Config.bech32PrefixAccAddr;
      _ChainConfiguration.gasPrice = "0.025";
      _ChainConfiguration.coingeckoId = loaded.currencies[0].coinGeckoId;
      _ChainConfiguration.experimentalSuggestChain = uri;

    }

    [System.Serializable]
    public class jsonDataClass
    {
      public string chainId;
      public string chainName;
      public string chainSymbolImageUrl;
      public string rpc;
      public string rest;
      public Currencies[] currencies;
      public Bech32Config bech32Config;
    }
    [System.Serializable]
    public class Currencies
    {
      public string coinDecimals;
      public string coinDenom;
      public string coinGeckoId;
      public string coinMinimalDenom;
      public string coinImageUrl;
    }
    [System.Serializable]
    public class Bech32Config
    {
      public string bech32PrefixAccAddr;
    }

    IEnumerator GetRequest(string uri, string type)
    {
      Debug.Log("Icon: " + _ChainConfiguration.Icon);
      var baseUri = "";
      if (type == "lcd")
      {
        baseUri = uri + "/cosmos/base/tendermint/v1beta1/node_info";
      }
      if (type == "rpc")
      {
        baseUri = uri;
      }
      using (UnityWebRequest webRequest = UnityWebRequest.Get(baseUri))
      {
        // Request and wait for the desired page.
        yield return webRequest.SendWebRequest();

        string[] pages = uri.Split('/');
        int page = pages.Length - 1;

        ViewResult = true;
        switch (webRequest.result)
        {
          case UnityWebRequest.Result.ConnectionError:
          case UnityWebRequest.Result.DataProcessingError:
            if (type == "lcd")
            {
              lcdTestConnectTrue = false;
              ErrorConnectLcd = true;
              error = webRequest.error;
            }
            if (type == "rpc")
            {
              rpcTestConnectTrue = false;
              ErrorConnectRpc = true;
              errorRpc = webRequest.error;
            }
            break;
          case UnityWebRequest.Result.ProtocolError:
            if (type == "lcd")
            {
              lcdTestConnectTrue = false;
              ErrorConnectLcd = true;
              error = webRequest.error;
            }
            if (type == "rpc")
            {
              rpcTestConnectTrue = uri.Contains("keplr.app") ? true : false;
              // ErrorConnectRpc = true;
              ErrorConnectRpc = uri.Contains("keplr.app") ? false : true;
              errorRpc = webRequest.error;
            }
            break;
          case UnityWebRequest.Result.Success:
            if (type == "lcd")
            {
              lcdTestConnectTrue = true;
              ErrorConnectLcd = false;
            }
            if (type == "rpc")
            {
              rpcTestConnectTrue = true;
              ErrorConnectRpc = false;
            }
            break;
        }
      }
    }
  }
  public enum SuggestChain
  {
    atomone,
    celestia,
    chihuahua,
    cosmoshub,
    osmosis,
    osmotest
  }
  public enum SelectSigner
  {
    Keplr,
    Cosmostation,
    Leap
  };

  [Serializable]
  public class MyClass
  {
    public int height;
  }
}


