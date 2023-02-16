using UnityEngine;
using TMPro;
using System;
using System.IO;
using Renci.SshNet;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
public class SFTPScript : MonoBehaviour
{
    [Header("Input Fields")]
    public TMP_InputField serverInput, usernameInput, passwordInput, remoteFilePathInput;
    public TMP_Text passwordHideButton;
    public bool passwordHidden = true;
    [Header("SFTP Configuration")]
    // Replace these with the appropriate values for your setup
    public string username = "user";
    [HideInInspector]
    public string password;
    [Tooltip("path for file you want to upload")]
    public string localFile = "C:\\file.txt";
    public int port = 22;
    public string host = "samba.example.com";
    public string remoteFilePath = "/home/kira/restaurant/";
    

    // Start is called before the first frame update
    void Start()
    {
        localFile = Application.persistentDataPath + localFile;
        //on first run, eye button wont work properly otherwise
        passwordHideButton.SetText("\uF06E");
        passwordInput.contentType = TMP_InputField.ContentType.Password;
        passwordInput.ForceLabelUpdate();

        //update values in input fields to defaults
        serverInput.text = host;
        usernameInput.text = username;
        password = passwordInput.text;
        //passwordInput.text = password;
        remoteFilePathInput.text = remoteFilePath;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void WriteToSFTP()
    {


        Debug.Log("early");


        string remoteFilePathFixed = remoteFilePath + "order" + DateTimeOffset.Now.ToUnixTimeSeconds().ToString() + ".yml";
        using (var client = new SftpClient(host, port, username, password))
        {
            client.Connect();
            if (client.IsConnected)
            {
                
                Debug.Log("I'm connected to the client");

                using (var fileStream = new FileStream(localFile, FileMode.Open))
                {

                    client.BufferSize = 4 * 1024; // bypass Payload error large files
                    Debug.Log(remoteFilePathFixed);
                    client.UploadFile(fileStream, remoteFilePathFixed);
                }
            }
            else
            {
                Debug.Log("I couldn't connect");
            }
        }

    }
    public Order RefreshRemoteOrders()
    {
        
        using (var client = new SftpClient(host, port, username, password))
        {
            Order order = (Order)ScriptableObject.CreateInstance(typeof(Order));

            client.Connect();
            // List all files in the specified directory
            var list = client.ListDirectory(@remoteFilePath);
            foreach (var file in list)
            {
                if (file.FullName != @remoteFilePath + @".." && file.FullName != @remoteFilePath + @".")
                {
                    UnitySystemConsoleRedirector.Redirect();
                    Console.WriteLine(file.FullName);
                    Debug.Log("early");
                    //Application.persistentDataPath + @"\orders\"
                    using (var sftp = new SftpClient(host, port, username, password))
                    {
                        sftp.Connect();
                        if (!Directory.Exists(@"C:\restaurantorders\"))
                        {
                            Directory.CreateDirectory(@"C:\restaurantorders\");
                        }
                        Debug.Log(@"C:\restaurantorders\" + file.FullName.Remove(0, remoteFilePath.Length));
                        using (Stream fileStream = File.Create(@"C:\restaurantorders\" + file.FullName.Remove(0, remoteFilePath.Length)))
                        {
                            sftp.DownloadFile(file.FullName, fileStream);
                            sftp.DeleteFile(file.FullName);
                            fileStream.Close();
                        }
                        string str = File.ReadAllText(@"C:\restaurantorders\" + file.FullName.Remove(0, remoteFilePath.Length));
                        var input = new StringReader(str);

                        var deserializer = new DeserializerBuilder()
                            .WithNamingConvention(CamelCaseNamingConvention.Instance)
                            .Build();
                        //this is fucking stupid.
                        Order testorder = (Order)ScriptableObject.CreateInstance(typeof(Order));
                        testorder = deserializer.Deserialize<Order>(input);
                        order.customer = testorder.customer;
                        order.timedate = testorder.timedate;
                        order.totalcost = testorder.totalcost;
                        order.order = testorder.order;
                    }
                }
                else Debug.Log(file.FullName);
            }
            return order;
        }
    }
    public void WorkerReadFromSFTP(string filepath, Order order)
    {


       

    }
    public void UpdateFilepath()
    {
        host = serverInput.text;
        username = usernameInput.text;
        password = passwordInput.text;
        remoteFilePath = remoteFilePathInput.text;
    }
    public void SettingsShowHidePassword()
    {
        if(passwordHidden)
        {
            //set it to the closed eye icon
            passwordHideButton.SetText("\uF070");
            passwordHidden = false;
            passwordInput.contentType = TMP_InputField.ContentType.Standard;
            passwordInput.ForceLabelUpdate();
        }
        else
        {
            //set it to the open eye icon
            passwordHideButton.SetText("\uF06E");
            passwordHidden = true;
            passwordInput.contentType = TMP_InputField.ContentType.Password;
            passwordInput.ForceLabelUpdate();
        }
    }
}

