using System.Collections.Generic;
using System.Web;

public class MockHttpSession : HttpSessionStateBase
{
    Dictionary<string, object> sessionDictionary = new Dictionary<string, object>();

    public override object this[string name]
    {
        get { return sessionDictionary.ContainsKey(name) ? sessionDictionary[name] : null; }
        set { sessionDictionary[name] = value; }
    }

    public override void Abandon()
    {
        sessionDictionary.Clear();
    }
}