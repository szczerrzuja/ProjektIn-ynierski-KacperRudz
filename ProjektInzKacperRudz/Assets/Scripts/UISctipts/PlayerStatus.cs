using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStatus : MonoBehaviour
{
    private Slider HPSlide;
    private Slider SPSlide;
    float maxHP, maxSP;

    // Start is called before the first frame update
    void Start()
    {
        HPSlide = transform.Find("HP").transform.Find("BAR").transform.Find("HPSlider").GetComponent<Slider>();
        SPSlide = transform.Find("SP").transform.Find("BAR").transform.Find("SPSlider").GetComponent<Slider>();

        HPSlide.wholeNumbers = false;
        HPSlide.maxValue = maxHP;
        HPSlide.value = maxHP;

        SPSlide.wholeNumbers = false;
        SPSlide.maxValue = maxSP;
        SPSlide.value = maxSP;
        
    }
    public void Initiate(float maxHealth, float maxStamina)
    {

        maxHP = maxHealth;
        HPSlide.maxValue = maxHP;

        maxSP = maxStamina;
        SPSlide.maxValue = maxSP;
    }
    public void SetCurrentLevels(float curHealth, float curStamina)
    {

        HPSlide.value = curHealth;

        SPSlide.value = curStamina;


    }
    // Update is called once per frame
    void Update()
    {

    }
}
