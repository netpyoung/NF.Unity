using System.Collections.Generic;
using NUnit.Framework;

namespace NFRuntime.Managements.OptionManagement.Test
{

    public class OptionManagementTest
    {
        public class OptionStorage : IOptionStorage
        {
            public Dictionary<string, float> _floatDic = new Dictionary<string, float>();
            public Dictionary<string, bool> _boolDic = new Dictionary<string, bool>();
            public bool GetBool(string key)
            {
                if (_boolDic.TryGetValue(key, out bool val))
                {
                    return val;
                }
                return false;
            }

            public float GetFloat(string key, float default_value)
            {
                if (_floatDic.TryGetValue(key, out float val))
                {
                    return val;
                }
                return default_value;
            }

            public int GetInt(string key, int default_value)
            {
                throw new System.NotImplementedException();
            }

            public string GetString(string key, string default_value)
            {
                throw new System.NotImplementedException();
            }

            public bool Load()
            {
                return true;
            }

            public bool SetBool(string key, bool is_on)
            {
                _boolDic[key] = is_on;
                return true;
            }

            public bool SetFloat(string key, float val)
            {
                _floatDic[key] = val;
                return true;
            }

            public bool SetInt(string key, int val)
            {
                throw new System.NotImplementedException();
            }

            public bool SetString(string key, string val)
            {
                throw new System.NotImplementedException();
            }
        }
        [Test]
        public void Hello()
        {
            var optionStorage = new OptionStorage();
            var optionSound = new OptionSound(optionStorage);
            Assert.IsFalse(optionSound.IsMute);
            optionSound.IsMute = true;
            Assert.IsTrue(optionSound.IsMute);
            Assert.IsTrue(optionStorage._boolDic[nameof(OptionSound.IsMute)]);
        }
    }
}