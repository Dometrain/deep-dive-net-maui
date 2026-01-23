using Newtonsoft.Json;

namespace HelloMaui.UnitTests.Mocks;

class MockPreferences : IPreferences
{
	readonly Dictionary<string, string> _dictionary = new();

	public bool ContainsKey(string key, string? sharedName = null)
	{
		return _dictionary.ContainsKey(key);
	}

	public void Remove(string key, string? sharedName = null)
	{
		_dictionary.Remove(key);
	}

	public void Clear(string? sharedName = null)
	{
		_dictionary.Clear();
	}

	public void Set<T>(string key, T value, string? sharedName = null)
	{
		var serializedValue = JsonConvert.SerializeObject(value);

		if (ContainsKey(key))
		{
			_dictionary[key] = serializedValue;
		}
		else
		{
			_dictionary.Add(key, serializedValue);
		}
	}

	public T Get<T>(string key, T defaultValue, string? sharedName = null)
	{
		if (ContainsKey(key))
		{
			var serializedValue = _dictionary[key];
			return JsonConvert.DeserializeObject<T>(serializedValue) ?? throw new InvalidOperationException("Key does not exist");
		}

		return defaultValue;
	}
}