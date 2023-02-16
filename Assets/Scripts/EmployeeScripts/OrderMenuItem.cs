using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class OrderMenuItem : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void FetchOrderData(string filepath)
    {
        string ymlContent = File.ReadAllText(filepath);

        var input = new StringReader(ymlContent);
        var yaml = new YamlStream();
        yaml.Load(input);

        var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
        foreach (var entry in mapping.Children)
        {
            var key = ((YamlScalarNode)entry.Key).Value;
            var value = ((YamlScalarNode)entry.Value).Value;
            UnitySystemConsoleRedirector.Redirect();
            System.Console.WriteLine("{0}: {1}", key, value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
