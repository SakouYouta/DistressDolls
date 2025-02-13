using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class DataSaveManager
{
    private static string DataFilePath = Application.streamingAssetsPath + "/SaveData.csv";                         // デッキのファイルパス
    private static int DeckIndex = 0;
    private static int PossessinoIndex = 1;

    #region カードデータ関連
    #region SaveDeckList()  デッキリストデータを保存
    public static void SaveDeckList(List<int> data)
    {
        try
        {
            List<string> lines = new List<string>();


            // 既存ファイルを読み込む（なければ新規作成）
            if (File.Exists(DataFilePath))
            {
                lines.AddRange(File.ReadAllLines(DataFilePath));
            }

            // 必要な行数まで拡張
            while (lines.Count <= DeckIndex)
            {
                lines.Add(""); // 空行を追加
            }

            lines[DeckIndex] = string.Join(",", data);
            File.WriteAllLines(DataFilePath, lines);

            Debug.Log($"データを {DataFilePath} の {DeckIndex} 行目に保存しました: {string.Join(", ", data)}");
        }
        catch (IOException ex)
        {
            Debug.LogError($"CSV保存エラー ({DataFilePath}): " + ex.Message);
        }
    }
    #endregion

    #region SavePossessionCard()  所持カードデータを保存
    public static void SavePossessionCard(List<int> data)
    {
        try
        {
            List<string> lines = new List<string>();
            // 既存ファイルを読み込む（なければ新規作成）
            if (File.Exists(DataFilePath))
            {
                lines.AddRange(File.ReadAllLines(DataFilePath));
            }

            // 必要な行数まで拡張
            while (lines.Count <= PossessinoIndex)
            {
                lines.Add(""); // 空行を追加
            }

            lines[PossessinoIndex] = string.Join(",", data);
            File.WriteAllLines(DataFilePath, lines);

            Debug.Log($"データを {DataFilePath} の {PossessinoIndex} 行目に保存しました: {string.Join(", ", data)}");
        }
        catch (IOException ex)
        {
            Debug.LogError($"CSV保存エラー ({DataFilePath}): " + ex.Message);
        }
    }
    #endregion

    #region LoadDeckList()  デッキデータの読み込み
    public static List<int> LoadDeckList()
    {
        try
        {
            if (!File.Exists(DataFilePath))
            {
                Debug.LogWarning($"指定されたファイルが見つかりません: {DataFilePath}");
                return new List<int>();
            }

            string[] lines = File.ReadAllLines(DataFilePath);

            // 指定行が範囲外なら空リストを返す
            if (DeckIndex >= lines.Length)
            {
                Debug.LogWarning($"指定行 ({DeckIndex}) はファイルの範囲外です: {DataFilePath}");
                return new List<int>();
            }

            // 行のデータをパース
            string[] stringArray = lines[DeckIndex].Split(',');
            List<int> data = new List<int>();
            foreach (var str in stringArray)
            {
                if (int.TryParse(str, out int value))
                {
                    data.Add(value);
                }
            }

            return data;
        }
        catch (IOException ex)
        {
            Debug.LogError($"CSV読み込みエラー ({DataFilePath}): " + ex.Message);
            return new List<int>();
        }
    }
    #endregion

    #region LoadPossessionCard()  所持カードの読み込み
    public static List<int> LoadPossessionCard()
    {
        try
        {
            if (!File.Exists(DataFilePath))
            {
                Debug.LogWarning($"指定されたファイルが見つかりません: {DataFilePath}");
                return new List<int>();
            }

            string[] lines = File.ReadAllLines(DataFilePath);

            // 指定行が範囲外なら空リストを返す
            if (PossessinoIndex >= lines.Length)
            {
                Debug.LogWarning($"指定行 ({PossessinoIndex}) はファイルの範囲外です: {DataFilePath}");
                return new List<int>();
            }

            // 行のデータをパース
            string[] stringArray = lines[PossessinoIndex].Split(',');
            List<int> data = new List<int>();
            foreach (var str in stringArray)
            {
                if (int.TryParse(str, out int value))
                {
                    data.Add(value);
                }
            }

            return data;
        }
        catch (IOException ex)
        {
            Debug.LogError($"CSV読み込みエラー ({DataFilePath}): " + ex.Message);
            return new List<int>();
        }
    }
    #endregion
    #endregion

    #region ソウルデータ関連
    #region SaveSoul()-ソウルを保存する
    public static void SaveSoul(int soul)
    {
        PlayerPrefs.SetInt("Soul", soul);
        PlayerPrefs.Save();
    }
    #endregion

    #region GetSoul()-ソウルを取得
    public static int GetSoul()
    {
        return PlayerPrefs.GetInt("Soul", 0);
    }
    #endregion

    #region AddSoul()-ソウルを増やす
    public static void AddSoul(int soul)
    {
        int AllSoul = GetSoul();
        Debug.Log(AllSoul);
        AllSoul += soul;
        SaveSoul(AllSoul);
    }
    #endregion

    #region SubtractionSoul()-ソウルを減らす
    public static bool SubtractionSoul(int soul)
    {
        int AllSoul = GetSoul();
        if (AllSoul >= soul)
        {
            AllSoul -= soul;
            SaveSoul(AllSoul);
            return true;
        }
        return false;
    }
    #endregion
    
    #endregion
}