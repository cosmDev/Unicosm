mergeInto(LibraryManager.library, {
  // Broadcast part
  CosmosBroadcast: function (chainId, chainRpc, chainGas, chaindenom, addressTo, amount, signer, experimentalSuggestChain) {
    try {
      var returnCosmos = exportCosmosConfig.sendToken(
        UTF8ToString(chainId),
        UTF8ToString(chainRpc), 
        UTF8ToString(chainGas), 
        UTF8ToString(chaindenom),
        UTF8ToString(addressTo), 
        UTF8ToString(amount), 
        UTF8ToString(signer),
        UTF8ToString(experimentalSuggestChain)
      )      
      returnCosmos.then(function (result) {
        console.log("returnSendTx", result)
        UnitosInstance.SendMessage('CosmosReturnData', 'CosmosReturnSendTokenCode', result.code);   
        UnitosInstance.SendMessage('CosmosReturnData', 'CosmosReturnSendTokenHash', result.transactionHash);       
      })
    } catch(error) {
      console.log(error)
    }     
  },
  CosmosConnectCall: function (chainId, signer, experimentalSuggestChain) {  
    try {
      var isConnected = exportCosmosConfig.signerConnect(UTF8ToString(chainId), UTF8ToString(signer), UTF8ToString(experimentalSuggestChain))
      isConnected.then(function (result) { 
        UnitosInstance.SendMessage("CosmosReturnData", 'CosmosReturnAddress', result[0].address);        
      })
    } catch(error) {
      console.log(error) 
    }   
  },  
  CosmosSignArbitraryCall: function (chainId, message, signer, experimentalSuggestChain) {    
    try {
      var returnCosmos = exportCosmosConfig.signArbitrary(UTF8ToString(chainId), UTF8ToString(message), UTF8ToString(signer), UTF8ToString(experimentalSuggestChain))
      returnCosmos.then(function (result) {
        UnitosInstance.SendMessage("CosmosReturnData", 'CosmosReturnSignArbitrary', JSON.stringify(result.signArbitrary)); 
        UnitosInstance.SendMessage("CosmosReturnData", 'CosmosReturnAddress', result.address);       
      })
    } catch(error) {
      console.log("catch error: ", error)
      alert(error)
    }     
  },  
  CosmosCosmWasmExecuteCall: function (chainId, rpcURL, gasPrice, chainDenom, contractAddr, execNameJson, execVariablesJson, signer, experimentalSuggestChain) {    
    try {
      var returnCosmWasm = exportCosmosConfig.wasmExecute(
        UTF8ToString(chainId), 
        UTF8ToString(rpcURL), 
        UTF8ToString(gasPrice), 
        UTF8ToString(chainDenom), 
        UTF8ToString(contractAddr), 
        UTF8ToString(execNameJson), 
        UTF8ToString(execVariablesJson),
        UTF8ToString(signer), 
        UTF8ToString(experimentalSuggestChain)
      )
      returnCosmWasm.then(function (result) {
        UnitosInstance.SendMessage("CosmosReturnData", 'CosmosReturnWasm', result.transactionHash);       
      })
    } catch(error) {
      console.log("catch error: ", error) 
    }      
  },
  CosmosCosmWasmQueryCall: function (rpcURL, contractAddr, queryName, queryData) {
    try {
      var returnCosmWasm = exportCosmosConfig.wasmQuery(
        UTF8ToString(rpcURL), 
        UTF8ToString(contractAddr), 
        UTF8ToString(queryName),
        UTF8ToString(queryData)          
      )
      returnCosmWasm.then(function (result) {
        UnitosInstance.SendMessage("CosmosReturnData", 'CosmosReturnWasmQuery', JSON.stringify(result));       
      })
    } catch(error) {
      console.log("catch error: ", error)
      alert(error)
    } 
  },
  // Query part (On dev)
  CosmosQueryAccount: function (chainRpc, chainDenom, chainExponent, returnAddress, address) {
    console.log('Debug from unity js chainRpc', UTF8ToString(chainRpc)) 
    console.log('Debug from unity js chainDenom', UTF8ToString(chainDenom)) 
    console.log('Debug from unity js chainExponent', UTF8ToString(chainExponent)) 
    console.log('Debug from unity js address', UTF8ToString(address)) 
    try {
      var returnCosmos = exportCosmosConfig.queryAccount(
        UTF8ToString(chainRpc), 
        UTF8ToString(chainDenom), 
        UTF8ToString(chainExponent), 
        UTF8ToString(returnAddress), 
        UTF8ToString(address)
      )
      returnCosmos.then(function (result) {
        UnitosInstance.SendMessage('CosmosReturnData', 'CosmosReturnAccountData', result.toString());     
      })
    } catch(error) {
      console.log(error)
      alert(error)
    }  
  },
});
