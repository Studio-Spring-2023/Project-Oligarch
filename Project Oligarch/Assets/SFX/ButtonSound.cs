using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public int hoverSoundIndex;
    public int clickSoundIndex; 

    private void Start()
    {
        // Add button click event listener
        Button button = GetComponent<Button>();
        button.onClick.AddListener(PlayClickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Play sound when hovering over the button
        SoundManager.instance.PlaySound(hoverSoundIndex, transform.position);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Play sound when clicking the button
        SoundManager.instance.PlaySound(clickSoundIndex, transform.position);
    }

    private void PlayClickSound()
    {
        // Play sound when the button is clicked
        SoundManager.instance.PlaySound(clickSoundIndex, transform.position);
    }
}
