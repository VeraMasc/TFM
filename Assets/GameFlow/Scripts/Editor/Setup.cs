using UnityEditor;
using UnityEngine;
using System.IO;

namespace GameFlow {

[InitializeOnLoad]	
class Setup {

static Setup() {
	// start the install automatically once the initial import is done
	AssetDatabase.importPackageCompleted += AutoInstall;
}

static void AutoInstall(string packageName) {
	AssetDatabase.importPackageCompleted -= AutoInstall;
	// cancel install if a different package was imported
	if (!packageName.StartsWith("GameFlow")) {
		return;
	}
	CopyDLLs();
}

// ------------------------------------------------------------------------------------------------

static int Ver(int major, int minor) {
	return major * 100 + minor;
}

static int v2018_4 = Ver(2018, 4);
static int v2019_4 = Ver(2019, 4);
static int v2020_3 = Ver(2020, 3);
static int v2021_1 = Ver(2021, 1);
static int v2021_2 = Ver(2021, 2);

static void CopyDLLs() {
	// get unity version as string and integer
	string[] parts = Application.unityVersion.Split("."[0]);
	string uvs = parts[0] + "." + parts[1];
	int uvi = Ver(int.Parse(parts[0]), int.Parse(parts[1]));
	// only copy the dlls for a supported unity version
	bool supported = uvi == v2018_4 || uvi == v2019_4 || uvi == v2020_3 || uvi == v2021_1;
	bool latest = uvi >= v2021_2;
	if (!supported && !latest) {
		Info($"Supported Unity versions are 2018.4, 2019.4, 2020.3 and 2021.x or newer.\n\n" + 
		  "Please upgrade your Unity and reimport the GameFlow package.\n");
		return;
	}
	// locate the GameFlow/Install/Versions folder
	string path = AssetDatabase.GUIDToAssetPath("a239dc7256cfb467c82cfa28712fc103");
	string cd = Directory.GetCurrentDirectory();
	bool exists = path != "" && Directory.Exists($"{cd}/{path}");
	if (!exists) {
		Info($"Unable to locate the GameFlow versions folder. " + 
		  "Please reimport the GameFlow package.\n");
		return;
	}
	// check source files
	string folder = latest ? "latest" : uvs;
	string src1 = $"{path}/{folder}/GameFlow";
	string src2 = $"{path}/{folder}/GameFlowEditor";
 	if (!File.Exists($"{cd}/{src1}") || !File.Exists($"{cd}/{src2}")) {
		Info($"Unable to locate the GameFlow libraries for Unity {uvs}. " + 
		  "Please reimport the GameFlow package.\n");
		return;
	}
	// locate GameFlow.dll and GameFlowEditor.dll
	string dst1 = AssetDatabase.GUIDToAssetPath("20990ec6b45e544b0b5f28b1e0c1c96b");
	string dst2 = AssetDatabase.GUIDToAssetPath("72b2acbe68e9a4cd4bb8e898cd9a2514");
 	if (dst1 == "" || !File.Exists($"{cd}/{dst1}") || dst2 == "" || !File.Exists($"{cd}/{dst2}")) {
		Info($"Unable to reinstall the GameFlow package. " + 
		  "Please reimport the GameFlow package.\n");
		return;
	}
	// overwrite the dummy .dlls with the specific version files
	AssetDatabase.DisallowAutoRefresh();
	File.Copy(src1, dst1, true);
 	File.Copy(src2, dst2, true);
	AssetDatabase.AllowAutoRefresh();
	AssetDatabase.Refresh();
	Debug.Log("GameFlow succesfully imported. Enjoy!");
}

// ------------------------------------------------------------------------------------------------

static string title = "GameFlow Installer";

static void Info(string message) {
	EditorUtility.DisplayDialog(title, message, "Ok");
}

}

}
