using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JsonSaveManager
{
    private static string filePath = Application.persistentDataPath + "/SavedData.json";

    // デッキリストデータを保存
    public static void SaveDeckList(List<int> deck)
    {
        string json = JsonUtility.ToJson(new CardIdListWrapper(deck), true); // JSON文字列に変換
        File.WriteAllText(filePath, json); // ファイルに書き込む
    }

    // デッキデータの読み込み
    public static List<int> LoadDeckList()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath); // ファイルから読み込む
            CardIdListWrapper data = JsonUtility.FromJson<CardIdListWrapper>(json);
            Debug.Log("取得したリストデータ：" + string.Join(", ", data.cardIds));
            return data.cardIds;
        }
        else
        {
            Debug.LogWarning("セーブデータは空です");
            return new List<int>();
        }
    }

    // JSONに変換可能なラッパークラス
    [System.Serializable]
    private class CardIdListWrapper
    {
        public List<int> cardIds;

        public CardIdListWrapper(List<int> ids)
        {
            cardIds = ids;
        }
    }
}
