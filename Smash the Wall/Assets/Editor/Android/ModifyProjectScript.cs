using System.Linq;
using UnityEditor;
using UnityEditor.Android;
using Unity.Android.Gradle;
using Unity.Android.Gradle.Manifest;
public class ModifyProjectScript1 : AndroidProjectFilesModifier
{
    public override void OnModifyAndroidProjectFiles(AndroidProjectFiles projectFiles)
    {
        var usesPermissionM0 = new UsesPermission();
        projectFiles.UnityLibraryManifest.Manifest.UsesPermissionList.AddElement(usesPermissionM0);
        usesPermissionM0.Attributes.Name.Set("com.google.android.gms.permission.AD_ID");

    }
}
