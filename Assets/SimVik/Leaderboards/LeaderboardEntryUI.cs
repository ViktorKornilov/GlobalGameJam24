using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SimVik.Leaderboards
{
    public class LeaderboardEntryUI : MonoBehaviour
    {
        public TMP_Text rankText;
        public TMP_Text nameText;
        public TMP_Text scoreText;

        public Color firstPlaceColor = Color.yellow;
        public Color secondPlaceColor = Color.gray;
        public Color thirdPlaceColor = Color.red;

        public Color cardColor = Color.white;
        public Color cardColorAlt = Color.gray;

        public void Set(int place,string name,int score)
        {
            rankText.text = place + ".";
            nameText.text = name;
            scoreText.text = score.ToString();

            if(place == 1) SetColor(firstPlaceColor);
            else if(place == 2) SetColor(secondPlaceColor);
            else if(place == 3) SetColor(thirdPlaceColor);

            GetComponent<Image>().color = place % 2 == 0 ? cardColor : cardColorAlt;
        }

        void SetColor(Color color)
        {
            rankText.color = color;
            nameText.color = color;
            scoreText.color = color;
        }
    }
}