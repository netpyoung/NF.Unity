using UnityEngine;

namespace NFRuntime.Managements.ConfigManagement
{
    public class PlayerPrefConfigDB : IConfigDB
    {
        public bool Load()
        {
            return true;
        }

        public bool GetBool(string key)
        {
            return PlayerPrefs.GetInt(key) == 1;
        }

        public float GetFloat(string key, float default_value)
        {
            return PlayerPrefs.GetFloat(key, default_value);
        }

        public int GetInt(string key, int default_value)
        {
            return PlayerPrefs.GetInt(key, default_value);
        }

        public string GetString(string key, string default_value)
        {
            return PlayerPrefs.GetString(key, default_value);
        }

        public bool SetBool(string key, bool is_on)
        {
            if (is_on)
            {
                PlayerPrefs.SetInt(key, 1);
            }
            else
            {
                PlayerPrefs.SetInt(key, 0);
            }

            PlayerPrefs.Save();

            return true;
        }

        public bool SetFloat(string key, float val)
        {
            PlayerPrefs.SetFloat(key, val);
            PlayerPrefs.Save();

            return true;
        }

        public bool SetInt(string key, int val)
        {
            PlayerPrefs.SetInt(key, val);
            PlayerPrefs.Save();

            return true;
        }

        public bool SetString(string key, string val)
        {
            PlayerPrefs.SetString(key, val);
            PlayerPrefs.Save();

            return true;
        }
    }
}