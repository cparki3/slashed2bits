using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using UnityEditor.iOS.Xcode;
using System.IO;

public class MyBuildPostprocessor {

	internal static void CopyAndReplaceDirectory(string srcPath, string dstPath)
	{
		if (Directory.Exists(dstPath))
			Directory.Delete(dstPath);
		if (File.Exists(dstPath))
			File.Delete(dstPath);

		Directory.CreateDirectory(dstPath);

		foreach (var file in Directory.GetFiles(srcPath))
			File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)));

		foreach (var dir in Directory.GetDirectories(srcPath))
			CopyAndReplaceDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)));
	}

	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget buildTarget, string path) {
	
		#if UNITY_IOS

		Debug.Log("OnPostprocessBuildiOS");
		string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

		PBXProject proj = new PBXProject ();
		proj.ReadFromString (File.ReadAllText (projPath));

		string target = proj.TargetGuidByName ("Unity-iPhone");

		// Set a custom link flag
		proj.AddBuildProperty (target, "OTHER_LDFLAGS", "-all_load");
		proj.AddBuildProperty (target, "OTHER_LDFLAGS", "-ObjC");

		// add frameworks
		proj.AddFrameworkToProject(target, "AdSupport.framework", true);
		proj.AddFrameworkToProject(target, "CoreTelephony.framework", true);
		proj.AddFrameworkToProject(target, "EventKit.framework", true);
		proj.AddFrameworkToProject(target, "EventKitUI.framework", true);
		proj.AddFrameworkToProject(target, "iAd.framework", true);
		proj.AddFrameworkToProject(target, "MessageUI.framework", true);
		proj.AddFrameworkToProject(target, "StoreKit.framework", true);
		proj.AddFrameworkToProject(target, "Security.framework", true);
		proj.AddFrameworkToProject(target, "GameKit.framework", true);

		File.WriteAllText (projPath, proj.WriteToString ());

		#endif
	}
}

