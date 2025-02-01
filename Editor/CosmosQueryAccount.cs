using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


#if UNITY_EDITOR 
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Reflection; 
 
using Sirenix.OdinInspector.Editor.Examples;
 
#endif

namespace CosmosDev.UniCosm
{
  [AddComponentMenu("UniCosm/CosmosQueryAccount")]
  [HelpURL("https://cosmdev.github.io/unicosm-doc/scripts/CosmosQueryAccount.cs.html")]
  public class CosmosQueryAccount : MonoBehaviour
  {
#if UNITY_EDITOR
            [Tooltip("You must link the channel configuration to use this script")]
#endif
    public CosmosDev.UniCosm.CosmosChainConfig chainConfig;

#if UNITY_EDITOR
            [Tooltip("Button linked to call this script.")]    
#endif

    public bool callOnLoad;
#if UNITY_EDITOR
        [ShowIf("@this.callOnLoad == false")]
#endif
    public Button buttonOnclick;


#if UNITY_EDITOR
        [EnumPaging]
#endif
    public SomeEnum SelectQuery;
    public string address = "osmo13jawsn574rf3f0u5rhu7e8n6sayx5gkw6hnnq3";
    [SerializeField]
    private Text _returnSdkQuery;



#if UNITY_EDITOR
        [Button(ButtonSizes.Large)]
        public void testSdkQuery() {  
            TaskOnClickSdkQuery(); 
        } 
#endif

    void Start()
    {
      _returnSdkQuery.text = "";

      if (callOnLoad)
        TaskOnClickSdkQuery();
      else
      {
        Button btn = buttonOnclick.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClickSdkQuery);
      }
    }

    void TaskOnClickSdkQuery()
    {
#if UNITY_EDITOR
                Debug.Log("Debug from editor");
                Debug.Log(SelectQuery);
                if (SelectQuery == SomeEnum.WalletAmount)
                    StartCoroutine(
                        GetRequestBalances(
                            "/cosmos/bank/v1beta1/balances/" + address + "/by_denom?denom=" + this.chainConfig._ChainConfiguration.chainDenom
                        )
                    );
                else if (SelectQuery == SomeEnum.WalletRewards)
                    StartCoroutine(
                        GetRequestRewards(
                            "/cosmos/distribution/v1beta1/delegators/" + address + "/rewards"
                        )
                    );
             
#elif UNITY_WEBGL
                Debug.Log("Debug from UNITY_WEBGL");
                Debug.Log(SelectQuery);
                address = PlayerPrefs.GetString("PlayerCosmosAddr");
                if (SelectQuery == SomeEnum.WalletAmount)
                    StartCoroutine(
                        GetRequestBalances(
                            "/cosmos/bank/v1beta1/balances/" + address + "/by_denom?denom=" + this.chainConfig._ChainConfiguration.chainDenom
                        )
                    );
                else if (SelectQuery == SomeEnum.WalletRewards)
                    StartCoroutine(
                        GetRequestRewards(
                            "/cosmos/distribution/v1beta1/delegators/" + address + "/rewards"
                        )
                    );      
#else
      Debug.Log("Debug from any other platform");
#endif
    }
    public IEnumerator GetRequestBalances(string uri)
    {
      Debug.Log(this.chainConfig._ChainConfiguration.apiURL + uri);
      using var www = UnityWebRequest.Get(this.chainConfig._ChainConfiguration.apiURL + uri);
      yield return www.SendWebRequest();

      if (www.result != UnityWebRequest.Result.Success)
      {
        Debug.LogWarning($"Error: {www.error}");
        yield break;
      }
      var text = www.downloadHandler.text;
      GetBalance getBalance = JsonUtility.FromJson<GetBalance>(text);
      // BaseAbilities abilities = getBalance.data[0].baseAbilities[0];
      //Debug.Log($"myObject: {getBalance.balance.amount}");    
      _returnSdkQuery.text = (getBalance.balance.amount / 1000000) + " " + this.chainConfig._ChainConfiguration.viewDenom;
    }
    public IEnumerator GetRequestRewards(string uri)
    {
      Debug.Log(this.chainConfig._ChainConfiguration.apiURL + uri);
      using var www = UnityWebRequest.Get(this.chainConfig._ChainConfiguration.apiURL + uri);
      yield return www.SendWebRequest();

      if (www.result != UnityWebRequest.Result.Success)
      {
        Debug.LogWarning($"Error: {www.error}");
        yield break;
      }
      var text = www.downloadHandler.text;
      GetRewards getRewards = JsonUtility.FromJson<GetRewards>(text);
      // BaseAbilities abilities = getRewards.data[0].baseAbilities[0];
      if (getRewards.total?.Length == 0)
      {
        _returnSdkQuery.text = "0 " + this.chainConfig._ChainConfiguration.chainDenom;
        yield break;
      }

      Debug.Log($"getRewards: {getRewards.total[0].amount}");
      _returnSdkQuery.text = (getRewards.total[0].amount / 1000000) + " " + this.chainConfig._ChainConfiguration.viewDenom;
    }
    [System.Serializable]
    class GetBalance
    {
      public Balance balance;
    }

    [System.Serializable]
    class Balance
    {
      public string denom;
      public double amount;
    }

    [System.Serializable]
    class GetRewards
    {
      public Total[] total;
    }

    [System.Serializable]
    class Total
    {
      public string denom;
      public double amount;
    }
    public enum SomeEnum
    {
      WalletAmount,
      WalletRewards
    }

  }
}
