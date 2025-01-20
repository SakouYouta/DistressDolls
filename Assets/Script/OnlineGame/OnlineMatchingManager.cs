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
        Debug.Log("マスターサーバーに接続中...");
    }

    #region OnConnectedToMaster() - マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // ランダムマッチングを試みる
        Debug.Log("マスターサーバーへの接続に成功しました。ランダムマッチングを開始します。");
        PhotonNetwork.JoinRandomRoom();
    }
    #endregion

    #region OnJoinedRoom() - 部屋への入室に成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        // 部屋への参加フラグを設定
        isEnterRoom = true;
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
        // デバッグログでフラグ状態を確認
        //Debug.Log($"Update Check - isEnterRoom: {isEnterRoom}, isGameReady: {isGameReady}, isMatching: {isMatching}");

        // 部屋に参加しており、ゲームの準備ができている場合にのみ処理を実行
        if (isEnterRoom && isGameReady && !isMatching)
        {
            // マッチング完了フラグを設定
            isMatching = true;
            Debug.Log("マッチング成功！ゲームを開始します。");

            // `photonView` のチェックと RPC の実行
            if (photonView != null)
            {
                photonView.RPC("StartOnlineGame", RpcTarget.All);
            }
            else
            {
                Debug.LogError("photonView が null です。スクリプトが正しくアタッチされているか確認してください。");
            }
        }
    }


    [PunRPC]
    private void StartOnlineGame()
    {
        // `OnlineGameManager.instance` の `null` チェック
        if (OnlineGameManager.instance != null)
        {
            OnlineGameManager.instance.StartGame(); // ゲームを開始
        }
        else
        {
            Debug.LogError("OnlineGameManager.instance が null です。シングルトンが正しく設定されているか確認してください。");
        }
    }
}
