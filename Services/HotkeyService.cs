using System.Windows.Input;

namespace DnDVoiceStudio.Services;

public class HotkeyService
{
    public event Action<Key>? HotkeyPressed;

    public void HandleKey(Key key)
    {
        HotkeyPressed?.Invoke(key);
    }
}