namespace NFRuntime.Managements.ConfigManagement
{
    public interface IConfigDB
    {
        bool Load();
        bool SetBool(string key, bool is_on);
        bool SetInt(string key, int val);
        bool SetFloat(string key, float val);
        bool GetBool(string key);
        int GetInt(string key, int default_value);
        float GetFloat(string key, float default_value);
        string GetString(string key, string default_value);
        bool SetString(string key, string val);
    }
}