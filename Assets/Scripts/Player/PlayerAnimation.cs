using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public SpriteRenderer teteSprite;

    public SpriteRenderer yeuxSprite;
    public List<Sprite> yeuxSpriteListe;

    public Animator habitsAnimator;
    public SpriteRenderer habitsSprite;

    public void Animate(float deplacementX, float deplacementY)
    {
        if (Mathf.Abs(deplacementX) > 0 || Mathf.Abs(deplacementY) > 0)
        {
            if (Mathf.Abs(deplacementY) >= Mathf.Abs(deplacementX))
            {
                habitsAnimator.SetBool("Vertical", true);
                yeuxSprite.sprite = yeuxSpriteListe[5];
                if (deplacementY >= 0)
                {
                    yeuxSprite.sprite = null;
                }
                else
                {
                    yeuxSprite.sprite = yeuxSpriteListe[5];
                }
            }
            else
            {
                habitsAnimator.SetBool("Vertical", false);
                yeuxSprite.sprite = yeuxSpriteListe[4];

                if (deplacementX > 0)
                {
                    habitsSprite.flipX = false;
                    yeuxSprite.flipX = false;
                }
                else
                {
                    habitsSprite.flipX = true;
                    yeuxSprite.flipX = true;
                }
            }
            habitsAnimator.SetBool("Idle", false);
        }
        else
        {
            habitsAnimator.SetBool("Idle", true);
        }

        Debug.Log("Animator Idle : " + habitsAnimator.GetBool("Idle"));
        Debug.Log("Animator Vertical : " + habitsAnimator.GetBool("Vertical"));

    }
}
