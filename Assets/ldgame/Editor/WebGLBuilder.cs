using UnityEditor;

public class WebGLBuilder
{
    public static void Build()
    {
        // 1
        string[] scenes = { "Assets/ldgame/main.unity" }; // Define your scenes here
        BuildPipeline.BuildPlayer(scenes, "path_to_output_directory", BuildTarget.WebGL, BuildOptions.None);
    }
}