using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class StoryDataManager : MonoBehaviour
{
    private void Start()
    {
        ConvertJsonToSO();
    }

    public StoryDataSO storyDataSO;

    private void ConvertJsonToSO()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "StoryData.json");

        if (File.Exists(path))
        {
            string jesonText = File.ReadAllText(path);

            List<StoryData> importedStorys = JsonConvert.DeserializeObject<List<StoryData>>(jesonText);

            if (storyDataSO != null)
            {
               storyDataSO.storys = importedStorys;

# if UNITY_EDITOR
                EditorUtility.SetDirty(storyDataSO);
                AssetDatabase.SaveAssets();
#endif
                Debug.Log("JSON 변환 성공");
            }
            else
                Debug.Log("StorySO 미연결");
        }
    }
}
