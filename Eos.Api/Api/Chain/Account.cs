using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eos.Api.Chain
{
    public class Account
    {
        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("head_block_num")]
        public uint HeadBlockNum { get; set; }

        [JsonProperty("head_block_time")]
        public DateTime HeadBlockTime { get; set; }

        [JsonProperty("privileged")]
        public bool Privileged { get; set; }

        [JsonProperty("last_code_update")]
        public DateTime LastCodeUpdate { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("core_liquid_balance")]
        public string CoreLiquidBalance { get; set; }

        [JsonProperty("ram_quota")]
        public ulong RamQuota { get; set; }

        [JsonProperty("net_weight")]
        public ulong NetWeight { get; set; }

        [JsonProperty("cpu_weight")]
        public ulong CpuWeight { get; set; }

        [JsonProperty("net_limit")]
        public ResourceLimit NetLimit { get; set; }

        [JsonProperty("cpu_limit")]
        public ResourceLimit CpuLimit { get; set; }

        [JsonProperty("ram_usage")]
        public ulong RamUssage { get; set; }

        [JsonProperty("permissions")]
        public IEnumerable<Permission> Permissions { get; set; }

        [JsonProperty("total_resources")]
        public TotalResources TotalResources { get; set; }

        [JsonProperty("self_delegated_bandwidth")]
        public SelfDelegatedBandwidth SelfDelegatedBandwidth { get; set; }

        [JsonProperty("refund_request")]
        public RefundRequest RefundRequest { get; set; }

        [JsonProperty("voter_info")]
        public VoterInfo VoterInfo { get; set; }
    }

    public class ResourceLimit
    {
        [JsonProperty("used")]
        public ulong Used { get; set; }

        [JsonProperty("available")]
        public ulong Available { get; set; }

        [JsonProperty("max")]
        public ulong Max { get; set; }
    }

    public class Permission
    {
        [JsonProperty("perm_name")]
        public string Name { get; set; }

        [JsonProperty("parent")]
        public string Parent { get; set; }

        [JsonProperty("required_auth")]
        public Authority RequiredAuth { get; set; }
    }

    public class Authority
    {
        [JsonProperty("threshold")]
        public uint Threshold { get; set; }

        [JsonProperty("keys")]
        public List<AuthorityKey> Keys { get; set; }

        [JsonProperty("accounts")]
        public List<string> Accounts { get; set; }

        [JsonProperty("waits")]
        public List<AuthorityWait> Waits { get; set; }
    }

    public class AuthorityKey
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("weight")]
        public uint Weight { get; set; }
    }

    public class AuthorityAccount
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("weight")]
        public uint Weight { get; set; }
    }

    public class AuthorityWait
    {
        [JsonProperty("wait_sec")]
        public string WaitSec { get; set; }

        [JsonProperty("weight")]
        public uint Weight { get; set; }
    }

    public class TotalResources
    {
        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("net_weight")]
        public string NetWeight { get; set; }

        [JsonProperty("cpu_weight")]
        public string CpuWeight { get; set; }

        [JsonProperty("ram_bytes")]
        public ulong RamBytes { get; set; }
    }

    public class SelfDelegatedBandwidth
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("net_weight")]
        public string NetWeight { get; set; }

        [JsonProperty("cpu_weight")]
        public string CpuWeight { get; set; }
    }

    public class RefundRequest
    {
        [JsonProperty("cpu_amount")]
        public string CpuAmount { get; set; }

        [JsonProperty("net_amount")]
        public string NetAmount { get; set; }
    }

    public class VoterInfo
    {
        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("proxy")]
        public string Proxy { get; set; }

        [JsonProperty("producers")]
        public List<string> Producers { get; set; }

        [JsonProperty("staked")]
        public ulong Staked { get; set; }

        [JsonProperty("last_vote_weight")]
        public double LastVoteWeight { get; set; }

        [JsonProperty("proxied_vote_weight")]
        public double ProxiedVoteWeight { get; set; }

        [JsonProperty("is_proxy")]
        public bool IsProxy { get; set; }
    }
}
