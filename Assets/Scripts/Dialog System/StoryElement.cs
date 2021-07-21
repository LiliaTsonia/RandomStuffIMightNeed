using System.Collections.Generic;

public class StoryElement
{
    private readonly Dialogue _dialogue;
    private Queue<string> _sentences = new Queue<string>();

    private int _currentTitleId;

    public string DialogTitle => _dialogue.Titles[_currentTitleId].Name;

    public StoryElement(Dialogue dialogue)
    {
        _dialogue = dialogue;
    }

    public void SetDialogueQueue()
    {
        _sentences.Clear();

        foreach (Dialogue.Title title in _dialogue.Titles)
        {
            _sentences.Enqueue(title.Sentence);
        }
    }

    public string GetNextSentence()
    {
        if (_sentences.Count == 0)
        {
            return null;
        }

        return _sentences.Dequeue();
    }
}
