using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using YamlDotNet.RepresentationModel;
public class Order : ScriptableObject
{
    public string customer { get; set; }
    public string timedate { get; set; }
    public string totalcost { get; set; }
    public string order { get; set; }

    
}
