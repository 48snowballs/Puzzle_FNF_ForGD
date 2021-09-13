using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = ("Data/Manager/SongsManager"), fileName = "SongsManager")]
public class SongManager : SerializedScriptableObject
{
    public List<Level> songData;
    public List<SongClip> songAudio;
    public Level GetLevelData(int lv)
    {
        return songData.Find(x => x.level == lv);
    }
    public SongClip GetSongClip(string name)
    {
        return songAudio.Find(x => x.song == name);
    }
#if UNITY_EDITOR
    [FolderPath]
    public string pathData;
    [Button]
    public void GenerateSFX()
    {
        songData.Clear();

        string[] fileEntries = Directory.GetFiles(pathData);
        for (int i = 0; i < fileEntries.Length; i++)
        {
            if (fileEntries[i].EndsWith(".json"))
            {
                var text = File.ReadAllText(fileEntries[i].Replace("\\", "/"));
                var song = JsonConvert.DeserializeObject<LevelData>(text);
                songData.Add(song.level);
            }
        }
    } 
#endif
}
[Serializable]
public class SongClip
{
    public string song;
    public AudioClip clipSong;
}
[Serializable]
public class Level
{
    public int level;
    public string song;
    public int numOfNodes;
    public Node[] nodes;
    public float speed;
    public int avatar;
    public float timeStart;
    public int gold;
}
[Serializable]
public class LevelData
{
    public Level level;
}

[Serializable]
public class Node
{
    public int numOfNode;
    public List<int> arrows;
    public float time;
}

