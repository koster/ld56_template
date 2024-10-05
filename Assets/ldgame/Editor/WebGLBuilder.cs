using System.IO;
using UnityEditor;
using UnityEditor.Build.Profile;

public class WebGLBuilder
{
    private const string BuildInfoFilePath = "Assets/BuildTracker/BuildInfo.cs"; // Путь к файлу BuildInfo.cs
    private const string BuildNumberFilePath = "Assets/BuildTracker/BuildNumber.txt"; // Путь к файлу для хранения номера билда

    [MenuItem("Build/Build WebGL")]
    public static void Build()
    {
        UpdateBuildInfo();

        string[] scenes = {  "Assets/ldgame/title_screen.unity", "Assets/ldgame/main.unity", "Assets/ldgame/end_screen.unity" };

        PlayerSettings.defaultWebScreenWidth = 1340;
        PlayerSettings.defaultWebScreenHeight = 710;

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = "webgl_build";
        buildPlayerOptions.target = BuildTarget.WebGL;
        buildPlayerOptions.options = BuildOptions.None;
        
        BuildPipeline.BuildPlayer(scenes, "webgl_build", BuildTarget.WebGL, BuildOptions.None);
    }

    private static void UpdateBuildInfo()
    {
        // Получаем текущий номер билда, если файл существует, или начинаем с 1
        int buildNumber = GetBuildNumber();

        // Увеличиваем номер билда
        buildNumber++;

        // Записываем новый номер билда в файл
        File.WriteAllText(BuildNumberFilePath, buildNumber.ToString());

        // Записываем в BuildInfo метку времени и номер билда
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string buildInfo = $"// This file is auto-generated during build\n" +
                           $"public static class BuildInfo {{\n" +
                           $"    public static readonly string BuildTimestamp = \"{timestamp}\";\n" +
                           $"    public static readonly int BuildNumber = {buildNumber};\n" +
                           $"}}";
        File.WriteAllText(BuildInfoFilePath, buildInfo);
        AssetDatabase.Refresh();
    }

    private static int GetBuildNumber()
    {
        if (File.Exists(BuildNumberFilePath))
        {
            string buildNumberString = File.ReadAllText(BuildNumberFilePath);
            if (int.TryParse(buildNumberString, out int buildNumber))
            {
                return buildNumber;
            }
        }

        return 1; // Если файл не найден, начинаем с 1
    }
}