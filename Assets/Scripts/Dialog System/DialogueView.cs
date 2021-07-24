using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Add logic for skip button;
/// Expand async prefab loading
/// </summary>

public class DialogueView : CommonView, IPointerClickHandler
{
    [SerializeField] private float _typeSpeed = 0.3f;

    [SerializeField] private GameObject _titleObjectLeft;
    [SerializeField] private GameObject _titleObjectRight;
    [SerializeField] private TextMeshProUGUI _titleNameLeft;
    [SerializeField] private TextMeshProUGUI _titleNameRight;

    [SerializeField] private TextMeshProUGUI _textBox;

    [SerializeField] private Button _btnSkip;

    private DialoguePresenter _presenter;
    private Dialogue.Title _currentTitle;

    public static DialogueView CreateView(RectTransform parent)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/DialogueView");
        var go = Instantiate(prefab, parent);

        DialogueView view = go.GetComponent<DialogueView>();
        return view;
    }

    public override void Init(params object[] args)
    {
        _presenter = new DialoguePresenter((Dialogue)args[0]);
        _presenter.SetDialogueQueue();
        Hide();
    }

    public override void Show()
    {
        base.Show();
        DisplayNextSentence();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnDialogueBoxClick();
    }

    private void OnDialogueBoxClick()
    {
        if (_currentTitle != null)
        {
            StopAllCoroutines();
            _textBox.text = _currentTitle.Sentence;
            _currentTitle = null;
            return;
        }

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        _currentTitle = _presenter.GetNextTitle();
        if (_currentTitle == null)
        {
            Hide();
            return;
        }

        SetTitleSide();

        StopAllCoroutines();
        StartCoroutine(TypeText());
    }

    private void SetTitleSide()
    {
        bool isLeftActive = false;

        switch (_currentTitle.Side)
        {
            case DialogueSide.Right:
                _titleNameRight.text = _currentTitle.Name;
                break;
            case DialogueSide.Left:
            default:
                _titleNameLeft.text = _currentTitle.Name;
                isLeftActive = true;
                break;
        }

        SetTitleHeaders(isLeftActive);
    }

    private void SetTitleHeaders(bool isLeftSideActive)
    {
        _titleObjectLeft.SetActive(isLeftSideActive);
        _titleObjectRight.SetActive(!isLeftSideActive);
    }

    private IEnumerator TypeText()
    {
        _textBox.text = string.Empty;

        foreach (char letter in _currentTitle.Sentence.ToCharArray())
        {
            _textBox.text += letter;
            yield return new WaitForSeconds(_typeSpeed);
        }

        _currentTitle = null;
    }
}
