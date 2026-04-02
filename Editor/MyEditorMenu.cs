public static class MyEditorMenu
{
	[Menu("Editor", "battlerite_copy_sucky_version/My Menu Option")]
	public static void OpenMyMenu()
	{
		EditorUtility.DisplayDialog("It worked!", "This is being called from your library's editor code!");
	}
}
