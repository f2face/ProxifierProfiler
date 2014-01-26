ProxifierProfiler
=================

.NET class library to create on-the-fly Proxifier profile.

Class library .NET untuk membuat profile Proxifier secara on-the-fly (sesaat sebelum dijalankan).

-----------------------------------------
<h4>Construct :</h4>
<b>f2faceProxifierProfiler()</b> : initialize f2faceProxifierProfiler

<h4>Properties :</h4>
bool <b>AutoModeDetection</b> : Detect DNS settings automatically. (default: false)<br/>
bool <b>ViaProxy</b> : Resolve hostnames through proxy. (default: true)<br/>
bool <b>TryLocalDnsFirst</b> : try to resolve via local DNS service first (this option may cause significant delay if local DNS is bool unavailable!). (default: false)<br/>
string <b>ExclusionList</b> : Hostnames which are not resolved through proxy. (default: %ComputerName%; localhost; *.local)<br/>
EncryptionMode <b>Encryption</b> : Additional security modes (not necessary, though). (default: disabled)<br/>
bool <b>HttpProxiesSupport</b> : Add HTTP proxy servers support. (default: true)<br/>
bool <b>HandleDirectConnections</b> : Handle direct connection. (default: false)<br/>
bool <b>ConnectionLoopDetection</b> : Infinite connection loop detection to prevent stability problem. (default: true)<br/>
bool <b>ProcessServices</b> : Handle connection of application run by other users. (default: true)<br/>
bool <b>ProcessOtherUsers</b> : Handle connection of windows services and other system processes. (default: true)

<h4>Methods :</h4>
int <b>tambahProxy(string label, string address, int port, ProxyType proxyType)</b> : Add a proxy to Proxifier proxy list and return the proxy id.<br/><br/>
void <b>tambahRule(bool enabled, string ruleName, string targetAddress, string Applications, string Ports, ActionType actionType, int proxyID = 0)</b> : Add a proxification rule.<br/><br/>
int <b>tambahProxyChain(ChainType chainType, string chainName, List&lt;KeyValuePair&lt;int, bool&gt;&gt; proxyIDList, int RedundancyTimeout = 30, bool RedundancyTryDirect = false)</b> : Add a proxy chain to Proxifier proxy chain list and return the chain id.<br/><br/>
void <b>setDefaultRule(ActionType action_type, int proxyID = 0)</b> : Set proxy for default proxification rule. (default ActionType: Direct)<br/><br/>
int <b>getProxyID(string proxyAddress, int proxyPort)</b> : Get and return proxy id based on specified proxy address and proxy port.<br/><br/>
int <b>getProxyID(string proxyLabel)</b> : Get and return proxy id based on specified proxy label.<br/><br/>
string <b>createProfile()</b> : Create Proxifier profile string. This method should be called in the end, after proxies, chain, and rules have been set.
