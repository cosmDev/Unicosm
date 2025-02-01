using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

using UnityEditor;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
#endif

namespace CosmosDev.UniCosm
{
	[AddComponentMenu("UniCosm/CosmosSignArbitrary")]
	[HelpURL("https://cosmdev.github.io/unicosm-doc/scripts/CosmosSignArbitrary.cs.html")]
	public class CosmosSignArbitrary : MonoBehaviour
	{
		[DllImport("__Internal")]
		private static extern bool CosmosSignArbitraryCall(string chainId, string message, string signerType, string experimentalSuggestChain);

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
        [Title("Sign configuration")] 
        [Tooltip("Message to verify and proof the ownership of wallet")]
#endif
		public string messageToVerify = "420";

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
        public void DebugSignArbitrary() { 
            if (myServer != null)
            {         
                if (myServer.ServerIsOnline == true)
                {
                    var sendSuggestChain = ""; 
                    if (this.chainConfig._ChainConfiguration.experimentalSuggestChain != "") {
                        sendSuggestChain = "&experimentalSuggestChain=" + this.chainConfig._ChainConfiguration.experimentalSuggestChain; 
                    }                     
                    Application.OpenURL(myServer.host + "/?type=CosmosSignArbitrary&chainId=" + 
                        chainConfig._ChainConfiguration.chainId + 
                        "&message=" + messageToVerify + 
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
                    Debug.Log(messageToVerify);
#elif UNITY_WEBGL
                    CosmosSignArbitraryCall(
                        this.chainConfig._ChainConfiguration.chainId, 
                        messageToVerify, 
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
	}
}
