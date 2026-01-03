using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveStats : MonoBehaviour
{
    public CharacterStats characterStats;
    public int savedExp;
    public int savedLevel;

    // Start is called before the first frame update
    void Start()
    {
        characterStats = FindObjectOfType<CharacterStats>().GetComponent<CharacterStats>();
        savedExp = characterStats.exp;
        savedLevel = characterStats.level;
    }

    
    
}
