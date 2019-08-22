using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core
{
    public interface ISumerator
    {
        string GetName();

    }

    public class Sumerating : ISumerator
    {
        private List<KeyValuePair<string, string>> _names = new List<KeyValuePair<string, string>>();

        public Sumerating(string property, string name)
        {
            _names.Add(new KeyValuePair<string, string>(property, name));
        }

        public string GetName()
        {
            return _names[0].Value;
        }
        public void Merge()
        {

        }

    }
    public class TestClass
    {

        public string DoStuff<T>(T thing) where T : ISumerator
        {
            return thing.GetName();
        }

    }
}
