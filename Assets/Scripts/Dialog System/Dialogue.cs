[System.Serializable]
public class Dialogue
{
    public int Id;
    public Title[] Titles;

    [System.Serializable]
    public class Title
    {
        public int TitleId;
        public string Name;
        public string Sentence;
    }
}
