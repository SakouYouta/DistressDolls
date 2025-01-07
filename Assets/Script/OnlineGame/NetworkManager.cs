using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    public static NetworkManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public async Task ConnectToServer(string ip, int port)
    {
        try
        {
            client = new TcpClient();
            await client.ConnectAsync(ip, port); // サーバーに接続
            stream = client.GetStream();
            Debug.Log("サーバーに接続しました！");
        }
        catch (Exception e)
        {
            Debug.LogError($"サーバー接続エラー: {e.Message}");
        }
    }

    public async Task SendMessageToServer(string message)
    {
        if (stream == null) return;

        byte[] data = Encoding.UTF8.GetBytes(message);
        await stream.WriteAsync(data, 0, data.Length);
        Debug.Log($"サーバーに送信: {message}");
    }

    public async Task<string> ReceiveMessageFromServer()
    {
        if (stream == null) return null;

        byte[] buffer = new byte[1024];
        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
        string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        Debug.Log($"サーバーから受信: {receivedMessage}");
        return receivedMessage;
    }

    private void OnApplicationQuit()
    {
        stream?.Close();
        client?.Close();
    }
}
