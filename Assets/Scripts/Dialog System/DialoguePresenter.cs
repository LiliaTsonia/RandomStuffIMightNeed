using System.Collections.Generic;

public class DialoguePresenter
{
    private readonly Dialogue _dialogue;
    private Queue<Dialogue.Title> _titles = new Queue<Dialogue.Title>();

    //private int _currentTitleId;

    public DialoguePresenter(Dialogue dialogue)
    {
        _dialogue = dialogue;
    }

    public void SetDialogueQueue()
    {
        _titles.Clear();

        foreach (Dialogue.Title title in _dialogue.Titles)
        {
            _titles.Enqueue(title);
        }
    }

    public Dialogue.Title GetNextTitle()
    {
        if (_titles.Count == 0)
        {
            return null;
        }

        return _titles.Dequeue();
    }
}
