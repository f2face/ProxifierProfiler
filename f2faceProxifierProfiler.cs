using System;
using System.Collections.Generic;
using System.Text;

namespace f2faceProxifierProfiler
{
    public class f2faceProxifierProfiler
    {
        private string output_data;
        private int _proxyCount;
        private ActionType defaultRuleActionType;
        private int defaultRuleProxyID;
        // List
        private List<string[]> ruleList;
        private List<string[]> proxyList;
        private List<object[]> chainList;
        // Parameters
        private bool _AutoModeDetection;
        private bool _ViaProxy;
        private bool _TryLocalDnsFirst;
        private string _ExclusionList;
        private EncryptionMode _Encryption;
        private bool _HttpProxiesSupport;
        private bool _HandleDirectConnections;
        private bool _ConnectionLoopDetection;
        private bool _ProcessServices;
        private bool _ProcessOtherUsers;
        // Enum
        public enum EncryptionMode { disabled, basic };
        public enum ProxyType { HTTP, HTTPS, SOCKS4, SOCKS5 };
        public enum ActionType { Direct, Block, Proxy };
        public enum ChainType { simple, redundancy, load_balancing };

        public int proxyCount
        {
            get { return _proxyCount; }
            set { _proxyCount = value; }
        }

        public bool AutoModeDetection
        {
            get { return _AutoModeDetection; }
            set { _AutoModeDetection = value; }
        }

        public bool ViaProxy
        {
            get { return _ViaProxy; }
            set { _ViaProxy = value; }
        }

        public bool TryLocalDnsFirst
        {
            get { return _TryLocalDnsFirst; }
            set { _TryLocalDnsFirst = value; }
        }

        public string ExclusionList
        {
            get { return _ExclusionList; }
            set { _ExclusionList = value; }
        }

        public EncryptionMode Encryption
        {
            get { return _Encryption; }
            set { _Encryption = value; }
        }

        public bool HttpProxiesSupport
        {
            get { return _HttpProxiesSupport; }
            set { _HttpProxiesSupport = value; }
        }

        public bool HandleDirectConnections
        {
            get { return _HandleDirectConnections; }
            set { _HandleDirectConnections = value; }
        }

        public bool ConnectionLoopDetection
        {
            get { return _ConnectionLoopDetection; }
            set { _ConnectionLoopDetection = value; }
        }

        public bool ProcessServices
        {
            get { return _ProcessServices; }
            set { _ProcessServices = value; }
        }

        public bool ProcessOtherUsers
        {
            get { return _ProcessOtherUsers; }
            set { _ProcessOtherUsers = value; }
        }

        public f2faceProxifierProfiler()
        {
            proxyCount = 0;
            AutoModeDetection = false;
            ViaProxy = false;
            TryLocalDnsFirst = false;
            ExclusionList = "%ComputerName%; localhost; *.local";
            Encryption = EncryptionMode.disabled;
            HttpProxiesSupport = true;
            HandleDirectConnections = false;
            ConnectionLoopDetection = true;
            ProcessServices = true;
            ProcessOtherUsers = true;
            defaultRuleActionType = ActionType.Direct;
            defaultRuleProxyID = 0;
            ruleList = new List<string[]>();
            proxyList = new List<string[]>();
            chainList = new List<object[]>();
            this.tambahRule(true, "Localhost", "localhost; 127.0.0.1; %ComputerName%;", null, null, ActionType.Direct);
        }

        public void tambahProxy(string label, string address, int port, ProxyType proxyType)
        {
            this.proxyCount += 1;
            this.proxyList.Add(new string[] {
                this.proxyCount.ToString(), // [0] ID
                proxyType.ToString(),       // [1] Proxy Type
                label,                      // [2] Label
                address,                    // [3] Proxy Address
                port.ToString()             // [4] Proxy Port
            });
        }

        public void tambahRule(bool enabled, string ruleName, string targetAddress, string Applications, string Ports, ActionType actionType, int proxyID = 0)
        {
            this.ruleList.Add(new string[]{
                enabled.ToString(),     // [0] Enabled?
                ruleName,               // [1] Name
                targetAddress,          // [2] Targets
                Applications,           // [3] Applications
                Ports,                  // [4] Ports
                actionType.ToString(),  // [5] Action type
                proxyID.ToString()      // [6] ID proxy
            });
        }

        public void tambahProxyChain(ChainType chainType, string chainName, List<KeyValuePair<int, bool>> proxyIDList, int RedundancyTimeout = 30, bool RedundancyTryDirect = false)
        {
            this.proxyCount += 1;
            List<KeyValuePair<int, bool>> proxyPairs = new List<KeyValuePair<int,bool>>();
            foreach (KeyValuePair<int, bool> proxyPair in proxyIDList)
            {
                proxyPairs.Add(new KeyValuePair<int, bool>(proxyPair.Key, proxyPair.Value));
            }
            chainList.Add(new object[]{
                this.proxyCount.ToString(), // [0] ID Chain
                chainType.ToString(),       // [1] Chain type
                chainName,                  // [2] Chain name
                proxyPairs,                 // [3] Proxies + status
                RedundancyTimeout,          // [4] RedundancyTimeout
                RedundancyTryDirect         // [5] RedundancyTryDirect
            });
        }

        private string profileOptions()
        {
            string str = "  <Options>" +
                     "\r\n    <Resolve>" +
                     "\r\n      <AutoModeDetection enabled=\"" + this.AutoModeDetection.ToString().ToLower() + "\" />" +
                     "\r\n      <ViaProxy enabled=\"" + this.ViaProxy.ToString().ToLower() + "\">" +
                     "\r\n        <TryLocalDnsFirst enabled=\"" + this.TryLocalDnsFirst.ToString().ToLower() + "\" />" +
                     "\r\n      </ViaProxy>" +
                     "\r\n      <ExclusionList>" + this.ExclusionList + "</ExclusionList>" +
                     "\r\n    </Resolve>" +
                     "\r\n    <Encryption mode=\"" + this.Encryption.ToString() + "\" />" +
                     "\r\n    <HttpProxiesSupport enabled=\"" + this.HttpProxiesSupport.ToString().ToLower() + "\" />" +
                     "\r\n    <HandleDirectConnections enabled=\"" + this.HandleDirectConnections.ToString().ToLower() + "\" />" +
                     "\r\n    <ConnectionLoopDetection enabled=\"" + this.ConnectionLoopDetection.ToString().ToLower() + "\" />" +
                     "\r\n    <ProcessServices enabled=\"" + this.ProcessServices.ToString().ToLower() + "\" />" +
                     "\r\n    <ProcessOtherUsers enabled=\"" + this.ProcessOtherUsers.ToString().ToLower() + "\" />" +
                     "\r\n  </Options>";
            return str;
        }

        private string profileProxyList()
        {
            string str = "  <ProxyList>";
            foreach (string[] proxyData in this.proxyList)
            {
                str += "\r\n    <Proxy id=\"" + proxyData[0] + "\" type=\"" + proxyData[1] + "\">" +
                       "\r\n      <Label>" + proxyData[2] + "</Label>" +
                       "\r\n      <Address>" + proxyData[3] + "</Address>" +
                       "\r\n      <Port>" + proxyData[4] + "</Port>" +
                       "\r\n      <Options>0</Options>" +
                       "\r\n    </Proxy>";
            }
            str += "\r\n  </ProxyList>";
            return str;
        }

        private string profileChain()
        {
            string str = "  <ChainList>";
            foreach (object[] chainData in chainList)
            {
                str += "\r\n    <Chain id=\"" + chainData[0].ToString() + "\" type=\"" + chainData[1].ToString() + "\">" +
                       "\r\n      <Name>" + chainData[2].ToString() + "</Name>";
                foreach (KeyValuePair<int, bool> proxyPair in chainData[3] as List<KeyValuePair<int, bool>>)
                {
                    str += "\r\n      <Proxy enabled=\"" + proxyPair.Value.ToString().ToLower() + "\">" + proxyPair.Key.ToString() + "</Proxy>";
                }
                if (chainData[1].ToString() == ChainType.redundancy.ToString())
                {
                    str += "\r\n      <RedundancyTimeout>" + chainData[4].ToString() + "</RedundancyTimeout>" +
                           "\r\n      <RedundancyTryDirect>" + chainData[5].ToString().ToLower() + "</RedundancyTryDirect>";
                }
                str += "\r\n    </Chain>";
            }
            str += "\r\n  </ChainList>";
            return str;
        }

        private string profileRuleList()
        {
            string str = "  <RuleList>";
            foreach (string[] rulesOpt in this.ruleList)
            {
                str += "\r\n    <Rule enabled=\"" + rulesOpt[0] + "\">" +
                       "\r\n      <Name>" + rulesOpt[1] + "</Name>";
                if (rulesOpt[2] != null)
                    str += "\r\n      <Targets>" + rulesOpt[2] + "</Targets>";
                if (rulesOpt[3] != null)
                    str += "\r\n      <Applications>" + rulesOpt[3] + "</Applications>";
                if (rulesOpt[4] != null)
                    str += "\r\n      <Ports>" + rulesOpt[4] + "</Ports>";
                if (rulesOpt[5] == "Proxy" && rulesOpt[5] != "0")
                    str += "\r\n      <Action type=\"Proxy\">" + rulesOpt[6] + "</Action>";
                else
                    str += "\r\n      <Action type=\"" + rulesOpt[5] + "\" />";
                str += "\r\n    </Rule>";
            }
            str += "\r\n  <RuleList>";
            return str;
        }

        public void setDefaultRule(ActionType action_type, int proxyID = 0)
        {
            defaultRuleActionType = action_type;
            defaultRuleProxyID = proxyID;
        }

        public int getProxyID(string proxyAddress, int proxyPort)
        {
            int proxyID = 0;
            foreach (string[] proxy in proxyList)
            {
                if (proxy[3] == proxyAddress && proxy[4] == proxyPort.ToString())
                    proxyID = int.Parse(proxy[0]);
            }
            return proxyID;
        }

        public int getProxyID(string proxyLabel)
        {
            int proxyID = 0;
            foreach (string[] proxy in proxyList)
            {
                if (proxy[2] == proxyLabel)
                    proxyID = int.Parse(proxy[0]);
            }
            return proxyID;
        }

        public string createProfile()
        {
            if (this.proxyList.Count > 0 && this.ruleList.Count > 1)
            {
                tambahRule(true, "Default", null, null, null, defaultRuleActionType, defaultRuleProxyID);
                this.output_data += "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";
                this.output_data += "\r\n<ProxifierProfile version=\"101\" platform=\"Windows\" product_id=\"0\" product_minver=\"310\">";
                this.output_data += "\r\n" + profileOptions();
                this.output_data += "\r\n" + profileProxyList();
                if (this.chainList.Count > 0)
                    this.output_data += "\r\n" + profileChain();
                this.output_data += "\r\n" + profileRuleList();
                this.output_data += "\r\n</ProxifierProfile>";
                return this.output_data;
            }
            else
            {
                throw new Exception("Proxy list kosong atau rules belum ditetapkan");
            }
        }
    }
}
