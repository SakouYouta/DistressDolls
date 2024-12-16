using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class DataSaveManager
{
    private static string DeckFilePath = Application.streamingAssetsPath + "/SaveData.csv";                         // デッキのファイルパス
    private static string PossessionCardFilePath = Application.streamingAssetsPath + "/PossessionCard.csv"; // 所持カードのファイルパス

    #region SaveDeckList()  デッキリストデータを保存
    public static void SaveDeckList(List<int> data, string filePath = null)
    {
        filePath = filePath ?? DeckFilePath;
        try
        {
            // データをカンマ区切りの1行の文字列に変換
            string csvContent = string.Join(",", data);
            // ファイルに書き込む
            File.WriteAllText(filePath, csvContent);
            Debug.Log("データがエクセル形式（CSV）で保存されました: " + filePath);
        }
        catch (IOException ex)
        {
            Debug.LogError("エクセル保存エラー: " + ex.Message);
        }
    }
    #endregion

    #region SavePossessionCard()  所持カードデータを保存
    public static void SavePossessionCard(List<int> data, string filePath = null)
    {
        filePath = filePath ?? PossessionCardFilePath;
        try
        {
            // データをカンマ区切りの1行の文字列に変換
            string csvContent = string.Join(",", data);
            // ファイルに書き込む
            File.WriteAllText(filePath, csvContent);
            Debug.Log("データがエクセル形式（CSV）で保存されました: " + filePath);
        }
        catch (IOException ex)
        {
            Debug.LogError("エクセル保存エラー: " + ex.Message);
        }
    }
    #endregion

    #region LoadDeckList()  デッキデータの読み込み
    public static List<int> LoadDeckList(string filePath = null)
    {
        filePath = filePath ?? DeckFilePath;
        try
        {
            if (File.Exists(filePath))
            {
                string csvContent = File.ReadAllText(filePath);
                string[] stringArray = csvContent.Split(',');
                List<int> data = new List<int>();
                foreach (var str in stringArray)
                {
                    if (int.TryParse(str, out int value))
                    {
                        data.Add(value);
                    }
                }
                //Debug.Log("エクセル形式（CSV）からデータを読み込みました: " + string.Join(", ", data));
                return data;
            }
            else
            {
                Debug.LogWarning("指定されたファイルが見つかりません: " + filePath);
                return new List<int>();
            }
        }
        catch (IOException ex)
        {
            Debug.LogError("エクセル読み込みエラー: " + ex.Message);
            return new List<int>();
        }
    }
    #endregion

    #region LoadPossessionCard()  所持カードの読み込み
    public static List<int> LoadPossessionCard(string filePath = null)
    {
        filePath = filePath ?? PossessionCardFilePath;
        try
        {
            if (File.Exists(filePath))
            {
                string csvContent = File.ReadAllText(filePath);
                string[] stringArray = csvContent.Split(',');
                List<int> data = new List<int>();
                foreach (var str in stringArray)
                {
                    if (int.TryParse(str, out int value))
                    {
                        data.Add(value);
                    }
                }
                //Debug.Log("エクセル形式（CSV）からデータを読み込みました: " + string.Join(", ", data));
                return data;
            }
            else
            {
                Debug.LogWarning("指定されたファイルが見つかりません: " + filePath);
                return new List<int>();
            }
        }
        catch (IOException ex)
        {
            Debug.LogError("エクセル読み込みエラー: " + ex.Message);
            return new List<int>();
        }
    }
    #endregion
}