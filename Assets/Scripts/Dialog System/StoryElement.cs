using System.Collections.Generic;

public class StoryElement
{
    private readonly Dialogue _dialogue;
    private Queue<string> _sentences = new Queue<string>();

    public string DialogTitle => _dialogue.Title;

    public StoryElement(Dialogue dialogue)
    {
        _dialogue = dialogue;
    }

    public void SetDialogueQueue()
    {
        _sentences.Clear();

        foreach (string sentence in _dialogue.Sentences)
        {
            _sentences.Enqueue(sentence);
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
