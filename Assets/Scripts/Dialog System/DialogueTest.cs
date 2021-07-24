using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTest : MonoBehaviour
{
    [SerializeField] private RectTransform _canvasRoot;
    [SerializeField] private Button _triggerBtn;

    private IEnumerator Start()
    {
        Task<Dialogue> dataTask = RealTimeDatabaseManager.GetDialogueDataById(1);
        yield return new WaitUntil(() => dataTask.IsCompleted);

        //TODO check if data exists
        DialogueView dialogueView = DialogueView.CreateView(_canvasRoot);
        dialogueView.Init(dataTask.Result);
        _triggerBtn.onClick.AddListener(() => { 
            dialogueView.Show();
            _triggerBtn.gameObject.SetActive(false);
        });
        _triggerBtn.gameObject.SetActive(true);
    }

}
