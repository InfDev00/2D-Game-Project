using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private TextMeshProUGUI lifeText;
    private TextMeshProUGUI coinText;
    private Slider HPSlider;

    void Start()
    {
        this.coinText = this.transform.Find("CoinText").gameObject.GetComponent<TextMeshProUGUI>();
        this.lifeText = this.transform.Find("LifeText").gameObject.GetComponent<TextMeshProUGUI>();
        this.HPSlider = this.transform.Find("HPSlider").gameObject.GetComponent<Slider>();
    }

    void Update()
    {
        lifeText.text = $"{GameManager.Instance.GetLife()}";
        coinText.text = $"{GameManager.Instance.GetCoins()}";
        HPSlider.value = PlayerManager.Instance.GetHP();
    }
}
