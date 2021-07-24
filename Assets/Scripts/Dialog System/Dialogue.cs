using System.Collections.Generic;

public enum DialogueSide : long
{
    Left,
    Right
}

[System.Serializable]
public class Dialogue
{
    public long Id;
    public List<Title> Titles;

    [System.Serializable]
    public class Title
    {
        public long TitleId;
        public DialogueSide Side;
        public string Name;
        public string Sentence;
    }
}
