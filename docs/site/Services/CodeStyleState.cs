using DateRecurrenceR.Docs.Models;

namespace DateRecurrenceR.Docs.Services;

/// <summary>
/// Holds the globally selected <see cref="CodeStyle"/> so the choice persists across pages.
/// Registered as a singleton; code-snippet boxes read it and raise <see cref="Changed"/> on toggle.
/// </summary>
public class CodeStyleState
{
    private CodeStyle _current = CodeStyle.Enumerator;

    public CodeStyle Current
    {
        get => _current;
        set
        {
            if (_current == value) return;
            _current = value;
            Changed?.Invoke();
        }
    }

    public event Action? Changed;
}
