using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    Normal,
    Fall
}

public class CharacterStateManager : MonoBehaviour
{
    public CharacterState State;
    public float FallTime;
    public Sprite NormalSprite;
    public Sprite FallSprite;

    private float TimeCount;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
        SetCharacter();
    }

    private void CheckState()
    {
        if (State == CharacterState.Fall)
        {
            TimeCount += Time.deltaTime;
            if (TimeCount > FallTime)
            {
                TimeCount = 0;
                State = CharacterState.Normal;
            }
        }
    }

    private void SetCharacter()
    {
        switch (State)
        {
            case CharacterState.Normal:
                GetComponent<SpriteRenderer>().sprite = NormalSprite;
                GetComponent<TreatField>().enabled = true;
                GetComponent<Capture>().enabled = true;
                break;
            case CharacterState.Fall:
                GetComponent<SpriteRenderer>().sprite = FallSprite;
                GetComponent<TreatField>().enabled = false;
                GetComponent<Capture>().enabled = false;
                break;
        }
    }
}
