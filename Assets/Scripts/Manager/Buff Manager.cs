using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{

    private enum Buff { jumpPower, stealth, heal };
    private float tic = 1f;
    private GameObject player;
    private PlayerManager playerScript;

    void Awake()
    {
        player = transform.parent.gameObject;
        playerScript = player.GetComponent<PlayerManager>();
    }

    IEnumerator Heal(int heal)
    {
        float timer = 0;
        while (timer < heal)
        {
            playerScript.AddHp(1);
            timer += tic;
            yield return new WaitForSeconds(tic);
        }
        yield return null;
    }

    IEnumerator Stealth(float time)
    {
        float timer = 0;
        player.tag = "Untagged";
        player.GetComponent<SpriteRenderer>().material.color = HexColor("#747474");
        while (timer < time)
        {
            timer += tic;
            yield return new WaitForSeconds(tic);
        }
        player.tag = "Player";
        player.GetComponent<SpriteRenderer>().material.color = HexColor("#FFFFFF");
        yield return null;
    }

    IEnumerator JumpPower(float time, int power)
    {
        float timer = 0;
        playerScript.AddJumpPower(power);
        while(timer < time)
        {
            timer += tic;
            yield return new WaitForSeconds(tic);
        }
        playerScript.AddJumpPower(-1 * power);
        yield return null;
    }

    public void BuffStart(string buffName, float time = 5f, int heal = 0, int jumpPower = 0)
    {
        Buff key = (Buff)Buff.Parse(typeof(Buff), buffName);
        switch(key)
        {
            case Buff.jumpPower:
                StartCoroutine(JumpPower(time, jumpPower));
                break;
            case Buff.heal:
                StartCoroutine(Heal(heal));
                break;
            case Buff.stealth:
                StartCoroutine(Stealth(time));
                break;
        }
    }

    public static Color HexColor(string hexCode)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(hexCode, out color))
        {
            return color;
        }

        Debug.LogError("[UnityExtension::HexColor]invalid hex code - " + hexCode);
        return Color.white;
    }
}
