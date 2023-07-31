using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private GameObject lifeText;
    private GameObject coinText;

    void Start()
    {
        this.coinText = this.transform.Find("CoinText").gameObject;
        this.lifeText = this.transform.Find("LifeText").gameObject;
    }

    void Update()
    {
        lifeText.GetComponent<TextMeshProUGUI>().text = $"{GameManager.Instance.GetLife()}";
        coinText.GetComponent<TextMeshProUGUI>().text = $"{GameManager.Instance.GetCoins()}";
    }
}
