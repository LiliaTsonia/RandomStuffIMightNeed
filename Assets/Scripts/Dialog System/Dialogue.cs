using System.Collections.Generic;

[System.Serializable]
public class Dialogue
{
    public long Id;
    public List<Title> Titles;

    [System.Serializable]
    public class Title
    {
        public long TitleId;
        public string Name;
        public string Sentence;
    }
}
