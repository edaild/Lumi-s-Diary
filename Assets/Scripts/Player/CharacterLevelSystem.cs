using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLevelSystem : MonoBehaviour
{
    public QuestSystem _questSystem;
    public CharacterHealthSystem _characterHealthSystem;

    public int MaxplayerExperience;


    private void Start()
    {
        _questSystem = UnityEngine.Object.FindObjectOfType<QuestSystem>();
    }

    public void UpdateLevel()
    {
        _characterHealthSystem = UnityEngine.Object.FindObjectOfType<CharacterHealthSystem>();
        if (!_characterHealthSystem || !_questSystem || _questSystem.playerExperience != MaxplayerExperience) return;

        MaxplayerExperience += 100;
        _questSystem.playerLevel += 1;
        _characterHealthSystem.character_Health += 100;

        _questSystem.playerExperience = 0;
    }
}
