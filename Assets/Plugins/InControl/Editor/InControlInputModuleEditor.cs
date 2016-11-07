#if UNITY_EDITOR && (UNITY_4_6 || UNITY_4_7 || UNITY_5)
namespace InControl
{
	using UnityEditor;
	using UnityEngine;


	[CustomEditor( typeof( InControlInputModule ) )]
	public class InControlInputModuleEditor : Editor
	{
		SerializedProperty submitButton;
		SerializedProperty cancelButton;
		SerializedProperty analogMoveThreshold;
		SerializedProperty moveRepeatFirstDuration;
		SerializedProperty moveRepeatDelayDuration;
		SerializedProperty allowMobileDevice;
		SerializedProperty allowMouseInput;
		SerializedProperty focusOnMouseHover;


		void OnEnable()
		{
			submitButton = serializedObject.FindProperty( "submitButton" );
			cancelButton = serializedObject.FindProperty( "cancelButton" );
			analogMoveThreshold = serializedObject.FindProperty( "analogMoveThreshold" );
			moveRepeatFirstDuration = serializedObject.FindProperty( "moveRepeatFirstDuration" );
			moveRepeatDelayDuration = serializedObject.FindProperty( "moveRepeatDelayDuration" );
			allowMobileDevice = serializedObject.FindProperty( "allowMobileDevice" );
			allowMouseInput = serializedObject.FindProperty( "allowMouseInput" );
			focusOnMouseHover = serializedObject.FindProperty( "focusOnMouseHover" );
		}


		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			GUILayout.Space( 10.0f );
			EditorGUILayout.LabelField( "Navigation", EditorStyles.boldLabel );

			analogMoveThreshold.floatValue = EditorGUILayout.Slider( "Analog Threshold", analogMoveThreshold.floatValue, 0.1f, 0.9f );
			moveRepeatFirstDuration.floatValue = EditorGUILayout.FloatField( "Delay Until Repeat", moveRepeatFirstDuration.floatValue );
			moveRepeatDelayDuration.floatValue = EditorGUILayout.FloatField( "Repeat Interval", moveRepeatDelayDuration.floatValue );

			GUILayout.Space( 10.0f );
			EditorGUILayout.LabelField( "Interaction", EditorStyles.boldLabel );

			EditorGUILayout.PropertyField( submitButton );
			EditorGUILayout.PropertyField( cancelButton );

			GUILayout.Space( 10.0f );
			EditorGUILayout.LabelField( "Options", EditorStyles.boldLabel );

			allowMobileDevice.boolValue = EditorGUILayout.Toggle( "Allow Mobile Device", allowMobileDevice.boolValue );
			allowMouseInput.boolValue = EditorGUILayout.Toggle( "Allow Mouse Input", allowMouseInput.boolValue );
			focusOnMouseHover.boolValue = EditorGUILayout.Toggle( "Focus Mouse On Hover", focusOnMouseHover.boolValue );

			serializedObject.ApplyModifiedProperties();
		}
	}
}
#endif