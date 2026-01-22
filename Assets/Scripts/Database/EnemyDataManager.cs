using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEditor;
public class EnemyDataManager : MonoBehaviour
{
    void Start()
    {
        ConvertJsonToSO();
    }

    public EnmeySO enemySO;

   void ConvertJsonToSO()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "EnemyData.Json");

        if (File.Exists(path))
        {
            string jsonText = File.ReadAllText(path);

            List<EnemyData> importedEnemy = JsonConvert.DeserializeObject<List<EnemyData>>(jsonText);

            if(enemySO != null)
            {
                enemySO.Enemys = importedEnemy;

                //EditorUtility.SetDirty(enemySO);
                //AssetDatabase.SaveAssets();

                Debug.Log("JSON 변환 성공");
            }
            else
                Debug.Log("EnemySO 미연결");
        }
    }


}
