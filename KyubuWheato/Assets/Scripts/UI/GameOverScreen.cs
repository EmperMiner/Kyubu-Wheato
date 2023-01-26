using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI WheatGainedText;
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject winningScreen;
    [SerializeField] private TextMeshProUGUI winningWheatGainedText;
    [SerializeField] private TextMeshProUGUI tipsText;
    private string[] tips = new string[47]
    {
        "You can choose between opening chests for temporary upgrades for a cheap price, or save up Wheat and spend them in the shop for expensive but permanent upgrades.",
        "Spiders come in group and they are fast, but they don't turn corners very well.",
        "I've tried so hard and so many times to combat camping, the act of throwing a dice and staying still in a corner. As long as you're actively moving, I don't see it as a problem.",
        "Mini-boss enemies are slower and tankier versions of normal enemies. They drop a lot of wheat upon death.",
        "Better prepare carefully before venturing into the secret level, if you can find it.",
        "Experiment with different playstyles by buying a variety of upgrades in the shop, especially because wheat is scarce in the beginning.",
        "Teresa-chan is the main protagonist of Kyubu Kyubu Dice, an arcade-puzzle game made by eldelnacho where you command a dice's movement.",
        "There are portals on the edges of most maps that teleport you to the other side of the map for easy travel.",
        "Do you want to have bad Tom?",
        "Special upgrades are not unlockable through the shop. They have a rare chance to be found in chests, and they are unlocked permanently by defeating certain bosses.",
        "Did you know that some of the levels took inspiration the original DOOM, Among Us and my idiocy?",
        "Kyubu Kombos, Hotkeying and Dice Previews are the crux of the early game since they are incredibly cheap to unlock and require some skills.",
        "The person mainly responsible for Kyubu Wheato!'s art direction is ScarBorrower. However, EmperDev also drew the main character and the food items in the shop.",
        "Kyubu Wheato! has a lot of speedrunning potential. You can toggle the timer on and off in the settings. Will you rise up to the challenge?",
        "Kyubu Wheato! gets real chaotic by the end, yo.",
        "You should utilize the secondary attack for clearing hoards of enemies whenever you have a lot of spare dice.",
        "Totems are a great way to gain powerups and more wheat, granted that you can defeat the ensuing flock of enemies. Don't get too cocky, you may get unwelcomed visitors.",
        "Kyubu Wheato! has two main bosses, one in level 11 and one... somewhere.",
        "'Steve' was meant to be the main protagonist of this game. However, after eldel gave EmperDev a surplus of motivation, he decided to alter the main character to be Teresa-chan.",
        "Black Cats are small enemies with surge guns. The color of the bullets indicate which direction the surge will occur. Red is Up, Yellow is Left, Green is Bottom and Blue is Right",
        "Kyubu Wheato! development spanned about six months, but active development only took place in about three. It's because of exams and a really addicting game.",
        "A will extend their hands and drag you down.",
        "B will summon the forth wall.",
        "C will multiple and divide.",
        "D will send the colorful crows.",
        "E will bloom their corns.",
        "F will digest your dice.",
        "Health and defense upgrades differ in that defense is ignored by certain late-game enemies.",
        "An open area is easier to maneuver between enemies. Don't be caught in tight areas and dead ends.",
        "Wheat Cards are require to pass most levels and they incentivize roaming around the maps if you do not have the Supermap Upgrade.",
        "Pressing R heals you for 35% of your health, at the cost of 10% of your current wheat. It has a cooldown, so be sure to use it carefully.",
        "Multishot 2 and Dice Loyalty trivializes most encounters in the game, so they are intentionally held off for late-game chaos.",
        "This is my first time programming a full indie game. Tell me what you think on the official itch.io page!",
        "Enemies are dumb. Get them to line up in a straight line and hit them with a six.",
        "Teresa-chan's favorite dish is Pastel de Choclo since her creator has Chilean roots. This upgrade makes your dice explode into deadly rays sometimes.",
        "You better continue buying that extra wheat drop chance. It's optimal gameplay... I think.",
        "Maybe buy a speed upgrade you turtle!",
        "Remember to pick up your dice before entering a new level, or else they will be lost... forever!",
        "Supposedly, Teresa-chan takes a break from exploring the Kyubu Kyubu Land by traveling through the multi-verse and helping innocent people.",
        "Successful attempts in beating the final boss of the game may grant access to unwelcoming visitors into the roster.",
        "The final boss was naked in the earliest phase of development.",
        "I like to obliterate my enemies and watch the drop dead when I activate Myriad Cookies.",
        "Developing the game was an amazing journey, especially with many playtesters giving thoughtful feedback.",
        "Wheat is the ultimate goal.",
        "The Paper Stars upgrade came to me as I was making star origamis in class.",
        "Development of the game also took place in class, where I ignored the teachers and jolted down ideas and code for the game on paper.",
        "Kyubu Kombos are activated when you throw consecutive dice values. However, you can also try throwing the numbers from one to six in order to see what happens."
    };

    private int MainMenu = 0;
    private int GameOverGameToMenu = 3;
    private int Level_1 = 4;

    private static Action onIngameLoaderCallback;

    private void Start()
    {
        gameOverScreen.SetActive(false);
        HUD.SetActive(true);
    }

    public void GameOverTrigger(int WheatGained)
    {
        HUD.SetActive(false);
        WheatGainedText.text = "Wheats Harvested:" + WheatGained.ToString(); 
        gameOverScreen.SetActive(true); 
        if (PlayerPrefs.GetInt("FirstDeath") == 0)
        {
            PlayerPrefs.SetInt("FirstDeath", 1);
            tipsText.text = "You should check out the shop for useful upgrades to get stronger. You have some Wheat to spend! Also, dying means you can spend the wheats you've harvested in the shop.";
        }
        else 
        {
            tipsText.text = tips[UnityEngine.Random.Range(0, tips.Length)];
        }
    }

    public void WinningTrigger(int WheatGained)
    {
        HUD.SetActive(false);
        winningWheatGainedText.text = "Wheats Harvested:" + WheatGained.ToString() + " + 5000 Bonus"; 
        winningScreen.SetActive(true); 
    }

    public void ButtonSelect()
    {
        FindObjectOfType<AudioManager>().PlaySound("UIButtonPress");
    }

    public void Restart()
    {
        SceneManager.LoadScene(Level_1);
    }

    public void BackToMainMenu()
    {
        onIngameLoaderCallback = () => { SceneManager.LoadScene(MainMenu); };

        SceneManager.LoadScene(GameOverGameToMenu);
    }

    public static void IngameLoaderCallback()
    {
        if (onIngameLoaderCallback!=null)
        {
            onIngameLoaderCallback();
            onIngameLoaderCallback = null;
        }
    }   
}
