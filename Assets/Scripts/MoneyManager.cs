using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public int currentMoney;
    public Text moneyText;
    // Start is called before the first frame update
    void Start()
    {
        if (!moneyText.gameObject.activeInHierarchy)
        {
            moneyText.gameObject.SetActive(true);
        }
    
        moneyText.text = currentMoney.ToString();
    }

    public void AddMoney(int moneyCollected)
    {
        currentMoney += moneyCollected;
        moneyText.text = currentMoney.ToString();        
    }
}
