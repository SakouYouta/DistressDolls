using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class OnlineMatchingManager : MonoBehaviourPunCallbacks
{
    // 部屋に入っているかどうかのフラグ
    bool isEnterRoom = false;
    // マッチング済みかどうかのフラグ
    bool isMatching = false;
    // ゲームが開始可能かどうかのフラグ
    bool isGameReady = false;

    public void OnMatchingButton()
    {
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    #region OnConnectedToMaster() - マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // ランダムマッチングを試みる
        PhotonNetwork.JoinRandomRoom();
    }
    #endregion

    #region OnJoinedRoom() - 部屋への入室に成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        // 部屋への参加フラグを設定
        isEnterRoom = true;

        // 部屋の情報を表示（デバッグ用）
        Debug.Log($"部屋に参加しました: {PhotonNetwork.CurrentRoom.Name}, プレイヤー数: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}");
    }
    #endregion

    #region OnJoinRandomFailed() - ランダムマッチングに失敗した時に呼ばれるコールバック
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 新しい部屋を作成する（最大2人の部屋）
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
        Debug.Log("新しい部屋を作成しました");
    }
    #endregion

    // 他のプレイヤーが部屋に参加した時に呼ばれるコールバック
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"プレイヤーが参加しました: {newPlayer.NickName}, 現在のプレイヤー数: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}");

        // プレイヤーが2人揃った場合にゲーム開始の準備を設定
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            isGameReady = true; // ゲーム開始準備フラグをオンにする
            Debug.Log("プレイヤーが揃いました！ゲームの準備ができました。");
        }
    }

    // 毎フレーム呼び出されるメソッド
    private void Update()
    {
        // マッチング済みであれば処理をスキップ
        if (isMatching) return;

        // 部屋に入室済みかつゲーム開始準備が整っている場合
        if (isEnterRoom && isGameReady)
        {
            isMatching = true; // マッチングフラグを設定
            Debug.Log("マッチング成功！ゲームを開始します。");
            OnlineGameManager.instance.StartGame(); // ゲーム開始処理
        }
    }
}
