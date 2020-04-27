using Mirror.Logging;
using UnityEditor;

namespace Mirror.EditorScripts.Logging
{
    [CustomEditor(typeof(LogSettingsConfig))]
    public class LogSettingsConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();


            LogSettingsConfig target = this.target as LogSettingsConfig;

            if (target.settings == null)
            {
                LogSettings newSettings = LogLevelsGUI.DrawCreateNewButton();
                if (newSettings != null)
                {
                    SerializedProperty settingsProp = serializedObject.FindProperty("settings");
                    settingsProp.objectReferenceValue = newSettings;
                    serializedObject.ApplyModifiedProperties();
                }
            }
            else
            {
                LogLevelsGUI.DrawLogFactoryDictionary(target.settings);
            }
        }
    }
}
