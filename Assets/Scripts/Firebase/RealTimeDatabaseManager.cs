using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class RealTimeDatabaseManager : MonoBehaviour
{
    //private static readonly string DATABASE_URL = "https://fir-test-project-8772b-default-rtdb.europe-west1.firebasedatabase.app/";
    private DatabaseReference _database;

    private void Start()
    {
        _database = FirebaseDatabase.DefaultInstance.RootReference;
        TestReadData();
    }

    private void TestSaveData()
    {
        Dialogue.Title[] titles =  { 
            new Dialogue.Title { TitleId = 0, Name = "Sam", Sentence = "수고했어요!" },
            new Dialogue.Title { TitleId = 1, Name = "Phill", Sentence = "네, 고마워요!" }
        };

        Dialogue dialogue = new Dialogue { Id = 2, Titles = titles };

        string json = JsonUtility.ToJson(dialogue);
    }

    private void TestReadData()
    {
        _database.Child("dialogues").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log(task.Result);
            }
        });
    }
}
