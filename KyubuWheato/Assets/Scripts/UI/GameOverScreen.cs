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
    [SerializeField] private TextMeshProUGUI HarvestersRoadText;
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject winningScreen;
    [SerializeField] private TextMeshProUGUI winningWheatGainedText;
    [SerializeField] private TextMeshProUGUI tipsText;
    private string[] tips = new string[57]
    {
 "You can choose between opening chests for temporary upgrades at a cheap price, or saving up Wheat and spending it in the shop for expensive but permanent upgrades.",
 "Spiders come in groups, and they are fast, but they don't turn corners very well.",
 "I've tried so hard and so many times to combat camping, the act of throwing a dice and staying still in a corner. As long as you're actively moving, I don't see it as a problem.",
 "Mini-boss enemies are slower and tankier versions of normal enemies. They drop a lot of wheat upon death.",
 "Better prepare carefully before venturing into the secret level, if you can find it.",
 "Experiment with different playstyles by buying a variety of upgrades in the shop, especially because wheat is scarce in the beginning.",
 "Teresa-chan is the main protagonist of Kyubu Kyubu Dice, an arcade-puzzle game made by Eldelnacho where you command a dice's movement to solve puzzles.",
 "There are portals on the edges of most maps that teleport you to the other side of the map for easy travel.",
 "Do you want to have bad Tom?",
 "Hidden Entrees are not unlockable through the shop. They have a rare chance to be found in chests, and they are unlocked permanently by defeating certain bosses.",
 "Did you know that some of the levels took inspiration from the original DOOM, Among Us, and my idiocy?",
 "Kyubu Kombos, Hotkeying and Dice Previewers are the crux of the early game since they are incredibly cheap to unlock and require some skills.",
 "The person mainly responsible for Kyubu Wheato!'s art direction is ScarBorrower. EmperDev drew the main character and the sprites in the shop.",
 "Kyubu Wheato! has a lot of speedrunning potential. You can toggle the timer on and off in the settings. Will you rise up to the challenge?",
 "Kyubu Wheato! gets real chaotic by the end, yo.",
 "You should utilize the charged attack for clearing hoards of enemies whenever you have a lot of spare dice.",
 "Totems are a great way to gain upgrades and wheat, granted that you can defeat the ensuing flock of enemies. Don't get too cocky, you may get unwelcomed visitors.",
 "Kyubu Wheato! has two major bosses, one in level 11 and another... somewhere.",
 "'Steve' was meant to be the main protagonist of this game. However, after Eldel gave EmperDev a surplus of motivation, EmperDev decided to alter the main character to be Teresa-chan.",
 "Black Cats are small enemies with surge bullets. The color of the bullets indicates in which direction the surge will occur. Red is Up, Yellow is Left, Green is Bottom and Blue is Right",
 "Kyubu Wheato! development spanned about seven months, but active development only took place in about three. It's because of exams and some really addicting games.",
 "A will reach out and drag you down.",
 "B will summon the fourth wall.",
 "C will multiple and divide.",
 "D will send colorful crows.",
 "E will bloom their corns.",
 "F will consume your dice.",
 "Health and defense upgrades differ in that defense is ignored by certain late-game enemies.",
 "An open area is easier to maneuver between enemies. Don't be caught in tight areas and dead ends.",
 "Wheat Cards are required to pass most levels, and they incentivize roaming around the maps if you do not have the Supermap Upgrade.",
 "Pressing R heals you for 35% of your health, at the cost of 10% of your current wheat. It has a cooldown, so be sure to use it carefully.",
 "Because Multishot II and Dice Loyalty trivialize the majority of encounters in the game, they are intentionally reserved for late-game chaos.",
 "This is my first time programming a full indie game. Tell me what you think on the official itch.io page!",
 "Enemies are dumb. Get them to line up in a straight line and hit them with a six.",
 "Teresa-chan's favorite dish is Pastel de Choclo, since her creator has Chilean roots. This upgrade makes your dice explode into deadly rays sometimes.",
 "You better buy that extra wheat drop chance in the shop. It's optimal gameplay... I think.",
 "Maybe buy a speed upgrade you turtle!",
 "Remember to pick up your dice before entering a new level, or else they will be lost... forever!",
 "Supposedly, Teresa-chan takes a break from exploring the Kyubu Kyubu Land by traveling through the multi-verse and helping innocent people.",
 "Successful attempts at defeating the game's final boss may add unwelcomed visitors to the roster.",
 "The final boss was naked in the earliest phase of development.",
 "I like to watch my enemies drop dead when I activate Myriad Cookies.",
 "Developing the game was an amazing 3-month journey, especially with many playtesters giving thoughtful feedback.",
 "Wheat is the ultimate goal.",
 "The Paper Stars upgrade came to me as I was making star origami in class.",
 "Development of the game also took place in class, where I ignored the teachers and jotted down ideas and code for the game on paper.",
 "Kyubu Kombos are activated when you throw consecutive dice values. You can also try throwing the numbers one through six to see what happens.",
 "Whales are cute. Whales in this game, less so. They come out of nowhere and surprise you with a horde of enemies.",
 "There was a creepy jumpscare of the whale in development, but it was deemed not creepy enough, so you'll get a cute whale instead.",
 "Level 5 Wheat Droprate used to grant the player 100% Wheat Droprate, but it was deemed too overpowered.",
 "The icon for Level 0-4 Strength was based on Minecraft swords, specifically a re-imagined sword made of Obsidian.",
 "The icon for Level 5 Strength was based on Red Scythe of the Seeker, a popular Roblox catalog item made by Yourius.",
 "The icon for Level 5 Defense is an image of a shield wielding a smaller version of itself and wearing an iron helmet from Minecraft.",
 "The icon for Level 5 Health was based on DaniDev's massive love of drinking milk. DaniDev inspired EmperDev to get into game development thanks to his entertaining videos.",
 "The icon for Level 5 Dice Cooldown was based on the popular timer used to speedrun games, Livesplit. ",
 "The icon for Level 5 Wheat Droprate was based on the Minecraft Haybale.",
 "The icon for Level 5 Dice Droprate was based on statistics' concept of Binomial Distribution."
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

    public void GameOverTrigger(int WheatGained, int level)
    {
        HUD.SetActive(false);
        WheatGainedText.text = "Wheats Harvested:" + WheatGained.ToString(); 
        HarvestersRoadText.text = "Harvester's road bonus:" + (level*200).ToString() + " Wheats";
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
