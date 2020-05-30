using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using VentureVeilStructures;

public class ProfilesAPI : MonoBehaviour
{
    public List<Profile> Profiles { get; set; }
    
    public bool LoadProfiles()
    {
        Profiles = new List<Profile>();
        LoadFromXML();
        return true;
    }

    public bool SaveProfile(Profile p)
    {
        LoadProfiles();
        for(int i = 0; i<Profiles.Count;i++)
        {
            if (p.Equals(Profiles[i]))
                return false;
        }

        WriteToXML(p);
        return true;
    }

    public bool DeleteProfile(Profile p)
    {
        string filepath = VVC.userDataDirPath + p.Username;
        Directory.Delete(filepath,true);
        File.Delete(filepath + ".meta");

        DeleteFromXML(p);
        return true;
    }

    private void LoadFromXML()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load("Assets/Data/Profiles.xml");
        XmlNodeList nodes = doc.SelectNodes("Profiles/Profile");

        string user;
        foreach(XmlNode node in nodes)
        {
            user = node.SelectSingleNode("Username").InnerText;
            Profiles.Add(new Profile(user));
        }
    }

    private void WriteToXML(Profile p)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load("Assets/Data/Profiles.xml");

        XmlNode root = doc.SelectSingleNode("Profiles");
        XmlNode newProfile = doc.CreateElement("Profile");
        XmlNode newUsername = doc.CreateElement("Username");
        newUsername.InnerText = p.Username;

        newProfile.AppendChild(newUsername);
        root.AppendChild(newProfile);

        doc.Save("Assets/Data/Profiles.xml");

        //create a doc for the new profile;
        System.IO.Directory.CreateDirectory(VVC.userDataDirPath + p.Username);

    }

    private void DeleteFromXML(Profile p)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load("Assets/Data/Profiles.xml");

        XmlNode node = doc.SelectSingleNode("/Profiles/Profile[Username='" + p.Username + "']");
        node.ParentNode.RemoveChild(node);

        doc.Save("Assets/Data/Profiles.xml");

    }
}
