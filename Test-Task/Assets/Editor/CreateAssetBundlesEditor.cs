using UnityEditor;

public class CreateAssetBundlesEditor 
{
    [MenuItem("Tools/Create Asset Bundles")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles(
            "Assets/AssetBundles", 
            BuildAssetBundleOptions.ChunkBasedCompression, 
            BuildTarget.StandaloneWindows64);
    }
}
