#if UNITY_EDITOR
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class IconGenerator : MonoBehaviour {
	
    // I should really develop a custom inspector for the next release ;)
    // Also, would you like to have custom overlays for every target?

	[Header("Prefabs")]
    [Tooltip("IconGenerator will generate icons from these prefabs / objects")]
    public Target[] targets;
    [Tooltip("These images will be applied to EVERY icon generated. Higher index = on top")]
    public Sprite[] overlays;

	[Tooltip("Custom folder used to save the icon. LEAVE EMPTY FOR DEFAULT!")]
	public string customFolder = ""; // You need to create folder manually, before running the script!

    // These are only used for generating the icon & debugging
    [Header("Debugging")]
	public Texture rawIcon;
    public Texture2D icon;
    public List<Texture2D> overlayIcons = new List<Texture2D>();

    private Texture2D finalIcon;

    private byte[] overlayBytes;

	void Start ()
	{
        GetOverlayTextures();
		int targetCount = 0;
		string iconName;

		if (targets == null) // Check if targets are specified
		{
            Debug.LogError("You need to specify targets!"); 
			return;
		}

        if (!Directory.Exists(Application.dataPath + "/" + customFolder))
        {
            Debug.LogError("Could not find the directory " + Application.dataPath + "/" + customFolder + ". Please create it first!");
            return;
        }

        foreach (Target target in targets)
		{
            if (target.obj == null)
            {
                // Skip the object.
                continue;
            }

            GameObject targetObj = target.obj;

			rawIcon = AssetPreview.GetAssetPreview (targetObj);
			icon = rawIcon as Texture2D;

            if (overlayIcons.Count != 0)
            {
                if(icon == null)
                {
                    Debug.LogError("There was an error generating image from " + targetObj.name + "! Are you sure this is an 3D object?"); 
                    continue;
                }

                icon = GetFinalTexture(icon, targetCount);
            }
            else
            {
                // Check the icon.
                if (icon == null)
                {
                    Debug.LogError("There was an error generating image from " + targetObj.name + "! Are you sure this is an 3D object?");
                    continue;
                }
            }

            //TextureScale.Point(icon, 512, 512); // Used for rescaling the final icon
            byte[] bytes = icon.EncodeToPNG ();

            if (IsNullOrWhiteSpace(target.name)) // Check if custom name is applied
                iconName = targetObj.name;
            else
                iconName = target.name;


			GameObject.Find("Canvas").GetComponent<IconGeneratorUIExample>().AddImage (icon,iconName); // Used for example, can be removed!

            if (customFolder == "") // Check if custom folder is specified!
			{
				File.WriteAllBytes (Application.dataPath + "/" + iconName + ".png", bytes);
				Debug.Log ("File saved in: " + Application.dataPath + "/" + iconName + ".png");
			}
			else
			{
				File.WriteAllBytes (Application.dataPath + "/" + customFolder + "/" + iconName + ".png", bytes);
				Debug.Log ("File saved in: " + Application.dataPath + "/" + customFolder + "/" + iconName + ".png");
			}

			targetCount++;
		}

        // This will fix the *need to click out of the engine* to see the generated icons bug.
        AssetDatabase.Refresh();
	}

    private void GetOverlayTextures()
    {
        for (int i = 0; i < overlays.Length; i++)
        {
            if (overlays[i] == null)
                continue;

            string overlayPath = AssetDatabase.GetAssetPath(overlays[i]);
            byte[] fileData = File.ReadAllBytes(overlayPath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            if (tex.height != 128)
                TextureScale.Point(tex, 128, 128);


            //Debug.Log("Processing overlay: " + overlayIcons.Count.ToString());
            overlayIcons.Add(tex);

            //Only uncomment these lines, if you know what you are doing. All overlays will be exported to a folder.
			//byte[] bytes = tex.EncodeToPNG();
            //File.WriteAllBytes(Application.dataPath + "/" + "debug" + "/" + overlays[i].name + ".png", bytes);
        }
    }

    private Texture2D GetFinalTexture(Texture2D texture, int id)
    {
        finalIcon = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);

        for (int i = 0; i < overlayIcons.Count; i++)
        {
            if(i == 0)
                CombineTextures(finalIcon, texture, overlayIcons[i]);
            else
                CombineTextures(finalIcon, finalIcon, overlayIcons[i]);
        }

        return finalIcon;
    }

    public void CombineTextures(Texture2D final, Texture2D image, Texture2D overlay)
    {
        var offset = new Vector2(((final.width - overlay.width) / 2), ((final.height - overlay.height) / 2));

        final.SetPixels(image.GetPixels());

        for (int y = 0; y < overlay.height; y++)
        {
            for (int x = 0; x < overlay.width; x++)
            {
                Color PixelColorFore = overlay.GetPixel(x, y) * overlay.GetPixel(x, y).a;
                Color PixelColorBack = final.GetPixel((int)x + (int)offset.x, y + (int)offset.y) * (1 - PixelColorFore.a);
                final.SetPixel((int)x + (int)offset.x, (int)y + (int)offset.y, PixelColorBack + PixelColorFore);
            }
        }

        final.Apply();
    }

	// This is the same as doing string.IsNullOrWhiteSpace in the .NET 4.x runtime.
	// By doing it as a separate custom function we can also support people who are using the old .NET 3.5 runtime
    public bool IsNullOrWhiteSpace(string value)
    {
        if (value != null)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                {
                    return false;
                }
            }
        }
        return true;
    }
}

[System.Serializable]
public class Target
{
    [Header("If the name value is empty, prefab name will be used as the filename!")]
    public string name;
    public GameObject obj;
}
#endif