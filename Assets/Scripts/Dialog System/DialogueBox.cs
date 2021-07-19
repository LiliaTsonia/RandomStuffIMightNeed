using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private float _typeSpeed = 0.1f;

    [SerializeField] private TextMeshProUGUI _titleName;
    [SerializeField] private TextMeshProUGUI _textBox;

    private StoryElement _storyElement;
    private string _currentSentence;

    public void SetNewDialog(Dialogue dialogue)
    {
        _storyElement = new StoryElement(dialogue);
        _storyElement.SetDialogueQueue();

        GetComponent<Button>().onClick.AddListener(OnDialogueBoxClick);
        DisplayNextSentence();
    }

    private void OnDialogueBoxClick()
    {
        if(_currentSentence != null)
        {
            StopAllCoroutines();
            _textBox.text = _currentSentence;
            return;
        }

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        _titleName.text = _storyElement.DialogTitle;

        _currentSentence = _storyElement.GetNextSentence();
        if(_currentSentence == null)
        {
            FinishDialogue();
            return;
        }

        StopAllCoroutines();
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        _textBox.text = string.Empty;

        foreach(char letter in _currentSentence.ToCharArray())
        {
            _textBox.text += letter;
            yield return new WaitForSeconds(_typeSpeed);
        }

        _currentSentence = null;
    }

    private void FinishDialogue()
    {
        //TODO implement skipping current dialogue queue
        GetComponent<Button>().onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
