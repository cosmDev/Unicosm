using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CosmosDev.UniCosm
{
  public class CosmosReturnData : MonoBehaviour
  {
    [SerializeField]
    private Text _returnAddress;
    [SerializeField]
    private Text _returnSignArbitrary;
    [SerializeField]
    private Text _returnCosmWasmQuery;
    [SerializeField]
    private Text _returnCosmWasmExecute;
    [SerializeField]
    private Text _returnSendTokenCode;
    [SerializeField]
    private Text _returnSendTokenHash;
    [SerializeField]
    private Text _returnSdkQuery;

    public void CosmosReturnAddress(string addr)
    {
      Debug.Log("Debug unity addr: " + addr);
      PlayerPrefs.SetString("PlayerCosmosAddr", addr);
      _returnAddress.text = PlayerPrefs.GetString("PlayerCosmosAddr");
    }
    public void CosmosReturnSignArbitrary(string data)
    {
      Debug.Log("Debug unity SignArbitrary: " + data);
      if (data == "true")
      {
        PlayerPrefs.SetString("PlayerCosmosSignArbitrary", "Wallet verified!");
        _returnSignArbitrary.text = PlayerPrefs.GetString("PlayerCosmosSignArbitrary");
      }
      else
      {
        PlayerPrefs.SetString("PlayerCosmosSignArbitrary", "Wallet not verified! :/");
        _returnSignArbitrary.text = PlayerPrefs.GetString("PlayerCosmosSignArbitrary");
      }
    }
    public void CosmosReturnWasm(string data)
    {
      Debug.Log("Debug unity CosmosReturnWasm: " + data);
      _returnCosmWasmExecute.text = data.ToString();
    }
    public void CosmosReturnWasmQuery(string data)
    {
      Debug.Log("Debug unity CosmosReturnWasmQuery: " + data);
      _returnCosmWasmQuery.text = data.ToString();
    }
    public void CosmosReturnSendTokenCode(int data)
    {
      Debug.Log("Debug unity ReturnSendToken: " + data);
      _returnSendTokenCode.text = "CodeId: " + data.ToString();
    }
    public void CosmosReturnSendTokenHash(string data)
    {
      Debug.Log("Debug unity ReturnSendToken: " + data);
      _returnSendTokenHash.text = "Tx hash: " + data;
    }
    public void CosmosReturnSdkQuery(string data)
    {
      Debug.Log("Debug unity CosmosReturnSdkQuery: " + data);
      _returnSdkQuery.text = data;
    }
  }
}
