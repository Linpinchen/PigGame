using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Manager : MonoBehaviour
{
    public Text NowPlayer_text;
    public Text TempFraction_text;
    public Text[] PlayersFraction_text;
    public Text WinnerText;
    public Button RollBtn;
    public Button HoldBtn;
    public Button Rest_Btn;
    public Image Diceimage;
    public Image Restimage;
    public Sprite[] Dice_Sprites;


    Player[] players;
    Player NowPlayer;
    System.Action Rest_Action;
    int Tempi;
    int RoundCount;
    Tweener tweener;
    IEnumerator enumerator;

    // Start is called before the first frame update
    void Start()
    {

        init();

        RollBtn.onClick.AddListener(delegate ()
        {
            RollBtn.interactable = false;
            HoldBtn.interactable = false;

            int TempDice = Random.Range(1, Dice_Sprites.Length + 1);

            enumerator = DiceRoll(TempDice);

            StartCoroutine(enumerator);

        });



        HoldBtn.onClick.AddListener(delegate ()
        {

            NowPlayer.Fraction += Tempi;

            PlayersFraction_text[RoundCount].text = NowPlayer.Fraction.ToString();

            if (NowPlayer.Fraction >= 100)
            {

                Restimage.gameObject.SetActive(true);
                WinnerText.text = $"贏家:{ NowPlayer.PlayerName }";

            }
            else
            {
                PlayerChange();
            }

        });


        Rest_Btn.onClick.AddListener(delegate ()
        {

            Rest();

        });



    }

    /// <summary>
    /// 遊戲初始設定
    /// </summary>
    void init()
    {

        players = new Player[] { new Player("Player1", ref Rest_Action), new Player("Player2", ref Rest_Action) };

        tweener = Diceimage.transform.DOShakePosition(10, new Vector3(50, 50, 0));
        tweener.timeScale = 10;
        tweener.SetAutoKill(false);
        tweener.Pause();


        Rest();
    }

    /// <summary>
    /// 遊戲重置設定
    /// </summary>
    void Rest()
    {
        Restimage.gameObject.SetActive(false);

        Tempi = 0;

        RoundCount = 0;

        Rest_Action();

        NowPlayer = players[0];

        NowPlayer_text.text = $"當前玩家 ：{ NowPlayer.PlayerName}";

        TempFraction_text.text = $"暫存分數：{Tempi}";

        PlayersFraction_text[0].text = players[0].Fraction.ToString();
        PlayersFraction_text[1].text = players[1].Fraction.ToString();



    }

    /// <summary>
    ///  玩家互換
    /// </summary>
    void PlayerChange()
    {

        if (RoundCount != players.Length - 1)
        {

            RoundCount++;
        }
        else
        {
            RoundCount = 0;
        }

        Tempi = 0;

        NowPlayer = players[RoundCount];

        NowPlayer_text.text = $"當前玩家：{NowPlayer.PlayerName}";

        TempFraction_text.text = "暫存分數：";


    }

    /// <summary>
    /// 搖骰
    /// </summary>
    /// <param name="Temp"></param>
    /// <returns></returns>
    IEnumerator DiceRoll(int Temp)
    {

        Diceimage.transform.DORestart();

        for (int t = 0; t < 20; t++)
        {
            int i = Random.Range(0, Dice_Sprites.Length);
            Diceimage.sprite = Dice_Sprites[i];
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.05f);

        Diceimage.sprite = Dice_Sprites[Temp - 1];

        if (Temp == 1)
        {
            PlayerChange();
        }
        else
        {

            Tempi += Temp;
            TempFraction_text.text = $"暫存分數：{Tempi}";

        }


        RollBtn.interactable = true;
        HoldBtn.interactable = true;

    }



}

/// <summary>
///角色模板 
/// </summary>
public class Player
{
    /// <summary>
    /// 玩家分數
    /// </summary>
    public int Fraction ;

    /// <summary>
    /// 玩家名稱
    /// </summary>
    public string PlayerName;


    public Player(string PlayerName, ref System.Action action)
    {
        Fraction = 0;
        this.PlayerName = PlayerName;
        action += RestFraction;
    }

    /// <summary>
    /// 重置玩家分數
    /// </summary>
    public void RestFraction()
    {

        Fraction = 0;

    }

}