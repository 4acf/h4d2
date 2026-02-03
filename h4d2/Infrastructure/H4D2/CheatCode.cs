using H4D2.Levels;

namespace H4D2.Infrastructure.H4D2;

public class CheatCode
{
    private readonly Level _level;

    private static readonly MovementKey[] _sequence =
    [
        MovementKey.D,
        MovementKey.A,
        MovementKey.D,
        MovementKey.W,
        MovementKey.A,
        MovementKey.D,
        MovementKey.S
    ];
    
    private readonly LinkedList<MovementKey> _inputHistory;
    
    public CheatCode(Level level)
    {
        _level = level;
        _inputHistory = [];
    }

    public void Update(Input input)
    {
        foreach (MovementKey key in input.PressedMovementKeys)
        {
            if(_inputHistory.Count == 0 || _inputHistory.Last() != key)
                _inputHistory.AddLast(key);
        }

        while (_inputHistory.Count > _sequence.Length)
            _inputHistory.RemoveFirst();

        if (_CheatCodeEntered())
        {
            _level.SpawnJoe();
            _inputHistory.Clear();
        }
    }

    private bool _CheatCodeEntered()
    {
        if (_inputHistory.Count != _sequence.Length)
            return false;

        int i = 0;
        foreach (MovementKey key in _inputHistory)
        {
            if (key != _sequence[i])
                return false;
            i++;
        }

        return true;
    }
}