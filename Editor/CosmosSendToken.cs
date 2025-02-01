using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

#if UNITY_EDITOR 
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Reflection; 
#endif

namespace CosmosDev.UniCosm
{
  [AddComponentMenu("UniCosm/CosmosSendToken")]
  [HelpURL("https://cosmdev.github.io/unicosm-doc/scripts/CosmosSendToken.cs.html")]
  public class CosmosSendToken : MonoBehaviour
  {
    [DllImport("__Internal")]
    private static extern bool CosmosBroadcast(
        string chainId,
        string chainRpc,
        string chainGas,
        string chaindenom,
        string addressTo,
        string amount,
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
        [OnValueChanged("UpdateDenomGui")]      
#endif
    public CosmosDev.UniCosm.CosmosChainConfig chainConfig;

#if UNITY_EDITOR
        [Title("Send token configuration")] 
        [Tooltip("Reception address of tokens, this address must correspond to the bech32 of the selected chain.")]
#endif
    public string addressTo = "";

#if UNITY_EDITOR
        [InlineProperty(LabelWidth = 60)]
#endif
    public AmountVector AmountConfig = new AmountVector();

#if UNITY_EDITOR
        [Title("Return information")] 
        [Tooltip("Button linked to call this script.")]
#endif
    public Button buttonOnclick; 

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
        public void DebugSendToken() { 
            if (myServer != null)
            {         
                if (myServer.ServerIsOnline == true)
                {
                    var sendSuggestChain = ""; 
                    if (this.chainConfig._ChainConfiguration.experimentalSuggestChain != "") {
                        sendSuggestChain = "&experimentalSuggestChain=" + this.chainConfig._ChainConfiguration.experimentalSuggestChain; 
                    } 

                    Application.OpenURL(myServer.host + "/?type=CosmosSendToken&chainId=" + this.chainConfig._ChainConfiguration.chainId + 
                        "&rpc=" + this.chainConfig._ChainConfiguration.rpcURL + 
                        "&gasPrice=" + this.chainConfig._ChainConfiguration.gasPrice + 
                        "&chainDenom=" + this.chainConfig._ChainConfiguration.chainDenom + 
                        "&addressTo=" + addressTo.ToString() + 
                        "&amount=" + this.AmountConfig.amount + 
                        "&signer=" + selectSigner.ToString() + 
                        sendSuggestChain
                    );            
                } 
            }  
        } 
 
#endif

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
#elif UNITY_WEBGL
                CosmosBroadcast(
                    this.chainConfig._ChainConfiguration.chainId.ToString(), 
                    this.chainConfig._ChainConfiguration.rpcURL.ToString(), 
                    this.chainConfig._ChainConfiguration.gasPrice.ToString(), 
                    this.chainConfig._ChainConfiguration.chainDenom.ToString(), 
                    addressTo.ToString(), 
                    this.AmountConfig.amount.ToString(), 
                    selectSigner.ToString(),
                    this.chainConfig._ChainConfiguration.experimentalSuggestChain
                ); 
#else
      Debug.Log("Debug from any other platform");
#endif
    }
    private void UpdateDenomGui()
    {
      if (this.chainConfig != null)
      {
        this.AmountConfig.denom = this.chainConfig._ChainConfiguration.viewDenom;
      }
      else
      {
        this.AmountConfig.denom = "";
      }
    }
  }

  [System.Serializable]
  public struct AmountVector
  {
#if UNITY_EDITOR
        [HorizontalGroup]
        [HideLabel]
#endif
    public int amount;

#if UNITY_EDITOR
        [HorizontalGroup]
        [HideLabel]
        [ReadOnly]
#endif
    public string denom;
  }
}
