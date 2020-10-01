using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthScoreController : MonoBehaviour
{
    [Range(0f, 100f)]
    public float trueHealth = 100f;
    [Range(0f,1f)]
    public float trueHealthIncreaseRate = 1f;   // Maybe make a gamemode in which you are constantly decreasing health
    [Range(0f, 100f)]
    public float greyHealth = 100f;

    public float scoreDeceleration;
    public float scoreBaseDecrement;
    public float scoreCurDecrement;

    public int scoreTier;
    public float scoreAtTier; // add an array for different score requirements at different tiers
    
    // add gross points

    public UiManager uiManager;

    public void Start()
    {
        uiManager.UpdateHealthDisplay(trueHealth, greyHealth);
        uiManager.UpdateScoreDisplay(scoreAtTier, scoreTier);

        IEnumerator coroutine = LerpTrueHealthAndScore();
        StartCoroutine(coroutine);
    }

    WaitForSeconds x = new WaitForSeconds(0.03f);

    public IEnumerator LerpTrueHealthAndScore()
    {
        while (true) {
            if (trueHealth != greyHealth) {
                trueHealth = (trueHealth < greyHealth - trueHealthIncreaseRate) ? trueHealth + trueHealthIncreaseRate : greyHealth;
                uiManager.UpdateHealthDisplay(trueHealth, greyHealth);
            }


            if (scoreAtTier == 0f && scoreTier == 0)
            {
                // Do nothing
            } 
            else
            {
                if (scoreAtTier < scoreCurDecrement && scoreTier > 0)
                {
                    scoreAtTier = 100f - (scoreCurDecrement - scoreAtTier);
                    scoreCurDecrement += scoreDeceleration;
                    scoreTier--;
                }
                else if (scoreAtTier < scoreCurDecrement && scoreTier == 0)
                {
                    scoreAtTier = 0f;
                }
                else
                {
                    scoreAtTier -= scoreCurDecrement;
                    scoreCurDecrement += scoreDeceleration;
                }

                uiManager.UpdateScoreDisplay(scoreAtTier, scoreTier);
            }

            yield return x;
        }
    }

    public void TakeDamage(int dmg)
    {
        
        trueHealth = (trueHealth < dmg)? (0f) : (trueHealth - dmg);
        greyHealth = (greyHealth < dmg)? (0f) : (greyHealth - dmg);

        uiManager.UpdateHealthDisplay(trueHealth, greyHealth);
    }

    public void IncreaseGreyHealth(float value)
    {
        greyHealth = (greyHealth <= 100f - value) ? (greyHealth + value) : (100f);
        uiManager.UpdateHealthDisplay(trueHealth, greyHealth);
    }

    public void IncreaseScore(float value)
    {
        scoreAtTier += value;
        scoreCurDecrement = scoreBaseDecrement;

        if(scoreAtTier > 100f)
        {
            scoreAtTier -= 100f;
            scoreTier++;
        }

        uiManager.UpdateScoreDisplay(scoreAtTier, scoreTier);
    }
}
