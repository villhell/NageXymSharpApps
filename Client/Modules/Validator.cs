using CatSdk.Utils;
using NageXymSharpApps.Client.Models;
using NageXymSharpApps.Shared.Models;
using nagexymsharpweb.Models;
using Newtonsoft.Json;

namespace NageXymSharpApps.Client.Modules
{
    /// <summary>
    /// 入力値をチェックするクラス
    /// </summary>
    public class Validator
    {
        private readonly HttpClient _httpClient;
        private List<DataItem> _dataItems;
        private string _nodeUrl;
        public Validator(HttpClient httpClient, List<DataItem> dataItems, string nodeUrl)
        {
            _httpClient= httpClient;
            _dataItems= dataItems;
            _nodeUrl= nodeUrl;
        }

        /// <summary>
        /// ValidateItems
        /// </summary>
        /// <returns></returns>
        public async Task ValidateItems()
        {
            var mosaicDict = new Dictionary<string, MosaicNamespaceInfo>(); 
            
            foreach (DataItem item in _dataItems)
            {
                // CheckがOKならスキップ
                if (string.Equals(item.Check, "OK"))
                {
                    item.Valid = true;
                    continue;
                }

                // アドレスをチェック
                var result = await ValidAddress(item);

                if (result)
                {
                    // 受信制限ありかどうか 制限があればresultはfalse
                    var isRestriction = await ValidRestrictionsAccount(item);
                    result = isRestriction ? false : true;
                }
                else
                {
                    // ネームスペースチェック
                    result = await ValidNamespace(item);
                    if (result)
                    {
                        // 受信制限ありかどうか
                        var isRestriction = await ValidRestrictionsAccount(item);
                        result = isRestriction ? false : true;
                    }
                }

                // ここでresult=falseなら以降の処理は行わない
                if (!result) continue;

                // 以前にチェックしたMosaicIdやMosaicNamespaceならチェック不要
                if(!mosaicDict.TryGetValue(item.MosaicNamespace, out MosaicNamespaceInfo? mosaicNamespaceInfo))
                {
                    // モザイクネームスペースをチェック
                    result = await ValidMosaicNamespace(item);

                    if(result)
                    {
                        // モザイクの詳細情報を取得
                        var mosaicDetail = await GetMosaicDetail(item.MosaicNamespaceInfo.Namespace.Alias.MosaicId);
                        item.MosaicNamespaceInfo.MosaicInfo = mosaicDetail!;
                        mosaicDict.Add(item.MosaicNamespace, item.MosaicNamespaceInfo);
                    }
                }
                // 以前にモザイクの情報を取得していたらそれを使用する
                else
                {
                    item.MosaicNamespaceInfo = mosaicNamespaceInfo;
                }

                // 全てのValidateを通過
                if (result) item.Valid = true;
            }
        }

        /// <summary>
        /// Valid Address
        /// </summary>
        /// <param name="item"></param>
        public async Task<bool> ValidAddress(DataItem item)
        {
            // アドレス欄が空白
            if (string.IsNullOrEmpty(item.Address))
            {
                item.Valid = false;
                return false;
            }

            // アドレス情報取得
            var account = await GetAddress(item.Address);

            if(account != null)
            {
                item.Valid= true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valid Namespace
        /// </summary>
        /// <param name="item"></param>
        /// <param name="namespaceString"></param>
        /// <returns></returns>
        public async Task<bool> ValidNamespace(DataItem item)
        {
            // アドレスをNamespaceIdに変換
            var namespaceId = Utils.StringToNamespaceId(item.Address);
            // ネームスペース情報取得
            var namespaceInfo = await GetNamespace(namespaceId);

            if(namespaceInfo != null)
            {
                item.AddressNamespace = item.Address;
                var address = namespaceInfo.Alias.Address ?? namespaceInfo.OwnerAddress;
                item.Address = Converter.AddressToString(Converter.HexToBytes(address));
                item.Valid = true;
                return true;
            }
            return false;
        }

        public async Task<bool> ValidRestrictionsAccount(DataItem item)
        {
            return await GetRestrictionsAccount(item.Address);
        }

        public async Task<bool> ValidMosaicNamespace(DataItem item)
        {          
            // モザイクをNamespaceIdに変換
            var namespaceId = Utils.StringToNamespaceId(item.MosaicNamespace);
            var namespaceInfo = await GetMosaicNamespace(namespaceId);

            if (namespaceInfo != null)
            {
                item.MosaicNamespaceInfo = new MosaicNamespaceInfo();
                item.MosaicNamespaceInfo.Namespace = namespaceInfo;
                return true;
            }
            return false;
        }
        #region アドレスを取得
        /// <summary>
        /// アドレスを取得
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public async Task<Account?> GetAddress(string address)
        {
            Account? account = null;
            var accountResponse = await _httpClient.GetAsync($"/api/accounts/{address}?node_url={_nodeUrl}");

            // Status OK
            if (accountResponse.IsSuccessStatusCode)
            {
                var accountContent = await accountResponse.Content.ReadAsStringAsync();
                var accountInfo = JsonConvert.DeserializeObject<AccountResponse>(accountContent);

                Console.WriteLine(accountInfo);
                if (accountInfo != null)
                {
                    account = accountInfo.Account;
                }
            }

            return account;
        }
        #endregion

        #region ネームスペース情報取得
        /// <summary>
        /// ネームスペース情報取得
        /// </summary>
        /// <param name="namespaceId"></param>
        /// <returns></returns>
        public async Task<Namespace?> GetNamespace(string namespaceId)
        {
            Namespace? namespaceInfo = null; 
            var addresNamespaceResult = await _httpClient.GetAsync($"/api/namespaces/{namespaceId}?node_url={_nodeUrl}");
            if (addresNamespaceResult.IsSuccessStatusCode)
            {
                var addressNamespaceContent = await addresNamespaceResult.Content.ReadAsStringAsync();
                var addressNamespaceInfo = JsonConvert.DeserializeObject<NamespaceResponse>(addressNamespaceContent);

                if (addressNamespaceInfo != null)
                {
                    // ネームスペース情報取得成功
                    namespaceInfo = addressNamespaceInfo.Namespace;
                }
            }
            return namespaceInfo;
        }
        #endregion

        #region 受信制限があるか
        /// <summary>
        /// 受信制限があるか
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public async Task<bool> GetRestrictionsAccount(string address)
        {
            // 受信制限があるか
            var restrictionsAccountResult = await _httpClient.GetAsync(string.Format("/api/restrictionsaccount?address={0}&node_url={1}", address, _nodeUrl));
            if (restrictionsAccountResult.IsSuccessStatusCode)
            {
                var restrictionsContent = await restrictionsAccountResult.Content.ReadAsStringAsync();
                var restrictionsAccount = JsonConvert.DeserializeObject<RestrictionsAccountResponse>(restrictionsContent);

                Console.WriteLine(restrictionsContent);

                // 受信制限情報が無し
                return restrictionsAccount!.Data.Count == 0 ? false : true;
            }

            return true;
        }
        #endregion

        #region モザイクのネームスペース情報を取得
        /// <summary>
        /// モザイクのネームスペース情報を取得
        /// </summary>
        /// <param name="namespaceId"></param>
        /// <param name="mosaicNamespaceInfo"></param>
        /// <returns></returns>
        public async Task<Namespace?> GetMosaicNamespace(string namespaceId)
        {
            Namespace? mosaicNamespace = null;
            var mosaicNamespaceResult = await _httpClient.GetAsync($"/api/namespaces/{namespaceId}?node_url={_nodeUrl}");
            if (mosaicNamespaceResult.IsSuccessStatusCode)
            {
                var mosaicNamespaceContent = await mosaicNamespaceResult.Content.ReadAsStringAsync();
                var mosaicNamespaceResponse = JsonConvert.DeserializeObject<NamespaceResponse>(mosaicNamespaceContent);

                // ネームスペース解決
                if (mosaicNamespaceResponse!.Namespace.Alias.MosaicId != null)
                {
                    mosaicNamespace = mosaicNamespaceResponse.Namespace;
                }
            }
            return mosaicNamespace;
        }
        #endregion

        #region モザイクの詳細情報を取得
        /// <summary>
        /// モザイクの詳細情報を取得
        /// </summary>
        /// <param name="mosaicId"></param>
        /// <param name="mosaicNamespaceInfo"></param>
        /// <returns></returns>
        public async Task<MosaicInfo?> GetMosaicDetail(string mosaicId)
        {
            MosaicInfo? mosaicInfo = null;

            // モザイクIDの情報を取得
            var mosaicResult = await _httpClient.GetAsync($"/api/mosaics/{mosaicId}?node_url={_nodeUrl}");
            if (mosaicResult.IsSuccessStatusCode)
            {
                var mosaicContent = await mosaicResult.Content.ReadAsStringAsync();
                mosaicInfo = JsonConvert.DeserializeObject<MosaicInfo>(mosaicContent);
            }
            return mosaicInfo;
        }
        #endregion
    }
}
