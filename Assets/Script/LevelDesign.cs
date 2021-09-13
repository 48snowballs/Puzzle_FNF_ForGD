using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.IO;

[CreateAssetMenu(menuName = ("Data/Manager/LevelDesign"), fileName = "LevelDesign")]
public class LevelDesign : SerializedScriptableObject
{
    public enum SONG
    {
        Bopeebo,
        Fresh
    }
    public enum AVATAR
    {
        Pico,
        Man,
        Woman
    }
    [SerializeField] private int level;
    [SerializeField] private SONG song;
    [SerializeField] private int numOfNodes;
    [SerializeField] private float speed;
    [SerializeField] private AVATAR avatar;
    [SerializeField] private float timeStart;
    [SerializeField] private int gold;
    [SerializeField] private List<Node> nodes;
    [Button]
    public void GenerateNode(int numOfNode,float time)
    {
        Node node = new Node();
        node.numOfNode = numOfNode;
        node.time = time;
        List<int> arr = new List<int>();
        for (int i = 0; i < numOfNode; i++)
        {
            int type = Random.Range(0, 4);
            arr.Add(type);
        }
        node.arrows = arr;
        nodes.Add(node);
        numOfNodes++;
    }

    [FolderPath]
    public string pathData;
    [Button]
    public void ExportToJson()
    {
        Level lv = new Level();
        lv.level = level;
        lv.song = song.ToString();
        lv.numOfNodes = numOfNodes;
        lv.nodes = nodes.ToArray();
        lv.speed = speed;
        if (avatar.Equals(AVATAR.Pico)) lv.avatar = 2;
        else if (avatar.Equals(AVATAR.Man)) lv.avatar = 3;
        else if (avatar.Equals(AVATAR.Woman)) lv.avatar = 4;
        lv.timeStart = timeStart;
        lv.gold = gold;

        LevelData levelData = new LevelData();
        levelData.level = lv;

        string js = JsonConvert.SerializeObject(levelData);
        File.WriteAllText(pathData + "/level_" + level + ".json", js);
    }

}
