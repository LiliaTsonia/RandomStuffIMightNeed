using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTest : MonoBehaviour
{
    [SerializeField] private RectTransform _canvasRoot;
    [SerializeField] private Button _triggerBtn;

    private DialogueView _dialogueView;

    private IEnumerator Start()
    {
        Task<Dialogue> dataTask = RealTimeDatabaseManager.GetDialogueDataById(1);
        yield return new WaitUntil(() => dataTask.IsCompleted);

        if(dataTask != null)
        {
            _dialogueView = DialogueView.CreateView(_canvasRoot);
            _dialogueView.Init(dataTask.Result);
            _triggerBtn.onClick.AddListener(() => {
                _dialogueView.Show();
                _triggerBtn.gameObject.SetActive(false);
            });
            _triggerBtn.gameObject.SetActive(true);
        }
    }

}
