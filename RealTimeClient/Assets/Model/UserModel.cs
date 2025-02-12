using Assets.Scripts;
using Cysharp.Net.Http;
using Cysharp.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion.Client;
using Newtonsoft.Json;
using Shared.Interfaces.Services;
using System.IO;
using UnityEngine;

namespace Assets.Model
{
    internal class UserModel:BaseModel
    {
        private int userId; // 登録ユーザーID

        public static UserModel instance;

        public static UserModel Instance
        {
            get
            {
                if (instance == null)
                {// nullのとき
                 // GameObjectを生成して、NetworkManagerコンポーネントを追加
                    GameObject gameObj = new GameObject("UserModel");
                    instance = gameObj.AddComponent<UserModel>();

                    // オブジェクトをシーン遷移時に削除しないようにする
                    DontDestroyOnLoad(gameObj);
                }

                return instance;
            }
        }

        public int userID { get { return userId; }}

        public async UniTask<bool> RegistUserAsync()
        {
            var handler = new YetAnotherHttpHandler() { Http2Only = true};
            var channel = GrpcChannel.ForAddress(ServerURL, new GrpcChannelOptions() { HttpHandler = handler });
            var cliant = MagicOnionClient.Create<IUserService>(channel);
            try
            {// 登録成功
                SaveUserData();
                Debug.Log("成功");
                return true;
            }
            catch (RpcException e)
            {// 登録失敗
                Debug.Log(e);
                Debug.Log("失敗");
                return false;
            }
        }

        public void SaveUserData()
        {
            // ユーザー情報を保存する
            SaveData saveData = new SaveData();
            saveData.UserID = this.userId;
            string json = JsonConvert.SerializeObject(saveData);
            // StreamWriterクラスでファイルにjsonを保存
            // persistentDataPathはアプリの保存ファイルを置く場所。OS毎に変えてくれる。
            var writer = new StreamWriter(Application.persistentDataPath + "/saveData.json");
            writer.Write(json);
            writer.Flush();
            writer.Close();
        }

        public bool LoadUserData()
        {
            if (!File.Exists(Application.persistentDataPath + "/saveData.json"))
            {
                return false;
            }

            var reader = new StreamReader(Application.persistentDataPath + "/saveData.json");
            string json = reader.ReadToEnd();
            reader.Close();
            // ローカルファイル名からユーザー名とユーザーIDを取得
            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);
            this.userId = saveData.UserID;
            // 読み込んだかどうか
            return true;
        }
    }
}
