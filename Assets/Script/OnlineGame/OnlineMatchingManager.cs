using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class OnlineMatchingManager : MonoBehaviourPunCallbacks
{    
    bool isEnterRoom; // 部屋に入ってるかどうかのフラグ
    bool isMatching; // マッチング済みかどうかのフラグ

    public void OnMatchingButton()
    {
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // ランダムマッチング
        PhotonNetwork.JoinRandomRoom();
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        isEnterRoom = true;
    }

    // 失敗した場合
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
    }

    // もし二人ならシーンを変える
    private void Update()
    {
        if (isMatching) return;

        if (isEnterRoom)
        {
            if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                isMatching = true;
                Debug.Log("マッチング成功");
                GameManager.instance.StartGame();
            }
        }
    }
}