using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Purpose of PlayerHealthScoreController.cs
 * 
 * PlayerHealthScoreController.cs handles the player's health and score multiplier. Player's health is split into two parts...
 * 
 * trueHealth represents the actual real health of the player. If this reaches 0, the player is meant to die (but for debugging purpose, this is not implemented)
 * greyHealth represents the what the true health of the player should be regrenerating towards. If greyHealth > trueHealth, trueHealth will regenerate towards greyHealth.
 * 
 * PlayerHealthScoreController.cs communicates with UiManager to update Health and Score Display when necessary
 */

public class PlayerHealthScoreController : MonoBehaviour
{
    [Range(0f, 100f)]
    public float trueHealth = 100f;
    [Range(0f,1f)]
    public float trueHealthIncreaseRate = 1f;   // Maybe make a gamemode in which you are constantly decreasing health?
    [Range(0f, 100f)]
    public float greyHealth = 100f;

    // handles score exponential decrease over time
    public float scoreDeceleration;
    public float scoreBaseDecrement;
    public float scoreCurDecrement;

    /* scoreAtTier is constantly decreases and accelerates over time. However, by increasing scoreAtTier, by getting points from killing enemies, scoreAtTier's rate of depletion resets
     * backs to its base rate of depletion.
     * 
     * if scoreAtTier is greater than 100, our scoreTier (which multiplies our points) increases by 1, and scoreAtTier is set to reset. However, if scoreAtTier decreases below 0,
     * scoreTier decreases by 1
     */

    public int scoreTier;
    public float scoreAtTier; // add an array for different score requirements at different tiers
    
    // add gross points

    public UiManager uiManager;

    public void Start()
    {
        uiManager.UpdateHealthDisplay(trueHealth, greyHealth);
        uiManager.UpdateScoreDisplay(scoreAtTier, scoreTier);

        IEnumerator coroutine = LerpTrueHealthAndScore(); // since we dont need to update health every frame, we can same computational power buy having occuring less frequently in a coroutine
        StartCoroutine(coroutine);
    }

    WaitForSeconds x = new WaitForSeconds(0.03f);

    public IEnumerator LerpTrueHealthAndScore()
    {
        while (true) {

            // increases trueHealth if greyHealth is greater than trueHealth
            if (trueHealth != greyHealth) {
                trueHealth = (trueHealth < greyHealth - trueHealthIncreaseRate) ? trueHealth + trueHealthIncreaseRate : greyHealth;
                uiManager.UpdateHealthDisplay(trueHealth, greyHealth);
            }


            if (scoreAtTier == 0f && scoreTier == 0)
            {
                // Do nothing, no need to UpdateScoreDisplay();
            } 
            else
            {

                // if scoreAtTier at going to go negative after another decrement, decrease the scoreTier by 1
                if (scoreAtTier < scoreCurDecrement && scoreTier > 0)
                {
                    scoreAtTier = 100f - (scoreCurDecrement - scoreAtTier);
                    scoreCurDecrement += scoreDeceleration;
                    scoreTier--;
                }
                
                // else if scoreTier is 0, set scoreAtTier to 0
                else if (scoreAtTier < scoreCurDecrement && scoreTier == 0)
                {
                    scoreAtTier = 0f;
                }

                //else, decrease scoreAtTier by normal
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
     
        // if trueHealth is about to go negative, set to 0, else decrease trueHealth by the amount of damage we have taken
        // this process is the same for greyHealth then we UpdateHealthDisplay
        trueHealth = (trueHealth < dmg)? (0f) : (trueHealth - dmg);
        greyHealth = (greyHealth < dmg)? (0f) : (greyHealth - dmg);

        uiManager.UpdateHealthDisplay(trueHealth, greyHealth);
    }

    public void IncreaseGreyHealth(float value)
    {
        // if we the amount we increase our greyHealth by will not increase over 100, then increase greyHealth by "value", else
        // set greyHealth to 100
        greyHealth = (greyHealth <= 100f - value) ? (greyHealth + value) : (100f);
        uiManager.UpdateHealthDisplay(trueHealth, greyHealth);
    }

    public void IncreaseScore(float value)
    { 
        scoreAtTier += value;
        scoreCurDecrement = scoreBaseDecrement; // resets scoreAtTier's rate of depletion

        // if scoreAtTier is greater than 100, increase scoreTier by 1
        if (scoreAtTier > 100f)
        {
            scoreAtTier -= 100f;
            scoreTier++;
        }

        uiManager.UpdateScoreDisplay(scoreAtTier, scoreTier);
    }
}
