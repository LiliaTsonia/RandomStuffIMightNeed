using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;
using System.Collections;
using SimpleJSON;

public class RealTimeDatabaseManager : MonoBehaviour
{
    //private static readonly string DATABASE_URL = "https://fir-test-project-8772b-default-rtdb.europe-west1.firebasedatabase.app/";
    private static DatabaseReference _database;

    private void Awake() //TODO remove to separate method
    {
        _database = FirebaseDatabase.DefaultInstance.GetReference("dialogues");

        //yield return StartCoroutine(TestDataSave());

        //---------------- Receive data -------------------------------
        //Task<Dialogue> dataTask = GetDialogueDataById(2);
        //yield return new WaitUntil(() => dataTask.IsCompleted);

        //foreach (var title in dataTask.Result.Titles)
        //{
        //    Debug.Log(title.Name + " : " + title.Sentence);
        //}
    }

    private async Task<long> GetDialoguesCount()
    {
        var dataSnaphot = await _database.GetValueAsync();
        return dataSnaphot.ChildrenCount;
    }

    private void SaveNewDialogue(Dialogue dialogue, long saveIndex)
    {
        string json = JsonUtility.ToJson(dialogue);
        _database.Child(saveIndex.ToString()).SetRawJsonValueAsync(json);
    }

    public static async Task<Dialogue> GetDialogueDataById(long dialogueId)
    {
        var dataSnapshot = await _database.GetValueAsync();

        if (!dataSnapshot.Exists)
        {
            return null;
        }

        string jsonRaw = dataSnapshot.GetRawJsonValue();

        JSONNode data = JSON.Parse(jsonRaw);
        foreach (JSONNode node in data)
        {
            if (node["Id"].Value.Equals(dialogueId.ToString()))
            {
                List<Dialogue.Title> dialogueTitles = new List<Dialogue.Title>();

                foreach (JSONNode title in node["Titles"].Values)
                {
                    dialogueTitles.Add(new Dialogue.Title
                    {
                        TitleId = long.Parse(title["TitleId"].Value),
                        Side = (DialogueSide)long.Parse(title["Side"].Value),
                        Name = title["Name"].Value,
                        Sentence = title["Sentence"].Value
                    }); ; ;
                }
                return new Dialogue { Id = dialogueId, Titles = dialogueTitles };
            }
        }

        return null;
    }

    private IEnumerator TestDataSave()
    {
        var dialoguesCountTask = GetDialoguesCount();
        yield return new WaitUntil(() => dialoguesCountTask.IsCompleted);

        if (dialoguesCountTask.Result != 0)
        {
            List<Dialogue.Title> titles = new List<Dialogue.Title>();
            titles.Add(new Dialogue.Title { TitleId = 0, Side = DialogueSide.Left, Name = "Sam", Sentence = "수고했어요!" });
            titles.Add(new Dialogue.Title { TitleId = 1, Side = DialogueSide.Right, Name = "Phill", Sentence = "네, 고마워요!" });
            titles.Add(new Dialogue.Title { TitleId = 2, Side = DialogueSide.Right, Name = "Phill", Sentence = "알 수 없는 기분이" });

            Dialogue dialogue = new Dialogue { Id = dialoguesCountTask.Result, Titles = titles };

            SaveNewDialogue(dialogue, dialoguesCountTask.Result);
        }
    }

    //private void TestReadDataFromSnapshot(int dialogueId)
    //{
    //    _database.GetValueAsync().ContinueWith(task =>
    //    {
    //        if (task.IsCompleted)
    //        {
    //            var json = task.Result.GetRawJsonValue();
    //            Debug.Log(json);
    //            foreach (DataSnapshot data in task.Result.Children)
    //            {
    //                DataSnapshot dialogue = data.Child("");
    //                long id = (long)dialogue.Child("Id").Value;

    //                if (id.Equals(dialogueId))
    //                {
    //                    List<Dialogue.Title> dialogueTitles = new List<Dialogue.Title>();
    //                    DataSnapshot dialogueData = dialogue.Child("Titles");

    //                    foreach (DataSnapshot titleData in dialogueData.Children)
    //                    {
    //                        IDictionary titles = (IDictionary)titleData.Value;

    //                        dialogueTitles.Add(new Dialogue.Title
    //                        {
    //                            TitleId = (long)titles["TitleId"],
    //                            Name = titles["Name"].ToString(),
    //                            Sentence = titles["Sentence"].ToString()
    //                        });
    //                    }

    //                    break;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            Debug.LogError(task.Exception.Message);
    //        }
    //    });
    //}
}
